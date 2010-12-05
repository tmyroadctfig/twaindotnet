using System;
using System.Collections.Generic;
using TwainDotNet.TwainNative;
using System.Runtime.InteropServices;
using TwainDotNet.Win32;
using System.Reflection;
using System.Drawing;
using log4net;

namespace TwainDotNet
{
    public class DataSourceManager : IDisposable
    {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(DataSourceManager));

        IWindowsMessageHook _messageHook;
        Event _eventMessage;

        public Identity ApplicationId { get; private set; }
        public DataSource DataSource { get; private set; }

        public DataSourceManager(Identity applicationId, IWindowsMessageHook messageHook)
        {
            // Make a copy of the identity in case it gets modified
            ApplicationId = applicationId.Clone();

            ScanningComplete += delegate { };
            TransferImage += delegate { };

            _messageHook = messageHook;
            _messageHook.FilterMessageCallback = FilterMessage;
            IntPtr windowHandle = _messageHook.WindowHandle;

            _eventMessage.EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WindowsMessage)));

            // Initialise the data source manager
            TwainResult result = Twain32Native.DsmParent(
                ApplicationId,
                IntPtr.Zero,
                DataGroup.Control,
                DataArgumentType.Parent,
                Message.OpenDSM,
                ref windowHandle);

            if (result == TwainResult.Success)
            {
                //according to the 2.0 spec (2-10) if (applicationId.SupportedGroups
                // | DataGroup.Dsm2) > 0 then we should call DM_Entry(id, 0, DG_Control, DAT_Entrypoint, MSG_Get, wh)
                //right here
                DataSource = DataSource.GetDefault(ApplicationId, _messageHook);
            }
            else
            {
                throw new TwainException("Error initialising DSM: " + result, result);
            }
        }

        ~DataSourceManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Notification that the scanning has completed.
        /// </summary>
        public event EventHandler<ScanningCompleteEventArgs> ScanningComplete;

        public event EventHandler<TransferImageEventArgs> TransferImage;

        public IWindowsMessageHook MessageHook { get { return _messageHook; } }

        public void StartScan(ScanSettings settings)
        {
            bool scanning = false;

            try
            {
                _messageHook.UseFilter = true;
                scanning = DataSource.Open(settings);
            }
            catch (TwainException e)
            {
                DataSource.Close();
                EndingScan();
                throw e;
            }
            finally
            {
                // Remove the message hook if scan setup failed
                if (!scanning)
                {
                    EndingScan();
                }
            }
        }

        protected IntPtr FilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (DataSource.SourceId.Id == 0)
            {
                handled = false;
                return IntPtr.Zero;
            }

            int pos = User32Native.GetMessagePos();

            WindowsMessage message = new WindowsMessage();
            message.hwnd = hwnd;
            message.message = msg;
            message.wParam = wParam;
            message.lParam = lParam;
            message.time = User32Native.GetMessageTime();
            message.x = (short)pos;
            message.y = (short)(pos >> 16);

            Marshal.StructureToPtr(message, _eventMessage.EventPtr, false);
            _eventMessage.Message = 0;

            TwainResult result = Twain32Native.DsEvent(
                ApplicationId,
                DataSource.SourceId,
                DataGroup.Control,
                DataArgumentType.Event,
                Message.ProcessEvent,
                ref _eventMessage);

            if (result == TwainResult.NotDSEvent)
            {
                handled = false;
                return IntPtr.Zero;
            }

            switch (_eventMessage.Message)
            {
                case Message.XFerReady:
                    Exception exception = null;
                    try
                    {
                        TransferPictures();
                    }
                    catch (Exception e)
                    {
                        exception = e;
                    }
                    finally
                    {
                        EndingScan();
                        DataSource.Close();
                    }
                    ScanningComplete(this, new ScanningCompleteEventArgs(exception));
                    break;

                case Message.CloseDS:
                    EndingScan();
                    DataSource.Close();
                    break;

                case Message.CloseDSOK:
                    EndingScan();
                    DataSource.Close();
                    break;

                case Message.DeviceEvent:
                    break;
            }

            handled = true;
            return IntPtr.Zero;
        }

        protected void TransferPictures()
        {
            if (DataSource.SourceId.Id == 0)
            {
                return;
            }

            PendingXfers pendingTransfer = new PendingXfers();
            TwainResult result;
            try
            {
                do
                {
                    pendingTransfer.Count = 0;
                    IntPtr hbitmap = IntPtr.Zero;

                    // Get the image info
                    ImageInfo imageInfo = new ImageInfo();
                    result = Twain32Native.DsImageInfo(
                        ApplicationId,
                        DataSource.SourceId,
                        DataGroup.Image,
                        DataArgumentType.ImageInfo,
                        Message.Get,
                        imageInfo);

                    if (result != TwainResult.Success)
                    {
                        DataSource.Close();
                        break;
                    }

                    // Transfer the image from the device
                    result = Twain32Native.DsImageTransfer(
                        ApplicationId,
                        DataSource.SourceId,
                        DataGroup.Image,
                        DataArgumentType.ImageNativeXfer,
                        Message.Get,
                        ref hbitmap);

                    if (result != TwainResult.XferDone)
                    {
                        DataSource.Close();
                        break;
                    }

                    // End pending transfers
                    result = Twain32Native.DsPendingTransfer(
                        ApplicationId,
                        DataSource.SourceId,
                        DataGroup.Control,
                        DataArgumentType.PendingXfers,
                        Message.EndXfer,
                        pendingTransfer);

                    if (result != TwainResult.Success)
                    {
                        DataSource.Close();
                        break;
                    }

                    if (hbitmap == IntPtr.Zero)
                    {
                        log.Warn("Transfer complete but bitmap pointer is still null.");
                    }
                    else
                    {
                        using (var renderer = new BitmapRenderer(hbitmap))
                        {
                            TransferImageEventArgs args = new TransferImageEventArgs(renderer.RenderToBitmap(), pendingTransfer.Count != 0);
                            TransferImage(this, args);
                            if (!args.ContinueScanning)
                                break;
                        }
                    }
                }
                while (pendingTransfer.Count != 0);
            }
            finally
            {
                // Reset any pending transfers
                result = Twain32Native.DsPendingTransfer(
                    ApplicationId,
                    DataSource.SourceId,
                    DataGroup.Control,
                    DataArgumentType.PendingXfers,
                    Message.Reset,
                    pendingTransfer);
            }
        }

        protected void EndingScan()
        {
            _messageHook.UseFilter = false;
        }

        public void SelectSource()
        {
            DataSource.Dispose();
            DataSource = DataSource.UserSelected(ApplicationId, _messageHook);
        }

        public void SelectSource(DataSource dataSource)
        {
            DataSource.Dispose();
            DataSource = dataSource;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Marshal.FreeHGlobal(_eventMessage.EventPtr);

            if (disposing)
            {
                DataSource.Dispose();

                IntPtr windowHandle = _messageHook.WindowHandle;

                if (ApplicationId.Id != 0)
                {
                    // Close down the data source manager
                    Twain32Native.DsmParent(
                        ApplicationId,
                        IntPtr.Zero,
                        DataGroup.Control,
                        DataArgumentType.Parent,
                        Message.CloseDSM,
                        ref windowHandle);
                }

                ApplicationId.Id = 0;
            }
        }

        public static ConditionCode GetConditionCode(Identity applicationId, Identity sourceId)
        {
            Status status = new Status();

            Twain32Native.DsmStatus(
                applicationId,
                sourceId,
                DataGroup.Control,
                DataArgumentType.Status,
                Message.Get,
                status);

            return status.ConditionCode;
        }

        public static readonly Identity DefaultApplicationId = new Identity()
        {
            Id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0),
            Version = new TwainVersion()
            {
                MajorNum = 1,
                MinorNum = 1,
                Language = Language.USA,
                Country = Country.USA,
                Info = Assembly.GetExecutingAssembly().FullName
            },
            ProtocolMajor = TwainConstants.ProtocolMajor,
            ProtocolMinor = TwainConstants.ProtocolMinor,
            SupportedGroups = (int)(DataGroup.Image | DataGroup.Control),
            Manufacturer = "TwainDotNet",
            ProductFamily = "TwainDotNet",
            ProductName = "TwainDotNet",
        };
    }
}

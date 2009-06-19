using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using TwainDotNet.TwainNative;
using TwainDotNet.Win32;
using System.Drawing;

namespace TwainDotNet
{
    public class Twain : System.Windows.Forms.IMessageFilter
    {
        private IntPtr _windowHandle;
        private Identity _applicationId;
        private Identity _defaultSourceId;
        private Event _eventMessage;
        private bool _messageFilterActive;

        public Twain(IntPtr windowHandle)
        {
            _applicationId = new Identity();
            _applicationId.Id = IntPtr.Zero;
            _applicationId.Version.MajorNum = 1;
            _applicationId.Version.MinorNum = 1;
            _applicationId.Version.Language = Language.USA;
            _applicationId.Version.Country = Country.USA;
            _applicationId.Version.Info = Assembly.GetExecutingAssembly().FullName;
            _applicationId.ProtocolMajor = TwainConstants.ProtocolMajor;
            _applicationId.ProtocolMinor = TwainConstants.ProtocolMinor;
            _applicationId.SupportedGroups = (int)(DataGroup.Image | DataGroup.Control);
            _applicationId.Manufacturer = "TwainDotNet";
            _applicationId.ProductFamily = "TwainDotNet";
            _applicationId.ProductName = "TwainDotNet";

            _defaultSourceId = new Identity();
            _defaultSourceId.Id = IntPtr.Zero;

            _eventMessage.EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WindowsMessage)));

            Initialise(windowHandle);

            Bitmaps = new List<Bitmap>();
            ScanningComplete += delegate {};
        }

        ~Twain()
        {
            Marshal.FreeHGlobal(_eventMessage.EventPtr);
        }

        private void Initialise(IntPtr windowHandle)
        {
            Close();

            // Initialise the data source manager
            TwainResult result = Twain32Native.DsmParent(
                _applicationId,
                IntPtr.Zero,
                DataGroup.Control,
                DataArgumentType.Parent,
                Message.OpenDSM,
                ref windowHandle);

            if (result == TwainResult.Success)
            {
                // Attempt to get information about the system default source
                result = Twain32Native.DsmIdentity(
                    _applicationId,
                    IntPtr.Zero,
                    DataGroup.Control,
                    DataArgumentType.Identity,
                    Message.GetDefault,
                    _defaultSourceId);

                if (result == TwainResult.Success)
                {
                    _windowHandle = windowHandle;
                }
                else
                {
                    // Failed to get default source information, close the data source manager
                    Close();
                    throw new TwainException("Error getting information about the default source: " + result);
                }
            }
            else
            {
                throw new TwainException("Error initialising DSM: " + result);
            }
        }

        /// <summary>
        /// Closes the data source manager.
        /// </summary>
        protected void Close()
        {
            CloseSource();

            if (_applicationId.Id != IntPtr.Zero)
            {
                // Close down the data source manager
                Twain32Native.DsmParent(
                    _applicationId,
                    IntPtr.Zero,
                    DataGroup.Control,
                    DataArgumentType.Parent,
                    Message.CloseDSM,
                    ref _windowHandle);
            }

            _applicationId.Id = IntPtr.Zero;
        }

        /// <summary>
        /// Closes the default data source.
        /// </summary>
        protected void CloseSource()
        {
            if (_defaultSourceId.Id != IntPtr.Zero)
            {
                UserInterface userInterface = new UserInterface();

                TwainResult result = Twain32Native.DsUserInterface(
                    _applicationId, 
                    _defaultSourceId, 
                    DataGroup.Control,
                    DataArgumentType.UserInterface, 
                    Message.DisableDS, 
                    userInterface);

                result = Twain32Native.DsmIdentity(
                    _applicationId, 
                    IntPtr.Zero,
                    DataGroup.Control,
                    DataArgumentType.Identity, 
                    Message.CloseDS,
                    _defaultSourceId);
            }
        }

        protected void EndingScan()
        {
            if (_messageFilterActive)
            {
                System.Windows.Forms.Application.RemoveMessageFilter(this);
                _messageFilterActive = false;
            }
        }

        protected IList<IntPtr> TransferPictures()
        {
            if (_defaultSourceId.Id == IntPtr.Zero)
            {
                return null;
            }

            List<IntPtr> picturePointers = new List<IntPtr>();            
            PendingXfers pendingTransfer = new PendingXfers();
            TwainResult result;

            do
            {
                pendingTransfer.Count = 0;
                IntPtr hbitmap = IntPtr.Zero;
                ImageInfo imageInfo = new ImageInfo();

                // Get the image info
                result = Twain32Native.DsImageInfo(
                    _applicationId,
                    _defaultSourceId, 
                    DataGroup.Image,
                    DataArgumentType.ImageInfo, 
                    Message.Get,
                    imageInfo);

                if (result != TwainResult.Success)
                {
                    CloseSource();
                    break;
                }

                // Transfer the image from the device
                result = Twain32Native.DsImageTransfer(
                    _applicationId, 
                    _defaultSourceId, 
                    DataGroup.Image, 
                    DataArgumentType.ImageNativeXfer, 
                    Message.Get,
                    ref hbitmap);

                if (result != TwainResult.XferDone)
                {
                    CloseSource();
                    break;
                }

                // End pending transfers
                result = Twain32Native.DsPendingTransfer(
                    _applicationId, 
                    _defaultSourceId,
                    DataGroup.Control,
                    DataArgumentType.PendingXfers, 
                    Message.EndXfer,
                    pendingTransfer);

                if (result != TwainResult.Success)
                {
                    CloseSource();
                    break;
                }

                picturePointers.Add(hbitmap);
            }
            while (pendingTransfer.Count != 0);

            // Reset any pending transfers
            result = Twain32Native.DsPendingTransfer(
                _applicationId, 
                _defaultSourceId, 
                DataGroup.Control, 
                DataArgumentType.PendingXfers, 
                Message.Reset, 
                pendingTransfer);

            return picturePointers;
        }

        protected void Acquire(ScanSettings settings)
        {
            CloseSource();

            if (_applicationId.Id == IntPtr.Zero)
            {
                Initialise(_windowHandle);

                if (_applicationId.Id == IntPtr.Zero)
                {
                    return;
                }
            }

            TwainResult result = Twain32Native.DsmIdentity(
                _applicationId, 
                IntPtr.Zero,
                DataGroup.Control, 
                DataArgumentType.Identity, 
                Message.OpenDS,
                _defaultSourceId);

            if (result != TwainResult.Success)
            {
                return;
            }

            UserInterface ui = new UserInterface();
            ui.ShowUI = (short)(settings.ShowTwainUI ? 1 : 0);
            ui.ModalUI = 1;
            ui.ParentHand = _windowHandle;
            result = Twain32Native.DsUserInterface(
                _applicationId,
                _defaultSourceId,
                DataGroup.Control,
                DataArgumentType.UserInterface,
                Message.EnableDS,
                ui);

            if (result != TwainResult.Success)
            {
                CloseSource();
                return;
            }

            if (!settings.ShowTwainUI)
            {
                Capability.SetCapability(Capabilities.XferCount, settings.TransferCount, _applicationId, _defaultSourceId);

                if (settings.UseDocumentFeeder)
                {
                    // Enable the document feeder
                    Capability.SetCapability(Capabilities.FeederEnabled, true, _applicationId, _defaultSourceId);

                    if (!Capability.GetBoolCapability(Capabilities.FeederLoaded, _applicationId, _defaultSourceId))
                    {
                        throw new FeederEmptyException();
                    }

                    Capability.SetCapability(Capabilities.FeedPage, true, _applicationId, _defaultSourceId);
                    Capability.SetCapability(Capabilities.AutoFeed, true, _applicationId, _defaultSourceId);
                }
            }
        }

        /// <summary>
        /// Notification that the scanning has completed.
        /// </summary>
        public event EventHandler ScanningComplete;

        /// <summary>
        /// The scanned in bitmaps.
        /// </summary>
        public IList<Bitmap> Bitmaps { get; private set; }

        /// <summary>
        /// Starts scanning.
        /// </summary>
        public void StartScanning(ScanSettings settings)
        {
            if (!_messageFilterActive)
            {
                _messageFilterActive = true;
                System.Windows.Forms.Application.AddMessageFilter(this);
            }

            try
            {
                Acquire(settings);
            }
            catch (TwainException e)
            {
                CloseSource();
                EndingScan();
                throw e;
            }
        }

        public void Select()
        {
            CloseSource();
            if (_applicationId.Id == IntPtr.Zero)
            {
                Initialise(_windowHandle);

                if (_applicationId.Id == IntPtr.Zero)
                {
                    return;
                }
            }

            Twain32Native.DsmIdentity(
                _applicationId,
                IntPtr.Zero,
                DataGroup.Control,
                DataArgumentType.Identity,
                Message.UserSelect,
                _defaultSourceId);
        }

        #region System.Windows.Forms.IMessageFilter

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (_defaultSourceId.Id == IntPtr.Zero)
            {
                return false;
            }

            int pos = User32Native.GetMessagePos();

            WindowsMessage message = new WindowsMessage();
            message.hwnd = m.HWnd;
            message.message = m.Msg;
            message.wParam = m.WParam;
            message.lParam = m.LParam;
            message.time = User32Native.GetMessageTime();
            message.x = (short)pos;
            message.y = (short)(pos >> 16);

            Marshal.StructureToPtr(message, _eventMessage.EventPtr, false);
            _eventMessage.Message = 0;

            TwainResult result = Twain32Native.DsEvent(
                _applicationId, 
                _defaultSourceId, 
                DataGroup.Control,
                DataArgumentType.Event,
                Message.ProcessEvent,
                ref _eventMessage);

            if (result == TwainResult.NotDSEvent)
            {
                return false;
            }

            switch (_eventMessage.Message)
            {
                case Message.XFerReady:
                    IList<IntPtr> imagePointers = TransferPictures();
                    
                    foreach (IntPtr image in imagePointers)
                    {
                        Bitmaps.Add(new BitmapRenderer(image).RenderToBitmap());
                    }

                    EndingScan();
                    CloseSource();

                    ScanningComplete(this, EventArgs.Empty);
                    break;

                case Message.CloseDS:
                    EndingScan();
                    CloseSource();
                    break;

                case Message.CloseDSOK:
                    EndingScan();
                    CloseSource();
                    break;

                case Message.DeviceEvent:
                    break;
            }

            return true;
        }

        #endregion
    }
}

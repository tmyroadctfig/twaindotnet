using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    public class DataSource : IDisposable
    {
        Identity _applicationId;
        IWindowsMessageHook _messageHook;

        public DataSource(Identity applicationId, Identity sourceId, IWindowsMessageHook messageHook)
        {
            _applicationId = applicationId;
            SourceId = sourceId;
            _messageHook = messageHook;
        }

        ~DataSource()
        {
            Dispose(false);
        }

        public Identity SourceId { get; private set; }

        public void Open(ScanSettings settings)
        {
            OpenSource();

            if (!settings.ShowTwainUI)
            {
                Capability.SetCapability(Capabilities.XferCount, settings.TransferCount, _applicationId, SourceId);

                if (settings.UseDocumentFeeder)
                {
                    // Enable the document feeder
                    Capability.SetCapability(Capabilities.FeederEnabled, true, _applicationId, SourceId);

                    if (!Capability.GetBoolCapability(Capabilities.FeederLoaded, _applicationId, SourceId))
                    {
                        throw new FeederEmptyException();
                    }

                    Capability.SetCapability(Capabilities.FeedPage, true, _applicationId, SourceId);
                    Capability.SetCapability(Capabilities.AutoFeed, true, _applicationId, SourceId);
                }

                if (settings.Resolution != null)
                {
                    Capability.SetCapability(Capabilities.IPixelType, (short)settings.Resolution.GetPixelType(), _applicationId, SourceId);

                    // TODO: Also set this for colour scanning
                    if (settings.Resolution.ColourSetting != ColourSetting.Colour)
                    {
                        Capability.SetCapability(Capabilities.BitDepth, settings.Resolution.GetBitDepth(), _applicationId, SourceId);
                    }

                    if (settings.Resolution.Dpi.HasValue)
                    {
                        int dpi = settings.Resolution.Dpi.Value;
                        Capability.SetCapability(Capabilities.XResolution, dpi, TwainType.Fix32, _applicationId, SourceId);
                        Capability.SetCapability(Capabilities.YResolution, dpi, TwainType.Fix32, _applicationId, SourceId);
                    }
                }
            }

            Enable(settings);
        }

        public void OpenSource()
        {
            var result = Twain32Native.DsmIdentity(
                   _applicationId,
                   IntPtr.Zero,
                   DataGroup.Control,
                   DataArgumentType.Identity,
                   Message.OpenDS,
                   SourceId);

            if (result != TwainResult.Success)
            {
                throw new TwainException("Error opening data source", result);
            }
        }

        public void Enable(ScanSettings settings)
        {
            UserInterface ui = new UserInterface();
            ui.ShowUI = (short)(settings.ShowTwainUI ? 1 : 0);
            ui.ModalUI = 1;
            ui.ParentHand = _messageHook.WindowHandle;

            var result = Twain32Native.DsUserInterface(
                _applicationId,
                SourceId,
                DataGroup.Control,
                DataArgumentType.UserInterface,
                Message.EnableDS,
                ui);

            if (result != TwainResult.Success)
            {
                Dispose();
                return;
            }
        }

        public static DataSource GetDefault(Identity applicationId, IWindowsMessageHook messageHook)
        {
            var defaultSourceId = new Identity();
            defaultSourceId.Id = IntPtr.Zero;

            // Attempt to get information about the system default source
            var result = Twain32Native.DsmIdentity(
                applicationId,
                IntPtr.Zero,
                DataGroup.Control,
                DataArgumentType.Identity,
                Message.GetDefault,
                defaultSourceId);

            if (result != TwainResult.Success)
            {
                throw new TwainException("Error getting information about the default source: " + result, result);
            }

            return new DataSource(applicationId, defaultSourceId, messageHook);
        }

        public static DataSource UserSelected(Identity applicationId, IWindowsMessageHook messageHook)
        {
            var defaultSourceId = new Identity();
            defaultSourceId.Id = IntPtr.Zero;

            Twain32Native.DsmIdentity(
                applicationId,
                IntPtr.Zero,
                DataGroup.Control,
                DataArgumentType.Identity,
                Message.UserSelect,
                defaultSourceId);

            return new DataSource(applicationId, defaultSourceId, messageHook);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        public void Close()
        {
            if (SourceId.Id != IntPtr.Zero)
            {
                UserInterface userInterface = new UserInterface();

                TwainResult result = Twain32Native.DsUserInterface(
                    _applicationId,
                    SourceId,
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
                    SourceId);
            }
        }
    }
}

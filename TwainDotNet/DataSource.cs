using System;
using System.Collections.Generic;
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
            SourceId = sourceId.Clone();
            _messageHook = messageHook;
        }

        ~DataSource()
        {
            Dispose(false);
        }

        public Identity SourceId { get; private set; }

        public void NegotiateTransferCount(ScanSettings scanSettings)
        {
            scanSettings.TransferCount = Capability.SetCapability(
                Capabilities.XferCount,
                scanSettings.TransferCount,
                _applicationId,
                SourceId);
        }

        public void NegotiateFeeder(ScanSettings scanSettings)
        {
            if (scanSettings.UseDocumentFeeder)
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
        }

        public PixelType GetPixelType(ScanSettings scanSettings)
        {
            switch (scanSettings.Resolution.ColourSetting)
            {
                case ColourSetting.BlackAndWhite:
                    return PixelType.BlackAndWhite;

                case ColourSetting.GreyScale:
                    return PixelType.Grey;

                case ColourSetting.Colour:
                    return PixelType.Rgb;
            }

            throw new NotImplementedException();
        }

        public short GetBitDepth(ScanSettings scanSettings)
        {
            switch (scanSettings.Resolution.ColourSetting)
            {
                case ColourSetting.BlackAndWhite:
                    return 1;

                case ColourSetting.GreyScale:
                    return 8;

                case ColourSetting.Colour:
                    return 16;
            }

            throw new NotImplementedException();
        }

        public bool PaperDetectable {
            get {
                return Capability.GetBoolCapability(Capabilities.FeederLoaded, _applicationId, SourceId);
            }
        }

        public void NegotiateColour(ScanSettings scanSettings)
        {
            Capability.SetCapability(Capabilities.IPixelType, (short)GetPixelType(scanSettings), _applicationId, SourceId);

            // TODO: Also set this for colour scanning
            if (scanSettings.Resolution.ColourSetting != ColourSetting.Colour)
            {
                Capability.SetCapability(Capabilities.BitDepth, GetBitDepth(scanSettings), _applicationId, SourceId);
            }
        }

        public void NegotiateResolution(ScanSettings scanSettings)
        {
            if (scanSettings.Resolution.Dpi.HasValue)
            {
                int dpi = scanSettings.Resolution.Dpi.Value;
                Capability.SetBasicCapability(Capabilities.XResolution, dpi, TwainType.Fix32, _applicationId, SourceId);
                Capability.SetBasicCapability(Capabilities.YResolution, dpi, TwainType.Fix32, _applicationId, SourceId);
            }
        }

        public void Open(ScanSettings settings)
        {
            OpenSource();

            if (!settings.ShowTwainUI)
            {
                NegotiateTransferCount(settings);
                NegotiateFeeder(settings);                

                if (settings.Resolution != null)
                {
                    NegotiateColour(settings);
                    NegotiateResolution(settings);
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

        public static List<DataSource> GetAllSources(Identity appID, IWindowsMessageHook winHook)
        {
            var s = new List<DataSource>();
            Identity id = new Identity();
            var res = Twain32Native.DsmIdentity(appID, IntPtr.Zero, DataGroup.Control, DataArgumentType.Identity, Message.GetFirst, id);

            if (res == TwainResult.EndOfList)
                return s;
            else if (res != TwainResult.Success) {
                throw new TwainException("MSG_GETFIRST call failed.", res);
            }
            else
                s.Add(new DataSource(appID, id, winHook));

            do
            {
                res = Twain32Native.DsmIdentity(appID, IntPtr.Zero, DataGroup.Control, DataArgumentType.Identity, Message.GetNext, id);
                if (res == TwainResult.EndOfList) break;
                if (res != TwainResult.Success)
                    throw new TwainException("MSG_GETNEXT call failed.", res);
                s.Add(new DataSource(appID, id, winHook));
            } while (true);

            return s;
        }

        public static DataSource GetSource(string sourceProductName, Identity appID, IWindowsMessageHook winHook)
        {
            //a little slower than it could be, if enumerating unnecessary sources is slow. But less code duplication.
            foreach (var source in GetAllSources(appID, winHook))
            {
                if (sourceProductName == source.SourceId.ProductName)
                    return source;
            }
            return null;
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

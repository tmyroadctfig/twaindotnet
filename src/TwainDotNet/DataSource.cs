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
            try
            {
                scanSettings.TransferCount = Capability.SetCapability(
                        Capabilities.XferCount,
                        scanSettings.TransferCount,
                        _applicationId,
                        SourceId);
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }
        }

        public void NegotiateFeeder(ScanSettings scanSettings)
        {

            try
            {
                if (scanSettings.UseDocumentFeeder.HasValue)
                {
                    Capability.SetCapability(Capabilities.FeederEnabled, scanSettings.UseDocumentFeeder.Value, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }

            try
            {
                if (scanSettings.UseAutoFeeder.HasValue)
                {
                    Capability.SetCapability(Capabilities.AutoFeed, scanSettings.UseAutoFeeder == true && scanSettings.UseDocumentFeeder == true, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }

            try
            {
                if (scanSettings.UseAutoScanCache.HasValue)
                {
                    Capability.SetCapability(Capabilities.AutoScan, scanSettings.UseAutoScanCache.Value, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
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

        public bool PaperDetectable
        {
            get
            {
                try
                {
                    return Capability.GetBoolCapability(Capabilities.FeederLoaded, _applicationId, SourceId);
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SupportsDuplex
        {
            get
            {
                try
                {
                    var cap = new Capability(Capabilities.Duplex, TwainType.Int16, _applicationId, SourceId);
                    return ((Duplex)cap.GetBasicValue().Int16Value) != Duplex.None;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool CheckDeviceOnline()
        {
            try
            {
                var sourceVersion = new Version(SourceId.ProtocolMajor, SourceId.ProtocolMinor);
                var minVer = ProtocolVersions.GetMinimumVersion(Capabilities.DeviceOnline);
                if (sourceVersion >= minVer)
                {
                    //var cap = new Capability(Capabilities.DeviceOnline, TwainType.Bool, _applicationId, SourceId);
                    var cap = Capability.GetBoolCapability(Capabilities.DeviceOnline, _applicationId, SourceId);
                    if (!cap)
                        throw new TwainException($"{SourceId.ProductName} is offline.");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void NegotiateColour(ScanSettings scanSettings)
        {
            try
            {
                Capability.SetBasicCapability(Capabilities.IPixelType, (ushort)GetPixelType(scanSettings), TwainType.UInt16, _applicationId, SourceId);
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }

            // TODO: Also set this for colour scanning
            try
            {
                if (scanSettings.Resolution.ColourSetting != ColourSetting.Colour)
                {
                    Capability.SetCapability(Capabilities.BitDepth, GetBitDepth(scanSettings), _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }

        }

        public void NegotiateResolution(ScanSettings scanSettings)
        {
            try
            {
                if (scanSettings.Resolution.Dpi.HasValue)
                {
                    int dpi = scanSettings.Resolution.Dpi.Value;
                    Capability.SetBasicCapability(Capabilities.XResolution, dpi, TwainType.Fix32, _applicationId, SourceId);
                    Capability.SetBasicCapability(Capabilities.YResolution, dpi, TwainType.Fix32, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }
        }

        public void NegotiateDuplex(ScanSettings scanSettings)
        {
            try
            {
                if (scanSettings.UseDuplex.HasValue && SupportsDuplex)
                {
                    Capability.SetCapability(Capabilities.DuplexEnabled, scanSettings.UseDuplex.Value, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }
        }

        public void NegotiateOrientation(ScanSettings scanSettings)
        {
            // Set orientation (default is portrait)
            try
            {
                var cap = new Capability(Capabilities.Orientation, TwainType.Int16, _applicationId, SourceId);
                if ((Orientation)cap.GetBasicValue().Int16Value != Orientation.Default)
                {
                    Capability.SetBasicCapability(Capabilities.Orientation, (ushort)scanSettings.Page.Orientation, TwainType.UInt16, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }
        }

        /// <summary>
        /// Negotiates the size of the page.
        /// </summary>
        /// <param name="scanSettings">The scan settings.</param>
        public void NegotiatePageSize(ScanSettings scanSettings)
        {
            try
            {
                var cap = new Capability(Capabilities.Supportedsizes, TwainType.Int16, _applicationId, SourceId);
                if ((PageType)cap.GetBasicValue().Int16Value != PageType.UsLetter)
                {
                    Capability.SetBasicCapability(Capabilities.Supportedsizes, (ushort)scanSettings.Page.Size, TwainType.UInt16, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }
        }

        /// <summary>
        /// Negotiates the automatic rotation capability.
        /// </summary>
        /// <param name="scanSettings">The scan settings.</param>
        public void NegotiateAutomaticRotate(ScanSettings scanSettings)
        {
            try
            {
                if (scanSettings.Rotation.AutomaticRotate)
                {
                    Capability.SetCapability(Capabilities.Automaticrotate, true, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }
        }

        /// <summary>
        /// Negotiates the automatic border detection capability.
        /// </summary>
        /// <param name="scanSettings">The scan settings.</param>
        public void NegotiateAutomaticBorderDetection(ScanSettings scanSettings)
        {
            try
            {
                if (scanSettings.Rotation.AutomaticBorderDetection)
                {
                    Capability.SetCapability(Capabilities.Automaticborderdetection, true, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }
        }

        /// <summary>
        /// Negotiates the indicator.
        /// </summary>
        /// <param name="scanSettings">The scan settings.</param>
        public void NegotiateProgressIndicator(ScanSettings scanSettings)
        {
            try
            {
                if (scanSettings.ShowProgressIndicatorUI.HasValue)
                {
                    Capability.SetCapability(Capabilities.Indicators, scanSettings.ShowProgressIndicatorUI.Value, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }
        }

        public bool Open(ScanSettings settings)
        {
            OpenSource();
            CheckDeviceOnline();

            if (settings.AbortWhenNoPaperDetectable && !PaperDetectable)
                throw new FeederEmptyException();

            // Set whether or not to show progress window
            NegotiateProgressIndicator(settings);
            NegotiateTransferCount(settings);
            NegotiateFeeder(settings);
            NegotiateDuplex(settings);

            if (settings.UseDocumentFeeder == true &&
                settings.Page != null)
            {
                NegotiatePageSize(settings);
                NegotiateOrientation(settings);
            }

            if (settings.Area != null)
            {
                NegotiateArea(settings);
            }

            if (settings.Resolution != null)
            {
                NegotiateColour(settings);
                NegotiateResolution(settings);
            }

            // Configure automatic rotation and image border detection
            if (settings.Rotation != null)
            {
                NegotiateAutomaticRotate(settings);
                NegotiateAutomaticBorderDetection(settings);
            }

            return Enable(settings);
        }

        private bool NegotiateArea(ScanSettings scanSettings)
        {
            var area = scanSettings.Area;

            if (area == null)
            {
                return false;
            }

            try
            {
                var cap = new Capability(Capabilities.IUnits, TwainType.Int16, _applicationId, SourceId);
                if ((Units)cap.GetBasicValue().Int16Value != area.Units)
                {
                    Capability.SetCapability(Capabilities.IUnits, (short)area.Units, _applicationId, SourceId);
                }
            }
            catch
            {
                // Do nothing if the data source does not support the requested capability
            }

            var imageLayout = new ImageLayout
            {
                Frame = new Frame
                {
                    Left = new Fix32(area.Left),
                    Top = new Fix32(area.Top),
                    Right = new Fix32(area.Right),
                    Bottom = new Fix32(area.Bottom)
                }
            };

            var result = Twain32Native.DsImageLayout(
                _applicationId,
                SourceId,
                DataGroup.Image,
                DataArgumentType.ImageLayout,
                Message.Set,
                imageLayout);

            if (result != TwainResult.Success)
            {
                throw new TwainException("DsImageLayout.GetDefault error", result);
            }

            return true;
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

        public bool Enable(ScanSettings settings)
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
                return false;
            }
            return true;
        }

        public static DataSource GetDefault(Identity applicationId, IWindowsMessageHook messageHook)
        {
            var defaultSourceId = new Identity();

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
                var status = DataSourceManager.GetConditionCode(applicationId, null);
                throw new TwainException("Error getting information about the default source: " + result, result, status);
            }

            return new DataSource(applicationId, defaultSourceId, messageHook);
        }

        public static DataSource UserSelected(Identity applicationId, IWindowsMessageHook messageHook)
        {
            var defaultSourceId = new Identity();

            // Show the TWAIN interface to allow the user to select a source
            Twain32Native.DsmIdentity(
                applicationId,
                IntPtr.Zero,
                DataGroup.Control,
                DataArgumentType.Identity,
                Message.UserSelect,
                defaultSourceId);

            return new DataSource(applicationId, defaultSourceId, messageHook);
        }
        public static List<string> GetAllSourceNames(Identity applicationId)
        {
            var sources = new List<string>();
            Identity id = new Identity();

            // Get the first source
            var result = Twain32Native.DsmIdentity(
                applicationId,
                IntPtr.Zero,
                DataGroup.Control,
                DataArgumentType.Identity,
                Message.GetFirst,
                id);

            if (result == TwainResult.EndOfList)
            {
                return sources;
            }
            else if (result != TwainResult.Success)
            {
                throw new TwainException("Error getting first source.", result);
            }
            else
            {
                sources.Add(id.ProductName);
            }

            while (true)
            {
                id = new Identity();
                // Get the next source
                result = Twain32Native.DsmIdentity(
                    applicationId,
                    IntPtr.Zero,
                    DataGroup.Control,
                    DataArgumentType.Identity,
                    Message.GetNext,
                    id);

                if (result == TwainResult.EndOfList)
                {
                    break;
                }
                else if (result != TwainResult.Success)
                {
                    throw new TwainException("Error enumerating sources.", result);
                }

                sources.Add(id.ProductName);
            }

            return sources;
        }
         
        public static DataSource GetSource(string sourceProductName, Identity applicationId, IWindowsMessageHook messageHook)
        {
            Identity id = new Identity();

            // Get the first source
            var result = Twain32Native.DsmIdentity(
                applicationId,
                IntPtr.Zero,
                DataGroup.Control,
                DataArgumentType.Identity,
                Message.GetFirst,
                id);

            if (result == TwainResult.EndOfList)
            {
                return null;
            }
            else if (result != TwainResult.Success)
            {
                throw new TwainException("Error getting first source.", result);
            }
            else if (id.ProductName == sourceProductName)
            {
                return new DataSource(applicationId, id, messageHook);
            }

            while (true)
            {
                id = new Identity();
                // Get the next source
                result = Twain32Native.DsmIdentity(
                    applicationId,
                    IntPtr.Zero,
                    DataGroup.Control,
                    DataArgumentType.Identity,
                    Message.GetNext,
                    id);

                if (result == TwainResult.EndOfList)
                {
                    break;
                }
                else if (result != TwainResult.Success)
                {
                    throw new TwainException("Error enumerating sources.", result);
                }

                else if (id.ProductName == sourceProductName)
                {
                    return new DataSource(applicationId, id, messageHook);
                }
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
            if (SourceId.Id != 0)
            {
                UserInterface userInterface = new UserInterface();

                TwainResult result = Twain32Native.DsUserInterface(
                    _applicationId,
                    SourceId,
                    DataGroup.Control,
                    DataArgumentType.UserInterface,
                    Message.DisableDS,
                    userInterface);

                if (result != TwainResult.Failure)
                {
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
}

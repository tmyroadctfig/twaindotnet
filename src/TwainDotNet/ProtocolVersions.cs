 
using System;
using System.Collections.Generic;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    /// <summary>
    /// Contains the minimum TWAIN protocol version for various things.
    /// </summary>
    public static class ProtocolVersions
    {
        internal static readonly Version v10 = new Version(1, 0);
        internal static readonly Version v11 = new Version(1, 1);
        internal static readonly Version v15 = new Version(1, 5);
        internal static readonly Version v16 = new Version(1, 6);
        internal static readonly Version v17 = new Version(1, 7);
        internal static readonly Version v18 = new Version(1, 8);
        internal static readonly Version v19 = new Version(1, 9);
        internal static readonly Version v20 = new Version(2, 0);
        internal static readonly Version v21 = new Version(2, 1);
        internal static readonly Version v22 = new Version(2, 2);
        internal static readonly Version v23 = new Version(2, 3);


        static readonly Dictionary<Capabilities, Version> __capMinVersions = new Dictionary<Capabilities, Version>
        {
            { Capabilities.AXferMech , v18 },
            { Capabilities.Alarms, v18 },
            { Capabilities.AlarmVolume, v18 },
            { Capabilities.Author, v10 },
            { Capabilities.AutoFeed, v10 },
            { Capabilities.AutomaticCapture, v18 },
            { Capabilities.AutomaticSenseMedium, v21 },
            { Capabilities.AutoScan, v16 },
            { Capabilities.BatteryMinutes, v18 },
            { Capabilities.BatteryPercentage, v18 },
            { Capabilities.CameraEnabled, v20 },
            { Capabilities.CameraOrder, v20 },
            { Capabilities.CameraPreviewUI, v18 },
            { Capabilities.CameraSide, v19 },
            { Capabilities.Caption, v10 },
            { Capabilities.ClearBuffers, v18 },
            { Capabilities.ClearPage, v10 },
            { Capabilities.CustomDSData, v17 },
            { Capabilities.CustomInterfaceGuid, v21 },
            { Capabilities.DeviceEvent, v18 },
            { Capabilities.DeviceOnline, v16 },
            { Capabilities.DeviceTimeDate, v18 },
            { Capabilities.DoubleFeedDetection, v22 },
            { Capabilities.DoubleFeedDetectionLength, v22 },
            { Capabilities.DoubleFeedDetectionResponse, v22 },
            { Capabilities.DoubleFeedDetectionSensitivity, v22 },
            { Capabilities.Duplex, v17 },
            { Capabilities.DuplexEnabled, v17 },
            { Capabilities.Enabledsuionly, v17 },
            { Capabilities.Endorser, v17 },
            { Capabilities.Extendedcaps, v10 },
            { Capabilities.FeederAlignment, v18 },
            { Capabilities.FeederEnabled, v10 },
            { Capabilities.FeederLoaded, v10 },
            { Capabilities.FeederOrder, v18 },
            { Capabilities.Feederpocket, v20 },
            { Capabilities.FeederPrep, v20},
            { Capabilities.FeedPage, v10 },
            { Capabilities.Indicators, v11 },
            { Capabilities.IndicatorsMode, v22 },
            { Capabilities.JobControl, v17 },
            { Capabilities.Language, v18 },
            { Capabilities.MaxBatchBuffers, v18 },
            { Capabilities.MicrEnabled, v20 },
            { Capabilities.PaperDetectable, v16 },
            { Capabilities.PaperHandling, v22 },
            { Capabilities.PowerSaveTime, v18 },
            { Capabilities.PowerSupply, v18 },
            { Capabilities.Printer, v18 },
            { Capabilities.PrinterEnabled, v18 },
            { Capabilities.PrinterCharRotation, v23 },
            { Capabilities.PrinterFontStyle, v23 },
            { Capabilities.PrinterIndex, v18 },
            { Capabilities.PrinterIndexLeadChar, v23 },
            { Capabilities.PrinterIndexMaxValue, v23 },
            { Capabilities.PrinterIndexNumDigits, v23 },
            { Capabilities.PrinterIndexStep, v23 },
            { Capabilities.PrinterIndexTrigger, v23 },
            { Capabilities.PrinterMode, v18 },
            { Capabilities.PrinterString, v18 },
            { Capabilities.PrinterStringPreview, v23 },
            { Capabilities.PrinterSuffix, v18 },
            { Capabilities.PrinterVerticalOffset, v22 },
            { Capabilities.ReAcquireAllowed, v18 },
            { Capabilities.RewindPage, v10 },
            { Capabilities.Segmented, v19 },
            { Capabilities.SerialNumber, v18 },
            { Capabilities.SupportedCapabilities, v10 },
            { Capabilities.SupportedCapsSegmentUnique, v22 },
            { Capabilities.SupportedDATs, v22 },
            { Capabilities.TimeBeforeFirstCapture, v18 },
            { Capabilities.TimeBetweenCaptures, v18 },
            { Capabilities.Timedate, v10 },
            { Capabilities.ThumbnailsEnabled, v17 },
            { Capabilities.UIControllable, v16 },
            { Capabilities.XferCount, v10 },

            { Capabilities.Autobright, v10 },
            { Capabilities.AutoDiscardBlankPages, v20 },
            { Capabilities.Automaticborderdetection, v18 },
            { Capabilities.AutomaticColorEnabled, v21 },
            { Capabilities.AutomaticColorNonColorPixelType, v18 },
            { Capabilities.AutomaticCropUsesFrame, v21 },
            { Capabilities.Automaticdeskew, v18 },
            { Capabilities.AutomaticLengthDetection, v21 },
            { Capabilities.Automaticrotate, v18 },
            { Capabilities.Autosize, v20 },
            { Capabilities.Barcodedetectionenabled, v18 },
            { Capabilities.Barcodemaxretries, v18 },
            { Capabilities.Barcodemaxsearchpriorities, v18 },
            { Capabilities.Barcodesearchmode, v18 },
            { Capabilities.Barcodesearchpriorities, v18 },
            { Capabilities.Barcodetimeout, v18 },
            { Capabilities.BitDepth, v10 },
            { Capabilities.Bitdepthreduction, v15 },
            { Capabilities.Bitorder, v10 },
            { Capabilities.Bitordercodes, v10 },
            { Capabilities.Brightness, v10 },
            { Capabilities.Ccittkfactor, v10 },
            { Capabilities.ColorManagementEnabled, v21 },
            { Capabilities.ICompression, v10 },
            { Capabilities.Contrast, v10 },
            { Capabilities.CustHalftone, v10 },
            { Capabilities.ExposureTime, v10 },
            { Capabilities.Extimageinfo, v17 },
            { Capabilities.Feedertype, v19 },
            { Capabilities.FilmType, v22 },
            { Capabilities.Filter, v10 },
            { Capabilities.Flashused, v16 }, // maybe
            { Capabilities.Flashused2, v18 },
            { Capabilities.Fliprotation, v18 },
            { Capabilities.Frames, v10 },
            { Capabilities.Gamma, v10 },
            { Capabilities.Halftones, v10 },
            { Capabilities.Highlight, v10 },
            { Capabilities.Iccprofile, v19 },
            { Capabilities.Imagedataset, v17 },
            { Capabilities.ImageFileFormat, v10 },
            { Capabilities.Imagefilter, v18 },
            { Capabilities.ImageMerge, v21 },
            { Capabilities.ImageMergeHeightThreshold, v21 },
            { Capabilities.Jpegpixeltype, v10 },
            { Capabilities.Jpegquality, v19 },
            { Capabilities.JpegSubsampling, v22 },
            { Capabilities.LampState, v10 },
            { Capabilities.Lightpath, v10 },
            { Capabilities.LightSource, v10 },
            { Capabilities.MaxFrames, v10 },
            { Capabilities.Minimumheight, v17 },
            { Capabilities.Minimumwidth, v17 },
            { Capabilities.Mirror, v22 },
            { Capabilities.Noisefilter, v18 },
            { Capabilities.Orientation, v10 },
            { Capabilities.Overscan, v18 },
            { Capabilities.Patchcodedetectionenabled, v18 },
            { Capabilities.Patchcodemaxretries, v18 },
            { Capabilities.Patchcodemaxsearchpriorities, v18 },
            { Capabilities.Patchcodesearchmode, v18 },
            { Capabilities.Patchcodesearchpriorities, v18 },
            { Capabilities.Patchcodetimeout, v18 },
            { Capabilities.PhysicalHeight, v10 },
            { Capabilities.PhysicalWidth, v10 },
            { Capabilities.Pixelflavor, v10 },
            { Capabilities.Pixelflavorcodes, v10 },
            { Capabilities.IPixelType, v10 },
            { Capabilities.Planarchunky, v10 },
            { Capabilities.Rotation, v10 },
            { Capabilities.Shadow, v10 },
            { Capabilities.Supportedbarcodetypes, v18 },
            { Capabilities.SupportedExtImageInfo, v21 },
            { Capabilities.Supportedpatchcodetypes, v18 },
            { Capabilities.Supportedsizes, v10 },
            { Capabilities.Threshold, v10 },
            { Capabilities.Tiles, v10 },
            { Capabilities.Timefill, v10 },
            { Capabilities.Undefinedimagesize, v16 },
            { Capabilities.IUnits, v10 },
            { Capabilities.IXferMech, v10 },
            { Capabilities.XNativeResolution, v10 },
            { Capabilities.XResolution, v10 },
            { Capabilities.Xscaling, v10 },
            { Capabilities.YNativeResolution, v10 },
            { Capabilities.YResolution, v10 },
            { Capabilities.Yscaling, v10 },
            { Capabilities.Zoomfactor, v18 },
        };

        /// <summary>
        /// Gets the minimum TWAIN protocol version for a capability.
        /// </summary>
        /// <param name="id">The capability type.</param>
        /// <returns></returns>
        public static Version GetMinimumVersion(Capabilities id)
        {
            if (__capMinVersions.ContainsKey(id))
            {
                return __capMinVersions[id];
            }
            return v10;
        }
    }
}

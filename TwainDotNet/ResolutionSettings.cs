using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    public class ResolutionSettings
    {
        /// <summary>
        /// The DPI to scan at. Set to null to use the current default setting.
        /// </summary>
        public int? Dpi { get; set; }

        /// <summary>
        /// The colour settings to use.
        /// </summary>
        public ColourSetting ColourSetting { get; set; }

        public PixelType GetPixelType()
        {
            switch (ColourSetting)
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

        public ushort GetBitDepth()
        {
            switch (ColourSetting)
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

        /// <summary>
        /// Fax quality resolution.
        /// </summary>
        public static readonly ResolutionSettings Fax = new ResolutionSettings()
        {
            Dpi = 200,
            ColourSetting = ColourSetting.BlackAndWhite
        };

        /// <summary>
        /// Photocopier quality resolution.
        /// </summary>
        public static readonly ResolutionSettings Photocopier = new ResolutionSettings()
        {
            Dpi = 300,
            ColourSetting = ColourSetting.GreyScale
        };

        /// <summary>
        /// Colour photocopier quality resolution.
        /// </summary>
        public static readonly ResolutionSettings ColourPhotocopier = new ResolutionSettings()
        {
            Dpi = 300,
            ColourSetting = ColourSetting.Colour
        };
    }

    public enum ColourSetting
    {
        BlackAndWhite,

        GreyScale,

        Colour
    }
}

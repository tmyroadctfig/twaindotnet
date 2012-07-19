using System;
using System.Collections.Generic;
using System.Text;

namespace TwainDotNet.TwainNative
{
    /// <summary>
    /// Twain spec ICAP_ORIENTATION values.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Default is zero rotation, same as Portrait.
        /// </summary>
        Default = 0,
        Rotate90 = 1,
        Rotate180 = 2,
        Rotate270 = 3,
        Portrait = Default,
        Landscape = Rotate270,

        /// <summary>
        /// Following require Twain 2.0+. 
        /// </summary>
        Auto = 4,
        AutoText = 5,
        AutoPicture = 6
    }
}

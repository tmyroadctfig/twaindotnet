using System;
using System.Collections.Generic;
using System.Text;

namespace TwainDotNet.TwainNative
{
    /// <summary>
    /// TWON_...
    /// </summary>
    public enum ContainerType : short
    {
        Array = 0x0003,
        Enum = 0x0004,
        One = 0x0005,
        Range = 0x0006,
        DontCare = -1
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TwainDotNet.TwainNative
{
    [Flags]
    public enum DataGroup : int
    {									
        Control = 0x0001,
        Image = 0x0002,
        Audio = 0x0004,
        DsmMask = 0xFFFF,

        //TWAIN 2.0 values
        //Dsm2 = 0x10000000,
        //App2 = 0x20000000,
        //Ds2 = 0x30000000,
    }
}

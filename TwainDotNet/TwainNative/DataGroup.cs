using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwainDotNet.TwainNative
{
    [Flags]
    public enum DataGroup : short
    {									
        Control = 0x0001,
        Image = 0x0002,
        Audio = 0x0004
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TwainDotNet.TwainNative
{
    public enum TwainType : short
    {
        Int8 = 0x0000,
        Int16 = 0x0001,
        Int32 = 0x0002,
        UInt8 = 0x0003,
        UInt16 = 0x0004,
        UInt32 = 0x0005,
        Bool = 0x0006,
        Fix32 = 0x0007,
        Frame = 0x0008,
        Str32 = 0x0009,
        Str64 = 0x000a,
        Str128 = 0x000b,
        Str255 = 0x000c,
        Str1024 = 0x000d,
        Str512 = 0x000e
    }
}

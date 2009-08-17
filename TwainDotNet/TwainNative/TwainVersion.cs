using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public struct TwainVersion
    {									// TW_VERSION
        public short MajorNum;
        public short MinorNum;
        public Language Language;
        public Country Country;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
        public string Info;
    }
}

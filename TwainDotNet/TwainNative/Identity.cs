using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public class Identity
    {
        public IntPtr Id;
        public TwainVersion Version;
        public short ProtocolMajor;
        public short ProtocolMinor;
        public int SupportedGroups;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
        public string Manufacturer;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
        public string ProductFamily;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
        public string ProductName;
    }
}

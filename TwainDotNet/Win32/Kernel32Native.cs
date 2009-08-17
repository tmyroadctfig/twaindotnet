using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.Win32
{
    public static class Kernel32Native
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalAlloc(GlobalAllocFlags flags, int size);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalLock(IntPtr handle);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern bool GlobalUnlock(IntPtr handle);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalFree(IntPtr handle);
    }

    [Flags]
    public enum GlobalAllocFlags : uint
    {
        MemFixed = 0,

        MemMoveable = 0x2,

        ZeroInit = 0x40,

        Handle = MemMoveable | ZeroInit
    }
}

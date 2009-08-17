using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct WindowsMessage
    {
        public IntPtr hwnd;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public int x;
        public int y;
    }
}

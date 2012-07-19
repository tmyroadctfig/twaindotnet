using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class UserInterface
    {
        public short ShowUI;				// bool is strictly 32 bit, so use short
        public short ModalUI;
        public IntPtr ParentHand;
    }
}

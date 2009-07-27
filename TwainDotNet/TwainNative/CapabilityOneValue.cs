using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using TwainDotNet.Win32;
using log4net;

namespace TwainDotNet.TwainNative
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class CapabilityOneValue
    {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(CapabilityOneValue));

        public CapabilityOneValue(Capabilities capabilities, int value, TwainType type)
        {
            Capabilities = capabilities;
            ContainerType = ContainerType.One;

            Handle = Kernel32Native.GlobalAlloc(GlobalAllocFlags.Handle, 6);
            IntPtr pv = Kernel32Native.GlobalLock(Handle);
            Marshal.WriteInt16(pv, 0, (short)type);
            Marshal.WriteInt32(pv, 2, value);
            Kernel32Native.GlobalUnlock(Handle);
        }

        ~CapabilityOneValue()
        {
            if (Handle != IntPtr.Zero)
            {
                Kernel32Native.GlobalFree(Handle);
            }
        }

        public Capabilities Capabilities;
        public ContainerType ContainerType;
        public IntPtr Handle;

        public int GetValue()
        {            
            IntPtr pv = Kernel32Native.GlobalLock(Handle);
            int value = Marshal.ReadInt32(pv, 2);
            Kernel32Native.GlobalUnlock(Handle);
            return value;
        }
    }
}

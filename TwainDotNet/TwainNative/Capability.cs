using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using TwainDotNet.Win32;

namespace TwainDotNet.TwainNative
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class Capability
    {
        public Capability(Capabilities capabilities, int value, TwainType type)
        {
            Capabilities = capabilities;
            ContainerType = ContainerType.One;

            Handle = Kernel32Native.GlobalAlloc(GlobalAllocFlags.Handle, 6);
            IntPtr pv = Kernel32Native.GlobalLock(Handle);
            Marshal.WriteInt16(pv, 0, (short)type);
            Marshal.WriteInt32(pv, 2, value);
            Kernel32Native.GlobalUnlock(Handle);
        }

        ~Capability()
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

        public static void SetCapability(Capabilities capabilities, int value,
            Identity applicationId, Identity sourceId)
        {
            SetCapability(capabilities, value, TwainType.Int32, applicationId, sourceId);
        }

        public static void SetCapability(Capabilities capabilities, ushort value,
            Identity applicationId, Identity sourceId)
        {
            SetCapability(capabilities, value, TwainType.UInt16, applicationId, sourceId);
        }

        public static void SetCapability(Capabilities capabilities, short value,
            Identity applicationId, Identity sourceId)
        {
            SetCapability(capabilities, (int)value, TwainType.Int16, applicationId, sourceId);
        }

        public static void SetCapability(Capabilities capabilities, bool value,
            Identity applicationId, Identity sourceId)
        {
            SetCapability(capabilities, value ? 1 : 0, TwainType.Bool, applicationId, sourceId);
        }

        public static void SetCapability(Capabilities capabilities, int value, TwainType type,
            Identity applicationId, Identity sourceId)
        {
            Capability capability = new Capability(capabilities, value, type);

            TwainResult result = Twain32Native.DsCapability(
                    applicationId,
                    sourceId,
                    DataGroup.Control,
                    DataArgumentType.Capability,
                    Message.Set,
                    capability);

            if (result != TwainResult.Success)
            {
                throw new TwainException("Failed to set capability.", result);
            }
        }

        public static int GetCapability(Capabilities capabilities, TwainType type, Identity applicationId,
            Identity sourceId)
        {
            Capability capability = new Capability(capabilities, 0, type);

            TwainResult result = Twain32Native.DsCapability(
                    applicationId,
                    sourceId,
                    DataGroup.Control,
                    DataArgumentType.Capability,
                    Message.Get,
                    capability);

            if (result != TwainResult.Success)
            {
                throw new TwainException("Failed to get capability.", result);
            }

            return capability.GetValue();
        }

        public static int GetInt32Capability(Capabilities capabilities, Identity applicationId,
            Identity sourceId)
        {
            return GetCapability(capabilities, TwainType.Int32, applicationId, sourceId);
        }

        public static short GetInt16Capability(Capabilities capabilities, Identity applicationId,
            Identity sourceId)
        {
            return (short) GetCapability(capabilities, TwainType.Int16, applicationId, sourceId);
        }

        public static bool GetBoolCapability(Capabilities capabilities, Identity applicationId,
            Identity sourceId)
        {
            return GetCapability(capabilities, TwainType.Bool, applicationId, sourceId) == 1;
        }
    }
}

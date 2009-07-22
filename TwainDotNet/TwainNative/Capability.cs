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
    public class Capability
    {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(Capability));

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
            log.Debug(string.Format("Attempting to set capabilities:{0}, value:{1}, type:{1}",
                capabilities, value, type));

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
                log.Debug(string.Format("Failed to set capabilities:{0}, value:{1}, type:{1}, result:{2}",
                    capabilities, value, type, result));

                if (result == TwainResult.Failure)
                {
                    Status status = new Status();

                    Twain32Native.DsmStatus(
                        applicationId,
                        sourceId,
                        DataGroup.Control,
                        DataArgumentType.Status,
                        Message.Get,
                        status);

                    log.Error(string.Format("Failed to set capabilites:{0} reason: {1}",
                        capabilities, status.ConditionCode));

                    throw new TwainException("Failed to set capability.", result, status.ConditionCode);
                }
                else if (result == TwainResult.CheckStatus)
                {
                    // TODO: query the capability from the device. E.g. a request to set resolution to 301 dpi
                    // may only successfully set the value to 300 dpi.
                }

                throw new TwainException("Failed to set capability.", result);
            }

            log.Debug("Set capabilities successfully");
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
                Status status = new Status();

                Twain32Native.DsmStatus(
                    applicationId,
                    sourceId,
                    DataGroup.Control,
                    DataArgumentType.Status,
                    Message.Get,
                    status);

                log.Error(string.Format("Failed to get capabilites:{0} reason: {1}",
                    capabilities, status.ConditionCode));

                throw new TwainException("Failed to get capability.", result, status.ConditionCode);
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

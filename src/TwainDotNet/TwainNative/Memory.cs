using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{

    // NOTE: Memory has to be a *struct* not *class* because it's embedded directly in other classes/structs
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct Memory
    {
        public MemoryFlags Flags;
        public UInt32 Length;
        public IntPtr TheMem;
    }

    /// <summary>
    /// Encodes which entity releases the buffer and how the buffer is referenced. 
    /// (page 8-47)
    /// 
    /// TWMF_APPOWNS 0x1
    /// TWMF_DSMOWNS 0x2
    /// TWMF_DSOWNS 0x4
    /// TWMF_POINTER 0x8
    /// TWMF_HANDLE 0x10
    /// </summary>
    public enum MemoryFlags : uint   // UInt32
    {
        AppOwns = 0x1,
        DsmOwns = 0x2,
        DsOwns = 0x4,
        Pointer = 0x8,
        Handle = 0x10,
    }

}

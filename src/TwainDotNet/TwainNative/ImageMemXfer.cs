using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    /// <summary>
    /// used with  DG_CONTROL / DAT_IMAGEMEMXFER / MSG_GET
    /// typedef struct {
    ///     TW_UINT16 Compression;
    ///     TW_UINT32 BytesPerRow;
    ///     TW_UINT32 Columns;
    ///     TW_UINT32 Rows;
    ///     TW_UINT32 XOffset;
    ///     TW_UINT32 YOffset;
    ///     TW_UINT32 BytesWritten;
    ///     TW_MEMORY Memory;
    /// } TW_IMAGEMEMXFER, FAR* pTW_IMAGEMEMXFER;
    /// 
    /// typedef struct {
    ///     TW_UINT32 Flags;
    ///     TW_UINT32 Length;
    ///     TW_MEMREF TheMem;
    /// } TW_MEMORY, FAR* pTW_MEMORY;
    /// </summary>

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class ImageMemXfer
    {
        public Compression Compression;
        public UInt32 BytesPerRow;
        public UInt32 Columns;
        public UInt32 Rows;
        public UInt32 XOffset;
        public UInt32 YOffset;
        public UInt32 BytesWritten;
        public Memory Memory;
    }    

}

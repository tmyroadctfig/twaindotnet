using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    /// <summary>
    /// used with  DG_CONTROL / DAT_SETUPMEMXFER / MSG_GET
    /// typedef struct {
    ///     TW_UINT32 MinBufSize /* Minimum buffer size in bytes */
    ///     TW_UINT32 MaxBufSize /* Maximum buffer size in bytes */
    ///     TW_UINT32 Preferred /* Preferred buffer size in bytes */
    /// } TW_SETUPMEMXFER, FAR* pTW_SETUPMEMXFER;
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class SetupMemXfer
    {
        public UInt32 MinBufSize;
        public UInt32 MaxBufSize;
        public UInt32 Preferred;
    }
}

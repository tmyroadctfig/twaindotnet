using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    /// <summary>
    /// /* DAT_PENDINGXFERS. Used with MSG_ENDXFER to indicate additional data. */
    /// typedef struct {
    ///    TW_UINT16 Count;
    ///    union {
    ///       TW_UINT32 EOJ;
    ///       TW_UINT32 Reserved;
    ///    };
    /// } TW_PENDINGXFERS, FAR *pTW_PENDINGXFERS;
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class PendingXfers
    {
        public short Count;
        public int Eoj;
    }
}

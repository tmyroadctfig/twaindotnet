using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    /// <summary>
    /// DAT_USERINTERFACE. Coordinates UI between application and data source. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class UserInterface
    {
        /// <summary>
        /// TRUE if DS should bring up its UI
        /// </summary>
        public short ShowUI;				// bool is strictly 32 bit, so use short

        /// <summary>
        /// For Mac only - true if the DS's UI is modal
        /// </summary>
        public short ModalUI;

        /// <summary>
        /// For windows only - Application window handle
        /// </summary>
        public IntPtr ParentHand;
    }
}

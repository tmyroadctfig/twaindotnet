using System;
using System.Collections.Generic;
using System.Text;

namespace TwainDotNet.TwainNative
{
    public enum TransferMechanism : short
    {
        Native = 0,

        File = 1,

        Memory = 2,

        // Value 3 was removed

        MemFile = 4
    }
}

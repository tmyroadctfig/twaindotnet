using System;
using System.Collections.Generic;
using System.Text;

namespace TwainDotNet.TwainNative
{
    public enum ConditionCode : short
    {
        Success = 0,

        /// <summary>
        /// Unknown failure.
        /// </summary>
        Bummer = 1,

        LowMemory = 2,

        NoDataSource = 3,

        /// <summary>
        /// Data source is already connected to the maximum number of connections.
        /// </summary>
        MaxConntions = 4,

        /// <summary>
        /// Data source or data source manager reported an error.
        /// </summary>
        OperationError = 5,

        BadCapability = 6,

        /// <summary>
        /// Unknown data group/data argument type combination.
        /// </summary>
        BadProtocol = 9,

        BadValue = 10,

        /// <summary>
        /// Message out of expected sequence.
        /// </summary>
        SequenceError = 11,

        /// <summary>
        /// Unknown destination application/source.
        /// </summary>
        BadDestination = 12,

        CapabilityUnsupported  = 13,

        /// <summary>
        /// Operation not supported by capability.
        /// </summary>
        CapabilityBadOperation = 14,

        CapabilitySequenceError = 15,

        /// <summary>
        /// File system operation is denied.
        /// </summary>
        Denied = 16,

        FileExists = 17,

        FileNotFound = 18,

        DirectoryNotEmpty = 19,

        PaperJam = 20,

        PaperDoubleFeed = 21,

        FileWriteError = 22,
                
        CheckDeviceOnline = 23
    }
}

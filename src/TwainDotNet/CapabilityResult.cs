using System;
using System.Collections.Generic;
using System.Text;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    public abstract class CapabilityResult
    {
        public ConditionCode ConditionCode { get; set; }

        public TwainResult ErrorCode { get; set; }
    }

    public class BasicCapabilityResult : CapabilityResult
    {
        public int RawBasicValue { get; set; }

        public bool BoolValue { get { return RawBasicValue == 1; } }

        public short Int16Value { get { return (short)RawBasicValue; } }

        public int Int32Value { get { return RawBasicValue; } }
    }
}

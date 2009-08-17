using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    public class TwainException : ApplicationException
    {
        public TwainException()
            : this(null, null)
        {
        }

        public TwainException(string message)
            : this(message, null)
        {
        }

        public TwainException(string message, TwainResult returnCode)
            : this(message, null)
        {
            ReturnCode = returnCode;
        }

        public TwainException(string message, TwainResult returnCode, ConditionCode conditionCode)
            : this(message, null)
        {
            ReturnCode = returnCode;
            ConditionCode = conditionCode;
        }

        protected TwainException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        public TwainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TwainResult? ReturnCode { get; private set; }

        public ConditionCode? ConditionCode { get; private set; }
    }
}

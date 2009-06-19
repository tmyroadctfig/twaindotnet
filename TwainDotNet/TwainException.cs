using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

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

        protected TwainException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        public TwainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

using System;

namespace TwainDotNet
{
    public class ScanningCompleteEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }
        public object ExtTag { private set; get; }

        public ScanningCompleteEventArgs(Exception exception,object extTag)
        {
            this.ExtTag = extTag;
            this.Exception = exception;
        }
    }
}

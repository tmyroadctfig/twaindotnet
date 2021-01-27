using System;
using System.Drawing;

namespace TwainDotNet
{
    public class TransferImageEventArgs : EventArgs
    {
        public Bitmap Image { get; private set; }
        public bool ContinueScanning { get; private set; }
        public int Count { get; set; }

        public object ExtTag { set; get; }


        public TransferImageEventArgs(Bitmap image, int count,object extTag)
        {
            this.Image = image;
            this.Count = count;
            this.ContinueScanning = count != 0;
            this.ExtTag = extTag;
        }
    }
}

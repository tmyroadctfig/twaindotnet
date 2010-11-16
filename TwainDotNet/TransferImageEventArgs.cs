using System;
using System.Drawing;

namespace TwainDotNet
{
    public class TransferImageEventArgs : EventArgs
    {
        public Bitmap Image { get; private set; }

        public TransferImageEventArgs(Bitmap image)
        {
            this.Image = image;
        }
    }
}

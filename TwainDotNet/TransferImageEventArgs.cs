using System;
using System.Drawing;

namespace TwainDotNet
{
    public class TransferImageEventArgs : EventArgs
    {
        public Image Image { get; private set; }

        public TransferImageEventArgs(Image image)
        {
            this.Image = image;
        }
    }
}

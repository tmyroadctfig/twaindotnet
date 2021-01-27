using System;
using System.Drawing;

namespace TwainDotNet
{
    public class TransferImageEventArgs : EventArgs
    {
        public Bitmap Image { get; private set; }
        public bool ContinueScanning { get; set; }
        public float PercentComplete { get; set; }

        public TransferImageEventArgs(Bitmap image, bool continueScanning, float percentComplete)
        {
            this.Image = image;
            this.ContinueScanning = continueScanning;
            this.PercentComplete = percentComplete;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using log4net;

namespace TwainDotNet.Win32
{
    public class BitmapRenderer
    {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(BitmapRenderer));

        IntPtr _bitmapPointer;
        IntPtr _pixelInfoPointer;
        Rectangle _rectangle;

        public BitmapRenderer(IntPtr dibHandle)
        {
            _bitmapPointer = Kernel32Native.GlobalLock(dibHandle);

            BitmapInfoHeader bitmapInfo = new BitmapInfoHeader();
            Marshal.PtrToStructure(_bitmapPointer, bitmapInfo);
            log.Debug(bitmapInfo.ToString());

            _rectangle = new Rectangle();
            _rectangle.X = _rectangle.Y = 0;
            _rectangle.Width = bitmapInfo.Width;
            _rectangle.Height = bitmapInfo.Height;

            if (bitmapInfo.SizeImage == 0)
            {
                bitmapInfo.SizeImage = ((((bitmapInfo.Width * bitmapInfo.BitCount) + 31) & ~31) >> 3) * bitmapInfo.Height;
            }


            // The following code only works on x86
            Debug.Assert(Marshal.SizeOf(typeof(IntPtr)) == 4);

            int pixelInfoPointer = bitmapInfo.ClrUsed;
            if ((pixelInfoPointer == 0) && (bitmapInfo.BitCount <= 8))
            {
                pixelInfoPointer = 1 << bitmapInfo.BitCount;
            }

            pixelInfoPointer = (pixelInfoPointer * 4) + bitmapInfo.Size + _bitmapPointer.ToInt32();

            _pixelInfoPointer = new IntPtr(pixelInfoPointer);
        }

        public Bitmap RenderToBitmap()
        {
            Bitmap bitmap = new Bitmap(_rectangle.Width, _rectangle.Height);

            Graphics graphics = Graphics.FromImage(bitmap);
            IntPtr hdc = graphics.GetHdc();

            Gdi32Native.SetDIBitsToDevice(hdc, 0, 0, _rectangle.Width, _rectangle.Height,
                0, 0, 0, _rectangle.Height, _pixelInfoPointer, _bitmapPointer, 0);

            graphics.ReleaseHdc(hdc);

            return bitmap;
        }
    }
}

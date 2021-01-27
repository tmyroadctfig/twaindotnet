using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using log4net;

namespace TwainDotNet.Win32
{
    public static class BitmapRenderer
    {
        static ILog log = LogManager.GetLogger(typeof(BitmapRenderer));

        private static float PpmToDpi(double pixelsPerMeter)
        {
            double pixelsPerMillimeter = (double)pixelsPerMeter / 1000.0;
            double dotsPerInch = pixelsPerMillimeter * 25.4;
            return (float)Math.Round(dotsPerInch, 2);
        }
        
        public static Bitmap NewBitmapFromHBitmap(IntPtr dibHandle) {

            IntPtr _bitmapPointer;
            IntPtr _pixelInfoPointer;
            Rectangle _rectangle;
            BitmapInfoHeader _bitmapInfo;
            Bitmap bitmap;

            _bitmapPointer = Kernel32Native.GlobalLock(dibHandle);
            try {
                _bitmapInfo = new BitmapInfoHeader();
                Marshal.PtrToStructure(_bitmapPointer, _bitmapInfo);
                log.Debug(_bitmapInfo.ToString());

                _rectangle = new Rectangle();
                _rectangle.X = _rectangle.Y = 0;
                _rectangle.Width = _bitmapInfo.Width;
                _rectangle.Height = _bitmapInfo.Height;

                if (_bitmapInfo.SizeImage == 0) {
                    _bitmapInfo.SizeImage = ((((_bitmapInfo.Width * _bitmapInfo.BitCount) + 31) & ~31) >> 3) * _bitmapInfo.Height;
                }


                // compute the offset to the pixel info, which follows the bitmap info header
                { 
                    // The following code only works on x86
                    Debug.Assert(Marshal.SizeOf(typeof(IntPtr)) == 4);                
                    int pixelInfoPointer = _bitmapInfo.ClrUsed;
                    if ((pixelInfoPointer == 0) && (_bitmapInfo.BitCount <= 8)) {
                        pixelInfoPointer = 1 << _bitmapInfo.BitCount;
                    }
                    pixelInfoPointer = (pixelInfoPointer * 4) + _bitmapInfo.Size + _bitmapPointer.ToInt32();
                    _pixelInfoPointer = new IntPtr(pixelInfoPointer);
                }

                // render to bitmap
                bitmap = new Bitmap(_rectangle.Width, _rectangle.Height);

                using (Graphics graphics = Graphics.FromImage(bitmap)) {
                    IntPtr hdc = graphics.GetHdc();

                    try {
                        Gdi32Native.SetDIBitsToDevice(hdc, 0, 0, _rectangle.Width, _rectangle.Height,
                            0, 0, 0, _rectangle.Height, _pixelInfoPointer, _bitmapPointer, 0);
                    }
                    finally {
                        graphics.ReleaseHdc(hdc);
                    }
                }

                bitmap.SetResolution(PpmToDpi(_bitmapInfo.XPelsPerMeter), PpmToDpi(_bitmapInfo.YPelsPerMeter));
            } finally {
                Kernel32Native.GlobalUnlock(dibHandle);
            }
            return bitmap;
        }

        public static Bitmap NewBitmapForImageInfo(TwainDotNet.TwainNative.ImageInfo imageInfo) 
        {
            var bitmap = new Bitmap(imageInfo.ImageWidth,imageInfo.ImageLength,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics graphics = Graphics.FromImage(bitmap)) {
                graphics.Clear(Color.White);
            }
            return bitmap;
        }

        private static void TransferPixelsGreyscale(Bitmap bitmap_dest,
            TwainDotNet.TwainNative.ImageInfo imageInfo, TwainDotNet.TwainNative.ImageMemXfer memxfer_src) {
            BitmapInfoHeaderIndexedColor bitmapInfo = new BitmapInfoHeaderIndexedColor();            
            bitmapInfo.Width = (int)memxfer_src.Columns;
            bitmapInfo.Height = -(int)memxfer_src.Rows;
            // bitmapInfo.Size = sizeof(BitmapInfoHeader);   // requires unsafe
            bitmapInfo.Size = 40; // the size of the initial header, not including color table
            bitmapInfo.Planes = 1;
            bitmapInfo.SizeImage = 0;
            bitmapInfo.BitCount = imageInfo.BitsPerPixel;

            if (imageInfo.Planar == TwainNative.TwainBool.True) {
                throw new TwainException("Planar format invalid for greyscale data.");
            }

            if (imageInfo.BitsPerPixel == 8) {
                BitmapInfoHeaderIndexedColor.setupGreyscaleIndices(ref bitmapInfo);
                bitmapInfo.ClrUsed = 256;
            }
            else if (imageInfo.BitsPerPixel == 1) {
                BitmapInfoHeaderIndexedColor.setupBWIndices(ref bitmapInfo);
                bitmapInfo.ClrUsed = 2;
            }
            else {
                throw new TwainException("TransferPixelsGreyscale() only supports 8 bits per pixel");
            }
            

            using (Graphics graphics = Graphics.FromImage(bitmap_dest)) {

                IntPtr hdc = graphics.GetHdc();
                try {
                    Gdi32Native.SetDIBitsToDevice(
                        hdc,
                        (int)memxfer_src.XOffset,
                        (int)memxfer_src.YOffset,
                        (int)memxfer_src.Columns,
                        (int)memxfer_src.Rows,
                        0, 0, 0,
                        (int)memxfer_src.Rows,
                        memxfer_src.Memory.TheMem,
                        bitmapInfo,
                        0);
                }
                finally {
                    graphics.ReleaseHdc(hdc);
                }
            }


        }

        private static void TransferPixelsRGB(Bitmap bitmap_dest,
            TwainDotNet.TwainNative.ImageInfo imageInfo, TwainDotNet.TwainNative.ImageMemXfer memxfer_src) {
            BitmapInfoHeader bitmapInfo = new BitmapInfoHeader();
            bitmapInfo.Width = (int)memxfer_src.Columns;
            bitmapInfo.Height = -(int)memxfer_src.Rows;
            // bitmapInfo.Size = sizeof(BitmapInfoHeader);   // requires unsafe
            bitmapInfo.Size = 40;
            bitmapInfo.Planes = 1;
            bitmapInfo.SizeImage = 0;
            bitmapInfo.ClrUsed = 0; // we are not using the indexed color table

            if (imageInfo.Planar == TwainNative.TwainBool.True) {
                // this means the data is in the buffer as RRRR-GGGGG-BBBB instead of RGB-RGB-RGB
                // we don't currently support this decoding
                throw new TwainException("Planar image format Unsupported.");
            }

            // imageInfo.BitsPerPixel can be (1, 8, 24, 40) - Twain Spec page (8-39)
            // "The number of bits in each image pixel (or bit depth). This value is invariant across the
            // image. 24 - bit R - G - B has BitsPerPixel = 24. 40 - bit C - M - Y - K has BitsPerPixel = 40. 
            // 8 - bit Grayscale has BitsPerPixel = 8.Black and White has BitsPerPixel = 1." 

            switch (imageInfo.BitsPerPixel) {                
                case 24:                
                    bitmapInfo.BitCount = imageInfo.BitsPerPixel;
                    break;
                case 1:    // imageInfo is 1bit B&W, should be in TransferPixelsGreyscale()
                case 8:    // imageInfo is 8bit grayscale, should be in TransferPixelsGreyscale()
                case 40:   // imageInfo is 40bit CMYL. DIBitmap only supports up to 32bits per pixel RGB.
                default:
                    throw new TwainException("TransferPixelsRGB: unhandled image bit depth: " + imageInfo.BitsPerPixel.ToString());
            }

            using (Graphics graphics = Graphics.FromImage(bitmap_dest)) {

                IntPtr hdc = graphics.GetHdc();
                try {
                    Gdi32Native.SetDIBitsToDevice(
                        hdc,
                        (int)memxfer_src.XOffset,
                        (int)memxfer_src.YOffset,
                        (int)memxfer_src.Columns,
                        (int)memxfer_src.Rows,
                        0, 0, 0,
                        (int)memxfer_src.Rows,
                        memxfer_src.Memory.TheMem,
                        bitmapInfo,
                        0);
                }
                finally {
                    graphics.ReleaseHdc(hdc);
                }
            }
        }

        public static void TransferPixels(Bitmap bitmap_dest, 
            TwainDotNet.TwainNative.ImageInfo imageInfo, TwainDotNet.TwainNative.ImageMemXfer memxfer_src) {

            switch (imageInfo.SamplesPerPixel) {
                case 1: // greyscale                    
                    switch (imageInfo.BitsPerPixel) {
                        case 1:
                        case 8:                    
                            TransferPixelsGreyscale(bitmap_dest,imageInfo,memxfer_src);
                            break;
                        default:
                            throw new TwainException("unsupported bits per pixel: " + imageInfo.BitsPerPixel);
                    }
                    break;
                case 3: // RGB
                    TransferPixelsRGB(bitmap_dest,imageInfo,memxfer_src);
                    break;
                case 4: // CMYK
                default:
                    throw new TwainException("Samples Per Pixel Unsupported: " + imageInfo.SamplesPerPixel.ToString());
            }

        }
    }
}

﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using log4net;

namespace TwainDotNet.Win32
{
    public class BitmapRenderer : IDisposable
    {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(BitmapRenderer));

        IntPtr _dibHandle;
        IntPtr _bitmapPointer;
        IntPtr _pixelInfoPointer;
        Rectangle _rectangle;
        BitmapInfoHeader _bitmapInfo;

        public BitmapRenderer(IntPtr dibHandle)
        {
            _dibHandle = dibHandle;
            _bitmapPointer = Kernel32Native.GlobalLock(dibHandle);

            _bitmapInfo = new BitmapInfoHeader();
            Marshal.PtrToStructure(_bitmapPointer, _bitmapInfo);
            log.Debug(_bitmapInfo.ToString());

            _rectangle = new Rectangle();
            _rectangle.X = _rectangle.Y = 0;
            _rectangle.Width = _bitmapInfo.Width;
            _rectangle.Height = _bitmapInfo.Height;

            if (_bitmapInfo.SizeImage == 0)
            {
                _bitmapInfo.SizeImage = ((((_bitmapInfo.Width * _bitmapInfo.BitCount) + 31) & ~31) >> 3) * _bitmapInfo.Height;
            }


            // The following code only works on x86
            Debug.Assert(Marshal.SizeOf(typeof(IntPtr)) == 4);

            int pixelInfoPointer = _bitmapInfo.ClrUsed;
            if ((pixelInfoPointer == 0) && (_bitmapInfo.BitCount <= 8))
            {
                pixelInfoPointer = 1 << _bitmapInfo.BitCount;
            }

            pixelInfoPointer = (pixelInfoPointer * 4) + _bitmapInfo.Size + _bitmapPointer.ToInt32();

            _pixelInfoPointer = new IntPtr(pixelInfoPointer);
        }

        ~BitmapRenderer()
        {
            Dispose(false);
        }

        public Bitmap RenderToBitmap()
        {
            Bitmap bitmap = new Bitmap(_rectangle.Width, _rectangle.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                IntPtr hdc = graphics.GetHdc();

                try
                {
                    Gdi32Native.SetDIBitsToDevice(hdc, 0, 0, _rectangle.Width, _rectangle.Height,
                        0, 0, 0, _rectangle.Height, _pixelInfoPointer, _bitmapPointer, 0);
                }
                finally
                {
                    graphics.ReleaseHdc(hdc);
                }
            }

            bitmap.SetResolution(PpmToDpi(_bitmapInfo.XPelsPerMeter), PpmToDpi(_bitmapInfo.YPelsPerMeter));

            return bitmap;
        }

        private static float PpmToDpi(double pixelsPerMeter)
        {
            double pixelsPerMillimeter = (double)pixelsPerMeter / 1000.0;
            double dotsPerInch = pixelsPerMillimeter * 25.4;
            return (float)Math.Round(dotsPerInch, 2);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Kernel32Native.GlobalUnlock(_dibHandle);
            Kernel32Native.GlobalFree(_dibHandle);
        }

        public static Bitmap NewBitmapForImageInfo(TwainDotNet.TwainNative.ImageInfo imageInfo) 
        {
            return new Bitmap(imageInfo.ImageWidth,imageInfo.ImageLength,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        public static void TransferPixels_Test(Bitmap bitmap_dest, ref int pixel_number, TwainDotNet.TwainNative.ImageInfo imageInfo, TwainDotNet.TwainNative.ImageMemXfer memxfer_src)
        {
            
            int bytes_per_pixel = imageInfo.BitsPerPixel / 8;
            int num_pixels_in_buffer = (int)memxfer_src.BytesWritten / bytes_per_pixel;

            // iterate through provided pixels, copying out pixel data
            for ( int i=0 ; i < num_pixels_in_buffer ; i++ ) {
                            
                int x = pixel_number % bitmap_dest.Width;
                int y = pixel_number / bitmap_dest.Width;
                bitmap_dest.SetPixel(x,y,Color.Red);
                pixel_number++;
            }
        }




        public static unsafe void TransferPixels(Bitmap bitmap_dest, ref int pixel_number, TwainDotNet.TwainNative.ImageInfo imageInfo, TwainDotNet.TwainNative.ImageMemXfer memxfer_src) {
            BitmapInfoHeaderStruct bitmapInfo = new BitmapInfoHeaderStruct();
            bitmapInfo.Width = (int) memxfer_src.Columns;
            bitmapInfo.Height = - (int) memxfer_src.Rows;
            bitmapInfo.Size = sizeof(BitmapInfoHeaderStruct);
            bitmapInfo.BitCount = imageInfo.BitsPerPixel;    
            bitmapInfo.Planes = 1;
            bitmapInfo.SizeImage = 0;            


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
                        new IntPtr(&bitmapInfo), 
                        0);
                }
                finally {
                    graphics.ReleaseHdc(hdc);
                }
            }

        }


        // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183376(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct  BitmapInfoHeaderStruct
        {
            public int Size;
            public int Width;
            public int Height;
            public short Planes;
            public short BitCount;
            public int Compression;
            public int SizeImage;
            public int XPelsPerMeter;
            public int YPelsPerMeter;
            public int ClrUsed;
            public int ClrImportant;

            public override string ToString() {
                return string.Format(
                    "s:{0} w:{1} h:{2} p:{3} bc:{4} c:{5} si:{6} xpels:{7} ypels:{8} cu:{9} ci:{10}",
                    Size,
                    Width,
                    Height,
                    Planes,
                    BitCount,
                    Compression,
                    SizeImage,
                    XPelsPerMeter,
                    YPelsPerMeter,
                    ClrUsed,
                    ClrImportant);
            }
        }

    }
}

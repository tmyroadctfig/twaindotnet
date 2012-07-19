using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class ImageInfo
    {
        public int XResolution;
        public int YResolution;
        public int ImageWidth;
        public int ImageLength;
        public short SamplesPerPixel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public short[] BitsPerSample;
        public short BitsPerPixel;
        public short Planar;
        public short PixelType;
        public Compression Compression;
    }
}

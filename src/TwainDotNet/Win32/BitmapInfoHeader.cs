using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

namespace TwainDotNet.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class BitmapInfoHeader
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

        public override string ToString()
        {
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

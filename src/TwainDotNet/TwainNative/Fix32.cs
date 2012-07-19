using System;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class Fix32
    {
        public short Whole;

        public ushort Frac;

        public Fix32(float f)
        {
            // http://www.dosadi.com/forums/archive/index.php?t-2534.html
            var val = (int)(f * 65536.0F);
            this.Whole = Convert.ToInt16(val >> 16);    // most significant 16 bits
            this.Frac = Convert.ToUInt16(val & 0xFFFF); // least
        }        

        public float ToFloat()
        {
            var frac = Convert.ToSingle(this.Frac);
            return this.Whole + frac / 65536.0F;
        }        
    }
}
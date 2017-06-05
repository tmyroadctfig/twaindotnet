using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBQuad {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        byte rgbReserved;
        public RGBQuad(byte red, byte green, byte blue) {
            this.rgbBlue = blue;
            this.rgbGreen = green;
            this.rgbRed = red;
            this.rgbReserved = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class BitmapInfoHeaderIndexedColor
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

        // and now a 256 entry indexed color table. there are other ways to do this, but they require /unsafe
        #region color table
        RGBQuad bmiColor_0; RGBQuad bmiColor_1; RGBQuad bmiColor_2; RGBQuad bmiColor_3; RGBQuad bmiColor_4;
        RGBQuad bmiColor_5; RGBQuad bmiColor_6; RGBQuad bmiColor_7; RGBQuad bmiColor_8; RGBQuad bmiColor_9;
        RGBQuad bmiColor_10; RGBQuad bmiColor_11; RGBQuad bmiColor_12; RGBQuad bmiColor_13; RGBQuad bmiColor_14;
        RGBQuad bmiColor_15; RGBQuad bmiColor_16; RGBQuad bmiColor_17; RGBQuad bmiColor_18; RGBQuad bmiColor_19;
        RGBQuad bmiColor_20; RGBQuad bmiColor_21; RGBQuad bmiColor_22; RGBQuad bmiColor_23; RGBQuad bmiColor_24;
        RGBQuad bmiColor_25; RGBQuad bmiColor_26; RGBQuad bmiColor_27; RGBQuad bmiColor_28; RGBQuad bmiColor_29;
        RGBQuad bmiColor_30; RGBQuad bmiColor_31; RGBQuad bmiColor_32; RGBQuad bmiColor_33; RGBQuad bmiColor_34;
        RGBQuad bmiColor_35; RGBQuad bmiColor_36; RGBQuad bmiColor_37; RGBQuad bmiColor_38; RGBQuad bmiColor_39;
        RGBQuad bmiColor_40; RGBQuad bmiColor_41; RGBQuad bmiColor_42; RGBQuad bmiColor_43; RGBQuad bmiColor_44;
        RGBQuad bmiColor_45; RGBQuad bmiColor_46; RGBQuad bmiColor_47; RGBQuad bmiColor_48; RGBQuad bmiColor_49;
        RGBQuad bmiColor_50; RGBQuad bmiColor_51; RGBQuad bmiColor_52; RGBQuad bmiColor_53; RGBQuad bmiColor_54;
        RGBQuad bmiColor_55; RGBQuad bmiColor_56; RGBQuad bmiColor_57; RGBQuad bmiColor_58; RGBQuad bmiColor_59;
        RGBQuad bmiColor_60; RGBQuad bmiColor_61; RGBQuad bmiColor_62; RGBQuad bmiColor_63; RGBQuad bmiColor_64;
        RGBQuad bmiColor_65; RGBQuad bmiColor_66; RGBQuad bmiColor_67; RGBQuad bmiColor_68; RGBQuad bmiColor_69;
        RGBQuad bmiColor_70; RGBQuad bmiColor_71; RGBQuad bmiColor_72; RGBQuad bmiColor_73; RGBQuad bmiColor_74;
        RGBQuad bmiColor_75; RGBQuad bmiColor_76; RGBQuad bmiColor_77; RGBQuad bmiColor_78; RGBQuad bmiColor_79;
        RGBQuad bmiColor_80; RGBQuad bmiColor_81; RGBQuad bmiColor_82; RGBQuad bmiColor_83; RGBQuad bmiColor_84;
        RGBQuad bmiColor_85; RGBQuad bmiColor_86; RGBQuad bmiColor_87; RGBQuad bmiColor_88; RGBQuad bmiColor_89;
        RGBQuad bmiColor_90; RGBQuad bmiColor_91; RGBQuad bmiColor_92; RGBQuad bmiColor_93; RGBQuad bmiColor_94;
        RGBQuad bmiColor_95; RGBQuad bmiColor_96; RGBQuad bmiColor_97; RGBQuad bmiColor_98; RGBQuad bmiColor_99;
        RGBQuad bmiColor_100; RGBQuad bmiColor_101; RGBQuad bmiColor_102; RGBQuad bmiColor_103; RGBQuad bmiColor_104;
        RGBQuad bmiColor_105; RGBQuad bmiColor_106; RGBQuad bmiColor_107; RGBQuad bmiColor_108; RGBQuad bmiColor_109;
        RGBQuad bmiColor_110; RGBQuad bmiColor_111; RGBQuad bmiColor_112; RGBQuad bmiColor_113; RGBQuad bmiColor_114;
        RGBQuad bmiColor_115; RGBQuad bmiColor_116; RGBQuad bmiColor_117; RGBQuad bmiColor_118; RGBQuad bmiColor_119;
        RGBQuad bmiColor_120; RGBQuad bmiColor_121; RGBQuad bmiColor_122; RGBQuad bmiColor_123; RGBQuad bmiColor_124;
        RGBQuad bmiColor_125; RGBQuad bmiColor_126; RGBQuad bmiColor_127; RGBQuad bmiColor_128; RGBQuad bmiColor_129;
        RGBQuad bmiColor_130; RGBQuad bmiColor_131; RGBQuad bmiColor_132; RGBQuad bmiColor_133; RGBQuad bmiColor_134;
        RGBQuad bmiColor_135; RGBQuad bmiColor_136; RGBQuad bmiColor_137; RGBQuad bmiColor_138; RGBQuad bmiColor_139;
        RGBQuad bmiColor_140; RGBQuad bmiColor_141; RGBQuad bmiColor_142; RGBQuad bmiColor_143; RGBQuad bmiColor_144;
        RGBQuad bmiColor_145; RGBQuad bmiColor_146; RGBQuad bmiColor_147; RGBQuad bmiColor_148; RGBQuad bmiColor_149;
        RGBQuad bmiColor_150; RGBQuad bmiColor_151; RGBQuad bmiColor_152; RGBQuad bmiColor_153; RGBQuad bmiColor_154;
        RGBQuad bmiColor_155; RGBQuad bmiColor_156; RGBQuad bmiColor_157; RGBQuad bmiColor_158; RGBQuad bmiColor_159;
        RGBQuad bmiColor_160; RGBQuad bmiColor_161; RGBQuad bmiColor_162; RGBQuad bmiColor_163; RGBQuad bmiColor_164;
        RGBQuad bmiColor_165; RGBQuad bmiColor_166; RGBQuad bmiColor_167; RGBQuad bmiColor_168; RGBQuad bmiColor_169;
        RGBQuad bmiColor_170; RGBQuad bmiColor_171; RGBQuad bmiColor_172; RGBQuad bmiColor_173; RGBQuad bmiColor_174;
        RGBQuad bmiColor_175; RGBQuad bmiColor_176; RGBQuad bmiColor_177; RGBQuad bmiColor_178; RGBQuad bmiColor_179;
        RGBQuad bmiColor_180; RGBQuad bmiColor_181; RGBQuad bmiColor_182; RGBQuad bmiColor_183; RGBQuad bmiColor_184;
        RGBQuad bmiColor_185; RGBQuad bmiColor_186; RGBQuad bmiColor_187; RGBQuad bmiColor_188; RGBQuad bmiColor_189;
        RGBQuad bmiColor_190; RGBQuad bmiColor_191; RGBQuad bmiColor_192; RGBQuad bmiColor_193; RGBQuad bmiColor_194;
        RGBQuad bmiColor_195; RGBQuad bmiColor_196; RGBQuad bmiColor_197; RGBQuad bmiColor_198; RGBQuad bmiColor_199;
        RGBQuad bmiColor_200; RGBQuad bmiColor_201; RGBQuad bmiColor_202; RGBQuad bmiColor_203; RGBQuad bmiColor_204;
        RGBQuad bmiColor_205; RGBQuad bmiColor_206; RGBQuad bmiColor_207; RGBQuad bmiColor_208; RGBQuad bmiColor_209;
        RGBQuad bmiColor_210; RGBQuad bmiColor_211; RGBQuad bmiColor_212; RGBQuad bmiColor_213; RGBQuad bmiColor_214;
        RGBQuad bmiColor_215; RGBQuad bmiColor_216; RGBQuad bmiColor_217; RGBQuad bmiColor_218; RGBQuad bmiColor_219;
        RGBQuad bmiColor_220; RGBQuad bmiColor_221; RGBQuad bmiColor_222; RGBQuad bmiColor_223; RGBQuad bmiColor_224;
        RGBQuad bmiColor_225; RGBQuad bmiColor_226; RGBQuad bmiColor_227; RGBQuad bmiColor_228; RGBQuad bmiColor_229;
        RGBQuad bmiColor_230; RGBQuad bmiColor_231; RGBQuad bmiColor_232; RGBQuad bmiColor_233; RGBQuad bmiColor_234;
        RGBQuad bmiColor_235; RGBQuad bmiColor_236; RGBQuad bmiColor_237; RGBQuad bmiColor_238; RGBQuad bmiColor_239;
        RGBQuad bmiColor_240; RGBQuad bmiColor_241; RGBQuad bmiColor_242; RGBQuad bmiColor_243; RGBQuad bmiColor_244;
        RGBQuad bmiColor_245; RGBQuad bmiColor_246; RGBQuad bmiColor_247; RGBQuad bmiColor_248; RGBQuad bmiColor_249;
        RGBQuad bmiColor_250; RGBQuad bmiColor_251; RGBQuad bmiColor_252; RGBQuad bmiColor_253; RGBQuad bmiColor_254;
        RGBQuad bmiColor_255; 

        #endregion

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
       
        public static void setupGreyscaleIndices(ref BitmapInfoHeaderIndexedColor hdr) {
            // we use reflection to populate the greyscale table to avoid typing a giant hardcoded table
            Object ohdr = hdr;
            Type otype = hdr.GetType();
            for (int i=0; i<256; i++) {
                var field = otype.GetField(String.Format("bmiColor_{0}",i), BindingFlags.Instance | BindingFlags.NonPublic);                
                field.SetValue(ohdr,new RGBQuad((byte)i,(byte)i,(byte)i));
            }
            hdr = (BitmapInfoHeaderIndexedColor)ohdr;
        }
        public static void setupBWIndices(ref BitmapInfoHeaderIndexedColor hdr) {
            hdr.bmiColor_0 = new RGBQuad(0, 0, 0);        // black
            hdr.bmiColor_1 = new RGBQuad(255, 255, 255);  // white
        }
    }



}

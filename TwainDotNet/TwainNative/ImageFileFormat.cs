
namespace TwainDotNet.TwainNative 
{
    /// <summary>
    /// Twain spec ICAP_IMAGEFILEFORMAT values.
    /// </summary>
    public enum ImageFileFormat : ushort
    {
        Tiff = 0,       /* Tagged Image File Format     */
        Pict = 1,       /* Macintosh PICT               */
        Bmp = 2,        /* Windows Bitmap               */
        Xbm = 3,        /* X-Windows Bitmap             */
        Jpeg = 4,       /* JPEG File Interchange Format */
        Fpx = 5,        /* Flash Pix                    */
        TiffMulti = 6,  /* Multi-page tiff file         */
        Png = 7,        
        Spiff = 8, 
        Exif = 9, 
        Pdf = 10,       /* 1.91 NB: this is not PDF/A */
        Jp2 = 11,       /* 1.91 */
        Jpx = 13,       /* 1.91 */
        Dejavu = 14,    /* 1.91 */
        PdfA = 15,      /* 2.0 Adobe PDF/A, Version 1*/
        PdfA2 = 16      /* 2.1 Adobe PDF/A, Version 2*/
    }
}


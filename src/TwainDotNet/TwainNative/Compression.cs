
namespace TwainDotNet.TwainNative
{
    /// <summary>
    /// Twain spec ICAP_COMPRESSION values.
    /// </summary>
    public enum Compression : short
    {
        None = 0,
        PackBits = 1,
        Group31d = 2,    /* Follows CCITT spec (no End Of Line)          */
        Group31dEol = 3, /* Follows CCITT spec (has End Of Line)         */
        Group32d = 4,    /* Follows CCITT spec (use cap for K Factor)    */
        Group4 = 5,      /* Follows CCITT spec                           */
        Jpeg = 6,        /* Use capability for more info                 */
        Lzw = 7,         /* Must license from Unisys and IBM to use      */
        Jbig = 8,        /* For Bitonal images  -- Added 1.7 KHL         */
        Png = 9,         /* Added 1.8 */
        Rle4 = 10,       /* Added 1.8 */
        Rle8 = 11,       /* Added 1.8 */
        BitFields = 12   /* Added 1.8 */

    }
}

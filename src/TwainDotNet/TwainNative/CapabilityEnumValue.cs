using System;
using System.Collections.Generic;
using System.Text;

namespace TwainDotNet.TwainNative
{
    /// <summary>
    /// /* TWON_ENUMERATION. Container for a collection of values. */
    /// typedef struct {
    ///    TW_UINT16  ItemType;
    ///    TW_UINT32  NumItems;     /* How many items in ItemList                 */
    ///    TW_UINT32  CurrentIndex; /* Current value is in ItemList[CurrentIndex] */
    ///    TW_UINT32  DefaultIndex; /* Powerup value is in ItemList[DefaultIndex] */
    ///    TW_UINT8   ItemList[1];  /* Array of ItemType values starts here       */
    /// } TW_ENUMERATION, FAR * pTW_ENUMERATION;
    /// </summary>
    public class CapabilityEnumValue
    {
        public TwainType TwainType { get; set; }
        public int ItemCount { get; set; }

        public int CurrentIndex { get; set; }
        public int DefaultIndex { get; set; }

#pragma warning disable 169
        /// <summary>
        /// The start of the array values
        /// </summary>
        byte _valueStart;
#pragma warning restore 169
    }
}

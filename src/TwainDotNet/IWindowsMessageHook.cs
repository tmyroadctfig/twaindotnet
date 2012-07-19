using System;
using System.Collections.Generic;
using System.Text;

namespace TwainDotNet
{
    public interface IWindowsMessageHook
    {
        /// <summary>
        /// Gets or sets if the message filter is in use.
        /// </summary>
        bool UseFilter { get; set; }

        /// <summary>
        /// The delegate to call back then the filter is in place and a message arrives.
        /// </summary>
        FilterMessage FilterMessageCallback { get; set; }

        /// <summary>
        /// The handle to the window that is performing the scanning.
        /// </summary>
        IntPtr WindowHandle { get; }
    }

    public delegate IntPtr FilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);
}

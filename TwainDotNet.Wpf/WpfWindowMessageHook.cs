using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace TwainDotNet.Wpf
{
    /// <summary>
    /// A windows message hook for WPF applications.
    /// </summary>
    public class WpfWindowMessageHook : IWindowsMessageHook
    {
        HwndSource _source;
        WindowInteropHelper _interopHelper;
        bool _usingFilter;

        public WpfWindowMessageHook(Window window)
        {
            _source = (HwndSource)PresentationSource.FromDependencyObject(window);
            _interopHelper = new WindowInteropHelper(window);            
        }

        public IntPtr FilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (FilterMessageCallback != null)
            {
                return FilterMessageCallback(hwnd, msg, wParam, lParam, ref handled);
            }

            return IntPtr.Zero;
        }

        public bool UseFilter
        {
            get
            {
                return _usingFilter;
            }
            set 
            {
                if (!_usingFilter && value == true)
                {
                    _source.AddHook(FilterMessage);
                    _usingFilter = true;
                }

                if (_usingFilter && value == false)
                {
                    _source.RemoveHook(FilterMessage);
                    _usingFilter = false;
                }
            }
        }

        public FilterMessage FilterMessageCallback { get; set; }

        public IntPtr WindowHandle { get { return _interopHelper.Handle; } }
    }
}

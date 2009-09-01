using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using TwainDotNet.TwainNative;
using TwainDotNet.Win32;
using System.Drawing;

namespace TwainDotNet
{
    public class Twain
    {
        DataSourceManager _dataSourceManager;

        public Twain(IWindowsMessageHook messageHook)
        {
            ScanningComplete += delegate {};
            
            _dataSourceManager = new DataSourceManager(DataSourceManager.DefaultApplicationId, messageHook);
            _dataSourceManager.ScanningComplete += delegate
            {
                ScanningComplete(this, EventArgs.Empty);
            };
        }

        /// <summary>
        /// Notification that the scanning has completed.
        /// </summary>
        public event EventHandler ScanningComplete;

        /// <summary>
        /// The scanned in images.
        /// </summary>
        public IList<Image> Images { get { return _dataSourceManager.Images; } }

        /// <summary>
        /// Starts scanning.
        /// </summary>
        public void StartScanning(ScanSettings settings)
        {
            _dataSourceManager.StartScan(settings);
        }

        /// <summary>
        /// Shows a dialog prompting the use to select the source to scan from.
        /// </summary>
        public void SelectSource()
        {
            _dataSourceManager.SelectSource();            
        }
    }
}

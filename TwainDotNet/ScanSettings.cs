using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace TwainDotNet
{
    public class ScanSettings : INotifyPropertyChanged
    {
        public ScanSettings()
        {
            ShoulTransferAllPages = true;
        }

        bool _showTwainUI;

        /// <summary>
        /// Indicates if the TWAIN/driver user interface should be used to pick the scan settings.
        /// </summary>
        public bool ShowTwainUI
        {
            get { return _showTwainUI; }
            set
            {
                if (value != _showTwainUI)
                {
                    _showTwainUI = value;
                    OnPropertyChanged("ShowTwainUI");
                }
            }
        }

        bool _useDocumentFeeder;

        /// <summary>
        /// Indicates if the automatic document feeder (ADF) should be the source of the document(s) to scan.
        /// </summary>
        public bool UseDocumentFeeder
        {
            get { return _useDocumentFeeder; }
            set
            {
                if (value != _useDocumentFeeder)
                {
                    _useDocumentFeeder = value;
                    OnPropertyChanged("UseDocumentFeeder");
                }
            }
        }

        short _transferCount;

        /// <summary>
        /// The number of pages to transfer.
        /// </summary>
        public short TransferCount
        {
            get { return _transferCount; }
            set
            {
                if (value != _transferCount)
                {
                    _transferCount = value;
                    OnPropertyChanged("TransferCount");
                    OnPropertyChanged("ShoulTransferAllPages");
                }
            }
        }

        /// <summary>
        /// Indicates if all pages should be transferred.
        /// </summary>
        public bool ShoulTransferAllPages
        {
            get { return _transferCount == TransferAllPages; }
            set { TransferCount = value ? TransferAllPages : (short) 1; }
        }

        ResolutionSettings _resolution;

        /// <summary>
        /// The resolution settings. Set null to use current defaults.
        /// </summary>
        public ResolutionSettings Resolution
        {
            get { return _resolution; }
            set
            {
                if (value != _resolution)
                {
                    _resolution = value;
                    OnPropertyChanged("Resolution");
                }
            }
        }

        bool _useDuplex;

        /// <summary>
        /// Whether to use duplexing, if supported.
        /// </summary>
        public bool UseDuplex
        {
            get { return _useDuplex; }
            set
            {
                if (value != _useDuplex)
                {
                    _useDuplex = value;
                    OnPropertyChanged("UseDuplex");
                }
            }
        }

        #region INotifyPropertyChanged Members

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        /// <summary>
        /// Default scan settings.
        /// </summary>
        public static readonly ScanSettings Default = new ScanSettings()
        {
            Resolution = ResolutionSettings.ColourPhotocopier
        };

        /// <summary>
        /// The value to set to scan all available pages.
        /// </summary>
        public const short TransferAllPages = -1;
    }
}

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
            ShouldTransferAllPages = true;
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

        bool? _showProgressIndicatorUI;

        /// <summary>
        /// Gets or sets a value indicating whether [show progress indicator ui].
        /// If TRUE, the Source will display a progress indicator during acquisition and transfer, regardless of whether the Source's user interface is active. 
        /// If FALSE, the progress indicator will be suppressed if the Source's user interface is inactive.
        /// The Source will continue to display device-specific instructions and error messages even with the Source user interface and progress indicators turned off. 
        /// </summary>
        /// <value><c>true</c> if [show progress indicator ui]; otherwise, <c>false</c>.</value>
        public bool? ShowProgressIndicatorUI
        {
            get { return _showProgressIndicatorUI; }
            set
            {
                if (value != _showProgressIndicatorUI)
                {
                    _showProgressIndicatorUI = value;
                    OnPropertyChanged("ShowProgressIndicatorUI");
                }
            }
        }

        bool? _useDocumentFeeder;

        /// <summary>
        /// Indicates if the automatic document feeder (ADF) should be the source of the document(s) to scan.
        /// </summary>
        public bool? UseDocumentFeeder
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

        bool? _useAutoFeeder;

        /// <summary>
        /// Indicates if the automatic document feeder (ADF) should continue feeding document(s) to scan after the negotiated number of pages are acquired.
        /// UseDocumentFeeder must be true
        /// </summary>
        public bool? UseAutoFeeder
        {
            get { return _useAutoFeeder; }
            set
            {
                if (value != _useAutoFeeder)
                {
                    _useAutoFeeder = value;
                    OnPropertyChanged("UseAutoFeeder");
                }
            }
        }

        bool? _useAutoScanCache;

        /// <summary>
        /// Indicates if the source should continue scanning without waiting for the application to request the image transfers.
        /// </summary>
        public bool? UseAutoScanCache
        {
            get { return _useAutoScanCache; }
            set
            {
                if (value != _useAutoScanCache)
                {
                    _useAutoScanCache = value;
                    OnPropertyChanged("UseAutoScanCache");
                }
            }
        }

        bool _abortWhenNoPaperDetectable;

        /// <summary>
        /// Indicates if the transfer should not start when no paper was detected (e.g. by the ADF).
        /// </summary>
        public bool AbortWhenNoPaperDetectable
        {
            get { return _abortWhenNoPaperDetectable; }
            set
            {
                if (value != _abortWhenNoPaperDetectable)
                {
                    _abortWhenNoPaperDetectable = value;
                    OnPropertyChanged("AbortWhenNoPaperDetectable");
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
                    OnPropertyChanged("ShouldTransferAllPages");
                }
            }
        }

        /// <summary>
        /// Indicates if all pages should be transferred.
        /// </summary>
        public bool ShouldTransferAllPages
        {
            get { return _transferCount == TransferAllPages; }
            set { TransferCount = value ? TransferAllPages : (short)1; }
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

        bool? _useDuplex;

        /// <summary>
        /// Whether to use duplexing, if supported.
        /// </summary>
        public bool? UseDuplex
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

        AreaSettings _area;

        public AreaSettings Area
        {
            get { return _area; }
            set
            {
                if (value != _area)
                {
                    _area = value;
                    OnPropertyChanged("Area");
                }
            }
        }

        PageSettings _page;

        /// <summary>
        /// The page / paper settings. Set null to use current defaults.
        /// </summary>
        /// <value>The page.</value>
        public PageSettings Page
        {
            get { return _page; }
            set
            {
                if (value != _page)
                {
                    _page = value;
                    OnPropertyChanged("Page");
                }
            }
        }

        RotationSettings _rotation;

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        public RotationSettings Rotation
        {
            get { return _rotation; }
            set
            {
                if (value != _rotation)
                {
                    _rotation = value;
                    OnPropertyChanged("Rotation");
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
            Resolution = ResolutionSettings.ColourPhotocopier,
            Page = PageSettings.Default,
            Rotation = new RotationSettings()
        };

        /// <summary>
        /// The value to set to scan all available pages.
        /// </summary>
        public const short TransferAllPages = -1;
    }
}

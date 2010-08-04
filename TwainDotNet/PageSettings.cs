using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    /// <summary>
    /// Page settings used for automatic document feeders
    /// scanning.
    /// </summary>
    public class PageSettings : INotifyPropertyChanged
    {
        Orientation _orientation;

        /// <summary>
        /// Gets or sets the page orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                if (value != _orientation)
                {
                    _orientation = value;
                    OnPropertyChanged("Orientation");
                }
            }
        }

        PageType _size;
        /// <summary>
        /// Gets or sets the Page Size.
        /// </summary>
        /// <value>The size.</value>
        public PageType Size
        {
            get { return _size; }
            set
            {
                if (value != _size)
                {
                    _size = value;
                    OnPropertyChanged("PaperSize");
                }
            }
        }

        public PageSettings()
        {
            Size = PageType.UsLetter;
            Orientation = Orientation.Default;
        }

        /// <summary>
        /// Default Page setup - A4 Letter and Portrait orientation
        /// </summary>
        public static readonly PageSettings Default = new PageSettings()
        {
            Size = PageType.UsLetter,
            Orientation = Orientation.Default
        };

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

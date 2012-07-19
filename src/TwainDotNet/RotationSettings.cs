using System.ComponentModel;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    /// <summary>
    /// Settings for hardware image rotation.  Includes
    /// hardware deskewing detection
    /// </summary>
    public class RotationSettings : INotifyPropertyChanged
    {
        private bool automaticDeskew;
        private bool automaticBorderDetection;
        private bool automaticRotate;
        private FlipRotation flipSideRotation;

        /// <summary>
        /// Gets or sets a value indicating whether [automatic deskew].
        /// </summary>
        /// <value><c>true</c> if [automatic deskew]; otherwise, <c>false</c>.</value>
        public bool AutomaticDeskew
        {
            get
            {
                return automaticDeskew;
            }
            set
            {
                if (value != automaticDeskew)
                {
                    automaticDeskew = value;
                    OnPropertyChanged("AutomaticDeskew");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic border detection].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [automatic border detection]; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticBorderDetection
        {
            get
            {
                return automaticBorderDetection;
            }
            set
            {
                if (value != automaticBorderDetection)
                {
                    automaticBorderDetection = value;
                    OnPropertyChanged("AutomaticBorderDetection");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic rotate].
        /// </summary>
        /// <value><c>true</c> if [automatic rotate]; otherwise, <c>false</c>.</value>
        public bool AutomaticRotate
        {
            get
            {
                return automaticRotate;
            }
            set
            {
                if (value != automaticRotate)
                {
                    automaticRotate = value;
                    OnPropertyChanged("AutomaticRotate");
                }
            }
        }

        /// <summary>
        /// Gets or sets the flip side rotation.
        /// </summary>
        /// <value>The flip side rotation.</value>
        public FlipRotation FlipSideRotation
        {
            get
            {
                return flipSideRotation;
            }
            set
            {
                if (value != flipSideRotation)
                {
                    flipSideRotation = value;
                    OnPropertyChanged("FlipSideRotation");
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
    }
}

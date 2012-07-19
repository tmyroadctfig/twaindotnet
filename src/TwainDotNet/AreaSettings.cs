using System.ComponentModel;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    public class AreaSettings : INotifyPropertyChanged
    {
        private Units _units;
        public Units Units
        {
            get { return _units; }
            set
            {
                _units = value;
                OnPropertyChanged("Units");
            }
        }

        private float _top;
        public float Top
        {
            get { return _top; }
            private set
            {
                _top = value;
                OnPropertyChanged("Top");
            }
        }

        private float _left;
        public float Left
        {
            get { return _left; }
            private set
            {
                _left = value;
                OnPropertyChanged("Left");
            }
        }

        private float _bottom;
        public float Bottom 
        {
            get { return _bottom; }
            private set
            {
                _bottom = value;
                OnPropertyChanged("Bottom");
            }
        }

        private float _right;
        public float Right
        {
            get { return _right; }
            private set
            {
                _right = value;
                OnPropertyChanged("Right");
            }
        }

        public AreaSettings(Units units, float top, float left, float bottom, float right)
        {
            _units = units;
            _top = top;
            _left = left;
            _bottom = bottom;
            _right = right;
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
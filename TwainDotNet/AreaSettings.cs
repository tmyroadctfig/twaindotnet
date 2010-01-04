namespace TwainDotNet
{
    using System.ComponentModel;
    using TwainNative;

    public class AreaSettings : INotifyPropertyChanged
    {
        private Units units;
        public Units Units
        {
            get { return this.units; }
            set
            {
                this.units = value;
                OnPropertyChanged("Units");
            }
        }

        private float _top;
        public float Top
        {
            get { return this._top; }
            private set
            {
                this._top = value;
                OnPropertyChanged("Top");
            }
        }

        private float _left;
        public float Left
        {
            get { return this._left; }
            private set
            {
                this._left = value;
                OnPropertyChanged("Left");
            }
        }

        private float _bottom;
        public float Bottom 
        {
            get { return this._bottom; }
            private set
            {
                this._bottom = value;
                OnPropertyChanged("Bottom");
            }
        }

        private float _right;
        public float Right
        {
            get { return this._right; }
            private set
            {
                this._right = value;
                OnPropertyChanged("Right");
            }
        }

        public AreaSettings(Units units, float top, float left, float bottom, float right)
        {
            this.Units = units;
            this._top = top;
            this._left = left;
            this._bottom = bottom;
            this._right = right;
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
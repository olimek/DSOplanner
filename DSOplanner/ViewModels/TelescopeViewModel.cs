using System.ComponentModel;

namespace DSOplanner.ViewModels
{
    public class TelescopeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _aperture;
        private double _focalLength;

        public double Aperture
        {
            get => _aperture;
            set
            {
                if (_aperture != value)
                {
                    _aperture = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Aperture)));
                }
            }
        }

        public double FocalLength
        {
            get => _focalLength;
            set
            {
                if (_focalLength != value)
                {
                    _focalLength = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FocalLength)));
                }
            }
        }
    }
}

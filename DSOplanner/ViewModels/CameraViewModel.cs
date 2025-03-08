using System.ComponentModel;

namespace DSOplanner.ViewModels
{
    public class CameraViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _pixelSize;
        private double _quantumEfficiency;
        private double _readNoise;

        public double PixelSize
        {
            get => _pixelSize;
            set
            {
                if (_pixelSize != value)
                {
                    _pixelSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PixelSize)));
                }
            }
        }

        public double QuantumEfficiency
        {
            get => _quantumEfficiency;
            set
            {
                if (_quantumEfficiency != value)
                {
                    _quantumEfficiency = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuantumEfficiency)));
                }
            }
        }

        public double ReadNoise
        {
            get => _readNoise;
            set
            {
                if (_readNoise != value)
                {
                    _readNoise = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReadNoise)));
                }
            }
        }
    }
}

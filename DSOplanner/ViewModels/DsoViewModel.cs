using System;
using System.ComponentModel;

namespace DSOplanner.ViewModels
{
    public class DsoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _excluded;
        private string _name;
        private string _description;
        private string _type;
        private double _rightAscension;
        private double _declination;
        private double _magnitude;
        private double _sizeArcmin;
        private string _spectralType;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public double RightAscension
        {
            get => _rightAscension;
            set
            {
                if (_rightAscension != value)
                {
                    _rightAscension = value;
                    OnPropertyChanged(nameof(RightAscension));
                    OnPropertyChanged(nameof(RightAscensionFormatted));
                }
            }
        }

        public double Declination
        {
            get => _declination;
            set
            {
                if (_declination != value)
                {
                    _declination = value;
                    OnPropertyChanged(nameof(Declination));
                    OnPropertyChanged(nameof(DeclinationFormatted));
                }
            }
        }

        public double Magnitude
        {
            get => _magnitude;
            set
            {
                if (_magnitude != value)
                {
                    _magnitude = value;
                    OnPropertyChanged(nameof(Magnitude));
                }
            }
        }

        public double SizeArcmin
        {
            get => _sizeArcmin;
            set
            {
                if (_sizeArcmin != value)
                {
                    _sizeArcmin = value;
                    OnPropertyChanged(nameof(SizeArcmin));
                }
            }
        }

        public string SpectralType
        {
            get => _spectralType;
            set
            {
                if (_spectralType != value)
                {
                    _spectralType = value;
                    OnPropertyChanged(nameof(SpectralType));
                }
            }
        }

        public bool Excluded
        {
            get => _excluded;
            set
            {
                if (_excluded != value)
                {
                    _excluded = value;
                    OnPropertyChanged(nameof(Excluded));
                }
            }
        }

        // Nowe właściwości formatujące wartości dla wyświetlania w UI
        public string RightAscensionFormatted => $"{RightAscension:F2} h";
        public string DeclinationFormatted => $"{Declination:+0.00;-0.00}°";

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

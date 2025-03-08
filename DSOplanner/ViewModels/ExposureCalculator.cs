using System;

namespace DSOplanner.ViewModels
{
    public static class ExposureCalculator
    {
        // Stałe fizyczne
        private const double h = 6.626e-34; // Stała Plancka [J·s]
        private const double c = 3.0e8; // Prędkość światła [m/s]
        private const double lambdaEff = 550e-9; // Średnia długość fali dla pasma V (m)

        // Zerowy punkt strumienia w fotonach/m²/s/arcsec²
        private const double zeroPointFlux = 3.631e-20; // W/m²/Hz (zerowy punkt dla mag=0)
        private static readonly double zero_point_flux_photons = zeroPointFlux * lambdaEff / (h * c) * 1e4;

        public static double CalculateExposure(DsoViewModel dso, TelescopeViewModel telescope, CameraViewModel camera, double skyBrightness)
        {
            // Powierzchnia zbierająca teleskopu [m²]
            double collectingArea = Math.PI * Math.Pow((telescope.Aperture / 2) / 1000, 2);

            // Strumień fotonów z obiektu (mag/arcsec²)
            double fluxPhotons = zero_point_flux_photons * Math.Pow(10, -0.4 * dso.Magnitude);

            // Przeliczenie SQM (skyBrightness) na fotony/m²/s/arcsec²
            double skyFluxPhotons = zero_point_flux_photons * Math.Pow(10, -0.4 * skyBrightness);

            // Rozmiar piksela w metrach
            double pixelSizeM = camera.PixelSize * 1e-6;

            // Skala obrazu w [arcsec/piksel]
            double scaleArcsecPerPixel = (206265 * pixelSizeM) / (telescope.FocalLength * 1e-3);

            // Powierzchnia piksela w [arcsec²]
            double pixelAreaArcsec2 = Math.Pow(scaleArcsecPerPixel, 2);

            // Początkowa wartość czasu ekspozycji
            double exposureTime = 5;
            double maxExposure = 300; // Limit do 5 minut

            while (exposureTime <= maxExposure)
            {
                // Obliczenie sygnału (liczba elektronów na piksel w danym czasie)
                double signal = fluxPhotons * exposureTime * camera.QuantumEfficiency;

                // Szum tła nieba
                double skySignal = skyFluxPhotons * exposureTime * camera.QuantumEfficiency;

                // Całkowity szum (szum obiektu + szum tła + szum odczytu)
                double noise = Math.Sqrt(signal + skySignal + Math.Pow(camera.ReadNoise, 2));

                // Stosunek sygnału do szumu (SNR)
                double snr = signal / noise;

                // Konwersja SNR do dB
                double snr_dB = 10 * Math.Log10(snr);

                // Jeśli osiągnięty SNR > 5 (czyli > 7 dB), zwracamy czas ekspozycji
                if (snr_dB >= 20.0)
                    return exposureTime;

                exposureTime += 5; // Zwiększanie kroku symulacji
            }

            return exposureTime; // Zwraca maksymalny czas, jeśli warunek SNR nie został spełniony
        }
    }
}

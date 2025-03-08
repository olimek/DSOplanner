using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DSOplanner.ViewModels
{
    public static class DsoCsvLoader
    {
        private static readonly Regex RaRegex = new(@"(\d+)h\s*(\d+)'?\s*(\d+)?", RegexOptions.Compiled);
        private static readonly Regex DecRegex = new(@"([-+]?\d+)º?\s*(\d+)'?\s*(\d+)?", RegexOptions.Compiled);
        private static StreamReader _reader;
        private const int PageSize = 200; // Increase batch size for better performance

        /// <summary>
        /// Initialize the CSV reader.
        /// </summary>
        public static async Task InitializeReader(string fileName)
        {
            if (_reader == null)
            {
                if (!await FileSystem.AppPackageFileExistsAsync(fileName)) return;

                Stream stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                _reader = new StreamReader(stream);
                await _reader.ReadLineAsync(); // Skip header
            }
        }

        /// <summary>
        /// Read the next batch of objects asynchronously.
        /// </summary>
        public static async Task<List<DsoViewModel>> GetNextBatch()
        {
            var dsoList = new List<DsoViewModel>(PageSize);

            if (_reader == null) return dsoList;

            string[] lines = new string[PageSize];
            for (int i = 0; i < PageSize; i++)
            {
                if (_reader.EndOfStream) break;
                lines[i] = await _reader.ReadLineAsync();
            }

            // Process data on a background thread
            return await Task.Run(() =>
            {
                var batch = new List<DsoViewModel>();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var dso = ParseDso(line);
                    if (dso != null) batch.Add(dso);
                }

                return batch;
            });
        }

        /// <summary>
        /// Parse a CSV line into a DSO object.
        /// </summary>
        private static DsoViewModel ParseDso(string line)
        {
            var values = line.Split(';');
            if (values.Length < 10) return null;

            try
            {
                return new DsoViewModel
                {
                    Name = values[0].Trim(),
                    Type = values[3].Trim(),
                    RightAscension = ParseRa(values[5].Trim()),
                    Declination = ParseDec(values[6].Trim()),
                    Magnitude = TryParseDouble(values[7], 99)
                };
            }
            catch
            {
                return null;
            }
        }

        private static double ParseRa(string raString)
        {
            var match = RaRegex.Match(raString);
            if (!match.Success) return 0;

            int hours = int.Parse(match.Groups[1].Value);
            int minutes = int.Parse(match.Groups[2].Value);
            int seconds = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;

            return hours + (minutes / 60.0) + (seconds / 3600.0);
        }

        private static double ParseDec(string decString)
        {
            var match = DecRegex.Match(decString);
            if (!match.Success) return 0;

            int degrees = int.Parse(match.Groups[1].Value);
            int minutes = int.Parse(match.Groups[2].Value);
            int seconds = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;

            double decimalDegrees = Math.Abs(degrees) + (minutes / 60.0) + (seconds / 3600.0);
            return degrees < 0 ? -decimalDegrees : decimalDegrees;
        }

        private static double TryParseDouble(string value, double defaultValue)
        {
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : defaultValue;
        }
    }
}

using System.Globalization;

namespace StepCounter.Converters
{
    public class DistanceConverter : IValueConverter
    {
        private const double StepLengthMeters = 0.78;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int steps)
            {
                double km = (steps * StepLengthMeters) / 1000.0;
                return $"{km:F2} km";
            }
            return "0 km";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

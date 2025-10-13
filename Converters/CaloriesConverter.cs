using System.Globalization;

namespace StepCounter.Converters
{
    public class CaloriesConverter : IValueConverter
    {
        private const double CaloriesPerStep = 0.04;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int steps)
            {
                double kcal = steps * CaloriesPerStep;
                return $"{kcal:F0} kcal";
            }
            return "0 kcal";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

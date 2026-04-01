using System.Globalization;

namespace ReadMe_perso.Converters
{
    public class ProgressConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int currentPage && currentPage > 0)
            {
                // Return a value between 0 and 1 for ProgressBar
                // Assuming max page is around 500-1000, we'll use a percentage
                return Math.Min(1.0, currentPage / 500.0);
            }

            return 0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

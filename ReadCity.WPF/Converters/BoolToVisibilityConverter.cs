using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReadCity.WPF.Converters
{
    /// <summary>
    /// Converts a <see cref="bool"/> to <see cref="Visibility"/>.
    /// True → Visible, False → Collapsed.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return value is true ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return value is Visibility.Visible;
        }
    }
}

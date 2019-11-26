using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sample
{
    public class TimeFocConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return string.Empty;
            }

            return $" ({value.ToString()})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
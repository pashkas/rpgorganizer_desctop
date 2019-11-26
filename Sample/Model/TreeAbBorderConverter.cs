using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Sample.Model
{
    public class TreeAbBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return Brushes.Gray;
            }

            var ab = value as AbilitiModel;
            if (ab!=null)
            {
                if (ab.IsEnebledProperty)
                {
                    return Brushes.Green;
                }
                if (ab.IsBuyVisibility == Visibility.Visible)
                {
                    return Brushes.GreenYellow;
                }
                return Brushes.Gray;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
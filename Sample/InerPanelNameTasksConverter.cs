using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Sample
{
    public class InerPanelNameTasksConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values == DependencyProperty.UnsetValue)
            {
                return "";
            }

            var count = (int)values[1];
            var val = (string)values[0];

            if (count > 36)
            {
                var plus = val.Split().Count() > 1 ? "..." : string.Empty;
                return string.Join(" ", val.Split().Take(1)) + plus;
            }
            if (count > 25)
            {
                var plus = val.Split().Count() > 2 ? "..." : string.Empty;
                return string.Join(" ", val.Split().Take(2)) + plus;
            }
            if (count > 16)
            {
                var plus = val.Split().Count() > 3 ? "..." : string.Empty;
                return string.Join(" ", val.Split().Take(3)) + plus;
            }

            return val;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
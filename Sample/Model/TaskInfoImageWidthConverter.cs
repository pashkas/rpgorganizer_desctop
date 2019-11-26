using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Ширина картинки информации о задаче
    /// </summary>
    public class TaskInfoImageWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return 0.0;
            }
            else
            {
                string width1 = System.Convert.ToString(Application.Current.FindResource("ToolInfoWidth"));
                width1 = width1.Remove(width1.Length - 1);
                double width = System.Convert.ToDouble(value);
                var delay = System.Convert.ToDouble(width1) + 1.0;
                return width / delay;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
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
    /// Видимость разных способов отображения задач
    /// </summary>
    public class TasksViewVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || parameter == null)
            {
                return Visibility.Collapsed;
            }

            string param = parameter.ToString();
            int val = System.Convert.ToInt32(value);

            if (param == "1_2")
            {
                if (val == 1 || val == 2)
                {
                    return Visibility.Visible;
                }
            }

            if (param == "1")
            {
                if (val == 1)
                {
                    return Visibility.Visible;
                }
            }

            if (param == "2")
            {
                if (val == 2)
                {
                    return Visibility.Visible;
                }
            }

            if (param == "3" && val == 3)
            {
                return Visibility.Visible;
            }

            if (param == "4" && val == 4)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
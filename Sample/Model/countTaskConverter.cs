using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class countTaskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return Brushes.White;
            }

            int val = System.Convert.ToInt32(value);

            int piv = StaticMetods.Config.AbOneLevelDays;

            if (val == piv)
            {
                if (parameter.ToString() == "привычка")
                {
                    return Brushes.Yellow;
                }
            }
            else if (val == StaticMetods.PersProperty.PersSettings.TaskCountForEducationProperty)
            {
                if (parameter.ToString() == "изучение")
                {
                    return Brushes.Yellow;
                }
            }
            else if (val == 0)
            {
                if (parameter.ToString() == "нет")
                {
                    return Brushes.Yellow;
                }
            }
            else if (val == 1)
            {
                if (parameter.ToString() == "дело")
                {
                    return Brushes.Yellow;
                }
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
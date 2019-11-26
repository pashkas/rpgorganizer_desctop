using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Значение в зависимости от коэффициента
    /// </summary>
    public class KValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = System.Convert.ToInt32(value);

            switch (val)
            {
                case 0:
                    return "Нет!";
                case 3:
                    return "Слабо";
                case 5:
                    return "Норм";
                case 7:
                    return "Сильно";
               case 10:
                    return "Супер!";
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
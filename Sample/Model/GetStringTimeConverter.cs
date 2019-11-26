using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Sample.Model
{
    public class GetStringTimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
                return "";
            var val = (Task) value;

            var time = GetTimeTask(val);

            return time;
        }

        public static string GetTimeTask(Task val)
        {
            
            if (val.TimeProperty.Hour == 11 && val.TimeProperty.Minute == 59 && val.TimeProperty.Second == 1)
            {
                return "утро";
            }
            else if (val.TimeProperty.Hour == 17 && val.TimeProperty.Minute == 59 && val.TimeProperty.Second == 1)
            {
                return "день";
            }
            else if (val.TimeProperty.Hour == 23 && val.TimeProperty.Minute == 58 && val.TimeProperty.Second == 1)
            {
                return "вечер";
            }
            else if (val.TimeProperty.Hour == 23 && val.TimeProperty.Minute == 59)
            {
                return "";
            }
            var time = $"{val.TimeProperty.ToString("t")}";

            //if (val.TimeMustProperty != 0)
            //{
            //    time = time + $"-{val.TimeProperty.AddMinutes(val.TimeMustProperty).ToString("t")}";
            //}
            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

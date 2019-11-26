using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Sample.ViewModel;

namespace Sample.Model
{
    public class AbTaskConv: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return "";
            }
            var isCurLev = (bool) values[0];
            var DateTim = (DateTime)values[1];
            var Tim = values[2].ToString();
            var rec = values[3].ToString();

            string txt = "";

            if (isCurLev)
            {
                if (isCurLev)
                {
                    string dat = DateTim.Date.ToShortDateString()+" | ";
                    if (DateTim.Date == MainViewModel.selectedTime.Date)
                    {
                        dat = "сегодня | ";
                    }
                    else if(DateTim.Date == MainViewModel.selectedTime.Date.AddDays(-1))
                    {
                        dat = "вчера | ";
                    }
                    else if (DateTim.Date == MainViewModel.selectedTime.Date.AddDays(1))
                    {
                        dat = "завтра | ";
                    }

                    txt = dat;
                }
            }
            if (!string.IsNullOrWhiteSpace(Tim))
            {
                txt += $"{Tim} | ";
            }
            txt += $"{rec}";
            return txt;


        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

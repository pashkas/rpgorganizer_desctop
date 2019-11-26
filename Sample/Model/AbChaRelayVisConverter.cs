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
    public class AbChaRelayVisConverter:IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
            {
                return false;
            }


            var charact = (Characteristic) values[1];
            var ab = (AbilitiModel) values[2];
            var val = ((NeedK) values[0]).KProperty;

            var any = charact.NeedAbilitisProperty.Any(n => n.AbilProperty!=ab && n.KoeficientProperty>=10);
            if (any && val == 10)
            {
                return false;
            }
            else if (val == 6 && charact.NeedAbilitisProperty.Count(n => n.AbilProperty != ab && n.KoeficientProperty == 6)>=2)
            {
                return false;
            }
            else
            {
                return true;
            }

            
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

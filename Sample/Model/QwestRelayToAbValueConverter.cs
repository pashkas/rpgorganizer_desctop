using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sample.Model
{
    public class QwestRelayToAbValueConverter:IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ab = values[0] as AbilitiModel;
            var qw = values[1] as Aim;

            if (ab!=null && qw!=null)
            {
                var firstOrDefault = ab.NeedAims.FirstOrDefault(n=>n.AimProperty==qw);
                if (firstOrDefault!=null)
                {
                    return $"+{firstOrDefault.KRel}";
                }
            }


            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

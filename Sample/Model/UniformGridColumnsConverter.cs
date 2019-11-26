using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class UniformGridColumnsConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return 1;
            }

            double count;

            try
            {
                count = System.Convert.ToDouble(value);
            }
            catch (Exception)
            {
                var list = value as IEnumerable;
                if (list != null)
                {
                    count = list.Cast<object>().Count();
                }
                else
                {
                    count = 1;
                }
            }

            double sqrt = 1;

            if (parameter == null)
            {
                if (Math.Abs(count) < 0.01)
                {
                    count = 1;
                }
                sqrt = Math.Sqrt(count);
            }
            else if(parameter.ToString() == "changes")
            {
                count = (int)value;
                if (count > 16)
                {
                    return 3;
                }
                if (count > 8)
                {
                    return 2;
                }
                return 1;
            }
            else if (parameter.ToString() == "ПанельЗадач")
            {
                count = (int)value;
                //if (count > 4)
                //{
                //    sqrt = 4;
                //}
                //else
                //{
                //    sqrt = count;
                //}

                //////!!!   sqrt = Math.Sqrt(count);

                //int mc = 14;
                if (count>20)
                {
                    sqrt = 3;
                }
                else if (count>10)
                {
                    sqrt = 2;
                }
                else
                {
                    sqrt = 1;
                }



                //sqrt = Math.Sqrt(count);
                //sqrt = count<=3 ? count : 3;
            }

            return System.Convert.ToInt32(Math.Ceiling(sqrt));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
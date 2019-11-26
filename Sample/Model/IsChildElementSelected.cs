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
    public class IsChildElementSelected : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var task = values[0] as Task;
            var lastParTask = values[1] as Task;
            Visibility isFocViz = (Visibility)values[2];
            bool isPlanMode = (bool)values[3];

            if (isPlanMode)
            {
                return Visibility.Collapsed;
            }

            if (!StaticMetods.PersProperty.PersSettings.IsHideNotSellectedLink)
            {
                if (task?.Recurrense.TypeInterval == TimeIntervals.Нет)
                {
                    return Visibility.Collapsed;
                }

                if (!task?.LinkedTasks?.Any() == true)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
            else
            {
                if (lastParTask != task)
                {
                    return Visibility.Collapsed;
                }

                if (task != null && task.LinkedTasks.Any())
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }

            //if (lastParTask != task)
            //{
            //    return Visibility.Collapsed;
            //}

            //if (task != null && task.LinkedTasks.Any())
            //{
            //    return Visibility.Visible;
            //}

            //return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

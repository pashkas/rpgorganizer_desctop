using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Sample.Model;
using Sample.ViewModel;

namespace Sample
{
    public class TimeBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return Brushes.White;
            DateTime tsk = ((DateTime) value);
            string par = string.Empty;
            if (parameter != null)
            {
                par = parameter.ToString();
            }
            if (MainViewModel.IsMorning().Invoke(tsk) && par=="утро")
            {
                return Brushes.Yellow;
            }
            if (MainViewModel.IsDay().Invoke(tsk) && par == "день")
            {
                return Brushes.Yellow;
            }
            if (MainViewModel.IsEvening().Invoke(tsk) && par == "вечер")
            {
                return Brushes.Yellow;
            }
            if (par == "нет" && (!MainViewModel.IsMorning().Invoke(tsk)&& !MainViewModel.IsDay().Invoke(tsk)&& !MainViewModel.IsEvening().Invoke(tsk)))
            {
                return Brushes.Yellow;
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
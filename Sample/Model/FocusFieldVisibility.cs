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
    /// Видимость зоны фокусировки
    /// </summary>
    public class FocusFieldVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (StaticMetods.PersProperty == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                if (StaticMetods.PersProperty.PersSettings.IsFourViewEnabledProperty == true)
                {
                    return Visibility.Collapsed;
                }

                if (StaticMetods.PersProperty.PersSettings.HideFocusFieldProperty == true)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
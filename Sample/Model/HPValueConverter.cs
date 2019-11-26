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

    public class HPValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return 0;
            }

            var hpProperty = StaticMetods.PersProperty.HPProperty;

            var showDamageNotHpProperty = StaticMetods.PersProperty.PersSettings.ShowDamageNotHPProperty;
            var maxHpProperty = hpProperty.MaxHPProperty;
            var currentHpProperty = hpProperty.CurrentHPProperty;

            switch (parameter.ToString())
            {
                case "значение":
                    if (showDamageNotHpProperty == true)
                    {
                        return maxHpProperty - currentHpProperty;
                    }
                    else
                    {
                        return currentHpProperty;
                    }
                case "текст":
                    if (showDamageNotHpProperty == true)
                    {
                        return string.Format(
                            "Урон: {0}/{1}",
                            (maxHpProperty - currentHpProperty).ToString(),
                            maxHpProperty.ToString());
                    }
                    else
                    {
                        return string.Format("HP: {0}/{1}", currentHpProperty, maxHpProperty);
                    }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
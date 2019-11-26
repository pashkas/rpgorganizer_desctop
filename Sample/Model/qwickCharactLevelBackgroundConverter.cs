// --------------------------------------------------------------------------------------------------------------------
// <copyright file="qwickCharactLevelBackgroundConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Конвертер для фона кнопок быстрого задания уровней характеристик
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows.Data;

    using Sample.ViewModel;

    /// <summary>
    /// Конвертер для фона кнопок быстрого задания уровней характеристик
    /// </summary>
    public class qwickCharactLevelBackgroundConverter : IValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Characteristic cha = value as Characteristic;
            if (cha == null)
            {
                return null;
            }

            double valueCharact = System.Convert.ToDouble(cha.ValueProperty);
            int level = 0;
            string color = "White";

            string s = parameter.ToString();

            if (s == "Критично")
            {
                if (level == 0)
                {
                    color = "Red";
                }
            }

            if (s == "ОчПлохо")
            {
                if (level >= 1 && level < 3)
                {
                    color = "Orange";
                }
            }

            if (s == "НижеСреднего")
            {
                if (level >= 3 && level < 5)
                {
                    color = "LimeGreen";
                }
            }

            if (s == "Норм")
            {
                if (level >= 5 && level < 7)
                {
                    color = "Lime";
                }
            }

            if (s == "Хорошо")
            {
                if (level >= 7 && level < 10)
                {
                    color = "Gold";
                }
            }

            if (s == "Супер")
            {
                if (level >= 10)
                {
                    color = "Yellow";
                }
            }

            return color;
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
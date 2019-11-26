// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChaToolTipConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The cha tool tip converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    /// The cha tool tip converter.
    /// </summary>
    public class ChaToolTipConverter : IValueConverter
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
            if (value == DependencyProperty.UnsetValue)
            {
                return string.Empty;
            }
            else
            {
                var charact = (Characteristic)value;
                if (charact == null)
                {
                    return string.Empty;
                }

                string text = string.Empty;
                var val = charact.ValueProperty;
                int level = charact.LevelProperty;
                var rang = charact.Rangs.OrderBy(n => n.LevelRang).FirstOrDefault(n => n.LevelRang >= level);

                text += " Название: " + charact.NameOfProperty;
                text += "\n Уровень: " + level.ToString();
                text += "\n Значение: " + val.ToString() + "/"
                        + Pers.ExpToLevel(level + 1, RpgItemsTypes.characteristic).ToString();
                text += "\n Ранг: " + rang.NameOfRang;
                text += "\n Описание ранга: " + rang.DeskriptionRangProperty;
                text += "\n Описание характеристики: " + charact.DescriptionProperty;

                return text;
            }
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
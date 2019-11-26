// --------------------------------------------------------------------------------------------------------------------
// <copyright file="expConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The exp converter.
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

    /// <summary>
    /// The exp converter.
    /// </summary>
    public class expConverter : IValueConverter
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
            int exp = System.Convert.ToInt32(value);
            if (exp <= 5 && exp >= 0)
            {
                return "Элементарно";
            }
            else if (exp < 10 && exp > 5)
            {
                return "Легко";
            }
            else if (exp < 20 && exp >= 10)
            {
                return "Нормально";
            }
            else if (exp < 30 && exp >= 20)
            {
                return "Стоит поднапрячься";
            }
            else if (exp < 50 && exp >= 30)
            {
                return "Сложно";
            }
            else if (exp < 90 && exp >= 50)
            {
                return "Очень сложно";
            }
            else if (exp >= 90)
            {
                return "Очень крупный проект или достижение";
            }

            return null;
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
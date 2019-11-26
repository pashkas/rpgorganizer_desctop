// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stringToDateConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The string to date converter.
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
    /// The string to date converter.
    /// </summary>
    public class stringToDateConverter : IValueConverter
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
            string strDate = (string)value;
            if (string.IsNullOrEmpty(strDate))
            {
                if (parameter != null && parameter.ToString() == "short")
                {
                    return DateTime.MinValue.ToShortDateString();
                }

                return DateTime.MinValue;
            }
            else
            {
                if (parameter != null && parameter.ToString() == "short")
                {
                    return DateTime.Parse(strDate).ToShortDateString();
                }

                return DateTime.Parse(strDate);
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime Date = (DateTime)value;
            return Date.ToString();
        }

        #endregion
    }
}
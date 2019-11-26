// --------------------------------------------------------------------------------------------------------------------
// <copyright file="verBacgroundConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The ver bacground converter.
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
    using System.Windows.Media;

    /// <summary>
    /// The ver bacground converter.
    /// </summary>
    public class verBacgroundConverter : IMultiValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="values">
        /// The values.
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
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return Brushes.White;
            }

            var val = System.Convert.ToDouble(values[0]);
            var min = System.Convert.ToDouble(values[1]);
            var max = System.Convert.ToDouble(values[2]);

            if (max == -1 && min == -1 && val == -1)
            {
                return Brushes.Yellow;
            }

            if (max == 0 && val >= min && min >= 0)
            {
                return Brushes.Yellow;
            }

            if (val > min && val <= max)
            {
                return Brushes.Yellow;
            }
            else
            {
                return Brushes.White;
            }
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetTypes">
        /// The target types.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object[]"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
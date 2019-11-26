// --------------------------------------------------------------------------------------------------------------------
// <copyright file="taskRelayValueConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Влияние квестов и задач на скилл
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Влияние квестов и задач на скилл
    /// </summary>
    public class taskRelayValueConverter : IMultiValueConverter
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
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return null;
            }
            if (parameter.ToString() == "квесты")
            {
                if (values[1] == null)
                {
                    return null;
                }

                var aim = (Aim)values[0];
                var abil = (AbilitiModel)values[1];
                var pers = (Pers)values[2];

                return "+ ";
            }

            return null;
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
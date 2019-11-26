// --------------------------------------------------------------------------------------------------------------------
// <copyright file="progLevelConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The prog level converter.
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
    /// The prog level converter.
    /// </summary>
    public class progLevelConverter : IValueConverter
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
            if (value != null && string.IsNullOrEmpty(value.ToString()))
            {
                return 0;
            }

            double val;
            bool isLinear = false;
            RpgItemsTypes rpgType;

            if (parameter != null && parameter.ToString() == "навык")
            {
                AbilitiModel ability = value as AbilitiModel;
                if (ability != null)
                {
                    val = ability.ValueProperty;
                }
                else
                {
                    return 0;
                }

                rpgType = RpgItemsTypes.ability;
            }
            else
            {
                val = System.Convert.ToDouble(value);
                rpgType = RpgItemsTypes.characteristic;
            }

           
            return 0;
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
            return null;
        }

        #endregion
    }
}
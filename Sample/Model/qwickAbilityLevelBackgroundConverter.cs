// --------------------------------------------------------------------------------------------------------------------
// <copyright file="qwickAbilityLevelBackgroundConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The qwick ability level background converter.
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
    /// The qwick ability level background converter.
    /// </summary>
    public class qwickAbilityLevelBackgroundConverter : IValueConverter
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
            AbilitiModel ability = value as AbilitiModel;
            if (ability == null)
            {
                return "White";
            }

            double valueAbility = System.Convert.ToDouble(ability.ValueProperty);

            string color = "White";

            string s = parameter.ToString();

            if (s == "Начинающий")
            {
                if (valueAbility >= 0 && valueAbility < 60)
                {
                    color = "LightGray";
                }
            }

            if (s == "Ученик")
            {
                if (valueAbility >= 60 && valueAbility < 300)
                {
                    color = "Yellow";
                }
            }

            if (s == "Продвинутый")
            {
                if (valueAbility >= 300 && valueAbility < 720)
                {
                    color = "Lime";
                }
            }

            if (s == "Мастер")
            {
                if (valueAbility >= 720)
                {
                    color = "SteelBlue";
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
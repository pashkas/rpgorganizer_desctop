// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NeedValueConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The need value converter.
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
    /// The need value converter.
    /// </summary>
    public class NeedValueConverter : IValueConverter
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
            if (parameter.ToString() == "навык")
            {
                NeedAbility needAbility = value as NeedAbility;
                double percentage;
                if (needAbility.IsValueProperty <= needAbility.ValueProperty)
                {
                    percentage = (needAbility.IsValueProperty - needAbility.FirstValueProperty) * 100
                                 / (needAbility.ValueProperty - needAbility.FirstValueProperty);
                }
                else
                {
                    percentage = 100;
                }

                return Math.Round(percentage, 0);
            }

            if (parameter.ToString() == "задача")
            {
                NeedTasks needTasks = value as NeedTasks;
                double percentage;
                if (needTasks.IsValueProperty <= needTasks.ValueProperty)
                {
                    percentage = (needTasks.IsValueProperty - needTasks.FirstValueProperty) * 100
                                 / (needTasks.ValueProperty - needTasks.FirstValueProperty);
                }
                else
                {
                    percentage = 100;
                }

                return Math.Round(percentage, 0);
            }

            if (parameter.ToString() == "характеристика")
            {
                NeedCharact needCharact = value as NeedCharact;
                double percentage;
                if (needCharact.IsValueProperty <= needCharact.ValueProperty)
                {
                    percentage = (needCharact.IsValueProperty - needCharact.FirstValueProperty) * 100
                                 / (needCharact.ValueProperty - needCharact.FirstValueProperty);
                }
                else
                {
                    percentage = 100;
                }

                return Math.Round(percentage, 0);
            }

            if (parameter.ToString() == "квест")
            {
                Aim aim = value as Aim;
                double percentage;

                return Math.Round(aim.AutoProgressValueProperty, 0);
            }

            if (parameter.ToString() == "квестТр")
            {
                CompositeAims ca = value as CompositeAims;
                Aim aim = ca.AimProperty;
                double percentage;
                return Math.Round(aim.AutoProgressValueProperty, 0);
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
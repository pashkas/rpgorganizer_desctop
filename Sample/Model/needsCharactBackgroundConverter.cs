// --------------------------------------------------------------------------------------------------------------------
// <copyright file="needsCharactBackgroundConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The needs charact background converter.
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
    using System.Windows.Media;

    /// <summary>
    /// The needs charact background converter.
    /// </summary>
    public class needsCharactBackgroundConverter : IMultiValueConverter
    {
        #region Methods

        /// <summary>
        /// Получить значение характеристики, которое есть
        /// </summary>
        /// <param name="per">
        /// Персонаж
        /// </param>
        /// <param name="charact">
        /// The charact.
        /// </param>
        /// <returns>
        /// значение
        /// </returns>
        private double GetIsValue(Pers per, Characteristic charact)
        {
            var chaPers = per.Characteristics.First(n => n == charact);

            return chaPers.ValueProperty;
        }

        #endregion

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
            double needValue = System.Convert.ToDouble(values[0]);
            string typeNeed = values[1].ToString();
            Characteristic charact = values[3] as Characteristic;
            double isValue = this.GetIsValue(values[2] as Pers, charact);

            if (typeNeed == ">=")
            {
                if (isValue >= needValue)
                {
                    return 1;
                }
            }

            if (typeNeed == "<=")
            {
                if (isValue <= needValue)
                {
                    return 1;
                }
            }

            return 0;
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
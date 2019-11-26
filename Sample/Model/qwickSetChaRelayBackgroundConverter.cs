// --------------------------------------------------------------------------------------------------------------------
// <copyright file="qwickSetChaRelayBackgroundConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The qwick set cha relay background converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// The qwick set cha relay background converter.
    /// </summary>
    public class qwickSetChaRelayBackgroundConverter : IMultiValueConverter
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
            Characteristic charact = values[0] as Characteristic;
            ObservableCollection<Rangs> rangs = values[1] as ObservableCollection<Rangs>;

            if (rangs == null)
            {
                return Brushes.White;
            }

            string s = parameter.ToString();

            double val = 0;

            if (s == "Нет")
            {
                double sum = 0;

                val += sum;

                if (val == 0)
                {
                    return Brushes.MediumSpringGreen;
                }
            }
            else if (s == "Слабо")
            {
                if (val > 0 && val < 110)
                {
                    return Brushes.Yellow;
                }
            }
            else if (s == "Норм")
            {
                if (val >= 110 && val < 165)
                {
                    return Brushes.Lime;
                }
            }
            else if (s == "Сильно")
            {
                if (val >= 165)
                {
                    return Brushes.LightSteelBlue;
                }
            }

            return Brushes.White;
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
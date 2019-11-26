// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharactDebuphValueConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The charact debuph value converter.
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

    using Sample.ViewModel;

    /// <summary>
    /// The charact debuph value converter.
    /// </summary>
    public class CharactDebuphValueConverter : IMultiValueConverter
    {
        #region Fields

        /// <summary>
        /// The debuph.
        /// </summary>
        private double debuph;

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
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return 0;
            }

            double value = System.Convert.ToDouble(values[0]);
            this.debuph = System.Convert.ToDouble(values[1]);
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
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            object[] ret = new object[2];
            ret[0] = System.Convert.ToDouble(value) - this.debuph;
            ret[1] = this.debuph;
            if (parameter != null)
            {
                int level;
                try
                {
                    level = System.Convert.ToInt32(value);
                }
                catch (Exception)
                {
                    level = 0;
                }

                switch (level)
                {
                    case 0:
                        ret[0] = 0;
                        break;
                    case 1:
                        ret[0] = MainViewModel.опытаДоПервогоУровня;
                        break;
                }

                ret[0] = MainViewModel.опытаДоПервогоУровня * (level - 1) * level;
                return ret;
            }
            else
            {
                return ret;
            }
        }

        #endregion
    }
}
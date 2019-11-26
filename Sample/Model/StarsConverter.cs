// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StarsConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Сколько звездочек для скиллов или характеристик
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

    using Sample.Properties;

    /// <summary>
    /// Сколько звездочек для скиллов или характеристик
    /// </summary>
    public class StarsConverter : IValueConverter
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
            List<int> list = new List<int>();

            try
            {
                if (parameter != null && parameter.ToString() == "навык")
                {
                    AbilitiModel abilitiModel = value as AbilitiModel;
                    int levelAbility = abilitiModel.LevelProperty;
                    var levelsInStar = System.Convert.ToDouble(Settings.Default.LevelsInStar);
                    var _stars = Math.Truncate(levelAbility / levelsInStar);
                    int stars = (int)_stars;
                    for (int i = 0; i < stars; i++)
                    {
                        list.Add(i);
                    }
                }
                else
                {
                    var first = System.Convert.ToDouble(System.Convert.ToInt32(value));
                    var levelsInStar = System.Convert.ToDouble(Settings.Default.LevelsInStar);
                    var _stars = Math.Truncate(first / levelsInStar);
                    int stars = (int)_stars;
                    for (int i = 0; i < stars; i++)
                    {
                        list.Add(i);
                    }
                }
            }
            catch
            {
            }

            return list;
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
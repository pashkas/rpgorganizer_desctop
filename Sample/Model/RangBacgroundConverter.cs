// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangBacgroundConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The rang bacground converter.
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
    /// The rang bacground converter.
    /// </summary>
    public class RangBacgroundConverter : IMultiValueConverter
    {
        #region Methods

        /// <summary>
        /// Получаем последний ранг, соответствующий уровню
        /// </summary>
        /// <param name="levelAbility">
        /// уровень
        /// </param>
        /// <param name="rangs">
        /// все ранги
        /// </param>
        /// <returns>
        /// The <see cref="Rangs"/>.
        /// </returns>
        private Rangs getLastRang(int levelAbility, ObservableCollection<Rangs> rangs)
        {
            return rangs.Where(n => n.LevelRang <= levelAbility).OrderBy(n => n.LevelRang).LastOrDefault();
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
            if (parameter.ToString() == "навык")
            {
                Rangs rang = values[0] as Rangs;
                AbilitiModel ability = values[1] as AbilitiModel;
                if (ability == null)
                {
                    return Brushes.White;
                }

                int levelAbility = ability.LevelProperty;
                Rangs lastRang = this.getLastRang(levelAbility, ability.Rangs);
                if (rang == lastRang)
                {
                    return Brushes.Yellow;
                }
                else
                {
                    return Brushes.White;
                }
            }

            if (parameter.ToString() == "характеристика")
            {
                Rangs rang = values[0] as Rangs;
                Characteristic charact = values[1] as Characteristic;
                if (charact == null)
                {
                    return Brushes.White;
                }

                int levelCharact = charact.LevelProperty;

                Rangs lastRang =
                    charact.Rangs.Where(n => n.LevelRang <= levelCharact).OrderBy(n => n.LevelRang).LastOrDefault();
                if (rang == lastRang)
                {
                    return Brushes.Yellow;
                }
                else
                {
                    return Brushes.White;
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
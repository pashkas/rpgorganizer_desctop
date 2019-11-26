// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeTaskAbilityNotDoneConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The change task ability not done converter.
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

    /// <summary>
    /// The change task ability not done converter.
    /// </summary>
    public class ChangeTaskAbilityNotDoneConverter : IMultiValueConverter
    {
        #region Fields

        /// <summary>
        /// The abiliti.
        /// </summary>
        private AbilitiModel abiliti;

        /// <summary>
        /// The change ability.
        /// </summary>
        private ObservableCollection<ChangeAbilityModele> changeAbility;

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
            this.abiliti = values[1] as AbilitiModel;
            this.changeAbility = values[0] as ObservableCollection<ChangeAbilityModele>;
            double changeAbilityProperty =
                this.changeAbility.First(n => n.AbilityProperty == this.abiliti).ChangeAbilityProperty;
            return changeAbilityProperty.ToString();
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
            this.changeAbility.First(n => n.AbilityProperty == this.abiliti).ChangeAbilityProperty =
                System.Convert.ToDouble(value);
            object[] ret = new[] { Binding.DoNothing, Binding.DoNothing };
            return ret;
        }

        #endregion
    }
}
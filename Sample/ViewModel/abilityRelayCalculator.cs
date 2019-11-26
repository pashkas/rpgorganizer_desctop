// --------------------------------------------------------------------------------------------------------------------
// <copyright file="abilityRelayCalculator.cs" company="">
//   
// </copyright>
// <summary>
//   The ability relay calculator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// The ability relay calculator.
    /// </summary>
    public class abilityRelayCalculator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Fields

        /// <summary>
        /// Текущее значение уровня.
        /// </summary>
        private int currentLevel;

        /// <summary>
        /// Текущее значение скилла.
        /// </summary>
        private double currentValue;

        /// <summary>
        /// Желаемый уровень.
        /// </summary>
        private int needLevel;

        /// <summary>
        /// Желаемое значение.
        /// </summary>
        private double needValue;

        /// <summary>
        /// Выбранный навык.
        /// </summary>
        private AbilitiModel selectedAbility;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Текущее значение уровня.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CurrentLevelProperty
        {
            get
            {
                return this.currentLevel;
            }

            set
            {
                if (this.currentLevel == value)
                {
                    return;
                }

                this.currentLevel = value;
                OnPropertyChanged(nameof(CurrentLevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Текущее значение скилла.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double CurrentValueProperty
        {
            get
            {
                return this.currentValue;
            }

            set
            {
                if (this.currentValue == value)
                {
                    return;
                }

                this.currentValue = value;
                this.CurrentLevelProperty = StaticMetods.GetLevel(this.currentValue, RpgItemsTypes.ability);
                OnPropertyChanged(nameof(CurrentValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Желаемый уровень.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int NeedLevelProperty
        {
            get
            {
                return this.needLevel;
            }

            set
            {
                if (this.needLevel == value)
                {
                    return;
                }

                this.needLevel = value;
                OnPropertyChanged(nameof(NeedLevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Желаемое значение.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double NeedValueProperty
        {
            get
            {
                return this.needValue;
            }

            set
            {
                if (this.needValue == value)
                {
                    return;
                }

                this.needValue = value;
                this.CurrentLevelProperty = StaticMetods.GetLevel(this.currentValue, RpgItemsTypes.ability);
                OnPropertyChanged(nameof(NeedValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранный скилл.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AbilitiModel SelectedAbilityProperty
        {
            get
            {
                return this.selectedAbility;
            }

            set
            {
                if (this.selectedAbility == value)
                {
                    return;
                }

                this.selectedAbility = value;

                this.CurrentValueProperty = this.selectedAbility.ValueProperty;
                OnPropertyChanged(nameof(CurrentValueProperty));
                this.NeedValueProperty = this.selectedAbility.ValueProperty;
                OnPropertyChanged(nameof(CurrentLevelProperty));

                OnPropertyChanged(nameof(SelectedAbilityProperty));
            }
        }

        #endregion
    }
}
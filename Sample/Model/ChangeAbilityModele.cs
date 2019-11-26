// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeAbilityModele.cs" company="">
//   
// </copyright>
// <summary>
//   Изменение скиллов
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// Изменение скиллов
    /// </summary>
    [Serializable]
    public class ChangeAbilityModele : INotifyPropertyChanged
    {
        /// <summary>
        /// Сколько раз нужно сделать до совершенства.
        /// </summary>
        private int countToPerfect;

        /// <summary>
        /// Сила влияния.
        /// </summary>
        private int kRelay;

        /// <summary>
        /// Sets and gets Сила влияния.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int KRelayProperty
        {
            get
            {
                return kRelay;
            }

            set
            {
                if (kRelay == value)
                {
                    return;
                }

                kRelay = value;
                OnPropertyChanged("KRelayProperty");
            }
        }

        /// <summary>
        /// Sets and gets Сколько раз нужно сделать до совершенства.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CountToPerfectProperty
        {
            get
            {
                return countToPerfect;
            }

            set
            {
                if (countToPerfect == value)
                {
                    return;
                }

                countToPerfect = value;
                OnPropertyChanged("CountToPerfectProperty");
            }
        }

        #region Public Events

        /// <summary>
        /// The property changed.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Скилл.
        /// </summary>
        private AbilitiModel ability;

        /// <summary>
        /// На сколько скилл меняется.
        /// </summary>
        private double changeAbility;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets скилл.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AbilitiModel AbilityProperty
        {
            get
            {
                return this.ability;
            }

            set
            {
                if (this.ability == value)
                {
                    return;
                }

                this.ability = value;
                this.OnPropertyChanged("AbilityProperty");
            }
        }

        /// <summary>
        /// Sets and gets На сколько скилл меняется.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ChangeAbilityProperty
        {
            get
            {
                this.changeAbility = Math.Round(this.changeAbility, 1);
                return this.changeAbility;
            }

            set
            {
                if (this.changeAbility == value)
                {
                    return;
                }

                this.changeAbility = Math.Round(value, 1);
                this.changeAbility = value;
                this.OnPropertyChanged("ChangeAbilityProperty");
            }
        }

        #endregion
    }
}
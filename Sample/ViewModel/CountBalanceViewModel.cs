// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountBalanceViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Вью модель для расчета баланса
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// Вью модель для расчета баланса
    /// </summary>
    public class CountBalanceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Methods

        /// <summary>
        /// Расчет баланса характеристик
        /// </summary>
        /// <param name="characteristic">
        /// характеристики
        /// </param>
        /// <param name="abilitis">
        /// скиллы
        /// </param>
        /// <param name="persParam">
        /// </param>
        /// <param name="maxChaLevelParam">
        /// </param>
        private void countChaBalance(
            Characteristic characteristic,
            ObservableCollection<AbilitiModel> abilitis,
            Pers persParam,
            int maxChaLevelParam)
        {
        }

        #endregion

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
        /// Максимальная прокачка характеристик от скиллов.
        /// </summary>
        private int maxCha;

        /// <summary>
        /// Максимальный уровень персонажа, который даст прокачка всех имеющихся на данный момент характеристик и скиллов до максимума.
        /// </summary>
        private int maxChaAbilLevel;

        /// <summary>
        /// Данные персонажа.
        /// </summary>
        private Pers pers;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CountBalanceViewModel"/> class.
        /// </summary>
        public CountBalanceViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountBalanceViewModel"/> class.
        /// </summary>
        /// <param name="_pers">
        /// Персонаж
        /// </param>
        public CountBalanceViewModel(Pers _pers)
        {
            this.PersProperty = _pers;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Максимальный уровень персонажа, который даст прокачка всех имеющихся на данный момент характеристик и скиллов до максимума.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxChaAbilLevelProperty
        {
            get
            {
                return this.maxChaAbilLevel;
            }

            set
            {
                if (this.maxChaAbilLevel == value)
                {
                    return;
                }

                this.maxChaAbilLevel = value;
                OnPropertyChanged(nameof(MaxChaAbilLevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальная прокачка характеристик от скиллов.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxChaProperty
        {
            get
            {
                return this.maxCha;
            }

            set
            {
                if (this.maxCha == value)
                {
                    return;
                }

                this.maxCha = value;
                OnPropertyChanged(nameof(MaxChaProperty));
            }
        }

        /// <summary>
        /// Sets and gets Данные персонажа.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Pers PersProperty
        {
            get
            {
                return this.pers;
            }

            set
            {
                if (this.pers == value)
                {
                    return;
                }

                this.pers = value;
                OnPropertyChanged(nameof(PersProperty));
            }
        }

        #endregion
    }
}
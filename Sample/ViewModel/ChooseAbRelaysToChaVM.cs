using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// Расчет влияний скиллов на характеристику
    /// </summary>
    public class ChooseAbRelaysToChaVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        /// Выбранная характеристика.
        /// </summary>
        private Characteristic selCharacteristic;

        public ChooseAbRelaysToChaVM()
        {
            this.PersProperty = StaticMetods.PersProperty;
        }

        /// <summary>
        /// Sets and gets Персонаж.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Pers PersProperty
        {
            get
            {
                return pers;
            }

            set
            {
                if (pers == value)
                {
                    return;
                }

                pers = value;
                OnPropertyChanged(nameof(PersProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранная характеристика.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Characteristic SelCharacteristicProperty
        {
            get
            {
                return selCharacteristic;
            }

            set
            {
                if (selCharacteristic == value)
                {
                    return;
                }

                selCharacteristic = value;
                OnPropertyChanged(nameof(SelCharacteristicProperty));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Задать выбранную характеристику
        /// </summary>
        public void SetSelCha(Characteristic cha)
        {
            this.SelCharacteristicProperty = cha;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
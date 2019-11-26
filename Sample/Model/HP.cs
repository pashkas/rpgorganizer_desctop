using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// Класс - здоровье персонажа
    /// </summary>
    [Serializable]
    public class HP : INotifyPropertyChanged
    {
        /// <summary>
        /// Текущий показатель здоровья.
        /// </summary>
        private int currentHP;

        /// <summary>
        /// Максимальное значение здоровья.
        /// </summary>
        private int maxHP;

        /// <summary>
        /// Sets and gets Текущий показатель здоровья.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CurrentHPProperty
        {
            get
            {
                if (currentHP > this.MaxHPProperty)
                {
                    currentHP = this.MaxHPProperty;
                }

                return currentHP;
            }

            set
            {
                if (currentHP == value)
                {
                    return;
                }

                if (value < 0)
                {
                    value = 0;
                }

                if (value > this.MaxHPProperty)
                {
                    value = this.MaxHPProperty;
                }

                currentHP = value;

               

                OnPropertyChanged(nameof(CurrentHPProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальное значение здоровья.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxHPProperty
        {
            get
            {
                return maxHP;
            }

            set
            {
                if (maxHP == value)
                {
                    return;
                }

                maxHP = value;
                OnPropertyChanged(nameof(MaxHPProperty));
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
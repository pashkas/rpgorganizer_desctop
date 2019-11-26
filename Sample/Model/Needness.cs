using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Media;

    using Sample.Annotations;

    /// <summary>
    /// Класс - потребности
    /// </summary>
    [Serializable]
    public class Needness : INotifyPropertyChanged
    {
        /// <summary>
        /// Цвет потребности.
        /// </summary>
        private string color;

        /// <summary>
        /// Описание потребности.
        /// </summary>
        private string description;

        /// <summary>
        /// За сколько минут потребность обнуляется.
        /// </summary>
        private int minutesToNull;

        /// <summary>
        /// Gets the Сдвинуть требование вверх или вниз.
        /// </summary>
        [field: NonSerialized]
        private GalaSoft.MvvmLight.Command.RelayCommand<string> moveNeednessCommand;

        /// <summary>
        /// Название потребности.
        /// </summary>
        private string nameOfNeedness;

        /// <summary>
        /// Комманда Обновить значение.
        /// </summary>
        [field: NonSerialized]
        private GalaSoft.MvvmLight.Command.RelayCommand refreshValueCommand;

        /// <summary>
        /// Комманда Удалить требование.
        /// </summary>
        [field: NonSerialized]
        private GalaSoft.MvvmLight.Command.RelayCommand removeNeednessCommand;

        /// <summary>
        /// Текущее значение потребности.
        /// </summary>
        private double valueOfNeedness;

        /// <summary>
        /// Sets and gets Название потребности.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameOfNeednessProperty
        {
            get
            {
                return nameOfNeedness;
            }

            set
            {
                if (nameOfNeedness == value)
                {
                    return;
                }

                nameOfNeedness = value;
                OnPropertyChanged(nameof(NameOfNeednessProperty));
            }
        }

        /// <summary>
        /// Sets and gets Текущее значение потребности.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ValueOfNeednessProperty
        {
            get
            {
                return valueOfNeedness;
            }

            set
            {
                if (valueOfNeedness == value)
                {
                    return;
                }

                if (value < 0)
                {
                    value = 0;
                }

                valueOfNeedness = value;
                OnPropertyChanged(nameof(ValueOfNeednessProperty));
                OnPropertyChanged(nameof(PercentegeOfValue));
            }
        }

        /// <summary>
        /// Sets and gets Описание потребности.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DescriptionProperty
        {
            get
            {
                return description;
            }

            set
            {
                if (description == value)
                {
                    return;
                }

                description = value;
                OnPropertyChanged(nameof(DescriptionProperty));
            }
        }

        /// <summary>
        /// Значение потребности в процентах
        /// </summary>
        public string PercentegeOfValue
        {
            get
            {
                return System.Convert.ToInt32(ValueOfNeednessProperty).ToString() + "%";
            }
        }

        /// <summary>
        /// Sets and gets За сколько минут потребность обнуляется.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MinutesToNullProperty
        {
            get
            {
                return minutesToNull;
            }

            set
            {
                if (minutesToNull == value)
                {
                    return;
                }

                minutesToNull = value;
                OnPropertyChanged(nameof(MinutesToNullProperty));
            }
        }

        /// <summary>
        /// Sets and gets Цвет потребности.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ColorProperty
        {
            get
            {
                return color;
            }

            set
            {
                if (color == value)
                {
                    return;
                }

                color = value;
                OnPropertyChanged(nameof(ColorProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Обновить значение.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand RefreshValueCommand
        {
            get
            {
                return refreshValueCommand ?? (refreshValueCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                    () =>
                    {
                        this.RefreshNeedness();
                        StaticMetods.terribleBuffIfNeed(StaticMetods.PersProperty);
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Сдвинуть требование вверх или вниз.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand<string> MoveNeednessCommand
        {
            get
            {
                return moveNeednessCommand
                       ?? (moveNeednessCommand = new GalaSoft.MvvmLight.Command.RelayCommand<string>(
                           (item) =>
                           {
                               var neednesses = StaticMetods.PersProperty.NeednessCollection;
                               var oldIndex = neednesses.IndexOf(this);

                               switch (item)
                               {
                                   case "down":
                                       neednesses.Move(oldIndex, oldIndex + 1);
                                       break;
                                   case "up":
                                       neednesses.Move(oldIndex, oldIndex - 1);
                                       break;
                               }
                           },
                           (item) =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               var neednesses = StaticMetods.PersProperty.NeednessCollection;

                               var indexOf = neednesses.IndexOf(this);

                               if (item == "down")
                               {
                                   if (indexOf + 1 >= neednesses.Count)
                                   {
                                       return false;
                                   }
                               }

                               if (item == "up")
                               {
                                   if (indexOf - 1 < 0)
                                   {
                                       return false;
                                   }
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Gets the комманда Удалить требование.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand RemoveNeednessCommand
        {
            get
            {
                return removeNeednessCommand
                       ?? (removeNeednessCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { StaticMetods.PersProperty.NeednessCollection.Remove(this); },
                               () => { return true; }));
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Получить новую потребность
        /// </summary>
        /// <returns>Новая потребность</returns>
        public static Needness GetNewNeedness()
        {
            return new Needness()
                   {
                       NameOfNeednessProperty = "Новая потребность",
                       ValueOfNeednessProperty = 100.0,
                       MinutesToNullProperty = 60,
                       ColorProperty = Brushes.Yellow.ToString()
                   };
        }

        /// <summary>
        /// Обновить потребность
        /// </summary>
        public void RefreshNeedness()
        {
            this.ValueOfNeednessProperty = 100.0;
        }

        /// <summary>
        /// Убавить потребность по таймеру
        /// </summary>
        public void minusNeedness()
        {
            var minus = 100.0 / System.Convert.ToDouble(MinutesToNullProperty);
            this.ValueOfNeednessProperty -= minus;
            OnPropertyChanged(nameof(PercentegeOfValue));
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// Родительский класс для задач, скиллов, характеристик и наград
    /// </summary>
    public abstract class RPGItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Цвет элемента.
        /// </summary>
        private string color;

        /// <summary>
        /// Картинка элемента.
        /// </summary>
        private byte[] Image;

        /// <summary>
        /// Минимальный уровень.
        /// </summary>
        private int minLevel;

        /// <summary>
        /// Название РПГ элемента.
        /// </summary>
        private string name;

        /// <summary>
        /// Значение РПГ элемента.
        /// </summary>
        private double progress;

        /// <summary>
        /// Sets and gets Название РПГ элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameProperty
        {
            get
            {
                return name;
            }

            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;
                OnPropertyChanged(nameof(NameProperty));
            }
        }

        /// <summary>
        /// Sets and gets Цвет элемента.
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
        /// Sets and gets Картинка элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte[] ImageProperty
        {
            get
            {
                return Image;
            }

            set
            {
                if (Image == value)
                {
                    return;
                }

                Image = value;
                OnPropertyChanged(nameof(ImageProperty));
            }
        }

        /// <summary>
        /// Ранги элемента
        /// </summary>
        public ObservableCollection<Rangs> RangseProperty { get; set; }

        /// <summary>
        /// Sets and gets Минимальный уровень.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MinLevelProperty
        {
            get
            {
                return minLevel;
            }

            set
            {
                if (setMinLevel(ref minLevel, value) == true)
                {
                    OnPropertyChanged(nameof(MinLevelProperty));
                }
            }
        }

        /// <summary>
        /// Sets and gets Значение РПГ элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ProgressProperty
        {
            get
            {
                return getProgress(ref progress);
            }

            set
            {
                if (setProgress(ref progress, value))
                {
                    OnPropertyChanged(nameof(ProgressProperty));
                }
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод для задания минимального уровня
        /// </summary>
        /// <param name="minLev">Минимальный уровень</param>
        /// <param name="val">Значение, которое хотим "задать"</param>
        public virtual bool setMinLevel(ref int minLev, int val)
        {
            if (minLev == val)
            {
                return false;
            }
            else
            {
                minLev = val;
                return true;
            }
        }

        /// <summary>
        /// Метод для получения прогресса элемента
        /// </summary>
        /// <param name="prog">Прогресс</param>
        /// <returns></returns>
        public virtual double getProgress(ref double prog)
        {
            return prog;
        }

        /// <summary>
        /// Метод для задания прогресса элемента
        /// </summary>
        /// <param name="prog">прогресс</param>
        /// <param name="val">значение которое хотим задать</param>
        public virtual bool setProgress(ref double prog, double val)
        {
            if (prog == val)
            {
                return false;
            }

            prog = val;

            return true;
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
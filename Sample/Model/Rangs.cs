// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rangs.cs" company="">
//   
// </copyright>
// <summary>
//   Ранги для уровней, скиллов и характеристик
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Media.Imaging;

    using Sample.Annotations;

    /// <summary>
    /// Ранги для уровней, скиллов и характеристик
    /// </summary>
    [Serializable]
    public class Rangs : INotifyPropertyChanged
    {
        /// <summary>
        /// Изображение.
        /// </summary>
        private byte[] image;

        [field: NonSerialized]
        public BitmapImage pic;

        /// <summary>
        /// Sets and gets Изображение.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte[] ImageProperty
        {
            get
            {
                return image;
            }

            set
            {
                if (image == value)
                {
                    return;
                }

                image = value;
                pic = StaticMetods.getImagePropertyFromImage(this.ImageProperty);
                OnPropertyChanged(nameof(ImageProperty));
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
        [NotifyPropertyChangedInvocator]
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
        /// Ссылка на файл или страничку.
        /// </summary>
        private string deskriptionRang;

        /// <summary>
        /// Опыт при достижении ранга (плюсуются все достигнутые опыты).
        /// </summary>
        private int expForRang;

        /// <summary>
        /// Путь к картинке.
        /// </summary>
        private string pathToImageProperty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Значение таймера для ранга.
        /// </summary>
        private int timerValue;

        /// <summary>
        /// Sets and gets Значение таймера для ранга.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TimerValueProperty
        {
            get
            {
                return timerValue;
            }

            set
            {
                if (timerValue == value)
                {
                    return;
                }

                timerValue = value;
                OnPropertyChanged(nameof(TimerValueProperty));
            }
        }

        /// <summary>
        /// Значение счетчика для ранга.
        /// </summary>
        private int counterValue;

        /// <summary>
        /// Sets and gets Значение счетчика для ранга.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CounterValueProperty
        {
            get
            {
                return counterValue;
            }

            set
            {
                if (counterValue == value)
                {
                    return;
                }

                counterValue = value;
                OnPropertyChanged(nameof(CounterValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Ссылка на файл или страничку.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DeskriptionRangProperty
        {
            get
            {
                return this.deskriptionRang;
            }

            set
            {
                if (this.deskriptionRang == value)
                {
                    return;
                }

                this.deskriptionRang = value;
                this.OnPropertyChanged(nameof(DeskriptionRangProperty));
            }
        }

        /// <summary>
        /// Sets and gets Опыт при достижении ранга (плюсуются все достигнутые опыты).
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ExpForRangProperty
        {
            get
            {
                return this.expForRang;
            }

            set
            {
                if (this.expForRang == value)
                {
                    return;
                }

                this.expForRang = value;
                this.OnPropertyChanged(nameof(ExpForRangProperty));
            }
        }

        /// <summary>
        /// Номер для весовой сортировки уровней рангов
        /// </summary>
        public int NumRang { get; set; }

        /// <summary>
        /// Sets and gets Уровень ранга.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LevelRang { get; set; }

        /// <summary>
        /// Sets and gets Название ранга.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameOfRang { get; set; }

        /// <summary>
        /// Sets and gets Путь к картинке.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PathToImageProperty
        {
            get
            {
                return this.pathToImageProperty;
            }

            set
            {
                if (this.pathToImageProperty == value)
                {
                    return;
                }

                this.pathToImageProperty = value;
                this.OnPropertyChanged(nameof(PathToImageProperty));
            }
        }
        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParametrModel.cs" company="">
//   
// </copyright>
// <summary>
//   Параметры для оценки характеристик
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// Параметры для оценки характеристик
    /// </summary>
    [Serializable]
    public class ParametrModel : INotifyPropertyChanged
    {
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
        /// Максимальный уровень для параметра по умолчанию = 10.
        /// </summary>
        private int maxValue = 10;

        /// <summary>
        /// Словесное обозначение максимального уровня параметра.
        /// </summary>
        private string maxValueName;

        /// <summary>
        /// Словесное обозначение среднего уровня параметра.
        /// </summary>
        private string midleValueName;

        /// <summary>
        /// Словесное обозначение минимального уровня параметра.
        /// </summary>
        private string minValueName;

        /// <summary>
        /// Название параметра.
        /// </summary>
        private string tittle;

        /// <summary>
        /// Значение уровня параметра.
        /// </summary>
        private double valueParam;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Словесное обозначение максимального уровня параметра.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MaxValueNameProperty
        {
            get
            {
                return this.maxValueName;
            }

            set
            {
                if (this.maxValueName == value)
                {
                    return;
                }

                this.maxValueName = value;
                this.OnPropertyChanged(nameof(MaxValueNameProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальное значение параметра.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxValueProperty
        {
            get
            {
                return this.maxValue;
            }

            set
            {
                if (this.maxValue == value)
                {
                    return;
                }

                this.maxValue = value;
                this.OnPropertyChanged(nameof(MaxValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Словесное обозначение среднего уровня параметра.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MidleValueNameProperty
        {
            get
            {
                return this.midleValueName;
            }

            set
            {
                if (this.midleValueName == value)
                {
                    return;
                }

                this.midleValueName = value;
                this.OnPropertyChanged(nameof(MidleValueNameProperty));
            }
        }

        /// <summary>
        /// Sets and gets Словесное обозначение минимального уровня параметра.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MinValueNameProperty
        {
            get
            {
                return this.minValueName;
            }

            set
            {
                if (this.minValueName == value)
                {
                    return;
                }

                this.minValueName = value;
                this.OnPropertyChanged(nameof(MinValueNameProperty));
            }
        }

        /// <summary>
        /// Sets and gets Название параметра.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TittleProperty
        {
            get
            {
                return this.tittle;
            }

            set
            {
                if (this.tittle == value)
                {
                    return;
                }

                this.tittle = value;
                this.OnPropertyChanged(nameof(TittleProperty));
            }
        }

        /// <summary>
        /// Sets and gets Значение уровня параметра.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ValueParamProperty
        {
            get
            {
                return this.valueParam;
            }

            set
            {
                if (this.valueParam == value)
                {
                    return;
                }

                this.valueParam = value;
                this.OnPropertyChanged(nameof(ValueParamProperty));
            }
        }

        #endregion
    }
}
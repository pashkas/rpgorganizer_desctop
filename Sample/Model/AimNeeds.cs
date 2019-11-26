namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// Базовый класс для требований квестов
    /// </summary>
    [Serializable]
    public class AimNeeds : INotifyPropertyChanged
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
        /// Изначальное значение для требования.
        /// </summary>
        private double firstValue;

        /// <summary>
        /// Выполнено ли условие?.
        /// </summary>
        private int isAgree;

        /// <summary>
        /// Текущее значение требования.
        /// </summary>
        private double isValue;

        /// <summary>
        /// Коэфициент сложности выполнения требования.
        /// </summary>
        private double koeficient;

        /// <summary>
        /// Прогресс в требовании.
        /// </summary>
        private double progress;

        /// <summary>
        /// Тип требования: больше равно, меньше равно, выполнена.
        /// </summary>
        private string typeNeed;

        /// <summary>
        /// Значение чему равна характеристика или скилл для требований.
        /// </summary>
        private double valueNeed;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Изначальное значение для требования.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double FirstValueProperty
        {
            get
            {
                return this.firstValue;
            }

            set
            {
                if (this.firstValue == value)
                {
                    return;
                }

                this.firstValue = value;
                this.OnPropertyChanged(nameof(FirstValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выполнено ли условие?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int IsAgreeProperty
        {
            get
            {
                return this.isAgree;
            }

            set
            {
                if (this.isAgree == value)
                {
                    return;
                }

                this.isAgree = value;
                this.OnPropertyChanged(nameof(IsAgreeProperty));
            }
        }

        /// <summary>
        /// Sets and gets Текущее значение требования.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double IsValueProperty
        {
            get
            {
                return this.isValue;
            }

            set
            {
                if (this.isValue == value)
                {
                    return;
                }

                this.isValue = value;
                this.OnPropertyChanged(nameof(IsValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Коэфициент сложности выполнения требования.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public virtual double KoeficientProperty
        {
            get
            {
                return this.koeficient;
            }

            set
            {
                if (this.koeficient == value)
                {
                    return;
                }

                this.koeficient = value;
                this.OnPropertyChanged(nameof(KoeficientProperty));
            }
        }

        /// <summary>
        /// Sets and gets Прогресс в требовании.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ProgressProperty
        {
            get
            {
                return this.progress;
            }

            set
            {
                if (this.progress == value)
                {
                    return;
                }

                this.progress = value;
                this.OnPropertyChanged(nameof(ProgressProperty));
            }
        }

        /// <summary>
        /// Sets and gets Тип требования: больше равно, меньше равно, выполнена.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TypeNeedProperty
        {
            get
            {
                return this.typeNeed;
            }

            set
            {
                if (this.typeNeed == value)
                {
                    return;
                }

                this.typeNeed = value;
                this.OnPropertyChanged(nameof(TypeNeedProperty));
            }
        }

        /// <summary>
        /// Sets and gets Значение чему равна характеристика или скилл для требований.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ValueProperty
        {
            get
            {
                return this.valueNeed;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                this.valueNeed = value;
                this.OnPropertyChanged(nameof(ValueProperty));
            }
        }

        #endregion
    }
}
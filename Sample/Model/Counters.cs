namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// Свободная статистика для Влада
    /// </summary>
    [Serializable]
    public class Counters : INotifyPropertyChanged
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
        /// Счетчик.
        /// </summary>
        private int count;

        /// <summary>
        /// Название счетчика.
        /// </summary>
        private string nameCounter;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Счетчик.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CountProperty
        {
            get
            {
                return this.count;
            }

            set
            {
                if (this.count == value)
                {
                    return;
                }

                this.count = value;
                this.OnPropertyChanged(nameof(CountProperty));
            }
        }

        /// <summary>
        /// Sets and gets Название счетчика.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameCounterProperty
        {
            get
            {
                return this.nameCounter;
            }

            set
            {
                if (this.nameCounter == value)
                {
                    return;
                }

                this.nameCounter = value;
                this.OnPropertyChanged(nameof(NameCounterProperty));
            }
        }

        #endregion
    }
}
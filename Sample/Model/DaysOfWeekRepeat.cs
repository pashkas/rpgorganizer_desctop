namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// The days of week repeat.
    /// </summary>
    [Serializable]
    public class DaysOfWeekRepeat : INotifyPropertyChanged
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
        /// Выбран для повтора?.
        /// </summary>
        private bool _checked;

        /// <summary>
        /// Название дня недели.
        /// </summary>
        private string nameDayOfWeek;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Выбран для повтора?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool CheckedProperty
        {
            get
            {
                return this._checked;
            }

            set
            {
                if (this._checked == value)
                {
                    return;
                }

                this._checked = value;
                this.OnPropertyChanged(nameof(CheckedProperty));
            }
        }

        /// <summary>
        /// День недели
        /// </summary>
        public DayOfWeek Day { get; set; }

        /// <summary>
        /// Sets and gets Название дня недели.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameDayOfWeekProperty
        {
            get
            {
                return this.nameDayOfWeek;
            }

            set
            {
                if (this.nameDayOfWeek == value)
                {
                    return;
                }

                this.nameDayOfWeek = value;
                this.OnPropertyChanged(nameof(NameDayOfWeekProperty));
            }
        }

        #endregion
    }
}
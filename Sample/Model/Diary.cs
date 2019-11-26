namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// Запись дневника
    /// </summary>
    [Serializable]
    public class Diary : INotifyPropertyChanged
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
        /// Дата записи.
        /// </summary>
        private string dateOfWrite;

        /// <summary>
        /// Путь к картинке записи дневника.
        /// </summary>
        private string pathToPic;

        /// <summary>
        /// Квест с которым запись связана.
        /// </summary>
        private Aim qwest;

        /// <summary>
        /// Запись.
        /// </summary>
        private string write;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the date of write date.
        /// </summary>
        public DateTime DateOfWriteDate
        {
            get
            {
                return DateTime.Parse(this.DateOfWriteProperty);
            }
        }

        /// <summary>
        /// Sets and gets Дата записи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DateOfWriteProperty
        {
            get
            {
                return this.dateOfWrite;
            }

            set
            {
                if (this.dateOfWrite == value)
                {
                    return;
                }

                this.dateOfWrite = value;
                this.OnPropertyChanged(nameof(DateOfWriteProperty));
            }
        }

        /// <summary>
        /// Sets and gets Путь к картинке записи дневника.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PathToPicProperty
        {
            get
            {
                return this.pathToPic;
            }

            set
            {
                if (this.pathToPic == value)
                {
                    return;
                }

                this.pathToPic = value;
                this.OnPropertyChanged(nameof(PathToPicProperty));
            }
        }

        /// <summary>
        /// Sets and gets Квест с которым запись связана.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Aim QwestProperty
        {
            get
            {
                return this.qwest;
            }

            set
            {
                if (this.qwest == value)
                {
                    return;
                }

                this.qwest = value;
                this.OnPropertyChanged(nameof(QwestProperty));
            }
        }

        /// <summary>
        /// Gets the short date of write.
        /// </summary>
        public string ShortDateOfWrite
        {
            get
            {
                return DateTime.Parse(this.DateOfWriteProperty).ToShortDateString();
            }
        }

        /// <summary>
        /// Sets and gets Запись.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WriteProperty
        {
            get
            {
                return this.write;
            }

            set
            {
                if (this.write == value)
                {
                    return;
                }

                this.write = value;
                this.OnPropertyChanged(nameof(WriteProperty));
            }
        }

        #endregion
    }
}
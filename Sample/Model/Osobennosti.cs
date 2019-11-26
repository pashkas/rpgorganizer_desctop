namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// The osobennosti.
    /// </summary>
    [Serializable]
    public class Osobennosti : INotifyPropertyChanged
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
        /// Название особенности.
        /// </summary>
        private string nameOsobennost;

        /// <summary>
        /// Путь к картинке особенности.
        /// </summary>
        private string pathToPic;

        /// <summary>
        /// Описание особенности.
        /// </summary>
        private string summaryOsobennost;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Название особенности.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameOsobennostProperty
        {
            get
            {
                return this.nameOsobennost;
            }

            set
            {
                if (this.nameOsobennost == value)
                {
                    return;
                }

                this.nameOsobennost = value;
                this.OnPropertyChanged(nameof(NameOsobennostProperty));
            }
        }

        /// <summary>
        /// Sets and gets Путь к картинке особенности.
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
        /// Sets and gets Описание особенности.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SummaryOsobennostProperty
        {
            get
            {
                return this.summaryOsobennost;
            }

            set
            {
                if (this.summaryOsobennost == value)
                {
                    return;
                }

                this.summaryOsobennost = value;
                this.OnPropertyChanged(nameof(SummaryOsobennostProperty));
            }
        }

        #endregion
    }
}
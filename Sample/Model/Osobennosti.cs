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
        /// �������� �����������.
        /// </summary>
        private string nameOsobennost;

        /// <summary>
        /// ���� � �������� �����������.
        /// </summary>
        private string pathToPic;

        /// <summary>
        /// �������� �����������.
        /// </summary>
        private string summaryOsobennost;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets �������� �����������.
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
        /// Sets and gets ���� � �������� �����������.
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
        /// Sets and gets �������� �����������.
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
namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// The aim cha reqwirements.
    /// </summary>
    [Serializable]
    public class AimChaReqwirements : INotifyPropertyChanged
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
        /// Характеристика для условий доступности квеста.
        /// </summary>
        private Characteristic cha;

        /// <summary>
        /// Минимальный уровень.
        /// </summary>
        private int minLevelProperty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Характеристика для условий доступности квеста.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Characteristic ChaProperty
        {
            get
            {
                return this.cha;
            }

            set
            {
                if (this.cha == value)
                {
                    return;
                }

                this.cha = value;
                this.OnPropertyChanged(nameof(ChaProperty));
            }
        }

        /// <summary>
        /// Sets and gets Минимальный уровень.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MinLevelPropertyProperty
        {
            get
            {
                return this.minLevelProperty;
            }

            set
            {
                if (this.minLevelProperty == value)
                {
                    return;
                }

                this.minLevelProperty = value;
                this.OnPropertyChanged(nameof(MinLevelPropertyProperty));
            }
        }

        #endregion
    }
}
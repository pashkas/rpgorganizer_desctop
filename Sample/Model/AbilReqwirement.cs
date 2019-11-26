namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// The aim abil reqwirements.
    /// </summary>
    [Serializable]
    public class AbilReqwirement : INotifyPropertyChanged
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
        /// Скилл для условия активности квеста.
        /// </summary>
        private AbilitiModel ability;

        /// <summary>
        /// Минимальный уровень скилла для условия активности квеста.
        /// </summary>
        private int minLevel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets скилл для условия активности квеста.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AbilitiModel AbilityProperty
        {
            get
            {
                return this.ability;
            }

            set
            {
                if (this.ability == value)
                {
                    return;
                }

                this.ability = value;
                this.OnPropertyChanged(nameof(AbilityProperty));
            }
        }

        /// <summary>
        /// Sets and gets Минимальный уровень скилла для условия активности квеста.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MinLevelProperty
        {
            get
            {
                return this.minLevel;
            }

            set
            {
                if (this.minLevel == value)
                {
                    return;
                }

                this.minLevel = value;
                this.OnPropertyChanged(nameof(MinLevelProperty));
            }
        }

        #endregion
    }
}
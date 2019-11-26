namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    using Annotations;

    /// <summary>
    /// The abill groups.
    /// </summary>
    [Serializable]
    public class AbillGroups : INotifyPropertyChanged
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
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Fields

        /// <summary>
        /// �������� ������.
        /// </summary>
        private string nameOfGroup;

        /// <summary>
        /// ��� ������.
        /// </summary>
        private string typeOfGroup;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets �������� ������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameOfGroupProperty
        {
            get
            {
                return this.nameOfGroup;
            }
        }

        /// <summary>
        /// Sets and gets ��� ������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TypeOfGroupProperty
        {
            get
            {
                return this.typeOfGroup;
            }
        }

        #endregion
    }
}
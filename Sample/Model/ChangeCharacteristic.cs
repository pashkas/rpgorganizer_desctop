// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeCharacteristic.cs" company="">
//   
// </copyright>
// <summary>
//   The change characteristic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// The change characteristic.
    /// </summary>
    [Serializable]
    public class ChangeCharacteristic : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Значение влияния на характеристику.
        /// </summary>
        private double val;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeCharacteristic"/> class.
        /// </summary>
        public ChangeCharacteristic()
        {
        }

        #endregion

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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Характеристика
        /// </summary>
        public Characteristic Charact { get; set; }

        /// <summary>
        /// Sets and gets Значение влияния на характеристику.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Val
        {
            get
            {
                this.val = Math.Round(this.val, 1);
                return this.val;
            }

            set
            {
                if (this.val == value)
                {
                    return;
                }

                this.val = Math.Round(value, 1);
                this.OnPropertyChanged("Val");
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeAimModel.cs" company="">
//   
// </copyright>
// <summary>
//   Путь к папке
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// Путь к папке
    /// </summary>
    [Serializable]
    public class ChangeAimModel : INotifyPropertyChanged
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
        /// Цель.
        /// </summary>
        private Aim aim;

        /// <summary>
        /// Значение влияния на цель.
        /// </summary>
        private double changeAimValue;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Цель.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Aim AimProperty
        {
            get
            {
                return this.aim;
            }

            set
            {
                if (this.aim == value)
                {
                    return;
                }

                this.aim = value;
                this.OnPropertyChanged("AimProperty");
            }
        }

        /// <summary>
        /// Sets and gets Значение влияния на цель.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ChangeAimValueProperty
        {
            get
            {
                return this.changeAimValue;
            }

            set
            {
                if (this.changeAimValue == value)
                {
                    return;
                }

                this.changeAimValue = value;
                this.OnPropertyChanged("ChangeAimValueProperty");
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharactParameters.cs" company="">
//   
// </copyright>
// <summary>
//   Параметры характеристик
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// Параметры характеристик
    /// </summary>
    [Serializable]
    public class CharactParameters : INotifyPropertyChanged
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
        /// Коэфициент влияния параметра.
        /// </summary>
        private int kRelayParametr;

        /// <summary>
        /// Параметр.
        /// </summary>
        private ParametrModel parametr;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Коэфициент влияния параметра.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int KRelayParametrProperty
        {
            get
            {
                return this.kRelayParametr;
            }

            set
            {
                if (this.kRelayParametr == value)
                {
                    return;
                }

                this.kRelayParametr = value;
                this.OnPropertyChanged(nameof(KRelayParametrProperty));
            }
        }

        /// <summary>
        /// Sets and gets Параметр.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ParametrModel ParametrProperty
        {
            get
            {
                return this.parametr;
            }

            set
            {
                if (this.parametr == value)
                {
                    return;
                }

                this.parametr = value;
                this.OnPropertyChanged(nameof(ParametrProperty));
            }
        }

        #endregion
    }
}
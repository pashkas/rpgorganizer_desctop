// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QwestsChangesModele.cs" company="">
//   
// </copyright>
// <summary>
//   The qwests changes modele.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.ComponentModel;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;

    /// <summary>
    /// The qwests changes modele.
    /// </summary>
    public class QwestsChangesModele : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Fields

        /// <summary>
        /// Опыт после щелчка на галочку.
        /// </summary>
        private double expAfter;

        /// <summary>
        /// Опыт до щелчка на галочку.
        /// </summary>
        private double expBefore;

        /// <summary>
        /// Уровень после щелчка на галочку.
        /// </summary>
        private int levelAfter;

        /// <summary>
        /// Уровень перед щелком на галочку.
        /// </summary>
        private int levelBefore;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Опыт после щелчка на галочку.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ExpAfterProperty
        {
            get
            {
                return this.expAfter;
            }

            set
            {
                if (this.expAfter == value)
                {
                    return;
                }

                this.expAfter = value;
                OnPropertyChanged(nameof(ExpAfterProperty));
            }
        }

        /// <summary>
        /// Sets and gets Опыт до щелчка на галочку.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ExpBeforeProperty
        {
            get
            {
                return this.expBefore;
            }

            set
            {
                if (this.expBefore == value)
                {
                    return;
                }

                this.expBefore = value;
                OnPropertyChanged(nameof(ExpBeforeProperty));
            }
        }

        /// <summary>
        /// Sets and gets Уровень после щелчка на галочку.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LevelAfterProperty
        {
            get
            {
                return this.levelAfter;
            }

            set
            {
                if (this.levelAfter == value)
                {
                    return;
                }

                this.levelAfter = value;
                OnPropertyChanged(nameof(LevelAfterProperty));
            }
        }

        /// <summary>
        /// Sets and gets Уровень перед щелком на галочку.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LevelBeforeProperty
        {
            get
            {
                return this.levelBefore;
            }

            set
            {
                if (this.levelBefore == value)
                {
                    return;
                }

                this.levelBefore = value;
                OnPropertyChanged(nameof(LevelBeforeProperty));
            }
        }

        #endregion
    }
}
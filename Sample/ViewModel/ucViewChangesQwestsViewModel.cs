// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucViewChangesQwestsViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   This class contains properties that a View can data bind to.
//   See http://www.galasoft.ch/mvvm
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using GalaSoft.MvvmLight;

namespace Sample.ViewModel
{
    using System;
    using System.ComponentModel;

    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ucViewChangesQwestsViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the ucViewChangesQwestsViewModel class.
        /// </summary>
        public ucViewChangesQwestsViewModel()
        {
            Messenger.Default.Register<QwestsChangesModele>(
                this,
                _changes =>
                {
                    this.QwestsChangesProperty = _changes;
                    OnPropertyChanged(nameof(QwestsChangesProperty));
                });
        }

        #endregion

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
        /// Максимальный уровень для прогресс бара.
        /// </summary>
        private double expMax;

        /// <summary>
        /// Минимальный опыт для прогресс бара.
        /// </summary>
        private double expMin;

        /// <summary>
        /// Изменения в опыте и уровне персонажа.
        /// </summary>
        private QwestsChangesModele qwestsChanges = new QwestsChangesModele();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Опыта после.
        /// </summary>
        public double ExpAfterProperty
        {
            get
            {
                return this.QwestsChangesProperty.ExpAfterProperty;
            }

            set
            {
                if (this.ExpAfterProperty == value)
                {
                    return;
                }

                this.QwestsChangesProperty.ExpAfterProperty = value;
                OnPropertyChanged(nameof(ExpAfterProperty));
            }
        }

        /// <summary>
        /// Gets or sets Опыта до.
        /// </summary>
        public double ExpBeforeProperty
        {
            get
            {
                return this.QwestsChangesProperty.ExpBeforeProperty;
            }

            set
            {
                if (this.ExpBeforeProperty == value)
                {
                    return;
                }

                this.QwestsChangesProperty.ExpBeforeProperty = value;
                OnPropertyChanged(nameof(ExpBeforeProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальный уровень для прогресс бара.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ExpMaxProperty
        {
            get
            {
                return this.expMax;
            }

            set
            {
                if (this.expMax == value)
                {
                    return;
                }

                this.expMax = value;
                OnPropertyChanged(nameof(ExpMaxProperty));
            }
        }

        /// <summary>
        /// Sets and gets Минимальный опыт для прогресс бара.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ExpMinProperty
        {
            get
            {
                return this.expMin;
            }

            set
            {
                if (this.expMin == value)
                {
                    return;
                }

                this.expMin = value;
                OnPropertyChanged(nameof(ExpMinProperty));
            }
        }

        /// <summary>
        /// Sets and gets Изменения в опыте и уровне персонажа.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public QwestsChangesModele QwestsChangesProperty
        {
            get
            {
                return this.qwestsChanges;
            }

            set
            {
                this.qwestsChanges = value;

                OnPropertyChanged(nameof(QwestsChangesProperty));

                OnPropertyChanged(nameof(levelBeforeProperty));
                OnPropertyChanged(nameof(levelAfterProperty));

                OnPropertyChanged(nameof(ExpBeforeProperty));
                OnPropertyChanged(nameof(ExpAfterProperty));

                this.ExpMinProperty = this.GetMinExp(this.levelBeforeProperty, this.ExpBeforeProperty);
                this.ExpMaxProperty = this.GetMaxExp(this.levelBeforeProperty, this.ExpBeforeProperty);
            }
        }

        /// <summary>
        /// Gets or sets Уровень после.
        /// </summary>
        public int levelAfterProperty
        {
            get
            {
                return this.QwestsChangesProperty.LevelAfterProperty;
            }

            set
            {
                if (this.levelAfterProperty == value)
                {
                    return;
                }

                this.QwestsChangesProperty.LevelAfterProperty = value;
                OnPropertyChanged(nameof(levelAfterProperty));
            }
        }

        /// <summary>
        /// Gets or sets Уровень до.
        /// </summary>
        public int levelBeforeProperty
        {
            get
            {
                return this.QwestsChangesProperty.LevelBeforeProperty;
            }

            set
            {
                if (this.levelBeforeProperty == value)
                {
                    return;
                }

                this.QwestsChangesProperty.LevelBeforeProperty = value;
                OnPropertyChanged(nameof(levelBeforeProperty));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Метод получения максимального значения опыта для прогресс бара
        /// </summary>
        /// <param name="level">
        /// уровень персонажа
        /// </param>
        /// <param name="exp">
        /// опыт
        /// </param>
        /// <returns>
        /// максимальное значение
        /// </returns>
        private double GetMaxExp(int level, double exp)
        {
            if (exp < MainViewModel.опытаДоПервогоУровня)
            {
                return MainViewModel.опытаДоПервогоУровня;
            }
            else
            {
                return MainViewModel.опытаДоПервогоУровня * level * (level + 1) - 1;
            }
        }

        /// <summary>
        /// Метод получения минимального значения опыта для прогресс бара
        /// </summary>
        /// <param name="level">
        /// уровень персонажа
        /// </param>
        /// <param name="exp">
        /// опыт
        /// </param>
        /// <returns>
        /// минимальное значение
        /// </returns>
        private double GetMinExp(int level, double exp)
        {
            if (exp < MainViewModel.опытаДоПервогоУровня)
            {
                return 0;
            }
            else
            {
                if (level == 1)
                {
                    return MainViewModel.опытаДоПервогоУровня;
                }

                return MainViewModel.опытаДоПервогоУровня * (level - 1) * level;
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeOfTask.cs" company="">
//   
// </copyright>
// <summary>
//   The type of task.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// The type of task.
    /// </summary>
    [Serializable]
    public class TypeOfTask : INotifyPropertyChanged
    {
        /// <summary>
        /// Изменение по умолчанию значения если задача пропущена.
        /// </summary>
        private int changeValueIfNotDonedefoult;

        /// <summary>
        /// Максимальное значение для задач по умолчанию.
        /// </summary>
        private int MaxTaskValueDefoult;

        /// <summary>
        /// Вид задач по умолчанию - нет, дело, привычка, обучение.
        /// </summary>
        private string vidZadach;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeOfTask"/> class.
        /// </summary>
        public TypeOfTask()
        {
        }

        #endregion

        /// <summary>
        /// Sets and gets Максимальное значение для задач по умолчанию.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxTaskValueDefoultProperty
        {
            get
            {
                return MaxTaskValueDefoult;
            }

            set
            {
                if (MaxTaskValueDefoult == value)
                {
                    return;
                }

                MaxTaskValueDefoult = value;
                OnPropertyChanged(nameof(MaxTaskValueDefoultProperty));
            }
        }

        /// <summary>
        /// Sets and gets Вид задач по умолчанию - нет, дело, привычка, обучение.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string VidZadachProperty
        {
            get
            {
                if (string.IsNullOrEmpty(vidZadach)
                    || (vidZadach != "Дело" && vidZadach != "Привычка" && vidZadach != "Обучение" && vidZadach != "Нет"))
                {
                    vidZadach = "Нет";
                }
                return vidZadach;
            }

            set
            {
                if (vidZadach == value)
                {
                    return;
                }

                vidZadach = value;
                OnPropertyChanged(nameof(VidZadachProperty));
            }
        }

        /// <summary>
        /// Sets and gets Изменение по умолчанию значения если задача пропущена.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ChangeValueIfNotDonedefoultProperty
        {
            get
            {
                return changeValueIfNotDonedefoult;
            }

            set
            {
                if (changeValueIfNotDonedefoult == value)
                {
                    return;
                }

                changeValueIfNotDonedefoult = value;
                OnPropertyChanged(nameof(ChangeValueIfNotDonedefoultProperty));
            }
        }

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
        /// Опыт по умолчанию.
        /// </summary>
        private int expForDefoult;

        /// <summary>
        /// Золото по умолчанию для этого типа задач.
        /// </summary>
        private int goldForDefoult;

        /// <summary>
        /// Время затраченное для всех задач этого типа.
        /// </summary>
        private int timeIs;

        /// <summary>
        /// Время запланированное для всех задач этого типа.
        /// </summary>
        private int timeMust;

        #endregion

        #region Public Properties

        /// <summary>
        /// Контекст по умолчанию
        /// </summary>
        public Context ContextForDefoult { get; set; }

        /// <summary>
        /// Sets and gets Опыт по умолчанию.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ExpForDefoultProperty
        {
            get
            {
                return this.expForDefoult;
            }

            set
            {
                if (this.expForDefoult == value)
                {
                    return;
                }

                this.expForDefoult = value;
                this.OnPropertyChanged(nameof(ExpForDefoultProperty));
            }
        }

        /// <summary>
        /// ИД типа задач
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// Sets and gets Золото по умолчанию для этого типа задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int GoldForDefoultProperty
        {
            get
            {
                return this.goldForDefoult;
            }

            set
            {
                if (this.goldForDefoult == value)
                {
                    return;
                }

                this.goldForDefoult = value;
                this.OnPropertyChanged(nameof(GoldForDefoultProperty));
            }
        }

        /// <summary>
        /// Gets or sets Интервал повторения по умолчанию
        /// </summary>
        public TimeIntervals IntervalForDefoult { get; set; }

        /// <summary>
        /// Gets or sets Название типа задачи
        /// </summary>
        public string NameOfTypeOfTask { get; set; }

        /// <summary>
        /// Статус по умолчанию
        /// </summary>
        public StatusTask StatusForDefoult { get; set; }

        /// <summary>
        /// Sets and gets Время затраченное для всех задач этого типа.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TimeIsProperty
        {
            get
            {
                return this.timeIs;
            }

            set
            {
                if (this.timeIs == value)
                {
                    return;
                }

                this.timeIs = value;
                this.OnPropertyChanged(nameof(TimeIsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Время запланированное для всех задач этого типа.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TimeMustProperty
        {
            get
            {
                return this.timeMust;
            }

            set
            {
                if (this.timeMust == value)
                {
                    return;
                }

                this.timeMust = value;
                this.OnPropertyChanged(nameof(TimeMustProperty));
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpAbilityModel.cs" company="">
//   
// </copyright>
// <summary>
//   The exp ability model.
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
    /// The exp ability model.
    /// </summary>
    [Serializable]
    public class ExpAbilityModel
    {
        #region Public Properties

        /// <summary>
        /// Скиллы для экспорта
        /// </summary>
        public AbilitiModel Ability { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        public List<expTask> Tasks { get; set; }

        #endregion
    }

    /// <summary>
    /// The exp task.
    /// </summary>
    [Serializable]
    public class expTask : INotifyPropertyChanged
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
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Сколько раз должна быть выполненна.
        /// </summary>
        private int count;

        /// <summary>
        /// Задача.
        /// </summary>
        private Task task;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Сколько раз должна быть выполненна.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CountProperty
        {
            get
            {
                return this.count;
            }

            set
            {
                if (this.count == value)
                {
                    return;
                }

                this.count = value;
                this.OnPropertyChanged(nameof(CountProperty));
            }
        }

        /// <summary>
        /// Sets and gets Задача.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task TaskProperty
        {
            get
            {
                return this.task;
            }

            set
            {
                if (this.task == value)
                {
                    return;
                }

                this.task = value;
                this.OnPropertyChanged(nameof(TaskProperty));
            }
        }

        #endregion
    }
}
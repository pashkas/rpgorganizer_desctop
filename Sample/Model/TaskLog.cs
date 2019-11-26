// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskLog.cs" company="">
//   
// </copyright>
// <summary>
//   The task log.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    /// <summary>
    /// The task log.
    /// </summary>
    [Serializable]
    public class TaskLog
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLog"/> class. 
        /// Конструктор
        /// </summary>
        /// <param name="nameOfTask">
        /// Название задачи
        /// </param>
        /// <param name="date">
        /// Дата выполнения
        /// </param>
        /// <param name="plusOrMinus">
        /// Выполненна или нет
        /// </param>
        public TaskLog(string nameOfTask, DateTime date, string plusOrMinus)
        {
            this.NameOfTaskProperty = nameOfTask;
            this.DateProperty = date.ToString();
            this.TypeOfDoProperty = plusOrMinus;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Дата действия над задачей
        /// </summary>
        public string DateProperty { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        public string NameOfTaskProperty { get; set; }

        /// <summary>
        /// Был нажат плюс или минус?
        /// </summary>
        public string TypeOfDoProperty { get; set; }

        /// <summary>
        /// Gets the when do property.
        /// </summary>
        public DateTime WhenDoProperty
        {
            get
            {
                return DateTime.Parse(this.DateProperty);
            }
        }

        #endregion
    }
}
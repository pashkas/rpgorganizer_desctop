// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskMessege.cs" company="">
//   
// </copyright>
// <summary>
//   Сообщение о добавлении/редактировании задач
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    /// <summary>
    /// Сообщение о добавлении/редактировании задач
    /// </summary>
    public class TaskMessege
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskMessege"/> class. 
        /// Конструктор для сообщения задачи
        /// </summary>
        /// <param name="task">
        /// Задача, которая будет добавляться/редактироваться
        /// </param>
        /// <param name="type">
        /// Тип задачи
        /// </param>
        /// <param name="tmodes">
        /// Редактирование/добавление задачи
        /// </param>
        /// <param name="pathToPic">
        /// </param>
        public TaskMessege(Task task, TypeOfTask type = null, byte[] img = null)
        {
            this.Task = task;
            this.Type = type;
            this.Image = img;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the path to pic.
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Задача, которая будет редактироваться/добавляться
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// Добавление/редактирование задачи
        /// </summary>
        public TaskEditModes Tmodes { get; set; }

        /// <summary>
        /// Тип задачи, которая будет добавляться/редактироваться
        /// </summary>
        public TypeOfTask Type { get; set; }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="">
//   
// </copyright>
// <summary>
//   Класс, который содержит в себе лог выполненных задач
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Класс, который содержит в себе лог выполненных задач
    /// </summary>
    [Serializable]
    public class Log
    {
        /// <summary>
        /// Когда был записан лог
        /// </summary>
        public string When { get; set; }

        /// <summary>
        /// Что записывается в лог
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Название элемента лога
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ид элемента лога
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Значение элемента
        /// </summary>
        public double Value { get; set; }
    }
}
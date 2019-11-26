// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeIntervals.cs" company="">
//   
// </copyright>
// <summary>
//   The time intervals.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    using System;

    /// <summary>
    /// The time intervals.
    /// </summary>
    [Serializable]
    public enum TimeIntervals
    {
        /// <summary>
        /// Не повторяется
        /// </summary>
        Нет,

        /// <summary>
        /// The сразу.
        /// </summary>
        Сразу,

        /// <summary>
        /// The день.
        /// </summary>
        День,

        /// <summary>
        /// Повтор через n дней после начала задачи
        /// </summary>
        ДниСначала,

        /// <summary>
        /// Повтор задачи по дням недели
        /// </summary>
        ДниНедели,

        /// <summary>
        /// Повтор по дням недели с начала задачи
        /// </summary>
        ДниНеделиСНачала,

        /// <summary>
        /// The неделя.
        /// </summary>
        Неделя,

        /// <summary>
        /// Повтор через n недель после начала
        /// </summary>
        НеделиСНачала,

        /// <summary>
        /// The месяц.
        /// </summary>
        Месяц,

        /// <summary>
        /// Повтор через n месяцев после начала
        /// </summary>
        МесяцыСНачала,

        Будни,

        Выходные,

        Ежедневно,

        Три,

        Четыре,

        Шесть
    }
}
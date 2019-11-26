using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    internal interface IRangable
    {
        /// <summary>
        /// Название текущего ранга
        /// </summary>
        string RangName { get; }

        /// <summary>
        /// Описание текущего ранга
        /// </summary>
        string RangDescription { get; }

        /// <summary>
        /// Название ранга начального уровня
        /// </summary>
        string FirstRangName { get; }

        /// <summary>
        /// Описание ранга начального уровня
        /// </summary>
        string FirstRangDescription { get; }

        /// <summary>
        /// Текущий ранг
        /// </summary>
        Rangs CurRang { get; }

        /// <summary>
        /// Начальный ранг
        /// </summary>
        Rangs FirstRang { get; set; }
    }
}
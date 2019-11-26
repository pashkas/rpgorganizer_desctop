using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    /// <summary>
    /// Запоминаем значения уровней характеристик и скиллов для разных уровней персонажа
    /// </summary>
    [Serializable]
    public class PersLevelsValues
    {
        /// <summary>
        /// Ид элемента
        /// </summary>
        public string ItemGuid { get; set; }

        /// <summary>
        /// Уровень элемента
        /// </summary>
        public int ItemLevel { get; set; }

        /// <summary>
        /// Уровень персонажа
        /// </summary>
        public int PersLevel { get; set; }
    }
}
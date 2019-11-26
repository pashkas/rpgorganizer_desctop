using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Model
{
    /// <summary>
    /// Элемент для словаря ссылок задач для главного окна.
    /// </summary>
    public class TaskLinkDictItem
    {
        /// <summary>
        /// Навык.
        /// </summary>
        public AbilitiModel Ability { get; set; }

        /// <summary>
        /// С какого уровня навыка доступна задача.
        /// </summary>
        public int AbilityLevelFrom { get; set; }

        /// <summary>
        /// По какой уровень навыка доступна задача.
        /// </summary>
        public int AbilityLevelTo { get; set; }

        /// <summary>
        /// Квест, если это задача и содержится в квесте.
        /// </summary>
        public Aim Qwest { get; set; }

        /// <summary>
        /// Ссылки на задачи навыков из квестов.
        /// </summary>
        public SortedSet<Task> Skills { get; set; } = new SortedSet<Task>();

        /// <summary>
        /// Задачи квестов - как ссылки.
        /// </summary>
        public SortedSet<Task> QwestTasks { get; set; } = new SortedSet<Task>();

        /// <summary>
        /// Есть скиллы? Прикреплено к навыкам?
        /// </summary>
        public bool HasSkills { get; set; }
    }
}

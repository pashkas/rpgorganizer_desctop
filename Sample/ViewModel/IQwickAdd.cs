using System.Collections.Generic;

namespace Sample.ViewModel
{
    public interface IQwickAdd
    {
        /// <summary>
        ///     Список задач для быстрого добавления задач
        /// </summary>
        List<QwickAdd> QwickAddTasksList { get; set; }

        void QwickAdd();
        void QwickAddElement(List<QwickAdd> qwickAddTasksList);
    }
}
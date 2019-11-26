using System.Collections;
using System.Collections.ObjectModel;
using Sample.Model;

namespace Sample.ViewModel
{
    public class TaskForTypesSorter : IComparer
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TaskForTypesSorter" /> class.
        /// </summary>
        /// <param name="tasksTypes">
        ///     The tasks types.
        /// </param>
        /// <param name="tasks">
        ///     The tasks.
        /// </param>
        public TaskForTypesSorter(ObservableCollection<TypeOfTask> tasksTypes, ObservableCollection<Task> tasks)
        {
            TasksTypes = tasksTypes;
            Tasks = tasks;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     The compare.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public int Compare(object x, object y)
        {
            var task1 = x as Task;
            var task2 = y as Task;
            var compareTo = task1.CompareTo(task2);
            return (int)compareTo;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Все задачи персонажа
        /// </summary>
        public ObservableCollection<Task> Tasks { get; set; }

        /// <summary>
        ///     Типы задач
        /// </summary>
        public ObservableCollection<TypeOfTask> TasksTypes { get; set; }

        #endregion Properties
    }
}
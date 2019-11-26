using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Sample.Model;

namespace Sample.ViewModel
{
    public interface IHaveTaskPanel
    {

        /// <summary>
        /// Передвинуть задачу в конец списка
        /// </summary>
        RelayCommand<Task> MoveTaskToEndOfListCommand { get; }

            /// <summary>
        /// Передвинуть задачу в начало списка
        /// </summary>
        RelayCommand<Task> MoveTaskToBeginOfListCommand { get; }

        /// <summary>
        /// Добавить новую задачу
        /// </summary>
        RelayCommand AddNewTask { get; }

            /// <summary>
        /// Выбранная задача
        /// </summary>
        Task SellectedTask { get; set; }

        /// <summary>
        /// Статусы задач
        /// </summary>
        List<StatusTask> Statuses { get; }

        /// <summary>
        ///     Gets the Редактирование задачи из альтернативного режима.
        /// </summary>
        RelayCommand<Task> AlterEditTaskCommand { get; }

        /// <summary>
        ///     Gets the Создать квест на основе задачи.
        /// </summary>
        RelayCommand<Task> TaskToQwestCommand { get; }

        /// <summary>
        ///     Gets the комманда альтернативное добавление задачи.
        /// </summary>
        RelayCommand AlternateAddTaskCommand { get; }

        /// <summary>
        ///     Gets the Альтернативное "Задача не сделана".
        /// </summary>
        RelayCommand<Task> AlternateMinusTaskCommand { get; }

        /// <summary>
        ///     Gets the Альтернативное сделание задачи.
        /// </summary>
        RelayCommand<Task> AlternatePlusTaskCommand { get; }

        /// <summary>
        ///     Gets the Удаление задачи из альтернативного режима.
        /// </summary>
        RelayCommand<Task> AlternateRemoveTaskCommand { get; }

        /// <summary>
        ///     Gets the Перенести задачу на завтра.
        /// </summary>
        RelayCommand<Task> SendTaskToTomorowCommand { get; }
    }
}
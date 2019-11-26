using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using GalaSoft.MvvmLight.Command;

    using Graphviz4Net.WPF.ViewModels;

    using Sample.Model;
    using Sample.View;

    /// <summary>
    /// Задачи для карты задач в главном окне
    /// </summary>
    public class MainViewTasksMapVM : TasksMapViewModele
    {
        /// <summary>
        /// Gets the Сделать квест посередине.
        /// </summary>
        private RelayCommand<EdgeLabelViewModel> addBetweenQwestCommand;

        /// <summary>
        /// Gets the добавить новую дочернюю задачу.
        /// </summary>
        private RelayCommand<TaskGraphItem> addNewChildTaskCommand;

        /// <summary>
        /// Gets the Добавить новую родительскую задачу.
        /// </summary>
        private RelayCommand<TaskGraphItem> addNewParrentTaskCommand;

        /// <summary>
        /// Комманда Добавить новую задачу.
        /// </summary>
        private RelayCommand addNewTaskCommand;

        public MainViewTasksMapVM()
        {
            this.PersProperty = StaticMetods.PersProperty;
            this.PathToGraphVizProperty = this.PersProperty.PersSettings.PathToGraphviz;
        }

        /// <summary>
        /// Gets the Добавить новую родительскую задачу.
        /// </summary>
        public new RelayCommand<TaskGraphItem> AddNewParrentTaskCommand
        {
            get
            {
                return this.addNewParrentTaskCommand
                       ?? (this.addNewParrentTaskCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<TaskGraphItem>(
                               (item) =>
                               {
                                   Task task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);

                                   var add = Task.AddTask(task.TaskType);

                                   if (add.Item1 == true)
                                   {
                                       task.NextActions.Add(add.Item2);

                                       this.MapUpdates();
                                   }
                               },
                               (item) =>
                               {
                                   if (item == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the добавить новую дочернюю задачу.
        /// </summary>
        public new RelayCommand<TaskGraphItem> AddNewChildTaskCommand
        {
            get
            {
                return this.addNewChildTaskCommand
                       ?? (this.addNewChildTaskCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<TaskGraphItem>(
                               (item) =>
                               {
                                   Task task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);

                                   var add = Task.AddTask(task.TaskType);

                                   if (add.Item1 == true)
                                   {
                                       add.Item2.NextActions.Add(task);

                                       this.MapUpdates();
                                   }
                               },
                               (item) =>
                               {
                                   if (item == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Задачи для карты задач главного окна
        /// </summary>
        public IEnumerable<Task> TasksForMap
        {
            get
            {
                return
                    this.PersProperty.Tasks.Where(
                        n =>
                            MainViewModel.IsTaskVisibleInCurrentView(
                                n,
                                SelectedViewProperty,
                                this.PersProperty,
                                true,
                                false));
            }
        }

        /// <summary>
        /// Gets the Сделать квест посередине.
        /// </summary>
        public new RelayCommand<EdgeLabelViewModel> AddBetweenQwestCommand
        {
            get
            {
                return this.addBetweenQwestCommand
                       ?? (this.addBetweenQwestCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<EdgeLabelViewModel>(
                               (item) =>
                               {
                                   var getTask =
                                       new Func<TaskGraphItem, Task>(
                                           graphItem =>
                                           {
                                               return this.PersProperty.Tasks.First(n => n.GUID == graphItem.Uid);
                                           });

                                   var parTask = getTask((TaskGraphItem)item.Edge.Destination);
                                   var chTask = getTask((TaskGraphItem)item.Edge.Source);

                                   var add = Task.AddTask(chTask.TaskType);
                                   if (add.Item1 == true)
                                   {
                                       var betweenTask = add.Item2;
                                       chTask.NextActions.Add(betweenTask);
                                       betweenTask.NextActions.Add(parTask);
                                       chTask.NextActions.Remove(parTask);

                                       this.MapUpdates();
                                   }
                               },
                               (item) =>
                               {
                                   if (item == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the комманда Добавить новую задачу.
        /// </summary>
        public new RelayCommand AddNewTaskCommand
        {
            get
            {
                return this.addNewTaskCommand
                       ?? (this.addNewTaskCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               Task.AddTask(this.PersProperty.TasksTypes.FirstOrDefault());
                               MapUpdates();
                           },
                           () => { return true; }));
            }
        }

        public new void MapUpdates()
        {
            this.buildGraph(TasksForMap.ToList());
        }
    }
}
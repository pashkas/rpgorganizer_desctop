// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QwestTaskMapViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The qwest task map view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Windows.Input;
    using System.Windows.Media;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Graphviz4Net.Graphs;

    using Sample.ViewModel;

    /// <summary>
    /// The qwest task map view model.
    /// </summary>
    public class QwestTaskMapViewModel : TasksMapViewModele
    {
        #region Public Methods and Operators

        public override void ShowMainElement(TaskGraphItem item)
        {
            Aim qwest = this.PersProperty.Aims.First(n => n.GUID == item.Uid);
            StaticMetods.editAim(qwest);
            PersProperty.SellectedAimProperty = qwest;
            SelectedAimProperty = PersProperty.SellectedAimProperty;
        }

        public override RelayCommand<TaskGraphItem> DeleteTaskCommand
        {
            get
            {
                return deleteTaskCommand
                       ?? (deleteTaskCommand = new GalaSoft.MvvmLight.Command.RelayCommand<TaskGraphItem>(
                           (item) =>
                           {
                               Task task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);
                               var needTask =
                                       StaticMetods.PersProperty.SellectedAimProperty.NeedsTasks.First(
                                           n => n.TaskProperty == task);

                               StaticMetods.PersProperty.SellectedAimProperty.DeleteTaskNeed(this.PersProperty, needTask);

                               this.MapUpdates();
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
        /// The map updates.
        /// </summary>
        public override void MapUpdates()
        {
            buildGraph();
        }

        public override void buildGraph(List<Task> _onlyThisTasks = null)
        {
            this.TasksGraphProperty = null;
            this.TasksGraphProperty = new Graph<TaskGraphItem>() { Rankdir = RankDirection.BottomToTop };

            var sellectedQwest = StaticMetods.PersProperty.SellectedAimProperty;

            // Добавляем квест
            TaskGraphItem taskGraphQwest = new TaskGraphItem()
                                           {
                                               Name = sellectedQwest.NameOfProperty,
                                               ImageProperty = sellectedQwest.ImageProperty,
                                               Type = "Квест",
                                               Uid = sellectedQwest.GUID,
                                               IsQwest = true,
                                               Color = Brushes.LimeGreen,
                                               BorderColor = Brushes.DarkSlateGray
                                           };

            this.TasksGraphProperty.AddVertex(taskGraphQwest);

            // Добавляем задачи к квесту
            var tasksWithoutParrents = (from needTaskse in sellectedQwest.NeedsTasks
                where
                    needTaskse.TaskProperty.NextActions.Any(
                        n => sellectedQwest.NeedsTasks.Any(q => q.TaskProperty == n)) == false
                select needTaskse).ToList();

            foreach (var tasksWithoutParrent in tasksWithoutParrents)
            {
                var taskGraphItem = GetTaskGraphItem(tasksWithoutParrent.TaskProperty);

                this.TasksGraphProperty.AddVertex(taskGraphItem);

                this.TasksGraphProperty.AddEdge(
                    new Edge<TaskGraphItem>(taskGraphItem, taskGraphQwest, new Arrow()) { Label = "+" });
            }

            var prevActionTasks =
                sellectedQwest.NeedsTasks.Except(tasksWithoutParrents);
            foreach (var prevTaske in prevActionTasks)
            {
                var taskGraphItem = GetTaskGraphItem(prevTaske.TaskProperty);

                this.TasksGraphProperty.AddVertex(taskGraphItem);
            }

            // Строим остальные связи
            this.buildLincs(
                this.PersProperty.Tasks,
                this.PersProperty.Characteristics,
                this.PersProperty.Abilitis,
                this.PersProperty.Aims,
                this.TasksGraphProperty);

            OnPropertyChanged(nameof(TasksGraphProperty));
        }

        #endregion
    }
}
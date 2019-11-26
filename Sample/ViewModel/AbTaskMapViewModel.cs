using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows.Input;
    using System.Windows.Media;

    using GalaSoft.MvvmLight.Command;

    using Graphviz4Net.Graphs;

    using Sample.Model;

    public class AbTaskMapViewModel : TasksMapViewModele
    {
        public AbTaskMapViewModel()
        {
            this.PersProperty = StaticMetods.PersProperty;
            this.PathToGraphVizProperty = this.PersProperty.PersSettings.PathToGraphviz;
            Thread.Sleep(500);
            this.buildGraph();
        }

        #region Public Methods and Operators

        public override void AddTaskToMainElement(UcTasksSettingsViewModel dataContext)
        {
            AbilitiModel ab = PersProperty.SellectedAbilityProperty;

            dataContext.AddNewTask(null);
            var need = QwestsViewModel.GetDefoultNeedTask(dataContext.SelectedTaskProperty);

            ab.NeedTasks.Add(need);
        }

        public override void ShowMainElement(TaskGraphItem item)
        {
            AbilitiModel ab = this.PersProperty.Abilitis.First(n => n.GUID == item.Uid);
            ab.EditAbility();
            this.PersProperty.SellectedAbilityProperty = ab;
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
                                       StaticMetods.PersProperty.SellectedAbilityProperty.NeedTasks.First(
                                           n => n.TaskProperty == task);


                               StaticMetods.PersProperty.SellectedAbilityProperty.DeleteNeedTaskCommand.Execute(needTask);

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

            var selAbility = StaticMetods.PersProperty.SellectedAbilityProperty;

            // Добавляем скилл
            TaskGraphItem taskGraphQwest = new TaskGraphItem()
                                           {
                                               Name = selAbility.NameOfProperty,
                                               ImageProperty = selAbility.ImageProperty,
                                               Type = "Квест",
                                               Uid = selAbility.GUID,
                                               IsQwest = true,
                                               Color = Brushes.LimeGreen,
                                               BorderColor = Brushes.DarkSlateGray
                                           };

            this.TasksGraphProperty.AddVertex(taskGraphQwest);

            // Добавляем задачи к скиллу
            var tasksWithoutParrents = (from needTaskse in selAbility.NeedTasks
                where
                    needTaskse.TaskProperty.NextActions.Any(n => selAbility.NeedTasks.Any(q => q.TaskProperty == n))
                    == false
                select needTaskse).ToList();

            foreach (var tasksWithoutParrent in tasksWithoutParrents)
            {
                var taskGraphItem = GetTaskGraphItem(tasksWithoutParrent.TaskProperty);

                this.TasksGraphProperty.AddVertex(taskGraphItem);

                this.TasksGraphProperty.AddEdge(
                    new Edge<TaskGraphItem>(taskGraphItem, taskGraphQwest, new Arrow()) { Label = "+" });
            }

            var prevActionTasks = selAbility.NeedTasks.Except(tasksWithoutParrents);
            foreach (var prevTaske in prevActionTasks)
            {
                var taskGraphItem = GetTaskGraphItem(prevTaske.TaskProperty);

                this.TasksGraphProperty.AddVertex(taskGraphItem);
            }

            // Строим остальные связи
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
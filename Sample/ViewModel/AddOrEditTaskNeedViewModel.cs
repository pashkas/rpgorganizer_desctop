using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.Model;

    public class AddOrEditTaskNeedViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Комманда Добавить новую задачу.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand addNewTaskCommand;

        /// <summary>
        /// Изображение для задач по умолчанию.
        /// </summary>
        private byte[] image;

        private Pers persProperty = StaticMetods.PersProperty;

        /// <summary>
        /// Выбранное требование.
        /// </summary>
        private NeedTasks sellectedNeed;

        public AddOrEditTaskNeedViewModel()
        {
            this.SellectedNeedProperty = new NeedTasks()
                                         {
                                             FirstValueProperty = 0,
                                             KoeficientProperty = 1,
                                             TaskProperty = persProperty.Tasks.FirstOrDefault(),
                                             TypeNeedProperty = ">=",
                                             ValueProperty = 1
                                         };
        }

        /// <summary>
        /// Sets and gets Изображение для задач по умолчанию.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte[] ImageProperty
        {
            get
            {
                return image;
            }

            set
            {
                if (image == value)
                {
                    return;
                }

                image = value;
                OnPropertyChanged(nameof(ImageProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Добавить новую задачу.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand AddNewTaskCommand
        {
            get
            {
                return addNewTaskCommand ?? (addNewTaskCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                    () =>
                    {
                        var task = Task.AddTask(persProperty.TasksTypes.FirstOrDefault());
                        OnPropertyChanged(nameof(AllTasks));
                        this.SellectedNeedProperty.TaskProperty = task.Item2;
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Выбранное требование.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public NeedTasks SellectedNeedProperty
        {
            get
            {
                return sellectedNeed;
            }

            set
            {
                if (sellectedNeed == value)
                {
                    return;
                }

                sellectedNeed = value;
                OnPropertyChanged(nameof(SellectedNeedProperty));
            }
        }

        /// <summary>
        /// Только скилы при выборе?
        /// </summary>
        public bool IsOnlySkiils { get; set; } = false;

        /// <summary>
        /// Все задачи
        /// </summary>
        public IEnumerable<Task> AllTasks
        {
            get
            {
                var orderedEnumerable = persProperty.Tasks.OrderBy(n => n.NameOfProperty);



                return IsOnlySkiils? orderedEnumerable.Where(n=>n.Recurrense.TypeInterval!=TimeIntervals.Нет):orderedEnumerable;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucContextSettingsViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Класс вью модель для юзерконтрола настройки контекстов
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// Класс вью модель для юзерконтрола настройки контекстов
    /// </summary>
    public class ucContextSettingsViewModel : INotifyPropertyChanged
    {
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

        #region Enums

        /// <summary>
        /// Типы для редактирования задач
        /// </summary>
        private enum editType
        {
            /// <summary>
            /// The добавление.
            /// </summary>
            Добавление,

            /// <summary>
            /// The редактирование.
            /// </summary>
            Редактирование
        }

        #endregion

        #region Fields

        /// <summary>
        /// The edit mode.
        /// </summary>
        private ucStatusesSettingsViewModel.editType EditMode;

        /// <summary>
        /// Комманда добавление задачи.
        /// </summary>
        private RelayCommand adContextCommand;

        /// <summary>
        /// Комманда Удалить статус.
        /// </summary>
        private RelayCommand delContextCommand;

        /// <summary>
        /// Комманда Команда на редактирование статуса.
        /// </summary>
        private RelayCommand editContextCommand;

        /// <summary>
        /// Открыто ли окно с редактированием названий статусов?.
        /// </summary>
        private bool isPopupOpen;

        /// <summary>
        /// Комманда Сдвинуть статус вниз.
        /// </summary>
        private RelayCommand moveDownCommand;

        /// <summary>
        /// Комманда Сдвинуть статус вверх.
        /// </summary>
        private RelayCommand moveUpCommand;

        /// <summary>
        /// Комманда Ок в редактировании статуса.
        /// </summary>
        private RelayCommand okCommand;

        /// <summary>
        /// Выбранная задача.
        /// </summary>
        private Context selectedContext;

        /// <summary>
        /// Задачи.
        /// </summary>
        private ObservableCollection<Task> tasks;

        /// <summary>
        /// Статусы задач.
        /// </summary>
        private ObservableCollection<Context> tasksContextCollection;

        /// <summary>
        /// Типы задач.
        /// </summary>
        private ObservableCollection<TypeOfTask> typesOfTaskCollection;

        /// <summary>
        /// Виды задач.
        /// </summary>
        private ObservableCollection<ViewsModel> viewsCollection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ucContextSettingsViewModel"/> class. 
        /// Initializes a new instance of the <see cref="ucStatusesSettingsViewModel"/> class.
        /// </summary>
        public ucContextSettingsViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ucContextSettingsViewModel"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="pTasks">
        /// The p tasks.
        /// </param>
        /// <param name="ptTypes">
        /// The pt Types.
        /// </param>
        /// <param name="pViews">
        /// </param>
        public ucContextSettingsViewModel(
            ObservableCollection<Context> context,
            ObservableCollection<Task> pTasks,
            ObservableCollection<TypeOfTask> ptTypes,
            ObservableCollection<ViewsModel> pViews)
        {
            this.TasksContextCollectionProperty = context;
            this.TasksProperty = pTasks;
            this.TypesOfTaskCollectionProperty = ptTypes;
            this.ViewsCollectionProperty = pViews;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the комманда добавление контекста.
        /// </summary>
        public RelayCommand AdContextCommand
        {
            get
            {
                return this.adContextCommand
                       ?? (this.adContextCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               this.EditMode = ucStatusesSettingsViewModel.editType.Добавление;
                               this.SelectedContextProperty = new Context()
                                                              {
                                                                  NameOfContext = "Контекст",
                                                                  Uid = Guid.NewGuid().ToString()
                                                              };

                               this.IsPopupOpenProperty = true;
                           },
                           () => true));
            }
        }

        /// <summary>
        /// Gets the комманда Удалить контекст.
        /// </summary>
        public RelayCommand DelContextCommand
        {
            get
            {
                return this.delContextCommand
                       ?? (this.delContextCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               // Задаем задачам контекст - самый первый
                               foreach (Task task in
                                   this.TasksProperty.Where(n => n.TaskContext == this.SelectedContextProperty))
                               {
                                   task.TaskContext = this.TasksContextCollectionProperty.First();
                               }

                               // Задаем типам задач контекст - самый первый
                               foreach (TypeOfTask typeOfTask in
                                   this.TypesOfTaskCollectionProperty.Where(
                                       n => n.ContextForDefoult == this.SelectedContextProperty))
                               {
                                   typeOfTask.ContextForDefoult = this.TasksContextCollectionProperty.First();
                               }

                               // Удаляем данный столбец из видов
                               foreach (var views in this.ViewsCollectionProperty)
                               {
                                   foreach (var cont in
                                       views.ViewContextsOfTasks.Where(
                                           contexts => contexts.taskContext == this.SelectedContextProperty).ToList())
                                   {
                                       views.ViewContextsOfTasks.Remove(cont);
                                   }
                               }

                               this.TasksContextCollectionProperty.Remove(this.SelectedContextProperty);
                           },
                           () =>
                           {
                               if (this.SelectedContextProperty == null)
                               {
                                   return false;
                               }
                               if (TasksContextCollectionProperty.Count<=1)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Gets the комманда Команда на редактирование статуса.
        /// </summary>
        public RelayCommand EditContextCommand
        {
            get
            {
                return this.editContextCommand
                       ?? (this.editContextCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               this.EditMode = ucStatusesSettingsViewModel.editType.Редактирование;
                               this.IsPopupOpenProperty = true;
                           },
                           () =>
                           {
                               if (this.SelectedContextProperty == null)
                               {
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }));
            }
        }

        /// <summary>
        /// Sets and gets Открыто ли окно с редактированием названий статусов?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPopupOpenProperty
        {
            get
            {
                return this.isPopupOpen;
            }

            set
            {
                if (this.isPopupOpen == value)
                {
                    return;
                }

                this.isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpenProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Сдвинуть статус вниз.
        /// </summary>
        public RelayCommand MoveDownCommand
        {
            get
            {
                return this.moveDownCommand
                       ?? (this.moveDownCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   this.TasksContextCollectionProperty.Move(
                                       this.TasksContextCollectionProperty.IndexOf(this.SelectedContextProperty),
                                       this.TasksContextCollectionProperty.IndexOf(this.SelectedContextProperty) + 1);
                               },
                               () =>
                               {
                                   if (this.SelectedContextProperty == null
                                       || this.TasksContextCollectionProperty == null)
                                   {
                                       return false;
                                   }
                                   else
                                   {
                                       
                                       if (this.TasksContextCollectionProperty.IndexOf(this.SelectedContextProperty) + 1
                                           >= this.TasksContextCollectionProperty.Count)
                                       {
                                           return false;
                                       }
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the комманда Сдвинуть статус вверх.
        /// </summary>
        public RelayCommand MoveUpCommand
        {
            get
            {
                return this.moveUpCommand
                       ?? (this.moveUpCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   this.TasksContextCollectionProperty.Move(
                                       this.TasksContextCollectionProperty.IndexOf(this.SelectedContextProperty),
                                       this.TasksContextCollectionProperty.IndexOf(this.SelectedContextProperty) - 1);
                               },
                               () =>
                               {
                                   if (this.SelectedContextProperty == null
                                       || this.TasksContextCollectionProperty == null)
                                   {
                                       return false;
                                   }

                                   if (this.TasksContextCollectionProperty.IndexOf(this.SelectedContextProperty)
                                           <= 0)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the комманда Ок в редактировании статуса.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return this.okCommand ?? (this.okCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                    () =>
                    {
                        if (this.EditMode == ucStatusesSettingsViewModel.editType.Редактирование)
                        {
                            this.IsPopupOpenProperty = false;
                        }

                        if (this.EditMode == ucStatusesSettingsViewModel.editType.Добавление)
                        {
                            this.TasksContextCollectionProperty.Add(this.SelectedContextProperty);

                            // Добавляем во все виды
                            foreach (var views in this.ViewsCollectionProperty)
                            {
                                views.ViewContextsOfTasks.Add(
                                    new ViewVisibleContexts()
                                    {
                                        isVisible = false,
                                        taskContext = this.SelectedContextProperty
                                    });
                            }

                            this.IsPopupOpenProperty = false;
                        }
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Выбранная задача.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Context SelectedContextProperty
        {
            get
            {
                return this.selectedContext;
            }

            set
            {
                if (this.selectedContext == value)
                {
                    return;
                }

                this.selectedContext = value;
                OnPropertyChanged(nameof(SelectedContextProperty));
                OnPropertyChanged(nameof(SelectedContextTextProperty));
            }
        }

        /// <summary>
        /// Gets or sets Название выбранного статуса.
        /// </summary>
        public string SelectedContextTextProperty
        {
            get
            {
                if (this.SelectedContextProperty != null)
                {
                    return this.SelectedContextProperty.NameOfContext;
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                if (this.SelectedContextTextProperty == value)
                {
                    return;
                }

                this.SelectedContextProperty.NameOfContext = value;
                OnPropertyChanged(nameof(SelectedContextTextProperty));
            }
        }

        /// <summary>
        /// Sets and gets Статусы задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Context> TasksContextCollectionProperty
        {
            get
            {
                return this.tasksContextCollection;
            }

            set
            {
                if (this.tasksContextCollection == value)
                {
                    return;
                }

                this.tasksContextCollection = value;
                OnPropertyChanged(nameof(TasksContextCollectionProperty));
            }
        }

        /// <summary>
        /// Sets and gets Задачи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Task> TasksProperty
        {
            get
            {
                return this.tasks;
            }

            set
            {
                if (this.tasks == value)
                {
                    return;
                }

                this.tasks = value;
                OnPropertyChanged(nameof(TasksProperty));
            }
        }

        /// <summary>
        /// Sets and gets Типы задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<TypeOfTask> TypesOfTaskCollectionProperty
        {
            get
            {
                return this.typesOfTaskCollection;
            }

            set
            {
                if (this.typesOfTaskCollection == value)
                {
                    return;
                }

                this.typesOfTaskCollection = value;
                OnPropertyChanged(nameof(TypesOfTaskCollectionProperty));
            }
        }

        /// <summary>
        /// Sets and gets Виды задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ViewsModel> ViewsCollectionProperty
        {
            get
            {
                return this.viewsCollection;
            }

            set
            {
                if (this.viewsCollection == value)
                {
                    return;
                }

                this.viewsCollection = value;
                OnPropertyChanged(nameof(ViewsCollectionProperty));
            }
        }

        #endregion
    }
}
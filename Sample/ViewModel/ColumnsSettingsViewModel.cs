// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnsSettingsViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The columns settings view model.
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

    using Sample.Model;

    /// <summary>
    /// The columns settings view model.
    /// </summary>
    internal class ColumnsSettingsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     The pers.
        /// </summary>
        public Pers Pers
        {
            get { return StaticMetods.PersProperty; }
            set
            {
                StaticMetods.PersProperty = value;
                OnPropertyChanged(nameof(Pers));
            }
        }

        #region Public Events

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Комманда Добавить тип.
        /// </summary>
        private RelayCommand beginAddTypeCommand;

        /// <summary>
        /// Вью модель для юзерконтролла с контекстами.
        /// </summary>
        private ucContextSettingsViewModel contSetVM;

        /// <summary>
        /// Выделенный тип задач
        /// </summary>
        private TypeOfTask currentTaskType;

        /// <summary>
        /// Открыто окно с добавлением колонки
        /// </summary>
        private bool isOpenAddNew;

        /// <summary>
        /// Новый тип задач
        /// </summary>
        /// <returns></returns>
        private TypeOfTask newTaskType;

        /// <summary>
        /// Комманда Ок в редактировании колонки.
        /// </summary>
        private RelayCommand okEditColumnCommand;

        /// <summary>
        /// Вью модель для юзерконтрола статусы.
        /// </summary>
        private ucStatusesSettingsViewModel statSetVM;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsSettingsViewModel"/> class.
        /// </summary>
        public ColumnsSettingsViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsSettingsViewModel"/> class. 
        /// Конструктор
        /// </summary>
        /// <param name="pers">
        /// The pers.
        /// </param>
        public ColumnsSettingsViewModel(Pers pers)
        {
            this.pers = pers;

            this.NewTaskType = new TypeOfTask()
                               {
                                   NameOfTypeOfTask = "Новый тип",
                                   IntervalForDefoult = TimeIntervals.Нет,
                                   ContextForDefoult = pers.Contexts.FirstOrDefault(),
                                   StatusForDefoult = pers.Statuses.FirstOrDefault()
                               };

            // Заполняем интервалы времени
            this.IntervalsOfTime = StaticMetods.GetRepeatIntervals();

            this.StatSetVMProperty = new ucStatusesSettingsViewModel(
                pers.Statuses,
                pers.Tasks,
                pers.TasksTypes,
                pers.Views);
            this.ContSetVMProperty = new ucContextSettingsViewModel(
                pers.Contexts,
                pers.Tasks,
                pers.TasksTypes,
                pers.Views);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the комманда Добавить тип.
        /// </summary>
        public RelayCommand BeginAddTypeCommand
        {
            get
            {
                return this.beginAddTypeCommand
                       ?? (this.beginAddTypeCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   this.NewTaskType = new TypeOfTask()
                                                      {
                                                          NameOfTypeOfTask = "Новый тип",
                                                          IntervalForDefoult = TimeIntervals.Нет,
                                                          ContextForDefoult = this.pers.Contexts.First(),
                                                          StatusForDefoult = this.pers.Statuses.First()
                                                      };
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Вью модель для юзерконтролла с контекстами.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ucContextSettingsViewModel ContSetVMProperty
        {
            get
            {
                return this.contSetVM;
            }

            set
            {
                if (this.contSetVM == value)
                {
                    return;
                }

                this.contSetVM = value;
                OnPropertyChanged("ContSetVMProperty");
            }
        }

        /// <summary>
        /// Gets or sets Выделенный тип задач
        /// </summary>
        public TypeOfTask CurrentTaskType
        {
            get
            {
                return this.currentTaskType;
            }

            set
            {
                this.currentTaskType = value;
                this.OnPropertyChanged("CurrentTaskType");
            }
        }

        /// <summary>
        /// Gets or sets the intervals of time.
        /// </summary>
        public ObservableCollection<IntervalsModel> IntervalsOfTime { get; set; }

        /// <summary>
        /// Открыто окно с добавлением колонки
        /// </summary>
        public bool IsOpenAddNew
        {
            get
            {
                return this.isOpenAddNew;
            }

            set
            {
                this.isOpenAddNew = value;
                this.OnPropertyChanged("IsOpenAddNew");
            }
        }

        /// <summary>
        /// Новый тип задач
        /// </summary>
        /// <returns></returns>
        public TypeOfTask NewTaskType
        {
            get
            {
                return this.newTaskType;
            }

            set
            {
                this.newTaskType = value;
                this.OnPropertyChanged("NewTaskType");
            }
        }

        /// <summary>
        /// Gets the комманда Ок в редактировании колонки.
        /// </summary>
        public RelayCommand OkEditColumnCommand
        {
            get
            {
                return this.okEditColumnCommand
                       ?? (this.okEditColumnCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   this.OnPropertyChanged(nameof(TasksTypes));
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Вью модель для юзерконтрола статусы.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ucStatusesSettingsViewModel StatSetVMProperty
        {
            get
            {
                return this.statSetVM;
            }

            set
            {
                if (this.statSetVM == value)
                {
                    return;
                }

                this.statSetVM = value;
                OnPropertyChanged("StatSetVMProperty");
            }
        }

        /// <summary>
        /// Колонки с задачами
        /// </summary>
        public List<TypeOfTask> TasksTypes
        {
            get
            {
                return Pers.TasksTypes.ToList();
            }
        }

        /// <summary>
        /// The pers.
        /// </summary>
        public Pers pers { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Добавление нового типа
        /// </summary>
        public void AddNew()
        {
            this.pers.TasksTypes.Add(this.NewTaskType);

            // Добавляем во все виды
            //foreach (var views in this.pers.Views)
            //{
            //    views.ViewTypesOfTasks.Add(new ViewVisibleTypes() { isVisible = false, taskType = this.NewTaskType });
            //}

            this.NewTaskType = new TypeOfTask()
                               {
                                   NameOfTypeOfTask = "Новый тип",
                                   IntervalForDefoult = TimeIntervals.Нет
                               };
            this.OnPropertyChanged("TasksTypes");
        }

        /// <summary>
        /// Комманда Удалить колонку задач.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand deleteCommand;

        /// <summary>
        /// Gets the комманда Удалить колонку задач.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                    Delete,
                    () =>
                    {
                        if (StaticMetods.PersProperty.TasksTypes.Count <= 1)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }

        /// <summary>
        /// Комманда Редактировать колонку - тип задач.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand editColumnCommand;

        /// <summary>
        /// Gets the комманда Редактировать колонку - тип задач.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand EditColumnCommand
        {
            get
            {
                return editColumnCommand
                       ?? (editColumnCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(() => { }, () => { return true; }));
            }
        }

        /// <summary>
        /// Удаление выбранной колонки
        /// </summary>
        public void Delete()
        {
            var firstTaskType = pers.TasksTypes.FirstOrDefault();

            // Для скиллов и квестов меняем типы задач, если надо
            if (Pers.PersSettings.DefoultTaskTypeForAbills == CurrentTaskType)
            {
                Pers.PersSettings.DefoultTaskTypeForAbills = firstTaskType;
            }

            if (Pers.PersSettings.DefoultTaskTypeForQwests == CurrentTaskType)
            {
                Pers.PersSettings.DefoultTaskTypeForQwests = firstTaskType;
            }


            // Задаем задачам данного столбца самый первый столбец
            foreach (var tasksToRemove in this.pers.Tasks.Where(n => n.TaskType == this.currentTaskType).ToList())
            {
                tasksToRemove.TaskType = firstTaskType;
            }

            // Удаляем данный столбец из видов
            foreach (var views in this.pers.Views)
            {
                foreach (var typeOfTask in
                    views.ViewTypesOfTasks.Where(typeOfTask => typeOfTask.taskType == this.currentTaskType).ToList())
                {
                    views.ViewTypesOfTasks.Remove(typeOfTask);
                }
            }

            // Удаляем тип задач
            this.pers.TasksTypes.Remove(this.CurrentTaskType);
            this.CurrentTaskType = null;
            this.OnPropertyChanged("TasksTypes");
        }

        /// <summary>
        /// Сместить вниз
        /// </summary>
        public void MoveDown()
        {
            int old = this.TasksTypes.IndexOf(this.CurrentTaskType);
            int ne = old + 1;
            if (this.TasksTypes.Count > ne && this.CurrentTaskType != null)
            {
                this.pers.TasksTypes.Move(old, ne);

                // Двигаем в видах
                foreach (var views in this.pers.Views)
                {
                    views.ViewTypesOfTasks.Move(old, ne);
                }
            }

            this.OnPropertyChanged("TasksTypes");
        }

        /// <summary>
        /// Сместить колонку вверх
        /// </summary>
        public void MoveUp()
        {
            int old = this.TasksTypes.IndexOf(this.CurrentTaskType);
            int ne = old - 1;
            if (ne >= 0 && this.CurrentTaskType != null)
            {
                this.pers.TasksTypes.Move(old, ne);

                // Двигаем в видах
                foreach (var views in this.pers.Views)
                {
                    views.ViewTypesOfTasks.Move(old, ne);
                }
            }

            this.OnPropertyChanged("TasksTypes");
        }

        #endregion
    }
}
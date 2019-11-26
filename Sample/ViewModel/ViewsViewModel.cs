// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewsViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The views view model.
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
    /// The views view model.
    /// </summary>
    public class ViewsViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsViewModel"/> class.
        /// </summary>
        /// <param name="views">
        /// The views.
        /// </param>
        /// <param name="persTaskTypes">
        /// The pers task types.
        /// </param>
        /// <param name="pStatuses">
        /// The p Statuses.
        /// </param>
        /// <param name="pContexts">
        /// The p Contexts.
        /// </param>
        public ViewsViewModel(
            ObservableCollection<ViewsModel> views,
            ObservableCollection<TypeOfTask> persTaskTypes,
            ObservableCollection<StatusTask> pStatuses,
            ObservableCollection<Context> pContexts)
        {
            this.PersTaskTypes = persTaskTypes;
            this.StatusesCollectionProperty = pStatuses;
            this.ContextsCollectionProperty = pContexts.OrderBy(n=>StaticMetods.PersProperty.Contexts.IndexOf(n));

            this.SelectedView = new ViewsModel();
            this.CloseSignal = false;
            this.OnPropertyChanged("CloseSignal");
        }

        /// <summary>
        ///     The pers.
        /// </summary>
        public Pers Pers
        {
            get
            {
                return StaticMetods.PersProperty;
            }
            set
            {
                StaticMetods.PersProperty = value;
                OnPropertyChanged(nameof(Pers));
            }
        }

        #endregion

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
        /// Контексты для задач.
        /// </summary>
        private IEnumerable<Context> contextsCollection;

        /// <summary>
        /// Сдвинуть вид вверх.
        /// </summary>
        private RelayCommand moveDownCommand;

        /// <summary>
        /// Сдвинуть вверх.
        /// </summary>
        private RelayCommand moveUpCommand;

        /// <summary>
        /// The selected view.
        /// </summary>
        private ViewsModel selectedView;

        /// <summary>
        /// Статусы задач.
        /// </summary>
        private ObservableCollection<StatusTask> statusesCollection;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether close signal.
        /// </summary>
        public bool CloseSignal { get; set; }

        /// <summary>
        /// Sets and gets Контексты для задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IEnumerable<Context> ContextsCollectionProperty
        {
            get
            {
                return this.contextsCollection;
            }

            set
            {
                if (this.contextsCollection == value)
                {
                    return;
                }

                this.contextsCollection = value;
                OnPropertyChanged("ContextsCollectionProperty");
            }
        }

        /// <summary>
        /// Gets the Сдвинуть вид вверх.
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
                                   this.Views.Move(
                                       this.Views.IndexOf(this.SelectedView),
                                       this.Views.IndexOf(this.SelectedView) + 1);
                               },
                               () =>
                               {
                                   if (this.SelectedView == null
                                       || this.Views.IndexOf(this.SelectedView) > this.Views.Count)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the Сдвинуть вверх.
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
                                   this.Views.Move(
                                       this.Views.IndexOf(this.SelectedView),
                                       this.Views.IndexOf(this.SelectedView) - 1);
                               },
                               () =>
                               {
                                   if (this.SelectedView == null || this.Views.IndexOf(this.SelectedView) - 1 < 0)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets or sets the new view.
        /// </summary>
        public ViewsModel NewView { get; set; }

        /// <summary>
        /// Типы задач персонажа
        /// </summary>
        public ObservableCollection<TypeOfTask> PersTaskTypes { get; set; }

        /// <summary>
        /// Выбранный вид
        /// </summary>
        public ViewsModel SelectedView
        {
            get
            {
                return this.selectedView;
            }

            set
            {
                this.selectedView = value;
                this.OnPropertyChanged("SelectedView");
            }
        }

        /// <summary>
        /// Sets and gets Статусы задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<StatusTask> StatusesCollectionProperty
        {
            get
            {
                return this.statusesCollection;
            }

            set
            {
                if (this.statusesCollection == value)
                {
                    return;
                }

                this.statusesCollection = value;
                OnPropertyChanged("StatusesCollectionProperty");
            }
        }

        public void OkEditView()
        {
            SelectedView.OnPropertyChanged(nameof(ViewsModel.NameOfView));
            OnPropertyChanged(nameof(Views));
        }

        /// <summary>
        /// Перечень доступных видов
        /// </summary>
        public ObservableCollection<ViewsModel> Views {
            get { return Pers.Views; } }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get new view.
        /// </summary>
        /// <param name="_taskTypes">
        /// </param>
        /// <param name="_statuses">
        /// </param>
        /// <param name="_contexts">
        /// </param>
        /// <returns>
        /// The <see cref="ViewsModel"/>.
        /// </returns>
        public static ViewsModel GetNewView(
            ObservableCollection<TypeOfTask> _taskTypes,
            ObservableCollection<StatusTask> _statuses,
            IEnumerable<Context> _contexts)
        {
            var nv = new ViewsModel
                     {
                         GUID = Guid.NewGuid().ToString(),
                         NameOfView = "Название нового вида",
                         ViewTypesOfTasks = new ObservableCollection<ViewVisibleTypes>(),
                         ViewStatusOfTasks = new ObservableCollection<ViewVisibleStatuses>(),
                         ViewContextsOfTasks = new ObservableCollection<ViewVisibleContexts>()
                     };

            // Добавляем к виду типы задач
            foreach (var persTaskType in _taskTypes)
            {
                nv.ViewTypesOfTasks.Add(new ViewVisibleTypes() { taskType = persTaskType, isVisible = false });
            }

            // Добавляем к виду статусы
            foreach (var statusTask in _statuses)
            {
                nv.ViewStatusOfTasks.Add(new ViewVisibleStatuses() { taskStatus = statusTask, isVisible = false });
            }

            // Добавляем к виду контексты
            foreach (var context in _contexts)
            {
                nv.ViewContextsOfTasks.Add(new ViewVisibleContexts() { taskContext = context, isVisible = false });
            }

            return nv;
        }

        /// <summary>
        /// Получение нового вида перед добавлением нового вида
        /// </summary>
        public void BeforeAddNiewView()
        {
            this.NewView = GetNewView(
                this.PersTaskTypes,
                this.StatusesCollectionProperty,
                this.ContextsCollectionProperty);
            this.OnPropertyChanged("NewView");
        }

        /// <summary>
        /// The close.
        /// </summary>
        public void Close()
        {
            this.CloseSignal = true;
            this.OnPropertyChanged("CloseSignal");
        }

        /// <summary>
        /// Удаление вида
        /// </summary>
        public void Delete()
        {
            this.Views.Remove(this.SelectedView);
        }

        /// <summary>
        /// The ok add view.
        /// </summary>
        public void OkAddView()
        {
            this.Views.Add(this.NewView);
        }

        #endregion
    }
}
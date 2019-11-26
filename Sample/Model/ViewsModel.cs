// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewsModel.cs" company="">
//   
// </copyright>
// <summary>
//   Класс, в котором содержатся виды задач
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using Sample.Annotations;

    /// <summary>
    /// Класс, в котором содержатся виды задач
    /// </summary>
    [Serializable]
    public class ViewsModel : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Public Properties

        /// <summary>
        /// Минимальный индекс в задачах
        /// </summary>
        public int MinTskInd { get; set; }


        /// <summary>
        /// Локация видна? В смысле у нее есть задачи - тогда видна!
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }


        /// <summary>
        /// Ид вида
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// Gets or sets the name of view.
        /// </summary>
        public string NameOfView { get; set; }

        /// <summary>
        /// Gets or sets Видимые контексты.
        /// </summary>
        public ObservableCollection<ViewVisibleContexts> ViewContextsOfTasks { get; set; }

        /// <summary>
        /// Gets or sets Видимые статусы.
        /// </summary>
        public ObservableCollection<ViewVisibleStatuses> ViewStatusOfTasks { get; set; }


        public List<ViewVisibleStatuses> VisStatuses
        {
            get
            {
                if (ViewStatusOfTasks == null)
                {
                    return new List<ViewVisibleStatuses>();
                }

                var viewVisibleStatuseses = ViewStatusOfTasks.ToList();

                return
                    viewVisibleStatuseses;
            }
        }

        /// <summary>
        /// Gets or sets the view types of tasks.
        /// </summary>
        public ObservableCollection<ViewVisibleTypes> ViewTypesOfTasks { get; set; }

        /// <summary>
        /// Отображение задач по умолчанию.
        /// </summary>
        private int defoultTaskView;

        private bool _isVisible;

        /// <summary>
        /// Sets and gets Отображение задач по умолчанию.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DefoultTaskViewProperty
        {
            get
            {
                return defoultTaskView;
            }

            set
            {
                if (defoultTaskView == value)
                {
                    return;
                }

                defoultTaskView = value;
                OnPropertyChanged(nameof(DefoultTaskViewProperty));
            }
        }

        #endregion
    }

    /// <summary>
    /// The view visible types.
    /// </summary>
    [Serializable]
    public class ViewVisibleTypes
    {
        #region Public Properties

        /// <summary>
        /// Виден ли этот тип задачи в виде
        /// </summary>
        public bool isVisible { get; set; }

        /// <summary>
        /// Тип задачи
        /// </summary>
        public TypeOfTask taskType { get; set; }

        #endregion
    }

    /// <summary>
    /// The view Статусы отображаемые в этом виде.
    /// </summary>
    [Serializable]
    public class ViewVisibleStatuses
    {
        #region Public Properties

        /// <summary>
        /// Виден ли этот тип задачи в виде
        /// </summary>
        public bool isVisible { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public StatusTask taskStatus { get; set; }


        #endregion
    }

    /// <summary>
    /// The view Контексты отображаемые в этом виде.
    /// </summary>
    [Serializable]
    public class ViewVisibleContexts
    {
        #region Public Properties

        /// <summary>
        /// Виден ли этот тип задачи в виде
        /// </summary>
        public bool isVisible { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public Context taskContext { get; set; }

        #endregion
    }
}
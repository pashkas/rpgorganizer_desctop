// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatisticViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Вью модель для показа статистики по задачам
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
    using System.Windows;
    using System.Windows.Data;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.View;

    /// <summary>
    /// Вью модель для показа статистики по задачам
    /// </summary>
    public class StatisticViewModel : INotifyPropertyChanged
    {
        #region Public Properties

        /// <summary>
        /// Задачи персонажа
        /// </summary>
        public ListCollectionView Tasks { get; set; }

        #endregion

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

        #region Fields

        /// <summary>
        /// Отображение статистики.
        /// </summary>
        private List<Tuple<string, int, int>> LogsView = new List<Tuple<string, int, int>>();

        /// <summary>
        /// Таблица с логами.
        /// </summary>
        private ObservableCollection<Log> logs;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticViewModel"/> class.
        /// </summary>
        public StatisticViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticViewModel"/> class.
        /// </summary>
        /// <param name="tasks">
        /// Задачи персонажа.
        /// </param>
        public StatisticViewModel(ObservableCollection<Task> tasks)
        {
            this.Tasks = (ListCollectionView)new CollectionViewSource { Source = tasks }.View;
            this.Tasks.GroupDescriptions.Clear();
            this.Tasks.GroupDescriptions.Add(new PropertyGroupDescription("TaskType.NameOfTypeOfTask"));
        }

        #endregion
    }
}
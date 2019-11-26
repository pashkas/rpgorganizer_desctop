// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CorrectRelaysAbilityViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The correct relays ability view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.Properties;

    /// <summary>
    /// The correct relays ability view model.
    /// </summary>
    public class CorrectRelaysAbilityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Public Methods and Operators

        /// <summary>
        /// Заполняем связанные задачи
        /// </summary>
        /// <param name="linkedTasks">
        /// </param>
        /// <param name="_pers">
        /// The _pers.
        /// </param>
        public void pullLinkedTasks(IEnumerable<RelTaskToAb> linkedTasks, Pers _pers)
        {
            this.TaskCountsProperty = new List<TaskAndCounts>();
            foreach (var relTaskToAb in linkedTasks)
            {
                this.TaskCountsProperty.Add(
                    new TaskAndCounts()
                    {
                        TaskProperty = relTaskToAb.TaskProperty,
                        CountProperty = _pers.PersSettings.TaskDoToMaxAbilityProperty
                    });
            }

            this.LevelToPerfectProperty = Settings.Default.MaxAbilLevel;

            this.RelayQwestsProperty = null;

            OnPropertyChanged(nameof(TaskCountsProperty));
        }

        #endregion

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
        /// Комманда Корректировка влияний.
        /// </summary>
        private RelayCommand correctRelaysCommand;

        /// <summary>
        /// Уровень до совершенства.
        /// </summary>
        private int levelToPerfect;

        /// <summary>
        /// Влияющие квесты.
        /// </summary>
        private List<QwestRelayAbil> relayQwests;

        /// <summary>
        /// Выбранный скилл, для которого корректируются значения.
        /// </summary>
        private AbilitiModel selectedAbility;

        /// <summary>
        /// Лист задач и сколько их раз нужно сделать.
        /// </summary>
        private List<TaskAndCounts> taskCounts;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the комманда Корректировка влияний.
        /// </summary>
        public RelayCommand CorrectRelaysCommand
        {
            get
            {
                return this.correctRelaysCommand
                       ?? (this.correctRelaysCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(() => { }, () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Уровень до совершенства.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LevelToPerfectProperty
        {
            get
            {
                return this.levelToPerfect;
            }

            set
            {
                if (this.levelToPerfect == value)
                {
                    return;
                }

                this.levelToPerfect = value;
                OnPropertyChanged(nameof(LevelToPerfectProperty));
            }
        }

        /// <summary>
        /// Sets and gets Влияющие квесты.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<QwestRelayAbil> RelayQwestsProperty
        {
            get
            {
                return this.relayQwests;
            }

            set
            {
                if (this.relayQwests == value)
                {
                    return;
                }

                this.relayQwests = value;
                OnPropertyChanged(nameof(RelayQwestsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранный скилл, для которого корректируются значения.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AbilitiModel SelectedAbilityProperty
        {
            get
            {
                return this.selectedAbility;
            }

            set
            {
                if (this.selectedAbility == value)
                {
                    return;
                }

                this.selectedAbility = value;
                OnPropertyChanged(nameof(SelectedAbilityProperty));
            }
        }

        /// <summary>
        /// Sets and gets Лист задач и сколько их раз нужно сделать.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<TaskAndCounts> TaskCountsProperty
        {
            get
            {
                return this.taskCounts;
            }

            set
            {
                if (this.taskCounts == value)
                {
                    return;
                }

                this.taskCounts = value;
                OnPropertyChanged(nameof(TaskCountsProperty));
            }
        }

        #endregion
    }

    /// <summary>
    /// The task and counts.
    /// </summary>
    public class TaskAndCounts
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the count property.
        /// </summary>
        public int CountProperty { get; set; }

        /// <summary>
        /// Gets or sets the task property.
        /// </summary>
        public Task TaskProperty { get; set; }

        #endregion
    }

    /// <summary>
    /// The qwest relay abil.
    /// </summary>
    public class QwestRelayAbil : INotifyPropertyChanged
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

        #region Fields

        /// <summary>
        /// Цель.
        /// </summary>
        private Aim aim;

        /// <summary>
        /// Коэффициент влияния.
        /// </summary>
        private int kRelay = 10;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Цель.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Aim AimProperty
        {
            get
            {
                return this.aim;
            }

            set
            {
                if (this.aim == value)
                {
                    return;
                }

                this.aim = value;
                OnPropertyChanged(nameof(AimProperty));
            }
        }

        /// <summary>
        /// Sets and gets Коэффициент влияния.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int KRelayProperty
        {
            get
            {
                return this.kRelay;
            }

            set
            {
                if (this.kRelay == value)
                {
                    return;
                }

                this.kRelay = value;
                OnPropertyChanged(nameof(KRelayProperty));
            }
        }

        #endregion
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using DotNetLead.DragDrop.UI.Behavior;
using GalaSoft.MvvmLight.Messaging;

namespace Sample.Model
{
    using System;

    /// <summary>
    /// Требования для задач
    /// </summary>
    [Serializable]
    public class NeedTasks : AimNeeds, IDragable, IDropable, IComparable
    {
        private int _minValue;

        /// <summary>
        /// Просто как ссылка.
        /// </summary>
        private bool asLink;

        /// <summary>
        /// Требования активны?.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Уровень требований.
        /// </summary>
        public int level;

        /// <summary>
        /// Шаг.
        /// </summary>
        private string step;

        /// <summary>
        /// номер шага.
        /// </summary>
        private int stepInt;

        /// <summary>
        /// Уровень требований.
        /// </summary>
        public int Tolevel;

        /// <summary>
        /// Sets and gets Просто как ссылка. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool AsLinkProperty
        {
            get
            {
                return asLink;
            }

            set
            {
                if (asLink == value)
                {
                    return;
                }

                asLink = value;
                OnPropertyChanged(nameof(AsLinkProperty));
                if (value) KRel = 0;
                TaskProperty?.ChangeValuesOfRelaytedItems();
            }
        }

        public Brush BackBrush
        {
            get { return CompositeAims.GetBrush(LevelProperty); }
        }

        public List<NeedTasks> ChildNeeds { get; set; }

        /// <summary>
        /// Sets and gets Требования активны?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsActiveProperty
        {
            get
            {
                return isActive;
            }

            set
            {
                if (isActive == value)
                {
                    return;
                }

                isActive = value;
                OnPropertyChanged(nameof(IsActiveProperty));
            }
        }

        /// <summary>
        /// Коэффициент влияния задачи на скиллы и квесты
        /// </summary>
        public override double KoeficientProperty
        {
            get
            {
                if (AsLinkProperty)
                {
                    return 0;
                }

                return TaskProperty.MaxValueOfTaskProperty;
            }
        }

        public double KRel
        {
            get
            {
                if (AsLinkProperty) _kRel = 0;
                return _kRel;
            }
            set
            {
                if (AsLinkProperty)
                {
                    value = 0;
                }

                if (value == _kRel) return;
                _kRel = value;
                OnPropertyChanged(nameof(KRel));
                TaskProperty?.ChangeValuesOfRelaytedItems();
            }
        }

        /// <summary>
        /// Sets and gets Уровень требований. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int LevelProperty
        {
            get
            {
                if (level > StaticMetods.MaxAbLevel)
                {
                    level = StaticMetods.MaxAbLevel;
                }

                if (level < 0)
                {
                    level = 0;
                }

                return level;
            }

            set
            {
                if (value > StaticMetods.MaxAbLevel)
                {
                    value = StaticMetods.MaxAbLevel;
                }

                if (value < 0)
                {
                    value = 0;
                }

                if (level == value)
                {
                    return;
                }

                if (Tolevel < value)
                {
                    Tolevel = value;
                }

                level = value;
                OnPropertyChanged(nameof(LevelProperty));
                OnPropertyChanged(nameof(BackBrush));
                OnPropertyChanged(nameof(ToBrush));
            }
        }

        /// <summary>
        /// Максимальное значение скилла при котором данное требование будет активно!
        /// </summary>
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (value == _maxValue) return;
                _maxValue = value;
                OnPropertyChanged(nameof(MaxValue));
            }
        }

        /// <summary>
        /// Минимальный уровень персонажа для задачи
        /// </summary>
        public int MinPersLevel
        {
            get
            {
                return _minPersLevel;
            }
            set
            {
                if (value == _minPersLevel) return;
                _minPersLevel = value;
                OnPropertyChanged(nameof(MinPersLevel));
            }
        }

        /// <summary>
        /// Минимальное значение скилла при котором данное требование будет активно!
        /// </summary>
        public int MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                if (value == _minValue) return;
                _minValue = value;
                OnPropertyChanged(nameof(MinValue));
            }
        }

        /// <summary>
        /// Прогресс
        /// </summary>
        public int Progress
        {
            get
            {
                if (TaskProperty == null)
                {
                    return 0;
                }
                else
                {
                    if (TaskProperty.IsDelProperty == true)
                    {
                        return 100;
                    }

                    var prog = Convert.ToDouble(TaskProperty.ValueOfTaskProperty)
                               / Convert.ToDouble(TaskProperty.MaxValueOfTaskProperty);
                    return Convert.ToInt32((prog * 100.0));
                }
            }
            set
            {
                return;
            }
        }

        /// <summary>
        /// Sets and gets номер шага. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int StepIntProperty
        {
            get
            {
                return stepInt;
            }

            set
            {
                if (stepInt == value)
                {
                    return;
                }

                stepInt = value;
                OnPropertyChanged(nameof(StepIntProperty));
            }
        }

        /// <summary>
        /// Sets and gets Шаг. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string StepProperty
        {
            get
            {
                return step;
            }

            set
            {
                if (step == value)
                {
                    return;
                }

                step = value;
                OnPropertyChanged(nameof(StepProperty));
            }
        }

        /// <summary>
        /// Gets or sets Задача
        /// </summary>
        public Task TaskProperty
        {
            get
            {
                return this.task;
            }

            set
            {
                this.task = value;
                this.OnPropertyChanged(nameof(TaskProperty));
            }
        }

        public Brush ToBrush
        {
            get { return CompositeAims.GetBrush(LevelProperty); }
        }

        /// <summary>
        /// Sets and gets Уровень требований. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int ToLevelProperty
        {
            get
            {
                if (Tolevel < LevelProperty)
                {
                    LevelProperty = Tolevel;
                }

                if (level > StaticMetods.MaxAbLevel)
                {
                    level = StaticMetods.MaxAbLevel;
                }

                if (level < 0)
                {
                    level = 0;
                }

                return Tolevel;
            }

            set
            {
                if (value > StaticMetods.MaxAbLevel)
                {
                    value = StaticMetods.MaxAbLevel;
                }

                if (value < 0)
                {
                    value = 0;
                }

                if (Tolevel == value)
                {
                    return;
                }

                Tolevel = value;
                OnPropertyChanged(nameof(ToLevelProperty));
                OnPropertyChanged(nameof(BackBrush));
                OnPropertyChanged(nameof(ToBrush));
            }
        }

        Type IDragable.DataType
        {
            get { return typeof(NeedTasks); }
        }

        Type IDropable.DataType
        {
            get { return typeof(NeedTasks); }
        }

        public int CompareTo(object obj)
        {
            NeedTasks other = (NeedTasks)obj;

            var byLev = LevelProperty.CompareTo(other.LevelProperty);
            if (byLev != 0)
            {
                return -byLev;
            }

            var byLevTo = ToLevelProperty.CompareTo(other.ToLevelProperty);
            if (byLevTo != 0)
            {
                return -byLevTo;
            }

            var byInd = StaticMetods.PersProperty.Tasks.IndexOf(TaskProperty).CompareTo(StaticMetods.PersProperty.Tasks.IndexOf(other.TaskProperty));
            return -byInd;
        }

        public void Drop(object data, int index = -1)
        {
            var TaskB = TaskProperty;
            var needTasks = data as NeedTasks;
            if (needTasks != null)
                Messenger.Default.Send(new MoveTaskMessege { taskA = needTasks.TaskProperty, taskB = TaskB, IgnorePlaningMode = true });

            var qwestsViewModel = StaticMetods.Locator.QwestsVM;

            foreach (Aim aim in StaticMetods.PersProperty.Aims.Where(n => n.NeedsTasks.Any(q => q == needTasks)))
            {
                aim.RefreshMissions();
            }

            qwestsViewModel.NeedsRefresh();
        }

        public void Remove(object i)
        {
        }

        #region Fields

        private double _kRel;

        private int _maxValue = 100;

        private int _minPersLevel;

        /// <summary>
        /// Задача
        /// </summary>
        private Task task;

        #endregion Fields
    }
}
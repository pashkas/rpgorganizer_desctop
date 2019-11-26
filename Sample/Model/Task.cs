using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DotNetLead.DragDrop.UI.Behavior;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Properties;
using Sample.View;
using Sample.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Sample.Model
{
    public class AllLincsComparer : IEqualityComparer<LinkThisTask>
    {
        public bool Equals(LinkThisTask x, LinkThisTask y)
        {
            return x.GuidOfElement == y.GuidOfElement;
        }

        public int GetHashCode(LinkThisTask obj)
        {
            return obj.GuidOfElement.GetHashCode();
        }
    }

    /// <summary>
    /// То, на что влияет задача. В каких квестах и скиллах она находится
    /// </summary>
    [Serializable]
    public class LinkThisTask
    {
        public string _backGroundColor;

        /// <summary>
        /// Кисть фона
        /// </summary>
        public Brush ForeGroundd
        {
            get
            {
                return new BrushConverter().ConvertFromString(_backGroundColor) as SolidColorBrush;
            }
            set
            {
                if (_backGroundColor == value.ToString())
                {
                    return;
                }

                _backGroundColor = value.ToString();
            }
        }

        /// <summary>
        /// Ид Элемента
        /// </summary>
        public string GuidOfElement { get; set; }

        /// <summary>
        /// Индекс. 1-навык, 2-квест.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Название элемента
        /// </summary>
        public string NameElement { get; set; }

        public BitmapImage Picture
        {
            get
            {
                if (StaticMetods.PersProperty == null)
                {
                    return null;
                }

                var abil = StaticMetods.PersProperty.Abilitis.FirstOrDefault(n => n.GUID == GuidOfElement);
                var qwest = StaticMetods.PersProperty.Abilitis.FirstOrDefault(n => n.GUID == GuidOfElement);

                if (abil != null) return abil.PictureProperty;
                if (qwest != null) return qwest.PictureProperty;

                return null;
            }
        }

        /// <summary>
        /// Короткое название
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Количество задач (активных) в этом элементе
        /// </summary>
        public int TasksCount { get; set; }
    }

    /// <summary>
    /// The task.
    /// </summary>
    [Serializable]
    public class Task : BaseRPGItem, IDropable, IDragable, IExpable, IComparable, IHaveRevords
    {
        /// <summary>
        /// Навык, который связан с задачей в режиме без навыков.
        /// </summary>
        public AbilitiModel NoAbsAb { get; set; }

        public List<AbilitiModel> _taskInAbilitis;

        [field: NonSerialized]
        public RelayCommand pomodoroTimerStartCommand;

        public string timeOfTask = "01.01.2001 23:59:00";

        /// <summary>
        /// Таймер
        /// </summary>
        [NonSerialized]
        private readonly DispatcherTimer timer = new DispatcherTimer { Interval = StaticMetods.timeSpan };

        private int _aimCounterMax;

        private int _aimMesure;

        private int _aimTimerMax;

        private List<LinkThisTask> _allLinksThisTask;

        /// <summary>
        /// Цвет фона задачи
        /// </summary>
        private string _backGroundColor = Brushes.Transparent.ToString();

        private double _boosterOfDone;
        private double _boosterOfFail;
        private int _damage;

        private List<LinkThisTask> _directTaskLinks;

        private bool _isBoss;

        private bool _isChalange = false;

        private bool _isLastClickNotDone;

        private string _isMoreHack;

        private bool _isPomodorroTimerStart;

        private bool _isSuper;

        private bool _isTaskActive;

        private bool _isWordHardness;

        private string _link;

        private List<Task> _linkedTasks;

        private int _mesure;

        private int _mesureIs;

        private double _miliSecsDoneForSort = DateTime.MaxValue.TimeOfDay.TotalMilliseconds;

        private string _nameOfGroup;

        private string _pathToIm;

        private int _plusExp;

        private int _plusGold;

        private string _plusName3;

        private int _pomodorroMax;

        private int _pomodorroNow;

        private double _pomodorroTimerTime;

        private double _pomodorroTinerDouble;

        private double _priority;

        private int _rage;

        private bool _repeatSubTasks;

        private string _reqvirements;

        private long _secondOfDone;

        private int _selFocTasksIndex;

        private string _state;

        private ObservableCollection<SubTask> _subTasks;

        private typeOfSubTaskRecurrenses _subTasksRec;

        private int _taskLevel = -1;

        private ObservableCollection<SubTask> _taskState;

        private StatusTask _taskStatus;

        private ObservableCollection<TimeClass> _timeEnd;

        private int _wave;

        /// <summary>
        /// Время начала задачи в формате ДАТА ТАЙМ.
        /// </summary>
        private string beginDate;

        /// <summary>
        /// Влияение на значение если не сделана.
        /// </summary>
        private double changeValueIfNotDone;

        /// <summary>
        /// Комманда Клик по счетчику.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand clickCounterCommand;

        /// <summary>
        /// Комманда Клик по значению счетчика задачи.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand clickMesureCommand;

        /// <summary>
        /// Комманда Клик по таймеру.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand clickTimerCommand;

        /// <summary>
        /// Максимальное значение счетчика.
        /// </summary>
        private int counterMaxValue = 1;

        /// <summary>
        /// Значение счетчика.
        /// </summary>
        private int counterValue;

        /// <summary>
        /// Значение счетчика.
        /// </summary>
        private int counterValuePlus;

        /// <summary>
        /// Дата окончания задачи
        /// </summary>
        private string endDate;

        /// <summary>
        /// Сложность задачи (влияет только на вероятности выпадения предетов).
        /// </summary>
        private int hardness;

        /// <summary>
        /// Альтернативное повторение задействовано?.
        /// </summary>
        private bool isAlternateReccurense;

        /// <summary>
        /// Активирован счетчик?.
        /// </summary>
        private bool isCounterEnabled;

        /// <summary>
        /// Задача удалена?.
        /// </summary>
        private bool isDel;

        /// <summary>
        /// Таймер запущен?.
        /// </summary>
        [field: NonSerialized]
        private bool isTimerStart;

        /// <summary>
        /// Ссылки от задачи.
        /// </summary>
        private ObservableCollection<Links> linkses;

        /// <summary>
        /// Максимальное значение задачи.
        /// </summary>
        private double maxValueOfTask;

        /// <summary>
        /// Таймер
        /// </summary>
        [NonSerialized]
        private DispatcherTimer newTimer = new DispatcherTimer { Interval = StaticMetods.timeSpan };

        /// <summary>
        /// Не повторять подзадачи по умолчанию.
        /// </summary>
        private bool notRepeatSubtasksForDefoult;

        /// <summary>
        /// Таймер Помодорро
        /// </summary>
        [NonSerialized]
        private DispatcherTimer pomodorroTimer = new DispatcherTimer { Interval = StaticMetods.timeSpan };

        /// <summary>
        /// Gets the Быстро задать дату (начала или срока).
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<string> qwickSetDateCommand;

        /// <summary>
        /// Затраченное время на задачу в минутах.
        /// </summary>
        private int timeIs;

        private string timeLastDone = DateTime.MinValue.ToString();

        /// <summary>
        /// Ориентировочное время на выполнение задачи в минутах.
        /// </summary>
        private int timeMust;

        /// <summary>
        /// Таймер активен?.
        /// </summary>
        private Visibility timerActive = Visibility.Collapsed;

        /// <summary>
        /// Значение таймера.
        /// </summary>
        private int timerValue;

        /// <summary>
        /// Значение задачи.
        /// </summary>
        private double valueOfTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        public Task()
        {
            GUID = Guid.NewGuid().ToString();
            NameOfProperty = "Новая задача";
            NextActions = new ObservableCollection<Task>();
            SubTasks = new ObservableCollection<SubTask>();
            timer.Tick += (sender, e) => { TimeIsProperty++; };
            BoosterOfDone = 1;
            BoosterOfFail = 1;
        }

        /// <summary>
        /// Ссылка на скилл
        /// </summary>
        public AbilitiModel AbilityLink
        {
            get { return TaskInAbilitis?.FirstOrDefault(); }
        }

        /// <summary>
        /// Максимальное целевое значение счетчика
        /// </summary>
        public int AimCounterMax
        {
            get
            {
                return _aimCounterMax;
            }
            set
            {
                if (value == _aimCounterMax) return;
                _aimCounterMax = value;
                OnPropertyChanged(nameof(AimCounterMax));
            }
        }

        /// <summary>
        /// Целевое значени количества
        /// </summary>
        public int AimMesure
        {
            get
            {
                return _aimMesure;
            }
            set
            {
                if (value == _aimMesure) return;
                _aimMesure = value;
                OnPropertyChanged(nameof(AimMesure));
                RecountAutoValues();
                OnPropertyChanged(nameof(LastMesure));
            }
        }

        /// <summary>
        /// Целевое значение таймера
        /// </summary>
        public int AimTimerMax
        {
            get
            {
                return _aimTimerMax;
            }
            set
            {
                if (value == _aimTimerMax) return;
                _aimTimerMax = value;
                OnPropertyChanged(nameof(AimTimerMax));
                RecountAutoValues();
                OnPropertyChanged(nameof(LastTimer));
            }
        }

        /// <summary>
        /// Если данная задача является скиллом - квесты в которых данная задача используется
        /// </summary>
        public IEnumerable<Aim> AimToSkill
        {
            get
            {
                var aimToSkill = StaticMetods.PersProperty.Aims.OrderBy(n => n)
                    .Where(n => !n.IsDoneProperty)
                    .Where(n => n.AbilitiLinksOf.Any(q => q.NeedTasks.Any(z => z.TaskProperty == this)));
                return aimToSkill;
            }
        }

        /// <summary>
        /// Ссылки на задачи в квестах
        /// </summary>
        public IEnumerable<Aim> AimToTask
        {
            get
            {
                var aimToTask =
                    StaticMetods.PersProperty.Aims.Where(n => n.LinksOfTasks.Any(q => q == this)).Distinct().ToList();
                return aimToTask;
            }
        }

        /// <summary>
        /// Полностью все на что задача влияет
        /// </summary>
        public List<LinkThisTask> AllLinksThisTask
        {
            get
            {
                return _allLinksThisTask;
            }
            set
            {
                _allLinksThisTask = value;
                OnPropertyChanged(nameof(AllLinksThisTask));
                OnPropertyChanged(nameof(AllLinksThisView));
                OnPropertyChanged(nameof(AllLinksThisTaskString));
            }
        }

        /// <summary>
        /// Текстовое представление ссылок
        /// </summary>
        public string AllLinksThisTaskString
        {
            get
            {
                var lnks = AllLinksThisView;
                if (lnks == null)
                {
                    return string.Empty;
                }
                return lnks.Aggregate("", (current, lnk) => current + lnk.ShortName + "");
            }
        }

        public IEnumerable<LinkThisTask> AllLinksThisView
        {
            get
            {
                //if (_allLinksThisTask == null)
                //{
                //    return _allLinksThisTask;
                //}
                //if (StaticMetods.Locator?.MainVM?.IsFocTaksVisibility == Visibility.Collapsed)
                //{
                //    if (StaticMetods.Locator?.MainVM?.SelectedView?.NameOfView == "Навыки")
                //    {
                //        return _allLinksThisTask.Where(n => n.ForeGroundd.ToString() == Brushes.SlateBlue.ToString());
                //    }
                //    else if (StaticMetods.Locator?.MainVM?.SelectedView?.NameOfView == "Квесты")
                //    {
                //        return _allLinksThisTask.Where(n => n.ForeGroundd.ToString() == Brushes.Green.ToString());

                //    }
                //}
                return _allLinksThisTask;
            }
        }

        /// <summary>
        /// Кисть фона задачи
        /// </summary>
        public Brush BackGroundBrush
        {
            get
            {
                if (string.IsNullOrEmpty(_backGroundColor) || _backGroundColor == Brushes.Transparent.ToString())
                {
                    return EndDateForeground;
                }
                return new BrushConverter().ConvertFromString(_backGroundColor) as SolidColorBrush;
            }
            set
            {
                if (_backGroundColor == value.ToString())
                {
                    return;
                }

                _backGroundColor = value.ToString();
                OnPropertyChanged(nameof(BackGroundBrush));
            }
        }

        /// <summary>
        /// Цвет фона задач
        /// </summary>
        public Color BackGroundColor
        {
            get
            {
                if (string.IsNullOrEmpty(_backGroundColor) || _backGroundColor == Brushes.Transparent.ToString())
                {
                    return Colors.Transparent;
                }

                return (Color)ColorConverter.ConvertFromString(_backGroundColor);
            }
            set
            {
                if (_backGroundColor == value.ToString())
                {
                    return;
                }

                _backGroundColor = value.ToString();
                OnPropertyChanged(nameof(BackGroundColor));
                OnPropertyChanged(nameof(BackGroundBrush));
            }
        }

        /// <summary>
        /// Связанный скилл с этой задачей
        /// </summary>
        public Task BaseOfSkill { get; set; }

        /// <summary>
        /// Sets and gets Время начала задачи в формате ДАТА ТАЙМ. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public DateTime BeginDateProperty
        {
            get
            {
                return StaticMetods.GetDateFromString(beginDate);
            }

            set
            {
                beginDate = value.Date.ToString();

                IsDateComplitesReccurence(ref value);

                beginDate = value.Date.ToString();

                if (EndDate < value.Date)
                {
                    EndDate = value.Date;
                }

                OnPropertyChanged(nameof(BeginDateProperty));
            }
        }

        /// <summary>
        /// Множитель если задача выполнена.
        /// </summary>
        public double BoosterOfDone
        {
            get
            {
                return _boosterOfDone;
            }
            set
            {
                _boosterOfDone = value;
            }
        }

        /// <summary>
        /// Бустер если задача провалена.
        /// </summary>
        public double BoosterOfFail
        {
            get
            {
                return _boosterOfFail;
            }
            set
            {
                _boosterOfFail = value;
            }
        }

        /// <summary>
        /// Sets and gets Влияение на значение если не сделана. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public double ChangeValueIfNotDoneProperty
        {
            get
            {
                return StaticMetods.Config.BuffTaskValue;
                //return changeValueIfNotDone;
            }

            set
            {
                if (changeValueIfNotDone == value)
                {
                    return;
                }

                changeValueIfNotDone = value;
                OnPropertyChanged(nameof(ChangeValueIfNotDoneProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Клик по счетчику.
        /// </summary>
        public RelayCommand ClickCounterCommand
        {
            get
            {
                return clickCounterCommand
                       ?? (clickCounterCommand =
                           new RelayCommand(
                               () => { CounterValuePlusProperty++; },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Клик по значению счетчика задачи.
        /// </summary>
        public RelayCommand ClickMesureCommand
        {
            get
            {
                return clickMesureCommand
                       ?? (clickMesureCommand =
                           new RelayCommand(
                               () =>
                               {
                                   if (!StaticMetods.PersProperty.IsFocTasks)
                                   {
                                       StaticMetods.Locator.MainVM.LastParTask =
                                           StaticMetods.Locator.MainVM.PrevLastParTask;
                                   }
                                   MesureIs++;
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Клик по таймеру.
        /// </summary>
        public RelayCommand ClickTimerCommand
        {
            get
            {
                return clickTimerCommand ?? (clickTimerCommand = new RelayCommand(
                    () =>
                    {
                        if (!StaticMetods.PersProperty.IsFocTasks)
                        {
                            StaticMetods.Locator.MainVM.LastParTask = StaticMetods.Locator.MainVM.PrevLastParTask;
                        }
                        if (newTimer != null && newTimer.IsEnabled)
                        {
                            //MessageBox.Show(new Form {TopMost = true}, $"Таймер остановлен!!!");
                            TimerStop();
                        }
                        else
                        {
                            //MessageBox.Show(new Form {TopMost = true}, $"Таймер запущен!!!");
                            TimerStart();
                        }
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Максимальное значение счетчика. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int CounterMaxValueProperty
        {
            get
            {
                return 0;
                return counterMaxValue;
            }

            set
            {
                if (counterMaxValue == value || value < 1)
                {
                    return;
                }

                counterMaxValue = value;
                OnPropertyChanged(nameof(CounterMaxValueProperty));
                OnPropertyChanged(nameof(PlusNameOf));
                OnPropertyChanged(nameof(CounterVisibility));
                RefreshTimeEnds();
            }
        }

        /// <summary>
        /// Видимость настройки счетчика
        /// </summary>
        public Visibility CounterSettingsVisibility
        {
            get
            {
                if (Recurrense.TypeInterval == TimeIntervals.Нет || Recurrense.TypeInterval == TimeIntervals.Сразу)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets and gets Значение счетчика. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int CounterValuePlusProperty
        {
            get
            {
                if (counterValuePlus > CounterMaxValueProperty)
                {
                    counterValuePlus = CounterMaxValueProperty;
                }
                return counterValuePlus;
            }

            set
            {
                if (counterValuePlus == value || value < 0 || counterValuePlus > CounterMaxValueProperty)
                {
                    return;
                }

                counterValuePlus = value;
                OnPropertyChanged(nameof(CounterVisibility));
                OnPropertyChanged(nameof(CounterValuePlusProperty));
            }
        }

        /// <summary>
        /// Sets and gets Значение счетчика. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int CounterValueProperty
        {
            get
            {
                return counterValue;
            }

            set
            {
                if (counterValue == value)
                {
                    return;
                }

                counterValue = value;
                OnPropertyChanged(nameof(CounterValueProperty));
            }
        }

        public Visibility CounterVisibility
        {
            get
            {
                if (CounterMaxValueProperty > 1 && Recurrense.TypeInterval != TimeIntervals.Нет &&
                    Recurrense.TypeInterval != TimeIntervals.Сразу)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Урон от задачи
        /// </summary>
        public int Damage
        {
            get
            {
                if (TaskStatus.NameOfStatus != "Первым делом")
                {
                    return 0;
                }

                var typeInterval = Recurrense.TypeInterval;
                double baseDamageHabbit = StaticMetods.PersProperty.PersSettings.DamageFromHabbit;
                double baseDamageTask = StaticMetods.PersProperty.PersSettings.DamageFromeTask;
                var damage = baseDamageTask;

                switch (typeInterval)
                {
                    case TimeIntervals.Нет:
                        damage = baseDamageTask;
                        break;

                    case TimeIntervals.Ежедневно:
                        damage = baseDamageHabbit;
                        break;

                    case TimeIntervals.День:
                    case TimeIntervals.ДниСначала:
                        damage = baseDamageHabbit * Convert.ToDouble(Recurrense.Interval);
                        break;

                    case TimeIntervals.Будни:
                        damage = baseDamageHabbit;
                        break;

                    case TimeIntervals.Выходные:
                        damage = baseDamageHabbit * 7.0 / 2.0;
                        break;

                    case TimeIntervals.ДниНедели:
                    case TimeIntervals.ДниНеделиСНачала:
                        double interv = DaysOfWeekRepeats.Count(n => n.CheckedProperty);
                        interv = interv != 0 ? interv : 1.0;
                        damage = baseDamageHabbit * 7.0 / interv;
                        break;

                    case TimeIntervals.Неделя:
                    case TimeIntervals.НеделиСНачала:
                        damage = baseDamageHabbit * 7.0;
                        break;
                }

                var inAbs = (from abilitiModel in StaticMetods.PersProperty.Abilitis
                             let any =
                                 abilitiModel.NeedTasks.Where(n =>
                                     n.LevelProperty == abilitiModel.CellValue - 1)
                             where any.Any(n => n.TaskProperty == this)
                             select new { ability = abilitiModel, needs = any, count = any.Count() }).ToList();

                if (inAbs.Any())
                {
                    damage = damage * Convert.ToDouble(inAbs.Count) / Convert.ToDouble(inAbs.Sum(n => n.count));
                }

                return UcTasksSettingsViewModel.DoubleToFloorToInt(damage);
                ;
            }
            set
            {
                return;
                if (value == _damage) return;
                if (value < 0) value = 0;
                _damage = value;
                OnPropertyChanged(nameof(Damage));
            }
        }

        /// <summary>
        /// Gets or sets Дата завершения
        /// </summary>
        public string DateOfDone { get; set; }

        /// <summary>
        /// Gets or Sets Дни недели для повторения
        /// </summary>
        public ObservableCollection<DaysOfWeekRepeat> DaysOfWeekRepeats { get; set; }

        public string DescriptionForAbility
        {
            get
            {
                var dsc = new List<string>();

                if (TimeMustProperty != 0)
                {
                    dsc.Add($"{TimeMustProperty}⧖");
                }

                if (Mesure != 0 &&
                       Recurrense.TypeInterval != TimeIntervals.Нет)
                {
                    dsc.Add($"{Mesure}✓");
                }

                if (Wave == 0)
                {
                    dsc.Add("∞");
                }
                else
                {
                    var fod = StaticMetods.PersProperty.PersSettings.WaveNames.FirstOrDefault(n => n.Item1 == Wave);
                    if (fod != null)
                    {
                        dsc.Add(fod.Item2.First().ToString().ToUpper() + fod.Item2.Substring(1));
                    }
                }

                dsc.Add(RecurrenceString);
                return string.Join("; ", dsc);
            }
        }

        public string DescriptionForAbilityLast
        {
            get
            {
                var dsc = new List<string>();

                if (AimTimerMax != 0)
                {
                    if (AimTimerMax != 0)
                    {
                        dsc.Add($"{AimTimerMax}⧖");
                    }
                    else if (TimeMustProperty != 0)
                    {
                        dsc.Add($"{TimeMustProperty}⧖");
                    }
                }

                if (Recurrense.TypeInterval != TimeIntervals.Нет)
                {
                    if (AimMesure != 0)
                    {
                        dsc.Add($"{AimMesure}✓");
                    }
                    else if (Mesure > 0)
                    {
                        dsc.Add($"{Mesure}✓");
                    }
                }

                if (Wave == 0)
                {
                    dsc.Add("∞");
                }
                else
                {
                    var fod = StaticMetods.PersProperty.PersSettings.WaveNames.FirstOrDefault(n => n.Item1 == Wave);
                    if (fod != null)
                    {
                        dsc.Add(fod.Item2.First().ToString().ToUpper() + fod.Item2.Substring(1));
                    }
                }

                dsc.Add(RecurrenceString);
                return string.Join("; ", dsc);
            }
        }

        /// <summary>
        /// На что влияет задача НАРЯМУЮ
        /// </summary>
        public List<LinkThisTask> DirectTaskLinks
        {
            get
            {
                return _directTaskLinks;
            }
            set
            {
                if (Equals(value, _directTaskLinks)) return;
                _directTaskLinks = value;
                OnPropertyChanged(nameof(DirectTaskLinks));
            }
        }

        /// <summary>
        /// Дата окончания задачи
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return StaticMetods.GetDateFromString(endDate).Date;
            }
            set
            {
                if (endDate == value.Date.ToString())
                {
                    return;
                }

                if (BeginDateProperty.Date > value.Date)
                {
                    IsDateComplitesReccurence(ref value);
                    BeginDateProperty = value;
                }

                endDate = value.Date.ToString();

                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(EndDateForeground));
            }
        }

        /// <summary>
        /// Цвет даты окончания задачи
        /// </summary>
        public Brush EndDateForeground
        {
            get
            {
                //if (EndDate.Date < MainViewModel.selectedTime)
                //{
                //    return Brushes.LightCoral;
                //}

                //if (IsSkill)
                //{
                //    //if (MainViewModel.IsMorning().Invoke(this))
                //    //{
                //    //    return new BrushConverter().ConvertFromString("#ffff99") as SolidColorBrush;
                //    //}
                //    //if (MainViewModel.IsDay().Invoke(this))
                //    //{
                //    //    return new BrushConverter().ConvertFromString("#98FB98") as SolidColorBrush;
                //    //}
                //    //if (MainViewModel.IsEvening().Invoke(this))
                //    //{
                //    //    return new BrushConverter().ConvertFromString("#ffe6c9") as SolidColorBrush;
                //    //}
                //    //if (IsMaxAb())
                //    //{
                //    //    return new BrushConverter().ConvertFromString("#ADD8E6") as SolidColorBrush;
                //    //}

                //    if (StaticMetods.PersProperty.IsSetViz && BeginDateProperty > MainViewModel.selectedTime) return Brushes.LightBlue;
                //    return new BrushConverter().ConvertFromString("#ffffa3") as SolidColorBrush;
                //    //return Brushes.Wheat;
                //}
                if (StaticMetods.PersProperty.IsPlanningModeMain && BeginDateProperty > MainViewModel.selectedTime) return Brushes.LightBlue;

                //if (Wave > 0)
                //{
                //    return new BrushConverter().ConvertFromString("#ffffa3") as SolidColorBrush;
                //}
                if (IsSkill)
                {
                    return new BrushConverter().ConvertFromString("#ffffa3") as SolidColorBrush;
                }

                return Brushes.White;

                //if (TaskStatus.NameOfStatus == "Планируется")
                //{
                //    return new BrushConverter().ConvertFromString("#ffff99") as SolidColorBrush;
                //}
            }
        }

        /// <summary>
        /// Минуты завершения для андроида для того, чтобы можно было сортировать задачи при экспорте
        /// в андроид
        /// </summary>
        public int EndMinutesAndroid { get; set; }

        /// <summary>
        /// ВРЕМЯ окончания
        /// </summary>
        public TimeClass EndTime
        {
            get
            {
                if (CounterMaxValueProperty == CounterValuePlusProperty)
                {
                    return new TimeClass { Hour = 9999 };
                }

                if (TimeEnds.Count <= CounterValuePlusProperty)
                {
                    RefreshTimeEnds();
                }

                return TimeEnds[CounterValuePlusProperty];
            }
        }

        /// <summary>
        /// Цвет названия задачи
        /// </summary>
        public string ForeGroundBrush
        {
            get
            {
                //if (IsChalange)
                //{
                //    return Brushes.HotPink.ToString();
                //}

                //if (IsBoss)
                //{
                //    return Brushes.HotPink.ToString();
                //}

                //if (StaticMetods.PersProperty?.PersSettings?.IsGhostBastersMode == true && IsSkill && IsMaxAb())
                //{
                //    return Colors.BlueViolet.ToString();
                //}

                //if (StaticMetods.Locator?.MainVM?.IsFocTaksVisibility == Visibility.Collapsed ||
                //    StaticMetods.PersProperty?.PersSettings?.IsGhostBastersMode == false)
                //{
                //    return "#FF606060";
                //}

                //if (Wave>0)
                //{
                //    return Brushes.Green.ToString();
                //}

                return "#FF606060";
            }
        }

        public Brush ForegroundForInterfacePanel
        {
            get
            {
                if (Recurrense.TypeInterval != TimeIntervals.Нет)
                {
                    return Brushes.Green;
                    //BackGroundBrush; //new BrushConverter().ConvertFromString("#a8ed66") as SolidColorBrush;
                }
                return Brushes.White;
            }
        }

        //        return sum == 0 ? StaticMetods.Config.ExpToTask : sum;
        //    }
        //}
        /// <summary>
        /// Sets and gets Сложность задачи (влияет только на вероятности выпадения предетов). Changes
        /// to that property's value raise the PropertyChanged event.
        /// </summary>
        public int HardnessProperty
        {
            get
            {
                return hardness;
            }

            set
            {
                if (hardness == value)
                {
                    return;
                }

                hardness = value;

                switch (hardness)
                {
                    case 1:
                        break;

                    case 2:
                        break;

                    case 3:
                        break;
                }

                OnPropertyChanged(nameof(HardnessProperty));
            }
        }

        // // Делим на счетчик, для дел - увеличиваем в два раза sum = sum / CounterMaxValueProperty;
        /// <summary>
        /// Индекс повтора - повторение 2, счетчик 1, дело 0
        /// </summary>
        public int IndexOfRepeat
        {
            get
            {
                if (Recurrense.TypeInterval == TimeIntervals.Нет)
                {
                    return 1;
                }

                return 0;
            }
        }

        // return sm; });
        /// <summary>
        /// Sets and gets Альтернативное повторение задействовано?. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool IsAlternateReccurenseProperty
        {
            get
            {
                return isAlternateReccurense;
            }

            set
            {
                if (isAlternateReccurense == value)
                {
                    return;
                }

                isAlternateReccurense = value;
                OnPropertyChanged(nameof(IsAlternateReccurenseProperty));
            }
        }

        // // Делим цену за задачу на число привычек в скилле var kNeedness =
        // n.GetAbNeedness(StaticMetods.PersProperty); var expa = (n.GetBaseCost() + kNeedness) *
        // StaticMetods.KAbLevExp(n); var sm = expa / all;
        /// <summary>
        /// Босс? Более крутая картинка и выделено цветом.
        /// </summary>
        public bool IsBoss
        {
            get
            {
                return _isBoss;
            }
            set
            {
                if (value == _isBoss) return;
                _isBoss = value;
                OnPropertyChanged(nameof(IsBoss));
                GetEnamyImage();
                OnPropertyChanged(nameof(ForeGroundBrush));
            }
        }

        /// <summary>
        /// Бустер? За очень сложные задачи двойной опыт
        /// </summary>
        public bool IsChalange
        {
            get
            {
                return _isChalange;
            }
            set
            {
                if (value == _isChalange) return;
                _isChalange = value;
                OnPropertyChanged(nameof(IsChalange));
                OnPropertyChanged(nameof(ForeGroundBrush));
                RecountTaskLevel(this);
            }
        }

        // // Если нет - делитель=1 all = all > 0 ? all : 1;
        /// <summary>
        /// Sets and gets Активирован счетчик?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsCounterEnabledProperty
        {
            get
            {
                return isCounterEnabled;
            }

            set
            {
                if (isCounterEnabled == value)
                {
                    return;
                }

                isCounterEnabled = value;
                OnPropertyChanged(nameof(IsCounterEnabledProperty));
            }
        }

        ///// <summary>
        ///// Опыт за задачу
        ///// </summary>
        //public double Exp
        //{
        //    get
        //    {
        //        var inAbills =
        //            StaticMetods.PersProperty.Abilitis.Where(
        //                n => n.SkillsActive.Union(n.TasksActive).Any(q => q == this));
        //        var sum = inAbills.Sum(n =>
        //        {
        //            // Считаем задачи-привычки
        //            double all = n.SkillsActive.Count;
        /// <summary>
        /// Sets and gets Задача удалена?. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsDelProperty
        {
            get
            {
                return isDel;
            }

            set
            {
                if (isDel == value)
                {
                    return;
                }

                isDel = value;
                OnPropertyChanged(nameof(IsDelProperty));
            }
        }

        /// <summary>
        /// Задача вообще активна?
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return MainViewModel.IsTaskVisibleInCurrentView(this, null, StaticMetods.PersProperty, true, true, true);
            }
        }

        /// <summary>
        /// Видимость "полностью завершена"
        /// </summary>
        public Visibility IsFullDoneVisibility
            => Recurrense.TypeInterval == TimeIntervals.Нет ? Visibility.Collapsed : Visibility.Visible;

        /// <summary>
        /// В последний раз задача была пропущена?
        /// </summary>
        public bool IsLastClickNotDone

        {
            get
            {
                return _isLastClickNotDone;
            }
            set
            {
                if (value == _isLastClickNotDone) return;
                _isLastClickNotDone = value;
                OnPropertyChanged(nameof(IsLastClickNotDone));
            }
        }

        public string IsMoreHack
        {
            get
            {
                return _isMoreHack;
            }
            set
            {
                if (value == _isMoreHack) return;
                _isMoreHack = value;
                OnPropertyChanged(nameof(IsMoreHack));
            }
        }

        /// <summary>
        /// Запущен таймер помидоров?
        /// </summary>
        public bool IsPomodorroTimerStart
        {
            get
            {
                return _isPomodorroTimerStart;
            }
            set
            {
                if (value == _isPomodorroTimerStart) return;
                _isPomodorroTimerStart = value;
                OnPropertyChanged(nameof(IsPomodorroTimerStart));
            }
        }

        /// <summary>
        /// Sets and gets Показывать прогресс задачи?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Visibility IsProgressVisibleProperty
        {
            get
            {
                if (MaxValueOfTaskProperty == 0 || MaxValueOfTaskProperty == 1)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Видны скиллы?
        /// </summary>
        public Visibility IsQwestSkillsVisibility
        {
            get { return AimToSkill.Any() ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Видимость требований
        /// </summary>
        public Visibility IsReqVisibility
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Reqvirements))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Видимость кнопки "Защита!"
        /// </summary>
        public Visibility IsShildVisible
        {
            get
            {
                if (Recurrense.TypeInterval == TimeIntervals.Нет || Recurrense.TypeInterval == TimeIntervals.Сразу)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Эта задача является скиллом?
        /// </summary>
        public bool IsSkill
        {
            get
            {
                var inAbs = (from abilitiModel in StaticMetods.PersProperty.Abilitis
                             from needTaskse in abilitiModel.NeedTasks
                             where needTaskse.TaskProperty == this
                             select new { needTaskse.LevelProperty, Hard = abilitiModel.GetBaseCost() }).ToList();

                return inAbs.Any();
            }
        }

        /// <summary>
        /// Относится к 20% самых эффективных задач
        /// </summary>
        public bool isSuper
        {
            get
            {
                return _isSuper;
            }
            set
            {
                if (value == _isSuper) return;
                _isSuper = value;
                OnPropertyChanged(nameof(isSuper));
            }
        }

        /// <summary>
        /// Задача активна в данном виде?
        /// </summary>
        public bool isTaskActive
        {
            get
            {
                return _isTaskActive;
            }
            set
            {
                if (value == _isTaskActive) return;
                _isTaskActive = value;
                OnPropertyChanged(nameof(isTaskActive));
            }
        }

        /// <summary>
        /// Sets and gets Таймер запущен?. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsTimerStartProperty
        {
            get
            {
                return isTimerStart;
            }

            set
            {
                if (isTimerStart == value)
                {
                    return;
                }

                isTimerStart = value;
                OnPropertyChanged(nameof(IsTimerStartProperty));
                OnPropertyChanged(nameof(TimerBorder));
            }
        }

        /// <summary>
        /// Видно ли настройка времени
        /// </summary>
        public bool IsTimeVisible
        {
            get
            {
                if (Recurrense.TypeInterval == TimeIntervals.Нет)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Видимость редактирования значения (макс, текущее, если пропущена)
        /// </summary>
        public Visibility IsValueSettingVisibility
            => Recurrense.TypeInterval == TimeIntervals.Нет ? Visibility.Collapsed : Visibility.Visible;

        /// <summary>
        /// Словесное обозначение сложности задачи
        /// </summary>
        public bool IsWordHardness
        {
            get
            {
                return _isWordHardness;
            }
            set
            {
                if (value == _isWordHardness) return;
                _isWordHardness = value;
                OnPropertyChanged(nameof(IsWordHardness));
                RecountAutoValues();
            }
        }

        public int LastMesure
        {
            get
            {
                if (AimMesure == 0)
                {
                    return Mesure;
                }
                if (AimMesure < 0)
                {
                    return 0;
                }
                return AimMesure;
            }
        }

        public int LastTimer
        {
            get
            {
                if (AimTimerMax == 0)
                {
                    return TimeMustProperty;
                }
                if (AimTimerMax < 0)
                {
                    return 0;
                }
                return AimTimerMax;
            }
        }

        public int LevelProperty { get; set; }

        /// <summary>
        /// Ссылка на что-то прямо из названия задачи
        /// </summary>
        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                if (value == _link) return;
                _link = value;
                OnPropertyChanged(nameof(Link));
            }
        }

        /// <summary>
        /// Связанные задачи - ссылки
        /// </summary>
        public List<Task> LinkedTasks
        {
            get
            {
                if (_linkedTasks == null)
                {
                    _linkedTasks = new List<Task>();
                }
                return _linkedTasks;
            }
            set
            {
                if (Equals(value, _linkedTasks)) return;
                _linkedTasks = value;
                //OnPropertyChanged(nameof(LinkedTasks));
            }
        }

        /// <summary>
        /// Sets and gets Ссылки от задачи. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<Links> LinksesProperty
        {
            get
            {
                return linkses ?? (linkses = new ObservableCollection<Links>());
            }

            set
            {
                if (linkses == value)
                {
                    return;
                }

                linkses = value;
                OnPropertyChanged(nameof(LinksesProperty));
            }
        }

        public Visibility LinksesVisible
        {
            get
            {
                if (LinksesProperty.Any())
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Sets and gets Максимальное значение задачи. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public double MaxValueOfTaskProperty
        {
            get
            {
                return StaticMetods.Config.MaxTaskValue;
            }

            set
            {
                return;
                if (maxValueOfTask == value)
                {
                    return;
                }

                value = value <= 0 ? 1 : value;

                maxValueOfTask = value;
                OnPropertyChanged(nameof(MaxValueOfTaskProperty));
                OnPropertyChanged(nameof(Progress));

                ChangeValuesOfRelaytedItems();
            }
        }

        /// <summary>
        /// Количество раз (просто число, например - отжаться 10 раз)
        /// </summary>
        public int Mesure
        {
            get
            {
                return _mesure;
            }
            set
            {
                if (value == _mesure) return;
                MesureIs = 0;
                _mesure = value;
                OnPropertyChanged(nameof(Mesure));
                OnPropertyChanged(nameof(PlusNameOf));
                OnPropertyChanged(nameof(MesureVisibility));
                OnPropertyChanged(nameof(LastMesure));
            }
        }

        /// <summary>
        /// Значение значения счетчика задачи
        /// </summary>
        public int MesureIs
        {
            get
            {
                return _mesureIs;
            }
            set
            {
                if (value == _mesureIs) return;
                _mesureIs = value;
                OnPropertyChanged(nameof(MesureIs));
                OnPropertyChanged(nameof(PlusNameOf));
            }
        }

        /// <summary>
        /// Видимость настройки счетчика
        /// </summary>
        public Visibility MesureSettingsVisibility
        {
            get
            {
                return Visibility.Collapsed;
                if (Recurrense.TypeInterval == TimeIntervals.Нет)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        public Visibility MesureVisibility
        {
            get
            {
                if (Recurrense.TypeInterval != TimeIntervals.Нет && Mesure > 0 && MesureIs <= Mesure)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public double MiliSecsDoneForSort
        {
            get
            {
                return _miliSecsDoneForSort;
            }
            set
            {
                if (value.Equals(_miliSecsDoneForSort)) return;
                if (value < 0) value = 0;
                _miliSecsDoneForSort = value;
                OnPropertyChanged(nameof(MiliSecsDoneForSort));
            }
        }

        /// <summary>
        /// Название группы в которую входит задача
        /// </summary>
        public string NameOfGroup
        {
            get
            {
                return _nameOfGroup;
            }
            set
            {
                if (value == _nameOfGroup) return;
                _nameOfGroup = value;
                OnPropertyChanged(nameof(NameOfGroup));
            }
        }

        /// <summary>
        /// Следующие действия для задачи
        /// </summary>
        public ObservableCollection<Task> NextActions { get; set; }

        /// <summary>
        /// Не сделанные подзадачи
        /// </summary>
        public IEnumerable<SubTask> NotDoneSubtasks
        {
            get { return SubTasks.Where(n => n.isDone == false); }
        }

        /// <summary>
        /// Первая не сделанная задача
        /// </summary>
        public IEnumerable<SubTask> NotDoneSubtasksFirstTasks
        {
            get { return SubTasks.Where(n => n.isDone == false).Take(1); }
        }

        /// <summary>
        /// Sets and gets Не повторять подзадачи по умолчанию. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool NotRepeatSubtasksForDefoultProperty
        {
            get
            {
                return notRepeatSubtasksForDefoult;
            }

            set
            {
                if (notRepeatSubtasksForDefoult == value)
                {
                    return;
                }

                notRepeatSubtasksForDefoult = value;
                OnPropertyChanged(nameof(NotRepeatSubtasksForDefoultProperty));
            }
        }

        /// <summary>
        /// Прозрачность (иногда нужно)
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public double Opacity
        {
            get
            {
                if (IsDelProperty)
                {
                    return 0.5;
                }
                return 1;
            }
        }

        /// <summary>
        /// Родительские задачи. Пока применяется только для скиллов.
        /// </summary>
        public List<Task> ParrentTasks
        {
            get
            {
                return (from task in StaticMetods.PersProperty.Tasks
                        where task.NextActions.Any(n => n == this)
                        select task).ToList();
            }
        }

        /// <summary>
        /// Путь к картинке. Так. На всякий случай.
        /// </summary>
        public string PathToIm
        {
            get
            {
                string enamiesDirectory = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Enamies"));
                string path = Path.Combine(enamiesDirectory, _pathToIm);

                if (File.Exists(Path.Combine(_pathToIm, path)) == false)
                    GetEnamyImage();

                return Path.Combine(enamiesDirectory, _pathToIm);
            }
            set
            {
                if (value == _pathToIm) return;
                _pathToIm = value;
                OnPropertyChanged(nameof(PathToIm));
            }
        }

        /// <summary>
        /// Плюс к опыту
        /// </summary>
        public int PlusExp
        {
            get
            {
                return _plusExp;
            }
            set
            {
                if (value == _plusExp) return;
                _plusExp = value;
                OnPropertyChanged(nameof(PlusExp));
                RefreshRev();
            }
        }

        /// <summary>
        /// Плюс к золоту
        /// </summary>
        public int PlusGold
        {
            get
            {
                return _plusGold;
            }
            set
            {
                if (value == _plusGold) return;
                _plusGold = value;
                OnPropertyChanged(nameof(PlusGold));
                RefreshRev();
            }
        }

        public string plusName3
        {
            get
            {
                return _plusName3;
            }
            set
            {
                if (value == _plusName3) return;
                _plusName3 = value;
                OnPropertyChanged(nameof(plusName3));
                OnPropertyChanged(nameof(PlusNameOf3));
                OnPropertyChanged(nameof(PlusNameOf));
            }
        }

        public string PlusNameOf
        {
            get
            {
                var plusName = GetPlusName(true);
                return plusName;
            }
        }

        /// <summary>
        /// Плюс к имени полное
        /// </summary>
        public string PlusNameOf2
        {
            get
            {
                var plusName = GetPlusName(true, true);
                return plusName;
            }
        }

        /// <summary>
        /// Плюс к имени полное
        /// </summary>
        public string PlusNameOf3
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(State))
                {
                    if (!string.IsNullOrWhiteSpace(NameOfProperty))
                    {
                        return " " + State;
                    }
                    else
                    {
                        return State;
                    }
                }
                //if (IsWordHardness)
                //{
                //    return $" ({plusName3})";
                //}
                return string.Empty;
            }
        }

        public string PlusNameOfLast
        {
            get
            {
                var plusName = GetPlusName(true, false, true);
                return plusName;
            }
        }

        /// <summary>
        /// Запуск таймера помодорро
        /// </summary>
        public RelayCommand PomodoroTimerStartCommand
        {
            get
            {
                return pomodoroTimerStartCommand ?? (pomodoroTimerStartCommand = new RelayCommand(
                    () =>
                    {
                        if (pomodorroTimer != null && pomodorroTimer.IsEnabled)
                        {
                            //MessageBox.Show(new Form {TopMost = true}, $"Pomodoro остановлен!!!");
                            PomodorroTimerStop();
                        }
                        else
                        {
                            //MessageBox.Show(new Form {TopMost = true}, $"Pomodoro запущен!!!");
                            PomodorroTimerStart();
                        }

                        OnPropertyChanged(nameof(IsPomodorroTimerStart));
                        OnPropertyChanged(nameof(PomodorroVisibillity));
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Максимальное количество помидоров
        /// </summary>
        public int PomodorroMax
        {
            get
            {
                return _pomodorroMax;
            }
            set
            {
                if (value == _pomodorroMax) return;
                _pomodorroMax = value;
                OnPropertyChanged(nameof(PomodorroVisibillity));
                OnPropertyChanged(nameof(PomodorroMax));
                OnPropertyChanged(nameof(PlusNameOf));
                PomodorroNow = 0;
            }
        }

        /// <summary>
        /// Пройдено помидоров
        /// </summary>
        public int PomodorroNow
        {
            get
            {
                return _pomodorroNow;
            }
            set
            {
                if (value == _pomodorroNow) return;
                _pomodorroNow = value;
                OnPropertyChanged(nameof(PomodorroNow));
                OnPropertyChanged(nameof(PlusNameOf));
            }
        }

        /// <summary>
        /// Прошло времени в таймере помидоров
        /// </summary>
        public double PomodorroTimerTime
        {
            get
            {
                return _pomodorroTimerTime;
            }
            set
            {
                if (value == _pomodorroTimerTime) return;
                _pomodorroTimerTime = value;
                OnPropertyChanged(nameof(PomodorroTimerTime));
            }
        }

        /// <summary>
        /// Таймер помидорро точный
        /// </summary>
        public double PomodorroTinerDouble

        {
            get
            {
                return _pomodorroTinerDouble;
            }
            set
            {
                if (value.Equals(_pomodorroTinerDouble)) return;
                _pomodorroTinerDouble = value;
                OnPropertyChanged(nameof(PomodorroTinerDouble));
            }
        }

        /// <summary>
        /// Видимость таймера помодорро
        /// </summary>
        public Visibility PomodorroVisibillity
        {
            get { return PomodorroMax != 0 && PomodorroNow < PomodorroMax ? Visibility.Visible : Visibility.Collapsed; }
        }

        public double Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                if (value.Equals(_priority)) return;
                _priority = value;
                OnPropertyChanged(nameof(Priority));
                OnPropertyChanged(nameof(PlusNameOf));
            }
        }

        public double Progress
        {
            get
            {
                if (IsDelProperty)
                {
                    return 1;
                }

                return Convert.ToDouble(ValueOfTaskProperty) / Convert.ToDouble(MaxValueOfTaskProperty);
            }
        }

        /// <summary>
        /// Gets the Быстро задать дату (начала или срока).
        /// </summary>
        public RelayCommand<string> QwickSetDateAimCommand
        {
            get
            {
                return qwickSetDateCommand
                       ?? (qwickSetDateCommand =
                           new RelayCommand<string>(
                               item =>
                               {
                                   switch (item)
                                   {
                                       case "началоС":
                                           BeginDateProperty = MainViewModel.selectedTime;
                                           break;

                                       case "конецС":
                                           EndDate = MainViewModel.selectedTime;
                                           break;

                                       case "началоЗ":
                                           BeginDateProperty = MainViewModel.selectedTime.AddDays(1);
                                           break;

                                       case "конецЗ":
                                           EndDate = MainViewModel.selectedTime.AddDays(1);
                                           break;
                                   }

                                   RefreshTaskEndDateForeground();
                               },
                               item =>
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
        /// Ярость. Чем больше, тем больше штраф за пропуски задачи.
        /// </summary>
        public int Rage
        {
            get
            {
                return _rage;
            }
            set
            {
                if (value == _rage) return;
                if (value < 0) value = 0;
                _rage = value;
                OnPropertyChanged(nameof(Rage));
            }
        }

        /// <summary>
        /// Строковое представление даты
        /// </summary>
        public string RecurrenceString
        {
            get
            {
                switch (Recurrense.TypeInterval)
                {
                    case TimeIntervals.Будни:
                        return "Пн-Пт";

                    case TimeIntervals.Выходные:
                        return "Сб-Вс";

                    case TimeIntervals.Сразу:
                        return "Сразу";

                    case TimeIntervals.День:
                    case TimeIntervals.ДниСначала:
                        return $"{Recurrense.Interval} дн.";

                    case TimeIntervals.Ежедневно:
                        return "Ежедн.";

                    case TimeIntervals.Неделя:
                    case TimeIntervals.НеделиСНачала:
                        return $"{Recurrense.Interval} нед.";

                    case TimeIntervals.Месяц:
                    case TimeIntervals.МесяцыСНачала:
                        return $"{Recurrense.Interval} мес.";

                    case TimeIntervals.ДниНедели:
                    case TimeIntervals.ДниНеделиСНачала:
                        var rec = string.Empty;
                        return DaysOfWeekRepeats.Where(n => n.CheckedProperty)
                            .Aggregate(rec,
                                (current, daysOfWeekRepeat) => current + daysOfWeekRepeat.NameDayOfWeekProperty + "; ");
                    case TimeIntervals.Три:
                        return "пн, ср, пт";

                    case TimeIntervals.Четыре:
                        return "пн, вт, чт, пт";

                    case TimeIntervals.Шесть:
                        return "пн-сб";
                }
                return "Нет";
            }
        }

        /// <summary>
        /// Gets or sets Как повторяется задача
        /// </summary>
        public TypeOfRecurrense Recurrense { get; set; }

        /// <summary>
        /// На что влияет эта задача
        /// </summary>
        public IEnumerable<TaskRelaysItem> RelaysItems
        {
            get
            {
                var taskChangeAbilitis =
                    StaticMetods.PersProperty.Abilitis.Where(
                        n => n.IsEnebledProperty && n.NeedTasks.Any(q => q.TaskProperty == this))
                        .Distinct()
                        .ToList();

                var relayAbilitis =
                    taskChangeAbilitis.Select(
                        n =>
                            new TaskRelaysItem
                            {
                                TypeProperty = "навык",
                                Tag = "Навык \"" + n.NameOfProperty + "\"",
                                LevelProperty = n.LevelVisibleProperty,
                                IsLevelVisibleProperty = true,
                                NameProperty = n.NameOfProperty,
                                ImageProperty = n.ImageProperty,
                                RangNameProperty = n.RangName,
                                GuidProperty = n.GUID,
                                DescProperty = n.DescriptionProperty,
                                ValProperty = n.LevelVisibleProperty,
                                ValMinProperty = 0,
                                ValMaxProperty = n.MaxLevelProperty
                            }).ToList();

                // Квесты, связанные с этими наыками
                //var relatedQwestsAbs = from abs in taskChangeAbilitis from needAim in abs.NeedAims.Where(n => n.AimProperty.IsActiveProperty) select needAim.AimProperty;

                // Квесты, связанные с задачей напрямую
                var relatedQwests = from qw in StaticMetods.PersProperty.Aims.Where(n => n.IsActiveProperty)
                                    where qw.NeedsTasks.Any(n => n.TaskProperty == this)
                                    select qw;

                var relayQwests =
                    relatedQwests.Distinct()
                        .Select(
                            n =>
                                new TaskRelaysItem
                                {
                                    IsLevelVisibleProperty = false,
                                    TypeProperty = "квест",
                                    Tag = "Квест \"" + n.NameOfProperty + "\"",
                                    NameProperty = n.NameOfProperty,
                                    ImageProperty = n.ImageProperty,
                                    GuidProperty = n.GUID,
                                    DescProperty = n.DescriptionProperty,
                                    ValProperty = n.AutoProgressValueProperty,
                                    ValMinProperty = 0,
                                    ValMaxProperty = 100
                                })
                        .ToList();

                var relays = relayAbilitis.Union(relayQwests);

                return relays;
            }
        }

        /// <summary>
        /// Видимость того на что влияет задача
        /// </summary>
        public Visibility RelaysVisibility
        {
            get { return RelaysItems.Any() ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Задача влияет на квесты...
        /// </summary>
        public List<Aim> RelToQwests
        {
            get
            {
                return StaticMetods.PersProperty.Aims.Where(n => n.NeedsTasks.Any(q => q.TaskProperty == this)).ToList();
            }
        }

        /// <summary>
        /// Повторять подзадачи
        /// </summary>
        public bool RepeatSubTasks
        {
            get
            {
                return _repeatSubTasks;
            }
            set
            {
                if (value == _repeatSubTasks) return;
                _repeatSubTasks = value;
                OnPropertyChanged(nameof(RepeatSubTasks));
            }
        }

        /// <summary>
        /// Требования задачи чтобы она была видна
        /// </summary>
        public string Reqvirements
        {
            get
            {
                return _reqvirements;
            }
            set
            {
                if (value == _reqvirements) return;
                _reqvirements = value;
                OnPropertyChanged(nameof(Reqvirements));
                OnPropertyChanged(nameof(IsReqVisibility));
            }
        }

        /// <summary>
        /// Секунды выполнения
        /// </summary>
        public long SecondOfDone
        {
            get
            {
                if (_secondOfDone == 0)
                {
                    _secondOfDone = GetSecOfDone();
                }

                return _secondOfDone;
            }
            set
            {
                if (value == _secondOfDone) return;
                _secondOfDone = value;
                OnPropertyChanged(nameof(SecondOfDone));
            }
        }

        /// <summary>
        /// Индекс в выбранных задачах
        /// </summary>
        public int SelFocTasksIndex
        {
            get
            {
                return _selFocTasksIndex;
            }
            set
            {
                if (value == _selFocTasksIndex) return;
                _selFocTasksIndex = value;
                OnPropertyChanged(nameof(SelFocTasksIndex));
            }
        }

        /// <summary>
        /// Прибавляется к названию задачи.
        /// </summary>
        public string State
        {
            get => _state; set
            {
                _state = value;
                OnPropertyChanged(nameof(PlusNameOf));
            }
        }

        /// <summary>
        /// Подзадачи
        /// </summary>
        public ObservableCollection<SubTask> SubTasks
        {
            get
            {
                if (_subTasks == null)
                {
                    _subTasks = new ObservableCollection<SubTask>();
                }

                return _subTasks;
            }
            set
            {
                _subTasks = value;
            }
        }

        /// <summary>
        /// Тип повторения для подзадач
        /// </summary>
        public typeOfSubTaskRecurrenses SubTasksRec
        {
            get
            {
                return _subTasksRec;
            }
            set
            {
                if (value == _subTasksRec) return;
                _subTasksRec = value;
                OnPropertyChanged(nameof(SubTasksRec));
            }
        }

        public string SubTasksString
        {
            get
            {
                if (SubTasks.Any(n => !n.isDone))
                {
                    var aggregate = SubTasks.Where(n => !n.isDone)
                        .Aggregate(string.Empty, (current, subTask) => current + $"{subTask.Tittle}; ");
                    return $" [{aggregate}]";
                }
                return string.Empty;
            }
        }

        public string SubTasksStringAb
        {
            get
            {
                if (SubTasks.Any(n => !n.isDone))
                {
                    var aggregate = SubTasks.Aggregate(string.Empty,
                        (current, subTask) => current + $"{subTask.Tittle}; ");
                    return $" [{aggregate}]";
                }
                return string.Empty;
            }
        }

        public Visibility SubTasksVisibility
        {
            get
            {
                if (NotDoneSubtasksFirstTasks.Any())
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Дополнительное название
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Контекст для задачи
        /// </summary>
        public Context TaskContext { get; set; }

        /// <summary>
        /// Задача влияет на скиллы...
        /// </summary>
        public List<AbilitiModel> TaskInAbilitis
        {
            get
            {
                if (_taskInAbilitis == null)
                {
                    _taskInAbilitis = new List<AbilitiModel>();
                }
                return _taskInAbilitis;
            }
            set
            {
                _taskInAbilitis = value;
            }

            //var inAbs = new List<AbilitiModel>();
            //foreach (
            //    var abilitiModel in
            //        StaticMetods.PersProperty.Abilitis.Where(n => n.RelatedTasks().Any(q => q == this)))
            //{
            //    inAbs.Add(abilitiModel);
            //}
            //return inAbs.Distinct().ToList();
        }

        /// <summary>
        /// Индекс задачи для экспорта в андроид
        /// </summary>
        public int TaskIndex { get; set; }

        public int TaskLevel
        {
            get
            {
                //if (_taskLevel < 1)
                //{
                //    _taskLevel = 1;
                //}
                return _taskLevel;
            }
            set
            {
                if (value == _taskLevel) return;
                _taskLevel = value;
                //GetEnamyImage();
                OnPropertyChanged(nameof(TimeMustProperty));
                OnPropertyChanged(nameof(TaskLevel));
            }
        }

        /// <summary>
        /// Ранги для задач
        /// </summary>
        public ObservableCollection<Rangs> TaskRangs { get; set; }

        /// <summary>
        /// Состояния задачи - название меняется в соответствие с уровнями навыка.
        /// </summary>
        public ObservableCollection<SubTask> TaskStates
        {
            get
            {
                return _taskState ?? (_taskState = new ObservableCollection<SubTask>());
            }
            set => _taskState = value;
        }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public StatusTask TaskStatus
        {
            get
            {
                if (_taskStatus == null)
                {
                    var newType = StaticMetods.PersProperty.Statuses.FirstOrDefault();
                    newType = Recurrense.TypeInterval == TimeIntervals.Нет
                        ? StaticMetods.PersProperty.Statuses.FirstOrDefault(n => n.NameOfStatus == "Планируется")
                        : StaticMetods.PersProperty.Statuses.FirstOrDefault(n => n.NameOfStatus == "Первым делом");

                    _taskStatus = newType;
                    OnPropertyChanged(nameof(EndDateForeground));
                    OnPropertyChanged(nameof(Damage));
                }

                return _taskStatus;
            }
            set
            {
                if (Equals(value, _taskStatus)) return;
                _taskStatus = value;
                OnPropertyChanged(nameof(TaskStatus));
                OnPropertyChanged(nameof(EndDateForeground));
                OnPropertyChanged(nameof(Damage));
            }
        }

        /// <summary>
        /// Gets or sets Тип задачи
        /// </summary>
        public TypeOfTask TaskType { get; set; }

        /// <summary>
        /// Времена завершения задачи
        /// </summary>
        public ObservableCollection<TimeClass> TimeEnds
        {
            get
            {
                if (_timeEnd == null)
                {
                    RefreshTimeEnds();
                }

                return _timeEnd;
            }
            set
            {
                if (Equals(value, _timeEnd)) return;
                _timeEnd = value;
                OnPropertyChanged(nameof(TimeEnds));
            }
        }

        /// <summary>
        /// Sets and gets Затраченное время на задачу в минутах. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int TimeIsProperty
        {
            get
            {
                return timeIs;
            }

            set
            {
                if (timeIs == value)
                {
                    return;
                }

                timeIs = value;
                OnPropertyChanged(nameof(TimeIsProperty));
            }
        }

        /// <summary>
        /// Время когда последний раз кликали по задачи (для умной сортировки)
        /// </summary>
        public DateTime TimeLastDone
        {
            get
            {
                if (string.IsNullOrWhiteSpace(timeLastDone))
                {
                    timeLastDone = DateTime.MaxValue.ToString();
                }
                return StaticMetods.GetDateFromString(timeLastDone);
            }
            set
            {
                timeLastDone = value.ToString();
                OnPropertyChanged(nameof(TimeLastDone));
            }
        }

        /// <summary>
        /// Sets and gets Ориентировочное время на выполнение задачи в минутах. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public int TimeMustProperty
        {
            get
            {
                return timeMust;
            }

            set
            {
                if (timeMust == value)
                {
                    return;
                }

                timeMust = value;
                TimerValueProperty = 0;
                OnPropertyChanged(nameof(TimeMustProperty));
                OnPropertyChanged(nameof(TimerVisibility));
                OnPropertyChanged(nameof(PlusNameOf));
                OnPropertyChanged(nameof(TimeProperty));
                OnPropertyChanged(nameof(TimeString));
                OnPropertyChanged(nameof(LastTimer));
            }
        }

        public DateTime TimeProperty
        {
            get
            {
                if (StaticMetods.PersProperty.PersSettings.IsSmartTaskSort)
                {
                    return new DateTime(2001, 1, 1, 23, 59, 0);
                }
                return StaticMetods.GetDateFromString(timeOfTask);
            }
            set
            {
                timeOfTask = value.ToString();
                OnPropertyChanged(nameof(TimeProperty));
                OnPropertyChanged(nameof(TimeString));
            }
        }

        /// <summary>
        /// Sets and gets Таймер активен?. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Visibility TimerActiveProperty
        {
            get
            {
                return timerActive;
            }

            set
            {
                if (timerActive == value)
                {
                    return;
                }

                timerActive = value;
                OnPropertyChanged(nameof(TimerActiveProperty));
            }
        }

        /// <summary>
        /// Цвет рамки таймера
        /// </summary>
        public Brush TimerBorder
        {
            get
            {
                if (IsTimerStartProperty)
                {
                    return Brushes.Gold;
                }
                return Brushes.Transparent;
            }
        }

        /// <summary>
        /// Видимость настройки таймера и времени
        /// </summary>
        public Visibility TimerSettingsVisibility
        {
            get
            {
                return Visibility.Collapsed;
                if (Recurrense.TypeInterval == TimeIntervals.Нет)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets and gets Значение таймера. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int TimerValueProperty
        {
            get
            {
                return timerValue;
            }

            set
            {
                if (timerValue == value || value < 0)
                {
                    return;
                }

                timerValue = value;
                OnPropertyChanged(nameof(TimerVisibility));
                OnPropertyChanged(nameof(TimerValueProperty));
                OnPropertyChanged(nameof(PlusNameOf));
            }
        }

        /// <summary>
        /// Видимость таймера
        /// </summary>
        public Visibility TimerVisibility
        {
            get
            {
                return TimeMustProperty != 0 && TimerValueProperty <= TimeMustProperty
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        public string TimeString
        {
            get { return GetStringTimeConverter.GetTimeTask(this); }
        }

        /// <summary>
        /// Для наград за элемент
        /// </summary>
        public ucElementRewardsViewModel UcElementRewardsViewModel
        {
            get { return new ucElementRewardsViewModel(this); }
        }

        public ucSetGoldExpRevardViewModel UcSetGoldExpRevardViewModel
        {
            get
            {
                return new ucSetGoldExpRevardViewModel() { Task = this };
            }
        }

        /// <summary>
        /// Sets and gets Значение задачи. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public double ValueOfTaskProperty
        {
            get
            {
                return Math.Round(valueOfTask, 2);
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > MaxValueOfTaskProperty)
                {
                    value = MaxValueOfTaskProperty;
                }

                if (valueOfTask == value)
                {
                    return;
                }

                valueOfTask = value;

                OnPropertyChanged(nameof(ValueOfTaskProperty));
                OnPropertyChanged(nameof(Progress));
                ChangeValuesOfRelaytedItems();
                //TaskLevel = RecountTaskLevel(this);
            }
        }

        /// <summary>
        /// Волна. Для сортировки когда приоритет
        /// </summary>
        public int Wave

        {
            get
            {
                if (Recurrense.TypeInterval == TimeIntervals.Нет)
                    _wave = 0;

                return _wave;
            }
            set
            {
                if (value < 0) value = 0;
                if (value == _wave) return;
                var tscs = StaticMetods.Locator.MainVM.Tasks;
                tscs.EditItem(this);
                _wave = value;
                OnPropertyChanged(nameof(Wave));
                OnPropertyChanged(nameof(EndDateForeground));
                tscs.CommitEdit();
                tscs.MoveCurrentTo(this);
            }
        }

        /// <summary>
        /// Gets the data type.
        /// </summary>
        Type IDragable.DataType
        {
            get
            {
                {
                    return typeof(Task);
                }
            }
        }

        /// <summary>
        /// Gets the data type.
        /// </summary>
        Type IDropable.DataType
        {
            get { return typeof(Task); }
        }

        public static double AbIncreaseFormula(AbilitiModel ab, double? directVal = null)
        {
            double abCellValue = ab.CellValue;

            if (directVal.HasValue)
                abCellValue = Math.Floor(directVal.Value);

            int levels = StaticMetods.PersProperty.BalanceLevels;
            double xpForFirstLevel = StaticMetods.PersProperty.BalanceForFirstLevel;
            double xpForLastLevel = StaticMetods.PersProperty.BalanceForLastLevel;

            double b = Math.Log(xpForLastLevel / xpForFirstLevel) / (levels - 1);
            double a = xpForFirstLevel / (Math.Exp(b) - 1.0);

            double old = a * Math.Exp(b * (abCellValue));
            double newExp = a * Math.Exp(b * (abCellValue + 1));
            double abIncreaseFormula = newExp - old;

            return 1 / abIncreaseFormula;
        }

        /// <summary>
        /// Добавить задачу
        /// </summary>
        /// <param name="taskType">Тип задачи</param>
        /// <param name="abil"></param>
        /// <param name="aim"></param>
        /// <param name="needLevel"></param>
        /// <returns>Нажато ОК?</returns>
        public static Tuple<bool, Task> AddTask(
            TypeOfTask taskType,
            AbilitiModel abil = null,
            Aim aim = null, int needLevel = 0, string nameOf=null
            )
        {
            if (CompareIsTaskTypesIs())
            {
                return null;
            }

            if (taskType == null)
            {
                taskType = StaticMetods.PersProperty.TasksTypes.First();
            }

            var addTask = new AddOrEditTaskView();
            var isOkPressed = false;
            addTask.UcTasksSettingsView.btnOk.Visibility = Visibility.Visible;
            addTask.UcTasksSettingsView.btnCansel.Visibility = Visibility.Visible;

            var context = (UcTasksSettingsViewModel)addTask.UcTasksSettingsView.DataContext;

            Action okClick = () =>
            {
                addTask.Close();
                isOkPressed = true;
            };

            addTask.UcTasksSettingsView.btnCansel.Click += (sender, args) =>
            {
                context.SelectedTaskProperty.Delete(StaticMetods.PersProperty);
                StaticMetods.Locator.MainVM.RefreshTasksInMainView();
                addTask.Close();
                isOkPressed = false;
            };

            addTask.UcTasksSettingsView.btnOk.Click += (sender, args) => { okClick.Invoke(); };

            context.AddNewTask(taskType, nameOf);

            var selectedTask = context.SelectedTaskProperty;
            //if (needLevel!=0)
            //{
            //    selectedTask.TaskLevel = needLevel;
            //}

            if (abil != null)
            {
                taskSettingForAbility(selectedTask, abil, needLevel);
            }

            if (aim != null)
            {
                taskSettingForQwest(selectedTask, aim);
            }

            context.RefreshInfoCommand.Execute(null);

            var typeInterval = selectedTask.Recurrense.TypeInterval;

            context.TaskBalanceDefoults();

            selectedTask.OnPropertyChanged(nameof(CounterSettingsVisibility));
            selectedTask.OnPropertyChanged(nameof(TimerSettingsVisibility));
            selectedTask.OnPropertyChanged(nameof(IsShildVisible));

            // Добавляем хоткей
            var saveCommand = new RelayCommand(() => { okClick.Invoke(); });
            addTask.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.S, ModifierKeys.Control)));

            StaticMetods.Locator.MainVM.RefreshRelations();
            StaticMetods.RecountTaskLevels();

            if (selectedTask.Recurrense.TypeInterval == TimeIntervals.Нет)
            {
                selectedTask.UcSetGoldExpRevardViewModel.SetRewCommand.Execute("простоGold");
            }

            addTask.ShowDialog();

            StaticMetods.Locator.MainVM.RefreshTasksPriority(true);

            if (isOkPressed && selectedTask != null)
            {
            }

            return new Tuple<bool, Task>(isOkPressed, selectedTask);
        }

        

        /// <summary>
        /// Получить на что ссылаются эти задачи
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static void GetLinksOfTasks(IEnumerable<Task> tasks)
        {
            if (StaticMetods.PersProperty == null)
            {
                return;
            }

            foreach (Task tsk in tasks)
            {
                SetLinkedTasks(tsk);
                foreach (var linkedTask in tsk.LinkedTasks)
                {
                    SetLinkedTasks(tsk);
                }
            }
        }

        public static DateTime GetNextBegin(Task task)
        {
            var selectedTime = MainViewModel.selectedTime;
            var recur = task.Recurrense.TypeInterval;
            var interval = task.Recurrense.Interval;
            var curTaskBegin = task.BeginDateProperty;

            switch (recur)
            {
                case TimeIntervals.Нет:
                    return curTaskBegin;

                case TimeIntervals.Сразу:
                    return curTaskBegin;

                case TimeIntervals.Ежедневно:
                    return selectedTime.AddDays(1);

                case TimeIntervals.Будни:
                    return addDaysOfWeek(GetWorkDays(), selectedTime);

                case TimeIntervals.Выходные:
                    return addDaysOfWeek(GetHolyDays(), selectedTime);

                case TimeIntervals.День:
                case TimeIntervals.ДниСначала:
                    return selectedTime.AddDays(interval);

                case TimeIntervals.ДниНедели:
                case TimeIntervals.ДниНеделиСНачала:
                    return addDaysOfWeek(task.DaysOfWeekRepeats, selectedTime);

                case TimeIntervals.Неделя:
                case TimeIntervals.НеделиСНачала:
                    return addWeeks(task.Recurrense, selectedTime);

                default:
                    return curTaskBegin;
            }

            return curTaskBegin;
        }

        public static double getPrioryty(Pers prs, Task task, out AbilitiModel ab, AbilitiModel abFind = null)
        {
            AbilitiModel ab1;
            if (abFind != null)
                ab1 = abFind;
            else
                ab1 = prs.Abilitis.FirstOrDefault(n => n.NeedTasks.Any(q => q.TaskProperty == task));

            ab = ab1;
            if (ab1 == null)
            {
                return 0;
            }

            var chAb = task.TaskAbylityChangeVal(ab) * task.BoosterOfDone;
            var abSymulate = new AbilitiModel() {ValueProperty = ab.ValueProperty };
            abSymulate.ValueIncrease(chAb, abSymulate.ValueProperty);
            var expToPers = prs.RetExp(ab, abSymulate.ValueProperty) - prs.PersExpProperty;

            return expToPers;

            //double k = task.KChangeValue(1);
            //double countThisLevNeeds = ab1.CountThisLevNeeds();
            //double priorOfAb = ab1.getPriorOfAb(ab1);
            //var prioryty = (priorOfAb * k) / countThisLevNeeds;
            //return prioryty;

            //double sum = 0;
            //foreach (var characteristic in chas)
            //{
            //    sum += (1.0/(ab1.CellValue + 1)*
            //            (characteristic.NeedAbilitisProperty.First(n => n.AbilProperty == ab1).KoeficientProperty)/
            //            (characteristic.NeedAbilitisProperty.Sum(n => n.KoeficientProperty)))
            //           *(prs.PersSettings.MaxChaLev - characteristic.FirstVal);
            //}
            //return sum;
        }

        public static long GetSecOfDone()
        {
            return 0;
            var start = new DateTime(DateTime.Now.Year, 01, 01);
            var end = DateTime.Now;
            var seconds = (end - start).TotalMilliseconds;
            return (long)seconds;
        }

        public static int LevelForMonsters()
        {
            var tl = System.Convert.ToInt32(Math.Floor(StaticMetods.PersProperty.PersLevelProperty / 10.0) * 10.0);
            return tl;
        }

        public static void RecountTaskLevel(Task tsk)
        {
            //var taskLevel = (int)Math.Ceiling(StaticMetods.PersProperty.PersLevelProperty / 10.0); //StaticMetods.PersProperty.Stage;
            //if (tsk.IsChalange) taskLevel++;
            //if (taskLevel < 1) taskLevel = 1;
            //if (taskLevel > StaticMetods.MaxPersAndMonstersRangs)
            //    taskLevel = StaticMetods.MaxPersAndMonstersRangs;

            var tl = LevelForMonsters();
            tsk.TaskLevel = tl; ; //taskLevel;

            return;
            //var pProgress = StaticMetods.PersProperty.PersLevelProperty /
            //                Convert.ToDouble(StaticMetods.PersProperty.MaxLevelProperty);

            //int lvl = 1;
            //if (tsk.TaskInAbilitis.Any())
            //{
            //    var cell =
            //        Convert.ToInt32(
            //            tsk.TaskInAbilitis.SelectMany(n => n.NeedTasks)
            //                .Where(n => n.TaskProperty == tsk)
            //                .Min(q => q.LevelProperty)); //Convert.ToInt32(tsk.TaskInAbilitis.Min(n => n.CellValue));
            //    lvl = cell;
            //}
            //var ab = StaticMetods.PersProperty.Abilitis;
            //if (ab?.Any() == true)
            //{
            //    lvl = Convert.ToInt32(ab.Min(n => n.CellValue));
            //}
            ////lvl = Convert.ToInt32(Math.Floor(StaticMetods.PersProperty.PersLevelProperty / 10.0) + 1);

            //if (lvl < 1) lvl = 1;

            ////lvl = lvl + tsk.Rage;
            ////lvl = Convert.ToInt32(Math.Floor(StaticMetods.PersProperty.PersLevelProperty / 10.0) + 1);

            //if (lvl > 5) lvl = 5;

            //tsk.TaskLevel = lvl;

            //int lvl;
            //if (StaticMetods.PersProperty.PersLevelProperty <= 10)
            //{
            //    lvl = 1;
            //}
            //else if (StaticMetods.PersProperty.PersLevelProperty <= 20)
            //{
            //    lvl = 2;
            //}
            //else if (StaticMetods.PersProperty.PersLevelProperty <= 30)
            //{
            //    lvl = 3;
            //}
            //else // максимальный 4-5
            //{
            //    lvl = 4;
            //}
            //tsk.TaskLevel = lvl;

            //var pProgress = StaticMetods.PersProperty.PersLevelProperty/
            //                Convert.ToDouble(StaticMetods.PersProperty.MaxLevelProperty);
            //int lvl;
            //if (pProgress < 0.25)
            //{
            //    lvl = 1;
            //}
            //else if (pProgress < 0.5)
            //{
            //    lvl = 2;
            //}
            //else if (pProgress < 0.75)
            //{
            //    lvl = 3;
            //}
            //else // максимальный 4-5
            //{
            //    lvl = 4;
            //}
            //tsk.TaskLevel = lvl;

            //if (tsk.IsSkill == false)
            //{
            //    // Это если квест или просто задача - зависит от уровня персонажа
            //    double pProgress = StaticMetods.PersProperty.PersLevelProperty / Convert.ToDouble(StaticMetods.PersProperty.MaxLevelProperty);
            //    int lvl;
            //    if (pProgress < 0.15)
            //    {
            //        lvl = 1;
            //    }
            //    else if (pProgress < 0.5)
            //    {
            //        lvl = 2;
            //    }
            //    else if (pProgress < 0.85)
            //    {
            //        lvl = 3;
            //    }
            //    else // максимальный 4-5
            //    {
            //        lvl = 4;
            //    }
            //    tsk.TaskLevel = lvl;
            //}
            //else
            //{
            //    var MinAb = tsk.TaskInAbilitis().OrderBy(n=>n.ValueProperty).First().CellValue;
            //    int lvl;
            //    if (MinAb < 1)
            //    {
            //        lvl = 1;
            //    }
            //    else if (MinAb < 2)
            //    {
            //        lvl = 2;
            //    }
            //    else if (MinAb < 4)
            //    {
            //        lvl = 3;
            //    }
            //    else // максимальный 4-5
            //    {
            //        lvl = 4;
            //    }
            //    tsk.TaskLevel = lvl;
            //}
        }

        /// <summary>
        /// Задать дату окончания
        /// </summary>
        /// <param name="task">задача</param>
        public static void SetEndDate(Task task)
        {
            if (task.Recurrense.TypeInterval == TimeIntervals.Нет)
                task.EndDate = DateTime.MaxValue;
            else
                task.EndDate = task.BeginDateProperty;
        }

        public static string SetPathToMonster(int taskLevel)
        {
            string path = string.Empty;
            var lvl = taskLevel;
            path = StaticMetods.PersProperty.RIG.GetNextImgPath(lvl);
            return path;
        }

        /// <summary>
        /// В каких видах присутствует задача
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static IEnumerable<ViewsModel> TaskInViews(Task task)
        {
            var views = new List<ViewsModel>();

            if (task.Recurrense.TypeInterval != TimeIntervals.Нет)
            {
                views = StaticMetods.PersProperty.Views.Where(n => n.NameOfView == "Привычки").ToList();
            }
            else
            {
                views = StaticMetods.PersProperty.Views.Where(n => n.NameOfView == "Задачи").ToList();
            }
            return views;
        }

        /// <summary>
        /// Настройки задачи если добавлена из скилла
        /// </summary>
        /// <param name="_task">задача</param>
        /// <param name="_abil">скилл</param>
        /// <param name="needLevel"></param>
        public static NeedTasks taskSettingForAbility(Task _task, AbilitiModel _abil, int needLevel, int toLevel = 0)
        {
            if (toLevel == 0)
            {
                toLevel = AbilitiModel.AbMaxLevel;
            }

            var needTasks = new NeedTasks
            {
                TaskProperty = _task,
                KoeficientProperty = StaticMetods.DefoultKForTaskNeed,
                KRel = 6
            };

            needTasks.ToLevelProperty = toLevel;
            needTasks.LevelProperty = needLevel;
            _abil.NeedTasks.Add(needTasks);

            _abil.NoAbsTask = _task;
            _task.NoAbsAb = _abil;

            return needTasks;
        }

        /// <summary>
        /// Добавление задачи из квеста
        /// </summary>
        /// <param name="_task">Задача</param>
        /// <param name="_aim">Квест</param>
        public static void taskSettingForQwest(Task _task, Aim _aim)
        {
            var needTasks = QwestsViewModel.GetDefoultNeedTask(_task);
            _aim.NeedsTasks.Add(needTasks);
        }

        public override void ChangeValuesOfRelaytedItems()
        {
            if (StaticMetods.PersProperty == null)
            {
                return;
            }

            // Расчитываем значения квестов
            var relatedQwests = StaticMetods.PersProperty.Aims.Where(n => n.NeedsTasks.Any(q => q.TaskProperty == this));
            foreach (var relatedQwest in relatedQwests)
            {
                relatedQwest.CountAutoProgress();
            }
        }

        /// <summary>
        /// Нажат + в задаче
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        /// <param name="isPlusClick">Был нажат плюс?</param>
        /// <param name="isToTomorrow">Перенести на завтра?</param>
        public void ClickPlusMinusTomorrowTask(Pers _pers, bool isPlusClick, bool isToTomorrow = false,
            bool doNotShowChanges = false, bool refreshImage = true)
        {
            TimeLastDone = MainViewModel.selectedTime;

            SkillsMayUpModel.GetBefore();

            IsLastClickNotDone = true;
            SecondOfDone = GetSecOfDone();
            var vc = new ViewChangesClass(_pers.InventoryItems.Union(_pers.ShopItems).ToList());
            var editableTask = this;
            TimerRefresh();

            if (!doNotShowChanges)
            {
                vc.GetValBefore();
            }

            // Если просто перенос на завтра
            if (isToTomorrow)
            {
                MoveTaskToTommorow();
                return;
            }

            // Если пропущена?
            if (isPlusClick == false)
            {
                MinusTask(_pers);
                StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.ЗадачаНеВыполнена, this);
            }

            // Если полностью сделана?
            if (isPlusClick)
            {
                PlusTask(_pers);
                StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.ЗадачаВыполнена, this);
            }

            ChangeValuesOfRelaytedItems();

            // Добавляем награду при необходимости
            if (isPlusClick)
                AddRandomeRewardFromTask(_pers, editableTask);

            StaticMetods.refreshShopItems(_pers);

            if (!doNotShowChanges)
            {
                vc.GetValAfter();
            }

            if (!doNotShowChanges)
            {
                // Показываем изменения от задачи
                var header = isPlusClick ? GetCustMotiv() : GetCustFail();

                header = $"{header}";

                Brush col = isPlusClick ? Brushes.Green : Brushes.Red;

                var itemImageProperty =
                    StaticMetods.pathToImage(isPlusClick
                        ? Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png")
                        : Path.Combine(Directory.GetCurrentDirectory(), "Images", "bad.png"));

                if (isPlusClick)
                {
                    StaticMetods.PlaySound(Resources.abLevelUp);
                }

                if (isPlusClick == false)
                {
                    StaticMetods.PlaySound(Resources.Fail);
                }

                vc.ShowChanges(header, col, itemImageProperty, MainViewModel.selectedTime.ToShortDateString());

                showMotivator(
                    _pers.PersSettings.IsTaskDoneMotivatorShowProperty,
                    _pers.PersSettings.PathToMotivatorsProperty,
                    isPlusClick);
            }

            OnPropertyChanged(nameof(EndTime));

            if (refreshImage)
            {
                var beforeLev = TaskLevel;
                RecountTaskLevel(this);
                if (beforeLev == TaskLevel)
                {
                    GetEnamyImage();
                }
            }

            SkillsMayUpModel.ShowAbUps();
        }

        public int CompareTo(object obj)
        {
            var prs = StaticMetods.PersProperty;
            var other = (Task)obj;

            if (prs == null)
                return 0;

            // Сделано
            var done = IsDelProperty.CompareTo(other.IsDelProperty);
            if (done != 0)
                return done;

            // По повторению
            var repCompare = IndexOfRepeat.CompareTo(other.IndexOfRepeat);
            if (repCompare != 0)
                return repCompare;

            //By Wave
            var bw = compareByWave(other);
            if (bw != 0)
                return bw;

            // По наличию квестов
            bool hasTdiThis = MainViewModel.TaskLinkDic.ContainsKey(this);
            bool hasTdiOther = MainViewModel.TaskLinkDic.ContainsKey(other);

            var reltoAny = RelToQwests.Any().CompareTo(other.RelToQwests.Any());
            if (reltoAny != 0)
                return -reltoAny;

            if (hasTdiThis
                && hasTdiOther)
            {
                var tdiThis = MainViewModel.TaskLinkDic[this];
                var tdiOther = MainViewModel.TaskLinkDic[other];

                // По квестам
                if (tdiThis.Qwest != null && tdiOther.Qwest != null && tdiThis.Qwest != tdiOther.Qwest)
                {
                    // Если у обоих есть скиллы, то задачам скиллов сравниваем
                    if (tdiThis.Skills.Any() && tdiOther.Skills.Any())
                    {
                        var skCompare = StaticMetods.PersProperty.Tasks.IndexOf(tdiThis.Skills.FirstOrDefault())
                            .CompareTo(StaticMetods.PersProperty.Tasks.IndexOf(tdiOther.Skills.FirstOrDefault()));
                        if (skCompare != 0)
                            return skCompare;
                    }

                    // По числ важности квестов
                    var qwCmp = GetQwestCompareValue(tdiThis).CompareTo(GetQwestCompareValue(tdiOther));
                    if (qwCmp != 0)
                        return -qwCmp;

                    // По значению квестов
                    var qwPrg = tdiThis.Qwest.AutoProgressValueProperty.CompareTo(tdiOther.Qwest.AutoProgressValueProperty);
                    if (qwPrg != 0)
                        return -qwPrg;
                }
            }

            // Индекс
            var ind =
                StaticMetods.PersProperty.Tasks.IndexOf(this)
                    .CompareTo(StaticMetods.PersProperty.Tasks.IndexOf(other));

            return ind;
        }

        /// <summary>
        /// Числовое значение проверки по квестам.
        /// </summary>
        /// <param name="tdiThis"></param>
        /// <returns></returns>
        private int GetQwestCompareValue(TaskLinkDictItem tdiThis)
        {
            // Есть активные скиллы
            if (tdiThis.HasSkills && tdiThis.Skills.Any())
                return 1;
            // Скиллы выполнены
            if (tdiThis.HasSkills && !tdiThis.Skills.Any())
                return -1;
            // Просто квест
            return 0;
        }

        public void CounterRefresh()
        {
            CounterValuePlusProperty = 0;
            MesureIs = 0;
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        public void Delete(Pers _pers)
        {
            // Удаляем из "следующих действий"
            removeTaskFromNextActions(_pers);

            // Удаляем из требований, скиллов и ссылок квестов
            removeTaskFromAimNeeds(_pers);

            // Удаляем из требований скиллов
            removeTaskFromAbilNeeds(_pers);

            // Обнуляем скиллы
            foreach (var source in _pers.Tasks.Where(n => n.BaseOfSkill == this))
            {
                source.BaseOfSkill = null;
            }

            _pers.Tasks.Remove(this);

            // Обновляем основные элементы игры
            StaticMetods.RecauntAllValues();

            Messenger.Default.Send("ПлюсМинусНажат!");
        }

        /// <summary>
        /// The drop.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        public void Drop(object data, int index)
        {
            var TaskB = this;
            var taskA = data as Task;

            StaticMetods.Locator.MainVM.MoveTask(taskA, TaskB);

            ////Messenger.Default.Send(new MoveTaskMessege {taskA = taskA, taskB = TaskB});
        }

        /// <summary>
        /// Редактировать задачу
        /// </summary>
        public void EditTask()
        {
            var editTask = new AddOrEditTaskView();
            editTask.UcTasksSettingsView.btnOk.Visibility = Visibility.Visible;
            editTask.UcTasksSettingsView.btnCansel.Visibility = Visibility.Collapsed;
            editTask.UcTasksSettingsView.btnOk.Click += (sender, args) => { editTask.Close(); };

            var context = (UcTasksSettingsViewModel)editTask.UcTasksSettingsView.DataContext;

            context.SetSelTask(this);

            // Добавляем хоткей
            var saveCommand = new RelayCommand(() =>
            {
                context.OkAddOrEditCommand.Execute(null);
                editTask.Close();
            });
            editTask.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.S, ModifierKeys.Control)));

            editTask.ShowDialog();

            StaticMetods.Locator.MainVM.Tasks.EditItem(this);
            StaticMetods.Locator.MainVM.Tasks.CommitEdit();
            //StaticMetods.Locator.MainVM.FocTasks.EditItem(this);
            //StaticMetods.Locator.MainVM.FocTasks.CommitEdit();
        }

        /// <summary>
        /// Получить баф хп
        /// </summary>
        /// <returns></returns>
        public int GetBuffHP()
        {
            if (IsSkill == false)
            {
                return 0;
            }

            var lev =
                Convert.ToInt32(StaticMetods.PersProperty.Abilitis.Where(
                    n => n.Skills.Any(q => q.NeedTask.TaskProperty == this))
                    .Max(n => Math.Ceiling((n.ValueProperty + 0.1) / 25.0)));
            lev = lev <= 4 ? lev : 4;
            return lev * 5;
        }

        public override byte[] GetDefoultImageFromElement()
        {
            return null;
            //return DefoultPicsAndImages.DefoultTaskImage;
        }

        /// <summary>
        /// Задать для задачи случайное изображение монстра в соответствии с ее уровнем
        /// </summary>
        public void GetEnamyImage()
        {
            ImageProperty = null;
            if (isDel)
            {
                return;
            }
            //var taskLevel = TaskLevel;
            //var path = SetPathToMonster(taskLevel);
            PathToIm = StaticMetods.PersProperty.RIG2.GetNextImage();
        }

        public string GetPlusName(bool isPlusCounter, bool isForAbs = false, bool getAllLast = false)
        {
            if (Recurrense.TypeInterval == TimeIntervals.Нет)
                return string.Empty;

            var plusName = string.Empty;

            List<string> PlNameOf = new List<string>();
            List<string> PlNameOf2 = new List<string>();

            if (isPlusCounter)
            {
                if (Mesure >= 1 &&
                    Recurrense.TypeInterval != TimeIntervals.Нет)
                {
                    var mesureIs = !isForAbs ? getAllLast ? Mesure : MesureIs : 0;
                    PlNameOf.Add($"{Mesure - mesureIs}✓");
                }
            }

            if (isPlusCounter)
            {
                if (CounterMaxValueProperty > 1 && Recurrense.TypeInterval != TimeIntervals.Сразу &&
                    Recurrense.TypeInterval != TimeIntervals.Нет)
                    PlNameOf.Add($"{CounterMaxValueProperty}");
            }

            if (TimeMustProperty != 0)
            {
                var timerValueProperty = !isForAbs ? getAllLast ? TimeMustProperty : TimerValueProperty : 0;
                PlNameOf.Add($"{TimeMustProperty - timerValueProperty}⧖");
            }

            if (Wave != 0)
            {
                var fod = StaticMetods.PersProperty.PersSettings.WaveNames.FirstOrDefault(n => n.Item1 == Wave);
                if (fod != null)
                {
                    PlNameOf.Add(fod.Item2.ToLower());
                }
            }

            // Чтобы опыт показывать

            var floor = Math.Floor(this.Priority);
            if (floor > 0)
            {
                List<string> tmp = new List<string>();
                tmp.Add($"+{floor} exp");
                PlNameOf2.Add(string.Join(" ", tmp));
            }

            string plusState = string.Empty;
            if (!string.IsNullOrWhiteSpace(State))
            {
                if (!string.IsNullOrWhiteSpace(NameOfProperty))
                {
                    plusState = " ";
                    plusState += State;
                }
                else
                {
                    plusState = State;
                }
            }

            plusName += string.Join(", ", PlNameOf2);

            if (!string.IsNullOrEmpty(plusName))
                plusName = " (" + plusName + ")";

            var pl1 = string.Join(", ", PlNameOf);

            if (!string.IsNullOrEmpty(pl1))
                pl1 = ", " + pl1;

            return plusState + pl1 + plusName;
        }

        /// <summary>
        /// Задача находится в требованиях квестов...
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public IEnumerable<Aim> InQwests(Pers pers)
        {
            return pers.Aims.Where(n => n.NeedsTasks.Any(q => q.TaskProperty == this));
        }

        /// <summary>
        /// Дата подходит к текущему повтору?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeOfRecurrense"></param>
        /// <returns></returns>
        public bool IsDateNotOk(DateTime value, TypeOfRecurrense typeOfRecurrense)
        {
            if (typeOfRecurrense.TypeInterval == TimeIntervals.Будни)
            {
                if (value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }
            }
            else if (typeOfRecurrense.TypeInterval == TimeIntervals.Выходные)
            {
                if (value.DayOfWeek != DayOfWeek.Saturday && value.DayOfWeek != DayOfWeek.Sunday)
                {
                    return true;
                }
            }
            else if (typeOfRecurrense.TypeInterval == TimeIntervals.ДниНедели ||
                     typeOfRecurrense.TypeInterval == TimeIntervals.ДниНеделиСНачала)
            {
                var time = value;
                if (DaysOfWeekRepeats.First(n => n.Day == time.DayOfWeek).CheckedProperty == false)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Индекс изменения значения (для задач у которых повтор не ежедневно!)
        /// </summary>
        public double KChangeValue(double count)
        {
            double k = 1;
            var typeInterval = Recurrense.TypeInterval;
            double interv = Recurrense.Interval;

            switch (typeInterval)
            {
                case TimeIntervals.Ежедневно:
                    // Прирост такой же как и по будням
                    k = 7.0 / 5.0;
                    break;

                case TimeIntervals.Будни:
                    k = 7.0 / 7.0;
                    break;

                case TimeIntervals.Выходные:
                    k = 7.0 / 2.0;
                    break;

                case TimeIntervals.День:
                case TimeIntervals.ДниСначала:
                    k = 7.0 / (7.0 / interv);
                    break;

                case TimeIntervals.ДниНедели:
                case TimeIntervals.ДниНеделиСНачала:
                    interv = DaysOfWeekRepeats.Count(n => n.CheckedProperty);
                    interv = interv != 0 ? interv : 1.0;
                    k = 7.0 / interv;
                    break;
            }

            k = k >= 7.0 / 5.0 ? k : 7.0 / 5.0;

            k = k * (1.0 / 1.4);

            if (k > 7)
            {
                k = 7;
            }

            var kChangeValue = k / count;
            return kChangeValue;
        }

        /// <summary>
        /// Запуск таймера помодорро
        /// </summary>
        public void PomodorroTimerStart()
        {
            pomodorroTimer = new DispatcherTimer { Interval = StaticMetods.timeSpan };
            PomodorroTimerTime = 0;
            PomodorroTinerDouble = 0;
            pomodorroTimer.Tick += (sender, e) =>
            {
                PomodorroTinerDouble += 100.0 / StaticMetods.PersProperty.PersSettings.PomodorroTime;
                PomodorroTimerTime = Math.Round(PomodorroTinerDouble);

                if (PomodorroTinerDouble >= 100.0)
                {
                    MessageBox.Show(new Form { TopMost = true }, $"\"{NameOfProperty}\" +1 pomodoro!!!");
                    PomodorroNow++;
                    PomodorroTimerStop();
                }
            };
            pomodorroTimer.Start();
            IsPomodorroTimerStart = true;
        }

        /// <summary>
        /// Стоп таймера помодорро
        /// </summary>
        public void PomodorroTimerStop()
        {
            pomodorroTimer?.Stop();
            PomodorroTimerTime = 0;
            PomodorroTinerDouble = 0;
            IsPomodorroTimerStart = false;
            OnPropertyChanged(nameof(PomodorroVisibillity));
        }

        public void RecountAutoValues()
        {
            if (!IsWordHardness)
            {
                plusName3 = string.Empty;
            }
            if (AimTimerMax != 0 || AimMesure != 0 || IsWordHardness || TaskStates.Count > 0)
            {
                var needTaskses =
                    StaticMetods.PersProperty.Abilitis.Where(n => n.NeedTasks.Any(q => q.TaskProperty == this))
                        .ToList();

                if (needTaskses.Any())
                {
                    double maxLvl = AbilitiModel.AbMaxLevel;
                    double curLvl = (int)needTaskses.Max(n => n.CellValue) + 1.0;
                    if (curLvl > maxLvl)
                        curLvl = maxLvl;
                    double progressInAb = curLvl / maxLvl;

                    if (TaskStates.Count > 0)
                        AutoRecountSetState(curLvl);
                    else
                        State = string.Empty;

                    if (AimTimerMax != 0)
                        TimeMustProperty = AutoRecountSetTimer(progressInAb);

                    if (AimMesure != 0)
                        Mesure = AutoRecountSetCounter(progressInAb);

                }
            }
        }

        private int AutoRecountSetCounter(double progressInAb)
        {
            var mesureMax = AimMesure;
            int countedMesure;

            if (mesureMax > 0)
                countedMesure = roundToAutoVals(mesureMax * progressInAb);
            // Если отрицательное значение
            else
            {
                var absMax = Math.Abs(mesureMax);
                countedMesure =
                    roundToAutoVals(
                        absMax - (absMax * progressInAb));
            }

            return countedMesure;
        }

        private int AutoRecountSetTimer(double progressInAb)
        {
            var timerMax = AimTimerMax;
            int countedTime;

            if (timerMax > 0)
                countedTime = roundToAutoVals(timerMax * progressInAb);
            // Если отрицательное значение
            else
            {
                var absMax = Math.Abs(timerMax);
                countedTime =
                    roundToAutoVals(
                        absMax - (absMax * progressInAb));
            }

            return countedTime;
        }

        private void AutoRecountSetState(double curLvl)
        {
            double maxLvl = AbilitiModel.AbMaxLevel;
            curLvl = curLvl - 1; // Потому что для других расчетов +1
            if (curLvl > maxLvl)
                curLvl = maxLvl;
            double progressInAb = curLvl / maxLvl;

            double count = TaskStates.Count;
            int ind = (int)Math.Floor(count * progressInAb);

            if (ind > TaskStates.Count - 1)
                ind = TaskStates.Count - 1;

            State = TaskStates[ind].Tittle;
        }

        public void RefreshBackGround()
        {
            OnPropertyChanged(nameof(BackGroundBrush));
            OnPropertyChanged(nameof(ForeGroundBrush));
        }

        public void RefreshContext()
        {
            OnPropertyChanged(nameof(TaskContext));
        }

        /// <summary>
        /// Сообщить об обновлении задач- ссылок
        /// </summary>
        public void RefreshLinkedTasksNotify()
        {
            OnPropertyChanged(nameof(LinkedTasks));
        }

        public void RefreshRev()
        {
            OnPropertyChanged(nameof(UcElementRewardsViewModel));
        }

        /// <summary>
        /// Обновить скиллы квеста
        /// </summary>
        public void RefreshSkillQwests()
        {
            OnPropertyChanged(nameof(AimToSkill));
            OnPropertyChanged(nameof(IsQwestSkillsVisibility));
            OnPropertyChanged(nameof(AimToTask));
        }

        /// <summary>
        /// Обновляем подзадачи этой задачи
        /// </summary>
        public void RefreshSubtasks()
        {
            OnPropertyChanged(nameof(NotDoneSubtasks));
            OnPropertyChanged(nameof(NotDoneSubtasksFirstTasks));
            OnPropertyChanged(nameof(SubTasksString));
        }

        public void RefreshTaskEndDateForeground()
        {
            OnPropertyChanged(nameof(EndDateForeground));
        }

        public void refreshTimeVisible()
        {
            OnPropertyChanged(nameof(IsTimeVisible));
        }

        /// <summary>
        /// Обновить данные для главного окна
        /// </summary>
        public void RefreshValuesForMainWindow()
        {
            OnPropertyChanged(nameof(DescriptionProperty));
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="i">The i.</param>
        public void Remove(object i)
        {
        }

        /// <summary>
        /// Задать дату для сортировки
        /// </summary>
        /// <param name="dtWhenDone">Время кликания</param>
        /// <param name="isDone">Задача выполнена?</param>
        public void SetMilSecsForSort(DateTime dtWhenDone, bool isDone)
        {
            if (dtWhenDone < MainViewModel.selectedTime)
            {
                dtWhenDone = MainViewModel.selectedTime;
            }

            //TimeLastDone = new DateTime(dtWhenDone.Subtract(MainViewModel.selectedTime).Ticks);
            MiliSecsDoneForSort = dtWhenDone.Subtract(MainViewModel.selectedTime).TotalMilliseconds;

            //if (isDone)
            //{
            //    MiliSecsDoneForSort = dtWhenDone.Subtract(MainViewModel.selectedTime).TotalMilliseconds;
            //}
            //else
            //{
            //    MiliSecsDoneForSort = DateTime.MaxValue.TimeOfDay.TotalMilliseconds;
            //    // Двигаем в конец списка
            //    var persTasks = StaticMetods.PersProperty.Tasks;
            //    var count = persTasks.Count;
            //    if (count>1)
            //    {
            //        persTasks.Move(persTasks.IndexOf(this), count - 1);
            //    }
            //}
        }

        public void setTaskRangse()
        {
            var observableCollection = TaskRangs;
            if (observableCollection != null)
            {
                observableCollection.Clear();
            }
            else
            {
                TaskRangs = new ObservableCollection<Rangs>();
            }

            if (Recurrense.TypeInterval == TimeIntervals.Нет)
            {
                return;
            }

            foreach (var rangse in
                StaticMetods.PersProperty.PersSettings.AbilRangsForDefoultProperty.OrderBy(n => n.LevelRang))
            {
                TaskRangs.Add(
                    new Rangs
                    {
                        LevelRang = rangse.LevelRang,
                        NameOfRang = rangse.NameOfRang,
                        DeskriptionRangProperty = string.Empty
                    });
            }

            OnPropertyChanged(nameof(TaskRangs));
        }

        /// <summary>
        /// Навык который прокачивает задача.
        /// </summary>
        /// <returns></returns>
        public AbilitiModel TaskAbiliti()
        {
            return StaticMetods.PersProperty.Abilitis.FirstOrDefault(n => n.Skills.Any(q => q.NeedTask.TaskProperty == this));
        }

        /// <summary>
        /// Значение на которое прокачивает навык задача (без понижения со значением навыка).
        /// </summary>
        /// <returns></returns>
        public double TaskAbylityChangeVal(AbilitiModel abilitiModel)
        {
            if (abilitiModel == null)
                return 0;

            double changeExp = 10;

            var thisAbSkills = abilitiModel.Skills.Where(
                n =>
                    abilitiModel.CellValue >= n.NeedTask.LevelProperty &&
                    abilitiModel.CellValue <= n.NeedTask.ToLevelProperty).ToList();

            var k = KChangeValue(thisAbSkills.Count());

            double booster = abilitiModel.Booster;

            var chVal = changeExp * k * booster; //* AbIncreaseFormula(abilitiModel); // (abilitiModel.CellValue + 1);

            return chVal;
        }

        /// <summary>
        /// Задача влияет на квесты...
        /// </summary>
        public List<Aim> TaskInQwests()
        {
            var inQwests = new List<Aim>();
            foreach (var qwest in StaticMetods.PersProperty.Aims.Where(n => n.RelatedTasks().Any(q => q == this)))
            {
                inQwests.Add(qwest);
            }
            return inQwests.Distinct().ToList();
        }

        /// <summary>
        /// Пауза на таймере
        /// </summary>
        public void TimerPause()
        {
            timer?.Stop();

            TimerActiveProperty = Visibility.Collapsed;
        }

        /// <summary>
        /// Обнуляем все значения таймера и останавливаем таймер
        /// </summary>
        public void TimerRefresh()
        {
            TimerStop();
            TimerValueProperty = 0;
            PomodorroTimerStop();
            PomodorroNow = 0;
        }

        /// <summary>
        /// Запуск таймера
        /// </summary>
        public void TimerStart()
        {
            newTimer = new DispatcherTimer { Interval = StaticMetods.timeSpan };

            newTimer.Tick += (sender, e) =>
            {
                TimerValueProperty++;

                if (TimerValueProperty >= TimeMustProperty)
                {
                    MessageBox.Show(new Form { TopMost = true }, $"Таймер для задачи \"{NameOfProperty}\" завершен!!!");
                    TimerStop();
                }
            };

            newTimer.Start();
            IsTimerStartProperty = true;
        }

        /// <summary>
        /// Остановка таймера
        /// </summary>
        public void TimerStop()
        {
            newTimer?.Stop();
            IsTimerStartProperty = false;
            OnPropertyChanged(nameof(TimerVisibility));
        }

        public void ToEndOfList()
        {
            var taskA = this;
            StaticMetods.Locator.MainVM.Tasks.MoveCurrentToLast();
            var taskB = (Task)StaticMetods.Locator.MainVM.Tasks.CurrentItem;
            StaticMetods.Locator.MainVM.MoveTask(taskA, taskB, true);
        }

        /// <summary>
        /// Быстро сделать квест на основе этой задачи
        /// </summary>
        /// <param name="pers"></param>
        public void ToQwest(Pers pers)
        {
            var qw = new Aim(pers) { NameOfProperty = NameOfProperty };
            foreach (var inQwest in InQwests(pers))
            {
                inQwest.CompositeAims.Add(new CompositeAims { AimProperty = qw });
            }

            foreach (var inQwest in InQwests(pers))
            {
                inQwest.CountAutoProgress();
            }

            Delete(pers);

            StaticMetods.editAim(qw);
        }

        /// <summary>
        /// Обновить строковое представление даты
        /// </summary>
        public void UpdateRecString()
        {
            OnPropertyChanged(nameof(RecurrenceString));
        }

        protected override BitmapImage GetDefoultPic()
        {
            return DefoultPicsAndImages.DefoultTaskPic;
        }

        /// <summary>
        /// Повторение по дням
        /// </summary>
        /// <param name="recurrense">Интервал повторения</param>
        /// <param name="selectedTime">Текущее время</param>
        /// <returns></returns>
        private static DateTime addDays(TypeOfRecurrense recurrense, DateTime selectedTime)
        {
            return selectedTime.AddDays(recurrense.Interval);
        }

        /// <summary>
        /// Повторение по дням недели
        /// </summary>
        /// <param name="daysOfWeekRepeats">Дни недели по которым следует повторять задачу</param>
        /// <param name="selectedTime">Текущее время</param>
        /// <returns></returns>
        private static DateTime addDaysOfWeek(
            ObservableCollection<DaysOfWeekRepeat> daysOfWeekRepeats,
            DateTime selectedTime)
        {
            var dateOfBeginTask = selectedTime;

            if (daysOfWeekRepeats.All(n => n.CheckedProperty == false))
            {
                return dateOfBeginTask;
            }

            var contains = false;
            do
            {
                dateOfBeginTask = dateOfBeginTask.AddDays(1);
                var count = daysOfWeekRepeats.Count(
                    n => n.CheckedProperty && n.Day == dateOfBeginTask.DayOfWeek);

                if (count != 0)
                {
                    contains = true;
                }
            } while (contains == false);
            return dateOfBeginTask;
        }

        /// <summary>
        /// Добавить месяцы к дате начала
        /// </summary>
        /// <param name="recurrense">Интервал повторения задачи</param>
        /// <param name="selectedTime">Текущее время</param>
        /// <returns></returns>
        private static DateTime addMounth(TypeOfRecurrense recurrense, DateTime selectedTime)
        {
            return selectedTime.AddMonths(recurrense.Interval);
        }

        /// <summary>
        /// При необходимости добавляем случайную награду
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        /// <param name="editableTask">Задача от которой добавляется награда</param>
        private static void AddRandomeRewardFromTask(Pers _pers, Task editableTask)
        {
            //var persLevel = _pers.PersLevelProperty;
            //// Добавляем случайную награду если нужно
            //var ver = MainViewModel.rnd.NextDouble();
            //var rewards = GetEnabledRewardsWithoutGold(_pers, ver, persLevel).ToList();
            //if (!rewards.Any())
            //{
            //    return;
            //}
            //var maxRND = rewards.Count();
            //var index = MainViewModel.rnd.Next(maxRND);
            //var revard = rewards[index];
            //if (revard != null)
            //{
            //    _pers.InventoryItems.Add(revard);
            //    StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.НаградаПолучена, revard);
            //}

            double ver = _pers.RandomRevard.GetRanVal();
            var rews = _pers.ShopItems.Where(n => !n.IsArtefact).Where(n => n.VeroyatnostProperty >= ver).ToList();
            if (!rews.Any()) return;
            var rev = rews[MainViewModel.rnd.Next(0, rews.Count)];
            _pers.InventoryItems.Add(rev);
            StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.НаградаПолучена, rev);
        }

        /// <summary>
        /// Добавить недели к дате начала
        /// </summary>
        /// <param name="recurrense">Интервал повторения задачи</param>
        /// <param name="selectedTime">Выбранное время</param>
        /// <returns></returns>
        private static DateTime addWeeks(TypeOfRecurrense recurrense, DateTime selectedTime)
        {
            return selectedTime.AddDays(recurrense.Interval * 7);
        }

        private static bool CompareIsTaskTypesIs()
        {
            if (StaticMetods.PersProperty.TasksTypes.FirstOrDefault() == null)
            {
                MessageBox.Show("Сначала добавьте типы задач!");
                return true;
            }
            return false;
        }

        private static IEnumerable<Revard> GetEnabledRewardsWithoutGold(Pers _pers, double ver, int persLevel)
        {
            var req = new ObservableCollection<string>();
            var revsWithoutGold =
                _pers.ShopItems.Where(n => n.IsFromeTasksProperty)
                    .Where(n => StaticMetods.IsEnRev(n, false, _pers.GoldProperty, _pers.PersLevelProperty, out req));

            return revsWithoutGold.Where(
                delegate (Revard n)
                {
                    var verReward = Convert.ToDouble(n.VeroyatnostProperty) / 100.0;

                    if (ver > verReward)
                    {
                        return false;
                    }

                    return true;
                });
        }

        private static ObservableCollection<DaysOfWeekRepeat> GetHolyDays()
        {
            var holidays = MainViewModel.GetDaysOfWeekCollection();
            holidays[5].CheckedProperty = true;
            holidays[6].CheckedProperty = true;
            return holidays;
        }

        /// <summary>
        /// Расчитать модификатор изменения скиллов от задач
        /// </summary>
        /// <param name="plusOrMinus"></param>
        /// <param name="lev"></param>
        /// <returns></returns>
        private static double GetModDivider(string plusOrMinus, double lev)
        {
            //lev = (lev) + 1;
            var divider = lev;
            divider = divider >= 1 ? divider : 1;
            return divider;
        }

        private static ObservableCollection<DaysOfWeekRepeat> GetWorkDays()
        {
            var workdays = MainViewModel.GetDaysOfWeekCollection();
            workdays[0].CheckedProperty = true;
            workdays[1].CheckedProperty = true;
            workdays[2].CheckedProperty = true;
            workdays[3].CheckedProperty = true;
            workdays[4].CheckedProperty = true;
            return workdays;
        }

        /// <summary>
        /// Задача ПОЛНОСТЬЮ выполнена?
        /// </summary>
        /// <param name="isPlusClick"></param>
        /// <returns></returns>
        private static bool IsFullDo(bool isPlusClick)
        {
            return Keyboard.Modifiers != ModifierKeys.Control && Keyboard.Modifiers != ModifierKeys.Alt && isPlusClick;
        }

        private static bool IsNotFullDo(bool isPlusClick)
        {
            return isPlusClick && (Keyboard.Modifiers == ModifierKeys.Control || Keyboard.Modifiers == ModifierKeys.Alt);
        }

        /// <summary>
        /// Задать ссылки на задачу.
        /// </summary>
        /// <param name="tsk"></param>
        private static void SetLinkedTasks(Task tsk)
        {
            if (MainViewModel.TaskLinkDic.ContainsKey(tsk))
            {
                var tdi = MainViewModel.TaskLinkDic[tsk];

                // Навыки
                if (tdi.Ability != null)
                {
                    tsk.LinkedTasks = tdi.QwestTasks.ToList();
                }

                // Квесты
                else if (tdi.Qwest != null)
                {
                    tsk.LinkedTasks = tdi.Skills.ToList();
                }
            }
        }

        /// <summary>
        /// Штрафуем за простроченную задачу на размер урона
        /// </summary>
        /// <param name="isTaskDone">Задача выполнена?</param>
        /// <param name="isHpActiveteProperty">Хит пойнты активны?</param>
        /// <returns></returns>
        private int ChangeHp(bool isTaskDone, bool isHpActiveteProperty)
        {
            if (!isHpActiveteProperty) return 0;

            if (isTaskDone) return 0;

            return -Damage;
        }

        /// <summary>
        /// Меняем значения других элементов
        /// </summary>
        /// <param name="plusOrMinus"></param>
        private void ChangeRelated(string plusOrMinus)
        {
            switch (plusOrMinus)
            {
                case "minus":
                    Rage++;
                    break;

                case "plus":
                    StaticMetods.PersProperty.GoldProperty += PlusGold;
                    Rage = 0;
                    break;
            }

            if (Recurrense.TypeInterval != TimeIntervals.Нет)
            {
                var abilitiModel = TaskAbiliti();

                if (abilitiModel == null)
                    return;

                double chVal = TaskAbylityChangeVal(abilitiModel);

                double kk = 1.0;

                if (plusOrMinus == "minus")
                {
                    //kk = -kk; //0.5;
                    if (!StaticMetods.PersProperty.PersSettings.IsHP)
                    {
                        kk = -kk;
                    }
                    else
                    {
                        StaticMetods.PersProperty.HP = StaticMetods.PersProperty.HP - 1;
                        kk = 0;
                    }
                }

                if (abilitiModel.NforAllDone < 1)
                {
                    if (plusOrMinus == "plus")
                    {
                        chVal = chVal * BoosterOfDone;
                        abilitiModel.ValueIncrease(chVal, abilitiModel.ValueProperty);
                    }
                    else
                    {
                        chVal = chVal * BoosterOfFail;
                        abilitiModel.ValueDecrease(chVal, abilitiModel.ValueProperty);
                    }
                    //chVal = chVal * kk;
                    //abilitiModel.ValueProperty += chVal;
                }
                else
                {
                    chVal = kk * ((StaticMetods.PersProperty.PersSettings.MaxAbLev + 0.999) / abilitiModel.NforAllDone);
                    abilitiModel.ValueProperty += chVal;
                }
            }
        }

        /// <summary>
        /// Менять ХП
        /// </summary>
        /// <param name="_pers"></param>
        /// <param name="isTaskDone"></param>
        private void chengeHP(Pers _pers, bool isTaskDone)
        {
            if (_pers.PersSettings.IsHPActiveteProperty == false)
            {
                return;
            }

            _pers.HPProperty.CurrentHPProperty += isTaskDone
                ? ChangeHp(true, _pers.PersSettings.IsHPActiveteProperty)
                : ChangeHp(false, _pers.PersSettings.IsHPActiveteProperty);
        }

        /// <summary>
        /// Изменение значения задачи
        /// </summary>
        /// <param name="isTaskDone"></param>
        private void chengeTaskFromeAlterProgRelays(bool isTaskDone)
        {
            if (isTaskDone)
            {
                ValueOfTaskProperty++;
            }
            else
            {
                ValueOfTaskProperty = ValueOfTaskProperty - ChangeValueIfNotDoneProperty;
            }
        }

        private int compareByWave(Task other)
        {
            var v1 = Wave == 0 ? 999 : Wave;
            var v2 = other.Wave == 0 ? 999 : other.Wave;
            var byWave = v1.CompareTo(v2);
            return byWave;
        }

        /// <summary>
        /// Случайное подбадривание
        /// </summary>
        /// <returns></returns>
        private string GetCustFail()
        {
            var motivators = new List<string>
            {
                "Кошмар",
                "Неудача",
                "Блин",
                "Соберись",
                "Ну-ну",
                "Fail",
                "Чудовищно",
                "Попробуй еще раз",
                "Плохо",
                "Осечка",
                "Промах",
                "Каналья",
                "Карамба",
                "Тысяча чертей",
                "Дохлые каракатицы",
                "Вот кокос",
                "Гром и молния",
                "Лопни моя селезенка"
            };
            var custom = MainViewModel.rnd.Next(0, motivators.Count - 1);
            return motivators[custom] + ", " + StaticMetods.PersProperty.NameOfProperty + "!";
        }

        /// <summary>
        /// Случайное подбадривание
        /// </summary>
        /// <returns></returns>
        private string GetCustMotiv()
        {
            var motivators = new List<string>
            {
                "Молодец",
                "Так держать",
                "Супер",
                "Класс",
                "Круто",
                "Хорошая работа",
                "Гениально",
                "Фантастика",
                "Отлично",
                "Грандиозно",
                "Легендарно",
                "Чудесно",
                "Фантастика",
                "Потрясающе",
                "Умничка",
                "Великолепно"
            };
            var custom = MainViewModel.rnd.Next(0, motivators.Count - 1);
            return motivators[custom] + ", " + StaticMetods.PersProperty.NameOfProperty + "!";
        }

        private int getGoldChengesFromTask(int addGoldFromHardness)
        {
            return Convert.ToInt32(Convert.ToDouble(addGoldFromHardness));
        }

        /// <summary>
        /// Получаем показатель прогресса задачи для автозначений
        /// </summary>
        /// <returns></returns>
        private double GetProgressForAutoValues()
        {
            if (IsSkill == false)
            {
                return 1;
            }

            var lev =
                Convert.ToInt32(StaticMetods.PersProperty.Abilitis.Where(
                    n => n.Skills.Any(q => q.NeedTask.TaskProperty == this))
                    .Max(n => n.CellValue + 1.0));

            if (lev > 100)
            {
                lev = 1;
            }

            return lev / 100.0;
        }

        /// <summary>
        /// Дата соответствует повторениям?
        /// </summary>
        /// <param name="value">Дата</param>
        private void IsDateComplitesReccurence(ref DateTime value)
        {
            var typeOfRecurrense = Recurrense;
            if (typeOfRecurrense == null) return;

            var notOk = IsDateNotOk(value, typeOfRecurrense);

            if (notOk)
            {
                value = GetNextBegin(this);
            }
        }

        private bool IsMaxAb()
        {
            return TaskInAbilitis.All(n => Math.Abs(n.CellValue - 5) < 0.01);
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            RefreshSubtasks();
        }

        /// <summary>
        /// Обработчик для минуса в задаче
        /// </summary>
        /// <param name="_pers"></param>
        private void MinusTask(Pers _pers)
        {
            var selectedTime = MainViewModel.selectedTime;

            ChangeRelated("minus");

            BeginDateProperty = GetNextBegin(this);
            if (Recurrense.TypeInterval == TimeIntervals.Нет)
            {
                BeginDateProperty = selectedTime.AddDays(1);
            }
            EndDate = BeginDateProperty;
            CounterRefresh();
            TimerRefresh();
            SubTasksRefresh();
        }

        /// <summary>
        /// Переносим задачу на завтра без всяких штрафов
        /// </summary>
        private void MoveTaskToTommorow()
        {
            BeginDateProperty = GetNextBegin(this);
            if (Recurrense.TypeInterval == TimeIntervals.Нет)
            {
                BeginDateProperty = MainViewModel.selectedTime.AddDays(1);
                if (EndDate.Date <= MainViewModel.selectedTime.Date)
                {
                    EndDate = BeginDateProperty;
                }
            }
            else
            {
                EndDate = BeginDateProperty;
            }

            CounterRefresh();
            TimerRefresh();
            SubTasksRefresh();
        }

        /// <summary>
        /// Обработчик для плюса в задаче
        /// </summary>
        /// <param name="_pers"></param>
        private void PlusTask(Pers _pers)
        {
            switch (Recurrense.TypeInterval)
            {
                case TimeIntervals.Нет:
                    BeginDateProperty = GetNextBegin(this);
                    IsDelProperty = true;
                    break;

                default:
                    BeginDateProperty = GetNextBegin(this);
                    EndDate = BeginDateProperty;
                    CounterRefresh();
                    break;
            }

            SubTasksRefresh();
            IsLastClickNotDone = false;
            ChangeRelated("plus");
            CounterValuePlusProperty++;
            TimerRefresh();
        }

        private void RefreshTimeEnds()
        {
            _timeEnd = new ObservableCollection<TimeClass>();

            var allTimeEnds = StaticMetods.PersProperty.Tasks.SelectMany(n => n.TimeEnds).ToList();

            var maxInd = allTimeEnds.Any() ? allTimeEnds.Max(q => q.Hour) : 0;

            for (var i = 0; i < CounterMaxValueProperty; i++)
            {
                _timeEnd.Add(new TimeClass { Hour = maxInd + i, Minutes = 0 });
            }

            OnPropertyChanged(nameof(TimeEnds));
        }

        private void removeTaskFromAbilNeeds(Pers _pers)
        {
            foreach (var abilitiModel in _pers.Abilitis)
            {
                foreach (var needTasks in abilitiModel.NeedTasks.Where(n => n.TaskProperty == this).ToList())
                {
                    abilitiModel.NeedTasks.Remove(needTasks);
                }
            }
        }

        private void removeTaskFromAimNeeds(Pers _pers)
        {
            foreach (var aim in _pers.Aims)
            {
                foreach (var taskToDel in aim.NeedsTasks.Where(n => n.TaskProperty == this).ToList())
                {
                    aim.NeedsTasks.Remove(taskToDel);
                }

                // Удаляем скиллы
                foreach (var taskToDel in aim.Spells.Where(n => n == this).ToList())
                {
                    aim.Spells.Remove(taskToDel);
                }

                // Удаляем ссылки
                foreach (var taskToDel in aim.LinksOfTasks.Where(n => n == this).ToList())
                {
                    aim.LinksOfTasks.Remove(taskToDel);
                }
            }
        }

        private void removeTaskFromNextActions(Pers _pers)
        {
            foreach (var task in _pers.Tasks)
            {
                foreach (var source in task.NextActions.Where(n => n == this).ToList())
                {
                    task.NextActions.Remove(source);
                }
            }
        }

        private int roundToAutoVals(double d)
        {
            var c = Math.Floor(d);
            if (c < 1) c = 1;
            return Convert.ToInt32(c);
        }

        /// <summary>
        /// Показать мотиватор?
        /// </summary>
        /// <param name="isTaskDoneMotivatorShowProperty">Показывать мотиваторы</param>
        /// <param name="pathToMotivatorsProperty">Путь к мотиваторам</param>
        /// <param name="isPlusClick">Был нажат плюс?</param>
        private void showMotivator(
            bool isTaskDoneMotivatorShowProperty,
            string pathToMotivatorsProperty,
            bool isPlusClick)
        {
            if (isTaskDoneMotivatorShowProperty == false || isPlusClick == false)
            {
                return;
            }

            var im = StaticMetods.GetRandomImage(pathToMotivatorsProperty);

            var mw = new MotivatorsWindow { Image = { Source = im.Item1 } };
            mw.ShowDialog();
        }

        /// <summary>
        /// Обновляем подзадачи
        /// </summary>
        private void SubTasksRefresh()
        {
            if (Recurrense.TypeInterval != TimeIntervals.Нет)
            {
                switch (SubTasksRec)
                {
                    case typeOfSubTaskRecurrenses.послеПовтора:
                        foreach (var task in SubTasks.ToList())
                        {
                            task.isDone = false;
                        }
                        break;

                    case typeOfSubTaskRecurrenses.послеВыполненияПодзадач:
                        if (SubTasks.All(n => n.isDone)) goto case typeOfSubTaskRecurrenses.послеПовтора;
                        break;

                    case typeOfSubTaskRecurrenses.неПовторятьУдалять:
                        foreach (var source in SubTasks.Where(n => n.isDone).ToList())
                        {
                            SubTasks.Remove(source);
                        }
                        break;
                }
            }
        }
    }

    [Serializable]
    public class TimeClass : INotifyPropertyChanged, IComparable<TimeClass>
    {
        private int _hour;

        private int _minutes;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public int Hour
        {
            get
            {
                return _hour;
            }
            set
            {
                if (value == _hour || value < 0 || value > 24) return;
                _hour = value;
                OnPropertyChanged(nameof(Hour));
            }
        }

        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                if (value == _minutes || value < 0 || value > 59) return;

                _minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public int CompareTo(TimeClass other)
        {
            var compareHourse = Hour.CompareTo(other.Hour);
            if (compareHourse != 0)
            {
                return compareHourse;
            }

            return Minutes.CompareTo(other.Minutes);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
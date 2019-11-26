//#define IsAbFirstHard

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DotNetLead.DragDrop.UI.Behavior;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.View;
using Sample.ViewModel;

namespace Sample.Model
{
    /// <summary>
    /// Способности, скиллы персонажа
    /// </summary>
    [Serializable]
    public class AbilitiModel : LevelableRPGItem, IComparable<AbilitiModel>, IRangable, IExpable, IDragable, IDropable,
        IToolTipable, IHaveRevords
    {
        public static string HaveNotAbPointsText = $"Недостаточно кристаллов знаний; ";

        public static string HaveNotNextLevText = $"Следующий уровень персонажа; ";

        public static string UpAnotherCharactsText = $"Прокачать другие характеристики; ";

        /// <summary>
        /// Задача, которая связана с этим навыком.
        /// </summary>
        public Task NoAbsTask { get; set; }

        /// <summary>
        /// Defines the _experiance
        /// </summary>
        public double _experiance;

        /// <summary>
        /// Управляющая характеристика
        /// </summary>
        public Characteristic ruleCharacteristic;

        /// <summary>
        /// Defines the _abCost
        /// </summary>
        private int _abCost;

        /// <summary>
        /// Defines the _abilReqwirementses
        /// </summary>
        private ObservableCollection<AbilReqwirement> _abilReqwirementses = new ObservableCollection<AbilReqwirement>();

        private double _booster;

        /// <summary>
        /// Defines the _buyedInThisLevel
        /// </summary>
        private bool _buyedInThisLevel;

        private double _cellValue;

        /// <summary>
        /// Defines the _complecsNeeds
        /// </summary>
        private List<ComplecsNeed> _complecsNeeds;

        /// <summary>
        /// Defines the _defoultTaskType
        /// </summary>
        private TypeOfTask _defoultTaskType;

        /// <summary>
        /// Defines the _defSelH
        /// </summary>
        private string _defSelH;

        private double _firstVal;

        /// <summary>
        /// Defines the _groupedComplexNeeds
        /// </summary>
        private List<GroupedComlexNeed> _groupedComplexNeeds;

        /// <summary>
        /// Defines the _hardness
        /// </summary>
        private int _hardness = 3;

        /// <summary>
        /// Defines the _isAutoCountMinValues
        /// </summary>
        private bool _isAutoCountMinValues = true;

        /// <summary>
        /// Defines the _isChecked
        /// </summary>
        private bool _isChecked;

        private int _isHalyava;

        /// <summary>
        /// Defines the _isMayToEnabled
        /// </summary>
        private bool _isMayToEnabled;

        private bool _isMonstersall;

        /// <summary>
        /// Defines the _isPayedProperty
        /// </summary>
        private bool _isPayedProperty;

        /// <summary>
        /// Defines the _isPosled
        /// </summary>
        private bool _isPosled;

        /// <summary>
        /// Defines the _kExpRelay
        /// </summary>
        private double _kExpRelay;

        private double _lastValue;

        /// <summary>
        /// Defines the _levelsWhenUp
        /// </summary>
        private List<int> _levelsWhenUp;

        /// <summary>
        /// Defines the _maxValue
        /// </summary>
        private double _maxValue;

        /// <summary>
        /// Defines the _minLevel
        /// </summary>
        private int _minLevel;

        /// <summary>
        /// Defines the _modificatorLearn
        /// </summary>
        private double _modificatorLearn = 10;

        /// <summary>
        /// Defines the _modificatorOfForget
        /// </summary>
        private double _modificatorOfForget = 10;

        /// <summary>
        /// Defines the _needAbilities
        /// </summary>
        private ObservableCollection<NeedAbility> _needAbilities;

        /// <summary>
        /// Defines the _needCharacts
        /// </summary>
        private ObservableCollection<NeedCharact> _needCharacts;

        /// <summary>
        /// Уровень требований по умолчанию при добавлении
        /// </summary>
        private int _needLevelForDefoult;

        private List<NeedsForLevel> _needsForLevels;

        private double _nforAllDone;

        /// <summary>
        /// Значение, которое отображается в прогресс барах.
        /// </summary>
        public double ValueToProgress
        {
            get
            {
                if (StaticMetods.PersProperty.PersSettings.IsFUDGE)
                {
                    return CellValue;
                }

                return ValueProperty;
            }
        }


        /// <summary>
        /// Defines the _notAllowReqwirements
        /// </summary>
        private string _notAllowReqwirements;

        private double _ntoMax;

        private List<RangseNeeds> _rangseNeedses;

        /// <summary>
        /// Defines the _reqwireAims
        /// </summary>
        private ObservableCollection<Aim> _reqwireAims;

        /// <summary>
        /// Defines the _selH
        /// </summary>
        private string _selH;

        /// <summary>
        /// Defines the _tesPriority
        /// </summary>
        private double _tesPriority;

        /// <summary>
        /// Defines the _toChaRelaysProperty
        /// </summary>
        private double _toChaRelaysProperty;

        /// <summary>
        /// Defines the _valueMaxDouble
        /// </summary>
        private double _valueMaxDouble;

        /// <summary>
        /// Defines the _valueMinDouble
        /// </summary>
        private double _valueMinDouble;

        /// <summary>
        /// Комманда Добавить требование квеста.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<string> addNeedAimCommand;

        /// <summary>
        /// Комманда Добавить требование по выполненным задачам.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<string> addNeedTaskCommand;

        /// <summary>
        /// Комманда Купить очко скилла.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand buyAbPointCommand;

        /// <summary>
        /// Gets the Удалить требование квеста.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<CompositeAims> delAimNeedCommand;

        /// <summary>
        /// Комманда Удалить все задачи скилла.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand deleteAllSkillsCommand;

        /// <summary>
        /// Gets the Удалить требование (условие).
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<object> deleteComplecsNeedCommand;

        /// <summary>
        /// Gets the Удалить требование задачи.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<NeedTasks> deleteNeedTaskCommand;

        /// <summary>
        /// Gets the Понизить уровень комплексного требования.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<ComplecsNeed> downComplecsNeedLevelCommand;

        /// <summary>
        /// Сложность (влияет только на вероятности выпадения предетов).
        /// </summary>
        private int hardness;

        /// <summary>
        /// Скилл автоматически становится активным, если все требования выполненны.
        /// </summary>
        private bool isAutoStart;

        /// <summary>
        /// Активен ли скилл?.
        /// </summary>
        private bool isEnebled;

        /// <summary>
        /// Купленный уровень.
        /// </summary>
        private int payedLevel;

        /// <summary>
        /// Предыдущие/следующие скиллы.
        /// </summary>
        [field: NonSerialized]
        private List<AbilitiModel> prevNextAb;

        /// <summary>
        /// Gets the Показать комплексный элемент.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<object> showComplecsNeedItemCommand;

        /// <summary>
        /// Gets the Показать квест.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<Aim> showQwestCommand;

        /// <summary>
        /// Gets the Показать квест из требований.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<CompositeAims> showQwestFromNeedCommand;

        /// <summary>
        /// Gets the Показать задачу из требований.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<NeedTasks> showTaskFromNeedCommand;

        /// <summary>
        /// Сумма коэффициентов влияния на характеристики.
        /// </summary>
        private double toChaRelays;

        /// <summary>
        /// Gets the Повысить уровень комплексного требования.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<ComplecsNeed> upComplecsNeedLevelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilitiModel"/> class.
        /// </summary>
        /// <param name="_pers">The <see cref="Pers"/></param>
        public AbilitiModel(Pers _pers)
        {
            GUID = Guid.NewGuid().ToString();
            IsFocusedProperty = _pers.PersSettings.IsFocusForDefoultForNewAbilityProperty;
            IsAutoStartProperty = true;
            NameOfProperty = "Название навыка";
            HardnessProperty = -1;
            Rangs =
                MainViewModel.GetDefoultRangsForAbilitis(_pers.PersSettings.AbilRangsForDefoultProperty)();
            Cvet = Colors.Gold.ToString();

            NeedTasks = new ObservableCollection<NeedTasks>();
            NeedAims = new ObservableCollection<CompositeAims>();

            _pers.Abilitis.Add(this);

            // Добавляем во все характеристики
            foreach (var characteristic in _pers.Characteristics)
            {
                characteristic.NeedAbilitisProperty.Add(new NeedAbility { AbilProperty = this });
            }

            // Добавляем во все магазины и инвентари
            foreach (var inventoryItem in _pers.InventoryItems)
            {
                inventoryItem.ChangeAbilitis.Add(
                    new ChangeAbilityModele { AbilityProperty = this, ChangeAbilityProperty = 0 });
            }

            foreach (var shopItem in _pers.ShopItems)
            {
                shopItem.ChangeAbilitis.Add(
                    new ChangeAbilityModele { AbilityProperty = this, ChangeAbilityProperty = 0 });
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilitiModel"/> class.
        /// </summary>
        public AbilitiModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilitiModel"/> class.
        /// </summary>
        /// <param name="abiliti">The <see cref="AbilitiModel"/></param>
        public AbilitiModel(AbilitiModel abiliti)
        {
            SetAbFromAb(abiliti);
            NeedTasks = abiliti.NeedTasks;
        }

        /// <summary>
        /// Скилл на основе скилла и записать в персонажа
        /// </summary>
        /// <param name="abiliti"></param>
        /// <param name="prs"></param>
        public AbilitiModel(AbilitiModel abiliti, Pers prs) : this(prs)
        {
            SetAbFromAb(abiliti);
        }

        /// <summary>
        /// Defines the AbMaxLevel
        /// </summary>
        public static int AbMaxLevel => StaticMetods.PersProperty.PersSettings.MaxAbLev;

        public static double MaxValueOfAbility => StaticMetods.PersProperty.PersSettings.MaxAbLev + 0.001;

        /// <summary>
        /// Подсказка покупка скилла
        /// </summary>
        public string AbBuyToolTip
        {
            get { return BuyToolTip; }
        }

        /// <summary>
        /// Стоимость скилла
        /// </summary>
        public int AbCost
        {
            get
            {
                return _abCost;
            }
            set
            {
                if (value == _abCost) return;
                _abCost = value;
                OnPropertyChanged(nameof(AbCost));
                OnPropertyChanged(nameof(AbBuyToolTip));
            }
        }

        /// <summary>
        /// Gets the AbCounter
        /// </summary>
        public double AbCounter
        {
            get
            {
                var valProp = ValueProperty;
                return GetCountedValue(valProp);
            }
        }

        /// <summary>
        /// Начальный ранг характеристики
        /// </summary>
        public string AbFirstRang
        {
            get
            {
                var name = StaticMetods.PersProperty.PersSettings.AbRangs[Convert.ToInt32(Convert.ToInt32(FirstVal))].Name;
                name = $"{name}";
                return name;
            }
        }

        /// <summary>
        /// Gets the AbForeGround
        /// </summary>
        public Brush AbForeGround
        {
            get
            {
                return Brushes.DarkSlateGray;

                //if (StaticMetods.PersProperty.AbilitisPoints <= 0)
                //{
                //    return Brushes.DarkSlateGray;
                //}

                //SolidColorBrush brush = Brushes.SlateGray;
                //if (IsBuyVisibility == Visibility.Visible)
                //{
                //    if (ToChaRelaysProperty >= 3)
                //    {
                //        return Brushes.ForestGreen;
                //    }
                //    if (ToChaRelaysProperty <= 1)
                //    {
                //        return brush;
                //    }
                //    return Brushes.SlateBlue;
                //}
                //else
                //{
                //    return brush;
                //}
            }
        }

        /// <summary>
        /// Общий прогресс
        /// </summary>
        public double AbilityProgress
        {
            get
            {
                var progress = ValueProperty / MaxVal;
                return progress;
            }
        }

        /// <summary>
        /// Задания?
        /// </summary>
        public IEnumerable<ComplecsNeed> ActiveComplecsNeeds
        {
            get
            {
                if (ComplecsNeeds == null)
                {
                    return new List<ComplecsNeed>();
                }

                var complecsNeeds = ComplecsNeeds.ToList();

                return complecsNeeds;
            }
        }

        /// <summary>
        /// Активные квесты
        /// </summary>
        public List<ComplecsNeed> ActiveQwests
        {
            get { return ActiveComplecsNeeds.Where(n => n.NeedQwest != null).ToList(); }
        }

        /// <summary>
        /// Gets the комманда Добавить требование квеста.
        /// </summary>
        public RelayCommand<string> AddNeedAimCommand
        {
            get
            {
                return addNeedAimCommand
                       ?? (addNeedAimCommand = new RelayCommand<string>(
                           item =>
                           {
                               var levelToNeed = NeedLevelForDefoult;
                               var qwestLevel = StaticMetods.PersProperty.PersLevelProperty;

                               var changeAbForDefoult = 15;
                               CompositeAims need = null;
                               if (item == "+")
                               {
                                   var add = StaticMetods.AddNewAim(
                                       StaticMetods.PersProperty,
                                       qwestLevel,
                                       null,
                                       this, levelToNeed);

                                   if (add != null)
                                   {
                                       var ca = new CompositeAims
                                       {
                                           AimProperty = add,
                                           KoeficientProperty = changeAbForDefoult,
                                           KRel = changeAbForDefoult,
                                           LevelProperty = levelToNeed
                                       };

                                       NeedAims.Add(ca);
                                       need = ca;
                                       OnPropertyChanged(nameof(ValueProperty));
                                   }
                               }
                               else
                               {
                                   // Выбор из списка
                                   var av = new AddOrEditNeedAimView();
                                   var context = StaticMetods.Locator.AddOrEditAimNeedVM;
                                   context.ImageProperty = ImageProperty;
                                   context.MinLevelForDefoultProperty = qwestLevel;
                                   context.SellectedNeedPropertyProperty = new CompositeAims
                                   {
                                       AimProperty = null,
                                       KoeficientProperty = changeAbForDefoult,
                                       KRel = changeAbForDefoult,
                                       LevelProperty = levelToNeed
                                   };

                                   av.btnOk.Click += (sender, args) =>
                                   {
                                       if (context.SellectedNeedPropertyProperty.AimProperty != null)
                                       {
                                           NeedAims.Add(context.SellectedNeedPropertyProperty);
                                           need = context.SellectedNeedPropertyProperty;
                                           OnPropertyChanged(nameof(ValueProperty));
                                       }
                                       av.Close();
                                       OnPropertyChanged(nameof(ValueProperty));
                                   };
                                   av.btnCansel.Click += (sender, args) => { av.Close(); };
                                   av.ShowDialog();
                               }

                               if (need != null)
                               {
                                   need.LevelProperty = GetNeedLev();
                                   need.ToLevelProperty = 4;
                               }

                               RefreshComplecsNeeds();
                               StaticMetods.Locator.AddOrEditAbilityVM.RefreshInfoCommand.Execute(null);
                           },
                           item => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Добавить требование по выполненным задачам.
        /// </summary>
        public RelayCommand<string> AddNeedTaskCommand
        {
            get
            {
                return addNeedTaskCommand
                       ?? (addNeedTaskCommand = new RelayCommand<string>(
                           item =>
                           {
                               var levelToNeed = GetNeedLev();

                               var defType = DefoultTaskType ??
                                             StaticMetods.PersProperty.TasksTypes.FirstOrDefault(
                                                 n => n.IntervalForDefoult == TimeIntervals.День);

                               NeedTasks need = null;
                               if (item == "+")
                               {
                                   // Новая задача
                                   var tsk = Task.AddTask(defType, this, null, levelToNeed);
                                   need = NeedTasks.LastOrDefault(n => n.TaskProperty == tsk.Item2);
                               }
                               else
                               {
                                   // Выбор из списка
                                   var av = new AddOrEditTaskNeedView();
                                   var context = StaticMetods.Locator.AddOrEditTaskNeedVM;
                                   context.IsOnlySkiils = true;
                                   context.ImageProperty = ImageProperty;
                                   context.SellectedNeedProperty = new NeedTasks
                                   {
                                       TaskProperty = null,
                                       KoeficientProperty =
                                           StaticMetods.DefoultKForTaskNeed,
                                       KRel = 6,
                                       LevelProperty = levelToNeed
                                   };

                                   av.btnOk.Click += (sender, args) =>
                                   {
                                       var taskProperty = context.SellectedNeedProperty.TaskProperty;

                                       if (taskProperty != null)
                                       {
                                           NeedTasks.Add(context.SellectedNeedProperty);
                                           need = context.SellectedNeedProperty;
                                           Task.RecountTaskLevel(taskProperty);
                                       }

                                       StaticMetods.RecountTaskLevels();
                                       av.Close();
                                       OnPropertyChanged(nameof(ValueProperty));
                                   };

                                   av.btnCansel.Click += (sender, args) => { av.Close(); };
                                   av.ShowDialog();
                                   context.IsOnlySkiils = false;
                               }

                               if (need != null)
                               {
                                   need.LevelProperty = GetNeedLev();
                                   need.ToLevelProperty = 10; //GetNeedLev(); //GetNeedLev();
                               }

                               RefreshComplecsNeeds();
                               StaticMetods.RecountTaskLevels();
                               OnPropertyChanged(nameof(Opacity));
                           },
                           item => { return true; }));
            }
        }

        /// <summary>
        /// Процент от квестов (прокачка)
        /// </summary>
        public int AllPercQwests
        {
            get { return Convert.ToInt32(NeedAims?.Sum(n => n.KRel)); }
        }

        /// <summary>
        /// Бустер прокачки. Чем больше, тем быстрее качается.
        /// </summary>
        public double Booster
        {
            get
            {
                if (_booster <= 0)
                {
                    _booster = StaticMetods.PersProperty.PersSettings.GlobalBooster;
                }
                return _booster;
            }
            set
            {
                if (value.Equals(_booster) || value <= 0) return;
                _booster = value;
                OnPropertyChanged(nameof(Booster));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether BuyedInThisLevel
        /// </summary>
        public bool BuyedInThisLevel
        {
            get
            {
                return _buyedInThisLevel;
            }
            set
            {
                if (value == _buyedInThisLevel) return;
                _buyedInThisLevel = value;
                OnPropertyChanged(nameof(BuyedInThisLevel));
                OnPropertyChanged(nameof(IsBuyVisibility));
            }
        }

        /// <summary>
        /// Gets the BuyToolTip
        /// </summary>
        public string BuyToolTip
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Минимальное целое значение прогресса скилла
        /// </summary>
        public double CellValue
        {
            get
            {
                return _cellValue;
            }
            set
            {
                if (value == _cellValue) return;

                if (value > StaticMetods.PersProperty.PersSettings.MaxAbLev)
                {
                    value = StaticMetods.PersProperty.PersSettings.MaxAbLev;
                }

                _cellValue = value;
                OnPropertyChanged(nameof(CellValue));
                OnPropertyChanged(nameof(ComplecsComplecsNeed));
                OnPropertyChanged(nameof(MinLevelProperty));
                OnPropertyChanged(nameof(NeedAbilities));
                OnPropertyChanged(nameof(NeedCharacts));
                OnPropertyChanged(nameof(RangName));
                OnPropertyChanged(nameof(ReqwireAims));
                OnPropertyChanged(nameof(ValueMaxDouble));
                OnPropertyChanged(nameof(ValueMinDouble));
                OnPropertyChanged(nameof(Percent));
                StaticMetods.PersProperty.RecountChaValues();
                StaticMetods.PersProperty.NewRecountExp();
                NeedTasks?.Select(n => n.TaskProperty).ToList().ForEach(n => n.RecountAutoValues());
            }
        }

        /// <summary>
        /// Gets the ChaPriority
        /// </summary>
        public double ChaPriority
        {
            get
            {
                var ch = StaticMetods.PersProperty.Characteristics.Sum(n => n.GetPsevdoRelay(this));
                return ch;
            }
        }

        /// <summary>
        /// Gets the ChBackground
        /// </summary>
        public Brush ChBackground
        {
            get { return IsChecked ? Brushes.Yellow : Brushes.White; }
        }

        public string ClearedNotAllowReqwirements
        {
            get
            {
                var replacements = new[]
                {
                    new {Find = AbilitiModel.HaveNotAbPointsText, Replace = ""},
                    new {Find = AbilitiModel.HaveNotNextLevText, Replace = ""},
                    new {Find = AbilitiModel.UpAnotherCharactsText, Replace = ""},
                };

                var text = NotAllowReqwirements;
                var aggregate = replacements.Aggregate(text,
                    (current, set) => current.Replace(set.Find, set.Replace));
                return
                    aggregate;
            }
        }

        /// <summary>
        /// Сгруппированные новые требования (новые)
        /// </summary>
        public Dictionary<CCKey, List<ComplecsNeed>> ComplecsComplecsNeed
        {
            get
            {
                var needs = ComplecsNeeds;
                var cn = GetNewCCNeeds(needs);

                return cn;

                //    //for (var index = 1; index <= 5; index++)
                //    //{
                //    //    var hl = HList[index];
                //    //    var index1 = index;
                //    //    var complecsNeeds =
                //    //        needs.Where(n => !n.IsQwest && index1 >= n.LevelProperty && index1 <= n.ToLevelProperty);
                //    //    var levDescr = StaticMetods.PersProperty.PersSettings.AbRangs[Convert.ToInt32(index)].Name + $"\n~ {GetLevelHardness(index)}%";
                //    //    var curLevDescr = StaticMetods.PersProperty.PersSettings.AbRangs[Convert.ToInt32(CellValue)].Name;
                //    //    cn.Add(new CCKey(hl, index, curLevDescr, levDescr), complecsNeeds.ToList());
                //    //}

                //    for (var index = StaticMetods.MaxAbLevel; index >= 0; index--)
                //    {
                //        //if (StaticMetods.PersProperty.IsSettingsVisibillity == Visibility.Collapsed)
                //        //{
                //        //    if (index != Convert.ToInt32(CellValue))
                //        //    {
                //        //        continue;
                //        //    }
                //        //}

                //        var abLevel = Convert.ToInt32(CellValue);
                //        var isSetViz0 = StaticMetods.Locator?.AddOrEditAbilityVM?.IsSetViz;//StaticMetods.PersProperty.IsSettingsVisibillity == Visibility.Visible;
                //        if (isSetViz0 == null) continue;
                //        var isSetViz = isSetViz0.Value;
                //        if (isSetViz)
                //        {
                //            var ind = index;
                //            var needsOfCorrectThisLevel =
                //                needs.Where(n => n.LevelProperty == ind);

                //            if (index != abLevel
                //                && index != StaticMetods.PersProperty.PersSettings.MaxAbLev
                //                &&
                //                !needsOfCorrectThisLevel.Any())
                //            {
                //                continue;
                //            }
                //        }
                //        else
                //        {
                //            if (index != abLevel)
                //            {
                //                continue;
                //            }
                //        }

                //        var hl = HList[index];
                //        var index1 = index;

                //        List<ComplecsNeed> complecsNeeds;

                //        bool isSettingsVisible = isSetViz;
                //        bool isThisLevel = index == abLevel;
                //        var maxAbLev = StaticMetods.PersProperty.PersSettings.MaxAbLev;
                //        bool isMaxLevel = index == maxAbLev;

                //        if (isSettingsVisible == false || isThisLevel || isMaxLevel)
                //        {
                //            complecsNeeds = needs.Where(n => index1 >= n.LevelProperty && index1 <= n.ToLevelProperty).ToList();
                //        }
                //        else
                //        {
                //            // Не на максе...
                //            var inThisLev = needs.Where(n => abLevel >= n.LevelProperty && abLevel <= n.ToLevelProperty);
                //            var inMaxLevel = needs.Where(n => maxAbLev >= n.LevelProperty && maxAbLev <= n.ToLevelProperty);
                //            complecsNeeds = needs.Where(n => index1 == n.LevelProperty).Except(inThisLev).Except(inMaxLevel).ToList();
                //            if (!complecsNeeds.Any())
                //            {
                //                continue;
                //            }
                //        }

                //        string levDescr;
                //        if (isSetViz &&
                //            index == StaticMetods.PersProperty.PersSettings.MaxAbLev
                //            && abLevel != StaticMetods.PersProperty.PersSettings.MaxAbLev)
                //        {
                //            levDescr = StaticMetods.PersProperty.PersSettings.AbRangs[Convert.ToInt32(index)].Name + "\n↑↑↑↑↑";
                //        }
                //        else
                //        {
                //            string s = string.Empty;
                //            if (isSetViz && index == abLevel)
                //            {
                //                s = "↓↓↓↓↓\n";
                //            }
                //            levDescr = s + StaticMetods.PersProperty.PersSettings.AbRangs[Convert.ToInt32(index)].Name;
                //        }

                //        // + $"\n~ {GetLevelHardness(index)}%";
                //        var curLevDescr = StaticMetods.PersProperty.PersSettings.AbRangs[abLevel].Name;
                //        var value = complecsNeeds.OrderBy(n => n.NeedTask).ToList();
                //        cn.Add(new CCKey(hl, index, curLevDescr, levDescr), value);
                //    }
                //    return cn;
                //}
            }
        }

        /// <summary>
        /// Комплексные требования (объединенные квестов и задач)
        /// </summary>
        public List<ComplecsNeed> ComplecsNeeds => GetComplexNeeds();

        /// <summary>
        /// Стоимость 1 уровня скилла
        /// </summary>
        public int CostProperty => 0;

        /// <summary>
        /// Тип для задач по умолчанию
        /// </summary>
        public TypeOfTask DefoultTaskType
        {
            get
            {
                if (_defoultTaskType == null)
                {
                    _defoultTaskType = StaticMetods.PersProperty.PersSettings.DefoultTaskTypeForAbills;
                }

                if (StaticMetods.PersProperty.TasksTypes.All(n => n != _defoultTaskType))
                {
                    _defoultTaskType = StaticMetods.PersProperty.TasksTypes.FirstOrDefault();
                }

                return _defoultTaskType;
            }
            set
            {
                if (Equals(value, _defoultTaskType)) return;
                _defoultTaskType = value;
                OnPropertyChanged(nameof(DefoultTaskType));
            }
        }

        ///// <summary>
        ///// Gets the CurLevDescr
        ///// </summary>
        //public string CurLevDescr
        //{
        //    get
        //    {
        //        string name =
        //            $" ({LevelProperty}/{MaxLevelProperty}) \"{CurRang.NameOfRang}\": {HardnessPercentage(CellValue - 1)}";
        //        return name;
        //    }
        //}
        /// <summary>
        /// По умолчанию выбранная сложность
        /// </summary>
        public string DefSelH
        {
            get
            {
                if (_defSelH == null)
                {
                    return HList.First();
                }
                return _defSelH;
            }
            set
            {
                if (value == _defSelH) return;
                _defSelH = value;
                OnPropertyChanged(nameof(DefSelH));
            }
        }

        //Pers.GetLevelCost((int)CellValue + 1);
        /// <summary>
        /// Gets the Удалить условие квеста.
        /// </summary>
        public RelayCommand<CompositeAims> DelAimNeedCommand
        {
            get
            {
                return delAimNeedCommand
                       ?? (delAimNeedCommand = new RelayCommand<CompositeAims>(
                           item =>
                           {
                               var messageBoxResult =
                                   MessageBox.Show(
                                       "Удалить также сам квест?",
                                       "Внимание!",
                                       MessageBoxButton.OKCancel);

                               if (messageBoxResult == MessageBoxResult.OK)
                               {
                                   StaticMetods.RemoveQwest(StaticMetods.PersProperty, item.AimProperty);
                               }
                               else
                               {
                                   NeedAims.Remove(item);
                               }

                               StaticMetods.RecauntAllValues();
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
        /// Gets the Удалить все задачи скилла.
        /// </summary>
        public RelayCommand DeleteAllSkillsCommand
        {
            get
            {
                return deleteAllSkillsCommand
                       ?? (deleteAllSkillsCommand =
                           new RelayCommand(
                               () =>
                               {
                                   foreach (var needTaskse in NeedTasks.ToList())
                                   {
                                       DeleteNeedTaskCommand.Execute(needTaskse);
                                   }

                                   RefreshComplecsNeeds();
                                   OnPropertyChanged(nameof(Opacity));
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Удалить требование (условие).
        /// </summary>
        public RelayCommand<object> DeleteComplecsNeedAimCommand
        {
            get
            {
                return deleteComplecsNeedCommand
                       ?? (deleteComplecsNeedCommand =
                           new RelayCommand<object>(
                               item =>
                               {
                                   var it = item as ComplecsNeed;
                                   if (it?.NeedTask != null)
                                   {
                                       DeleteNeedTaskCommand.Execute(it.NeedTask);
                                   }

                                   if (it?.NeedQwest != null)
                                   {
                                       DelAimNeedCommand.Execute(it.NeedQwest);
                                   }

                                   StaticMetods.RecauntAllValues();

                                   RefreshComplecsNeeds();
                               },
                               item =>
                               {
                                   var it = item as ComplecsNeed;
                                   if (it == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the Удалить условие задачи.
        /// </summary>
        public RelayCommand<NeedTasks> DeleteNeedTaskCommand
        {
            get
            {
                return deleteNeedTaskCommand
                       ?? (deleteNeedTaskCommand = new RelayCommand<NeedTasks>(
                           item =>
                           {
                               var inAbs = from abilitiModel in StaticMetods.PersProperty.Abilitis
                                           where abilitiModel != this
                                           from needTaskse in abilitiModel.NeedTasks
                                           where needTaskse.TaskProperty == item.TaskProperty
                                           select needTaskse;

                               var inQwests = from aim in StaticMetods.PersProperty.Aims
                                              from needsTask in aim.NeedsTasks
                                              where needsTask.TaskProperty == item.TaskProperty
                                              select needsTask;

                               var inAll = inAbs.Concat(inQwests).ToList();

                               if (inAll.Any())
                               {
                                   NeedTasks.Remove(item);
                               }
                               else
                               {
                                   // Удаляем задачу (из требований удаляется автоматом)
                                   item.TaskProperty.Delete(StaticMetods.PersProperty);
                               }

                               StaticMetods.RecauntAllValues();

                               StaticMetods.RecountTaskLevels();
                               RefreshComplecsNeeds();
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
        /// Для импорта описание
        /// </summary>
        public string Description2
        {
            get
            {
                var desc = "";
                if (NeedTasks != null && NeedTasks.Any())
                {
                    for (var i = 0; i < 4; i++)
                    {
                        desc += "\n";
                        desc += $"# {StaticMetods.PersProperty.PersSettings.AbRangs[i].Name}";
                        desc += "\n";
                        foreach (var nt in NeedTasks.Where(n => n.LevelProperty <= i && n.ToLevelProperty >= i))
                        {
                            desc += $"* {nt.TaskProperty.NameOfProperty} {nt.TaskProperty.PlusNameOf2}";
                            desc += "\n";
                        }
                    }
                }
                return desc;
            }
        }

        /// <summary>
        /// Опыт за этот конретный скилл
        /// </summary>
        public double Experiance
        {
            get
            {
                return _experiance;
            }
            set
            {
                if (value.Equals(_experiance)) return;
                //double k = TESPriority > 1 ? 1 : 0.5;
                //bool isExpLessZero = value <= _experiance && (_experiance - value) * 50 * k >= StaticMetods.PersProperty.PersExpProperty;
                //if (isExpLessZero)
                //{
                //    value = StaticMetods.PersProperty.PersExpProperty / (50.0 * k);
                //}
                _experiance = value;
                StaticMetods.RecountPersExp();
                OnPropertyChanged(nameof(Experiance));
            }
        }

        public double FakeForSort
        {
            get
            {
                //double ff = NeedTasks.Count(n => CellValue >= n.LevelProperty &&
                //        CellValue <= n.ToLevelProperty);
                //if (ff == 0) ff = 1;
                double boost = Booster;

                double nMax = NforAllDone;
                if (!(nMax >= 1))
                    return ValueProperty + boost * Task.AbIncreaseFormula(this) * 10.0; //(1 / (CellValue + 1));
                else
                    return ValueProperty + ((MaxValueOfAbility) / nMax);
            }
        }

        public double FirstVal
        {
            get
            {
                return _firstVal;
            }
            set
            {
                if (value.Equals(_firstVal)) return;
                if (value < 0) value = 0;
                if (value > StaticMetods.PersProperty.PersSettings.MaxAbLev)
                    value = StaticMetods.PersProperty.PersSettings.MaxAbLev;
                val = 0;
                _firstVal = value;
                OnPropertyChanged(nameof(ValueProperty));
                CellValue = Math.Floor(ValueProperty);
                OnPropertyChanged(nameof(FirstVal));
                OnPropertyChanged(nameof(AbFirstRang));
                StaticMetods.PersProperty.RecountChaValues();
                OnPropertyChanged(nameof(ComplecsComplecsNeed));
            }
        }

        /// <summary>
        /// Цвет надписи
        /// </summary>
        public Brush ForeGroundd
        {
            get
            {
                if (!IsEnebledProperty)
                {
                    return Brushes.Red;
                }
                //if (!NeedTasks.Any() && !NeedAims.Any())
                //{
                //    return Brushes.Red;
                //}
                return Brushes.DarkSlateGray;
            }
        }

        /// <summary>
        /// Gets or sets the GroupedComplexNeeds
        /// </summary>
        public List<GroupedComlexNeed> GroupedComplexNeeds
        {
            get
            {
                return _groupedComplexNeeds;
            }
            set
            {
                if (Equals(value, _groupedComplexNeeds)) return;
                _groupedComplexNeeds = value;
                OnPropertyChanged(nameof(GroupedComplexNeeds));
            }
        }

        /// <summary>
        /// Сложность скилла. Влияет на опыт и стоимость.
        /// </summary>
        public int Hardness
        {
            get
            {
                return _hardness;
            }
            set
            {
                if (value == _hardness) return;
                _hardness = value;
                OnPropertyChanged(nameof(Hardness));
                StaticMetods.PersProperty.UpdateAbilityPoints();
                StaticMetods.RecountPersExp();
                StaticMetods.RecountTaskLevels();
                GetReqwirements();
            }
        }

        /// <summary>
        /// Название сложности
        /// </summary>
        public string HardnessName
        {
            get
            {
                switch (HardnessProperty)
                {
                    case -1:
                        return "Норм";

                    case 0:
                        return "Сложно";

                    case 1:
                        return "Оч. сложно";

                    default:
                        return "Норм";
                }
            }
        }

        /// <summary>
        /// Sets and gets Сложность скилла (влияет стоимость его активации). Changes to that
        /// property's value raise the PropertyChanged event.
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

                OnPropertyChanged(nameof(HardnessProperty));
                OnPropertyChanged(nameof(CostProperty));

                var enumerable =
                    NeedTasks?.Where(n => n.LevelProperty >= CellValue - 1).Select(n => n.TaskProperty);

                if (enumerable != null)
                    foreach (
                        var tsk in
                            enumerable)
                    {
                    }

                ChangeValuesOfRelaytedItems();

                RefreshComplecsNeeds();
            }
        }

        /// <summary>
        /// Sets and gets Сложность скилла название. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string HardnessStringProperty
        {
            get
            {
                switch (HardnessProperty)
                {
                    case -1:
                        return "(" + "нет" + ")";

                    case 0:
                        return "(" + "нормальный" + ")";

                    case 1:
                        return "(" + "сложный" + ")";

                    default:
                        return "(" + "оч. сложный" + ")";
                }
            }
        }

        /// <summary>
        /// Gets the HList
        /// </summary>
        public List<string> HList
        {
            get { return StaticMetods.PersProperty.PersSettings.AbRangs.Select(n => n.Name).ToList(); }
        }

        /// <summary>
        /// Gets the IsAbsQwestsVisible
        /// </summary>
        public Visibility IsAbsQwestsVisible
        {
            get { return CellValue > 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Все требования для перехода на новый уровень выполнены?
        /// </summary>
        public bool IsAllNeedsComplete
        {
            get
            {
                if (LevelProperty == MaxLevelProperty)
                {
                    return false;
                }

                if (CellValue != 0)
                {
                    // Если требований вообще нет

                    if (
                        ComplecsNeeds.Where(n => n.AsLink == false && Math.Abs(n.KRelay) > 0.01)
                            .Any(n => n.LevelProperty == CellValue - 1) ==
                        false)
                    {
                        return false;
                    }
                }

                // Если требования не выполнены или выполнены

                return ComplecsNeeds.Where(n => n.AsLink == false && Math.Abs(n.KRelay) > 0.01)
                    .Where(n => n.LevelProperty <= CellValue - 1)
                    .All(n => n.ProgressProperty >= 99);
            }
        }

        /// <summary>
        /// Авто расчет минимальных значений
        /// </summary>
        public bool IsAutoCountMinValues
        {
            get
            {
                return _isAutoCountMinValues;
            }
            set
            {
                if (value == _isAutoCountMinValues) return;
                _isAutoCountMinValues = value;
                OnPropertyChanged(nameof(IsAutoCountMinValues));
                RefreshComplecsNeeds();
            }
        }

        /// <summary>
        /// Sets and gets скилл автоматически становится активным, если все требования выполненны.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsAutoStartProperty
        {
            get
            {
                return isAutoStart;
            }

            set
            {
                if (isAutoStart == value)
                {
                    return;
                }

                isAutoStart = value;
                OnPropertyChanged(nameof(IsAutoStartProperty));
            }
        }

        /// <summary>
        /// Видимость покупки скилла
        /// </summary>
        public Visibility IsBuyVisibility
        {
            get
            {
                ////if (!IsMayToEnabled) return Visibility.Collapsed;
                ////if (BuyedInThisLevel) return Visibility.Collapsed;
                ////if (StaticMetods.PersProperty.AbilitisPoints < CostProperty)
                ////{
                ////    return Visibility.Collapsed;
                ////}

                //if (CellValue >= StaticMetods.MaxAbLevel)
                //{
                //    return Visibility.Collapsed;
                //}

                //// Проверка по требованиям (будет для каждого уровня)
                //if (!string.IsNullOrWhiteSpace(NotAllowReqwirements))
                //{
                //    return Visibility.Collapsed;
                //}

                //// Проверка по халявным
                ////bool isHalyava = StaticMetods.PersProperty.Abilitis.Any(n => n.IsHalyava > 0 && n != this);
                ////if (isHalyava && IsHalyava == 0)
                ////{
                ////    return Visibility.Collapsed;
                ////}

                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Для импорта
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if (value == _isChecked) return;
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
                OnPropertyChanged(nameof(ChBackground));
            }
        }

        /// <summary>
        /// Sets and gets Активен ли скилл?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsEnebledProperty
        {
            get
            {
                GetReqwirements();

                //if (IsPayedProperty == false)
                //{
                //    return false;
                //}
                if (!string.IsNullOrEmpty(NotAllowReqwirements))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Прокачка бедет халявной?
        /// </summary>
        public int IsHalyava
        {
            get
            {
                return _isHalyava;
            }
            set
            {
                _isHalyava = value;
                OnPropertyChanged(nameof(IsHalyava));
            }
        }

        /// <summary>
        /// Видно ли настройка сложности скилла?
        /// </summary>
        public Visibility IsHardnessSettingsVisibillity
        {
            get { return CellValue > 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Может быть доступен
        /// </summary>
        public bool IsMayToEnabled
        {
            get
            {
                return _isMayToEnabled;
            }
            set
            {
                if (value == _isMayToEnabled) return;
                _isMayToEnabled = value;
                OnPropertyChanged(nameof(IsMayToEnabled));
                OnPropertyChanged(nameof(IsBuyVisibility));
                if (value == false && IsPayedProperty)
                {
                    SaleAb(this);
                }
            }
        }

        public bool IsMonstersall
        {
            get
            {
                return _isMonstersall;
            }
            set
            {
                if (value == _isMonstersall) return;
                _isMonstersall = value;
                OnPropertyChanged(nameof(IsMonstersall));
            }
        }

        /// <summary>
        /// Скилл куплен за очки скиллов
        /// </summary>
        public bool IsPayedProperty
        {
            get
            {
                return _isPayedProperty;
            }
            set
            {
                if (value == _isPayedProperty) return;
                _isPayedProperty = value;

                //MinLevelProperty = IsPayedProperty == false
                //    ? StaticMetods.PersProperty.MaxLevelProperty
                //    : StaticMetods.PersProperty.PersLevelProperty;

                OnPropertyChanged(nameof(IsPayedProperty));
                RefreshAbBuySale();
                OnPropertyChanged(nameof(RangName));
                OnPropertyChanged(nameof(MinLevelProperty));
                OnPropertyChanged(nameof(SelH));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsPosled
        /// </summary>
        public bool IsPosled
        {
            get
            {
                return _isPosled;
            }
            set
            {
                if (value == _isPosled) return;
                _isPosled = value;
                OnPropertyChanged(nameof(IsPosled));
                RefreshComplecsNeeds();
            }
        }

        /// <summary>
        /// Видна кнопка "сбросить скилл?"
        /// </summary>
        public Visibility IsResetVisible
        {
            get
            {
                return Visibility.Collapsed;

                //return PayedLevelProperty < 1 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// Видимость продажи скилла
        /// </summary>
        public Visibility IsSaleVisibility
        {
            get { return IsPayedProperty ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Коэффициент влияния на опыт
        /// </summary>
        public double KExpRelay
        {
            get
            {
                return _kExpRelay;
            }
            set
            {
                if (value.Equals(_kExpRelay)) return;
                _kExpRelay = value;
                OnPropertyChanged(nameof(KExpRelay));
            }
        }

        /// <summary>
        /// Достигнутое при последнем получении уровня значение.
        /// </summary>
        public double LastValue
        {
            get => _lastValue;
            set
            {
                if (_lastValue == value)
                {
                    return;
                }

                int beforeLevel = (int)CellValue;
                _lastValue = value;
                CellValue = Math.Floor(_lastValue);
                int afterLevel = (int)CellValue;

                OnPropertyChanged(nameof(ValueProperty));
                OnPropertyChanged(nameof(RoundValue));
                OnPropertyChanged(nameof(Percentage));
                OnPropertyChanged(nameof(RangName));

                StaticMetods.PersProperty.RecountChaValues();

                foreach (var needTaskse in NeedTasks)
                {
                    needTaskse.TaskProperty.RefreshBackGround();
                }
                if (beforeLevel != afterLevel)
                {
                    var tasks = NeedTasks.Select(n => n.TaskProperty).ToList();
                    tasks.ForEach(Task.RecountTaskLevel);

                    // Умный перенос дат
                    var thisAbNeedTasks =
                        NeedTasks.Where(n =>
                        {
                            return n.LevelProperty <= beforeLevel && n.ToLevelProperty >= beforeLevel;
                        }).ToList();

                    var nextAbNeedTasks =
                        NeedTasks.Where(n =>
                        {
                            return n.LevelProperty <= afterLevel && n.ToLevelProperty >= afterLevel;
                        }).Except(thisAbNeedTasks).ToList();

                    foreach (var nextAbNeedTask in nextAbNeedTasks)
                    {
                        var inThisLevel = (from n in thisAbNeedTasks
                                           let s = new string(n.TaskProperty.NameOfProperty.Where(char.IsLetter).ToArray())
                                           let s2 =
                                               new string(nextAbNeedTask.TaskProperty.NameOfProperty.Where(char.IsLetter).ToArray())
                                           where s == s2
                                           select n).FirstOrDefault();

                        if (inThisLevel != null)
                        {
                            StaticMetods.Locator.MainVM.MoveTask(nextAbNeedTask.TaskProperty, inThisLevel.TaskProperty);
                            nextAbNeedTask.TaskProperty.BeginDateProperty = inThisLevel.TaskProperty.BeginDateProperty;
                        }
                        else
                        {
                            nextAbNeedTask.TaskProperty.BeginDateProperty = MainViewModel.selectedTime.Date;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the LevelsWhenUp
        /// </summary>
        public List<int> LevelsWhenUp
        {
            get
            {
                return _levelsWhenUp ?? (_levelsWhenUp = new List<int>());
            }
            set
            {
                if (Equals(value, _levelsWhenUp)) return;
                _levelsWhenUp = value;
                OnPropertyChanged(nameof(LevelsWhenUp));
            }
        }

        /// <summary>
        /// Ссылки на квесты
        /// </summary>
        public List<Aim> LinkedQwests
        {
            get
            {
                var any = StaticMetods.PersProperty.Aims.Where(n => n.IsDoneProperty == false)
                    .Where(n => n.AbilitiLinksOf.Any(q => q == this));
                var linkedQwests = any
                    .Union(NeedAims.Select(n => n.AimProperty))
                    .Distinct()
                    .OrderBy(n => n)
                    .ToList();
                return
                    linkedQwests;
            }
        }

        /// <summary>
        /// Максимальное значение для счетчика скиллов
        /// </summary>
        public double MaxAbCounter
        {
            get { return GetCountedValue(100.0); }
        }

        /// <summary>
        /// Gets the MaxLevelProperty
        /// </summary>
        public override int MaxLevelProperty
        {
            get { return AbMaxLevel; }
        }

        /// <summary>
        /// Максимальное значение скилла
        /// </summary>
        public double MaxValue
        {
            get
            {
                return AbilitiModel.MaxValueOfAbility;
            }
        }

        /// <summary>
        /// Sets and gets Минимальный уровень для доступности скилла. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public int MinLevelProperty
        {
            get
            {
                //if (IsPayedProperty == false)
                //{
                //    return StaticMetods.PersProperty.MaxLevelProperty;
                //}

                //return NeedsForLevels.First(n => n.Level == (int)CellValue).PersLevel;
                return _minLevel;
            }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > 100)
                    value = StaticMetods.PersProperty.MaxLevelProperty;

                //if (_minLevel == value)
                //    return;

                _minLevel = value;
                //NeedsForLevels.First(n => n.Level == (int)CellValue).PersLevel = value;
                OnPropertyChanged(nameof(MinLevelProperty));
                OnPropertyChanged(nameof(IsEnebledProperty));
            }
        }

        /// <summary>
        /// Модификатор обучения
        /// </summary>
        public double ModificatorLearn
        {
            get
            {
                if (_modificatorLearn <= 0)
                {
                    _modificatorLearn = 10;
                }
                return _modificatorLearn;
            }
            set
            {
                if (value.Equals(_modificatorLearn)) return;
                _modificatorLearn = value;
                OnPropertyChanged(nameof(ModificatorLearn));
            }
        }

        /// <summary>
        /// Модификатор забывания скилла
        /// </summary>
        public double ModificatorOfForget
        {
            get
            {
                return 10;
                if (_modificatorOfForget <= 0)
                {
                    _modificatorOfForget = 10;
                }
                return _modificatorOfForget;
            }
            set
            {
                return;
                if (value.Equals(_modificatorOfForget)) return;
                _modificatorOfForget = value;
                OnPropertyChanged(nameof(ModificatorOfForget));
            }
        }

        /// <summary>
        /// Требования уровня скиллов для доступности скилла
        /// </summary>
        public ObservableCollection<NeedAbility> NeedAbilities => _needAbilities ?? (_needAbilities = new ObservableCollection<NeedAbility>());

        /// <summary>
        /// Требования квестов для прокачки скилла
        /// </summary>
        public ObservableCollection<CompositeAims> NeedAims { get; set; }

        /// <summary>
        /// Требования характеристик для доступности прокачки навыка
        /// </summary>
        public ObservableCollection<NeedCharact> NeedCharacts => _needCharacts ?? (_needCharacts = new ObservableCollection<NeedCharact>());

        /// <summary>
        /// Уровень требований по умолчанию при добавлении
        /// </summary>
        public int NeedLevelForDefoult
        {
            get
            {
                return _needLevelForDefoult;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > MaxLevelProperty)
                {
                    value = MaxLevelProperty;
                }

                if (_needLevelForDefoult == value)
                {
                    return;
                }

                _needLevelForDefoult = value;

                OnPropertyChanged(nameof(NeedLevelForDefoult));
            }
        }

        public List<NeedsForLevel> NeedsForLevels
        {
            get
            {
                CheckNeedsForLevels();
                return _needsForLevels;
            }
            set
            {
                if (Equals(value, _needsForLevels)) return;
                _needsForLevels = value;
                OnPropertyChanged(nameof(NeedsForLevels));
            }
        }

        /// <summary>
        /// Требования задач для прокачки скилла
        /// </summary>
        public ObservableCollection<NeedTasks> NeedTasks { get; set; }

        public double NforAllDone
        {
            get { return _nforAllDone; }
            set
            {
                if (value.Equals(_nforAllDone) || value < 1) return;

                _nforAllDone = value;
                OnPropertyChanged(nameof(NforAllDone));
            }
        }

        /// <summary>
        /// Описания требований для доступности скилла
        /// </summary>
        public string NotAllowReqwirements
        {
            get
            {
                return _notAllowReqwirements ?? (_notAllowReqwirements = string.Empty);
            }
            set
            {
                if (value == _notAllowReqwirements) return;
                _notAllowReqwirements = value;
                OnPropertyChanged(nameof(NotAllowReqwirements));
                OnPropertyChanged(nameof(ClearedNotAllowReqwirements));
                OnPropertyChanged(nameof(ForeGroundd));
            }
        }

        /// <summary>
        /// Ссылки на квесты
        /// </summary>
        public List<Aim> NotDoneLinkedQwests
        {
            get
            {
                return
                    StaticMetods.PersProperty.Aims.Where(n => !n.IsDoneProperty && n.IsActiveProperty)
                        .Where(n => n.AbilitiLinksOf.Any(q => q == this))
                        .OrderBy(n => n)
                        .ToList();
            }
        }

        /// <summary>
        /// Прозрачность для окна перса
        /// </summary>
        public double Opacity
        {
            get
            {
                if (NeedTasks.Any() || StaticMetods.PersProperty.Aims.Any(q => q.UpUbilitys.Any(z => z.Ability == this)))
                {
                    return 1.0;
                }
                else
                {
                    return 0.5;
                }
                //if (IsEnebledProperty == false)
                //{
                //    return 0.65;
                //}
                //return 1.0;
            }
        }

        public double Percent
        {
            get
            {
                var percent = Math.Round(((ValueProperty - ValueMinDouble) / (ValueMaxDouble - ValueMinDouble)) * 100.0);
                return percent;
            }
        }

        public string PercentString
        {
            get { return $" ({Percent}%)"; }
        }

        /// <summary>
        /// Задания которые будут отображаться в пипбое
        /// </summary>
        public IEnumerable<ComplecsNeed> PipBoyNeeds
        {
            get
            {
                var complecsNeeds =
                    ComplecsNeeds.Where(
                        n =>
                            n.IsQwest
                                ? !n.NeedQwest.AimProperty.IsDoneProperty && n.LevelProperty <= CellValue
                                : n.NeedTask.TaskProperty.IsEnabled).ToList();

                return complecsNeeds;
            }
        }

        //        payedLevel = value;
        //        OnPropertyChanged(nameof(PayedLevelProperty));
        //        OnPropertyChanged(nameof(RangName));
        //    }
        //}
        /// <summary>
        /// Gets the PlusAbName
        /// </summary>
        public string PlusAbName
        {
            get
            {
                return string.Empty;
                if (ToChaRelaysProperty >= 3)
                {
                    return "* ";
                }
                if (ToChaRelaysProperty <= 1)
                {
                    return "~ ";
                }
                return "+ ";
            }
        }

        // if (payedLevel == value || moreThenMax) { return; }
        /// <summary>
        /// Потенциальное влияние на опыт
        /// </summary>
        public double PotencialExpChange
        {
            get
            {
                var pot = (from cha in StaticMetods.PersProperty.Characteristics
                           let count = cha.NeedAbilitisProperty.Count(n => n.KoeficientProperty > 0)
                           from needAbility in cha.NeedAbilitisProperty
                           where needAbility.AbilProperty == this
                           select Convert.ToDouble(needAbility.KoeficientProperty) / count).Sum();
                return pot;
            }
        }

        // set { return; var moreThenMax = value > MaxLevelProperty;
        /// <summary>
        /// Sets and gets Предыдущие/следующие скиллы. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public List<AbilitiModel> PrevNextAbProperty
        {
            get
            {
                return prevNextAb;
            }

            set
            {
                if (prevNextAb == value)
                {
                    return;
                }

                prevNextAb = value;
                OnPropertyChanged(nameof(PrevNextAbProperty));
            }
        }

        // //var levelProperty = Convert.ToInt32(payedLevelProperty); //return levelProperty; }
        /// <summary>
        /// Псевдо значение для расчета опыта
        /// </summary>
        public double PseudoValue
        {
            get
            {
                var nVal = ValueProperty / 10.0;
                return ValueProperty * (1 + nVal);
            }
        }

        ///// <summary>
        ///// Sets and gets Купленный уровень. Changes to that property's value raise the
        ///// PropertyChanged event.
        ///// </summary>
        //public int PayedLevelProperty
        //{
        //    get
        //    {
        //        return 0;
        //        //if (!IsPayedProperty)
        //        //{
        //        //    return 0;
        //        //}
        //        //var payedLevelProperty = CellValue;
        //        //if (payedLevelProperty > 5)
        //        //{
        //        //    payedLevelProperty = 5;
        //        //}
        /// <summary>
        /// Активные квесты скилла (на текущий уровень)
        /// </summary>
        public List<Aim> QwestsActive
        {
            get
            {
                return
                    NeedAims.Where(n => n.LevelProperty == CellValue - 1)
                        .Where(n => !n.AimProperty.IsDoneProperty)
                        .Select(n => n.AimProperty)
                        .ToList();
            }
        }

        /// <summary>
        /// Квесты, влияющие на скилл
        /// </summary>
        public List<ComplecsNeed> QwestsRellays
        {
            get
            {
                return
                    ActiveComplecsNeeds.Where(n => n.NeedQwest != null)
                        .OrderBy(n => n.NeedQwest.LevelProperty)
                        .ThenBy(n => n.NeedQwest.AimProperty)
                        .ToList();
            }
        }

        /// <summary>
        /// Gets the RangName
        /// </summary>
        public override string RangName
        {
            get
            {
                if (StaticMetods.PersProperty.PersSettings.Is10AbLevels
                    || StaticMetods.PersProperty.PersSettings.Is5_5_50)
                {
                    return Characteristic.RoundValStr(ValueProperty <= StaticMetods.PersProperty.PersSettings.MaxAbLev
                        ? ValueProperty : StaticMetods.PersProperty.PersSettings.MaxAbLev);
                }
                else if (StaticMetods.PersProperty.PersSettings.IsFUDGE
                    ) {
                    var lev = CellValue;
                    if (lev > StaticMetods.MaxAbLevel)
                        lev = StaticMetods.MaxAbLevel;

                    return " " + StaticMetods.PersProperty.PersSettings.AbRangs[Convert.ToInt32(lev)].Name;
                }
                else
                {
                    var lev = CellValue;
                    if (lev > StaticMetods.MaxAbLevel)
                        lev = StaticMetods.MaxAbLevel;

                    return " " + StaticMetods.PersProperty.PersSettings.AbRangs[Convert.ToInt32(lev)].Name + "%";
                }
            }
        }

        /// <summary>
        /// Отдельные требования для возможности прокачаться до следующего уровня
        /// </summary>
        public List<RangseNeeds> RangseNeedses
        {
            get
            {
                if (_rangseNeedses == null)
                {
                    _rangseNeedses = new List<RangseNeeds>();
                }
                return _rangseNeedses;
            }
            set
            {
                _rangseNeedses = value;
            }
        }

        /// <summary>
        /// Задачи, которые влияют на скилл
        /// </summary>
        public List<Task> RelTasks
        {
            get
            {
                var enumerable = NeedTasks.Select(n => n.TaskProperty);

                return enumerable.ToList();
            }
        }

        /// <summary>
        /// Характеристики на которые влияет скилл
        /// </summary>
        public IEnumerable<Tuple<Characteristic, string>> RelToCharacts
        {
            get
            {
                return
                    (from characteristic in StaticMetods.PersProperty.Characteristics
                     from needAbility in characteristic.NeedAbilitisProperty
                     where needAbility.RightK > 0 && needAbility.AbilProperty == this
                     select new { needAbility, characteristic }).OrderByDescending(n => n.needAbility.RightK)
                        .Select(n => new Tuple<Characteristic, string>(n.characteristic, n.needAbility.GetKoefName));
            }
        }

        /// <summary>
        /// Требования квестов для того, чтобы можно было прокачать скилл
        /// </summary>
        public ObservableCollection<Aim> ReqwireAims => _reqwireAims ?? (_reqwireAims = new ObservableCollection<Aim>());

        /// <summary>
        /// Округленное до десятых значение прогресса скилла
        /// </summary>
        public double RoundValue => Math.Round(ValueProperty, 1);

        /// <summary>
        /// Управляющая характеристика
        /// </summary>
        public Characteristic RuleCharacterisic
        {
            get
            {
                //if (ruleCharacteristic == null)
                //{
                //    var characteristics = StaticMetods.PersProperty.Characteristics;
                //    if (characteristics?.Any() != true)
                //    {
                //        return null;
                //    }
                //    var ChaS =
                //        characteristics.Where(
                //            n => n.NeedAbilitisProperty.Any(q => q.AbilProperty == this && q.RightK > 0)).ToList();
                //    if (!ChaS.Any())
                //    {
                //        characteristics.First().NeedAbilitisProperty.First(n => n.AbilProperty == this).SetBaseK(10);
                //        ruleCharacteristic = characteristics.First();
                //    }
                //    else
                //    {
                //        ruleCharacteristic = ChaS.First();
                //    }
                //}

                return ruleCharacteristic;
            }
            set
            {
                return;
                //var beforeCharact = ruleCharacteristic;
                //var characteristics = StaticMetods.PersProperty.Characteristics;
                //foreach (var characteristic in characteristics)
                //{
                //    var needAbility = characteristic.NeedAbilitisProperty.First(n => n.AbilProperty == this);
                //    needAbility.SetBaseK(characteristic == value ? 10 : 0);
                //}
                //ruleCharacteristic = value;

                //ruleCharacteristic.RecountChaValue();
                //beforeCharact?.RecountChaValue();

                //OnPropertyChanged(nameof(RuleCharacterisic));
                //OnPropertyChanged(nameof(IsBuyVisibility));
            }
        }

        /// <summary>
        /// Gets or sets the SelH
        /// </summary>
        public string SelH
        {
            get
            {
                return DefSelH;

                //var lev = Convert.ToInt32(CellValue);
                //return HList[lev];
                //return _selH ?? (_selH = HList.First());
            }
            set
            {
                DefSelH = value;
                return;
                if (value == _selH) return;
                _selH = value;
                OnPropertyChanged(nameof(SelH));
            }
        }

        /// <summary>
        /// Gets the Показать комплексный элемент.
        /// </summary>
        public RelayCommand<object> ShowComplecsNeedItemAimCommand
        {
            get
            {
                return showComplecsNeedItemCommand
                       ?? (showComplecsNeedItemCommand =
                           new RelayCommand<object>(
                               item =>
                               {
                                   var it = item as ComplecsNeed;
                                   if (it != null)
                                   {
                                       if (it?.NeedTask != null)
                                       {
                                           ShowTaskFromNeedCommand.Execute(it.NeedTask);
                                       }

                                       if (it?.NeedQwest != null)
                                       {
                                           ShowQwestFromNeedCommand.Execute(it.NeedQwest);
                                       }
                                   }
                                   else
                                   {
                                       var cha = item as Characteristic;
                                       cha?.EditCharacteristic();
                                   }

                                   RefreshComplecsNeeds();
                               },
                               item => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Показать квест.
        /// </summary>
        public RelayCommand<Aim> ShowQwestCommand
        {
            get
            {
                return showQwestCommand
                       ?? (showQwestCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               StaticMetods.editAim(item);
                               RefreshComplecsNeeds();
                               OnPropertyChanged(nameof(ActiveComplecsNeeds));
                               OnPropertyChanged(nameof(PipBoyNeeds));
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
        /// Gets the Показать квест из требований.
        /// </summary>
        public RelayCommand<CompositeAims> ShowQwestFromNeedCommand
        {
            get
            {
                return showQwestFromNeedCommand
                       ?? (showQwestFromNeedCommand =
                           new RelayCommand<CompositeAims>(
                               item =>
                               {
                                   StaticMetods.editAim(item.AimProperty);

                                   OnPropertyChanged(nameof(ValueProperty));
                                   RefreshComplecsNeeds();
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
        /// Gets the Показать задачу из требований.
        /// </summary>
        public RelayCommand<NeedTasks> ShowTaskFromNeedCommand
        {
            get
            {
                return showTaskFromNeedCommand
                       ?? (showTaskFromNeedCommand = new RelayCommand<NeedTasks>(
                           item =>
                           {
                               item.TaskProperty.EditTask();
                               OnPropertyChanged(nameof(ValueProperty));
                               RefreshComplecsNeeds();
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
        /// Способности
        /// </summary>
        public List<ComplecsNeed> Skills
        {
            get
            {
                return
                    ActiveComplecsNeeds.Where(n => n.NeedTask != null)
                        .OrderBy(n => n.NeedTask.LevelProperty)
                        .ThenBy(n => n.NeedTask.MaxValue)
                        .ThenBy(n => n.NeedTask.TaskProperty)
                        .ToList();
            }
        }

        /// <summary>
        /// Активные способности скилла - на текущий уровень
        /// </summary>
        public List<Task> SkillsActive
        {
            get
            {
                return
                    NeedTasks
                        .Where(n => !n.TaskProperty.IsDelProperty)
                        .Where(n => n.TaskProperty.Recurrense.TypeInterval != TimeIntervals.Нет)
                        .Select(n => n.TaskProperty)
                        .ToList();
            }
        }

        /// <summary>
        /// Активные дела скилла - на текущий уровень
        /// </summary>
        public List<Task> TasksActive
        {
            get
            {
                return
                    NeedTasks.Where(n => n.LevelProperty == CellValue - 1)
                        .Where(n => !n.TaskProperty.IsDelProperty)
                        .Where(n => n.TaskProperty.Recurrense.TypeInterval == TimeIntervals.Нет)
                        .Select(n => n.TaskProperty)
                        .ToList();
            }
        }

        /// <summary>
        /// Приоритет для расчета опыта в стиле ТЕС. Главный, второстепенный, остальной...
        /// </summary>
        public double TESPriority
        {
            get
            {
                return _tesPriority;
            }
            set
            {
                if (value.Equals(_tesPriority)) return;
                _tesPriority = value;
                OnPropertyChanged(nameof(TESPriority));
            }
        }

        /// <summary>
        /// Sets and gets Сумма коэффициентов влияния на характеристики. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public double ToChaRelaysProperty
        {
            get
            {
                return _toChaRelaysProperty;
            }
            set
            {
                if (_toChaRelaysProperty == value) return;
                _toChaRelaysProperty = value;
                StaticMetods.RecountPersExp();
                OnPropertyChanged(nameof(PlusAbName));
            }
        }

        /// <summary>
        /// Gets the ToolTip
        /// </summary>
        public string ToolTip =>
            $"\bНавык\b \"{NameOfProperty}\"\n\bПрогресс:\b {LevelProperty}/{MaxLevelProperty}\n\bОписание\b: {DescriptionProperty}"
            ;

        /// <summary>
        /// Полностью затрачено на прокачку этого скилла
        /// </summary>
        public int TotalPayedCost => Pers.AbTotalCost(this);

        /// <summary>
        /// Для наград за элемент
        /// </summary>
        public ucElementRewardsViewModel UcElementRewardsViewModel
        {
            get { return new ucElementRewardsViewModel(this); }
        }

        /// <summary>
        /// Значение для расчета опыта
        /// </summary>
        public double ValToCountExp
        {
            get
            {
                var valToThisLevel = Pers.ExpToLevel(LevelProperty, RpgItemsTypes.ability);
                var valToNextLevel = Pers.ExpToLevel(LevelProperty + 1, RpgItemsTypes.ability);

                return IsAllNeedsComplete && CellValue != 0 ? valToNextLevel - 0.01 : valToThisLevel;
            }
        }

        /// <summary>
        /// Максимальное дробное значение
        /// </summary>
        public double ValueMaxDouble
        {
            get
            {
                return (CellValue + 1);
            }
            set
            {
                return;
                if (value.Equals(_valueMaxDouble)) return;
                _valueMaxDouble = value;
                OnPropertyChanged(nameof(ValueMaxDouble));
            }
        }

        /// <summary>
        /// Минимальное дробное значение
        /// </summary>
        public double ValueMinDouble
        {
            get
            {
                return CellValue;
            }
            set
            {
                return;
                if (value.Equals(_valueMinDouble)) return;
                _valueMinDouble = value;
                OnPropertyChanged(nameof(ValueMinDouble));
            }
        }

        /// <summary>
        /// Значение прогресса скилла
        /// </summary>
        public override double ValueProperty
        {
            get
            {
                var max = MaxValueOfAbility;
                var sum = val + FirstVal;
                return sum <= max ? sum : max;
            }
            set
            {
                double sum = this.val + _firstVal;

                if (sum == value)
                    return;

                var max = MaxValueOfAbility;

                if (value > max)
                    value = max;
                else if (value < 0)
                    value = _firstVal;

                val = value - _firstVal;

                StaticMetods.PersProperty.RecountChaValues();
                StaticMetods.PersProperty.NewRecountExp();
                CellValue = Math.Floor(ValueProperty);

                OnPropertyChanged(nameof(ValueToProgress));
            }
        }

        /// <summary>
        /// Gets the DataType
        /// </summary>
        Type IDragable.DataType
        {
            get { return typeof(AbilitiModel); }
        }

        /// <summary>
        /// Gets the DataType
        /// </summary>
        Type IDropable.DataType
        {
            get { return typeof(AbilitiModel); }
        }

        /// <summary>
        /// Максимальное значение скилла
        /// </summary>
        private double MaxVal
        {
            get
            {
                var maxVal = Pers.ExpToLevel(MaxLevelProperty, RpgItemsTypes.ability);
                return maxVal;
            }
        }

        /// <summary>
        /// Добавление нового скилла
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        /// <param name="charact">Характеристика, которая влияет на скилл</param>
        /// <param name="NeedTasks">Задачи от которых зависит скилл</param>
        /// <param name="NeedAims">Квесты от которых зависит скилл</param>
        /// <returns></returns>
        public static AbilitiModel AddAbility(
            Pers _pers,
            Characteristic charact = null,
            ObservableCollection<NeedTasks> NeedTasks = null,
            ObservableCollection<CompositeAims> NeedAims = null, string nameOf = "")
        {
            if (_pers.PersSettings.IsNoAbs)
            {
                var ability = new AbilitiModel(_pers);

                if (charact!=null)
                {
                    charact.NeedAbilitisProperty.First(n => n.AbilProperty == ability).KoeficientProperty = 10;
                    ability.RuleCharacterisic = charact;
                }

                var tsk = Task.AddTask(_pers.TasksTypes.FirstOrDefault(), ability, null, 0, nameOf);
                

                if (tsk.Item1 == false)
                {
                    StaticMetods.DeleteAbility(_pers, ability);
                }

                return ability;
            }
            else
            {
                var addAbilityView = new AddOrEditAbilityView
                {
                    btnOk =
                {
                    Visibility =
                        Visibility.Collapsed
                },
                    btnAddAbility =
                {
                    Visibility =
                        Visibility
                            .Visible
                },
                    btnCansel =
                {
                    Visibility =
                        Visibility.Visible
                }
                };

                var context = (AddOrEditAbilityViewModel)addAbilityView.DataContext;
                context.addAb();
                var selAbiliti = context.SelectedAbilitiModelProperty;
                context.IsSetViz = true;
                //selAbiliti.MinLevelProperty = _pers.MaxLevelProperty;
                if (charact != null)
                {
                    charact.NeedAbilitisProperty.First(n => n.AbilProperty == selAbiliti).KoeficientProperty = 10;
                    selAbiliti.RuleCharacterisic = charact;
                }
                else
                {
                    if (NeedTasks != null)
                    {
                        foreach (var needTaskse in NeedTasks)
                        {
                            selAbiliti.NeedTasks.Add(needTaskse);
                        }
                    }

                    if (NeedAims != null)
                    {
                        foreach (var needAim in NeedAims)
                        {
                            selAbiliti.NeedAims.Add(needAim);
                        }
                    }
                }

                selAbiliti.SetMinMaxValue();
                selAbiliti.CountVisibleLevelValue();
                selAbiliti.ImageProperty = DefoultPicsAndImages.DefoultAbilImage;
                context.RefreshInfoCommand.Execute(null);
                //addAbilityView.chaRelays.IsExpanded = true;
                if (string.IsNullOrEmpty(nameOf)) nameOf = "Название";
                context.SelectedAbilitiModelProperty.NameOfProperty = nameOf;
                Messenger.Default.Send("Фокусировка на названии!");
                FocusManager.SetFocusedElement(addAbilityView, addAbilityView.txtName);
                addAbilityView.ShowDialog();
                StaticMetods.RecauntAllValues();

                StaticMetods.PersProperty.RecountChaValues();
                _pers.NewRecountExp();

                return context.SelectedAbilitiModelProperty;
            }
        }

        /// <summary>
        /// Покупка нового уровня для скилла
        /// </summary>
        /// <param name="selAb"></param>
        /// <param name="persProperty"></param>
        /// <param name="notOpenToEdit"></param>
        public static void BuyAbLevel(AbilitiModel selAb, Pers persProperty, bool notOpenToEdit = false)
        {
            return;

            //SkillsMayUpModel.GetBefore();

            //// Для артефактов и бейджей
            //var vc = new ViewChangesClass(persProperty.InventoryItems.Union(persProperty.ShopItems).ToList());
            //vc.GetValBefore();

            ////if (persProperty.AbilitisPoints < 1) return;
            //if (persProperty.AbilitisPoints < selAb.AbCost) return;
            //var before = persProperty.AbilitisPoints;
            //selAb.BuyedInThisLevel = true;

            //var isActivate = selAb.IsPayedProperty == false;
            ////------------------------------------------------
            //var viewChanges = new List<viewChangesModel>();
            //StaticMetods.PlaySound(Resources.NewLevel);
            //var congrant = new Congranteletions();
            //congrant.btnClose.Click += (o, args) => { congrant.Close(); };
            //Messenger.Default.Send<Window>(congrant);
            //congrant.Topmost = true;
            //congrant.storyPanel.Visibility = Visibility.Collapsed;

            //var prs = StaticMetods.PersProperty;
            //var chaBefore = prs.Characteristics.Select(n => new { cha = n, val = n.CellValue }).ToList();
            //var abBefore = prs.Characteristics.Select(n => new { cha = n, val = n.ValueProperty }).ToList();
            ////------------------------------------------------

            //selAb.AddLevelsWhenUp(persProperty.PersLevelProperty);

            //if (isActivate)
            //{
            //    // Активировать скилл
            //    selAb.IsPayedProperty = true;
            //    //Квесты...
            //    foreach (var skill in selAb.Skills)
            //    {
            //        foreach (var aim in skill.NeedTask.TaskProperty.AimToSkill)
            //        {
            //            if (aim.MinLevelProperty > StaticMetods.PersProperty.PersLevelProperty)
            //                aim.MinLevelProperty = StaticMetods.PersProperty.PersLevelProperty;
            //            aim.IsActiveProperty = true;
            //        }
            //    }
            //    var thisAbLevelTasks = (from needTask in selAb.NeedTasks
            //                            select needTask).ToList();
            //    foreach (var task in thisAbLevelTasks)
            //    {
            //        task.TaskProperty.BeginDateProperty = MainViewModel.selectedTime;
            //    }
            //    StaticMetods.RecauntAllValues();
            //    persProperty.UpdateAbilityPoints();
            //    StaticMetods.RefreshAllQwests(StaticMetods.PersProperty, true, true, true);
            //    selAb.OnPropertyChanged(nameof(CurRang));
            //    selAb.OnPropertyChanged(nameof(CurLevDescr));
            //    selAb.RefreshComplecsNeeds();
            //    StaticMetods.PersProperty.UpdateAbilityPoints();
            //    selAb.MinLevelProperty = persProperty.PersLevelProperty;
            //    selAb.SelH = selAb.HList.First();
            //    StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.НавыкОткрыт, selAb);
            //}
            //else
            //{
            //    StaticMetods.PersProperty.UpdateAbilityPoints();
            //    //var after = persProperty.AbilitisPoints;
            //    //var levWhereClone = selAb.CellValue - 1;
            //    //if (!selAb.NeedTasks.Any(n => n.LevelProperty == selAb.CellValue))
            //    //{
            //    //    CloneTasksForNextLevel(selAb, levWhereClone);
            //    //}
            //}

            //// Показать подбадривающее окно!
            //ShowAbUpDownMessege(selAb, !isActivate);

            //foreach (var characteristic in prs.Characteristics)
            //{
            //    characteristic.ValueProperty = characteristic.GetChaValue();
            //}
            //var changeCharacts = (from characteristic in prs.Characteristics
            //                      join vb in chaBefore
            //                          on characteristic.GUID equals vb.cha.GUID
            //                      select
            //                          new { cha = characteristic, before = vb.val, after = characteristic.CellValue })
            //    .Where(n => n.after != n.before);
            //var chaChenge = (
            //    from chengeCha in changeCharacts.ToList()
            //    let fromLev = chengeCha.before
            //    let toLev = chengeCha.after
            //    let characteristic = chengeCha.cha
            //    select
            //        new viewChangesModel(
            //            "Характеристика",
            //            $"{characteristic.NameOfProperty}:",
            //            characteristic.Cvet,
            //            chengeCha.before,
            //            chengeCha.after,
            //            characteristic.Rangs)
            //        {
            //            @from = fromLev,
            //            to = toLev,
            //            ChangeProperty = toLev - fromLev,
            //            MinValueProperty = 0.0, //characteristic.ValueMinDouble,
            //            MaxValueProperty = StaticMetods.MaxChaLevel, //characteristic.ValueMaxDouble,
            //            ImageProperty = characteristic.ImageProperty,
            //            RangProperty = $"{characteristic.RangName.ToLower()}",
            //            RangProperty2 = "",
            //            IsValVisibleProperty = Visibility.Collapsed,
            //            IsShowScale = true
            //        }).ToList();

            //foreach (var vm in chaChenge)
            //{
            //    vm.ChangeString = ""; //viewChangesModel.ChangeProperty > 0? $"+": $"-";
            //}
            //var vcmCha = new List<viewChangesModel>();
            //vcmCha.AddRange(chaChenge.OrderBy(n => n.to)); //.Where(n => Math.Abs(n.ChangeProperty) >= 0.001)
            //viewChanges.AddRange(vcmCha);
            //Messenger.Default.Send(viewChanges);

            //var saveCommand = new RelayCommand(() => { congrant.Close(); });
            //congrant.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Space)));
            //congrant.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Return)));
            //if (vcmCha.Any())
            //{
            //    congrant.imgImage.Source = StaticMetods.getImagePropertyFromImage(
            //        StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png")));
            //    congrant.txtHeader.Foreground = Brushes.ForestGreen;
            //    congrant.txtMessege.Foreground = Brushes.ForestGreen;
            //    congrant.txtHeader.Text = "Характеристики подрастают!";
            //    congrant.txtMessege.Visibility = Visibility.Collapsed;
            //    //congrant.txtMessege.Text = "Характеристики подрастают!";
            //    congrant.ShowDialog();
            //}

            //// Артефакты и бейджи
            //vc.GetValAfter();
            //Brush col = Brushes.Green;
            //var itemImageProperty =
            //    StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png"));

            //var header = $"Получены награды!";
            //vc.ShowChanges(header, col, itemImageProperty, string.Empty, false, false, true);

            //// Удаляем требования для текущего уровня
            //ClearReqvirements(selAb);

            //SkillsMayUpModel.ShowAbUps();

            //if (StaticMetods.PersProperty.IsSetViz == false)
            //{
            //    StaticMetods.PersProperty.IsSetViz = true;
            //    MainViewModel.IsEditModeAfterAbLevUp = true;
            //}
            //else
            //{
            //    MainViewModel.IsEditModeAfterAbLevUp = false;
            //}

            ////------------------------------------------------
            //if (notOpenToEdit == false)
            //{
            //    AddOrEditAbilityViewModel.OpenAbNeedsSettings(selAb); // Открываем редактирование
            //}
        }

        /// <summary>
        /// Удалить все требования скилла
        /// </summary>
        /// <param name="selAb"></param>
        public static void ClearReqvirements(AbilitiModel selAb)
        {
            //selAb.NeedCharacts.Clear();
            //selAb.NeedAbilities.Clear();
            //selAb.ReqwireAims.Clear();
            //selAb.MinLevelProperty = 1;
        }

        /// <summary>
        /// Клонируем задачи для следующего уровня навыка
        /// </summary>
        /// <param name="selAb">Навык</param>
        /// <param name="levWhereClone">Уровень С КОТОРОГО клонируем (предыдущий)</param>
        public static void CloneTasksForNextLevel(AbilitiModel selAb, double levWhereClone, bool isInvert = false)
        {
            int nl = (int)levWhereClone + 1;
            if (isInvert)
            {
                nl = (int)levWhereClone - 1;
            }
            if (nl > StaticMetods.MaxAbLevel)
            {
                return;
            }
            if (nl <= 0)
            {
                return;
            }

            foreach (var source in selAb.NeedTasks.Where(n => n.LevelProperty == nl).ToList())
            {
                source.TaskProperty.Delete(StaticMetods.PersProperty);
            }

            foreach (
                var needTaskse in
                    selAb.NeedTasks.Where(n => n.LevelProperty >= levWhereClone && n.ToLevelProperty <= levWhereClone)
                        .OrderBy(n => n.TaskProperty)
                        .ToList())
            {
                var nextLev = nl;
                var tsk = AddOrEditAbilityViewModel.CloneTask(needTaskse, nextLev, selAb, false);
                tsk.RecountAutoValues();
            }

            selAb.RefreshComplecsNeeds();
        }

        /// <summary>
        /// Сравниваем навыки по тому, какой влияет на более слабую характеристику
        /// </summary>
        /// <param name="persProperty"></param>
        /// <param name="ab"></param>
        /// <param name="ab2"></param>
        /// <returns></returns>
        public static int CompareToRelayOfHealChas(Pers persProperty, AbilitiModel ab, AbilitiModel ab2)
        {
            var minCha1 = minRelChaVal(persProperty, ab);
            var minCha2 = minRelChaVal(persProperty, ab2);
            return minCha1.CompareTo(minCha2);
        }

        /// <summary>
        /// The GetLevelHardness
        /// </summary>
        /// <param name="abVal">The <see cref="int"/></param>
        /// <returns>The <see cref="double"/></returns>
        public static double GetLevelHardness(int abVal)
        {
            //double thisLevCost = Pers.AbCostByLev(abVal);
            //double max = Pers.AbCostByLev(5);
            //double result = thisLevCost / max;
            //return StaticMetods.RoundTo5(result * 100.0);
            var d = (double)((double)abVal / StaticMetods.MaxAbLevel) * 100.0;
            return d; //StaticMetods.RoundTo5(d);
        }

        /// <summary>
        /// The SaleAb
        /// </summary>
        /// <param name="item">The <see cref="AbilitiModel"/></param>
        public static void SaleAb(AbilitiModel item)
        {
            item.IsPayedProperty = false;
            //item.ValueProperty = 0;
            foreach (var skill in item.Skills)
            {
                foreach (var aim in skill.NeedTask.TaskProperty.AimToSkill)
                {
                    aim.MinLevelProperty = StaticMetods.PersProperty.MaxLevelProperty;
                }
            }
            StaticMetods.PersProperty.UpdateAbilityPoints();
        }

        /// <summary>
        /// Отказ/продажа от нового уровня скилла
        /// </summary>
        /// <param name="selAb"></param>
        /// <param name="persProperty"></param>
        public static void SaleAbLevel(AbilitiModel selAb, Pers persProperty)
        {
        }

        /// <summary>
        /// The ChangeChecked
        /// </summary>
        public void ChangeChecked()
        {
        }

        /// <summary>
        /// The ChangeValuesOfRelaytedItems
        /// </summary>
        public override void ChangeValuesOfRelaytedItems()
        {
            //// Расчитываем значения скиллов
            //UpdateChaValues();
            //SetToChaRelays();
            //StaticMetods.RecountTaskLevels();
        }

        /// <summary>
        /// The CheckIfPerk
        /// </summary>
        /// <param name="_value">The <see cref="int"/></param>
        /// <param name="_maxLevelProperty">The <see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        public override int CheckIfPerk(int _value, int _maxLevelProperty)
        {
            return _value;
        }

        /// <summary>
        /// Проверка требований для уровней
        /// </summary>
        public void CheckNeedsForLevels()
        {
            if (_needsForLevels == null ||
                _needsForLevels.Count != StaticMetods.PersProperty.PersSettings.MaxLevOfAbForProg + 1)
            {
                _needsForLevels = new List<NeedsForLevel>();
                for (int i = 0; i < StaticMetods.PersProperty.PersSettings.MaxLevOfAbForProg + 1; i++)
                {
                    _needsForLevels.Add(new NeedsForLevel { Level = i });
                }
            }
        }

        /// <summary>
        /// The ClearValue
        /// </summary>
        public void ClearValue()
        {
            LevelsWhenUp.Clear();
            RecountValue();
        }

        /// <summary>
        /// Сравнение скиллов
        /// </summary>
        /// <param name="other">Другой скилл</param>
        /// <returns></returns>
        public int CompareTo(AbilitiModel other)
        {
            // Сравниваем по важности
            //var compareByTes = TESPriority.CompareTo(other.TESPriority);
            //if (compareByTes != 0)
            //{
            //    return -compareByTes;
            //}

            //// Сравниваем по доступности
            //var compareByEnabled = IsEnebledProperty.CompareTo(other.IsEnebledProperty);
            //if (compareByEnabled != 0)
            //{
            //    return -compareByEnabled;
            //}

            // По купленности
            //var compareByBay = IsPayedProperty.CompareTo(other.IsPayedProperty);
            //if (compareByBay != 0)
            //{
            //    return -compareByBay;
            //}

            ////// Сравниваем по возможности доступности
            //int compareByMayEnabled = IsMayToEnabled.CompareTo(other.IsMayToEnabled);
            //if (compareByMayEnabled != 0)
            //{
            //    return -compareByMayEnabled;
            //}

            //// Сравниваем по возможности покупки
            //int compareByBy = IsBuyVisibility.CompareTo(other.IsBuyVisibility);
            //if (compareByBy != 0)
            //{
            //    return compareByBy;
            //}

            //// Сравниваем по влиятельности
            //var exp = ToChaRelaysProperty.CompareTo(other.ToChaRelaysProperty);
            //if (exp != 0)
            //{
            //    return -exp;
            //}

            // Сравниваем по влиятельности на самую слабую характеристику
            //var thisRelCha = StaticMetods.PersProperty.Characteristics.Where(
            //    n => n.NeedAbilitisProperty.Any(q => q.AbilProperty == this && q.RightK > 0)).ToList();
            //var thisMin = thisRelCha.Any() ? thisRelCha.Min(q => q.ValueProperty) : 999;
            //var otherRelCha = StaticMetods.PersProperty.Characteristics.Where(
            //    n => n.NeedAbilitisProperty.Any(q => q.AbilProperty == other && q.RightK > 0)).ToList();
            //var otherMin = otherRelCha.Any() ? otherRelCha.Min(q => q.ValueProperty) : 999;
            //var minCha =
            //   thisMin.CompareTo(otherMin);
            //if (minCha != 0)
            //{
            //    return minCha;
            //}

            // По доступности
            var byAllow = string.IsNullOrWhiteSpace(NotAllowReqwirements)
                .CompareTo(string.IsNullOrWhiteSpace(other.NotAllowReqwirements));
            if (byAllow != 0)
            {
                return -byAllow;
            }

            //// Сравниваем по прогрессу
            var compareByProg = ValueProperty.CompareTo(other.ValueProperty);
            if (compareByProg != 0)
            {
                return -compareByProg;
            }

            // По мин. уровню
            var forMinLev = MinLevelProperty.CompareTo(other.MinLevelProperty);
            if (forMinLev != 0)
            {
                return forMinLev;
            }

            //По важности
            ////var chas = StaticMetods.PersProperty.Characteristics;
            ////if (chas?.Any() == true)
            ////{
            ////    var it = chas.Sum(n =>
            ////        n.NeedAbilitisProperty.Where(q => q.AbilProperty == this).Sum(q => q.KoeficientProperty));
            ////    var oth = chas.Sum(n =>
            ////        n.NeedAbilitisProperty.Where(q => q.AbilProperty == other).Sum(q => q.KoeficientProperty));
            ////    var compareByPrior = it.CompareTo(oth);
            ////    if (compareByPrior != 0)
            ////    {
            ////        return -compareByPrior;
            ////    }
            ////}

            // По заполненности
            var compareByTasks = NeedTasks.Any().CompareTo(other.NeedTasks.Any());
            if (compareByTasks != 0)
            {
                return -compareByTasks;
            }

            // По важности 2 Fake
            double thSum = getPriorOfAb(this);
            double othSum = getPriorOfAb(other);
            var compPr = thSum.CompareTo(othSum);
            //int byHealChas = CompareToRelayOfHealChas(StaticMetods.PersProperty, this, other);
            if (compPr != 0)
            {
                return -compPr;
            }
            //if (StaticMetods.PersProperty.IsSortByBalance)
            //{
            //    if (byHealChas != 0)
            //    {
            //        return byHealChas;
            //    }

            // if (compPr != 0) { return -compPr; }

            //}
            //else
            //{
            //    if (compPr != 0)
            //    {
            //        return -compPr;
            //    }

            //    if (byHealChas != 0)
            //    {
            //        return byHealChas;
            //    }
            //}

            // Сравнение по требованиям
            //var compareByReq = ClearedNotAllowReqwirements.Count().CompareTo(other.ClearedNotAllowReqwirements.Count());
            //if (compareByReq != 0)
            //{
            //    return compareByReq;
            //}

            //var exp2 = TESPriority.CompareTo(other.TESPriority);
            //if (exp2 != 0)
            //{
            //    return -exp2;
            //}

            ////Сравниваем по индексу
            //var allAbills = StaticMetods.PersProperty.Abilitis;
            //return allAbills.IndexOf(this).CompareTo(allAbills.IndexOf(other));

            return NameOfProperty.CompareTo(other.NameOfProperty);
        }

        public double CountThisLevNeeds()
        {
            var thisLevNeeds = NeedTasks.Count(n => CellValue >= n.LevelProperty && CellValue <= n.ToLevelProperty);
            return thisLevNeeds > 0 ? thisLevNeeds : 1;
        }

        /// <summary>
        /// The Drop
        /// </summary>
        /// <param name="data">The <see cref="object"/></param>
        /// <param name="index">The <see cref="int"/></param>
        public void Drop(object data, int index = -1)
        {
            return;
            var allablist = StaticMetods.Locator.ucAbilitisVM.ChaAbilitises;
            var allAbs = StaticMetods.PersProperty.Abilitis;
            var indB = allAbs.IndexOf(this);
            var chaA = data as AbilitiModel;
            var indA = allAbs.IndexOf(chaA);
            allAbs.Move(indA, indB);
            allablist.Refresh();
            foreach (var characteristic in StaticMetods.PersProperty.Characteristics)
            {
                characteristic.RefreshRelAbs();
            }
        }

        /// <summary>
        /// Открывает окно редактирования нового скилла
        /// </summary>
        public void EditAbility(Characteristic cha = null, int selH = -1, bool isEditMode = false)
        {
            if (StaticMetods.PersProperty.PersSettings.IsNoAbs)
            {
                NoAbsTask?.EditTask();
            }
            else
            {
                if (StaticMetods.PersProperty.IsSetViz == true) isEditMode = true;
                if (!NeedTasks.Any()) isEditMode = true;

                var _selectedAbility = this;

                var showAbilityView = new AddOrEditAbilityView
                {
                    btnOk =
                {
                    Visibility =
                        Visibility.Visible
                },
                    btnAddAbility =
                {
                    Visibility =
                        Visibility
                            .Collapsed
                },
                    btnCansel =
                {
                    Visibility =
                        Visibility.Collapsed
                }
                };

                StaticMetods.Locator.AddOrEditAbilityVM.ChaToPrevNext = cha;
                StaticMetods.Locator.AddOrEditAbilityVM.IsSetViz = isEditMode;
                StaticMetods.Locator.AddOrEditAbilityVM.SetSelAbiliti(_selectedAbility);

                //showAbilityView.chaRelays.IsExpanded = false;
                var index = (int)CellValue;
                index = index > 0 ? index : 0;
                index = selH > 0 ? selH : index;
                if (index > StaticMetods.MaxAbLevel)
                {
                    index = StaticMetods.MaxAbLevel;
                }
                SelH = HList[index];

                var saveCommand = new RelayCommand(() =>
                {
                    StaticMetods.Locator.AddOrEditAbilityVM.OkCommand.Execute(null);
                    showAbilityView.Close();
                });
                showAbilityView.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.S, ModifierKeys.Control)));

                // Экспорт скиллов
                var ExportChaCommand = new RelayCommand(() => StaticMetods.Locator.AddOrEditAbilityVM.IsDevMode = true);
                showAbilityView.InputBindings.Add(new KeyBinding(ExportChaCommand,
                    new KeyGesture(Key.E,
                        ModifierKeys.Control)));

                showAbilityView.ShowDialog();

                StaticMetods.AbillitisRefresh(StaticMetods.PersProperty);
                StaticMetods.RefreshAllQwests(StaticMetods.PersProperty, true, true, true);

            }
        }

        /// <summary>
        /// Важность скилла (в зависимости от влияния на характеристики)
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public double GetAbNeedness(Pers pers)
        {
            return 0;
            double chaCount = pers.Characteristics.Count;
            var maxChaRelay = 10.0;
            var KSum = (from characteristic in pers.Characteristics
                        from needAbility in characteristic.NeedAbilitisProperty
                        where needAbility.AbilProperty == this
                        select needAbility).Sum(n => n.KoeficientProperty);

            var k = 6.0 / chaCount;
            var needness = k * KSum / maxChaRelay;
            return needness;
        }

        /// <summary>
        /// Базовая стоимость
        /// </summary>
        /// <returns>Базовая стоимость в зависимости от сложности</returns>
        public int GetBaseCost()
        {
            var hardnessProperty = HardnessProperty;
            var baseCost = GetHardCost(hardnessProperty);
            return baseCost;
        }

        /// <summary>
        /// The GetDefoultImageFromElement
        /// </summary>
        /// <returns>The <see cref="byte[]"/></returns>
        public override byte[] GetDefoultImageFromElement()
        {
            return DefoultPicsAndImages.DefoultAbilImage;
        }

        /// <summary>
        /// The GetLevel
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public override int GetLevel()
        {
            return 0;
        }

        /// <summary>
        /// Получаем мин и макс значения для условий прокачки
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public int GetNeedLev()
        {
            var rng = HList.FirstOrDefault(n => n == DefSelH);
            var num = HList.IndexOf(rng);
            return num;
        }

        public double getPriorOfAb(AbilitiModel ab)
        {
            //var chas =
            //    StaticMetods.PersProperty.Characteristics.Where(
            //        n => n.NeedAbilitisProperty.Any(q => q.AbilProperty == ab && q.KoeficientProperty > 0)).ToList();
            //double bef = chas.Sum(n => n.ValueProperty);
            //chas.ForEach(n => Characteristic.recountOneChaVal(n, ab));
            //double after = chas.Sum(n => n.FakeForSort);
            //return after - bef;
            double bef = StaticMetods.PersProperty.RetExp(ab, 0);
            double aft = StaticMetods.PersProperty.RetExp(ab, 1);
            double priorOfAb = aft - bef;
            //var ofAb = Math.Round(priorOfAb, 1);
            return priorOfAb;
        }

        /// <summary>
        /// Характеристики на которые влияет навык.
        /// </summary>
        /// <returns></returns>
        public List<Characteristic> GetRelCharacteristics()
        {
            return
                StaticMetods.PersProperty.Characteristics.Where(n =>
                    n.NeedAbilitisProperty.Any(q => q.KoeficientProperty > 0 && q.AbilProperty == this)).ToList();
        }

        /// <summary>
        /// Получить требования скилла
        /// </summary>
        public void GetReqwirements()
        {
            _notAllowReqwirements = string.Empty;
            _isMayToEnabled = true;

            //if (StaticMetods.PersProperty.AbilitisPoints < CostProperty)
            //{
            //    _notAllowReqwirements += HaveNotAbPointsText;
            //    //goto exit;
            //}

            if (MinLevelProperty > StaticMetods.PersProperty.PersLevelProperty)
            {
                _notAllowReqwirements += $"Уровень персонажа >={MinLevelProperty}; ";
                _isMayToEnabled = false;
                goto exit;
            }

            // Проверка по уже купленным
            //if (LevelsWhenUp.Any())
            //{
            //    if (StaticMetods.PersProperty.PersLevelProperty <= LevelsWhenUp.Max())
            //    {
            //        _notAllowReqwirements += HaveNotNextLevText;
            //        _isMayToEnabled = false;
            //        //goto exit;
            //    }
            //}

            // Проверка доступности по слабым характеристикам
            //if (StaticMetods.PersProperty.Characteristics != null && StaticMetods.PersProperty.Characteristics.Any() &&
            //    StaticMetods.PersProperty.PersSettings.IsCharactBalanse)
            //{
            //    //double max = StaticMetods.PersProperty.Characteristics.Max(n => n.CellValue);
            //    //var ruleCha = RuleCharacterisic;
            //    //var all = !StaticMetods.PersProperty.Characteristics.All(n => n.CellValue == max);
            //    //if (ruleCha.CellValue >= max && all)
            //    //{
            //    //    _notAllowReqwirements += $"Прокачать другие характеристики; ";
            //    //    _isMayToEnabled = false;
            //    //    goto exit;
            //    //}
            //    double min = StaticMetods.PersProperty.Characteristics.Min(n => n.CellValue);
            //    var ruleCha = RuleCharacterisic;
            //    if (ruleCha?.CellValue > min)
            //    {
            //        _notAllowReqwirements += UpAnotherCharactsText;
            //        _isMayToEnabled = false;
            //        //goto exit;
            //    }
            //}

            // Проверка доступности по квестам
            foreach (var aim in ReqwireAims.Where(n => !n.IsDoneProperty))
            {
                _notAllowReqwirements += $"{aim.NameOfProperty}; ";
                _isMayToEnabled = false;
            }
            if (!string.IsNullOrWhiteSpace(NotAllowReqwirements)) goto exit;

            // Проверка доступности по характеристикам
            StaticMetods.GetCharactReq(NeedCharacts, ref _notAllowReqwirements, ref _isMayToEnabled);
            if (!string.IsNullOrWhiteSpace(NotAllowReqwirements)) goto exit;

            // Проверка доступности по скиллам
            StaticMetods.GetAbillsReq(NeedAbilities, ref _notAllowReqwirements, ref _isMayToEnabled);
            if (!string.IsNullOrWhiteSpace(NotAllowReqwirements)) goto exit;

            exit:
            //CheckIsHalyava(NotAllowReqwirements);
            OnPropertyChanged(nameof(IsMayToEnabled));
            OnPropertyChanged(nameof(IsMayToEnabled));
            OnPropertyChanged(nameof(IsBuyVisibility));
            OnPropertyChanged(nameof(NotAllowReqwirements));
        }

        /// <summary>
        /// Пересчитывает автоматом минимальные значения для скиллов
        /// </summary>
        public void RecountMinValues()
        {
            return;
            if (IsAutoCountMinValues && NeedTasks != null && NeedTasks.Any())
            {
                double count = NeedTasks.Count;
                var minVal = NeedAims.Sum(n => n.KRel);
                if (count >= 0)
                {
                    var needsWithoutParrents = NeedTasks.Where(n => !n.TaskProperty.ParrentTasks.Any()).ToList();

                    var oneVal = (100.0 - minVal) / needsWithoutParrents.Count();
                    var mVal = minVal;
                    foreach (var needTaskse in needsWithoutParrents)
                    {
                        needTaskse.MinValue = Convert.ToInt32(mVal);
                        if (mVal != minVal)
                            needTaskse.MinValue = Convert.ToInt32(StaticMetods.RoundTo10(needTaskse.MinValue));
                        needTaskse.MaxValue = 100;
                        mVal += oneVal;
                    }

                    foreach (var needTaskse in needsWithoutParrents.Where(n => n.TaskProperty.NextActions.Any()))
                    {
                        var nexxts = new List<NeedTasks>();
                        GetAllNexxts(needTaskse, nexxts);
                        double min = needTaskse.MinValue;
                        var d = nexxts.Count;
                        if (d <= 0) continue;
                        var one = (100.0 - min) / d;

                        for (var i = 0; i < nexxts.Count; i++)
                        {
                            if (i != 0)
                            {
                                min = min + one;
                                nexxts[i].MinValue = Convert.ToInt32(min);
                                nexxts[i - 1].MaxValue = nexxts[i].MinValue - 1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Пересчет значения и фильтров
        /// </summary>
        public void RecountValue()
        {
            //ValueProperty = LevelsWhenUp.Count(n => n <= StaticMetods.PersProperty.PersLevelProperty) * 20.0;
            foreach (var needTaskse in NeedTasks)
            {
                needTaskse.TaskProperty.IsDelProperty = CellValue > needTaskse.ToLevelProperty &&
                                                        needTaskse.LevelProperty <= CellValue;
            }
        }

        /// <summary>
        /// Обновить видимости продажи покупки скилла
        /// </summary>
        public void RefreshAbBuySale()
        {
            OnPropertyChanged(nameof(IsBuyVisibility));
            OnPropertyChanged(nameof(IsSaleVisibility));
        }

        /// <summary>
        /// The RefreshComplecsNeeds
        /// </summary>
        public void RefreshComplecsNeeds()
        {
            RecountMinValues();
            OnPropertyChanged(nameof(ComplecsNeeds));
            OnPropertyChanged(nameof(ActiveComplecsNeeds));
            OnPropertyChanged(nameof(PipBoyNeeds));
            OnPropertyChanged(nameof(Skills));
            OnPropertyChanged(nameof(ActiveQwests));
            OnPropertyChanged(nameof(IsAllNeedsComplete));
            OnPropertyChanged(nameof(QwestsRellays));
            OnPropertyChanged(nameof(AllPercQwests));
            OnPropertyChanged(nameof(ComplecsComplecsNeed));
        }

        public void RefreshElRevard()
        {
            OnPropertyChanged(nameof(UcElementRewardsViewModel));
        }

        /// <summary>
        /// The RefreshEnabled
        /// </summary>
        public void RefreshEnabled()
        {
            GetReqwirements();
            OnPropertyChanged(nameof(IsEnebledProperty));
            OnPropertyChanged(nameof(IsBuyVisibility));
        }

        /// <summary>
        /// The RefreshLinkedQwests
        /// </summary>
        public void RefreshLinkedQwests()
        {
            OnPropertyChanged(nameof(LinkedQwests));
            OnPropertyChanged(nameof(NotDoneLinkedQwests));
        }

        public void RefreshRev()
        {
            OnPropertyChanged(nameof(UcElementRewardsViewModel));
        }

        /// <summary>
        /// Все связанные задачи с скиллом и его дочерними квестами
        /// </summary>
        /// <returns></returns>
        public List<Task> RelatedTasks()
        {
            IEnumerable<Task> rel = NeedTasks.Select(n => n.TaskProperty).ToList();

            var inQwests = (from task in rel
                            from aim in StaticMetods.PersProperty.Aims
                            where aim.NeedsTasks.Any(n => n.TaskProperty == task)
                            from needTaskse in aim.NeedsTasks
                            where needTaskse.TaskProperty != task
                            select needTaskse.TaskProperty).ToList();

            var inNeedTasks =
                NeedAims.Select(b => b.AimProperty)
                    .SelectMany(compositeAimse => compositeAimse.NeedsTasks.Select(n => n.TaskProperty))
                    .ToList();

            inQwests.AddRange(inNeedTasks);

            var inComposite = from needAim in NeedAims
                              from needsTask in needAim.AimProperty.NeedsTasks
                              select needsTask.TaskProperty;

            rel = rel.Concat(inQwests).Concat(inComposite).Distinct();

            return rel.ToList();
        }

        /// <summary>
        /// The Remove
        /// </summary>
        /// <param name="i">The <see cref="object"/></param>
        public void Remove(object i)
        {
        }

        /// <summary>
        /// The SetMinMaxValue
        /// </summary>
        public override void SetMinMaxValue()
        {
            ValueMinProperty =
                Convert.ToInt32(Pers.ExpToLevel(LevelProperty, RpgItemsTypes.ability));
            ValueMaxProperty =
                Convert.ToInt32(Pers.ExpToLevel(LevelProperty + 1, RpgItemsTypes.ability));
        }

        /// <summary>
        /// The UpdateAbilValue
        /// </summary>
        public void UpdateAbilValue()
        {
            OnPropertyChanged(nameof(LevelProperty));
        }

        ///// <summary>
        ///// The SetToChaRelays
        ///// </summary>
        //public void SetToChaRelays()
        //{
        //    ToChaRelaysProperty =
        //        StaticMetods.PersProperty.Characteristics.SelectMany(
        //            q => q.NeedAbilitisProperty.Where(n => n.AbilProperty == this)).Sum(n => n.RightK);
        //}
        /// <summary>
        /// The UpdateToolTip
        /// </summary>
        public override void UpdateToolTip()
        {
            OnPropertyChanged(nameof(ToolTip));
        }

        /// <summary>
        /// Понижает значение навыка с учетом формулы.
        /// </summary>
        /// <param name="chVal"></param>
        /// <param name="curVal"></param>
        public void ValueDecrease(double chVal, double curVal)
        {
            while (true)
            {
                double prevLvl = GetPrevLvl(curVal);

                if (prevLvl == curVal)
                    prevLvl = prevLvl - 1;

                double lastForPrev = GetLastForPrev(prevLvl, curVal);

                double change = chVal * Task.AbIncreaseFormula(this, prevLvl);
                if (change <= lastForPrev)
                {
                    curVal -= change;
                    break;
                }

                var iterationChange = lastForPrev / Task.AbIncreaseFormula(this, prevLvl);
                chVal -= iterationChange;
                curVal -= lastForPrev;

                if (curVal <= 0)
                {
                    curVal = 0;
                    break;
                }
            }

            ValueProperty = curVal;
        }

        /// <summary>
        /// Повышает значение навыка с учетом формулы.
        /// </summary>
        /// <param name="chVal"></param>
        /// <param name="curVal"></param>
        public void ValueIncrease(double chVal, double curVal)
        {
            if (double.IsNaN(chVal) || double.IsInfinity(chVal)
                || double.IsNaN(curVal) || double.IsInfinity(curVal))
                return;

            while (true)
            {
                double nextLvl = GetNextLvl(curVal);
                double lastForNext = GetLastForNext(nextLvl, curVal);

                double change = chVal * Task.AbIncreaseFormula(this, curVal);
                if (change <= lastForNext)
                {
                    curVal += change;

                    break;
                }

                var iterationChange = lastForNext / Task.AbIncreaseFormula(this, curVal);
                chVal -= iterationChange;
                curVal += lastForNext;
            }

            ValueProperty = curVal;
        }

        /// <summary>
        /// The GetDefoultPic
        /// </summary>
        /// <returns>The <see cref="BitmapImage"/></returns>
        protected override BitmapImage GetDefoultPic()
        {
            return DefoultPicsAndImages.DefoultAbilPic;
        }

        /// <summary>
        /// The SetLevel
        /// </summary>
        /// <param name="value">The <see cref="int"/></param>
        protected override void SetLevel(int value)
        {
            if (level == value)
            {
                return;
            }

            level = value;

            OnPropertyChanged(nameof(LevelProperty));

            SetMinMaxValue();

            OnPropertyChanged(nameof(CurRang));
        }

        /// <summary>
        /// The ActivateAbility
        /// </summary>
        /// <param name="selAb">The <see cref="AbilitiModel"/></param>
        /// <returns>The <see cref="int"/></returns>
        private static int ActivateAbility(AbilitiModel selAb)
        {
            return 1;
        }

        /// <summary>
        /// The AddCopyToQwests
        /// </summary>
        /// <param name="tsk">The <see cref="Task"/></param>
        /// <param name="nTsk">The <see cref="Task"/></param>
        private static void AddCopyToQwests(Task tsk, Task nTsk)
        {
            foreach (
                var aim in
                    StaticMetods.PersProperty.Aims
                        .Where(n => n.NeedsTasks.Any(q => q.TaskProperty == tsk)))
            {
                var firstNeed = aim.NeedsTasks.First(n => n.TaskProperty == tsk);
                var nNeed = QwestsViewModel.GetDefoultNeedTask(nTsk);
                nNeed.AsLinkProperty = firstNeed.AsLinkProperty;
                aim.NeedsTasks.Add(nNeed);
            }
        }

        /// <summary>
        /// Характеристики на которые влияет навык
        /// </summary>
        /// <param name="persProperty"></param>
        /// <param name="ab"></param>
        /// <returns></returns>
        private static List<Characteristic> ChasOfAb(Pers persProperty, AbilitiModel ab)
        {
            return persProperty.Characteristics.Where(n => n.NeedAbilitisProperty.Any(q => q.AbilProperty == ab && q.KoeficientProperty > 0)).ToList();
        }

        /// <summary>
        /// Проверяем макс и мин для скиллов
        /// </summary>
        /// <param name="abilitiModel"></param>
        /// <param name="v">Значение</param>
        private static void compareAbMinMax(AbilitiModel abilitiModel, ref double v)
        {
            //// Не может быть больше максимального значения
            //if (v > abilitiModel.MaxValue)
            //{
            //    v = abilitiModel.MaxValue;
            //    return;
            //}

            //// Не может быть больше чем макс - бонусы за невыполненные квесты
            //var sumOfNotDoneAims = 100.0 - abilitiModel.NeedAims.Where(n => !n.AimProperty.IsDoneProperty).Sum(n => n.KRel * 20.0);
            //if (v > sumOfNotDoneAims)
            //{
            //    v = sumOfNotDoneAims;
            //}

            //// Не может быть меньше чем бонусы за выполненные квесты
            //var sumOfDoneAims = abilitiModel.NeedAims.Where(n => n.AimProperty.IsDoneProperty).Sum(n => n.KRel * 20.0);
            //if (v < sumOfDoneAims)
            //{
            //    v = sumOfDoneAims;
            //}

            if (v < 0)
            {
                v = 0;
            }

            if (v > StaticMetods.MaxAbLevel * 20.0)
            {
                v = StaticMetods.MaxAbLevel * 20.0;
            }
        }

        /// <summary>
        /// Копировать значения задачи
        /// </summary>
        /// <param name="tsk">С которой копируем</param>
        /// <param name="nTsk">В которую копируем</param>
        private static void CopyTaskValues(Task tsk, Task nTsk)
        {
            nTsk.NameOfProperty = tsk.NameOfProperty;
            nTsk.Recurrense.TypeInterval = tsk.Recurrense.TypeInterval;
            nTsk.Recurrense.Interval = tsk.Recurrense.Interval;

            nTsk.BeginDateProperty = tsk.BeginDateProperty;
            nTsk.DateOfDone = tsk.DateOfDone;

            // Дни недели
            foreach (var daysOfWeekRepeat in tsk.DaysOfWeekRepeats)
            {
                nTsk.DaysOfWeekRepeats.First(n => n.Day == daysOfWeekRepeat.Day).CheckedProperty =
                    daysOfWeekRepeat.CheckedProperty;
            }

            // Подзадачи
            //foreach (var subTask in tsk.SubTasks)
            //{
            //    nTsk.SubTasks.Add(new SubTask {Tittle = subTask.Tittle});
            //}

            nTsk.ImageProperty = tsk.ImageProperty;
            nTsk.NameOfProperty = tsk.NameOfProperty;
            nTsk.CounterMaxValueProperty = tsk.CounterMaxValueProperty;
            nTsk.TimeMustProperty = tsk.TimeMustProperty;

            // Целевые значения
            nTsk.AimCounterMax = tsk.AimCounterMax;
            nTsk.AimTimerMax = tsk.AimTimerMax;
            nTsk.AimMesure = tsk.AimMesure;

            // Переносим на следующий день дату
            nTsk.BeginDateProperty = nTsk.BeginDateProperty > MainViewModel.selectedTime
                ? MainViewModel.selectedTime.AddDays(1)
                : MainViewModel.selectedTime;

            // Ссылка на скилл
            nTsk.BaseOfSkill = tsk;
        }

        /// <summary>
        /// The GetCountedValue
        /// </summary>
        /// <param name="valProp">The <see cref="double"/></param>
        /// <returns>The <see cref="double"/></returns>
        private static double GetCountedValue(double valProp)
        {
            var val = 1 + valProp / 20.0;
            var countedValue = (Math.Pow(2 * val - 1, 2) - 1) * 0.25;
            return countedValue;
        }

        ///// <summary>
        ///// The DoneThisAbLevelTasks
        ///// </summary>
        ///// <param name="selAb">The <see cref="AbilitiModel"/></param>
        //private static void DoneThisAbLevelTasks(AbilitiModel selAb)
        //{
        //    foreach (
        //        var needTaskse in
        //            selAb.NeedTasks.Where(n => n.AsLinkProperty == false)
        //                .Where(n => n.LevelProperty < selAb.PayedLevelProperty)
        //        )
        //    {
        //        needTaskse.TaskProperty.IsDelProperty = true;
        //    }
        //}
        /// <summary>
        /// The GetHardCost
        /// </summary>
        /// <param name="hardnessProperty">The <see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        private static int GetHardCost(int hardnessProperty)
        {
            int baseCost;
            switch (hardnessProperty)
            {
                case -1:
                    baseCost = 1;
                    break;

                case 0:
                    baseCost = 2;
                    break;

                case 1:
                    baseCost = AbMaxLevel;
                    break;

                case 2:
                    baseCost = 6;
                    break;

                default:
                    baseCost = 0;
                    break;
            }
            return baseCost;
        }

        private static double GetNextLvl(double curVal)
        {
            double thisLvl = Math.Floor(curVal);
            double nextLvl = thisLvl + 1;
            return nextLvl;
        }

        //        if (selectedAimProperty.IsDoneProperty == false)
        //        {
        //            QwestsViewModel.GetQwestChanges(persProperty, selectedAimProperty, true);
        //        }
        //    }
        //}
        /// <summary>
        /// The LastDay
        /// </summary>
        /// <param name="firstDay">The <see cref="double"/></param>
        /// <param name="maxAbLevelMoreThenFirst">The <see cref="double"/></param>
        /// <returns>The <see cref="double"/></returns>
        private static double LastDay(double firstDay, double maxAbLevelMoreThenFirst)
        {
            if (firstDay == 0)
            {
                return 1;
            }

            return firstDay * maxAbLevelMoreThenFirst;
        }

        private static double minRelChaVal(Pers persProperty, AbilitiModel ab)
        {
            if (ab == null)
            {
                return double.MaxValue;
            }

            var ch1 = ChasOfAb(persProperty, ab);
            var minCha1 = ch1.Any() ? ch1.Min(n => n.ValueProperty) : double.MaxValue;
            return minCha1;
        }

        /// <summary>
        /// Показать подбадривающее окно покупки скилла
        /// </summary>
        /// <param name="selAb"></param>
        private static void ShowAbUpDownMessege(AbilitiModel selAb, bool isUp = false)
        {
            isUp = true;
            var nowrang = selAb.RangName;
            var headerText = !isUp
                ? $"Навык \"{selAb.NameOfProperty}\" активирован!!!"
                : $"Навык \"{selAb.NameOfProperty}\" прокачан до уровня \"{nowrang}\"!!!";
            AddOrEditAbilityViewModel.showAbLevelChange(headerText, selAb, Brushes.Green);
        }

        //            if (tsk2.BaseOfSkill == tsk1)
        //            {
        //                foreach (var aim in persProperty.Aims)
        //                {
        //                    foreach (var spell in aim.Spells.Where(n => n == tsk1).ToList())
        //                    {
        //                        aim.Spells.Remove(spell);
        //                        aim.Spells.Add(tsk2);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        /// <summary>
        /// Добавить новый уровень когда поднято значение
        /// </summary>
        /// <param name="persLevelProperty"></param>
        private void AddLevelsWhenUp(int persLevelProperty)
        {
            LevelsWhenUp.Add(persLevelProperty);
            RecountValue();
        }

        // foreach (var thisL in thisLevSkills.ToList()) { var tsk1 = beforeL.TaskProperty; var tsk2
        // = thisL.TaskProperty;
        /// <summary>
        /// Проверка на халявность
        /// </summary>
        /// <param name="notAllowReqwirements"></param>
        private void CheckIsHalyava(string notAllowReqwirements)
        {
            if (IsPayedProperty)
            {
                if (string.IsNullOrWhiteSpace(notAllowReqwirements))
                {
                    var thisLev =
                        NeedTasks.Where(n => n.LevelProperty == CellValue)
                            .Select(
                                n =>
                                    $"{n.TaskProperty.NameOfProperty}{n.TaskProperty.TimeMustProperty}{n.TaskProperty.Mesure}{n.TaskProperty.SubTasksString}");
                    var nextLev =
                        NeedTasks.Where(n => n.LevelProperty == CellValue + 1)
                            .Select(
                                n =>
                                    $"{n.TaskProperty.NameOfProperty}{n.TaskProperty.TimeMustProperty}{n.TaskProperty.Mesure}{n.TaskProperty.SubTasksString}");
                    var a = thisLev.Aggregate("", (x, y) => x.ToString() + y.ToString());
                    var b = nextLev.Aggregate("", (x, y) => x.ToString() + y.ToString());
                    if (a == b)
                    {
                        IsHalyava = 2;
                    }
                    else
                    {
                        IsHalyava = 0;
                    }
                }
                else
                {
                    IsHalyava = 0;
                }
            }
            else
            {
                IsHalyava = 0;
            }
        }

        // foreach (var beforeL in beforeLevSkills.ToList()) { var thisLevSkills = selAb.NeedTasks
        // .Where(n => n.LevelProperty == selAb.PayedLevelProperty - 1);
        /// <summary>
        /// The ComplNeedsForLevel
        /// </summary>
        /// <param name="allNeeds">The <see cref="List{ComplecsNeed}"/></param>
        /// <param name="lev">The <see cref="int"/></param>
        /// <returns>The <see cref="IEnumerable{ComplecsNeed}"/></returns>
        private IEnumerable<ComplecsNeed> ComplNeedsForLevel(List<ComplecsNeed> allNeeds, int lev)
        {
            var all = new List<ComplecsNeed>();

            var needs = allNeeds.Where(n => n.LevelProperty == lev).ToList();

            var taskNeeds = needs.Where(n => n.NeedTask != null).ToList();
            var qwestNeeds = needs.Where(n => n.NeedQwest != null).ToList();

            all.AddRange(taskNeeds);

            foreach (var complecsNeed in qwestNeeds)
            {
                all.Add(complecsNeed);

                var links =
                    complecsNeed.NeedQwest.AimProperty.AllCompositeQwests.Except(
                        qwestNeeds.Select(n => n.NeedQwest.AimProperty));

                foreach (var link in links)
                {
                    var needQwest = new CompositeAims { AimProperty = link, LevelProperty = lev, AsLinkProperty = true };
                    var cn = new ComplecsNeed
                    {
                        NameProperty = needQwest.AimProperty.NameOfProperty,
                        NeedTask = null,
                        NeedQwest = needQwest
                    };

                    var complecsNeedLevel = cn.LevelProperty;

                    cn.IsEnabledProperty = complecsNeedLevel < CellValue;

                    cn.GroupName = GetComplecsNeedGroupName(complecsNeedLevel);
                    cn.BackgroundBrush = complecsNeedLevel < CellValue - 1
                        ? Brushes.Yellow
                        : Brushes.White;

                    all.Add(cn);
                }
            }

            // Квесты в которых скилл является скиллом
            var skills =
                StaticMetods.PersProperty.Aims.Where(n => !n.IsDoneProperty && n.IsActiveProperty)
                    .Where(n => n.Skills.Intersect(SkillsActive).Any());
            foreach (var link in skills)
            {
                var needQwest = new CompositeAims { AimProperty = link, LevelProperty = lev, AsLinkProperty = true };
                var cn = new ComplecsNeed
                {
                    NameProperty = needQwest.AimProperty.NameOfProperty,
                    NeedTask = null,
                    NeedQwest = needQwest
                };

                var complecsNeedLevel = cn.LevelProperty;

                cn.IsEnabledProperty = complecsNeedLevel < CellValue;

                cn.GroupName = GetComplecsNeedGroupName(complecsNeedLevel);
                cn.BackgroundBrush = complecsNeedLevel < CellValue - 1
                    ? Brushes.Yellow
                    : Brushes.White;

                all.Add(cn);
            }

            return all;
        }

        ///// <summary>
        ///// The CopyQwestsToNextLevel
        ///// </summary>
        ///// <param name="selAb">The <see cref="AbilitiModel"/></param>
        //private static void CopyQwestsToNextLevel(AbilitiModel selAb)
        //{
        //    var thisAbLevQwests =
        //        selAb.NeedAims.Where(n => !n.AimProperty.IsDoneProperty)
        //            .Where(n => n.LevelProperty == selAb.PayedLevelProperty - 1)
        //            .Where(n => n.KRel == 0)
        //            .ToList();
        //    var nextAbLevQwests =
        //        selAb.NeedAims.Where(n => !n.AimProperty.IsDoneProperty)
        //            .Where(n => n.LevelProperty == selAb.PayedLevelProperty)
        //            .Where(n => n.KRel == 0)
        //            .ToList();
        //    var qw = thisAbLevQwests.Except(nextAbLevQwests, new NeedQwestsNameComparer()).ToList();
        //    foreach (var nQwest in qw)
        //    {
        //        nQwest.LevelProperty = selAb.PayedLevelProperty;
        //    }
        //}
        ///// <summary>
        ///// The DoneThisAbLevelQwests
        ///// </summary>
        ///// <param name="selAb">The <see cref="AbilitiModel"/></param>
        ///// <param name="persProperty">The <see cref="Pers"/></param>
        //private static void DoneThisAbLevelQwests(AbilitiModel selAb, Pers persProperty)
        //{
        //    foreach (
        //        var compositeAimse in
        //            selAb.NeedAims.Where(n => n.AsLinkProperty == false)
        //                .Where(n => n.LevelProperty < selAb.PayedLevelProperty))
        //    {
        //        var selectedAimProperty = compositeAimse.AimProperty;
        ///// <summary>
        ///// The RecountQwestsSkills
        ///// </summary>
        ///// <param name="selAb">The <see cref="AbilitiModel"/></param>
        ///// <param name="persProperty">The <see cref="Pers"/></param>
        //private static void RecountQwestsSkills(AbilitiModel selAb, Pers persProperty)
        //{
        //    var beforeLevSkills = selAb.NeedTasks
        //        .Where(n => n.LevelProperty == selAb.PayedLevelProperty - 2);
        /// <summary>
        /// Получаем все следующие для скиллов
        /// </summary>
        /// <param name="needTaskse"></param>
        /// <param name="nexxts"></param>
        private void GetAllNexxts(NeedTasks needTaskse, List<NeedTasks> nexxts)
        {
            nexxts.Add(needTaskse);
            var nexts = needTaskse.TaskProperty.NextActions;
            foreach (var nextAction in nexts)
            {
                var need = NeedTasks.FirstOrDefault(n => n.TaskProperty == nextAction);
                if (need == null) continue;
                GetAllNexxts(need, nexxts);
            }
        }

        /// <summary>
        /// The GetComplecsNeedGroupName
        /// </summary>
        /// <param name="complecsNeedLevel">The <see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string GetComplecsNeedGroupName(int complecsNeedLevel)
        {
            var rang = Rangs.FirstOrDefault(n => n.LevelRang == complecsNeedLevel);
            var nameOfRang = rang != null ? rang.NameOfRang : string.Empty;
            string grName = $"{complecsNeedLevel}. \"{nameOfRang}\" {HardnessPercentage(complecsNeedLevel)}";
            return grName;
        }

        /// <summary>
        /// Расчет комплексных требований
        /// </summary>
        /// <returns></returns>
        private List<ComplecsNeed> GetComplexNeeds()
        {
            var tsk = (from needTaskse in NeedTasks
                       orderby needTaskse.TaskProperty
                       select new ComplecsNeed
                       {
                           NameProperty = needTaskse.TaskProperty.NameOfProperty,
                           NeedTask = needTaskse,
                           NeedQwest = null
                       }).ToList();

            var all = new List<ComplecsNeed>();
            all.AddRange(
                tsk.OrderBy(n => n.NeedTask.TaskProperty));

            foreach (var complecsNeed in all)
            {
                if (complecsNeed.NeedQwest != null)
                {
                    complecsNeed.BackgroundBrush = Brushes.White;
                }
                else
                {
                    if (complecsNeed.NeedTask != null)
                    {
                        var brr = new BrushConverter().ConvertFromString("#e4e4e4") as SolidColorBrush;
                        complecsNeed.BackgroundBrush = complecsNeed.NeedTask.TaskProperty.ParrentTasks.Count <= 0
                            ? brr
                            : Brushes.White;
                    }
                }
            }
            var orderedComplexNeeds = all;
            return orderedComplexNeeds.ToList();
        }

        /// <summary>
        /// The GetGroupedComplexNeeds
        /// </summary>
        /// <returns>The <see cref="List{GroupedComlexNeed}"/></returns>
        private List<GroupedComlexNeed> GetGroupedComplexNeeds()
        {
            var gn = new List<GroupedComlexNeed>();

            var allNeeds = ComplecsNeeds.ToList();

            for (var i = MaxLevelProperty - 1; i > 0; i--)
            {
                var name = GetComplecsNeedGroupName(i);
                var complecsNeeds = ComplNeedsForLevel(allNeeds, i);
                gn.Add(new GroupedComlexNeed(i, name, complecsNeeds.ToList()));
            }

            return gn;
        }

        private double GetLastForNext(double nextLvl, double curVal)
        {
            return nextLvl - curVal;
        }

        private double GetLastForPrev(double prevLvl, double curVal)
        {
            return curVal - prevLvl;
        }

        private Dictionary<CCKey, List<ComplecsNeed>> GetNewCCNeeds(List<ComplecsNeed> needs)
        {
            var newCcNeeds = new Dictionary<CCKey, List<ComplecsNeed>>();

            var abLevel = Convert.ToInt32(CellValue);

            List<ComplecsNeed> complecsNeeds;
            string levDescr = string.Empty;
            var hl = default(string);
            int index = default(int);

            var isSetViz0 = StaticMetods.Locator?.AddOrEditAbilityVM?.IsSetViz;

            if (isSetViz0 != null && isSetViz0.Value)
            {
                // Если активирован расширенный режим
                complecsNeeds = needs;
                hl = HList[StaticMetods.MaxAbLevel];
                index = StaticMetods.MaxAbLevel;
            }
            else
            {
                // Если обычный режим
                hl = HList[abLevel];
                levDescr = hl.ToString();
                complecsNeeds = needs.Where(n => abLevel >= n.LevelProperty && abLevel <= n.ToLevelProperty).ToList();
                index = abLevel;
            }

            var curLevDescr = StaticMetods.PersProperty.PersSettings.AbRangs[abLevel].Name;
            var value = complecsNeeds.OrderBy(n => n.NeedTask).ToList();
            newCcNeeds.Add(new CCKey(hl, index, curLevDescr, levDescr), value);

            return newCcNeeds;
        }

        private double GetPrevLvl(double curVal)
        {
            double prev = Math.Floor(curVal);
            return prev;
        }

        /// <summary>
        /// The HardnessPercentage
        /// </summary>
        /// <param name="complecsNeedLevel">The <see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string HardnessPercentage(int complecsNeedLevel)
        {
            double i = complecsNeedLevel + 1;
            var s = "~";

            var perc = ValPerc(i) * 100.0;

            var vall = Math.Round(perc, 0);

            var d = HardnessProperty + 1.0;

            return
                $"{s} {Convert.ToInt32(vall)} %, {i * StaticMetods.Config.AbOneLevelDays} дн.";
        }

        /// <summary>
        /// Обновить стоимость скилла
        /// </summary>
        private void RefreshCost()
        {
            OnPropertyChanged(nameof(CostProperty));
        }

        /// <summary>
        /// The SetAbFromAb
        /// </summary>
        /// <param name="abiliti">The <see cref="AbilitiModel"/></param>
        private void SetAbFromAb(AbilitiModel abiliti)
        {
            NameOfProperty = abiliti.NameOfProperty;
            DescriptionProperty = abiliti.DescriptionProperty;
            ImageProperty = abiliti.ImageProperty;
            Type = abiliti.Type;
        }

        ///// <summary>
        ///// Обновить значения связанных характеристик
        ///// </summary>
        //private void UpdateChaValues()
        //{
        //    foreach (var cha in
        //        StaticMetods.PersProperty.Characteristics.Where(
        //            n => n.NeedAbilitisProperty.Any(q => q.AbilProperty == this && q.KoeficientProperty != 0)))
        //    {
        //        cha.ValueProperty = cha.GetChaValue();
        //    }
        //}

        /// <summary>
        /// The ValPerc
        /// </summary>
        /// <param name="lev">The <see cref="double"/></param>
        /// <returns>The <see cref="double"/></returns>
        private double ValPerc(double lev)
        {
            double valMax = (MaxLevelProperty + 1) * 100;
            var valThis = lev * 100;

            if (valMax == 0)
            {
                return 0;
            }

            var perc = valThis / valMax;
            return perc;
        }
    }

    /// <summary>
    /// Defines the <see cref="CCKey"/>
    /// </summary>
    public class CCKey : INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the _hardnessDescr
        /// </summary>
        private string _hardnessDescr;

        /// <summary>
        /// Defines the _name
        /// </summary>
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="CCKey"/> class.
        /// </summary>
        /// <param name="name">The <see cref="string"/></param>
        /// <param name="ind">The <see cref="int"/></param>
        /// <param name="curLevDescr">The <see cref="string"/></param>
        public CCKey(string name, int ind, string curLevDescr, string hardnessDescr)
        {
            Name = name;
            index = ind;
            HardnessDescr = hardnessDescr;
            if (ind == 1)
            {
                IsFirstLev = true;
            }
            if (ind == StaticMetods.MaxAbLevel)
            {
                IsCurOrLastLev = true;
                IsLastLev = true;
            }
            else
            {
                IsCurOrLastLev = curLevDescr == name;
            }
            IsCurLev = curLevDescr == name;
        }

        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public bool AntyFL => !IsFirstLev;

        public bool AntyLL => !IsLastLev;

        /// <summary>
        /// Цвет
        /// </summary>
        public Color BacBrush
        {
            get
            {
                return Colors.LightGray;
                ////var rangse = StaticMetods.PersProperty.PersSettings.AbRangs;
                //double max = StaticMetods.PersProperty.PersSettings.MaxLevOfAbForProg;

                //if (index/max <= 1.0/3.0)
                //{
                //    return Colors.LightGray;
                //}
                //else if (index / max > 2.0 / 3.0)
                //{
                //    return Colors.SteelBlue;
                //}
                //else
                //{
                //    return Colors.Yellow;
                //}

                //var ind = index;
                //if (Name == rangse[1].Name)
                //{
                //    return Colors.LightGray;
                //}
                //if (Name == rangse[2].Name)
                //{
                //    return Colors.Yellow;
                //}
                //if (Name == rangse[3].Name)
                //{
                //    return Colors.YellowGreen;
                //}
                //if (Name == rangse[4].Name)
                //{
                //    return Colors.Green;
                //}
                //if (Name == rangse[5].Name)
                //{
                //    return Colors.SteelBlue;
                //}

                //return Colors.White;
            }
        }

        /// <summary>
        /// Gets or sets the HardnessDescr
        /// </summary>
        public string HardnessDescr
        {
            get
            {
                return _hardnessDescr;
            }
            set
            {
                if (value == _hardnessDescr) return;
                _hardnessDescr = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Индекс требования
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsCurLev
        /// </summary>
        public bool IsCurLev { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsCurOrLastLev
        /// </summary>
        public bool IsCurOrLastLev { get; set; }

        /// <summary>
        /// Первый уровень?
        /// </summary>
        public bool IsFirstLev { get; set; }

        /// <summary>
        /// Последний уровень?
        /// </summary>
        public bool IsLastLev { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BacBrush));
            }
        }

        /// <summary>
        /// The OnPropertyChanged
        /// </summary>
        /// <param name="propertyName">The <see cref="string"/></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Defines the <see cref="NeedQwestsNameComparer"/>
    /// </summary>
    public class NeedQwestsNameComparer : IEqualityComparer<CompositeAims>
    {
        /// <summary>
        /// The Equals
        /// </summary>
        /// <param name="x">The <see cref="CompositeAims"/></param>
        /// <param name="y">The <see cref="CompositeAims"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Equals(CompositeAims x, CompositeAims y)
        {
            if (x.AimProperty.NameOfProperty == y.AimProperty.NameOfProperty)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The GetHashCode
        /// </summary>
        /// <param name="obj">The <see cref="CompositeAims"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int GetHashCode(CompositeAims obj)
        {
            return obj.AimProperty.NameOfProperty.GetHashCode();
        }
    }

    [Serializable]
    public class NeedsForLevel
    {
        public NeedsForLevel()
        {
            NeedCharacts = new ObservableCollection<NeedCharact>();
            NeedAbilities = new ObservableCollection<NeedAbility>();
            NeedAims = new ObservableCollection<Aim>();
        }

        public int Level { get; set; }

        public ObservableCollection<NeedAbility> NeedAbilities { get; set; }

        public ObservableCollection<Aim> NeedAims { get; set; }

        public ObservableCollection<NeedCharact> NeedCharacts { get; set; }

        public int PersLevel { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="NeedTaskNameComparer"/>
    /// </summary>
    public class NeedTaskNameComparer : IEqualityComparer<NeedTasks>
    {
        /// <summary>
        /// The Equals
        /// </summary>
        /// <param name="x">The <see cref="NeedTasks"/></param>
        /// <param name="y">The <see cref="NeedTasks"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Equals(NeedTasks x, NeedTasks y)
        {
            if (x.TaskProperty.NameOfProperty == y.TaskProperty.NameOfProperty)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The GetHashCode
        /// </summary>
        /// <param name="obj">The <see cref="NeedTasks"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int GetHashCode(NeedTasks obj)
        {
            return obj.TaskProperty.NameOfProperty.GetHashCode();
        }
    }

    /// <summary>
    /// Требования рангов
    /// </summary>
    [Serializable]
    public class RangseNeeds
    {
        public RangseNeeds()
        {
            ReqwireAims = new ObservableCollection<Aim>();
            AbilityNeedAbilities = new ObservableCollection<NeedAbility>();
            AbilityNeedCharacts = new ObservableCollection<NeedCharact>();
            PersLevel = 0;
        }

        /// <summary>
        /// Требование других скиллов для того, чтобы прокачать скилл до следующего уровня
        /// </summary>
        public ObservableCollection<NeedAbility> AbilityNeedAbilities { get; set; }

        /// <summary>
        /// Требования характеристик для того, чтобы прокачать или открыть скилл
        /// </summary>
        public ObservableCollection<NeedCharact> AbilityNeedCharacts { get; set; }

        /// <summary>
        /// Значение скила для которого эти требования актуальны
        /// </summary>
        public int CellValue { get; set; }

        /// <summary>
        /// Общий уровень персонажа для того, чтобы прокачать или открыть скилл
        /// </summary>
        public int PersLevel { get; set; }

        /// <summary>
        /// Требования по квестам, которые нужно выполнить чтобы прокачать скилл
        /// </summary>
        public ObservableCollection<Aim> ReqwireAims { get; set; }
    }
}
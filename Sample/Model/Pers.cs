using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Properties;
using Sample.View;
using Sample.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Sample.Model
{
    [Serializable]
    public class ChaLevAndValue
    {
        public ChaLevAndValue(int lev, string guid, double val)
        {
            Lev = lev;
            Guid = guid;
            Val = val;
        }

        public string Guid { get; set; }

        public int Lev { get; set; }

        public double Val { get; set; }
    }

    [Serializable]
    public class ImagesAndPositions
    {
        public ImagesAndPositions()
        {
            Images = new List<RIGImage>();
            Position = 0;
        }

        /// <summary>
        /// Изображения
        /// </summary>
        public List<RIGImage> Images { get; set; }

        public int Level { get; set; }

        /// <summary>
        /// Позиция
        /// </summary>
        public int Position { get; set; }
    }

    /// <summary>
    /// The pers.
    /// </summary>
    [Serializable]
    public class Pers : LevelableRPGItem
    {

        /// <summary>
        /// У перса 50 уровней.
        /// </summary>
        public bool BalanceIs50Levels
        {
            get
            {
                if (PersSettings.Is10AbLevels)
                    return false;
                else if (PersSettings.Is5_5_50)
                    return true;
                else if (PersSettings.IsFUDGE)
                    return false;

                return false;
            }
        }

        /// <summary>
        /// Сложность на первом уровне.
        /// </summary>
        public int BalanceForFirstLevel
        {
            get
            {
                if (PersSettings.Is10AbLevels)
                    return 20; // 2 дн.
                else if (PersSettings.Is5_5_50)
                    return 50; // 5 дн.
                else if (PersSettings.IsFUDGE)
                    return 30; // 3 дн.

                return 3; // 2 дн.
            }
        }

        /// <summary>
        /// Сложность на первом уровне.
        /// </summary>
        public int BalanceForLastLevel
        {
            get
            {
                if (PersSettings.Is10AbLevels)
                    return 200; // 20 дн.
                else if (PersSettings.Is5_5_50)
                    return 500; // 50 дн.
                else if (PersSettings.IsFUDGE)
                    return 150; // 15 дн.

                return 15; // 20 дн.
            }
        }

        /// <summary>
        /// Уровней навыков в балансе.
        /// </summary>
        public int BalanceLevels
        {
            get
            {
                if (PersSettings.Is10AbLevels)
                    return 10;
                else if (PersSettings.Is5_5_50)
                    return 5;
                else if (PersSettings.IsFUDGE)
                    return 5;

                return 100;
            }
        }



        public static double ExpOneLev = 1000.0;

        /// <summary>
        /// Стоимость открытия навыка
        /// </summary>
        public static int OpenAbCost = 1;

        public ChaLevAndValues _chaLevAndValues;

        public bool focBeforePlanning;

        /// <summary>
        /// Золото.
        /// </summary>
        public int gold;

        public bool isCheckMinLev = false;

        private int _bigHpBottles;

        private List<string> _bookOfSuccess;

        private double _expBuff;

        private int _hitPoints;

        private double _hp;

        private bool _isHideHud;

        private bool _isMapsEnabled;

        private bool _isParetto;

        private bool _isPlaningMode;

        private bool _isPlanningModeMain;

        private bool _isQwestsEnabled;

        private bool _isRewardsEnabled;

        private bool _isSetViz;

        private bool _isSleepForNewLevel;

        private bool _isSortByBalance;

        private bool _isSortByPrioryty;

        private int _maxGettedLevel;

        private int _middleHpBottles;

        private RandomRevard _randomRevard = new RandomRevard();

        private RandomeImageGenerator _rig;

        private RandomeImageGenerator2 _rig2;

        private int _smallHpBottles;

        private int _stage;

        private ViewsModel _viewForDefoult;

        private ObservableCollection<ViewsModel> _views;

        private string dateLastUseProgram;

        /// <summary>
        /// Дата и время последнего сохранения в проге
        /// </summary>
        private string dateOfLastSave;

        /// <summary>
        /// Изображение.
        /// </summary>
        private byte[] image;

        /// <summary>
        /// Картинка в зависимости от звания?.
        /// </summary>
        private bool imageFromeRangs;

        /// <summary>
        /// Активированно автосохранение?.
        /// </summary>
        private bool isAutosave;

        /// <summary>
        /// Последняя дата использования программы (строка).
        /// </summary>
        private string lastDateOfUse;

        private List<PersLevelsValues> levelsValueses;

        private ObservableCollection<ViewsModel> NewViews;

        /// <summary>
        /// Купленные очки скиллов.
        /// </summary>
        private int payedAbPoints;

        /// <summary>
        /// Опыт персонажа.
        /// </summary>
        private int persExp;

        /// <summary>
        /// Уровень персонажа.
        /// </summary>
        private int persLevel;

        /// <summary>
        /// Настройки программы
        /// </summary>
        private SettingsPers persSettings = new SettingsPers();

        /// <summary>
        /// Очки скиллов + если обычные закончатся.
        /// </summary>
        private int plusAbPoints;

        /// <summary>
        /// Плюс к опыту за выполненные задачи.
        /// </summary>
        private double plusExpFromTasks;

        private ObservableCollection<Rangs> rangs;

        /// <summary>
        /// Выбранный скилл.
        /// </summary>
        private AbilitiModel sellectedAbility;

        /// <summary>
        /// Выбранная в данный момент цель (квест).
        /// </summary>
        private Aim sellectedAim;

        /// <summary>
        /// Показывать только задачи, завершающиеся сегодня.
        /// </summary>
        private bool showOnlyTodayTasks;

        public Pers(RandomeImageGenerator rig)
        {
            RIG = rig;
        }

        /// <summary>
        /// Активировать навыков на первом уровне
        /// </summary>
        public static double AbFirstLev => PointsPerLevel;

        /// <summary>
        /// Картинка опыта
        /// </summary>
        public static byte[] ExpImageProperty
        {
            get
            {
                return StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "опыт.png"));
            }
        }

        public static double ExpK => 100.0 / TasksDoForFirstLevel;

        /// <summary>
        /// Sets and gets Картинка золота. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public static byte[] GoldImageProperty
        {
            get
            {
                return StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "gold.png"));
            }
        }

        /// <summary>
        /// Картинка опыта
        /// </summary>
        public static byte[] LevelImageProperty
        {
            get
            {
                return StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "levelUp.png"));
            }
        }

        /// <summary>
        /// Дается очков навыков за уровень
        /// </summary>
        public static double PointsPerLevel => StaticMetods.GetAutoRecauntAbPointsPerLev();

        /// <summary>
        /// Дней на достижение первого уровня
        /// </summary>
        public static double TasksDoForFirstLevel => Math.Floor(PointsPerLevel);

        /// <summary>
        /// Gets or Sets Скиллы
        /// </summary>
        public ObservableCollection<AbilitiModel> Abilitis { get; set; }

        /// <summary>
        /// Число очков скиллов персонажа
        /// </summary>
        public int AbilitisPoints
        {
            get { return GetAbPoints(PersLevelProperty); }
        }

        /// <summary>
        /// Цели персонажа
        /// </summary>
        public ObservableCollection<Aim> Aims { get; set; }

        /// <summary>
        /// </summary>
        public double Balance
        {
            get
            {
                var maxK = Characteristics.Count;
                if (maxK == 0)
                {
                    return 0;
                }

                var k = Characteristics.Sum(n => n.KExpRelayProperty);

                var balance = k / maxK;

                return balance;
            }
        }

        /// <summary>
        /// Количество больших бутылочек здоровья
        /// </summary>
        public int BigHpBottles
        {
            get
            {
                return _bigHpBottles;
            }
            set
            {
                if (value == _bigHpBottles) return;
                _bigHpBottles = value;
                OnPropertyChanged(nameof(BigHpBottles));
            }
        }

        public List<string> BookOfSuccess
        {
            get => _bookOfSuccess ?? (_bookOfSuccess = new List<string>());
            set => _bookOfSuccess = value;
        }

        public ChaLevAndValues ChaLevAndValues
        {
            get
            {
                if (_chaLevAndValues == null)
                {
                    _chaLevAndValues = new ChaLevAndValues(this);
                }
                return _chaLevAndValues;
            }
        }

        /// <summary>
        /// Характер персонажа
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// Gets or sets Характеристики персонажа
        /// </summary>
        public ObservableCollection<Characteristic> Characteristics { get; set; }

        /// <summary>
        /// Класс персонажа
        /// </summary>
        public Class Class1 { get; set; }

        /// <summary>
        /// Скрывать бОльшие уровни
        /// </summary>
        public bool closeMoreLev { get; set; }

        /// <summary>
        /// Контексты для задач
        /// </summary>
        public ObservableCollection<Context> Contexts { get; set; }

        /// <summary>
        /// Счетчики
        /// </summary>
        public ObservableCollection<Counters> CountersCollection { get; set; }

        /// <summary>
        /// Текущее положение ползунка здоровья для интерфейса
        /// </summary>
        public int CurHpFrontEnd
        {
            get
            {
                return HPIneger;

                //if (PersSettings.IsDamageNotHP)
                //{
                //    return HPIneger;
                //}
                //return HpHp;
            }
        }

        public override Rangs CurRang
        {
            get
            {
                if (Rangs == null || !Rangs.Any())
                {
                    return null;
                }

                var lev = PersLevelProperty;
                var firstOrDefault = GetRang(lev);
                return firstOrDefault;
            }
        }

        public DateTime DateLastUseProgram
        {
            get
            {
                var _date = DateTime.Now;
                DateTime _parsDate;
                var success = DateTime.TryParse(dateLastUseProgram, out _parsDate);
                if (success)
                {
                    _date = _parsDate;
                }
                return _date;
            }
            set
            {
                dateLastUseProgram = value.ToString();
            }
        }

        /// <summary>
        /// Дата последнего использования программы
        /// </summary>
        public DateTime DateOfLastUse
        {
            get
            {
                var _date = DateTime.Now;

                DateTime _parsDate;
                var success = DateTime.TryParse(LastDateOfUseProperty, out _parsDate);

                if (success)
                {
                    _date = _parsDate;
                }

                return _date.Date;
            }
        }

        /// <summary>
        /// Sets and gets Записи дневника. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ObservableCollection<Diary> DiaryProperty { get; set; }

        /// <summary>
        /// Минус к опыту
        /// </summary>
        public double ExpBuff
        {
            get
            {
                return _expBuff;
            }
            set
            {
                if (value.Equals(_expBuff)) return;
                _expBuff = value;
                OnPropertyChanged(nameof(ExpBuff));
            }
        }

        public string ExpString
        {
            get
            {
                var pexp = PersExpProperty.ToString().SplitInParts(3);
                var next = ValueMaxProperty.ToString().SplitInParts(3);
                return $"{pexp} / {next}";
            }
        }

        public int ExtraPoints { get; set; }

        /// <summary>
        /// Sets and gets Золото. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int GoldProperty
        {
            get
            {
                return gold;
            }

            set
            {
                if (gold == value || IsRewardsEnabled == false)
                {
                    return;
                }

                gold = value;
                OnPropertyChanged(nameof(GoldProperty));
                StaticMetods.refreshShopItems(this);
            }
        }

        /// <summary>
        /// Высота строки с данными о персе
        /// </summary>
        public double HeightPersRow { get; set; }

        /// <summary>
        /// История персонажа
        /// </summary>
        public string History { get; set; }

        /// <summary>
        /// Здоровье
        /// </summary>
        public int HitPoints
        {
            get
            {
                if (_hitPoints > 100)
                {
                    _hitPoints = 100;
                }

                return _hitPoints;
            }
            set
            {
                if (value == _hitPoints) return;
                if (value > 100) value = 100;
                _hitPoints = value;

                // Баф!!!
                OnPropertyChanged(nameof(HitPoints));
            }
        }

        /// <summary>
        /// Очки здоровья
        /// </summary>
        public double HP
        {
            get
            {
                if (_hp > MaxHP)
                {
                    _hp = MaxHP;
                }

                return _hp;
            }
            set
            {
                if (!persSettings.IsHP)
                {
                    return;
                }

                if (value <= 0)
                {
                    value = MaxHP;

                    foreach (var ab in Abilitis)
                    {
                        ab.ValueProperty = ab.LastValue;
                    }

                    foreach (var cha in Characteristics)
                    {
                        cha.RecountChaValue();
                    }

                    NewRecountExp();
                }

                _hp = value;

                RefreshAllAboutHp();
            }
        }

        /// <summary>
        /// Хп а не урон
        /// </summary>
        public int HpHp => Convert.ToInt32(Math.Ceiling(MaxHP - HP));

        /// <summary>
        /// Очков жизней - целое
        /// </summary>
        public int HPIneger => Convert.ToInt32(Math.Floor(HP));

        public string HPOrDmgString
        {
            get
            {
                if (PersSettings.IsDamageNotHP)
                {
                    return "DMG";
                }
                return "HP";
            }
        }

        /// <summary>
        /// Показатель здоровья
        /// </summary>
        public HP HPProperty { get; set; }

        /// <summary>
        /// Sets and gets Картинка в зависимости от звания?. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool ImageFromeRangsProperty
        {
            get
            {
                return imageFromeRangs;
            }

            set
            {
                if (imageFromeRangs == value)
                {
                    return;
                }

                imageFromeRangs = value;
                OnPropertyChanged(nameof(ImageFromeRangsProperty));
            }
        }

        /// <summary>
        /// Изображение
        /// </summary>
        public new byte[] ImageProperty
        {
            get
            {
                //if (PersSettings.IsActtivateRangse)
                //{
                //    byte[] imageProperty = null;

                // if (CurRang != null) { imageProperty = CurRang.ImageProperty; }

                //    return imageProperty ?? GetDefoultImageFromElement();
                //}
                return image;

                //if (this.ImageFromeRangsProperty && this.CurRang != null)
                //{
                //    if (this.CurRang.ImageProperty == null)
                //    {
                //        this.CurRang.ImageProperty = GetDefoultImageFromElement();
                //    }
                //    return CurRang.ImageProperty;
                //}
                //if (this.image == null)
                //{
                //    this.image = GetDefoultImageFromElement();
                //}
                //return image;
            }
            set
            {
                //if (PersSettings.IsActtivateRangse)
                //{
                //    if (CurRang != null)
                //    {
                //        CurRang.ImageProperty = value;
                //    }
                //}
                //else
                //{
                if (value == null)
                {
                    return;
                }

                image = value;

                OnPropertyChanged(nameof(ImageProperty));
                //}

                OnPropertyChanged(nameof(ImageProperty));

                //if (this.image == value)
                //{
                //    return;
                //}

                //this.image = value;

                //this.OnPropertyChanged(nameof(ImageProperty));
            }
        }

        /// <summary>
        /// Награды в рюкзаке )))
        /// </summary>
        public ObservableCollection<Revard> InventoryItems { get; set; }

        public IEnumerable<Task> InverseTasks
        {
            get { return Tasks.OrderByDescending(n => Tasks.IndexOf(n)); }
        }

        /// <summary>
        /// Sets and gets Активированно автосохранение?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsAutosaveProperty
        {
            get
            {
                return isAutosave;
            }

            set
            {
                if (isAutosave == value)
                {
                    return;
                }

                isAutosave = value;
                OnPropertyChanged(nameof(IsAutosaveProperty));
            }
        }

        /// <summary>
        /// Gets or sets отображаются ли завершенные цели?
        /// </summary>
        public bool isCloseCompleteAims { get; set; }

        /// <summary>
        /// Gets or sets отображаются недоступные цели?
        /// </summary>
        public bool isCloseNotNowAims { get; set; }

        public bool IsFirstUse { get; set; }

        /// <summary>
        /// Активен ли сфокусированный вид
        /// </summary>
        public bool IsFocTasks { get; set; }

        /// <summary>
        /// Скрывать шапку персонажа
        /// </summary>
        public bool IsHideHUD
        {
            get
            {
                return false;
                return _isHideHud;
            }
            set
            {
                return;
                if (value == _isHideHud) return;
                _isHideHud = value;
                OnPropertyChanged(nameof(IsHideHUD));
            }
        }

        /// <summary>
        /// Карты активны?
        /// </summary>
        public bool IsMapsEnabled
        {
            get
            {
                return true;
            }
            set
            {
                return;
                if (value == _isMapsEnabled) return;
                if (value)
                {
                    MessageBox.Show("Доступна новая способность: карты!!!");
                }
                _isMapsEnabled = value;
                OnPropertyChanged(nameof(IsMapsEnabled));
            }
        }

        /// <summary>
        /// Правило паретто - лишние задачи даже не показываются
        /// </summary>
        public bool IsParetto
        {
            get { return _isParetto; }
            set
            {
                if (value == _isParetto) return;
                _isParetto = value;
                OnPropertyChanged(nameof(IsParetto));
            }
        }

        public bool IsPlaningMode
        {
            get
            {
                return true;

                //return _isPlaningMode;
            }
            set
            {
                return;

                //if (value == _isPlaningMode) return;
                //_isPlaningMode = value;
                //OnPropertyChanged(nameof(IsPlaningMode));
                //OnPropertyChanged(nameof(IsSettingsVisibillity));
            }
        }

        public bool IsPlanningModeMain
        {
            get => _isPlanningModeMain; set
            {
                _isPlanningModeMain = value;
                if (value == _isPlanningModeMain) return;
                _isPlaningMode = value;
                OnPropertyChanged(nameof(IsPlanningModeMain));
                OnPropertyChanged(nameof(IsSettingsVisibillity));
            }
        }

        /// <summary>
        /// Квесты активны?
        /// </summary>
        public bool IsQwestsEnabled
        {
            get
            {
                return
                    true;
            }
            set
            {
                return;
                if (value == _isQwestsEnabled) return;
                if (value)
                {
                    MessageBox.Show("Доступна новая способность: квесты!!!");
                }
                _isQwestsEnabled = value;
                OnPropertyChanged(nameof(IsQwestsEnabled));
            }
        }

        /// <summary>
        /// Награды активны?
        /// </summary>
        public bool IsRewardsEnabled
        {
            get
            {
                return true;
            }
            set
            {
                return;
                if (value == _isRewardsEnabled) return;
                if (value)
                {
                    MessageBox.Show("Доступна новая способность: награды!!!");
                }
                _isRewardsEnabled = value;
                OnPropertyChanged(nameof(IsRewardsEnabled));
            }
        }

        public Visibility IsSettingsVisibillity
        {
            get
            {
                //if (!IsPlaningMode) return Visibility.Collapsed;
                if (!IsSetViz) return Visibility.Collapsed;
                return Visibility.Visible;
            }
        }

        public bool IsSetViz
        {
            get
            {
                return IsPlaningMode;
                //return _isSetViz;
            }
            set
            {
                //if (value == _isSetViz) return;
                //_isSetViz = value;
                //OnPropertyChanged(nameof(IsSetViz));
                //OnPropertyChanged(nameof(IsSettingsVisibillity));
            }
        }

        /// <summary>
        /// Надо поспать для роста уровня?
        /// </summary>
        public bool IsSleepForNewLevel
        {
            get
            {
                return false;
                return _isSleepForNewLevel;
            }
            set
            {
                return;
                if (value == _isSleepForNewLevel) return;
                _isSleepForNewLevel = value;
                OnPropertyChanged(nameof(IsSleepForNewLevel));
            }
        }

        public bool IsSortByBalance
        {
            get { return _isSortByBalance; }
            set
            {
                if (value == _isSortByBalance) return;
                _isSortByBalance = value;
                OnPropertyChanged(nameof(IsSortByBalance));
            }
        }

        public bool IsSortByPrioryty
        {
            get { return _isSortByPrioryty; }
            set
            {
                if (value == _isSortByPrioryty) return;
                _isSortByPrioryty = value;
                OnPropertyChanged(nameof(IsSortByPrioryty));
            }
        }

        /// <summary>
        /// Sets and gets Последняя дата использования программы (строка). Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public string LastDateOfUseProperty
        {
            get
            {
                return lastDateOfUse;
            }

            set
            {
                if (lastDateOfUse == value)
                {
                    return;
                }

                lastDateOfUse = value;
                OnPropertyChanged(nameof(LastDateOfUseProperty));
            }
        }

        /// <summary>
        /// Значения уровней и скиллов для разных уровней персонажа
        /// </summary>
        public List<PersLevelsValues> LevelsValueses
        {
            get { return levelsValueses ?? (levelsValueses = new List<PersLevelsValues>()); }
            set { levelsValueses = value; }
        }

        /// <summary>
        /// Полученный опыт при достижении последнего уровня.
        /// </summary>
        public int MaxGettedExp { get; set; }

        /// <summary>
        /// Максимальный полученный уровень за игру этим персонажем
        /// </summary>
        public int MaxGettedLevel
        {
            get
            {
                return _maxGettedLevel;
            }
            set
            {
                if (value == _maxGettedLevel) return;
                _maxGettedLevel = value;
                OnPropertyChanged(nameof(MaxGettedLevel));
            }
        }

        public int MaxHitPoints
        {
            get { return StaticMetods.MaxHitPoints; }
        }

        /// <summary>
        /// Максимальное количество очков здоровья
        /// </summary>
        public double MaxHP
        {
            get { return MaxHitPoints; }
        }

        /// <summary>
        /// Максимальное количество здоровья целое
        /// </summary>
        public int MaxHPIneger => Convert.ToInt32(Math.Floor(MaxHP));

        public override int MaxLevelProperty
        {
            get
            {
                if (BalanceIs50Levels)
                {
                    return 50;
                }

                return 100;
                //var expa = StaticMetods.GetExp(this, true);
                //double lev = StaticMetods.GetLevel(expa, RpgItemsTypes.exp);
                //var mL = Math.Floor(lev);
                //var l = Convert.ToInt32(mL);
                //if (l>= Abilitis.Count+1)
                //{
                //    return l;
                //}
                //else
                //{
                //    return Abilitis.Count+1;
                //}
            }
        }

        public int MaxRangLev
        {
            get
            {
                var sortedRangs = Rangs.OrderByDescending(n => n.LevelRang);
                var after = sortedRangs.LastOrDefault(n => n.LevelRang > PersLevelProperty) ??
                            sortedRangs.LastOrDefault();
                if (after != null) return after.LevelRang;
                return 50;
            }
        }

        /// <summary>
        /// Количество средних бутылочек здоровья
        /// </summary>
        public int MiddleHpBottles
        {
            get
            {
                return _middleHpBottles;
            }
            set
            {
                if (value == _middleHpBottles) return;
                _middleHpBottles = value;
                OnPropertyChanged(nameof(MiddleHpBottles));
            }
        }

        public int MinRangLev
        {
            get
            {
                int vall = 1;
                var sortedRangs = Rangs.OrderByDescending(n => n.LevelRang);
                var before = sortedRangs.FirstOrDefault(n => n.LevelRang <= PersLevelProperty) ??
                             sortedRangs.FirstOrDefault();
                if (before != null) vall = before.LevelRang;
                return vall >= 1 ? vall : 1;
            }
        }

        /// <summary>
        /// Коллекция потребностей
        /// </summary>
        public ObservableCollection<Needness> NeednessCollection { get; set; }

        /// <summary>
        /// Отсортированный вид локаций
        /// </summary>
        public List<ViewsModel> OrderedViewsModel
        {
            get { return Views.ToList(); }
        }

        /// <summary>
        /// Gets or sets Путь к картинке персонажа
        /// </summary>
        public string PathToImageProperty { get; set; }

        /// <summary>
        /// Sets and gets Купленные очки скиллов. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int PayedAbPointsProperty
        {
            get
            {
                return 0;
                return payedAbPoints;
            }

            set
            {
                return;
                if (payedAbPoints == value)
                {
                    return;
                }

                payedAbPoints = value;
                OnPropertyChanged(nameof(PayedAbPointsProperty));
            }
        }

        /// <summary>
        /// Прогресс в процентах
        /// </summary>
        public new double Percentage
        {
            get
            {
                double cur = PersExpProperty - ValueMinProperty;
                double max = ValueMaxProperty - ValueMinProperty;
                var progress = 100.0 * cur / max;
                return Math.Round(progress);
            }
        }

        /// <summary>
        /// Опыт за просто какие-то задачи или там квесты
        /// </summary>
        public int PersExpFromeTasksAndQwests { get; set; }

        /// <summary>
        /// Sets and gets Опыт персонажа. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int PersExpProperty
        {
            get
            {
                return persExp;
            }
            set
            {
                if (persExp == value)
                {
                    return;
                }

                if (value < 0)
                {
                    value = 0;
                }

                persExp = value;
                OnPropertyChanged(nameof(PersExpProperty));
                RecountPersLevel();
                RefreshRang();
                OnPropertyChanged(nameof(Percentage));
                OnPropertyChanged(nameof(ExpString));
            }
        }

        /// <summary>
        /// Sets and gets Уровень персонажа. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int PersLevelProperty
        {
            get
            {
                return persLevel;
            }

            set
            {
                if (persLevel == value)
                {
                    return;
                }

                if (value > persLevel)
                {
                    HP = MaxHitPoints;

                    MaxGettedLevel = value;

                    MaxGettedExp = PersExpProperty;

                    foreach (var ab in Abilitis)
                    {
                        ab.LastValue = ab.ValueProperty;
                    }

                    foreach (var cha in Characteristics)
                    {
                        cha.LastValue = cha.ValueProperty;
                    }
                }

                persLevel = value;

                foreach (var abilitiModel in Abilitis)
                {
                    abilitiModel.RecountValue();
                }

                OnPropertyChanged(nameof(PersLevelProperty));
                OnPropertyChanged(nameof(PersRang));
                setCurRang();
                OnPropertyChanged(nameof(ImageProperty));
                RefreshProgramFunctions();
                StaticMetods.refreshShopItems(this);
                UpdateAbilityPoints();
                StaticMetods.Locator.ucAbilitisVM.RefreshAbilitis();
                SetMinMaxValue();
                Heal();
            }
        }

        /// <summary>
        /// Звание персонажа
        /// </summary>
        public string PersRang
        {
            get
            {
                var firstOrDefault =
                    Rangs.OrderByDescending(n => n.LevelRang)
                        .FirstOrDefault(n => n.LevelRang <= PersLevelProperty);
                if (firstOrDefault != null)
                {
                    return firstOrDefault.NameOfRang;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Настройки программы
        /// </summary>
        public SettingsPers PersSettings
        {
            get { return persSettings; }

            set { persSettings = value; }
        }

        /// <summary>
        /// Заслуженное количество очков знаний
        /// </summary>
        public double PlusAbPoints { get; set; }

        /// <summary>
        /// Sets and gets Очки скиллов + если обычные закончатся. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int PlusAbPointsProperty
        {
            get
            {
                return plusAbPoints;
            }

            set
            {
                if (plusAbPoints == value)
                {
                    return;
                }

                plusAbPoints = value;
                OnPropertyChanged(nameof(PlusAbPointsProperty));
                OnPropertyChanged(nameof(AbilitisPoints));
            }
        }

        /// <summary>
        /// Sets and gets Плюс к опыту за выполненные задачи. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public double PlusExpFromTasksProperty
        {
            get
            {
                return plusExpFromTasks;
            }

            set
            {
                if (plusExpFromTasks == value)
                {
                    return;
                }

                plusExpFromTasks = value >= 0 ? value : 0;
                OnPropertyChanged(nameof(PlusExpFromTasksProperty));
            }
        }

        public RandomRevard RandomRevard
        {
            get { return _randomRevard ?? (_randomRevard = new RandomRevard()); }
            set { _randomRevard = value; }
        }

        /// <summary>
        /// Sets and gets Ранги. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public new ObservableCollection<Rangs> Rangs
        {
            get
            {
                if (rangs == null)
                {
                    rangs = new ObservableCollection<Rangs>();
                }
                if (rangs.Count != StaticMetods.MaxPersAndMonstersRangs)
                {
                    rangs = getDefoultPersRangse();
                }
                return rangs;
            }
            set
            {
                rangs = value;
                OnPropertyChanged(nameof(Rangs));
            }
        }

        /// <summary>
        /// Расса персонажа
        /// </summary>
        public string Rase { get; set; }

        /// <summary>
        /// Расса персонажа
        /// </summary>
        public Rase Rase1 { get; set; }

        /// <summary>
        /// Генератор случайных изображений
        /// </summary>
        public RandomeImageGenerator RIG
        {
            get { return _rig ?? (_rig = new RandomeImageGenerator()); }
            set { _rig = value; }
        }

        public RandomeImageGenerator2 RIG2
        {
            get => _rig2 ?? (_rig2 = new RandomeImageGenerator2());
            set
            {
                if (Equals(value, _rig2)) return;
                _rig2 = value;
                OnPropertyChanged(nameof(RIG2));
            }
        }

        /// <summary>
        /// Sets and gets Выбранный скилл. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public AbilitiModel SellectedAbilityProperty
        {
            get
            {
                return sellectedAbility;
            }

            set
            {
                if (sellectedAbility == value)
                {
                    return;
                }

                sellectedAbility = value;
                OnPropertyChanged(nameof(SellectedAbilityProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранная в данный момент цель (квест). Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public Aim SellectedAimProperty
        {
            get
            {
                return sellectedAim;
            }

            set
            {
                if (sellectedAim == value)
                {
                    return;
                }

                sellectedAim = value;
                OnPropertyChanged(nameof(SellectedAimProperty));
            }
        }

        /// <summary>
        /// Награды в магазине
        /// </summary>
        public ObservableCollection<Revard> ShopItems { get; set; }

        /// <summary>
        /// Строка - текущая дата
        /// </summary>
        public string ShortDateString
        {
            get { return DateOfLastUse.ToString("ddd, dd-MMM", CultureInfo.GetCultureInfo("ru-ru")); }
        }

        /// <summary>
        /// Sets and gets Показывать только задачи, завершающиеся сегодня. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool ShowOnlyTodayTasksProperty
        {
            get
            {
                return showOnlyTodayTasks;
            }

            set
            {
                if (showOnlyTodayTasks == value)
                {
                    return;
                }

                showOnlyTodayTasks = value;
                OnPropertyChanged(nameof(ShowOnlyTodayTasksProperty));
            }
        }

        /// <summary>
        /// Количество маленьких бутылочек здоровья
        /// </summary>
        public int SmallHpBottles
        {
            get
            {
                return _smallHpBottles;
            }
            set
            {
                if (value == _smallHpBottles) return;
                _smallHpBottles = value;
                OnPropertyChanged(nameof(SmallHpBottles));
            }
        }

        /// <summary>
        /// Ступень
        /// </summary>
        public int Stage
        {
            get
            {
                return _stage;
            }
            set
            {
                if (value > StaticMetods.MaxPersAndMonstersRangs)
                {
                    value = StaticMetods.MaxPersAndMonstersRangs;
                }
                if (_stage == value) return;
                _stage = value;
                foreach (var task in Tasks)
                {
                    Task.RecountTaskLevel(task);
                }
            }
        }

        /// <summary>
        /// Статусы для задач
        /// </summary>
        public ObservableCollection<StatusTask> Statuses { get; set; }

        /// <summary>
        /// Лог задач
        /// </summary>
        public List<TaskLog> TaskLogProperty { get; set; }

        /// <summary>
        /// Gets or sets Задачи персонажа
        /// </summary>
        public ObservableCollection<Task> Tasks { get; set; }

        /// <summary>
        /// Gets or sets Типы задач
        /// </summary>
        public ObservableCollection<TypeOfTask> TasksTypes { get; set; }

        /// <summary>
        /// Версия персонажа
        /// </summary>
        public int Version { get; set; } = 2;

        /// <summary>
        /// Gets or sets the view for defoult.
        /// </summary>
        public ViewsModel ViewForDefoult
        {
            get => _viewForDefoult ?? (_viewForDefoult = Views.FirstOrDefault());
            set => _viewForDefoult = value;
        }

        /// <summary>
        /// Gets or sets Виды, фильтры для задач
        /// </summary>
        public ObservableCollection<ViewsModel> Views
        {
            get
            {
                if (NewViews == null || NewViews.Count != 4)
                {
                    NewViews = new ObservableCollection<ViewsModel>
                    {
                        new ViewsModel {NameOfView = "Все", GUID = Guid.NewGuid().ToString()},
                        new ViewsModel {NameOfView = "Навыки", GUID = Guid.NewGuid().ToString()},
                        new ViewsModel {NameOfView = "Квесты", GUID = Guid.NewGuid().ToString()},
                        new ViewsModel {NameOfView = "Монстры", GUID = Guid.NewGuid().ToString()}
                    };
                }

                if (NewViews[0].NameOfView != "Все")
                {
                    NewViews[0].NameOfView = "Все";
                }
                if (NewViews[1].NameOfView != "Навыки")
                {
                    NewViews[1].NameOfView = "Навыки";
                }
                if (NewViews[2].NameOfView != "Квесты")
                {
                    NewViews[2].NameOfView = "Квесты";
                }
                if (NewViews[3].NameOfView != "Монстры")
                {
                    NewViews[3].NameOfView = "Монстры";
                }

                return NewViews;
            }
            set
            {
                return;
            }
        }

        public List<StatusTask> VisibleStatuses
        {
            get
            {
                return
                    Statuses.ToList();

                //return
                //    Statuses.Where(
                //        n =>
                //            n.NameOfStatus.Contains("Первым делом") || n.NameOfStatus.Contains("Когда-нибудь") ||
                //            n.NameOfStatus.Contains("Планируется")).ToList();
            }
        }

        public static int AbCostByLev(double lev)
        {
            var costt = 0;
            for (var i = 1; i <= lev; i++)
            {
                costt += GetLevelCost(i);
                //if (i == 1)
                //{
                //    // Стоимость первого уровня задана в настройках
                //    costt += Pers.OpenAbCost;
                //}
                //else
                //{
                //    costt += i;
                //}
            }
            return costt;
        }

        /// <summary>
        /// Формула стоимости навыков
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public static int AbTotalCost(AbilitiModel ab)
        {
            if (!ab.IsPayedProperty)
            {
                return 0;
            }

            var cellValue = ab.LevelsWhenUp.Count;
            var costt = AbCostByLev(cellValue);

            return costt;
        }

        // = 3;
        /// <summary>
        /// Формула опыта
        /// </summary>
        /// <param name="level">Уровень</param>
        /// <param name="rpgType">Тип рпг элемента (характеристика, скилл?)</param>
        /// <returns>The <see cref="double"/>.</returns>
        public static double ExpToLevel(int level, RpgItemsTypes rpgType)
        {
            var expToLevel = ((level) * ExpOneLev) - ExpOneLev;
            return expToLevel;
            //return 50 * (level - 1) * (level);
            //var n = level - 1;
            //if (n < 1)
            //{
            //    return 0;
            //}
            //var expToPrev = ExpToLevel(n, RpgItemsTypes.exp);

            ////var k = 1.0;
            ////for (int i = 1; i < level; i++)
            ////{
            ////    k = k + (1.0/i);
            ////}
            //var k = 1; //Math.Sqrt(n); //Math.Sqrt(n);  //1 + 0.5 * Math.Sqrt(n - 1);
            ////if (k > 5) k = 5;
            //var exp = 100.0*n*k + expToPrev;
            //exp = StaticMetods.RoundTo100(exp);
            //return exp;
            //if (level < 1)
            //{
            //    return 0;
            //}
            //var expToPrev = ExpToLevel(level - 2, RpgItemsTypes.exp);
            //return 100.0 * (level - 1) + expToPrev;

            //if (level <= 1)
            //{
            //    return 0;
            //}
            ////-------------------------
            //var n = level;
            //double k = 0;
            //for (int i = 1; i < n; i++)
            //{
            //    k += i;
            //}
            //var expToLevel = 100.0 * k + ExpToLevel(n - 1, RpgItemsTypes.exp);
            ////var expToLevel = 50.0 * n * (n + 1) + ExpToLevel(n, RpgItemsTypes.exp);
            //return expToLevel;
        }

        public static void GetArtefactChanges(ref List<viewChangesModel> viewChanges, List<Revard> rv)
        {
            foreach (var revard in rv)
            {
                if (StaticMetods.PersProperty.InventoryItems.Contains(revard))
                {
                    var vm = new viewChangesModel("награда",
                        $"{revard.GetTypeOfRevard()} \"{revard.NameOfProperty}\"!!!",
                        Colors.Gold.ToString(), 0,
                        1,
                        null);
                    vm.@from = 0;
                    vm.to = 1;
                    vm.ChangeProperty = vm.to - vm.@from;
                    vm.MinValueProperty = 0;
                    vm.MaxValueProperty = 1;
                    vm.ImageProperty = revard.ImageProperty;
                    vm.RangProperty = "";
                    vm.RangProperty2 = "";
                    vm.IsValVisibleProperty = Visibility.Collapsed;
                    vm.ChangeString = "";
                    viewChanges.Add(vm);
                }
                else
                {
                    var vm = new viewChangesModel("награда",
                        $"{revard.GetTypeOfRevard()} \"{revard.NameOfProperty}\"!!!",
                        Colors.LightCoral.ToString(), 0,
                        1,
                        null);
                    vm.@from = 1;
                    vm.to = 0;
                    vm.ChangeProperty = vm.to - vm.@from;
                    vm.MinValueProperty = 0;
                    vm.MaxValueProperty = 1;
                    vm.ImageProperty = revard.ImageProperty;
                    vm.RangProperty = "";
                    vm.RangProperty2 = "";
                    vm.IsValVisibleProperty = Visibility.Collapsed;
                    vm.ChangeString = "";
                    viewChanges.Add(vm);
                }
            }
        }

        //= 3;
        /// <summary>
        /// Стоимость для покупки для уровня
        /// </summary>
        /// <param name="lev">Купленный уровень</param>
        /// <param name="abilitiModel"></param>
        /// <returns>Стоимость для покупки</returns>
        public static int GetLevelCost(int lev)
        {
            return 0;
            //if (!abilitiModel.IsPayedProperty)
            //{
            //    return Pers.OpenAbCost;
            //}

            //int c = 0;

            //for (int i = 1; i <= lev; i++)
            //{
            //    c += i;
            //}

            //return c;
            //return lev;
        }

        /// <summary>
        /// Загружаем обучающую компанию
        /// </summary>
        /// <returns></returns>
        public static Pers LoadLerningTour()
        {
            var pers = LoadPers(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "LearningPers"));
            pers._maxGettedLevel = 1;
            return pers;
        }

        /// <summary>
        /// Загрузить (импортировать) персонажа из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public static Pers LoadPers(string path)
        {
            try
            {
                Pers pers;
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    pers = (Pers)formatter.Deserialize(fs);
                    SetPersSettings(pers);
                }

                StaticMetods.PersProperty = pers;

                if (path == Path.Combine(Directory.GetCurrentDirectory(), "Templates", "LearningPers"))
                {
                    pers.ToNullPers();
                }

                return pers;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// После сна - пересчитать очки навыков и навыки
        /// </summary>
        /// <param name="prs">Перс</param>
        public static void NewLevelAfterSleep(Pers prs)
        {
            //if (prs.PersLevelProperty > prs.MaxGettedLevel) prs.MaxGettedLevel = prs.PersLevelProperty;
            //prs.RecountPlusAbPoints();
            //foreach (var abilitiModel in prs.Abilitis)
            //{
            //    abilitiModel.BuyedInThisLevel = false;
            //    abilitiModel.GetReqwirements();
            //}
        }

        /// <summary>
        /// Настройка для показа изменений
        /// </summary>
        /// <param name="viewChangesModel"></param>
        /// <param name="change"></param>
        /// <param name="colIfUpgree"></param>
        /// <param name="colIfDegree"></param>
        public static void SetChangesNameAndColor(viewChangesModel viewChangesModel, double change,
            string colIfUpgree = "", string colIfDegree = "")
        {
            if (string.IsNullOrEmpty(colIfUpgree))
            {
                colIfUpgree = Colors.Gold.ToString();
            }
            if (string.IsNullOrEmpty(colIfDegree))
            {
                colIfDegree = Colors.LightCoral.ToString();
            }

            if (change > 0)
            {
                viewChangesModel.ChangeString = $"+{change}";
                viewChangesModel.Цвет = colIfUpgree;
            }
            else
            {
                viewChangesModel.ChangeString = $"{change}";
                viewChangesModel.Цвет = colIfDegree;
            }
        }

        /// <summary>
        /// Задать пресет
        /// </summary>
        /// <param name="pers">Персонаж</param>
        /// <param name="presetName">Название пресета</param>
        public static void SetPreset(Pers pers, string presetName)
        {
            switch (presetName)
            {
                case "simple":
                    break;

                case "tes":
                    break;

                case "dm":
                    break;
            }

            SetPersSettings(pers);
            Messenger.Default.Send("Обновить ранги навыков!");
            Messenger.Default.Send("Обновить ранги характеристик!");
        }

        /// <summary>
        /// Когда здоровье меньше или равно нулю - вычитаем из опыта
        /// </summary>
        public void BuffEXP()
        {
            //if (PersSettings.IsHP == false) return;
            //double buff = 0;
            //if (PersLevelProperty < 2)
            //{
            //    buff = 0;
            //    ExpBuff = 0;
            //    foreach (var abilitiModel in Abilitis)
            //    {
            //        abilitiModel.Experiance = 0;
            //    }
            //}
            //else
            //{
            //    //double percentage = (Convert.ToDouble(PersExpProperty) - ValueMinProperty)/(ValueMaxProperty - ValueMinProperty);
            //    var i = PersLevelProperty; //PersLevelProperty-1;
            //    if (i < 1) i = 1;
            //    double expToLev = ExpToLevel(i, RpgItemsTypes.exp);
            //    //double expToLev2 = ExpToLevel(PersLevelProperty, RpgItemsTypes.exp);
            //    //double plus = (expToLev2 - expToLev1)*percentage;
            //    buff = PersExpProperty - expToLev; //+ plus);
            //}

            //// Все задачи на завтра переносим
            //var notToDone =
            //    MainViewModel.GetNotDonnedTodayTasks();
            //foreach (
            //    var task in
            //        notToDone)
            //{
            //    task.ClickPlusMinusTomorrowTask(StaticMetods.PersProperty, true, true, true);
            //}

            //ViewChangesWindow vc = new ViewChangesWindow();
            //var header =
            //    "Ой! Получены критические повреждения!\nОтдохни в таверне и завтра возвращайся к приключениям!!!";
            //Brush col = Brushes.Red;
            //var itemImageProperty =
            //    StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "bad.png"));
            //vc.headerText.Text = header;
            //vc.headerText.Foreground = col;
            //vc.Image.Source = StaticMetods.getImagePropertyFromImage(itemImageProperty);
            //vc.dateText.Visibility = Visibility.Collapsed;
            //vc.imEndOfTurn.Visibility = Visibility.Visible;
            //vc.btnOk.Click += (sender, args) => { vc.Close(); };
            //vc.ShowDialog();

            ////double buff = MaxHP*2.0;
            ////if (PersExpProperty<buff)
            ////{
            ////    buff = PersExpProperty;
            ////}
            //ExpBuff += buff;
            //PersExpProperty = StaticMetods.GetExp(this);
            //Tasks.ToList().ForEach(n => n.Rage = 0);
            //Heal();
        }

        /// <summary>
        /// Штрафуем персонажа если очки здоровья равны нулю
        /// </summary>
        public void BuffPersToNullHP()
        {
            System.Windows.MessageBox.Show("Поражение!!! Получайте штраф опыта!");
            Messenger.Default.Send("!RIP!");
        }

        public override void ChangeValuesOfRelaytedItems()
        {
        }

        /// <summary>
        /// Проверка на штраф по ХП
        /// </summary>
        public bool CheckBuffExpByHp()
        {
            if (HP >= MaxHP)
            {
                BuffEXP();
                RefreshAllAboutHp();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Считаем изменения в уровнях скиллов и характеристик
        /// </summary>
        /// <param name="valBefore"></param>
        /// <param name="valAfter"></param>
        public ObservableCollection<Tuple<string, BitmapImage, string, string>> GetAbLevelsChange(
            List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter)
        {
            // тип, название, картинка, повышен/понижен, уровень, ранг?
            var levelsChange =
                new ObservableCollection<Tuple<string, BitmapImage, string, string>>();
            foreach (var before in valBefore)
            {
                var pers = this;
                var ab = pers.Abilitis.FirstOrDefault(n => n.GUID == before.Item2);
                if (ab == null)
                {
                    continue;
                }
                var imageProperty2 = ab.ImageProperty;
                string header = $"Навык \"{ab.NameOfProperty}\" ";
                var valueBefore = Convert.ToDouble(before.Item3);
                var val2 = valAfter.FirstOrDefault(n => n.Item1 == "навык" && n.Item2 == before.Item2);
                if (val2 == null)
                {
                    continue;
                }
                var valueAfter = Convert.ToDouble(val2.Item3);

                // Уровень повышен?
                var alfa = string.Empty; // Повышен или понижен
                var levelAfter = Convert.ToInt32(valueAfter);
                if (Convert.ToInt32(valueBefore) < levelAfter)
                {
                    alfa = "повышен";
                    header += "↑ ";
                }
                else if (Convert.ToInt32(valueBefore) > levelAfter)
                {
                    // Уровень понижен?
                    alfa = "понижен";
                    header += "↓ ";
                }
                header += $"{ab.RangName}";
                if (string.IsNullOrEmpty(alfa) == false)
                {
                    levelsChange.Add(
                        new Tuple<string, BitmapImage, string, string>(
                            header,
                            StaticMetods.getImagePropertyFromImage(imageProperty2),
                            alfa, ab.GUID));
                }
            }
            return levelsChange;
        }

        /// <summary>
        /// Расчет числа очков скиллов
        /// </summary>
        /// <returns></returns>
        public int GetAbPoints(int curLevel)
        {
            return 0;
            //double payedAb = Abilitis.Sum(n => n.TotalPayedCost);
            //double abP = PlusAbPoints - payedAb;
            //return Convert.ToInt32(Math.Ceiling(abP));
        }

        //    // Расчет уровней и опыта персонажа
        //    PersExpProperty = StaticMetods.GetExp(this);
        //    PersLevelProperty = StaticMetods.GetLevel(PersExpProperty, RpgItemsTypes.exp);
        //}
        /// <summary>
        /// Считаем изменения в уровнях скиллов и характеристик
        /// </summary>
        /// <param name="valBefore"></param>
        /// <param name="valAfter"></param>
        public ObservableCollection<Tuple<string, BitmapImage, string, Characteristic>> GetChaLevelsChange(
            List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter)
        {
            // тип, название, картинка, повышен/понижен, уровень, ранг?
            var levelsChange =
                new ObservableCollection<Tuple<string, BitmapImage, string, Characteristic>>();

            var beff = valBefore.Where(n => n.Item1 == "характеристика");
            var aff = valAfter.Where(n => n.Item1 == "характеристика");

            var split = beff.Join(aff, i => i.Item2, o => o.Item2,
                (i, o) =>
                    new
                    {
                        cha = Characteristics.First(n => n.GUID == i.Item2),
                        change = Convert.ToInt32(o.Item3) - Convert.ToInt32(i.Item3)
                    }).Where(n => n.change != 0).ToList();

            foreach (var channge in split)
            {
                var pers = this;
                var characteristic = channge.cha;
                var imageProperty2 = characteristic.ImageProperty;
                string header = $"Характеристика \"{characteristic.NameOfProperty}\" ";

                // Уровень повышен?
                var alfa = string.Empty; // Повышен или понижен
                if (channge.change > 0)
                {
                    alfa = "повышен";
                    header += "↑ ";
                }
                else if (channge.change < 0)
                {
                    // Уровень понижен?
                    alfa = "понижен";
                    header += "↓ ";
                }

                header += $"{characteristic.ChaRang}";

                if (string.IsNullOrEmpty(alfa) == false)
                {
                    levelsChange.Add(
                        new Tuple<string, BitmapImage, string, Characteristic>(
                            header,
                            StaticMetods.getImagePropertyFromImage(imageProperty2),
                            alfa, characteristic));
                }
            }

            return levelsChange;
        }

        // // Расчет уровней характеристик foreach (var characteristic in Characteristics) {
        // characteristic.ValueProperty = characteristic.GetChaValue(); characteristic.LevelProperty
        // = characteristic.GetLevel(); characteristic.SetMinMaxValue(); }
        /// <summary>
        /// The get changes.
        /// </summary>
        /// <param name="valBefore">The val before.</param>
        /// <param name="valAfter">The val after.</param>
        /// <param name="viewChanges">The view changes.</param>
        /// <param name="beforeExp">The before exp.</param>
        /// <param name="afterExp">The after exp.</param>
        /// <param name="beforeGold">The before gold.</param>
        /// <param name="afterGold">The after gold.</param>
        public void GetChanges(
            List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter,
            List<viewChangesModel> viewChanges,
            out double beforeExp,
            out double afterExp,
            out int beforeGold,
            out int afterGold, List<Revard> allRevards)
        {
            var pers = this;

            // Склеенные изменения
            var splitChanges = (from before in valBefore
                                let bef = Convert.ToDouble(before.Item3)
                                join after in valAfter on before.Item2 equals after.Item2
                                let aft = Convert.ToDouble(after.Item3)
                                where Math.Abs(aft - bef) >= 0.01
                                select new { guid = before.Item2, before = bef, after = aft }).ToList();

            // Изменения рангов
            var bExp = Convert.ToDouble(valBefore.First(n => n.Item1 == "опыт").Item3);
            var aExp = PersExpProperty;
            GetRangsChanges(viewChanges, bExp, aExp, pers);

            // Опыт
            ChangesExp(valBefore, valAfter, viewChanges, out beforeExp, out afterExp, pers);

            var abBefore = (from ab in pers.Abilitis
                            let any = valBefore.FirstOrDefault(n => n.Item2 == ab.GUID)
                            where any != null
                            let minAb = Convert.ToDouble(any.Item3)
                            select new { abiliti = ab, before = minAb, after = ab.CellValue }).Where(
                            n => Math.Abs(n.after - n.before) >= 0.01);

            if (pers.persSettings.Is10AbLevels
                || pers.persSettings.Is5_5_50)
            {
                abBefore = (from ab in pers.Abilitis
                            let any = valBefore.FirstOrDefault(n => n.Item2 == ab.GUID + "з")
                            where any != null
                            let minAb = Math.Round(Convert.ToDouble(any.Item3), 2)
                            select new { abiliti = ab, before = minAb, after = ab.ValueProperty }).Where(
                            n => Math.Abs(n.after - n.before) >= 0.01);
            }

            List<viewChangesModel> abChenge = (
                from changeAbiliti in abBefore.ToList()
                let fromLev = changeAbiliti.before
                let toLev = changeAbiliti.after
                let abiliti = changeAbiliti.abiliti
                select
                    new viewChangesModel(
                        "НавыкУр",
                        $"{abiliti.NameOfProperty}:",
                        abiliti.Cvet,
                        changeAbiliti.before,
                        changeAbiliti.after,
                        abiliti.Rangs)
                    {
                        @from = fromLev,
                        to = toLev,
                        ChangeProperty = toLev - fromLev,
                        MinValueProperty = 0,
                        MaxValueProperty = StaticMetods.MaxAbLevel,
                        ImageProperty = abiliti.ImageProperty,
                        RangProperty = $"{abiliti.RangName}",
                        RangProperty2 = "",
                        IsValVisibleProperty = Visibility.Collapsed
                    }).ToList();

            foreach (var viewChangesModel in abChenge)
            {
                SetChangesNameAndColor(viewChangesModel, viewChangesModel.ChangeProperty);
                viewChangesModel.ChangeString = ""; //viewChangesModel.ChangeProperty > 0 ? $"+" : $"-";
            }

            var changeCharacts = (from characteristic in pers.Characteristics
                                  join vb in valBefore
                                  on characteristic.GUID + "з" equals vb.Item2
                                  select new { cha = characteristic, before = Math.Round(Convert.ToDouble(vb.Item3), 2), after = Math.Round(characteristic.ValueProperty, 2) })
                                  .Where(n => Math.Abs(n.after - n.before) >= 0.01);

            if (pers.persSettings.IsFUDGE)
            {
                changeCharacts = (from characteristic in pers.Characteristics
                                  join vb in valBefore
                                  on characteristic.GUID equals vb.Item2
                                  select new { cha = characteristic, before = Convert.ToDouble(vb.Item3), after = characteristic.CellValue })
                                  .Where(n => Math.Abs(n.after - n.before) >= 0.01);
            }

            var chaChenge = (
                from chengeCha in changeCharacts.ToList()
                let fromLev = chengeCha.before
                let toLev = chengeCha.after
                let characteristic = chengeCha.cha
                select
                    new viewChangesModel(
                        "ХарактеристикаУр",
                        $"{characteristic.NameOfProperty}:",
                        characteristic.Cvet,
                        chengeCha.before,
                        chengeCha.after,
                        characteristic.Rangs)
                    {
                        @from = fromLev,
                        to = toLev,
                        ChangeProperty = toLev - fromLev,
                        MinValueProperty = 0,
                        MaxValueProperty = StaticMetods.MaxChaLevel,
                        ImageProperty = characteristic.ImageProperty,
                        RangProperty = $"{characteristic.RangName}",
                        RangProperty2 = "",
                        IsValVisibleProperty = Visibility.Collapsed
                    }).ToList();

            foreach (var viewChangesModel in chaChenge)
            {
                SetChangesNameAndColor(viewChangesModel, viewChangesModel.ChangeProperty);
                viewChangesModel.ChangeString = "";
            }

            viewChanges.AddRange(chaChenge.Where(n => n.ChangeProperty > 0).OrderByDescending(n => n.to));
            viewChanges.AddRange(chaChenge.Where(n => n.ChangeProperty < 0).OrderByDescending(n => n.to));

            viewChanges.AddRange(abChenge.Where(n => n.ChangeProperty > 0).OrderByDescending(n => n.to));
            viewChanges.AddRange(abChenge.Where(n => n.ChangeProperty < 0).OrderByDescending(n => n.to));

            // Здоровье
            ChangesHP(valBefore, valAfter, viewChanges, pers);

            var changeAim = from qwest in pers.Aims.OrderBy(n => n.CompositeAims.Count)
                            join change1 in splitChanges on qwest.GUID equals change1.guid
                            select new { qwest, change1.before, change1.after };

            foreach (var aimChange in changeAim.OrderBy(n => n.after))
            {
                if (Math.Abs(aimChange.after - aimChange.before) >= 1.0)
                {
                    var changes = new viewChangesModel(
                        "Квест",
                        $"Квест \"{aimChange.qwest.NameOfProperty}\":",
                        Brushes.Gold.ToString(),
                        aimChange.before,
                        aimChange.after,
                        new ObservableCollection<Rangs>())
                    {
                        MaxValueProperty = 100.0,
                        @from = aimChange.before,
                        to = aimChange.after,
                        RangProperty2 = "%"
                    };

                    SetChangesNameAndColor(changes, changes.ChangeProperty);
                    changes.ChangeProperty = changes.to - changes.@from;
                    changes.ImageProperty = aimChange.qwest.ImageProperty;
                    changes.ChangeString = changes.ChangeProperty > 0
                        ? $"+{changes.ChangeProperty}%"
                        : $"{changes.ChangeProperty}%";

                    //changes.RangProperty = $"Квест \"{aimChange.qwest.NameOfProperty}\": ";
                    changes.RangProperty2 = $"%";
                    changes.IsValVisibleProperty = Visibility.Visible;

                    viewChanges.Add(changes);
                }
            }

            // Золото
            ChangesGold(valBefore, valAfter, viewChanges, out beforeGold, out afterGold);

            // Инвентарь
            ChangesInventory(valBefore, valAfter, viewChanges, allRevards);

            var rv = QwestsViewModel.AddArtefacts(StaticMetods.PersProperty);
            GetArtefactChanges(ref viewChanges, rv);

            // Награды доступны ChangesRewards(valBefore, valAfter, viewChanges, allRevards);
        }

        /// <summary>
        /// Получить дату последнего сохранения в проге в формате дата тайм
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateOfLastSave()
        {
            DateTime date;
            DateTime.TryParse(dateOfLastSave, out date);
            return date;
        }

        public override byte[] GetDefoultImageFromElement()
        {
            var Im = StaticMetods.pathToImage(Path.Combine(Environment.CurrentDirectory, "Images", "лего друид.jpg"));
            return Im;
        }

        public override int GetLevel()
        {
            return StaticMetods.GetLevel(ValueProperty, RpgItemsTypes.exp);
        }

        /// <summary>
        /// The get values collection.
        /// </summary>
        /// <param name="valBefore">The val before.</param>
        /// <param name="task">Задача, если нужно отобразить также значение задачи</param>
        public void GetValuesCollection(List<Tuple<string, string, string>> valBefore, Task task = null)
        {
            // Здоровье
            HPChanges(valBefore);

            // Опыт
            ExpChanges(valBefore);

            // скиллы
            AbChanges(valBefore);

            // Характеристики
            ChaChenges(valBefore);

            // Квесты
            QwestChanges(valBefore);

            // Инвентарь
            valBefore.AddRange(
                from inv in InventoryItems
                select new Tuple<string, string, string>("предмет", inv.GUID, "1"));

            // Доступные награды
            valBefore.AddRange(from inv in ShopItems
                               select new Tuple<string, string, string>("награда", inv.GUID, inv.IsEnabledProperty ? "1" : "0"));

            // Золото
            GoldChanges(valBefore);
        }

        /// <summary>
        /// С получением нового уровня или обнулением перса подлечиваемся
        /// </summary>
        public void Heal()
        {
            // HP = 0;
        }

        /// <summary>
        /// Новый пересчет опыта
        /// </summary>
        public void NewRecountExp()
        {
            if (Characteristics?.Any() != true)
            {
                PersExpProperty = 0;
                return;
            }

            // С балансом
            var exp = RetExp(null);

            PersExpProperty = (int)exp;
        }

        public void RecountChaValues()
        {
            foreach (var characteristic in Characteristics)
            {
                Characteristic.recountOneChaVal(characteristic);
            }
        }

        public void RecountPersLevel()
        {
            PersLevelProperty = StaticMetods.GetLevel(PersExpProperty, RpgItemsTypes.exp);

            //int i = 0;
            //while (PersExpProperty >= Pers.ExpToLevel(i, RpgItemsTypes.exp))
            //{
            //    i++;
            //}

            //PersLevelProperty = i - 1;

            //var sqrt = Math.Sqrt((exp / 125.0) + 1.0);
            //int level = (int)Math.Floor((1 + sqrt) / 2.0);

            //return level;
        }

        public void RecountPlusAbPoints()
        {
            //return;
            //PersExpProperty = StaticMetods.GetExp(this);
            ////PersLevelProperty = StaticMetods.GetLevel(PersExpProperty, RpgItemsTypes.exp);
            //var pointsPerLevel = PointsPerLevel;
            //double plusPoints = MaxGettedLevel * pointsPerLevel;
            //if (plusPoints < 0) plusPoints = 0;
            //PlusAbPoints = plusPoints;
            //OnPropertyChanged(nameof(AbilitisPoints));
            //IsSleepForNewLevel = false;
        }

        /// <summary>
        /// Пересчитываем уровни рангов персонажа
        /// </summary>
        public void RecountRangLevels()
        {
            Rangs[0].LevelRang = 0;
            Rangs[0].NumRang = 1;

            double v = 20.0;

            if (BalanceIs50Levels)
            {
                v = 10;
            }

            for (int i = 1; i < Rangs.Count; i++)
            {
                Rangs[i].LevelRang = (int)Math.Ceiling(i * v);
                Rangs[i].NumRang = i + 1;
            }

            RefreshRang();
        }

        public void RefreshAllAboutHp()
        {
            OnPropertyChanged(nameof(HP));
            OnPropertyChanged(nameof(HPIneger));
            OnPropertyChanged(nameof(MaxHP));
            OnPropertyChanged(nameof(MaxHPIneger));
            OnPropertyChanged(nameof(HpHp));
            OnPropertyChanged(nameof(CurHpFrontEnd));
            OnPropertyChanged(nameof(HPOrDmgString));
        }

        public void RefreshMaxPersLevel()
        {
            OnPropertyChanged(nameof(MaxLevelProperty));
        }

        ///// <summary>
        /////     Пересчитать все характеристики персонажа
        ///// </summary>
        //public void RefreshCharacteristics()
        //{
        //    foreach (var characteristic in Characteristics)
        //    {
        //        characteristic.ValueProperty = characteristic.GetChaValue();
        //    }
        //}
        public void RefreshOrderedLocations()
        {
            OnPropertyChanged(nameof(OrderedViewsModel));
        }

        public void RefreshRang()
        {
            OnPropertyChanged(nameof(CurRang));
            OnPropertyChanged(nameof(MinRangLev));
            OnPropertyChanged(nameof(MaxRangLev));
            Stage = Rangs.First(n => n.NameOfRang == CurRang.NameOfRang).NumRang;
        }

        /// <summary>
        /// Возвращает опыт.
        /// </summary>
        /// <param name="ab">Если навык не пуст, то фейковый расчет.</param>
        /// <param name="directFakeVal">Для этого навыка можго задать конкретное файк значение.</param>
        /// <returns></returns>
        public double RetExp(AbilitiModel ab, double? directFakeVal = null)
        {

            if (Characteristics == null || Characteristics.Any() == false || Abilitis == null || Abilitis.Any() == false)
                return 0;

            foreach (var characteristic in Characteristics)
            {
                Characteristic.recountOneChaVal(characteristic, ab, directFakeVal);
            }

            double GetChaVal(Characteristic cha)
            {
                bool noFake = false;

                if (ab == null)
                {
                    noFake = true;
                }
                else
                {
                    if (cha.NeedAbilitisProperty.FirstOrDefault(n=>n.AbilProperty==ab && n.KoeficientProperty>0)==null)
                    {
                        noFake = true;
                    }
                }

                double d = noFake ? cha.ValueProperty : cha.FakeForSort;
                

                if (d > PersSettings.MaxChaLev)
                    d = PersSettings.MaxChaLev;

                return d;
            }

            double max = 0;
            double cur = 0;

            bool isForCha = true;
            if (!isForCha)
            {
                // Расчет по навыкам
                foreach (var abil in Abilitis)
                {
                    abil.KExpRelay = Characteristics
                        .Sum(n => n.NeedAbilitisProperty.Where(q => q.AbilProperty == abil)
                        .Sum(q =>
                        {
                            //double maxx = n.NeedAbilitisProperty.Max(z => z.KoeficientProperty);
                            return q.KoeficientProperty; // / maxx;
                        }));

                    double valueProperty = abil.ValueProperty;
                    if (abil == ab)
                    {
                        if (directFakeVal.HasValue)
                            valueProperty = directFakeVal.Value;
                        else
                            valueProperty = ab.FakeForSort;
                    }

                    max += abil.KExpRelay * abil.MaxValue;
                    cur += abil.KExpRelay * valueProperty;
                }
            }
            else
            {
                // Расчет по характеристикам
                double start = Characteristics.Sum(n => n.FirstVal);
                max = (Characteristics.Count * PersSettings.MaxChaLev) - start;
                var sum = Characteristics.Sum(n => GetChaVal(n));
                cur = sum - start;
            }

            double progress = cur / max;
            double maxLev = 100.001;

            if (StaticMetods.PersProperty.BalanceIs50Levels)
                maxLev = 50.001;

            double lev = (maxLev * progress);
            double exp = lev * ExpOneLev;

            return exp;
        }

        /// <summary>
        /// Задать дату последнего сохранения
        /// </summary>
        public void SetDateOfLastsave()
        {
            dateOfLastSave = DateTime.Now.ToString();
        }

        public override void SetMinMaxValue()
        {
            ValueMinProperty =
                Convert.ToInt32(ExpToLevel(PersLevelProperty, RpgItemsTypes.exp));

            ValueMaxProperty =
                Convert.ToInt32(ExpToLevel(PersLevelProperty + 1, RpgItemsTypes.exp));
        }

        public void ShowChangeAbPoints(int beforeAbPoints, int afterAbPoints)
        {
            var changeAbPoints = afterAbPoints - beforeAbPoints;
            if (afterAbPoints > 0 && changeAbPoints != 0)
            {
                var vc = new ViewChangesClass(InventoryItems.Union(ShopItems).ToList());
                var itemImageProperty =
                    StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png"));
                var vcm = new List<viewChangesModel>();
                var viewChangesModel = new viewChangesModel(
                    string.Empty,
                    "Кристаллы знаний:",
                    Colors.Gold.ToString(),
                    beforeAbPoints,
                    afterAbPoints,
                    null)
                {
                    ImageProperty =
                        StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images",
                            "diamond.png")),
                    ChangeProperty = afterAbPoints - beforeAbPoints,
                    MinValueProperty = 0,
                    MaxValueProperty = afterAbPoints > beforeAbPoints ? afterAbPoints : beforeAbPoints,
                    Цвет = Brushes.Transparent.ToString()
                };
                var isChangePlus = viewChangesModel.ChangeProperty > 0;
                var pre = isChangePlus ? "+" : "";
                viewChangesModel.ChangeString = $"{pre}{viewChangesModel.ChangeProperty}";

                viewChangesModel.RangProperty = $"";
                viewChangesModel.RangProperty2 = $"";
                viewChangesModel.IsValVisibleProperty = Visibility.Visible;

                vcm.Add(viewChangesModel);
                vc.ViewChanges = vcm;

                var header = isChangePlus ? "Получены кристаллы знания!" : "Потрачены кристаллы знания!";
                StaticMetods.ShowItemToPersChanges(header, itemImageProperty, vcm,
                    Brushes.Green, "", null, false);

                StaticMetods.Locator.MainVM.OpenQwickButtonCommand.Execute("Информация");
            }
        }

        /// <summary>
        /// Показать изменения в уровнях характеристик и скиллов
        /// </summary>
        /// <param name="valBefore">Значения до</param>
        /// <param name="valAfter">Значения после</param>
        public void ShowChangeChaLevels(
            List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter)
        {
            var _pers = this;

            var changesLevels = _pers.GetChaLevelsChange(valBefore, valAfter);

            if (changesLevels != null && changesLevels.Count != 0)
            {
                foreach (var changesLevel in
                    changesLevels)
                {
                    //var lv = new LevelsChangesView();
                    //var header = changesLevel.Item1;
                    //lv.Image.Source = changesLevel.Item2;
                    //lv.Header.Text = header;
                    //lv.Header.Foreground = changesLevel.Item3 == "повышен" ? Brushes.Green : Brushes.Red;
                    //lv.btnOk.Click += (sender, args) => { lv.Close(); };

                    //// Добавляем хоткей
                    //var saveCommand = new RelayCommand(() => { lv.Close(); });
                    //lv.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Space)));
                    //lv.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Return)));

                    //lv.down.Visibility = Visibility.Collapsed;
                    //lv.up.Visibility = Visibility.Collapsed;

                    //lv.ShowDialog();
                    StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.ХарактеристикаПрокачана,
                        Characteristics.First(n => n.GUID == changesLevel.Item4.GUID));
                }
            }
        }

        /// <summary>
        /// Отобразить изменения уровня персонажа
        /// </summary>
        /// <param name="afterExp">Опыт после</param>
        /// <param name="beforeExp">Опыт до</param>
        /// <param name="viewChanges"></param>
        /// <param name="isShowPersInfo">Показывать инфу о персе чтобы купить ОН?</param>
        public void ShowChangePersLevel(double afterExp, double beforeExp, List<viewChangesModel> viewChanges,
            out bool isShowPersInfo)
        {
            isShowPersInfo = false;
            // Показываем изменения уровня персонажа
            var beforeLevel = StaticMetods.GetLevel(beforeExp, RpgItemsTypes.exp);
            var afterLevel = StaticMetods.GetLevel(afterExp, RpgItemsTypes.exp);

            var pers = StaticMetods.PersProperty;
            var sortedRangs = pers.Rangs.OrderByDescending(n => n.LevelRang);

            var vc = new ViewChangesClass(this.InventoryItems.Union(this.ShopItems).ToList());
            vc.GetValBefore();

            // Если новый уровень больше предыдущего
            if (afterLevel > beforeLevel)
            {
                // isShowPersInfo = true;
                ShowUpCongrant(viewChanges, sortedRangs, beforeLevel, afterLevel);
                StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.УровеньПовышен, null);

                if (PersSettings.IsActtivateRangse)
                {
                    if (GetRang(beforeLevel) != GetRang(afterLevel))
                    {
                        ShowRangChange();
                        //MessageBox.Show("Монстры набирают силу!!!");
                        StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.РангИзменен, null);
                    }
                }
            }
            // Если новый уровень меньше
            else if (afterLevel < beforeLevel)
            {
                // isShowPersInfo = true;
                ShowUpCongrant(viewChanges, sortedRangs, beforeLevel, afterLevel);
                StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.УровеньПонижен, null);
            }

            if (GetRang(beforeLevel) != GetRang(afterLevel))
                foreach (var tsk in Tasks)
                {
                    tsk.PathToIm = RIG2.GetNextImage();

                    foreach (var sbtsk in tsk.SubTasks)
                        sbtsk.PathToIm = RIG2.GetNextImage();
                }

            StaticMetods.AbillitisRefresh(StaticMetods.PersProperty);
            StaticMetods.RefreshAllQwests(StaticMetods.PersProperty, false, true, true);

            // Конец игры, если уровень перса 100
            if (StaticMetods.PersProperty.PersLevelProperty >= 100)
                StaticMetods.ShowGameOver();
        }

        /// <summary>
        /// Показать изменения в уровнях скиллов
        /// </summary>
        /// <param name="valBefore">Значения до</param>
        /// <param name="valAfter">Значения после</param>
        public List<AbilitiModel> ShowChengeAbLevels(
            List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter, bool isEditAb = true, bool isShow = true)
        {
            var abList = new List<AbilitiModel>();
            var _pers = this;
            var changesLevels = _pers.GetAbLevelsChange(valBefore, valAfter);

            if (changesLevels != null && changesLevels.Count != 0)
            {
                foreach (var changesLevel in
                    changesLevels)
                {
                    //if (isShow)
                    //{
                    //    var lv = new LevelsChangesView();
                    //    var header = changesLevel.Item1;
                    //    lv.Image.Source = changesLevel.Item2;
                    //    lv.Header.Foreground = changesLevel.Item3 == "повышен" ? Brushes.Green : Brushes.Red;
                    //    lv.Header.Text = header;
                    //    lv.btnOk.Click += (sender, args) => { lv.Close(); };

                    //    // Добавляем хоткей
                    //    var saveCommand = new RelayCommand(() => { lv.Close(); });
                    //    lv.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Space)));
                    //    lv.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Return)));

                    //    lv.down.Visibility = Visibility.Collapsed;
                    //    lv.up.Visibility = Visibility.Collapsed;
                    //    lv.ShowDialog();
                    //}

                    var abilitiModel = Abilitis.First(n => n.GUID == changesLevel.Item4);
                    //if (isEditAb) abList.Add(abilitiModel);
                    StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.НавыкПрокачан, abilitiModel);
                }
            }

            return abList;
            //var abBefore = valBefore.Where(n => n.Item1 == "навык");
            //var abAfter = valAfter.Where(n => n.Item1 == "навык");
            //var splitAb =
            //    abAfter.Join(abBefore, lvl => lvl.Item2, o => o.Item2,
            //        (lvl, o) =>
            //            new
            //            {
            //                abil = Abilitis.First(n => n.GUID == lvl.Item2),
            //                mod = Convert.ToDouble(lvl.Item3) - Convert.ToDouble(o.Item3)
            //            }).Where(n=>n.mod!=0).ToList();

            //foreach (var split in splitAb)
            //{
            //    var ability = split.abil;
            //    ability.RefreshComplecsNeeds();

            // // Открываем окно

            //    //if (isEditAb) ability.EditAbility();
            //    StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.НавыкПрокачан, Abilitis.First(n=>n.GUID==ability.GUID));
            //}
        }

        /// <summary>
        /// Показать что квесты готовы!
        /// </summary>
        /// <param name="valBefore"></param>
        /// <param name="valAfter"></param>
        public List<Aim> ShowDoneQwests(
            List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter,
            bool isShow = true)
        {
            var _pers = this;
            var beforeNotDone = valBefore.Where(n => n.Item1 == "квест" && Convert.ToDouble(n.Item3) < 99.99);
            var afrerDone = valAfter.Where(n => n.Item1 == "квест" && Convert.ToDouble(n.Item3) >= 99.99);
            var newDone = afrerDone.Join(beforeNotDone, i => i.Item2, o => o.Item2,
                (i, o) => _pers.Aims.First(n => n.GUID == i.Item2));
            List<Aim> qw = new List<Aim>();

            foreach (var donnedQwests in
                newDone)
            {
                if (isShow)
                {
                    var lv = new LevelsChangesView();
                    var header = $"Все миссии квеста \"{donnedQwests.NameOfProperty}\" выполнены!!!";
                    lv.Image.Source = donnedQwests.PictureProperty;
                    lv.down.Visibility = Visibility.Collapsed;
                    lv.Header.Text = header;
                    lv.btnOk.Click += (sender, args) =>
                    {
                        lv.Close();
                        //donnedQwests.BeginDate = MainViewModel.selectedTime;
                        //StaticMetods.editAim(donnedQwests);
                    };
                    lv.down.Visibility = Visibility.Collapsed;
                    lv.up.Visibility = Visibility.Collapsed;
                    lv.ShowDialog();

                    //donnedQwests.BeginDate = MainViewModel.selectedTime;
                    StaticMetods.editAim(donnedQwests);
                }
                qw.Add(donnedQwests);
            }

            return qw;
        }

        /// <summary>
        /// Обнуление некоторых параметров
        /// </summary>
        public void ToNullPers(bool isSoftBegin = false)
        {
            BookOfSuccess.Clear();
            _maxGettedLevel = 1;
            ExtraPoints = 0;
            PlusExpFromTasksProperty = 0;
            gold = 0;
            HitPoints = 100;
            HP = MaxHP;

            foreach (var abilitiModel in Abilitis)
            {
                abilitiModel.BuyedInThisLevel = false;
                abilitiModel.ClearValue();
                abilitiModel.FirstVal = 0;
                abilitiModel.ValueProperty = 0;
                abilitiModel.LastValue = 0;
            }

            if (!isSoftBegin)
            {
                Characteristics.ToList().ForEach(n => n.FirstVal = 0);
            }

            foreach (var abilitiModel in Abilitis)
            {
                AbilitiModel.ClearReqvirements(abilitiModel);
                abilitiModel.NeedsForLevels.Clear();
            }

            foreach (var n in Characteristics.ToList())
            {
                n.LastValue = n.FirstVal;
                n.RecountChaValue();
            }

            foreach (var task in Tasks)
            {
                task.GetEnamyImage();
                task.Rage = 0;
            }

            PersExpFromeTasksAndQwests = 0;
            ExpBuff = 0;
            Heal();

            //ChaLevAndValues.ChaLevAndValuesList.Clear();
            //RecountPlusAbPoints();

            foreach (var task in this.Tasks)
            {
                task.EndDate = MainViewModel.selectedTime;
            }

            IsParetto = false;
            IsSortByPrioryty = false;
            NewRecountExp();
            StaticMetods.Locator.MainVM.RefreshTasksPriority(true);
        }

        /// <summary>
        /// Обновить очки скиллов
        /// </summary>
        public void UpdateAbilityPoints()
        {
            OnPropertyChanged(nameof(AbilitisPoints));
            foreach (var abilitiModel in Abilitis)
            {
                abilitiModel.RefreshAbBuySale();
            }
        }

        public void updateDateString()
        {
            OnPropertyChanged(nameof(ShortDateString));
        }

        protected override BitmapImage GetDefoultPic()
        {
            return new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Images", "лего друид.jpg")));
        }

        /// <summary>
        /// Изменение значений скиллов персонажа с изменениями уровней персонажа
        /// </summary>
        /// <param name="beforeLevel"></param>
        /// <param name="afterLevel"></param>
        /// <param name="prs"></param>
        /// <param name="absToEdit"></param>
        /// <returns></returns>
        private static List<viewChangesModel> ChangeAbilitiesWithChangePersLevels(int beforeLevel, int afterLevel,
            Pers prs, List<AbilitiModel> absToEdit)
        {
            return new List<viewChangesModel>();
            //var vcmAb = new List<viewChangesModel>();
            //if (afterLevel > beforeLevel)
            //{
            //    ChangeAbilitisWhenPersLevelUp(prs, absToEdit, vcmAb);
            //}
            //else
            //{
            //    ChangeAbilitisWhenPersLevelDown(beforeLevel, prs, absToEdit, vcmAb);
            //}
            //return vcmAb;
        }

        private static void ChangesGold(List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter, List<viewChangesModel> viewChanges, out int beforeGold,
            out int afterGold)
        {
            beforeGold = Convert.ToInt32(valBefore.First(n => n.Item1 == "золото").Item3);
            afterGold = Convert.ToInt32(valAfter.First(n => n.Item1 == "золото").Item3);
            if (beforeGold != afterGold)
            {
                var viewChangesModel = new viewChangesModel(
                    string.Empty,
                    "Золото",
                    Colors.Gold.ToString(),
                    beforeGold,
                    afterGold,
                    null)
                {
                    ImageProperty = GoldImageProperty,
                    ChangeProperty = afterGold - beforeGold,
                    MinValueProperty = 0,
                    MaxValueProperty = afterGold > beforeGold ? afterGold : beforeGold,
                    Цвет = Brushes.Transparent.ToString(),
                    RangProperty = $"{StaticMetods.PersProperty.GoldProperty} золотых"
                };

                SetChangesNameAndColor(viewChangesModel, viewChangesModel.ChangeProperty);

                viewChangesModel.RangProperty = $"";
                viewChangesModel.RangProperty2 = $" золотых";
                viewChangesModel.IsValVisibleProperty = Visibility.Visible;

                viewChanges.Add(viewChangesModel);
            }
        }

        private static void ChangesInventory(List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter, List<viewChangesModel> viewChanges, List<Revard> allRevards)
        {
            var beff = valBefore.Where(n => n.Item1 == "предмет").ToList();
            var aff = valAfter.Where(n => n.Item1 == "предмет").ToList();

            var addedRev = new List<Tuple<string, string, string>>(); // Добавилось
            var delatedRev = new List<Tuple<string, string, string>>(); // Удалилось

            var res = aff.Select(n => new { Key = n, Val = 1 })
                .Concat(beff.Select(n => new { Key = n, Val = -1 }))
                .GroupBy(n => n.Key, n => n.Val)
                .SelectMany(g => Enumerable.Repeat(g.Key, Math.Max(0, g.Sum())))
                .ToList();

            var res2 = beff.Select(n => new { Key = n, Val = 1 })
                .Concat(aff.Select(n => new { Key = n, Val = -1 }))
                .GroupBy(n => n.Key, n => n.Val)
                .SelectMany(g => Enumerable.Repeat(g.Key, Math.Max(0, g.Sum())))
                .ToList();

            foreach (var af in res)
            {
                addedRev.Add(new Tuple<string, string, string>("предмет", af.Item2, "1"));
            }

            // Удалилось
            foreach (var bf in res2)
            {
                delatedRev.Add(new Tuple<string, string, string>("предмет", bf.Item2, "1"));
            }

            foreach (var rev in addedRev)
            {
                var inv = allRevards.First(n => n.GUID == rev.Item2);
                var vm = new viewChangesModel(string.Empty, $"{inv.GetTypeOfRevard()} \"{inv.NameOfProperty}\"!!!",
                    Colors.Gold.ToString(), 0,
                    1,
                    null);
                vm.@from = 0;
                vm.to = 1;
                vm.ChangeProperty = vm.to - vm.@from;
                vm.MinValueProperty = 0;
                vm.MaxValueProperty = 1;
                vm.ImageProperty = inv.ImageProperty;
                vm.RangProperty = "";
                vm.RangProperty2 = "";
                vm.IsValVisibleProperty = Visibility.Collapsed;
                vm.ChangeString = "";
                viewChanges.Add(vm);
            }

            foreach (var rev in delatedRev)
            {
                var inv = allRevards.First(n => n.GUID == rev.Item2);
                var vm = new viewChangesModel(string.Empty, $"{inv.GetTypeOfRevard()} \"{inv.NameOfProperty}\"!!!",
                    Colors.Gold.ToString(), 0,
                    1,
                    null);
                vm.@from = 1;
                vm.to = 0;
                vm.ChangeProperty = vm.to - vm.@from;
                vm.MinValueProperty = 0;
                vm.MaxValueProperty = 1;
                vm.ImageProperty = inv.ImageProperty;
                vm.RangProperty = "";
                vm.RangProperty2 = "";
                vm.IsValVisibleProperty = Visibility.Collapsed;
                vm.ChangeString = "";
                viewChanges.Add(vm);
            }
        }

        /// <summary>
        /// Задать настройки программы в соответствие с настройками персонажа
        /// </summary>
        /// <param name="persParam">Персонаж</param>
        private static void SetPersSettings(Pers persParam)
        {
            var settingsPers = persParam.PersSettings;
            var progSettings = Settings.Default;
            progSettings.showSplashes = settingsPers.ShowSplashesProperty;
            progSettings.showUserSplashes = settingsPers.ShowSplashesFromFolderProperty;
            progSettings.ShowFileSplashName = settingsPers.ShowFileSplashNameProperty;
            progSettings.timeShowSplash = settingsPers.TimeShowSplashProperty;
            progSettings.LevelsInStar = settingsPers.LevelsInStarProperty;
            progSettings.MaxChaLevel = settingsPers.MaxCharactLevelProperty;
            progSettings.MaxAbilLevel = settingsPers.AbilMaxLevelProperty;
            progSettings.isPassProtect = settingsPers.IsPassProtectProperty;
            progSettings.Pass = settingsPers.PassProperty;
            progSettings.Save();
        }

        private static void ShowAbChaChengesWithNewLevel(ViewChangesClass vc, bool isUpLevel, string header)
        {
            vc.GetValAfter();

            Brush col = isUpLevel ? Brushes.Green : Brushes.Red;

            var itemImageProperty =
                StaticMetods.pathToImage(isUpLevel
                    ? Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png")
                    : Path.Combine(Directory.GetCurrentDirectory(), "Images", "bad.png"));

            if (isUpLevel)
            {
                StaticMetods.PlaySound(Resources.abLevelUp);
            }

            if (isUpLevel == false)
            {
                StaticMetods.PlaySound(Resources.Fail);
            }

            vc.ShowChanges(header, col, itemImageProperty, MainViewModel.selectedTime.ToShortDateString());
        }

        //private static List<AbilitiModel> GetChangesByNewLevel(List<viewChangesModel> viewChanges, int beforeLevel,
        //    int afterLevel, Pers prs)
        //{
        //    var absToEdit = new List<AbilitiModel>();

        // //#region КЗ

        // //var beforeAbPoints = prs.GetAbPoints(beforeLevel); //var afterAbPoints =
        // prs.GetAbPoints(afterLevel); //var pointsChanges = new viewChangesModel( // String.Empty,
        // // "Кристаллы знаний:", // Colors.Gold.ToString(), // beforeAbPoints, // afterAbPoints, //
        // null) //{ // ImageProperty = //
        // StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", //
        // "diamond.png")), // ChangeProperty = afterAbPoints - beforeAbPoints, // MinValueProperty =
        // 0, // MaxValueProperty = afterAbPoints > beforeAbPoints ? afterAbPoints : beforeAbPoints,
        // // Цвет = Brushes.Transparent.ToString() //}; //var isChangePlus =
        // pointsChanges.ChangeProperty > 0; //var pre = isChangePlus ? "+" : "";
        // //pointsChanges.ChangeString = $"{pre}{pointsChanges.ChangeProperty}";
        // //pointsChanges.RangProperty = $""; //pointsChanges.RangProperty2 = $"";
        // //pointsChanges.IsValVisibleProperty = Visibility.Visible; //#endregion КЗ

        // #region Характеристики

        // var chaBefore = prs.Characteristics.Select(n => new {cha = n, val =
        // n.CellValue}).ToList(); var vcmAb = ChangeAbilitiesWithChangePersLevels(beforeLevel,
        // afterLevel, prs, absToEdit); foreach (var characteristic in prs.Characteristics) {
        // characteristic.ValueProperty = characteristic.GetChaValue(); } var changeCharacts = (from
        // characteristic in prs.Characteristics join vb in chaBefore on characteristic.GUID equals
        // vb.cha.GUID select new {cha = characteristic, before = vb.val, after =
        // characteristic.CellValue}) .Where(n => n.after != n.before);

        // var chaChenge = ( from chengeCha in changeCharacts.ToList() let fromLev = chengeCha.before
        // let toLev = chengeCha.after let characteristic = chengeCha.cha select new
        // viewChangesModel( "Характеристика", $"{characteristic.NameOfProperty}:",
        // characteristic.Cvet, chengeCha.before, chengeCha.after, characteristic.Rangs) { @from =
        // fromLev, to = toLev, ChangeProperty = toLev - fromLev, MinValueProperty = 0.0,
        // //characteristic.ValueMinDouble, MaxValueProperty = 5.0, //characteristic.ValueMaxDouble,
        // ImageProperty = characteristic.ImageProperty, RangProperty =
        // $"{characteristic.RangName.ToLower()}", RangProperty2 = "", IsValVisibleProperty =
        // Visibility.Collapsed }).ToList();

        // foreach (var vm in chaChenge) { vm.ChangeString = ""; //viewChangesModel.ChangeProperty >
        // 0? $"+": $"-"; }

        // var vcmCha = new List<viewChangesModel>(); vcmCha.AddRange(chaChenge.OrderBy(n => n.to));
        // //.Where(n => Math.Abs(n.ChangeProperty) >= 0.001)

        // #endregion Характеристики

        // viewChanges.AddRange(vcmCha); viewChanges.AddRange(vcmAb); //viewChanges.Add(pointsChanges);

        // var rv = QwestsViewModel.AddArtefacts(StaticMetods.PersProperty); GetArtefactChanges(ref
        // viewChanges, rv); Messenger.Default.Send(viewChanges);

        //    return absToEdit;
        //}
        private static void ShowDownCongrant(List<viewChangesModel> viewChanges)
        {
            var congrant = new Congranteletions();
            congrant.btnClose.Click += (o, args) => { congrant.Close(); };

            Messenger.Default.Send<Window>(congrant);
            congrant.Topmost = true;
            Messenger.Default.Send(viewChanges);
            congrant.txtHeader.Text = "FAIL!!! \n :-(";
            congrant.txtMessege.Text = "Уровень понижен!";
            congrant.imgImage.Visibility = Visibility.Visible;
            congrant.imgImage.Source =
                StaticMetods.getImagePropertyFromImage(
                    StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "bad.png")));
            congrant.storyPanel.Visibility = Visibility.Collapsed;

            // Добавляем хоткей
            var saveCommand3 = new RelayCommand(() => { congrant.Close(); });
            congrant.InputBindings.Add(new KeyBinding(saveCommand3, new KeyGesture(Key.Space)));
            congrant.InputBindings.Add(new KeyBinding(saveCommand3, new KeyGesture(Key.Return)));
            congrant.ShowDialog();
        }

        private static void ShowUpCongrant(List<viewChangesModel> viewChanges, IOrderedEnumerable<Rangs> sortedRangs,
            int beforeLevel, int afterLevel)
        {
            var prs = StaticMetods.PersProperty;
            viewChanges.Clear();
            StaticMetods.PlaySound(Resources.NewLevel);
            var congrant = new Congranteletions();
            congrant.btnClose.Click += (o, args) => { congrant.Close(); };
            Messenger.Default.Send<Window>(congrant);
            congrant.Topmost = true;
            if (afterLevel > beforeLevel)
            {
                congrant.txtHeader.Foreground = Brushes.ForestGreen;
                congrant.txtMessege.Foreground = Brushes.ForestGreen;
                congrant.txtMessege.Text = $"Вы достигли {afterLevel}-го уровня!!!";
            }
            else
            {
                congrant.txtHeader.Foreground = Brushes.Red;
                congrant.txtMessege.Foreground = Brushes.Red;
                congrant.txtHeader.Text = "FAIL!!! \n :-(";
                congrant.txtMessege.Text = "Уровень понижен!";
                congrant.imgImage.Source =
                    StaticMetods.getImagePropertyFromImage(
                        StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "bad.png")));
                prs.IsSleepForNewLevel = false;
            }

            if (StaticMetods.PersProperty.PersSettings.IsActtivateRangse)
            {
                congrant.storyPanel.Visibility = Visibility.Visible;
                var before = sortedRangs.FirstOrDefault(n => n.LevelRang <= beforeLevel);
                var after = sortedRangs.LastOrDefault(n => n.LevelRang > beforeLevel);
                if ((before == null && after == null) || after == null)
                {
                    congrant.storyPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (before != null)
                    {
                        congrant.storyBeforeRang.Text = before.NameOfRang;
                        congrant.storyBeforeImage.Source = StaticMetods.getImagePropertyFromImage(before.ImageProperty);
                        congrant.storyProgress.Minimum = before.LevelRang >= 1 ? before.LevelRang : 1;
                    }
                    else
                    {
                        congrant.storyBeforeRang.Text = "";
                        //congrant.storyBeforeImage.Source = StaticMetods.getImagePropertyFromImage(before.ImageProperty);
                        congrant.storyProgress.Minimum = 1;
                    }
                    if (after != null)
                    {
                        congrant.storyAfterRang.Text = after.NameOfRang;
                        congrant.storyAfterImage.Source = StaticMetods.getImagePropertyFromImage(after.ImageProperty);
                        congrant.storyProgress.Maximum = after.LevelRang;
                    }
                    else
                    {
                        congrant.storyAfterRang.Text = "";
                        //congrant.storyAfterImage.Source = StaticMetods.getImagePropertyFromImage(after.ImageProperty);
                        congrant.storyProgress.Maximum = before?.LevelRang ?? 0;
                    }

                    congrant.DoubleAnimation.From = beforeLevel;
                    congrant.DoubleAnimation.To = afterLevel;
                }
            }
            else
            {
                congrant.storyPanel.Visibility = Visibility.Collapsed;
            }

            // Изменения с уровнем
            //GetChangesByNewLevel(viewChanges, beforeLevel, afterLevel, prs);
            //-----------------

            // Добавляем хоткей
            var saveCommand = new RelayCommand(() => { congrant.Close(); });
            congrant.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Space)));
            congrant.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Return)));
            congrant.UcViewChangesView.Visibility = Visibility.Visible;
            congrant.ShowDialog();

            //foreach (var abilitiModel in abs)
            //{
            //    abilitiModel.EditAbility();
            //}
        }

        /// <summary>
        /// Изменения скиллов
        /// </summary>
        /// <param name="valBefore"></param>
        private void AbChanges(List<Tuple<string, string, string>> valBefore)
        {
            if (PersSettings.IsNoAbs)
            {
                return;
            }

            valBefore.AddRange(
                from abiliti in Abilitis
                let payedLevelProperty =
                    abiliti.CellValue
                select new Tuple<string, string, string>("навык", abiliti.GUID, payedLevelProperty.ToString()));

            //var g = Abilitis.FirstOrDefault(n => n.NameOfProperty == "Общительность");
            //var cc = valBefore.Where(n => n.Item2 == g.GUID);

            // Значения навыков
            valBefore.AddRange(
                from abiliti in Abilitis
                select new Tuple<string, string, string>("навыкЗ", abiliti.GUID + "з", abiliti.ValueProperty.ToString()));
        }

        /// <summary>
        /// Очки скиллов за выполненные квесты
        /// </summary>
        /// <returns></returns>
        private int AbPointsFromQwests()
        {
            var sum = 0;
            foreach (var aim in Aims.Where(n => n.IsDoneProperty))
            {
                switch (aim.HardnessProperty)
                {
                    case 0:
                        sum += 0;
                        break;

                    case 1:
                        sum += PersSettings.AbPointsForEasyQwest;
                        break;

                    case 2:
                        sum += PersSettings.AbPointsForNormalQwest;
                        break;

                    case 3:
                        sum += PersSettings.AbPointsForHardQwest;
                        break;

                    case 4:
                        sum += PersSettings.AbPointsForVeryHardQwest;
                        break;
                }
            }

            return sum;
        }

        private void ChaChenges(List<Tuple<string, string, string>> valBefore)
        {
            var collection = from characteristic1 in Characteristics
                             select
                                 new Tuple<string, string, string>(
                                     "характеристика",
                                     characteristic1.GUID,
                                     characteristic1.CellValue.ToString());

            valBefore.AddRange(
                collection);

            // Значения характеристик
            var collection2 = from ch in Characteristics
                              select new Tuple<string, string, string>("характеристикаЗ", ch.GUID + "з", Math.Round(ch.ValueProperty, 2).ToString());

            valBefore.AddRange(
                collection2);
        }

        private void ChangesExp(List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter, List<viewChangesModel> viewChanges, out double beforeExp,
            out double afterExp,
            Pers pers)
        {
            beforeExp = Convert.ToDouble(valBefore.First(n => n.Item1 == "опыт").Item3);
            //afterExp = Convert.ToDouble(valAfter.First(n => n.Item1 == "опыт").Item3);
            afterExp = PersExpProperty;

            if (Math.Abs(beforeExp - afterExp) >= 1.0)
            {
                string str = $"Уровень ";
                var viewChangesModel = new viewChangesModel(
                    "Опыт",
                    str,
                    Colors.Gold.ToString(),
                    Convert.ToDouble(beforeExp),
                    Convert.ToDouble(afterExp),
                    pers.Rangs)
                {
                    ImageProperty = ExpImageProperty,
                    @from = Convert.ToDouble(beforeExp),
                    to = Convert.ToDouble(afterExp),
                    MinValueProperty = ExpToLevel(PersLevelProperty, RpgItemsTypes.exp),
                    MaxValueProperty = ExpToLevel(PersLevelProperty + 1, RpgItemsTypes.exp)
                };

                SetChangesNameAndColor(viewChangesModel, viewChangesModel.ChangeProperty);
                // Минимум, максимум и уровень надо пересчитать здесь
                viewChangesModel.ChangeProperty = Convert.ToInt32(viewChangesModel.ChangeProperty);

                viewChangesModel.RangProperty2 = $"/{ValueMaxProperty}";
                viewChangesModel.IsValVisibleProperty = Visibility.Visible;

                viewChanges.Add(viewChangesModel);
            }
        }

        private void ChangesHP(List<Tuple<string, string, string>> valBefore,
            List<Tuple<string, string, string>> valAfter, List<viewChangesModel> viewChanges, Pers pers)
        {
            var beforeHP = Convert.ToInt32(valBefore.First(n => n.Item2 == "здоровье").Item3);
            var afterFOrD = valAfter.FirstOrDefault(n => n.Item2 == "здоровье");
            if (afterFOrD != null)
            {
                var afterHP = Convert.ToInt32(afterFOrD.Item3);

                if (Math.Abs(beforeHP - afterHP) >= 1.0)
                {
                    var viewChangesModel = new viewChangesModel(
                        string.Empty,
                        "",
                        Brushes.LightCoral.ToString(),
                        Convert.ToDouble(beforeHP),
                        Convert.ToDouble(afterHP),
                        pers.Rangs)
                    {
                        ImageProperty =
                            StaticMetods.pathToImage(
                                Path.Combine(Directory.GetCurrentDirectory(), "Images", "HP.png")),
                        IsValVisibleProperty = Visibility.Visible,
                        IsToVisible = Visibility.Visible,
                        IsRangVisibleProperty = Visibility.Collapsed,
                        @from = Convert.ToDouble(beforeHP),
                        to = Convert.ToDouble(afterHP),
                        MinValueProperty = 0,
                        MaxValueProperty = pers.MaxHPIneger
                    };

                    SetChangesNameAndColor(viewChangesModel, viewChangesModel.ChangeProperty,
                        Colors.LightCoral.ToString(), Colors.LightCoral.ToString());
                    viewChangesModel.ChangeProperty = Convert.ToInt32(viewChangesModel.ChangeProperty);
                    viewChangesModel.RangProperty = $"{HPOrDmgString}: ";
                    viewChanges.Add(viewChangesModel);
                }
            }
        }

        private void ExpChanges(List<Tuple<string, string, string>> valBefore)
        {
            valBefore.Add(new Tuple<string, string, string>("опыт", string.Empty, PersExpProperty.ToString()));
        }

        private ObservableCollection<Rangs> getDefoultPersRangse()
        {
            rangs = new ObservableCollection<Rangs>();
            rangs.Add(new Rangs { NameOfRang = "Обыватель" });
            rangs.Add(new Rangs { NameOfRang = "Искатель приключений" });
            rangs.Add(new Rangs { NameOfRang = "Падаван" });
            rangs.Add(new Rangs { NameOfRang = "Джедай" });
            rangs.Add(new Rangs { NameOfRang = "Герой" });
            rangs.Add(new Rangs { NameOfRang = "Легенда" });
            return rangs;
        }

        /// <summary>
        /// Получаем значение здоровья для изменений
        /// </summary>
        /// <returns></returns>
        private string getHPToChanges(bool isText)
        {
            return HPIneger.ToString();
        }

        //CurRang.LevelRang;
        private Rangs GetRang(int lev)
        {
            var sortedRangs = Rangs.OrderByDescending(n => n.LevelRang);
            var firstOrDefault = sortedRangs.FirstOrDefault(n => n.LevelRang <= lev);
            return firstOrDefault;
        }

        private void GetRangsChanges(List<viewChangesModel> viewChanges, double beforeExp, double afterExp, Pers pers)
        {
            var beforelev = StaticMetods.GetLevel(beforeExp, RpgItemsTypes.exp);
            var afterLev = StaticMetods.GetLevel(afterExp, RpgItemsTypes.exp);

            var sortedRangs = Rangs.OrderByDescending(n => n.LevelRang);
            var before = sortedRangs.FirstOrDefault(n => n.LevelRang <= beforelev);
            var after = sortedRangs.LastOrDefault(n => n.LevelRang > beforelev);

            double minimum = 0;
            double maximum = 1;

            if ((before != null || after != null) && after != null)
            {
                if (before != null)
                {
                    minimum = before.LevelRang >= 1 ? before.LevelRang : 1;
                }
                else
                {
                    minimum = 1;
                }
                if (after != null)
                {
                    maximum = after.LevelRang;
                }
            }

            if (Math.Abs(beforelev - afterLev) >= 1.0)
            {
                string str = string.Empty;
                if (PersSettings.IsActtivateRangse && CurRang != null)
                {
                    str += $"{CurRang.NameOfRang}";
                }

                var viewChangesModel = new viewChangesModel(
                    "Ранг",
                    str,
                    Colors.Gold.ToString(),
                    Convert.ToDouble(beforelev),
                    Convert.ToDouble(afterLev),
                    pers.Rangs)
                {
                    ImageProperty = LevelImageProperty,
                    @from = Convert.ToDouble(beforelev),
                    to = Convert.ToDouble(afterLev),
                    MinValueProperty = minimum,
                    MaxValueProperty = maximum
                };

                SetChangesNameAndColor(viewChangesModel, viewChangesModel.ChangeProperty);
                viewChangesModel.ChangeString = string.Empty;
                viewChangesModel.ChangeProperty = Convert.ToInt32(viewChangesModel.ChangeProperty);

                viewChangesModel.IsValVisibleProperty = Visibility.Collapsed;

                viewChanges.Add(viewChangesModel);
            }
        }

        private void GoldChanges(List<Tuple<string, string, string>> valBefore)
        {
            valBefore.Add(new Tuple<string, string, string>("золото", string.Empty, GoldProperty.ToString()));
        }

        private void HPChanges(List<Tuple<string, string, string>> valBefore)
        {
            valBefore.Add(
                new Tuple<string, string, string>(getHPToChanges(true), "здоровье", getHPToChanges(false)));
        }

        /// <summary>
        /// Добавляем экстра очки скиллов, если все уже прокачано!
        /// </summary>
        /// <param name="abPoints"></param>
        private void IfAllAbsUpp()
        {
            var ab = GetAbPoints(PersLevelProperty) + ExtraPoints;
            var isAllAbUp = Abilitis.Where(n => n.CellValue > 0).All(n => n.IsAllNeedsComplete);

            var minCost = Abilitis.Min(n => n.CostProperty);

            if (isAllAbUp && ab < minCost)
            {
                ExtraPoints += minCost - ab;
            }
        }

        private void QwestChanges(List<Tuple<string, string, string>> valBefore)
        {
            valBefore.AddRange(
                from aim1 in Aims
                let progress = aim1.AutoProgressValueProperty
                select new Tuple<string, string, string>("квест", aim1.GUID, progress.ToString()));
        }

        /// <summary>
        /// Пересчитываем все видимые значения уровней характеристик и скиллов
        /// </summary>
        private void RecountVisibleLevelsValues()
        {
            foreach (var characteristic in Characteristics)
            {
                characteristic.CountVisibleLevelValue();
            }

            foreach (var abilitiModel in Abilitis)
            {
                abilitiModel.CountVisibleLevelValue();
            }
        }

        /// <summary>
        /// Обновить функциональные возможности программы
        /// </summary>
        private void RefreshProgramFunctions()
        {
            if (PersLevelProperty < 10)
            {
                IsRewardsEnabled = false;
                IsQwestsEnabled = false;
                IsMapsEnabled = false;
            }
            else if (PersLevelProperty < 20)
            {
                IsRewardsEnabled = false;
                IsMapsEnabled = false;
            }
            else if (PersLevelProperty < 30)
            {
                IsMapsEnabled = false;
            }

            if (PersLevelProperty >= 10)
            {
                IsQwestsEnabled = true;
            }
            else if (PersLevelProperty >= 20)
            {
                IsQwestsEnabled = true;
                IsRewardsEnabled = true;
            }
            else if (PersLevelProperty >= 30)
            {
                IsRewardsEnabled = true;
                IsQwestsEnabled = true;
                IsMapsEnabled = true;
            }
        }

        //private void ShowChangeLevelsOfCharacts()
        //{
        //    // Показываем изменения уровней характеристик
        //    var vc2 = new ViewChangesClass(InventoryItems.Union(ShopItems).ToList());
        //    vc2.GetValBefore();
        //    foreach (var characteristic in Characteristics)
        //    {
        //        characteristic.ValueProperty = characteristic.GetChaValue(true);
        //    }
        //    vc2.GetValAfter();
        //    ShowChangeChaLevels(vc2.ValBefore, vc2.ValAfter);
        //}

        private void ShowRangChange()
        {
            var nr = new NewRang();

            nr.txtHeader.Text = $"Получено новое звание - \''{CurRang.NameOfRang}\''!!!";
            nr.img.Source =
                StaticMetods.getImagePropertyFromImage(
                    StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png")));
            nr.MediaElement.LoadedBehavior = MediaState.Manual;
            nr.MediaElement.Source =
                new Uri(Path.Combine(Environment.CurrentDirectory, "Images", "Congrant.mp3"));

            if (!PersSettings.DisableSounds)
            {
                nr.MediaElement.Play();
            }

            // Добавляем хоткей
            var saveCommand2 = new RelayCommand(() => { nr.Close(); });
            nr.InputBindings.Add(new KeyBinding(saveCommand2, new KeyGesture(Key.Space)));
            nr.InputBindings.Add(new KeyBinding(saveCommand2, new KeyGesture(Key.Return)));

            nr.ShowDialog();
        }
    }

    [Serializable]
    public class RandomeImageGenerator
    {
        //private ImagesAndPositions _lowEnamies;
        //private ImagesAndPositions _normEnamies;
        //private ImagesAndPositions _hiEnamies;
        //private ImagesAndPositions _maxEnamies;
        private List<ImagesAndPositions> _imagesAndPositions;

        /// <summary>
        /// Картинки и их позиции
        /// </summary>
        public List<ImagesAndPositions> ImagesAndPositions
        {
            get
            {
                if (_imagesAndPositions == null)
                {
                    _imagesAndPositions = new List<ImagesAndPositions>();
                }
                return _imagesAndPositions;
            }
            set
            {
                _imagesAndPositions = value;
            }
        }

        public int MaxMonstersLev { get; set; }

        /// <summary>
        /// Заполнить
        /// </summary>
        /// <param name="level"></param>
        /// <param name="imgs"></param>
        public void FillImages(int level, ImagesAndPositions imgs)
        {
            //MaxMonstersLev = StaticMetods.MaxPersAndMonstersRangs;
            imgs.Images = new List<RIGImage>();
            //var l = level*(5.0/StaticMetods.MaxAbLevel);
            //var frome = (int)Math.Floor(l);
            //if (level == 1) frome = 1;
            //var to = (int)Math.Ceiling(l);

            //var l = 5 - StaticMetods.MaxAbLevel;
            //int frome;
            //if (level == 1) frome = 1;
            //else
            //{
            //    frome = level + l;
            //    if (frome < 1) frome = 1;
            //}
            //var to = level + l;
            //if (to < 1) to = 1;

            //for (int i = frome; i <= to; i++)
            //{
            //    var path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Enamies", i.ToString()));
            //    allFiles.AddRange(Directory.EnumerateFiles(path));
            //}

            var allImg = new List<RIGImage>();
            var dir = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Enamies"));
            var orderedEnumerable = Directory.EnumerateDirectories(dir).OrderBy(n => n).ToList();
            foreach (var directory in orderedEnumerable)
            {
                var af = Directory.EnumerateFiles(directory);
                var files =
                    af
                        .Where(n => n.EndsWith(".jpg") || n.EndsWith(".png"))
                        .Select(
                            n =>
                                new RIGImage
                                {
                                    FolderName = level.ToString(),
                                    Guid = Guid.NewGuid().ToString(),
                                    NameOfFile = n
                                })
                        .OrderBy(n => n.Guid).Take(100)
                        .ToList();
                allImg.AddRange(files);
            }

            double imgCount = allImg.Count - 100;

            double progr = level / 90.0;
            if (progr > 1) progr = 1;

            int skip = (int)(imgCount * progr);

            var rigImages = allImg.Skip(skip).Take(100);
            var orderBy = rigImages.OrderBy(n => Guid.NewGuid().ToString()).ToList();
            imgs.Images.AddRange(orderBy);
            imgs.Position = 0;
            imgs.Level = level;
            ImagesAndPositions.Add(imgs);
        }

        /// <summary>
        /// Получить путь к картинке
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GetNextImgPath(int level)
        {
            //var path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Enamies"));
            if (ImagesAndPositions.Count > 1)
            {
                ImagesAndPositions.Clear();
            }

            ImagesAndPositions imgs = ImagesAndPositions.FirstOrDefault(n => n.Level == level); //ImagesAndPositions.FirstOrDefault(n => n.Level == level);
            if (imgs == null)
            {
                imgs = new ImagesAndPositions();
                FillImages(level, imgs);
            }

            if (imgs.Images == null || !imgs.Images.Any() || imgs.Position >= imgs.Images.Count ||
                !File.Exists(imgs.Images[imgs.Position].NameOfFile))
            {
                ImagesAndPositions.Remove(imgs);
                imgs = new ImagesAndPositions();
                FillImages(level, imgs);
            }

            var nextImgPath = imgs.Images[imgs.Position].NameOfFile;
            imgs.Position++;
            return nextImgPath;
        }
    }

    [Serializable]
    public class RandomeImageGenerator2
    {
        public List<Stack<string>> ImgGens;

        public RandomeImageGenerator2()
        {
            ImgGens = new List<Stack<string>>
            {
                new Stack<string>(),
                new Stack<string>(),
                new Stack<string>(),
                new Stack<string>(),
                new Stack<string>(),
                new Stack<string>()
            };
        }

        private string enamiesDirectory => Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Enamies"));

        public string GetNextImage()
        {
            int lvlRang = StaticMetods.PersProperty.Rangs.IndexOf(StaticMetods.PersProperty.CurRang);

            if (!StaticMetods.PersProperty.BalanceIs50Levels)
            {
                if (StaticMetods.PersProperty.PersLevelProperty >= 90)
                {
                    // Максимальный уровень монстров
                    lvlRang = 5;
                }
            }
            else
            {
                if (StaticMetods.PersProperty.PersLevelProperty >= 45)
                {
                    // Максимальный уровень монстров
                    lvlRang = 5;
                }
            }

            //var max = StaticMetods.PersProperty.Rangs.Count;

            //var ost = StaticMetods.PersProperty.PersLevelProperty % 10;
            //var thisLvlChance = (10 - ost) / 10.0;
            //var nextLvlChanse = ost / 10.0;

            //var chance = MainViewModel.rnd.NextDouble();

            //if (chance > thisLvlChance && lvlRang < 6)
            //{
            //    lvlRang++;
            //}

            if (ImgGens[lvlRang].Count == 0)
            {
                FillImages(lvlRang);
            }

            string fname = ImgGens[lvlRang].Pop();

            if (!File.Exists(Path.Combine(enamiesDirectory, fname)))
            {
                FillImages(lvlRang);
                fname = ImgGens[lvlRang].Pop();
            }

            return fname;
        }

        private void FillImages(int lvlRang)
        {
            string dir = Path.Combine(enamiesDirectory, lvlRang.ToString());

            var af = Directory.EnumerateFiles(dir);

            var files = af.Where(n => n.EndsWith(".jpg") || n.EndsWith(".png"))
                .Select(q => Path.Combine(lvlRang.ToString(), (new FileInfo(q)).Name))
                    .OrderBy(n => Guid.NewGuid().ToString()).ToList();

            ImgGens[lvlRang] = new Stack<string>(files);
        }
    }

    [Serializable]
    public class RandomRevard
    {
        private int curItem;

        private List<int> listOfVers;

        public int GetRanVal()
        {
            curItem++;
            if (listOfVers == null || listOfVers.Count != 100 || curItem >= 100)
            {
                fillRandomes();
            }
            return listOfVers[curItem];
        }

        private void fillRandomes()
        {
            var list = new[]
            {
                new {guid = "", val = 1}
            }.ToList();
            list.Clear();
            for (int i = 1; i <= 100; i++)
            {
                list.Add(new { guid = Guid.NewGuid().ToString(), val = i });
            }
            listOfVers = list.OrderBy(n => n.guid).Select(n => n.val).ToList();
            curItem = 0;
        }
    }

    [Serializable]
    public class RIGImage
    {
        /// <summary>
        /// Название папки
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// Гуид
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string NameOfFile { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DotNetLead.DragDrop.UI.Behavior;
using Sample.ViewModel;

namespace Sample.Model
{
    public interface IHaveRevords
    {
        /// <summary>
        /// Для наград за элемент
        /// </summary>
        ucElementRewardsViewModel UcElementRewardsViewModel { get; }

        void RefreshRev();
    }

    /// <summary>
    /// Цель
    /// </summary>
    [Serializable]
    public class Aim : BaseRPGItem, IComparable<Aim>, IExpable, IDropable, IDragable, IHaveRevords
    {
        private ObservableCollection<AbilitiModel> _abilitiLinksOf;

        private ObservableCollection<Task> _linksOfTasks;

        private int _minLevelPropertyToDone;
        private ObservableCollection<NeedAbility> _needAbilities;

        private ObservableCollection<NeedAbility> _needAbilitiesToDone;

        private ObservableCollection<NeedCharact> _needCharacts;

        private ObservableCollection<NeedCharact> _needCharactsToDone;

        private string _notAllowReqwirements;

        private int _plusExp;

        private int _progressInt;

        private ObservableCollection<Task> _spells;

        private ObservableCollection<UpUbility> _upUbilitys = new ObservableCollection<UpUbility>();

        /// <summary>
        /// Значение автопрогресса.
        /// </summary>
        private double autoProgressValue;

        /// <summary>
        /// Дата начала квеста.
        /// </summary>
        private string dateOfBegin;

        /// <summary>
        /// Золото если квест выполнен.
        /// </summary>
        private int goldIfDone;

        /// <summary>
        /// Сложность квеста.
        /// </summary>
        private int hardness;

        /// <summary>
        /// Активна?.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Квест автоматически становится активным, если выполнены все необходимые требования.
        /// </summary>
        private bool isAutoActive;

        /// <summary>
        /// Цель завершена?.
        /// </summary>
        private bool isDone;

        /// <summary>
        /// Минимальный уровень для доступности цели.
        /// </summary>
        private int minLevel;

        /// <summary>
        /// Требования.
        /// </summary>
        private ObservableCollection<Aim> needs;

        /// <summary>
        /// Предыдущие/следующие квесты.
        /// </summary>
        [field: NonSerialized]
        private List<Aim> prevNextQwests;

        /// <summary>
        /// Статус - доступно, завершено, не доступно.
        /// </summary>
        private string status;

        /// <summary>
        /// Тип задачи по умолчанию.
        /// </summary>
        private TypeOfTask typeOfTaskDefoult;

        /// <summary>
        /// Виртуальный опыт для автосортировки квестов.
        /// </summary>
        private int virtualExp;

        public Aim()
        {
        }

        public Aim(Pers _pers)
        {
            _spells = new ObservableCollection<Task>();
            GUID = Guid.NewGuid().ToString();
            ImageProperty = DefoultPicsAndImages.DefoultQwestImage;
            CompositeAims = new ObservableCollection<CompositeAims>();
            IsDoneProperty = false;
            IsAutoActiveProperty = _pers.PersSettings.IsQwestAutoActiveForDefoultProperty;
            NameOfProperty = "Название квеста";
            IsFocusedProperty = true;
            DescriptionProperty = string.Empty;
            IsActiveProperty = true;
            Needs = new ObservableCollection<Aim>();
            StatusProperty = "0. Добавляется";
            NeedsTasks = new ObservableCollection<NeedTasks>();
            MinLevelProperty = 0;
            //MinLevelProperty = _pers.PersSettings.MaxLevelToNewQwestsProperty
            //    ? _pers.MaxLevelProperty
            //    : _pers.PersLevelProperty;

            _pers.Aims.Add(this);
        }

        /// <summary>
        /// Ссылки из квеста на скиллы
        /// </summary>
        public ObservableCollection<AbilitiModel> AbilitiLinksOf
        {
            get
            {
                return _abilitiLinksOf ?? (_abilitiLinksOf = new ObservableCollection<AbilitiModel>());
            }
            set
            {
                if (Equals(value, _abilitiLinksOf)) return;
                _abilitiLinksOf = value;
                OnPropertyChanged(nameof(AbilitiLinksOf));
            }
        }

        /// <summary>
        /// Активные миссии
        /// </summary>
        public List<NeedTasks> ActiveMissions
        {
            get
            {
                var activeMissions = (from needTaskse in NeedsTasks
                                      where needTaskse.TaskProperty.IsDelProperty == false
                                      select needTaskse).OrderBy(n => n.TaskProperty).Take(1).ToList();

                return
                    activeMissions;
            }
        }

        /// <summary>
        /// Активные скиллы
        /// </summary>
        public List<Task> ActiveSkills
        {
            get
            {
                var ab = (from spell in Spells
                          from abilitiModel in StaticMetods.PersProperty.Abilitis
                          from needTaskse in abilitiModel.NeedTasks
                          where needTaskse.TaskProperty == spell
                          where
                              needTaskse.LevelProperty >= abilitiModel.CellValue &&
                              needTaskse.ToLevelProperty <= abilitiModel.CellValue
                          select needTaskse.TaskProperty).Distinct().ToList();

                return ab;
            }
        }

        public List<Aim> AimComposite
        {
            get { return CompositeAims.Select(n => n.AimProperty).ToList(); }
        }

        /// <summary>
        /// Все квесты, входящие в состав этого
        /// </summary>
        public List<Aim> AllCompositeQwests
        {
            get
            {
                List<Aim> composite = new List<Aim>();
                var compose =
                    CompositeAims.Union(NeedAbilities.SelectMany(n => n.AbilProperty.NeedAims))
                        .Select(n => n.AimProperty);
                foreach (var compositeAimse in compose)
                {
                    GetCompAimss(compositeAimse, ref composite);
                    composite.Add(compositeAimse);
                }

                return composite;
            }
        }

        /// <summary>
        /// Все родительские квесты этого
        /// </summary>
        public List<Aim> AllParrentAims
        {
            get
            {
                List<Aim> composite = new List<Aim>();

                foreach (
                    var aiim in
                        StaticMetods.PersProperty.Aims.Where(n => n.CompositeAims.Any(q => q.AimProperty == this)))
                {
                    GetParAimss(aiim, ref composite);
                    composite.Add(aiim);
                }

                return composite;
            }
        }

        /// <summary>
        /// Все влияющие задачи (требования, напрямую, на характеристики или скиллы)
        /// </summary>
        public List<Task> AllRelTasks
        {
            get
            {
                var relToAim = RelTasks;

                return relToAim.ToList();
            }
        }

        /// <summary>
        /// Sets and gets Значение автопрогресса. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public double AutoProgressValueProperty
        {
            get
            {
                if (IsDoneProperty)
                {
                    return 100.0;
                }

                if (autoProgressValue < 0)
                {
                    return 0;
                }

                return autoProgressValue;
            }

            set
            {
                if (Math.Abs(autoProgressValue - value) < 0.01)
                {
                    return;
                }

                autoProgressValue = value;
                ChangeValuesOfRelaytedItems();
                OnPropertyChanged(nameof(AutoProgressValueProperty));
                OnPropertyChanged(nameof(IsDoneVisible));
                OnPropertyChanged(nameof(IsNextVisible));
                OnPropertyChanged(nameof(Percentage));
                RefreshProgInt();
            }
        }

        /// <summary>
        /// Дата начала квеста (формат даты)
        /// </summary>
        public DateTime BeginDate
        {
            get
            {
                DateTime begTime = string.IsNullOrEmpty(dateOfBegin) ? DateTime.MinValue : DateTime.Parse(dateOfBegin);
                return begTime;
            }
            set
            {
                if (dateOfBegin == value.ToString())
                {
                    return;
                }

                dateOfBegin = value.ToString();
                OnPropertyChanged(nameof(BeginDate));
            }
        }

        /// <summary>
        /// Квесты, которые входят в состав этого
        /// </summary>
        public ObservableCollection<CompositeAims> CompositeAims { get; set; }

        /// <summary>
        /// Цвет надписи
        /// </summary>
        public Brush ForeGroundd
        {
            get
            {
                if (!IsActiveProperty)
                    return Brushes.DarkSlateGray;

                if (!CompositeAims.Any() && !NeedsTasks.Any()
                    && !Skills.Any() && !NeedAbilitiesToDone.Any() && !NeedCharactsToDone.Any()
                    && MinLevelPropertyToDone == 0)
                    return Brushes.Red;

                return Brushes.DarkSlateGray;
            }
        }

        /// <summary>
        /// Sets and gets Золото если квест выполнен. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int GoldIfDoneProperty
        {
            get
            {
                return goldIfDone;
            }

            set
            {
                if (goldIfDone == value)
                {
                    return;
                }

                goldIfDone = value;
                OnPropertyChanged(nameof(GoldIfDoneProperty));
                RefreshRev();
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
                    case 0:
                        return "Нет";

                    case 1:
                        return "Легко";

                    case 2:
                        return "Норм";

                    case 3:
                        return "Сложно";

                    case 4:
                        return "Оч. сложно";

                    default:
                        return "Норм";
                }
            }
        }

        /// <summary>
        /// Sets and gets Сложность квеста. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int HardnessProperty
        {
            get
            {
                if (hardness == 0) hardness = 1;
                return hardness;
            }

            set
            {
                if (hardness == value)
                {
                    return;
                }

                hardness = value;

                var nAimsInAb = (from abilitiModel in StaticMetods.PersProperty.Abilitis
                                 from compositeAimse in abilitiModel.NeedAims
                                 where compositeAimse.AimProperty == this
                                 select compositeAimse).ToList();

                nAimsInAb.ForEach(n => n.RefreshKRel());

                OnPropertyChanged(nameof(HardnessProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активна?. Changes to that property's value raise the PropertyChanged event.
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
                StaticMetods.AbillitisRefresh(StaticMetods.PersProperty);
            }
        }

        /// <summary>
        /// Sets and gets Квест автоматически становится активным, если выполнены все необходимые
        /// требования. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsAutoActiveProperty
        {
            get
            {
                return isAutoActive;
            }

            set
            {
                if (isAutoActive == value)
                {
                    return;
                }

                isAutoActive = value;
                OnPropertyChanged(nameof(IsAutoActiveProperty));

                IsActiveProperty = isAutoActive;
                StaticMetods.RefreshAllQwests(StaticMetods.PersProperty, true, true, false);
            }
        }

        /// <summary>
        /// Sets and gets Цель завершена?. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsDoneProperty
        {
            get
            {
                return isDone;
            }

            set
            {
                if (isDone == value)
                {
                    return;
                }

                isDone = value;

                if (isDone)
                {
                    var timee = MainViewModel.selectedTime;
                    timee = timee.AddHours(DateTime.Now.Hour);
                    timee = timee.AddMinutes(DateTime.Now.Minute);
                    timee = timee.AddSeconds(DateTime.Now.Second);
                    BeginDate = timee;
                }

                OnPropertyChanged(nameof(IsDoneProperty));
                OnPropertyChanged(nameof(IsNextVisible));
            }
        }

        /// <summary>
        /// Видимость завершения Миссии
        /// </summary>
        public Visibility IsDoneVisible
        {
            get
            {
                return (AutoProgressValueProperty >= 99.9) || IsDoneProperty ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Видимость завершения Миссии
        /// </summary>
        public Visibility IsNextVisible
        {
            get
            {
                return Visibility.Visible;
                ////if (IsDoneProperty)
                ////{
                ////    return Visibility.Visible;
                ////}

                //if (IsDoneVisible == Visibility.Collapsed)
                //{
                //    return Visibility.Visible;
                //}

                //return Visibility.Collapsed;
            }
        }

        public Visibility IsReqVisibility
        {
            get { return string.IsNullOrEmpty(NotAllowReqwirements) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility IsStarVisible
        {
            get
            {
                if (IsDoneProperty)
                {
                    return Visibility.Collapsed;
                }
                return AutoProgressValueProperty >= 99.9 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Коффициент важности для расчета прогресса квеста
        /// </summary>
        public double KValOfAim => Convert.ToDouble(StaticMetods.GetMod(HardnessProperty));

        /// <summary>
        /// Ссылки на задачи
        /// </summary>
        public ObservableCollection<Task> LinksOfTasks
        {
            get { return _linksOfTasks ?? (_linksOfTasks = new ObservableCollection<Task>()); }
            set { _linksOfTasks = value; }
        }

        /// <summary>
        /// Только ссылки на активные задачи скиллов
        /// </summary>
        public List<Task> LinksOfTasksActive
        {
            get { return LinksOfTasks.Where(n => n.IsEnabled).ToList(); }
        }

        /// <summary>
        /// Sets and gets Минимальный уровень для доступности цели. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int MinLevelProperty
        {
            get
            {
                return minLevel;
            }

            set
            {
                if (value < 0) value = 0;
                if (value > StaticMetods.PersProperty.MaxLevelProperty)
                    value = StaticMetods.PersProperty.MaxLevelProperty;

                if (minLevel == value)
                    return;

                minLevel = value;
                OnPropertyChanged(nameof(MinLevelProperty));
                IsActiveProperty = AimsViewModel.IsQwestActive(this, StaticMetods.PersProperty);
            }
        }

        /// <summary>
        /// Уровень персонажа для выполнения квеста.
        /// </summary>
        public int MinLevelPropertyToDone
        {
            get => _minLevelPropertyToDone; set
            {
                if (value < 0) value = 0;
                if (value > StaticMetods.PersProperty.MaxLevelProperty)
                    value = StaticMetods.PersProperty.MaxLevelProperty;

                if (_minLevelPropertyToDone == value)
                    return;

                _minLevelPropertyToDone = value;
                OnPropertyChanged(nameof(MinLevelPropertyToDone));
            }
        }

        /// <summary>
        /// Требования уровня скиллов для доступности квеста
        /// </summary>
        public ObservableCollection<NeedAbility> NeedAbilities
        {
            get
            {
                return _needAbilities ?? (_needAbilities = new ObservableCollection<NeedAbility>());
            }
            set
            {
                if (Equals(value, _needAbilities)) return;
                _needAbilities = value;
                OnPropertyChanged(nameof(NeedAbilities));
            }
        }

        /// <summary>
        /// Требования уровня скиллов для завершения квестов
        /// </summary>
        public ObservableCollection<NeedAbility> NeedAbilitiesToDone
        {
            get
            {
                return _needAbilitiesToDone ?? (_needAbilitiesToDone = new ObservableCollection<NeedAbility>());
            }
            set
            {
                if (Equals(value, _needAbilitiesToDone)) return;
                _needAbilitiesToDone = value;
                OnPropertyChanged(nameof(NeedAbilitiesToDone));
            }
        }

        /// <summary>
        /// Требования характеристик для доступности квеста
        /// </summary>
        public ObservableCollection<NeedCharact> NeedCharacts
        {
            get
            {
                return _needCharacts ?? (_needCharacts = new ObservableCollection<NeedCharact>());
            }
            set
            {
                if (Equals(value, _needCharacts)) return;
                _needCharacts = value;
                OnPropertyChanged(nameof(NeedCharacts));
            }
        }

        /// <summary>
        /// Требования характеристик для завершения квеста
        /// </summary>
        public ObservableCollection<NeedCharact> NeedCharactsToDone
        {
            get
            {
                return _needCharactsToDone ?? (_needCharactsToDone = new ObservableCollection<NeedCharact>());
            }
            set
            {
                if (Equals(value, _needCharactsToDone)) return;
                _needCharactsToDone = value;
                OnPropertyChanged(nameof(NeedCharacts));
            }
        }

        /// <summary>
        /// Sets and gets Требования. Какие квесты надо выполнить перед этим? Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public ObservableCollection<Aim> Needs
        {
            get
            {
                return needs ?? (needs = new ObservableCollection<Aim>());
            }

            set
            {
                if (needs == value)
                {
                    return;
                }

                needs = value;
                OnPropertyChanged(nameof(Needs));
            }
        }

        /// <summary>
        /// Требования Задач
        /// </summary>
        public ObservableCollection<NeedTasks> NeedsTasks { get; set; }

        /// <summary>
        /// Описания требований для доступности квеста
        /// </summary>
        public string NotAllowReqwirements
        {
            get
            {
                return _notAllowReqwirements;
            }
            set
            {
                if (value == _notAllowReqwirements) return;
                _notAllowReqwirements = value;
                OnPropertyChanged(nameof(NotAllowReqwirements));
                OnPropertyChanged(nameof(IsReqVisibility));
            }
        }

        /// <summary>
        /// Прозрачность (Для квестов без миссий есть)
        /// </summary>
        public double Opacity
        {
            get
            {
                if (!IsActiveProperty)
                {
                    return 0.6;
                }
                return 1;
            }
        }

        /// <summary>
        /// Прогресс в процентах
        /// </summary>
        public double Percentage
        {
            get
            {
                double cur = AutoProgressValueProperty;
                double max = 100.0 - AutoProgressValueProperty;
                double progress = 100.0 * cur / max;
                return Math.Round(progress);
            }
        }

        /// <summary>
        /// Плюс к опыту при выполнении
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
        /// Sets and gets Предыдущие/следующие квесты. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public List<Aim> PrevNextQwestsProperty
        {
            get
            {
                return prevNextQwests;
            }

            set
            {
                if (prevNextQwests == value)
                {
                    return;
                }

                prevNextQwests = value;
                OnPropertyChanged(nameof(PrevNextQwestsProperty));
            }
        }

        /// <summary>
        /// Прогресс целый
        /// </summary>
        public int ProgressInt
        {
            get
            {
                return _progressInt;
            }
            set
            {
                if (value == _progressInt) return;
                _progressInt = value;
                OnPropertyChanged(nameof(ProgressInt));
            }
        }


        public void RefrDescr()
        {
            OnPropertyChanged(nameof(QwestPipBoyDescr));
        }

        public FlowDocument QwestPipBoyDescr
        {
            get
            {
                FlowDocument doc = new FlowDocument() { LineHeight = double.NaN};

                // Описание

                if (!string.IsNullOrEmpty(this.DescriptionProperty))
                    doc.Blocks.Add(new Paragraph(new Run(this.DescriptionProperty)));

                // Награды
                var abUps = (from abilitiModel in StaticMetods.PersProperty.Abilitis
                             from nA in abilitiModel.NeedAims
                             where nA.AimProperty == this && nA.KRel > 0
                             select new { Abiliti = abilitiModel, Change = nA.KRel }).ToList();

                List<Revard> revs = StaticMetods.PersProperty.ShopItems.Where(n => n.NeedQwests.Any(q => q == this)).ToList();

                if (revs.Any()
                    || abUps.Any())
                {
                    doc.Blocks.Add(new Paragraph(new Run($"Награды:")));

                    List lst = new List();

                    foreach (var item in revs)
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.NameOfProperty}"))));
                    }

                    foreach (var item in abUps)
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.Abiliti.NameOfProperty} + {item.Change}"))));
                    }

                    doc.Blocks.Add(lst);
                }

                SolidColorBrush red = Brushes.Red;

                // Требования
                if (Needs.Any(n => !n.IsDoneProperty)
                    || NeedAbilities.Any(n => n.ValueProperty > n.AbilProperty.ValueProperty)
                    || NeedCharacts.Any(n => n.ValueProperty > n.CharactProperty.ValueProperty)
                    || MinLevelProperty > StaticMetods.PersProperty.PersLevelProperty
                    || AbilitiLinksOf.Any())
                {
                    List lst = new List();

                    // Не выполненные требования
                    if (MinLevelProperty > StaticMetods.PersProperty.PersLevelProperty)
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"Уровень ≥ {MinLevelProperty}") { Foreground = red })));
                    }

                    foreach (var item in NeedCharacts.Where(n => n.ValueProperty > n.CharactProperty.ValueProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.CharactProperty.NameOfProperty} ≥ {item.ValueProperty}")) { Foreground = red }));
                    }

                    foreach (var item in NeedAbilities.Where(n => n.ValueProperty > n.AbilProperty.ValueProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.AbilProperty.NameOfProperty} ≥ {item.ValueProperty}")) { Foreground = red }));
                    }

                    foreach (var item in Needs.Where(n => !n.IsDoneProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.NameOfProperty}")) { Foreground = red }));
                    }

                    // Выполненные требования
                    if (MinLevelProperty <= StaticMetods.PersProperty.PersLevelProperty)
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"Уровень ≥ {MinLevelProperty}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    foreach (var item in NeedCharacts.Where(n => n.ValueProperty <= n.CharactProperty.ValueProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.CharactProperty.NameOfProperty} ≥ {item.ValueProperty}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    foreach (var item in NeedAbilities.Where(n => n.ValueProperty <= n.AbilProperty.ValueProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.AbilProperty.NameOfProperty} ≥ {item.ValueProperty}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    foreach (var item in Needs.Where(n => n.IsDoneProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.NameOfProperty}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    doc.Blocks.Add(lst);
                }
                // Если требования все выполнены, то условия
                else
                {
                    List lst = new List();

                    // Ссылки
                    foreach (var item in AbilitiLinksOf)
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.NameOfProperty}")) { Foreground = Brushes.Blue, TextDecorations = TextDecorations.Underline }));
                    }

                    // Не выполненные
                    if (StaticMetods.PersProperty.PersLevelProperty < MinLevelPropertyToDone)
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"Уровень персонажа ≥ {MinLevelPropertyToDone}"))));
                    }

                    foreach (var item in NeedCharactsToDone.Where(n => n.ValueProperty > n.CharactProperty.ValueProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.CharactProperty.NameOfProperty} ≥ {item.ValueProperty}"))));
                    }

                    foreach (var item in NeedAbilitiesToDone.Where(n => n.ValueProperty > n.AbilProperty.ValueProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.AbilProperty.NameOfProperty} ≥ {item.ValueProperty}"))));
                    }

                    foreach (var item in CompositeAims.Where(n => !n.AimProperty.isDone))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.AimProperty.NameOfProperty}"))));
                    }

                    foreach (var item in NeedsTasks.Where(n => !n.TaskProperty.IsDelProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.TaskProperty.NameOfProperty}"))));
                    }

                    // Выполненные
                    if (StaticMetods.PersProperty.PersLevelProperty >= MinLevelPropertyToDone && MinLevelPropertyToDone > 0)
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"Уровень персонажа ≥ {MinLevelPropertyToDone}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    foreach (var item in NeedCharactsToDone.Where(n => n.ValueProperty <= n.CharactProperty.ValueProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.CharactProperty.NameOfProperty} ≥ {item.ValueProperty}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    foreach (var item in NeedAbilitiesToDone.Where(n => n.ValueProperty <= n.AbilProperty.ValueProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.AbilProperty.NameOfProperty} ≥ {item.ValueProperty}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    foreach (var item in CompositeAims.Where(n => n.AimProperty.isDone))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.AimProperty.NameOfProperty}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    foreach (var item in NeedsTasks.Where(n => n.TaskProperty.IsDelProperty))
                    {
                        lst.ListItems.Add(new ListItem(new Paragraph(new Run($"{item.TaskProperty.NameOfProperty}") { TextDecorations = TextDecorations.Strikethrough })));
                    }

                    doc.Blocks.Add(lst);
                }

                return doc;
            }
        }

        public List<CompositeAims> QwNeedQwests
        {
            get { return CompositeAims.OrderBy(n => n.AimProperty).ToList(); }
        }

        public List<CompositeAims> QwNeedQwestsActive
        {
            get { return QwNeedQwests.Where(n => n.AimProperty.IsActiveProperty).ToList(); }
        }

        /// <summary>
        /// Влияющие на квест задачи (в требованиях или влияющие напрямую)
        /// </summary>
        public List<Task> RelTasks
        {
            get
            {
                var tasksInNeeds = NeedsTasks.Select(n => n.TaskProperty);
                var taskRelays = from task in StaticMetods.PersProperty.Tasks
                                 where task.RelToQwests.Any(n => n == this)
                                 select task;
                return tasksInNeeds.Union(taskRelays).ToList();
            }
        }

        public IEnumerable<AbilitiModel> RelToAbilitiModels
        {
            get
            {
                return (from abilitiModel in StaticMetods.PersProperty.Abilitis
                        from compositeAimse in abilitiModel.NeedAims
                        where compositeAimse.AimProperty == this
                        select new { abilitiModel, compositeAimse }).OrderByDescending(n => n.compositeAimse.KRel)
                    .Select(n => n.abilitiModel)
                    .Distinct();
            }
        }

        /// <summary>
        /// Задачи, которые в связанных с квестом скиллов
        /// </summary>
        public List<Task> Skills
        {
            get
            {
                //var abs = (from abilitiModel in StaticMetods.PersProperty.Abilitis
                //    from compositeAimse in abilitiModel.NeedAims
                //    where compositeAimse.LevelProperty == abilitiModel.PayedLevelProperty - 1
                //    where
                //        compositeAimse.AimProperty == this ||
                //        compositeAimse.AimProperty.AllCompositeQwests.Any(q => q == this)
                //    select abilitiModel).Distinct().ToList();

                //var skills = abs.SelectMany(n => n.SkillsActive).Distinct().ToList();

                return Spells.ToList();
            }
        }

        /// <summary>
        /// Скиллы, которые используются в этом квесте
        /// </summary>
        public ObservableCollection<Task> Spells
        {
            get { return _spells; }
            set { _spells = value; }
        }

        /// <summary>
        /// Sets and gets Статус - доступно, завершено, не доступно. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public string StatusProperty
        {
            get
            {
                return status;
            }

            set
            {
                if (status == value)
                {
                    return;
                }

                status = value;
                OnPropertyChanged(nameof(StatusProperty));
            }
        }

        /// <summary>
        /// Прогресс задач
        /// </summary>
        public double TasksProgress
        {
            get
            {
                if (!NeedsTasks.Any())
                {
                    return 100.0;
                }

                var tasksProgress = Convert.ToDouble(NeedsTasks.Sum(n => n.Progress)) / NeedsTasks.Count;
                return tasksProgress;
            }
        }

        public string ToolTip
                    => $"Квест: \"{NameOfProperty}\"\nПрогресс: {Math.Round(AutoProgressValueProperty, 0)}%";

        /// <summary>
        /// Sets and gets Тип задачи по умолчанию. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public TypeOfTask TypeOfTaskDefoultProperty
        {
            get
            {
                typeOfTaskDefoult = typeOfTaskDefoult ??
                                    (typeOfTaskDefoult = StaticMetods.PersProperty.PersSettings.DefoultTaskTypeForQwests);

                if (StaticMetods.PersProperty.TasksTypes.All(n => n != typeOfTaskDefoult))
                {
                    typeOfTaskDefoult = StaticMetods.PersProperty.TasksTypes.FirstOrDefault();
                }

                return typeOfTaskDefoult;
            }

            set
            {
                if (typeOfTaskDefoult == value)
                {
                    return;
                }

                typeOfTaskDefoult = value;
                OnPropertyChanged(nameof(TypeOfTaskDefoultProperty));
            }
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
                return new ucSetGoldExpRevardViewModel() { Qwest = this };
            }
        }

        /// <summary>
        /// Для прокачки навыков
        /// </summary>
        public ObservableCollection<UpUbility> UpUbilitys
        {
            get
            {
                return _upUbilitys ?? (_upUbilitys = new ObservableCollection<UpUbility>());
            }
            set
            {
                if (Equals(value, _upUbilitys)) return;
                _upUbilitys = value;
                OnPropertyChanged(nameof(UpUbilitys));
            }
        }

        /// <summary>
        /// Sets and gets Виртуальный опыт для автосортировки квестов. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public int VirtualExpProperty
        {
            get
            {
                return virtualExp;
            }

            set
            {
                if (virtualExp == value)
                {
                    return;
                }

                virtualExp = value;
                OnPropertyChanged(nameof(VirtualExpProperty));
            }
        }

        Type IDropable.DataType
        {
            get { return typeof(Aim); }
        }

        Type IDragable.DataType
        {
            get { return typeof(Aim); }
        }

        public IEnumerable<Aim> AllPrev(Pers persProperty)
        {
            var nnn = Needs;
            var ab = from abilitiModel in InAbills(persProperty)
                     let lev = abilitiModel.NeedAims.First(n => n.AimProperty == this).LevelProperty
                     from na in abilitiModel.NeedAims
                     where na.LevelProperty < lev
                     select na.AimProperty;

            return nnn.Union(ab).Distinct();
        }

        public override void ChangeValuesOfRelaytedItems()
        {
            // Пересчет значений квестов
            foreach (
                Aim aim in StaticMetods.PersProperty.Aims.Where(n => n.CompositeAims.Any(q => q.AimProperty == this)))
            {
                aim.CountAutoProgress();
            }
        }

        /// <summary>
        /// Сравнение квестов
        /// </summary>
        /// <param name="other">Другой квест</param>
        /// <returns>Результат сравнения</returns>
        public int CompareTo(Aim other)
        {
            // Сравнение по статусу
            if (StatusProperty != other.StatusProperty) return StatusProperty.CompareTo(other.StatusProperty);

            // Сравнение по есть задачи?
            //var haveTasksCompare = NeedsTasks.Any().CompareTo(other.NeedsTasks.Any());
            //if (haveTasksCompare != 0)
            //{
            //    return -haveTasksCompare;
            //}

            // Сравнение по есть активные задачи
            //var haveActive =
            //    NeedsTasks.Any(q => q.TaskProperty.IsEnabled)
            //        .CompareTo(other.NeedsTasks.Any(q => q.TaskProperty.IsEnabled));
            //if (haveActive != 0)
            //{
            //    return -haveActive;
            //}

            // Сравнение по прогрессу
            var progCompare = autoProgressValue.CompareTo(other.autoProgressValue);
            if (progCompare != 0)
            {
                return -progCompare;
            }

            // Сравнение по дате завершения
            if (IsDoneProperty && other.IsDoneProperty)
            {
                return -BeginDate.CompareTo(other.BeginDate);
            }

            //// Сравнение по сложности
            //var hrd = HardnessProperty.CompareTo(other.HardnessProperty);
            //if (hrd != 0)
            //{
            //    return hrd;
            //}

            return NameOfProperty.CompareTo(other.NameOfProperty);

            // Сравнение по следующий/предыдущий
            //if (Needs.Contains(other))
            //{
            //    return 1;
            //}
            //else if (other.Needs.Contains(this))
            //{
            //    return -1;
            //}

            //// Сравнение по количеству составных
            //var compareByCompCount = AllCompositeQwests.Count(n => !n.IsDoneProperty).CompareTo(other.AllCompositeQwests.Count(n=>!n.IsDoneProperty));
            //if (compareByCompCount!=0)
            //{
            //    return compareByCompCount;
            //}

            //// Сравнение по дочерний/родительский
            //if (AllCompositeQwests.Contains(other))
            //{
            //    return 1;
            //}
            //else if (other.AllCompositeQwests.Contains(this))
            //{
            //    return -1;
            //}

            //if (!CompositeAims.Any() && other.CompositeAims.Any())
            //{
            //    return -1;
            //}
            //if (CompositeAims.Any() && !other.CompositeAims.Any())
            //{
            //    return 1;
            //}

            // Сравнение по потенциальной активности
            //var thisPotential =
            //    Skills.Any(
            //        n =>
            //            MainViewModel.IsTaskVisibleInCurrentView(n, null, StaticMetods.PersProperty, false, true, false,
            //                true));
            //var otherPotential =
            //   other.Skills.Any(
            //       n =>
            //           MainViewModel.IsTaskVisibleInCurrentView(n, null, StaticMetods.PersProperty, false, true, false,
            //               true));
            //var compareByPotential = thisPotential.CompareTo(otherPotential);
            //if (compareByPotential != 0) return -compareByPotential;

            //var thisSkillsNull = Skills.Count == 0;
            //var otherSkillsNull = other.Skills.Count == 0;
            //var compareNullSkills = thisSkillsNull.CompareTo(otherSkillsNull);
            //if (compareNullSkills != 0) return -compareNullSkills;

            ////var allAims = StaticMetods.PersProperty.Aims;

            ////return allAims.IndexOf(this).CompareTo(allAims.IndexOf(other));

            // Сравнение по прогрессу
            //var progCompare = this.AutoProgressValueProperty.CompareTo(other.AutoProgressValueProperty);
            //if (progCompare!=0)
            //{
            //    return -progCompare;
            //}

            //return this.NameOfProperty.CompareTo(other.NameOfProperty);
        }

        /// <summary>
        /// Расчитываем значения для автопрогресса
        /// </summary>
        public void CountAutoProgress()
        {
            // Если квест выполнен, то его прогресс максимален
            if (IsDoneProperty)
            {
                // Назначаем значения
                AutoProgressValueProperty = 100.0;
                return;
            }

            if (CompositeAims.Any(n => n.AimProperty.IsDoneProperty == false) ||
                Needs.Any(n => n.IsDoneProperty == false))
            {
                autoProgressValue = -0.01;
                OnPropertyChanged(nameof(AutoProgressValueProperty));
                return;
            }

            // Считаем значения
            double allNeedTask = NeedsTasks.Count;
            double allNeedQwest = CompositeAims.Count;
            double allNeedAbs = NeedAbilitiesToDone.Count;
            double allNeedCha = NeedCharactsToDone.Count;
            double levToDone = MinLevelPropertyToDone > 0? 1: 0;
           

            double allNeedTaskDone = NeedsTasks.Count(n => n.TaskProperty.IsDelProperty);
            double allNeedQwestDone = CompositeAims.Count(n => n.AimProperty.isDone);
            double allNeedAbsDone = NeedAbilitiesToDone.Count(n => n.AbilProperty.CellValue >= n.ValueProperty);
            double allNeedChaDone = NeedCharactsToDone.Count(n => n.CharactProperty.CellValue >= n.ValueProperty);
            double levToDoneDone = 0;
            if (MinLevelPropertyToDone > 0 && StaticMetods.PersProperty.PersLevelProperty >= MinLevelPropertyToDone)
            {
                levToDoneDone = 1;
            }

            double max = allNeedTask + allNeedQwest + allNeedAbs + allNeedCha + levToDone;
            double cur = allNeedTaskDone + allNeedQwestDone + allNeedAbsDone + allNeedChaDone + levToDoneDone;

            if (max != 0)
                AutoProgressValueProperty = Math.Round((cur / max) * 100.0, 0);
            else
                AutoProgressValueProperty = 0;

            //double val2 = 0;

            //List<Task> AllNeedTasks = new List<Task>();
            //List<Aim> AllQwests = new List<Aim>();

            //foreach (var compositeAimse in CompositeAims)
            //{
            //    GetALLRelatedTasks(compositeAimse.AimProperty, ref AllQwests);
            //}
            //AllNeedTasks.AddRange(NeedsTasks.Select(q => q.TaskProperty));
            //AllNeedTasks.AddRange(AllQwests.SelectMany(n => n.NeedsTasks.Select(q => q.TaskProperty)));

            //if (AllNeedTasks.Any())
            //{
            //    double all = AllNeedTasks.Count;
            //    double done = AllQwests.Where(q => q.IsDoneProperty).SelectMany(q => q.NeedsTasks).Count() +
            //                  NeedsTasks.Count(n => n.TaskProperty.IsDelProperty);
            //    val2 = 100.0 * (done / all);
            //}

            //AutoProgressValueProperty = Math.Round(val2, 0);
        }

        /// <summary>
        /// Удалить составной квест (от которого зависит прогресс этого квеста)
        /// </summary>
        /// <param name="pers">Перс</param>
        /// <param name="composite">Составной квест</param>
        public void DeleteCompositeQwestNeed(Pers pers, CompositeAims composite)
        {
            MessageBoxResult messageBoxResult =
                MessageBox.Show(
                    "Удалить также сам квест?",
                    "Внимание!",
                    MessageBoxButton.OKCancel);

            if (messageBoxResult == MessageBoxResult.Cancel)
            {
                CompositeAims.Remove(composite);
            }
            else
            {
                // Удаляем квест
                StaticMetods.RemoveQwest(pers, composite.AimProperty);
            }

            StaticMetods.RecauntAllValues();
        }

        /// <summary>
        /// Удаление требования задачи для прокачки квеста
        /// </summary>
        /// <param name="pers"></param>
        /// <param name="taskNeed"></param>
        public void DeleteTaskNeed(Pers pers, NeedTasks taskNeed)
        {
            var inAbs = from abilitiModel in pers.Abilitis
                        from needTaskse in abilitiModel.NeedTasks
                        where needTaskse.TaskProperty == taskNeed.TaskProperty
                        select needTaskse;

            var inQwests = from aim in pers.Aims
                           where aim != this
                           from needsTask in aim.NeedsTasks
                           where needsTask.TaskProperty == taskNeed.TaskProperty
                           select needsTask;

            var inAll = inAbs.Concat(inQwests).ToList();

            if (inAll.Any())
            {
                NeedsTasks.Remove(taskNeed);
            }
            else
            {
                taskNeed.TaskProperty?.Delete(pers);
            }

            StaticMetods.RecauntAllValues();
        }

        public void Drop(object data, int index = -1)
        {
            return;
            var allAims = StaticMetods.PersProperty.Aims;
            int indB = allAims.IndexOf(this);
            var aimA = data as Aim;
            int indA = allAims.IndexOf(aimA);
            allAims.Move(indA, indB);
            StaticMetods.Locator.AimsVM.QCollectionViewProperty.Refresh();
        }

        public override byte[] GetDefoultImageFromElement()
        {
            return DefoultPicsAndImages.DefoultQwestImage;
        }

        /// <summary>
        /// Минимальный уровень требований составных и предыдущих квестов
        /// </summary>
        /// <returns></returns>
        public int GetMinNeedsLev()
        {
            var minNeeds = Needs.Any() ? Needs.Max(n => n.MinLevelProperty) : 0;
            var minCompl = CompositeAims.Any() ? CompositeAims.Max(n => n.AimProperty.MinLevelProperty) : 0;
            return Math.Max(minNeeds, minCompl);
        }

        public IEnumerable<AbilitiModel> InAbills(Pers persProperty)
        {
            return persProperty.Abilitis.Where(n => n.NeedAims.Any(q => q.AimProperty == this));
        }

        public void RefreshActiveSkills()
        {
            OnPropertyChanged(nameof(ActiveSkills));
            OnPropertyChanged(nameof(LinksOfTasks));
        }

        /// <summary>
        /// Обновить составные квесты у квеста
        /// </summary>
        public void RefreshCompositeAims()
        {
            OnPropertyChanged(nameof(CompositeAims));
        }

        public void RefreshDoneVisibillity()
        {
            OnPropertyChanged(nameof(IsDoneVisible));
            OnPropertyChanged(nameof(IsStarVisible));
        }

        public void RefreshElRevard()
        {
            OnPropertyChanged(nameof(UcElementRewardsViewModel));
        }

        /// <summary>
        /// Обновить активные ссылки
        /// </summary>
        public void RefreshLinks()
        {
            OnPropertyChanged(nameof(LinksOfTasksActive));
        }

        /// <summary>
        /// Обновить активные миссии квеста
        /// </summary>
        public void RefreshMissions()
        {
            OnPropertyChanged(nameof(ActiveMissions));
        }

        public void RefreshProgInt()
        {
            if(!double.IsNaN(AutoProgressValueProperty))
                ProgressInt = Convert.ToInt32(Math.Floor(AutoProgressValueProperty / 10.0));
        }

        public void RefreshRev()
        {
            OnPropertyChanged(nameof(UcElementRewardsViewModel));
        }

        /// <summary>
        /// Все связанные с квестом и его дочерними квестами требования задач
        /// </summary>
        /// <returns>Задачи</returns>
        public List<NeedTasks> RelatedNeedTasks()
        {
            List<NeedTasks> rel = NeedsTasks.ToList();

            foreach (var compositeAimse in CompositeAims)
            {
                getCompositeRelNeedTasks(compositeAimse.AimProperty, ref rel);
            }

            return rel;
        }

        /// <summary>
        /// Все связанные с квестом и его дочерними квестами задачи
        /// </summary>
        /// <returns>Задачи</returns>
        public List<Task> RelatedTasks()
        {
            List<Task> rel = NeedsTasks.Select(n => n.TaskProperty).ToList();

            foreach (var compositeAimse in CompositeAims)
            {
                getCompositeRelTasks(compositeAimse.AimProperty, ref rel);
            }

            return rel.ToList();
        }

        public void Remove(object i)
        {
        }

        /// <summary>
        /// Перенести все задачи на завтра
        /// </summary>
        public void TasksToTommorow()
        {
            foreach (var needTaskse in NeedsTasks)
            {
                if (needTaskse.TaskProperty.BeginDateProperty <= MainViewModel.selectedTime)
                {
                    needTaskse.TaskProperty.BeginDateProperty = MainViewModel.selectedTime.AddDays(1);
                    if (needTaskse.TaskProperty.EndDate < needTaskse.TaskProperty.BeginDateProperty)
                    {
                        needTaskse.TaskProperty.EndDate = needTaskse.TaskProperty.BeginDateProperty;
                    }
                }
            }
        }

        protected override BitmapImage GetDefoultPic()
        {
            return DefoultPicsAndImages.DefoultQwestPic;
        }

        /// <summary>
        /// Получить вообще все задачи, связанные с этим квестов. В том числе от композитных квестов.
        /// </summary>
        /// <param name="aim"></param>
        /// <returns></returns>
        private void GetALLRelatedTasks(Aim aim, ref List<Aim> allQw)
        {
            foreach (var compositeAimse in aim.CompositeAims)
            {
                GetALLRelatedTasks(compositeAimse.AimProperty, ref allQw);
            }

            allQw.Add(aim);
        }

        private void GetCompAimss(Aim qw, ref List<Aim> composite)
        {
            foreach (
                var compositeAimse in qw.CompositeAims.Union(qw.NeedAbilities.SelectMany(n => n.AbilProperty.NeedAims)))
            {
                GetCompAimss(compositeAimse.AimProperty, ref composite);
                composite.Add(compositeAimse.AimProperty);
            }
        }

        /// <summary>
        /// Задачи из дочерних квестов
        /// </summary>
        /// <param name="aimProperty">квест</param>
        /// <param name="rel">список задач</param>
        private void getCompositeRelNeedTasks(Aim aimProperty, ref List<NeedTasks> rel)
        {
            foreach (var compositeAimse in aimProperty.CompositeAims)
            {
                getCompositeRelNeedTasks(compositeAimse.AimProperty, ref rel);
            }

            var second = aimProperty.NeedsTasks;

            rel = rel.Concat(second).ToList();
        }

        /// <summary>
        /// Задачи из дочерних квестов
        /// </summary>
        /// <param name="aimProperty">квест</param>
        /// <param name="rel">список задач</param>
        private void getCompositeRelTasks(Aim aimProperty, ref List<Task> rel)
        {
            foreach (var compositeAimse in aimProperty.CompositeAims)
            {
                getCompositeRelTasks(compositeAimse.AimProperty, ref rel);
            }

            var second = aimProperty.NeedsTasks.Select(n => n.TaskProperty);

            rel = rel.Concat(second).ToList();
        }

        /// <summary>
        /// Получить все родительские квесты
        /// </summary>
        /// <param name="qw"></param>
        /// <param name="composite"></param>
        private void GetParAimss(Aim qw, ref List<Aim> composite)
        {
            foreach (
                var compositeAimse in
                    StaticMetods.PersProperty.Aims.Where(n => n.CompositeAims.Any(q => q.AimProperty == this)))
            {
                GetCompAimss(compositeAimse, ref composite);
                composite.Add(compositeAimse);
            }
        }
    }

    [Serializable]
    public class UpUbility
    {
        public AbilitiModel Ability { get; set; }
        public Aim QwToUp { get; set; }
        public double ValueToUp { get; set; }
    }
}
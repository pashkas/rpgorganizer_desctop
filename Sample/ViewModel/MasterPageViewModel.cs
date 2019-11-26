using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.View;

namespace Sample.ViewModel
{
    public class HardnessK
    {
        public int KProperty { get; set; }

        public string NameProperty { get; set; }
    }

    public class MasterCharacteristic : INotifyPropertyChanged
    {
        private int _kExpRelay;

        public byte[] ImageProperty { get; set; }

        private ChaRangs _startValue;

        private string name;

        public MasterCharacteristic()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            MasterRelayAbilitis = new ObservableCollection<MasterRelayAbiliti>();
            MasterRelayAbilitis.CollectionChanged += MasterRelayAbilitis_CollectionChanged;
            _kExpRelay = 6;
            _startValue = StaticMetods.PersProperty.PersSettings.CharacteristicRangs.FirstOrDefault();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }

        /// <summary>
        /// Влияние характеристики на опыт
        /// </summary>
        public int KExpRelay
        {
            get { return _kExpRelay; }
            set
            {
                if (value == _kExpRelay) return;
                _kExpRelay = value;
                OnPropertyChanged(nameof(KExpRelay));
            }
        }

        /// <summary>
        ///     скиллы, которые влияют на характеристику
        /// </summary>
        public ObservableCollection<MasterRelayAbiliti> MasterRelayAbilitis { get; set; }

        /// <summary>
        ///     Название характеристики
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                {
                    return;
                }
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ChaRangs StartValue
        {
            get => _startValue;
            set
            {
                _startValue = value;
                OnPropertyChanged(nameof(StartValue));
            }
        }

        /// <summary>
        /// Описание характеристики
        /// </summary>
        public string Summary { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MasterRelayAbilitis_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    Messenger.Default.Send<MasterRelayAbiliti>(oldItem as MasterRelayAbiliti);
                }
            }
            if (e.NewItems != null)
            {
                foreach (var oldItem in e.NewItems)
                {
                    Messenger.Default.Send<MasterRelayAbiliti>(oldItem as MasterRelayAbiliti);
                }
            }
        }
    }

    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MasterPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Комманда Очистить скиллы.
        /// </summary>
        private RelayCommand clearAbilitisCommand;

        /// <summary>
        /// Комманда Очистить все характеристики.
        /// </summary>
        private RelayCommand clearAllCharacteristicsCommand;

        /// <summary>
        ///     Комманда Готово! Создать нового персонажа!.
        /// </summary>
        private RelayCommand finishCommand;

        /// <summary>
        ///     Комманда Выбрать картинку для персонажа.
        /// </summary>
        private RelayCommand getPersImageCommand;

        /// <summary>
        ///     Мастер персонаж.
        /// </summary>
        private MasterPers MasterPers;

        /// <summary>
        /// Обновить все скиллы перса. На основе дистинкта навыков из характеристик.
        /// </summary>
        private RelayCommand refreshAllSkills;

        /// <summary>
        ///     Initializes a new instance of the MasterPageViewModel class.
        /// </summary>
        public MasterPageViewModel()
        {
            MasterPers = new MasterPers();

            foreach (var masterCharacteristic in MasterPers.MasterCharacteristics)
            {
                foreach (var masterRelayAbiliti in masterCharacteristic.MasterRelayAbilitis)
                {
                    ChangeAnotherItems(masterRelayAbiliti);
                }
            }

            MasterPers.PathToImage = StaticMetods.PersProperty.ImageProperty;//StaticMetods.pathToImage(Path.Combine(Environment.CurrentDirectory, "Images", "лего друид.jpg"));

            Messenger.Default.Register<MasterRelayAbiliti>(this, ChangeAnotherItems);

            MasterPersProperty.MasterCharacteristics.CollectionChanged += MasterCharacteristics_CollectionChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<HardnessK> HardnessK
        {
            get
            {
                var needKs = new List<HardnessK>
                {
                    new HardnessK {KProperty = 1, NameProperty = "Норм"},
                    new HardnessK {KProperty = 2, NameProperty = "Сложно"},
                    new HardnessK {KProperty = 3, NameProperty = "Оч. сложно"}
                };

                return needKs;
            }
        }

        public static List<NeedK> NeedK
        {
            get
            {
                var needKs = new List<NeedK>
                {
                    new NeedK {KProperty = 3, NameProperty = "Слабо"},
                    new NeedK {KProperty = 6, NameProperty = "Норм"},
                    new NeedK {KProperty = 10, NameProperty = "Сильно"}
                };
                return needKs;
            }
        }

        /// <summary>
        /// Мастер завершается сразу после настройки скиллов.
        /// </summary>
        public bool CanFinishAfterSkills
        {
            get
            {
                return StaticMetods.PersProperty.PersSettings.IsNoAbs;
            }
        }

        /// <summary>
        /// Gets the Очистить скиллы.
        /// </summary>
        public RelayCommand ClearAbilitisCommand
        {
            get
            {
                return clearAbilitisCommand
                       ?? (clearAbilitisCommand =
                           new RelayCommand(
                               () =>
                               {
                                   foreach (var masterCharacteristic in MasterPersProperty.MasterCharacteristics)
                                   {
                                       masterCharacteristic.MasterRelayAbilitis.Clear();
                                   }
                               },
                               () =>
                               {
                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the Очистить все характеристики.
        /// </summary>
        public RelayCommand ClearAllCharacteristicsCommand
        {
            get
            {
                return clearAllCharacteristicsCommand
                       ?? (clearAllCharacteristicsCommand =
                           new RelayCommand(
                               () =>
                               {
                                   MasterPersProperty.MasterCharacteristics.Clear();
                               },
                               () =>
                               {
                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the комманда Готово! Создать нового персонажа!.
        /// </summary>
        public RelayCommand FinishCommand
        {
            get
            {
                return finishCommand ?? (finishCommand = new RelayCommand(
                    () =>
                    {
                        // Если без навыков, то обновляем все навыки - добавляем им по задаче с таким наванием.
                        if (StaticMetods.PersProperty.PersSettings.IsNoAbs)
                        {
                            foreach (var cha in MasterPers.MasterCharacteristics)
                            {
                                foreach (var ab in cha.MasterRelayAbilitis)
                                {
                                    ab.MasterRelayTasks.Clear();
                                    ab.MasterRelayTasks.Add(new MasterTask() { Name = ab.Name });
                                }
                            }
                        }

                       
                        var isFudge = StaticMetods.PersProperty.PersSettings.IsFUDGE;
                        var is555 = StaticMetods.PersProperty.PersSettings.Is5_5_50;
                        var is10l = StaticMetods.PersProperty.PersSettings.Is10AbLevels;
                        var isNoAbs = StaticMetods.PersProperty.PersSettings.IsNoAbs;

                        var pers = Pers.LoadPers(
                             Path.Combine(Directory.GetCurrentDirectory(), "Templates", "LearningPers"));

                        // Сначала удаляем характеристики
                        foreach (var characteristic in pers.Characteristics.ToList())
                            characteristic.RemoveCharacteristic(pers);

                        // Теперь все скиллы
                        foreach (var abilitiModel in pers.Abilitis.ToList())
                            StaticMetods.DeleteAbility(pers, abilitiModel);

                        // Теперь этим методом подчищаем все остальное и перезагружаем прогу
                        pers.ShopItems.Clear();

                        foreach (var t in pers.Tasks.ToList())
                            t.Delete(pers);

                        foreach (var qw in pers.Aims.ToList())
                            StaticMetods.RemoveQwest(pers, qw, false);

                        pers.ToNullPers();

                        pers.PersSettings.Is10AbLevels = is10l;
                        pers.PersSettings.Is5_5_50 = is555;
                        pers.PersSettings.IsFUDGE = isFudge;
                        pers.PersSettings.IsNoAbs = isNoAbs;

                        pers.LastDateOfUseProperty = DateTime.Now.ToString();
                        pers.ViewForDefoult = pers.Views.FirstOrDefault();
                        pers.NameOfProperty = MasterPers.NameOfPers;
                        pers.Character = MasterPers.Mirovozzrenie;
                        pers.ImageProperty = MasterPers.PathToImage;
                        pers.History = MasterPers.About;
                        pers.Class1.ValueProperty = MasterPers.Class;

                        var addTask = new AddOrEditTaskView();
                        var context = (UcTasksSettingsViewModel)addTask.UcTasksSettingsView.DataContext;

                        // Настраиваем характеристики
                        Dictionary<MasterCharacteristic, Characteristic> dicChaMastrerCha
                        = new Dictionary<MasterCharacteristic, Characteristic>();

                        foreach (var masterCharacteristic in MasterPers.MasterCharacteristics)
                        {
                            var charact = new Characteristic(pers)
                            {
                                NameOfProperty = masterCharacteristic.Name,
                                FirstVal = pers.PersSettings.CharacteristicRangs.IndexOf(pers.PersSettings.CharacteristicRangs.FirstOrDefault(n => n.Name == masterCharacteristic.StartValue.Name)),
                                DescriptionProperty = masterCharacteristic.Summary
                            };

                            dicChaMastrerCha.Add(masterCharacteristic, charact);
                        }

                        // Навыки
                        Dictionary<MasterRelayAbiliti, AbilitiModel> dicMasterAbAb
                                = new Dictionary<MasterRelayAbiliti, AbilitiModel>();

                        foreach (var masterCharacteristic in MasterPers.MasterCharacteristics)
                        {
                            var charact = dicChaMastrerCha[masterCharacteristic];

                            foreach (MasterRelayAbiliti masterRelayAbiliti in masterCharacteristic.MasterRelayAbilitis)
                            {
                                // Если уже есть скилл с таким названием...
                                //var isInAbil = charact.NeedAbilitisProperty.FirstOrDefault(n => n.AbilProperty.NameOfProperty == masterRelayAbiliti.Name);
                                //if (isInAbil != null)
                                //{
                                //    isInAbil.KoeficientProperty = 10;
                                //    continue;
                                //}

                                // Если нет - добавляем новый скилл
                                var ability = new AbilitiModel(pers)
                                {
                                    NameOfProperty = masterRelayAbiliti.Name,
                                    DescriptionProperty = masterRelayAbiliti.Summary
                                };

                                // Настраиваем влияние на характеристику
                                charact.NeedAbilitisProperty.First(n => n.AbilProperty == ability).KoeficientProperty = 10;

                                dicMasterAbAb.Add(masterRelayAbiliti, ability);
                            }
                        }

                        // Задачи
                        foreach (var masterCharacteristic in MasterPers.MasterCharacteristics)
                        {
                            foreach (MasterRelayAbiliti ab in masterCharacteristic.MasterRelayAbilitis)
                            {
                                var ability = dicMasterAbAb[ab];

                                // Добавляем задачи
                                foreach (var mTsk in ab.MasterRelayTasks)
                                {
                                    context.AddNewTask(StaticMetods.PersProperty.TasksTypes.First());
                                    var tsk = context.SelectedTaskProperty;
                                    context.TaskBalanceDefoults();
                                    Task.RecountTaskLevel(tsk);
                                    tsk.NameOfProperty = mTsk.Name;
                                    tsk.Recurrense.Interval = 1;
                                    tsk.Recurrense.TypeInterval = TimeIntervals.Будни;
                                    tsk.AimTimerMax = mTsk.AimTime;
                                    tsk.AimMesure = mTsk.AimVal;
                                    tsk.AimCounterMax = mTsk.AimVal;
                                    tsk.TaskStates = mTsk.States;
                                    Task.taskSettingForAbility(tsk, ability, 0);
                                }

                                // Настраиваем последовательности задач для навыка
                                var nt = ability.NeedTasks.OrderByDescending(n => n).ToList();

                                if (ab.IsTasksQued || ab.IsTasksParallel)
                                {
                                    if (ab.IsTasksQued)
                                        AddOrEditAbilityViewModel.SetNeedTasksQued(nt);
                                    else if (ab.IsTasksParallel)
                                        AddOrEditAbilityViewModel.SetNeedTasksParallel(nt);
                                }
                                else
                                    AddOrEditAbilityViewModel.SetNeedTasksAllNow(nt);
                            }
                        }

                        StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.ПолностьюСНачала, null);
                        StaticMetods.PersProperty = pers;

                        foreach (var source in StaticMetods.PersProperty.Tasks.Where(n => n.IsDelProperty).ToList())
                            source.Delete(StaticMetods.PersProperty);

                        // StaticMetods.PersProperty.Tasks.FirstOrDefault()?.Delete(pers);

                        //List<System.Threading.Tasks.Task> tsks = new List<System.Threading.Tasks.Task>();

                        // Характеристики
                        foreach (var cha in StaticMetods.PersProperty.Characteristics)
                        {
                            //tsks.Add(System.Threading.Tasks.Task<byte[]>.Run(() =>
                            //{
                            //    return InetImageGen.ImageByWord(cha.NameOfProperty);
                            //}).ContinueWith((img) =>
                            //{
                            //    cha.ImageProperty =
                            //    img.Result;
                            //}, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext()));
                            cha.ImageProperty = InetImageGen.ImageByWord(cha.NameOfProperty);
                        }

                        // Навыки
                        foreach (var ab in StaticMetods.PersProperty.Abilitis)
                        {
                            //tsks.Add(System.Threading.Tasks.Task<byte[]>.Run(() =>
                            //{
                            //    return InetImageGen.ImageByWord(ab.NameOfProperty);
                            //}).ContinueWith((img) =>
                            //{
                            //    ab.ImageProperty =
                            //    img.Result;
                            //}, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext()));
                            ab.ImageProperty = InetImageGen.ImageByWord(ab.NameOfProperty);
                        }

                        StaticMetods.Locator.MainVM.LetSBegin(true);

                        // Дожидаемся завершения всех задач
                        //System.Threading.Tasks.Task.WhenAll(tsks.ToArray()).ContinueWith((res) =>
                        //{
                        //    res.Wait();

                        //    StaticMetods.PersProperty.ToNullPers();

                        //}, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
                    },
                    () => true));
            }
        }

        /// <summary>
        ///     Gets the комманда Выбрать картинку для персонажа.
        /// </summary>
        public RelayCommand GetPersImageCommand
        {
            get
            {
                return getPersImageCommand
                       ?? (getPersImageCommand =
                           new RelayCommand(
                               () => { this.MasterPersProperty.PathToImage = StaticMetods.GetPathToImage(this.MasterPersProperty.PathToImage); },
                               () => true));
            }
        }

        public bool isFinish { get; set; }

        /// <summary>
        /// Настройка скиллов - последняя страница мастера.
        /// </summary>
        public bool IsSkillsLastPage
        {
            get
            {
                return !StaticMetods.PersProperty.PersSettings.IsNoAbs;
            }
        }

        public List<NeedK> KExpRelays
        {
            get
            {
                var needKs = new List<NeedK>
                {
                    new NeedK {KProperty = 3, NameProperty = "Слабо"},
                    new NeedK {KProperty = 6, NameProperty = "Норм"},
                    new NeedK {KProperty = 10, NameProperty = "Сильно"}
                };
                return needKs;
            }
        }

        /// <summary>
        ///     Sets and gets Мастер персонаж.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public MasterPers MasterPersProperty
        {
            get { return MasterPers; }

            set
            {
                if (MasterPers == value)
                {
                    return;
                }

                MasterPers = value;
                OnPropertyChanged(nameof(MasterPersProperty));
            }
        }

        /// <summary>
        /// Обновить все скиллы на основе дистинкта навыков из всех характеристик.
        /// </summary>
        public RelayCommand RefreshAllSkillsCommand
        {
            get
            {
                return refreshAllSkills ?? (
                    refreshAllSkills = new RelayCommand(() =>
                    {
                        if (!StaticMetods.PersProperty.PersSettings.IsNoAbs)
                        {
                            MasterPersProperty.MasterAbilities = MasterPersProperty.MasterCharacteristics
                            .SelectMany(n => n.MasterRelayAbilitis)
                            .DistinctBy(n => n.Name)
                            .ToList();

                            foreach (var ma in MasterPersProperty.MasterAbilities)
                            {
                                ma.MasterRelayTasks.Clear();
                                ma.MasterRelayTasks.Add(new MasterTask() { Name = ma.Name });
                            }
                        }
                    })
                    );
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangeAnotherItems(MasterRelayAbiliti masterRelayAbiliti)
        {
            var sameAbs = (from masterCharacteristic in MasterPersProperty.MasterCharacteristics
                           from relayAbiliti in masterCharacteristic.MasterRelayAbilitis
                           where relayAbiliti.Name == masterRelayAbiliti.Name
                           select new { masterCharacteristic, relayAbiliti }).ToList();

            Brush custColor = PickBrush();

            if (sameAbs.Count() > 1)
            {
                foreach (var abiliti in sameAbs)
                {
                    abiliti.relayAbiliti.HardnessProperty = masterRelayAbiliti.HardnessProperty;
                    abiliti.relayAbiliti.BackgroundColor = custColor;
                    string relToCha = $"{abiliti.masterCharacteristic.Name};";

                    var relToAnother =
                        from masterCharacteristic in MasterPersProperty.MasterCharacteristics
                        where masterCharacteristic != abiliti.masterCharacteristic
                        where masterCharacteristic.MasterRelayAbilitis.Any(n => n.Name == abiliti.relayAbiliti.Name)
                        select masterCharacteristic;

                    foreach (var characteristic in relToAnother)
                    {
                        relToCha = relToCha + $" {characteristic.Name};";
                    }

                    abiliti.relayAbiliti.RelayCharacts = relToCha;
                }
            }
            if (sameAbs.Count == 1)
            {
                sameAbs.First().relayAbiliti.BackgroundColor = Brushes.White;
                sameAbs.First().relayAbiliti.RelayCharacts = $"{sameAbs.First().masterCharacteristic.Name};";
            }
        }

        private void MasterCharacteristics_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var masterCharacteristic in MasterPers.MasterCharacteristics)
            {
                foreach (var masterRelayAbiliti in masterCharacteristic.MasterRelayAbilitis)
                {
                    ChangeAnotherItems(masterRelayAbiliti);
                }
            }
        }

        private Brush PickBrush()
        {
            var R = MainViewModel.rnd.Next(150, 225);
            var B = MainViewModel.rnd.Next(150, 225);
            var G = MainViewModel.rnd.Next(150, 225);
            var brush = new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));

            return brush;
        }
    }

    public class MasterPers : INotifyPropertyChanged
    {
        private List<ChaRangs> _chaRangs;
        private List<MasterRelayAbiliti> _masterAbilities;
        private string about;

        private string mirovozzrenie;

        private string nameOfPers;

        private byte[] pathToImage;

        private string persClass;

        public MasterPers()
        {
            MasterCharacteristics = new ObservableCollection<MasterCharacteristic>();
            MasterAbilities = new List<MasterRelayAbiliti>();
            MasterQwests = new ObservableCollection<MasterQwest>();
            NameOfPers = "Хиро";
            Mirovozzrenie = "Добрый";
            Class = "Искатель приключений";
            About =
                "Обычный человек, который хочет стать лучше и найти свое место в жизни.";
            PathToImage = null;
            MasterCharacteristics.CollectionChanged += MasterCharacteristics_CollectionChanged;
            ChaRangs = StaticMetods.PersProperty.PersSettings.CharacteristicRangs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Описание персонажа
        /// </summary>
        public string About
        {
            get { return about; }
            set
            {
                if (value == about)
                {
                    return;
                }
                about = value;
                OnPropertyChanged(nameof(About));
            }
        }

        public List<MasterCharacteristic> AllCharacts => MasterCharacteristics.ToList();

        public List<ChaRangs> ChaRangs
        {
            get { return _chaRangs; }
            set
            {
                _chaRangs = value;
                OnPropertyChanged(nameof(ChaRangs));
            }
        }

        /// <summary>
        ///     Класс персонажа
        /// </summary>
        public string Class
        {
            get { return persClass; }
            set
            {
                if (value == persClass)
                {
                    return;
                }
                persClass = value;
                OnPropertyChanged(nameof(Class));
            }
        }

        /// <summary>
        /// Навыки персонажа. Пробег по характеристикам и дистинкт по названиям.
        /// </summary>
        public List<MasterRelayAbiliti> MasterAbilities
        {
            get
            {
                return _masterAbilities;
            }

            set
            {
                _masterAbilities = value;
                OnPropertyChanged(nameof(MasterAbilities));
            }
        }

        /// <summary>
        ///     Характеристики персонажа со связанными скиллами
        /// </summary>
        public ObservableCollection<MasterCharacteristic> MasterCharacteristics { get; set; }

        /// <summary>
        ///     Квесты персонажа
        /// </summary>
        public ObservableCollection<MasterQwest> MasterQwests { get; set; }

        /// <summary>
        ///     Мировоззрение персонажа
        /// </summary>
        public string Mirovozzrenie
        {
            get { return mirovozzrenie; }
            set
            {
                if (value == mirovozzrenie)
                {
                    return;
                }
                mirovozzrenie = value;
                OnPropertyChanged(nameof(Mirovozzrenie));
            }
        }

        /// <summary>
        ///     Имя персонажа
        /// </summary>
        public string NameOfPers
        {
            get { return nameOfPers; }
            set
            {
                if (value == nameOfPers)
                {
                    return;
                }
                nameOfPers = value;
                OnPropertyChanged(nameof(NameOfPers));
            }
        }

        /// <summary>
        ///     Путь к картинке персонажа
        /// </summary>
        public byte[] PathToImage
        {
            get { return pathToImage; }
            set
            {
                if (value == pathToImage)
                {
                    return;
                }
                pathToImage = value;
                OnPropertyChanged(nameof(PathToImage));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MasterCharacteristics_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(AllCharacts));
        }
    }

    public class MasterQwest : INotifyPropertyChanged
    {
        private string name;

        public MasterQwest()
        {
            Name = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Название квеста
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                {
                    return;
                }
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MasterRelayAbiliti : INotifyPropertyChanged
    {
        private Brush _backgroundColor = Brushes.White;
        private bool _isTasksParallel;
        private bool _isTasksQued;
        private double _kChaRelay;
        private string _relayCharacts;

        public byte[] ImageProperty { get; set; }

        /// <summary>
        ///     Сложность.
        /// </summary>
        private int hardness;

        private bool isPerk;
        private string name;

        public MasterRelayAbiliti()
        {
            Name = string.Empty;
            HardnessProperty = 1;
            IsPerk = false;
            KChaRelay = 3.0;
            MasterRelayTasks = new ObservableCollection<MasterTask>();
            Id = Guid.NewGuid().ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Цвет фона (выделяем одинаковые)
        /// </summary>
        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (value.Equals(_backgroundColor)) return;
                _backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        /// <summary>
        ///     Sets and gets Сложность.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int HardnessProperty
        {
            get { return hardness; }

            set
            {
                if (hardness == value)
                {
                    return;
                }

                hardness = value;
                OnPropertyChanged(nameof(HardnessProperty));
                Messenger.Default.Send<MasterRelayAbiliti>(this);
            }
        }

        public string Id { get; set; }

        /// <summary>
        ///     Перк?
        /// </summary>
        public bool IsPerk
        {
            get { return isPerk; }
            set
            {
                if (value.Equals(isPerk))
                {
                    return;
                }
                isPerk = value;
                OnPropertyChanged(nameof(IsPerk));
            }
        }

        /// <summary>
        /// Задачи добавляются постепенно?
        /// </summary>
        public bool IsTasksParallel
        {
            get
            {
                return _isTasksParallel;
            }
            set
            {
                _isTasksParallel = value;
                OnPropertyChanged(nameof(IsTasksParallel));

                if (value == true)
                {
                    IsTasksQued = false;
                }
            }
        }

        /// <summary>
        /// Задачи по очереди?
        /// </summary>
        public bool IsTasksQued
        {
            get
            {
                return _isTasksQued;
            }
            set
            {
                _isTasksQued = value;
                OnPropertyChanged(nameof(IsTasksQued));

                if (value == true)
                {
                    IsTasksParallel = false;
                }
            }
        }

        /// <summary>
        /// Влияние на характеристику
        /// </summary>
        public double KChaRelay
        {
            get { return _kChaRelay; }
            set
            {
                if (value.Equals(_kChaRelay)) return;
                _kChaRelay = value;
                OnPropertyChanged(nameof(KChaRelay));
            }
        }

        public ObservableCollection<MasterTask> MasterRelayTasks { get; set; }

        /// <summary>
        ///     Название скилла
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                {
                    return;
                }
                name = value;
                OnPropertyChanged(nameof(Name));
                Messenger.Default.Send<MasterRelayAbiliti>(this);
            }
        }

        /// <summary>
        /// Влияние на характеристики
        /// </summary>
        public string RelayCharacts
        {
            get { return _relayCharacts; }
            set
            {
                if (value == _relayCharacts) return;
                _relayCharacts = value;
                OnPropertyChanged(nameof(RelayCharacts));
            }
        }

        /// <summary>
        /// Описание скилла
        /// </summary>
        public string Summary { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MasterTask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Целевое количество времени.
        /// </summary>
        public int AimTime { get; set; }

        /// <summary>
        /// Целевое количество повторений.
        /// </summary>
        public int AimVal { get; set; }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        public ObservableCollection<SubTask> States { get; set; } = new ObservableCollection<SubTask>();

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
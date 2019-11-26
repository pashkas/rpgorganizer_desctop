using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.Properties;
using Sample.View;

namespace Sample.ViewModel
{
    /// <summary>
    ///     скиллы, на которые влияет квест
    /// </summary>
    public class AbLevelUps
    {
        /// <summary>
        ///     скилл на который влияет квест
        /// </summary>
        public AbilitiModel Abiliti { get; set; }

        /// <summary>
        ///     Значение изменения
        /// </summary>
        public double ChangeVal
        {
            get { return Qwest?.KRel ?? 0; }
            set
            {
                if (Qwest != null)
                {
                    if (Qwest?.KRel == value) return;
                    Qwest.KRel = value;
                    Abiliti.RecountMinValues();
                    StaticMetods.Locator.QwestsVM.getRelaysItems();
                }
            }
        }

        /// <summary>
        ///     Начальный ранг с которого доступен квест
        /// </summary>
        public int LevelFrom
        {
            get { return Qwest?.LevelProperty ?? 1; }
            set
            {
                if (Qwest != null)
                {
                    if (Qwest?.LevelProperty == value) return;
                    Qwest.LevelProperty = value;
                    Abiliti.RecountMinValues();
                }
            }
        }

        /// <summary>
        ///     Условие-требование скилла
        /// </summary>
        public CompositeAims Qwest { get; set; }
    }

    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>See http://www.galasoft.ch/mvvm</para>
    /// </summary>
    public class QwestsViewModel : INotifyPropertyChanged, IItemsRelaysable, IQwickAdd, IHaveNeedAbilities
    {

        public ucRevardAbilityNeedViewModel NeedAbilitiesToDoneDataContext => new ucRevardAbilityNeedViewModel(Pers, SelectedAimProperty.NeedAbilitiesToDone, null, SelectedAimProperty);

        public ucRevardNeedCharacteristics NeedChaToDoneDataContext => new ucRevardNeedCharacteristics(Pers, SelectedAimProperty.NeedCharactsToDone, SelectedAimProperty);


        /// <summary>
        ///     Initializes a new instance of the QwestsViewModel class.
        /// </summary>
        public QwestsViewModel()
        {
            Pers = StaticMetods.PersProperty;

            RelaysItemsVm = new ucRelaysItemsVM
            {
                IsNeedsProperty = false,
                IsReqvirementsProperty = false,
                ParrentDataContext = this
            };

            Messenger.Default.Send(
                new closeAimsMwssege { IsCloseAims = IsCompleteAimsCloseProperty });

            Messenger.Default.Send(
                new notNowAimsMwssege { IsCloseNotNowAims = isCloseNotNowAimsProperty });

            // Сюда зафигачить требования листвью

            NeedsRefresh();

            Messenger.Default.Register<string>(
                this,
                _string =>
                {
                    if (_string == "Ок в задаче")
                    {
                        IsEsitOpenProperty = false;

                        TasksNeedsProperty = GetQwestNeeds();
                        OnPropertyChanged(nameof(NeedTasksView));
                        NeedsRefresh();
                    }

                    if (_string == "Квест добавлен")
                    {
                    }

                    if (_string == "Показ требований навыков")
                    {
                    }

                    if (_string == "ПлюсМинусНажат!")
                    {
                        OnPropertyChanged(nameof(SelectedAimProperty));
                        NeedsRefresh();
                    }
                });
        }

        public RelayCommand chooseTaskToLinkCommand;

        /// <summary>
        ///     Добавить следующий квест.
        /// </summary>
        private RelayCommand _addNextQwest;

        /// <summary>
        ///     Выбрать скилл для того, чтобы квест мог его прокачивать.
        /// </summary>
        private RelayCommand _chooseAbToUPSCommand;

        /// <summary>
        ///     Выбрать скилл для того, чтобы в квест можно было бы забубенить условия скилла.
        /// </summary>
        private RelayCommand _chooseAbToUslCommand;

        /// <summary>
        ///     Выбрать характеристику для того, чтобы сделать из нее условия квеста.
        /// </summary>
        private RelayCommand _chooseChaToUslCommand;

        /// <summary>
        ///     Удалить инфу по прокачке скилла
        /// </summary>
        private RelayCommand<object> _delAbLevelUps;

        /// <summary>
        ///     Удалить артефакт
        /// </summary>
        private RelayCommand<object> _deleteArtefact;

        /// <summary>
        ///     Удалить требование скилла
        /// </summary>
        private RelayCommand<object> _delNeedAb;

        /// <summary>
        ///     Удалить требование характеристик из квеста
        /// </summary>
        private RelayCommand<object> _delNeedCha;

        /// <summary>
        ///     Команда на завершение квеста
        /// </summary>
        private RelayCommand<object> _doneQwest;

        private int _gold;
        private bool _isQwickAddTaskVisible;
        private List<QwickAdd> _qwickAddTasksList = new List<QwickAdd>();
        private object _selNeed;

        /// <summary>
        ///     Показать скилл из прокачки скиллов
        /// </summary>
        private RelayCommand<object> _showAbFromAbLevUps;

        /// <summary>
        ///     Показать скилл из требований скилла
        /// </summary>
        private RelayCommand<object> _showNeedAb;

        /// <summary>
        ///     Показать характеристику из требований квеста
        /// </summary>
        private RelayCommand<object> _showNeedCha;

        private IOrderedEnumerable<Task> _tasksNeedsProperty;

        /// <summary>
        ///     Gets the Добавить навык для прокачки квестом.
        /// </summary>
        private RelayCommand<object> addAbToLevelUpsCommand;

        /// <summary>
        ///     Gets the Добавить новый скилл, задачу, характеристику.
        /// </summary>
        private RelayCommand<string> addChaAbilTaskCommand;

        /// <summary>
        ///     Комманда Добавить составной квест для выбранного квеста.
        /// </summary>
        private RelayCommand<string> addCompositeQwestCommand;

        /// <summary>
        ///     Комманда Добавить требование по задаче.
        /// </summary>
        private RelayCommand<string> addNeedTaskCommand;

        /// <summary>
        ///     Gets the Добавить связь к квесту.
        /// </summary>
        private RelayCommand<Aim> addNodeCommand;

        /// <summary>
        ///     Gets the Добавить требование для квеста (другой квест, который надо выполнить ПЕРЕД этим).
        /// </summary>
        private RelayCommand addQwestReqCommand;

        /// <summary>
        ///     Комманда Добавить награду за квест.
        /// </summary>
        private RelayCommand addRevardCommand;

        /// <summary>
        ///     Добавить новую задачу.
        /// </summary>
        private RelayCommand<TypeOfTask> adNewTaskCommand;

        /// <summary>
        ///     Выбор картинки.
        /// </summary>
        private RelayCommand chooseImagePropertyCommand;

        /// <summary>
        ///     Gets the Ок в выборе составного квеста.
        /// </summary>
        private RelayCommand<Aim> chooseQwestForCompositeCommand;

        /// <summary>
        ///     Команда на закрытие.
        /// </summary>
        private RelayCommand closeCommand;

        /// <summary>
        ///     Сигнал на закрытие.
        /// </summary>
        private bool closeSignal;

        /// <summary>
        ///     Gets the Копировать требование в другой квест.
        /// </summary>
        private RelayCommand<Aim> copyNeedCommand;

        /// <summary>
        ///     Gets the Удалить требование из квеста или сам квест.
        /// </summary>
        private RelayCommand<QwestRelayQwest> deleteNeedFromQwestCommand;

        /// <summary>
        ///     Gets the Удалить связь из квеста.
        /// </summary>
        private RelayCommand<object> delNodeCommand;

        /// <summary>
        ///     Gets the Удалить скилл из квеста.
        /// </summary>
        private RelayCommand<object> delSpellCommand;

        /// <summary>
        ///     Комманда Снять размытость.
        /// </summary>
        private RelayCommand disableBlurCommand;

        private RelayCommand imgGenFromWord;

        /// <summary>
        ///     Открыт ли выбор составного квеста?.
        /// </summary>
        private bool isChooseCompositeQwestOpen;

        /// <summary>
        ///     Редактируется ли требование или добавляется?.
        /// </summary>
        private bool isEditNeed;

        /// <summary>
        ///     Активна ли форма?.
        /// </summary>
        private bool isEnabled;

        /// <summary>
        ///     Открыть редактирование задачи.
        /// </summary>
        private bool isEsitOpen;

        /// <summary>
        ///     Показать перемещение требования.
        /// </summary>
        private bool isOpenMoveNeed;

        /// <summary>
        ///     Открыто ли добавление требований к задачам?.
        /// </summary>
        private bool isOpenTaskNeed;

        /// <summary>
        ///     Видимость изменений опыта.
        /// </summary>
        private bool isQwestsChangesVisible;

        /// <summary>
        ///     Открыть окно показать изменения?.
        /// </summary>
        private bool isViewChangesOpenProperty;

        /// <summary>
        ///     Gets the показать копирование/перемещение требования скилла.
        /// </summary>
        private RelayCommand<NeedTasks> makeQwestFromTask;

        /// <summary>
        ///     Gets the Переместить требование.
        /// </summary>
        private RelayCommand<Aim> moveNeedCommand;

        /// <summary>
        ///     Gets the Сдвинуть требование задачи вниз.
        /// </summary>
        private RelayCommand<NeedTasks> moveNeedTaskDownCommand;

        /// <summary>
        ///     Gets the Сдвинуть требование задач вверх.
        /// </summary>
        private RelayCommand<NeedTasks> moveNeedTaskUpCommand;

        /// <summary>
        ///     Комманда Ок добавить требование для задач.
        /// </summary>
        private RelayCommand okAddTaskNeedCommand;

        /// <summary>
        ///     Кнопка ок в целях.
        /// </summary>
        private RelayCommand okAimCommand;

        /// <summary>
        ///     Комманда ок быстрое добавление задач.
        /// </summary>
        private RelayCommand okQwickAddTasksCommand;

        /// <summary>
        ///     Комманда Открыть карту задач квеста.
        /// </summary>
        private RelayCommand openQwestTaskMapCommand;

        /// <summary>
        ///     Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        ///     Gets the Предыдущая/следующая характеристика.
        /// </summary>
        private RelayCommand<string> prevNextCommand;

        /// <summary>
        ///     Для отображения изменений в квестах после щелчка завершено.
        /// </summary>
        private QwestsChangesModele qwestsChanges = new QwestsChangesModele();

        /// <summary>
        ///     Gets the Быстро задать дату.
        /// </summary>
        private RelayCommand<string> qwickSetDateCommand;

        /// <summary>
        ///     Комманда Обновление информации.
        /// </summary>
        private RelayCommand refreshInfoCommand;

        /// <summary>
        ///     Gets the Удалить связь - составной квест.
        /// </summary>
        private RelayCommand<object> removeCompositeQwestNeedCommand;

        /// <summary>
        ///     Gets the Удалить требование задачи (или задачу).
        /// </summary>
        private RelayCommand<NeedTasks> removeTaskNeedCommand;

        /// <summary>
        ///     Выбранный дочерний квест.
        /// </summary>
        private Aim selectedNeedAim;

        /// <summary>
        ///     Выделенное требование для задач.
        /// </summary>
        private NeedTasks selectedNeedTask;

        /// <summary>
        ///     Комманда Послать сообщение о размытости.
        /// </summary>
        private RelayCommand setBlurCommand;

        /// <summary>
        ///     Gets the Показать перемещение требования.
        /// </summary>
        private RelayCommand<object> showMoveNeedCommand;

        /// <summary>
        ///     Gets the Показать квест.
        /// </summary>
        private RelayCommand<object> showQwestCommand;

        /// <summary>
        ///     Gets the Показать задачи.
        /// </summary>
        private RelayCommand<object> showTaskCommand;

        /// <summary>
        ///     Фильтр для задач.
        /// </summary>
        private string tasksFilter = string.Empty;

        /// <summary>
        ///     Создать квест на основе этой задачи.
        /// </summary>
        private RelayCommand<Task> toQwestCommand;

        /// <summary>
        ///     Gets the Поднять или понизить минимальный уровень скилла.
        /// </summary>
        private RelayCommand<string> upDownMinLevelCommand;

        /// <summary>
        ///     Gets the Просмотр и редактирование награды.
        /// </summary>
        private RelayCommand<Revard> viewRewardCommand;

        /// <summary>
        ///     Текст какое требование перенести или копировать.
        /// </summary>
        private string whatNeedMove;

        /// <summary>
        ///     скиллы на которые влияет этот квест
        /// </summary>
        public List<AbLevelUps> AbsLevelUp
        {
            get
            {
                var abUps = from abilitiModel in Pers.Abilitis
                            from nA in abilitiModel.NeedAims
                            where nA.AimProperty == SelectedAimProperty
                                  && nA.KRel > 0
                            select new AbLevelUps { Abiliti = abilitiModel, Qwest = nA };
                return abUps.ToList();
            }
        }

        /// <summary>
        ///     Gets the Добавить навык для прокачки квестом.
        /// </summary>
        public RelayCommand<object> AddAbToLevelUpsCommand
        {
            get
            {
                return addAbToLevelUpsCommand
                       ?? (addAbToLevelUpsCommand = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as AbilitiModel;
                               if (it == null) return;
                               SelectedAimProperty?.UpUbilitys.Add(new UpUbility() { Ability = it, ValueToUp = 1 });
                               RefreshQwestInfo();
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
        ///     Gets the Добавить новый скилл, задачу, характеристику.
        /// </summary>
        public RelayCommand<string> AddChaAbilTaskCommand
        {
            get
            {
                return addChaAbilTaskCommand ?? (addChaAbilTaskCommand = new RelayCommand<string>(
                    item =>
                    {
                        switch (item.ToString())
                        {
                            case "навык":
                                break;

                            case "характеристика":
                                break;

                            case "задача":
                                break;
                        }
                    },
                    item =>
                    {
                        if (item == null)
                        {
                            return false;
                        }

                        if (item.ToString() == "навык")
                        {
                            return StaticMetods.MayAddAbility(Pers);
                        }

                        return true;
                    }));
            }
        }

        /// <summary>
        ///     Gets the комманда Добавить составной квест для выбранного квеста.
        /// </summary>
        public RelayCommand<string> AddCompositeQwestCommand
        {
            get
            {
                return addCompositeQwestCommand
                       ?? (addCompositeQwestCommand = new RelayCommand<string>(
                           item =>
                           {
                               if (item == "+")
                               {
                                   var thisAim = SelectedAimProperty;
                                   StaticMetods.AddCompositeQwest(Pers, SelectedAimProperty);
                                   StaticMetods.RefreshAllQwests(Pers, true, true, true);
                                   StaticMetods.Locator.AimsVM.SelectedAimProperty = thisAim;
                                   Messenger.Default.Send("Требования квеста!!!");
                                   NeedsRefresh();
                               }
                               else
                               {
                                   IsChooseCompositeQwestOpenProperty = true;
                               }

                               SelectedAimProperty.RefreshCompositeAims();
                               NeedsRefresh();
                           },
                           item => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Добавить требование по задаче.
        /// </summary>
        public RelayCommand<string> AddNeedTaskCommand
        {
            get
            {
                return addNeedTaskCommand ?? (addNeedTaskCommand = new RelayCommand<string>(
                    item =>
                    {
                        if (item == "+")
                        {
                            if (Keyboard.Modifiers == ModifierKeys.Alt)
                            {
                                QwickAdd();
                            }
                            else
                            {
                                var typeOfTaskDefoultProperty = SelectedAimProperty.TypeOfTaskDefoultProperty;
                                AddNewTaskCommandExecute(typeOfTaskDefoultProperty);
                            }

                            OnPropertyChanged(nameof(QwNeedTasks));
                        }
                        else if (item == "skill")
                        {
                            ChooseAbilityToQwestLink();
                        }
                        else
                        {
                            TasksNeedsProperty = GetQwestNeeds();
                            SelectedNeedTaskProperty =
                                GetDefoultNeedTask(TasksNeedsProperty.FirstOrDefault());
                            var ctq = new ChooseTaskToQwest();
                            ctq.DataContext = this;
                            ctq.btnOk.Click += (sender, args) =>
                            {
                                OkAddTaskNeedCommand.Execute(null);
                                ctq.Close();
                            };
                            ctq.btnCansel.Click += (sender, args) => { ctq.Close(); };

                            ctq.ShowDialog();
                        }
                    },
                    item => { return true; }));
            }
        }

        public RelayCommand AddNextQwestCommand
        {
            get
            {
                return _addNextQwest
                       ?? (_addNextQwest = new RelayCommand(
                           () =>
                           {
                               var thisAim = SelectedAimProperty;
                               StaticMetods.addParrentQwest(Pers, SelectedAimProperty, true);
                               StaticMetods.RefreshAllQwests(Pers, true, true, true);
                               SelectedAimProperty = thisAim;
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Добавить связь к квесту.
        /// </summary>
        public RelayCommand<Aim> AddNodeCommand
        {
            get
            {
                return addNodeCommand
                       ?? (addNodeCommand =
                           new RelayCommand<Aim>(
                               item =>
                               {
                                   SelectedAimProperty.Needs.Add(item);
                                   RefreshQwestInfo();
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
        ///     Gets the Добавить требование для квеста (другой квест, который надо выполнить ПЕРЕД этим).
        /// </summary>
        public RelayCommand AddQwestReqCommand
        {
            get
            {
                return addQwestReqCommand
                       ?? (addQwestReqCommand = new RelayCommand(
                           () =>
                           {
                               var thisAim = SelectedAimProperty;
                               StaticMetods.addChildAim(Pers, SelectedAimProperty);
                               StaticMetods.RefreshAllQwests(Pers, true, true, true);
                               SelectedAimProperty = thisAim;
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Добавить награду за квест.
        /// </summary>
        public RelayCommand AddRevardCommand
        {
            get
            {
                return addRevardCommand ?? (addRevardCommand = new RelayCommand(
                    () =>
                    {
                        var addRevard = new AddOrEditRevard();
                        var addRevardVM = new AddOrEditRevardViewModel(null, true, false);
                        addRevard.DataContext = addRevardVM;
                        var revardProperty = addRevardVM.RevardProperty;
                        revardProperty.IsArtefact = true;
                        revardProperty.NeedQwests.Add(SelectedAimProperty);
                        // addRevard.Topmost = true;
                        addRevard.btnOk.Visibility = Visibility.Collapsed;
                        addRevard.btnAdd.Visibility = Visibility.Visible;
                        addRevard.btnCansel.Visibility = Visibility.Visible;
                        addRevard.btnAdd.Click += (sender, args) => { addRevard.Close(); };
                        addRevard.btnCansel.Click += (sender, args) => { addRevard.Close(); };
                        addRevard.ShowDialog();
                        StaticMetods.refreshShopItems(Pers);
                        RefreshQwestInfo();
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Добавить новую задачу.
        /// </summary>
        public RelayCommand<TypeOfTask> AdNewTaskCommand
        {
            get
            {
                return adNewTaskCommand ?? (adNewTaskCommand = new RelayCommand<TypeOfTask>(
                    _type =>
                    {
                        var typeOfTaskDefoultProperty = SelectedAimProperty.TypeOfTaskDefoultProperty
                                                        ?? Pers.TasksTypes.FirstOrDefault();
                        AddNewTaskCommandExecute(typeOfTaskDefoultProperty);
                    },
                    _type => true));
            }
        }

        /// <summary>
        ///     Gets Все цели.
        /// </summary>
        public IEnumerable<Aim> AllAimsProperty
        {
            get
            {
                return
                    Pers?.Aims.Where(n => n != SelectedAimProperty)
                        .Where(n => !n.IsDoneProperty)
                        .OrderBy(n => n.NameOfProperty);
            }
        }

        /// <summary>
        ///     Все требования квеста
        /// </summary>
        public List<AllAimNeeds> AllNeeds
        {
            get
            {
                if (SelectedAimProperty == null)
                {
                    return null;
                }

                var qwests = (from compositeAimse in SelectedAimProperty.CompositeAims
                              let progressProperty = compositeAimse.ProgressProperty
                              select
                                  new AllAimNeeds
                                  {
                                      Type = "Квест",
                                      NameOfNeed = compositeAimse.AimProperty.NameOfProperty,
                                      Progress = progressProperty,
                                      Image = compositeAimse.AimProperty.ImageProperty,
                                      GUID = compositeAimse.AimProperty.GUID
                                  }).OrderByDescending(n => n.Progress);

                var taskNeeds = (from needTasks in SelectedAimProperty.NeedsTasks
                                 select
                                     new AllAimNeeds
                                     {
                                         Type = "Задача",
                                         NameOfNeed = needTasks.TaskProperty.NameOfProperty,
                                         Progress = needTasks.ProgressProperty,
                                         Image = needTasks.TaskProperty.ImageProperty,
                                         GUID = needTasks.TaskProperty.GUID
                                     }).OrderByDescending(n => n.Progress);

                return qwests.Concat(taskNeeds).OrderByDescending(n => n.Progress).ToList();
            }
        }

        /// <summary>
        ///     Gets the Выбрать скилл для того, чтобы квест мог его прокачивать.
        /// </summary>
        public RelayCommand ChooseAbToUPSCommand
        {
            get
            {
                return _chooseAbToUPSCommand
                       ?? (_chooseAbToUPSCommand = new RelayCommand(
                           () =>
                           {
                               var cho = new ChooseAbility();
                               cho.txtHeader.Text = "Выберите навык";
                               var abilitiModels = AbsLevelUp.Select(n => n.Abiliti);
                               cho.lstAbbs.ItemsSource =
                                   getListOfAbilitiesToChoose(abilitiModels);
                               cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                               cho.btnOk.Click += (sender, args) =>
                               {
                                   var ab = cho.lstAbbs.SelectedValue as AbilitiModel;
                                   if (ab == null) return;
                                   var ca = new CompositeAims
                                   {
                                       AimProperty = SelectedAimProperty,
                                       KoeficientProperty = 1,
                                       KRel = 1,
                                       LevelProperty = 0 //ab.NeedLevelForDefoult
                                   };

                                   ca.LevelProperty = 0; //ab.GetNeedLev();
                                   ca.ToLevelProperty = ab.GetNeedLev();

                                   ab.NeedAims.Add(ca);
                                   cho.Close();
                               };
                               cho.ShowDialog();
                               RefreshQwestInfo();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Выбрать скилл для того, чтобы в квест можно было бы забубенить условия скилла.
        /// </summary>
        public RelayCommand ChooseAbToUslCommand
        {
            get
            {
                return _chooseAbToUslCommand
                       ?? (_chooseAbToUslCommand = new RelayCommand(
                           () =>
                           {
                               var cho = new ChooseAbility();
                               cho.txtHeader.Text = "Выберите навык";
                               var abilitiModels = SelectedAimProperty.NeedAbilities.Select(n => n.AbilProperty);
                               cho.lstAbbs.ItemsSource =
                                   getListOfAbilitiesToChoose(abilitiModels);
                               cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                               cho.btnOk.Click += (sender, args) =>
                               {
                                   var ab = cho.lstAbbs.SelectedValue as AbilitiModel;
                                   if (ab == null) return;
                                   var ca = new NeedAbility { AbilProperty = ab, ValueProperty = 100 };
                                   SelectedAimProperty.NeedAbilities.Add(ca);
                                   cho.Close();
                               };
                               cho.ShowDialog();
                               RefreshQwestInfo();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Выбрать характеристику для того, чтобы сделать из нее условия квеста.
        /// </summary>
        public RelayCommand ChooseChaToUslCommand
        {
            get
            {
                return _chooseChaToUslCommand
                       ?? (_chooseChaToUslCommand = new RelayCommand(
                           () =>
                           {
                               var cho = new ChooseAbility();
                               cho.txtHeader.Text = "Выберите характеристику";
                               cho.lstAbbs.ItemsSource =
                                   Pers.Characteristics.Except(
                                       SelectedAimProperty.NeedCharacts.Select(n => n.CharactProperty))
                                       .OrderBy(n => n.NameOfProperty);
                               cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                               cho.btnOk.Click += (sender, args) =>
                               {
                                   var ab = cho.lstAbbs.SelectedValue as Characteristic;
                                   if (ab == null) return;
                                   var ca = new NeedCharact { CharactProperty = ab, ValueProperty = 10 };
                                   SelectedAimProperty.NeedCharacts.Add(ca);
                                   cho.Close();
                               };
                               cho.ShowDialog();
                               RefreshQwestInfo();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Выбор картинки.
        /// </summary>
        public RelayCommand ChooseImagePropertyCommand
        {
            get
            {
                return chooseImagePropertyCommand
                       ?? (chooseImagePropertyCommand =
                           new RelayCommand(
                               () =>
                               {
                                   SelectedAimProperty.ImageProperty =
                                       StaticMetods.GetPathToImage(SelectedAimProperty.ImageProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Ок в выборе составного квеста.
        /// </summary>
        public RelayCommand<Aim> ChooseQwestForCompositeCommand
        {
            get
            {
                return chooseQwestForCompositeCommand
                       ?? (chooseQwestForCompositeCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               if (SelectedAimProperty.CompositeAims.Any(n => n.AimProperty == item) == false)
                               {
                                   SelectedAimProperty.CompositeAims.Add(
                                       new CompositeAims { AimProperty = item, KoeficientProperty = 10 });
                                   NeedsRefresh();
                               }

                               IsChooseCompositeQwestOpenProperty = false;
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
        ///     Выбрать задачу для ссылки к квесту
        /// </summary>
        public RelayCommand ChooseTaskToLinkCommand
        {
            get
            {
                return chooseTaskToLinkCommand ?? (chooseTaskToLinkCommand = new RelayCommand(
                    () =>
                    {
                        var cho = new ChooseAbility
                        {
                            txtHeader = { Text = "Выберите задачу" },
                            lstAbbs =
                            {
                                ItemsSource =
                                    Pers.Tasks.Where(n => n.Recurrense.TypeInterval != TimeIntervals.Нет).Where(
                                        n =>
                                            MainViewModel.IsTaskVisibleInCurrentView(n, null, Pers, true, true, true,
                                                true)).OrderBy(n => n.NameOfProperty).ToList()
                            }
                        };

                        cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                        cho.btnOk.Click += (sender, args) =>
                        {
                            var ab = cho.lstAbbs.SelectedValue as Task;
                            if (ab == null) return;
                            SelectedAimProperty.LinksOfTasks.Add(ab);
                            cho.Close();
                        };
                        cho.ShowDialog();
                        RefreshQwestInfo();
                    },
                    () => true));
            }
        }

        /// <summary>
        ///     Gets the Команда на закрытие.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ?? (closeCommand = new RelayCommand(
                    () =>
                    {
                        CloseSignal = true;
                        closeSignal = false;
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Сигнал на закрытие. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public bool CloseSignal
        {
            get { return closeSignal; }

            set
            {
                if (closeSignal == value)
                {
                    return;
                }

                closeSignal = value;
                OnPropertyChanged(nameof(CloseSignal));
            }
        }

        /// <summary>
        ///     Gets or sets summary.
        /// </summary>
        public string colorProperty
        {
            get
            {
                if (SelectedAimProperty == null)
                {
                    return Colors.GreenYellow.ToString();
                }

                var color = Colors.GreenYellow.ToString();

                return color;
            }
        }

        /// <summary>
        ///     Gets the Копировать требование в другой квест.
        /// </summary>
        public RelayCommand<Aim> CopyNeedCommand
        {
            get
            {
                return copyNeedCommand ?? (copyNeedCommand = new RelayCommand<Aim>(
                    item =>
                    {
                        switch (WhatNeedMoveProperty)
                        {
                            case "квест":
                                item.Needs.Add(SelectedNeedAimProperty);
                                break;

                            case "задача":
                                item.NeedsTasks.Add(SelectedNeedTaskProperty);
                                break;
                        }

                        NeedsRefresh();
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
        ///     Удалить инфу по прокачке скилла
        /// </summary>
        public RelayCommand<object> DelAbLevelUpsCommand
        {
            get
            {
                return _delAbLevelUps ??
                       (_delAbLevelUps = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as UpUbility;
                               if (it != null)
                               {
                                   SelectedAimProperty.UpUbilitys.Remove(it);
                               }
                               RefreshQwestInfo();
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Удалить артефакт
        /// </summary>
        public RelayCommand<object> DeleteArtefactCommand
        {
            get
            {
                return _deleteArtefact ??
                       (_deleteArtefact = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as Revard;
                               Pers.ShopItems.Remove(it);
                               Pers.InventoryItems.Remove(it);
                               StaticMetods.refreshShopItems(Pers);
                               RefreshQwestInfo();
                           },
                           item =>
                           {
                               if (!(item is Revard))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Gets the Удалить требование из квеста или сам квест.
        /// </summary>
        public RelayCommand<QwestRelayQwest> DeleteNeedFromQwestCommand
        {
            get
            {
                return deleteNeedFromQwestCommand
                       ?? (deleteNeedFromQwestCommand =
                           new RelayCommand<QwestRelayQwest>(
                               item =>
                               {
                                   if (Keyboard.Modifiers == ModifierKeys.Control)
                                   {
                                       StaticMetods.RemoveQwest(Pers, item.Qwest);
                                   }
                                   else
                                   {
                                       var NeedsInQwest =
                                           item.Qwest.CompositeAims.Where(
                                               n => n.AimProperty == item.QwestNeed.AimProperty).ToList();

                                       foreach (var qwNeed in NeedsInQwest)
                                       {
                                           item.Qwest.CompositeAims.Remove(qwNeed);
                                       }

                                       item.Qwest.CountAutoProgress();
                                   }

                                   RefreshQwestInfo();
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
        ///     Удалить требование скилла
        /// </summary>
        public RelayCommand<object> DelNeedAbCommand
        {
            get
            {
                return _delNeedAb ??
                       (_delNeedAb = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as NeedAbility;
                               SelectedAimProperty.NeedAbilities.Remove(it);
                               RefreshQwestInfo();
                           },
                           item =>
                           {
                               if (!(item is NeedAbility))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Удалить требование характеристик из квеста
        /// </summary>
        public RelayCommand<object> DelNeedChaCommand
        {
            get
            {
                return _delNeedCha ??
                       (_delNeedCha = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as NeedCharact;
                               SelectedAimProperty.NeedCharacts.Remove(it);
                               RefreshQwestInfo();
                           },
                           item =>
                           {
                               if (!(item is NeedCharact))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Gets the Удалить связь из квеста.
        /// </summary>
        public RelayCommand<object> DelNodeCommand
        {
            get
            {
                return delNodeCommand
                       ?? (delNodeCommand =
                           new RelayCommand<object>(
                               item =>
                               {
                                   var it = item as Aim;
                                   SelectedAimProperty.Needs.Remove(it);
                                   RefreshQwestInfo();
                               },
                               item =>
                               {
                                   var it = item as Aim;
                                   if (it == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the Удалить скилл из квеста.
        /// </summary>
        public RelayCommand<object> DelSpellCommand
        {
            get
            {
                return delSpellCommand
                       ?? (delSpellCommand = new RelayCommand<object>(
                           item =>
                           {
                               var ab = item as AbilitiModel;
                               var tsk = item as Task;

                               if (ab != null)
                               {
                                   SelectedAimProperty.AbilitiLinksOf.Remove(
                                       SelectedAimProperty.AbilitiLinksOf.FirstOrDefault(n => n == ab));
                               }
                               else if (tsk != null)
                               {
                                   SelectedAimProperty.LinksOfTasks.Remove(
                                       SelectedAimProperty.LinksOfTasks.FirstOrDefault(n => n == tsk));
                               }
                               NeedsRefresh();
                           },
                           item =>
                           {
                               var ab = item as AbilitiModel;
                               var tsk = item as Task;
                               if (ab == null && tsk == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Команда на завершение квеста
        /// </summary>
        public RelayCommand<object> DoneQwestCommand
        {
            get
            {
                return _doneQwest ??
                       (_doneQwest = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as Aim;
                               SelectedAimProperty = it;
                               IsQwestDoneProperty = true;
                               StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.КвестВыполнен,
                                   SelectedAimProperty);
                               RefreshInfoCommand.Execute(null);
                           },
                           item =>
                           {
                               if (!(item is Aim))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Опыт за квест
        /// </summary>
        public int ExpForQwest
        {
            get { return SelectedAimProperty.PlusExp; }
            set
            {
                SelectedAimProperty.PlusExp = value;
                OnPropertyChanged(nameof(ExpForQwest));
                getRelaysItems();
                SelectedAimProperty?.RefreshRev();
            }
        }

        /// <summary>
        ///     Золото
        /// </summary>
        public int Gold
        {
            get { return _gold; }
            set
            {
                if (value == _gold) return;
                _gold = value;
                OnPropertyChanged(nameof(Gold));
            }
        }

        /// <summary>
        ///     (Старое) Золото за квест
        /// </summary>
        public int GoldForQwest
        {
            get { return SelectedAimProperty.GoldIfDoneProperty; }
            set
            {
                SelectedAimProperty.GoldIfDoneProperty = value;
                OnPropertyChanged(nameof(GoldForQwest));
                getRelaysItems();
                SelectedAimProperty?.RefreshRev();
            }
        }

        public int HardnessOfQwest
        {
            get { return SelectedAimProperty?.HardnessProperty ?? 0; }
            set
            {
                if (SelectedAimProperty.HardnessProperty == value)
                {
                    return;
                }

                SelectedAimProperty.HardnessProperty = value;

                OnPropertyChanged(nameof(HardnessOfQwest));
                RefreshQwestInfo();
            }
        }

        public RelayCommand ImgGenFromWord
        {
            get
            {
                return imgGenFromWord ?? (imgGenFromWord = new RelayCommand(() =>
                             {
                                 System.Threading.Tasks.Task<byte[]>.Run(() =>
                                 {
                                     return InetImageGen.ImageByWord(SelectedAimProperty.NameOfProperty);
                                 }).ContinueWith((img) => {
                                     SelectedAimProperty.ImageProperty =
                                     img.Result;
                                 }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
                             }));
            }
        }

        /// <summary>
        ///     Sets and gets Открыт ли выбор составного квеста?. Changes to that property's value raise
        ///     the PropertyChanged event.
        /// </summary>
        public bool IsChooseCompositeQwestOpenProperty
        {
            get { return isChooseCompositeQwestOpen; }

            set
            {
                if (isChooseCompositeQwestOpen == value)
                {
                    return;
                }

                isChooseCompositeQwestOpen = value;
                OnPropertyChanged(nameof(IsChooseCompositeQwestOpenProperty));
            }
        }

        /// <summary>
        ///     Gets or sets Скрывать недоступные цели?.
        /// </summary>
        public bool isCloseNotNowAimsProperty
        {
            get
            {
                if (Pers != null)
                {
                    return Pers.isCloseNotNowAims;
                }
                return false;
            }

            set
            {
                if (isCloseNotNowAimsProperty == value)
                {
                    return;
                }

                Pers.isCloseNotNowAims = value;
                OnPropertyChanged(nameof(isCloseNotNowAimsProperty));

                Messenger.Default.Send(
                    new notNowAimsMwssege { IsCloseNotNowAims = isCloseNotNowAimsProperty });
            }
        }

        /// <summary>
        ///     Gets or sets Скрывать ли завершенные цели.
        /// </summary>
        public bool IsCompleteAimsCloseProperty
        {
            get
            {
                if (Pers != null)
                {
                    return Pers.isCloseCompleteAims;
                }
                return false;
            }

            set
            {
                if (IsCompleteAimsCloseProperty == value)
                {
                    return;
                }

                Pers.isCloseCompleteAims = value;
                OnPropertyChanged(nameof(IsCompleteAimsCloseProperty));

                Messenger.Default.Send(
                    new closeAimsMwssege { IsCloseAims = IsCompleteAimsCloseProperty });
            }
        }

        /// <summary>
        ///     Sets and gets Редактируется ли требование или добавляется?. Changes to that property's
        ///     value raise the PropertyChanged event.
        /// </summary>
        public bool IsEditNeedProperty
        {
            get { return isEditNeed; }

            set
            {
                if (isEditNeed == value)
                {
                    return;
                }

                isEditNeed = value;
                OnPropertyChanged(nameof(IsEditNeedProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Активна ли форма?. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public bool IsEnabledProperty
        {
            get { return isEnabled; }

            set
            {
                if (isEnabled == value)
                {
                    return;
                }

                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabledProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Открыть редактирование задачи. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public bool IsEsitOpenProperty
        {
            get { return isEsitOpen; }

            set
            {
                if (isEsitOpen == value)
                {
                    return;
                }

                isEsitOpen = value;
                OnPropertyChanged(nameof(IsEsitOpenProperty));
            }
        }

        public bool IsFilterSkills { get; set; }

        /// <summary>
        ///     Sets and gets Показать перемещение требования. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public bool IsOpenMoveNeedProperty
        {
            get { return isOpenMoveNeed; }

            set
            {
                if (isOpenMoveNeed == value)
                {
                    return;
                }

                isOpenMoveNeed = value;
                OnPropertyChanged(nameof(IsOpenMoveNeedProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Открыто ли добавление требований к задачам?. Changes to that property's
        ///     value raise the PropertyChanged event.
        /// </summary>
        public bool IsOpenTaskNeedProperty
        {
            get { return isOpenTaskNeed; }

            set
            {
                if (isOpenTaskNeed == value)
                {
                    return;
                }

                isOpenTaskNeed = value;
                OnPropertyChanged(nameof(IsOpenTaskNeedProperty));
            }
        }

        /// <summary>
        ///     Gets or sets Свойство готовности выбранного квеста.
        /// </summary>
        public bool IsQwestDoneProperty
        {
            get
            {
                if (SelectedAimProperty == null)
                {
                    return false;
                }

                return SelectedAimProperty.IsDoneProperty;
            }

            set
            {
                var isDoneProperty = value;
                if (IsQwestDoneProperty == isDoneProperty)
                {
                    return;
                }

                if (IsQwestDoneProperty == false && SelectedAimProperty.AutoProgressValueProperty < 99.9)
                {
                    return;
                }

                var selectedAimProperty = SelectedAimProperty;
                var persParam = Pers;

                GetQwestChanges(persParam, selectedAimProperty, isDoneProperty);

                SelectedAimProperty = selectedAimProperty;
                if (selectedAimProperty != null)
                {
                    selectedAimProperty.MinLevelProperty = Pers.PersLevelProperty;
                }

                OnPropertyChanged(nameof(IsQwestDoneProperty));

                StaticMetods.RefreshAllQwests(Pers, true, true, true);
                SelectedAimProperty = selectedAimProperty;

                RefreshQwestInfo();
            }
        }

        /// <summary>
        ///     Sets and gets Видимость изменений опыта. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public bool IsQwestsChangesVisibleProperty
        {
            get { return isQwestsChangesVisible; }

            set
            {
                if (isQwestsChangesVisible == value)
                {
                    return;
                }

                isQwestsChangesVisible = value;
                OnPropertyChanged(nameof(IsQwestsChangesVisibleProperty));
            }
        }

        /// <summary>
        ///     Видимость окна быстрого добавления задач
        /// </summary>
        public bool IsQwickAddTaskVisible
        {
            get { return _isQwickAddTaskVisible; }
            set
            {
                if (value == _isQwickAddTaskVisible) return;
                _isQwickAddTaskVisible = value;
                OnPropertyChanged(nameof(IsQwickAddTaskVisible));
            }
        }

        /// <summary>
        ///     Sets and gets Открыть окно показать изменения?. Changes to that property's value raise
        ///     the PropertyChanged event.
        /// </summary>
        public bool IsViewChangesOpenPropertyProperty
        {
            get { return isViewChangesOpenProperty; }

            set
            {
                if (isViewChangesOpenProperty == value)
                {
                    return;
                }

                isViewChangesOpenProperty = value;
                OnPropertyChanged(nameof(IsViewChangesOpenPropertyProperty));
            }
        }

        /// <summary>
        ///     Gets the показать копирование/перемещение требования скилла.
        /// </summary>
        public RelayCommand<NeedTasks> MakeQwestFromTask
        {
            get
            {
                return makeQwestFromTask
                       ?? (makeQwestFromTask = new RelayCommand<NeedTasks>(
                           item =>
                           {
                               var thisAim = SelectedAimProperty;
                               var childQw = StaticMetods.AddCompositeQwest(Pers, SelectedAimProperty, item.TaskProperty);

                               if (childQw != null)
                               {
                                   // Предыдущие квесты - связь
                                   var prevQwests = thisAim.Needs.ToList();
                                   thisAim.Needs.Clear();
                                   foreach (var qw in prevQwests.ToList())
                                   {
                                       StaticMetods.addChildAim(Pers, childQw, qw);
                                   }
                                   StaticMetods.addChildAim(Pers, thisAim, childQw);
                               }

                               StaticMetods.RefreshAllQwests(Pers, true, true, true);
                               StaticMetods.Locator.AimsVM.SelectedAimProperty = thisAim;
                               Messenger.Default.Send("Требования квеста!!!");
                               item.TaskProperty.Delete(Pers);
                               NeedsRefresh();
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
        ///     Gets the Переместить требование.
        /// </summary>
        public RelayCommand<Aim> MoveNeedCommand
        {
            get
            {
                return moveNeedCommand ?? (moveNeedCommand = new RelayCommand<Aim>(
                    item =>
                    {
                        //switch (WhatNeedMoveProperty)
                        //{
                        //    case "квест":
                        //        item.Needs.Add(SelectedNeedAimProperty);
                        //        SelectedAimProperty.Needs.Remove(SelectedNeedAimProperty);
                        //        break;

                        //    case "задача":
                        //        item.NeedsTasks.Add(SelectedNeedTaskProperty);
                        //        SelectedAimProperty.NeedsTasks.Remove(SelectedNeedTaskProperty);
                        //        break;
                        //}

                        var sn = SelNeed as NeedTasks;
                        if (sn == null) return;

                        item.NeedsTasks.Add(sn);
                        SelectedAimProperty.NeedsTasks.Remove(sn);

                        RefreshQwestInfo();
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
        ///     Gets the Сдвинуть требование задачи вниз.
        /// </summary>
        public RelayCommand<NeedTasks> MoveNeedTaskDownCommand
        {
            get
            {
                return moveNeedTaskDownCommand
                       ?? (moveNeedTaskDownCommand = new RelayCommand<NeedTasks>(
                           item =>
                           {
                               var taskA = item.TaskProperty;
                               var indThis = QwNeedTasks.IndexOf(item);
                               var taskB = QwNeedTasks[indThis + 1].TaskProperty;

                               StaticMetods.Locator.MainVM.MoveTask(taskA, taskB, true);

                               OnPropertyChanged(nameof(QwNeedTasks));
                           },
                           item =>
                           {
                               if (item == null || QwNeedTasks.IndexOf(item) == QwNeedTasks.Count - 1)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть требование задач вверх.
        /// </summary>
        public RelayCommand<NeedTasks> MoveNeedTaskUpCommand
        {
            get
            {
                return moveNeedTaskUpCommand
                       ?? (moveNeedTaskUpCommand = new RelayCommand<NeedTasks>(
                           item =>
                           {
                               var taskA = item.TaskProperty;
                               var indThis = QwNeedTasks.IndexOf(item);
                               var taskB = QwNeedTasks[indThis - 1].TaskProperty;

                               StaticMetods.Locator.MainVM.MoveTask(taskA, taskB, true);

                               OnPropertyChanged(nameof(QwNeedTasks));
                           },
                           item =>
                           {
                               if (item == null || QwNeedTasks.IndexOf(item) == 0)
                               {
                                   return false;
                               }
                               return true;
                           }))
                    ;
            }
        }

        public ucRevardAbilityNeedViewModel NeedAbilitiesDataContext => new ucRevardAbilityNeedViewModel(Pers, SelectedAimProperty.NeedAbilities, null, SelectedAimProperty);

        /// <summary>
        ///     Gets or sets the need qwests view.
        /// </summary>
        public ListCollectionView NeedQwestsView { get; set; }

        public ucRelaysItemsVM NeedsItemsVM { get; set; }

        /// <summary>
        ///     Gets or sets the need tasks view.
        /// </summary>
        public IEnumerable<NeedTasks> NeedTasksView
        {
            get
            {
                var selectedAimProperty = SelectedAimProperty;
                if (selectedAimProperty != null)
                {
                    var needsList = new List<NeedTasks>();
                    var needsWithoutNext = new List<NeedTasks>();

                    var nnn = from needsTask in selectedAimProperty.NeedsTasks
                              let nextAct = (from next in needsTask.TaskProperty.NextActions
                                             where selectedAimProperty.NeedsTasks.Any(q => q.TaskProperty == next)
                                             select next)
                              where nextAct.Any() == false
                              select needsTask;

                    needsWithoutNext.AddRange(nnn);

                    foreach (var needTaskse in selectedAimProperty.NeedsTasks)
                    {
                        needTaskse.ChildNeeds = new List<NeedTasks>();
                    }

                    foreach (var needTaskse in needsWithoutNext)
                    {
                        needsList.Add(needTaskse);
                        addChilds(SelectedAimProperty.NeedsTasks, needTaskse);
                    }

                    return
                        needsList;
                }
                return null;
            }
        }

        /// <summary>
        ///     Gets the комманда Ок добавить требование для задач.
        /// </summary>
        public RelayCommand OkAddTaskNeedCommand
        {
            get
            {
                return okAddTaskNeedCommand ?? (okAddTaskNeedCommand = new RelayCommand(
                    () =>
                    {
                        if (IsEditNeedProperty == false)
                        {
                            SelectedAimProperty.NeedsTasks.Add(SelectedNeedTaskProperty);
                            SelectedNeedTaskProperty.TaskProperty.ChangeValuesOfRelaytedItems();
                        }

                        NeedsRefresh();
                    },
                    () =>
                    {
                        if (SelectedNeedTaskProperty?.TaskProperty == null)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }

        /// <summary>
        ///     Gets the Кнопка ок в целях.
        /// </summary>
        public RelayCommand OkAimCommand
        {
            get
            {
                return okAimCommand ?? (okAimCommand = new RelayCommand(
                    () =>
                    {
                        var cc = SelectedAimProperty;
                        cc.OnPropertyChanged(nameof(Aim.Skills));
                        cc.RefrDescr();
                        SelectedAimProperty = null;
                        SelectedAimProperty = cc;

                        StaticMetods.RefreshAllQwests(Pers, true, true, true);
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the ок быстрое добавление задач.
        /// </summary>
        public RelayCommand OkQwickAddTasksCommand
        {
            get
            {
                return okQwickAddTasksCommand
                       ?? (okQwickAddTasksCommand =
                           new RelayCommand(
                               () =>
                               {
                                   var addTask = new AddOrEditTaskView();
                                   var context = (UcTasksSettingsViewModel)addTask.UcTasksSettingsView.DataContext;

                                   foreach (var name in QwickAddTasksList)
                                   {
                                       context.AddNewTask(SelectedAimProperty.TypeOfTaskDefoultProperty);
                                       var tsk = context.SelectedTaskProperty;
                                       context.TaskBalanceDefoults();
                                       tsk.NameOfProperty = name.name;
                                       Task.RecountTaskLevel(tsk);
                                       var needTask = GetDefoultNeedTask(tsk);
                                       SelectedAimProperty.NeedsTasks.Add(needTask);
                                   }

                                   RefreshQwestInfo();
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Открыть карту задач квеста.
        /// </summary>
        public RelayCommand OpenQwestTaskMapCommand
        {
            get
            {
                return openQwestTaskMapCommand
                       ?? (openQwestTaskMapCommand = new RelayCommand(
                           () =>
                           {
                               var qt = new QwestsMapTasksWindow();

                               qt.btnClose.Click += (sender, args) =>
                               {
                                   qt.Close();
                                   RefreshInfoCommand.Execute(null);
                               };

                               PersSettingsViewModel.refreshQwestTasks(SelectedAimProperty);

                               qt.ShowDialog();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Персонаж. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Pers Pers
        {
            get { return pers; }

            set
            {
                if (pers == value)
                {
                    return;
                }

                pers = value;
                OnPropertyChanged(nameof(Pers));
            }
        }

        /// <summary>
        ///     Gets the Предыдущий/следующий квест.
        /// </summary>
        public RelayCommand<string> PrevNextCommand
        {
            get
            {
                return prevNextCommand
                       ?? (prevNextCommand = new RelayCommand<string>(
                           item =>
                           {
                               Aim other = null;
                               var it = SelectedAimProperty;
                               var aimCollection = StaticMetods.Locator.AimsVM.QCollectionViewProperty;
                               var ind = aimCollection.IndexOf(it);

                               if (item == "next")
                               {
                                   ind = aimCollection.Count > ind + 1 ? ind + 1 : 0;
                               }
                               else if (item == "prev")
                               {
                                   ind = ind - 1 >= 0 ? ind - 1 : aimCollection.Count - 1;
                               }

                               other = aimCollection.GetItemAt(ind) as Aim;

                               if (other != null && other != it)
                               {
                                   OkAimCommand.Execute(null);
                                   SelectedAimProperty = other;
                               }
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
        ///     Сложности квеста
        /// </summary>
        public List<QwestHardness> QwestHardnesses
        {
            get
            {
                var qwestHardness = new List<QwestHardness>
                {
                    new QwestHardness("Легко", 1, "Меньше 1 дня"),
                    new QwestHardness("Норм", 2, "1-2 дня"),
                    new QwestHardness("Сложно", 3, "Несколько дней упорной работы"),
                    new QwestHardness("Оч. сложно", 4, "Сложный, долгий проект")
                };

                return qwestHardness;
            }
        }

        /// <summary>
        ///     Gets Награды за квест.
        /// </summary>
        public IEnumerable<Revard> QwestRewardsProperty
        {
            get
            {
                var qwestRewardsProperty =
                    Pers.ShopItems.Where(n => n.NeedQwests.Any(q => q == SelectedAimProperty));
                return qwestRewardsProperty;
            }
        }

        /// <summary>
        ///     Sets and gets Для отображения изменений в квестах после щелчка завершено. Changes to that
        ///     property's value raise the PropertyChanged event.
        /// </summary>
        public QwestsChangesModele QwestsChangesProperty
        {
            get { return qwestsChanges; }

            set
            {
                if (qwestsChanges == value)
                {
                    return;
                }

                qwestsChanges = value;
                OnPropertyChanged(nameof(QwestsChangesProperty));
            }
        }

        /// <summary>
        ///     Список задач для быстрого добавления задач
        /// </summary>
        public List<QwickAdd> QwickAddTasksList
        {
            get { return _qwickAddTasksList; }
            set
            {
                if (Equals(value, _qwickAddTasksList)) return;
                _qwickAddTasksList = value;
                OnPropertyChanged(nameof(QwickAddTasksList));
                OnPropertyChanged(nameof(OkQwickAddTasksCommand));
            }
        }

        /// <summary>
        ///     Gets the Быстро задать дату.
        /// </summary>
        public RelayCommand<string> QwickSetDateCommand
        {
            get
            {
                return qwickSetDateCommand
                       ?? (qwickSetDateCommand = new RelayCommand<string>(
                           item =>
                           {
                               if (item == "сегодня")
                               {
                                   SelectedAimProperty.BeginDate = MainViewModel.selectedTime;
                               }
                               else if (item == "завтра")
                               {
                                   SelectedAimProperty.BeginDate = MainViewModel.selectedTime.AddDays(1);
                               }
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

        public List<CompositeAims> QwNeedQwests
        {
            get { return SelectedAimProperty?.QwNeedQwests; }
        }

        public List<NeedTasks> QwNeedTasks
        {
            get { return SelectedAimProperty?.NeedsTasks.OrderBy(n => n.TaskProperty).ToList(); }
        }

        /// <summary>
        ///     Gets the комманда Обновление информации.
        /// </summary>
        public RelayCommand RefreshInfoCommand
        {
            get
            {
                return refreshInfoCommand
                       ?? (refreshInfoCommand =
                           new RelayCommand(() => { RefreshQwestInfo(); }, () => { return true; }));
            }
        }

        public ucRelaysItemsVM RelaysItemsVm { get; set; }

        /// <summary>
        ///     Gets the Удалить связь - составной квест.
        /// </summary>
        public RelayCommand<object> RemoveCompositeQwestNeedCommand
        {
            get
            {
                return removeCompositeQwestNeedCommand
                       ?? (removeCompositeQwestNeedCommand = new RelayCommand<object>(
                           item =>
                           {
                               var qw = item as CompositeAims;
                               SelectedAimProperty.DeleteCompositeQwestNeed(Pers, qw);
                               OnPropertyChanged(nameof(QwNeedQwests));
                               NeedsRefresh();
                               RefreshInfoCommand.Execute(null);
                           },
                           item =>
                           {
                               var qw = item as CompositeAims;
                               if (qw == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Удалить требование задачи (или задачу).
        /// </summary>
        public RelayCommand<NeedTasks> RemoveTaskNeedCommand
        {
            get
            {
                return removeTaskNeedCommand ?? (removeTaskNeedCommand = new RelayCommand<NeedTasks>(
                    item =>
                    {
                        SelectedAimProperty.DeleteTaskNeed(Pers, item);
                        NeedsRefresh();
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

        public ucRelaysItemsVM ReqvireItemsVm { get; set; }

        /// <summary>
        ///     Sets and gets Выбранная цель. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim SelectedAimProperty
        {
            get { return Pers.SellectedAimProperty; }

            set
            {
                Pers.SellectedAimProperty = value;
                OnPropertyChanged(nameof(SelectedAimProperty));
                OnPropertyChanged(nameof(IsQwestDoneProperty));
                OnPropertyChanged(nameof(AllAimsProperty));
                OnPropertyChanged(nameof(HardnessOfQwest));
                OnPropertyChanged(nameof(QwNeedTasks));
                OnPropertyChanged(nameof(QwNeedQwests));
                RefreshQwestInfo();
            }
        }

        /// <summary>
        ///     Sets and gets Выбранный дочерний квест. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public Aim SelectedNeedAimProperty
        {
            get { return selectedNeedAim; }

            set
            {
                if (selectedNeedAim == value)
                {
                    return;
                }

                selectedNeedAim = value;
                OnPropertyChanged(nameof(SelectedNeedAimProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Выделенное требование для задач. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public NeedTasks SelectedNeedTaskProperty
        {
            get { return selectedNeedTask; }

            set
            {
                if (selectedNeedTask == value)
                {
                    return;
                }

                selectedNeedTask = value;
                OnPropertyChanged(nameof(SelectedNeedTaskProperty));
            }
        }

        /// <summary>
        ///     Выбранное требование
        /// </summary>
        public object SelNeed
        {
            get { return _selNeed; }
            set
            {
                if (Equals(value, _selNeed)) return;
                _selNeed = value;
                OnPropertyChanged(nameof(SelNeed));
            }
        }

        /// <summary>
        ///     Показать скилл из прокачки скиллов
        /// </summary>
        public RelayCommand<object> ShowAbFromAbLevUpsCommand
        {
            get
            {
                return _showAbFromAbLevUps ??
                       (_showAbFromAbLevUps = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as AbilitiModel;
                               it?.EditAbility();
                               RefreshQwestInfo();
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Gets the Показать перемещение требования.
        /// </summary>
        public RelayCommand<object> ShowMoveNeedCommand
        {
            get
            {
                return showMoveNeedCommand ?? (showMoveNeedCommand = new RelayCommand<object>(
                    item =>
                    {
                        SelectedNeedAimProperty = item as Aim;
                        if (SelectedAimProperty == null)
                        {
                            return;
                        }
                        WhatNeedMoveProperty = "квест";
                        IsOpenMoveNeedProperty = true;
                    },
                    item =>
                    {
                        var it = item as Aim;
                        if (it == null)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }

        /// <summary>
        ///     Показать скилл из требований скилла
        /// </summary>
        public RelayCommand<object> ShowNeedAbCommand
        {
            get
            {
                return _showNeedAb ??
                       (_showNeedAb = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as NeedAbility;
                               it.AbilProperty.EditAbility();
                               RefreshQwestInfo();
                           },
                           item =>
                           {
                               if (!(item is NeedAbility))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Показать характеристику из требований квеста
        /// </summary>
        public RelayCommand<object> ShowNeedChaCommand
        {
            get
            {
                return _showNeedCha ??
                       (_showNeedCha = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as NeedCharact;
                               it.CharactProperty.EditCharacteristic();
                               RefreshQwestInfo();
                           },
                           item =>
                           {
                               if (!(item is NeedCharact))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Gets the Показать квест.
        /// </summary>
        public RelayCommand<object> ShowQwestCommand
        {
            get
            {
                return showQwestCommand ?? (showQwestCommand = new RelayCommand<object>(
                    item =>
                    {
                        var ca = item as CompositeAims;
                        var qw = item as Aim;

                        var aaim = ca?.AimProperty ?? qw;

                        var selAim = SelectedAimProperty;
                        StaticMetods.editAim(aaim);
                        SelectedAimProperty = selAim;
                    },
                    item =>
                    {
                        var ca = item as CompositeAims;
                        var qw = item as Aim;
                        if (ca == null && qw == null)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }

        /// <summary>
        ///     Gets the Показать задачи.
        /// </summary>
        public RelayCommand<object> ShowTaskCommand
        {
            get
            {
                return showTaskCommand ?? (showTaskCommand = new RelayCommand<object>(
                    it =>
                    {
                        var item = it as NeedTasks;
                        var item2 = it as Task;
                        var tsk = item?.TaskProperty ?? item2;
                        tsk?.EditTask();
                        var it3 = it as AbilitiModel;
                        it3?.EditAbility();
                        OnPropertyChanged(nameof(SelectedAimProperty));
                        NeedsRefresh();
                    },
                    it =>
                    {
                        var item = it as NeedTasks;
                        var item2 = it as Task;
                        var tsk = item?.TaskProperty ?? item2;
                        var it3 = it as AbilitiModel;
                        if (tsk == null && it3 == null)
                        {
                            return false;
                        }

                        var firstOrDefault = Pers.Tasks.FirstOrDefault(n => n == tsk);
                        if (firstOrDefault == null && it3 == null)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }

        /// <summary>
        ///     Sets and gets Фильтр для задач. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public string TasksFilterProperty
        {
            get { return tasksFilter; }

            set
            {
                if (tasksFilter == value)
                {
                    return;
                }

                tasksFilter = value;
                OnPropertyChanged(nameof(TasksFilterProperty));

                if (IsFilterSkills)
                {
                    TasksNeedsProperty = GetQwestSkills();
                }
                else
                {
                    TasksNeedsProperty = GetQwestNeeds();
                }

                SelectedNeedTaskProperty.TaskProperty = TasksNeedsProperty.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Gets or sets Задачи для требований квеста.
        /// </summary>
        public IOrderedEnumerable<Task> TasksNeedsProperty
        {
            get { return _tasksNeedsProperty; }
            set
            {
                _tasksNeedsProperty = value;
                OnPropertyChanged(nameof(TasksNeedsProperty));
            }
        }

        /// <summary>
        ///     Gets or sets Тип для задач по умолчанию.
        /// </summary>
        public TypeOfTask taskTypeDefoultProperty
        {
            get
            {
                var selectedAimProperty = SelectedAimProperty;
                return selectedAimProperty?.TypeOfTaskDefoultProperty;
            }

            set
            {
                if (taskTypeDefoultProperty == value)
                {
                    return;
                }

                SelectedAimProperty.TypeOfTaskDefoultProperty = value;
                OnPropertyChanged(nameof(taskTypeDefoultProperty));
            }
        }

        /// <summary>
        ///     Gets the Создать квест на основе этой задачи.
        /// </summary>
        public RelayCommand<Task> ToQwestCommand
        {
            get
            {
                return toQwestCommand
                       ?? (toQwestCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.ToQwest(Pers);
                               NeedsRefresh();
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

        public ucItemRevardsViewModel ucItemRevardsViewModel
        {
            get
            {
                return new ucItemRevardsViewModel(SelectedAimProperty);
            }
        }

        /// <summary>
        ///     Gets the Поднять или понизить минимальный уровень скилла.
        /// </summary>
        public RelayCommand<string> UpDownMinLevelCommand
        {
            get
            {
                return upDownMinLevelCommand ?? (upDownMinLevelCommand = new RelayCommand<string>(
                    item =>
                    {
                        switch (item)
                        {
                            case "UP":
                                SelectedAimProperty.MinLevelProperty++;
                                break;

                            case "DOWN":
                                SelectedAimProperty.MinLevelProperty--;
                                break;

                            case "!":
                                SelectedAimProperty.MinLevelProperty = Pers.PersLevelProperty;
                                break;
                        }
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
        ///     Gets the Просмотр и редактирование награды.
        /// </summary>
        public RelayCommand<Revard> ViewRewardCommand
        {
            get
            {
                return viewRewardCommand
                       ?? (viewRewardCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               ucRewardsViewModel.EditReward(item);
                               RefreshQwestInfo();
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
        ///     Sets and gets Текст какое требование перенести или копировать. Changes to that property's
        ///     value raise the PropertyChanged event.
        /// </summary>
        public string WhatNeedMoveProperty
        {
            get { return whatNeedMove; }

            set
            {
                if (whatNeedMove == value)
                {
                    return;
                }

                whatNeedMove = value;
                OnPropertyChanged(nameof(WhatNeedMoveProperty));
            }
        }

        public List<AbilitiModel> OrderedAbsToUps
        {
            get
            {
                if (SelectedAimProperty?.UpUbilitys != null)
                    return
                        StaticMetods.PersProperty.Abilitis.Except(SelectedAimProperty?.UpUbilitys.Select(n => n.Ability))
                            .OrderBy(n => n.NameOfProperty)
                            .ToList();
                else
                {
                    return new List<AbilitiModel>();
                }
            }
        }

        /// <summary>
        ///     Добавить артефакт
        /// </summary>
        /// <param name="persParam"></param>
        public static List<Revard> AddArtefacts(Pers persParam)
        {
            List<Revard> rv = new List<Revard>();

            // Добавляем артефакты и бейджи
            foreach (
                Revard revard in
                    persParam.ShopItems.Where(n => n.IsArtefact)
                        .Where(n =>
                        {
                            var observableCollection = new ObservableCollection<string>();
                            return StaticMetods.IsEnRev(n, false, persParam.GoldProperty, persParam.PersLevelProperty,
                                out observableCollection);
                        })
                        .ToList())
            {
                rv.Add(revard);
                persParam.InventoryItems.Add(revard);
                persParam.ShopItems.Remove(revard);
            }

            // Удаляем неподходящие бейджи
            foreach (
                Revard revard in
                    persParam.InventoryItems.Where(n => n.IsBaige)
                        .Where(n =>
                        {
                            var observableCollection = new ObservableCollection<string>();
                            return
                                !StaticMetods.IsEnRev(n, false, persParam.GoldProperty, persParam.PersLevelProperty,
                                    out observableCollection);
                        })
                        .ToList())
            {
                rv.Add(revard);
                persParam.ShopItems.Add(revard);
                persParam.InventoryItems.Remove(revard);
            }
            return rv;
        }

        /// <summary>
        ///     Добавить задачу для требований квеста
        /// </summary>
        /// <param name="fromeAbilitisProperty"></param>
        /// <param name="selectedAimProperty"></param>
        /// <param name="persProperty"></param>
        public static void AddTaskToQwestNeeds(bool fromeAbilitisProperty, Aim selectedAimProperty, Pers persProperty)
        {
            var showTask = new AddOrEditTaskView();

            // Задаем влияние по умолчанию
            var dataContext = showTask.UcTasksSettingsView.DataContext as UcTasksSettingsViewModel;

            showTask.UcTasksSettingsView.btnOk.Click += (sender, args) =>
            {
                // Обновляем

                if (fromeAbilitisProperty == false && selectedAimProperty != null)
                {
                    var needTasks =
                        GetDefoultNeedTask(dataContext.SelectedTaskProperty);

                    selectedAimProperty.NeedsTasks.Add(needTasks);
                }

                showTask.Close();
            };

            showTask.UcTasksSettingsView.btnCansel.Click += (sender, args) => { showTask.Close(); };

            var taskType = selectedAimProperty != null
                ? selectedAimProperty.TypeOfTaskDefoultProperty
                : persProperty.TasksTypes.FirstOrDefault();

            dataContext.AddNewTask(taskType);

            // showTask.Topmost = true;
            showTask.ShowDialog();
        }

        /// <summary>
        ///     Получаем требование по умолчанию для задачи квеста
        /// </summary>
        /// <param name="taskToNeed">задача для требования</param>
        /// <returns>Требование</returns>
        public static NeedTasks GetDefoultNeedTask(Task taskToNeed)
        {
            var koef = StaticMetods.DefoultKForTaskNeed;

            return new NeedTasks
            {
                TaskProperty = taskToNeed,
                TypeNeedProperty = "Выполнена на данный момент",
                KoeficientProperty = koef,
                IsValueProperty = 0.0,
                ValueProperty = 1.0,
                FirstValueProperty = 0.0,
                IsAgreeProperty = 0
            };
        }

        public static IOrderedEnumerable<AbilitiModel> getListOfAbilitiesToChoose(
            IEnumerable<AbilitiModel> abilitiModels)
        {
            return
                StaticMetods.PersProperty.Abilitis.Except(abilitiModels)
                    .OrderByDescending(n => n.IsEnebledProperty)
                    .ThenBy(n => n.NameOfProperty);
        }

        /// <summary>
        ///     Показываем изменения, связанные с изменением окончания квеста
        /// </summary>
        /// <param name="persParam">перс</param>
        /// <param name="selectedAimProperty">цель</param>
        /// <param name="isDoneProperty">значение сделанности квеста</param>
        public static void GetQwestChanges(Pers persParam, Aim selectedAimProperty, bool isDoneProperty)
        {
            var vc = new ViewChangesClass(persParam.InventoryItems.Union(persParam.ShopItems).ToList());
            vc.GetValBefore();

            // Меняем значения Меняем значение сделанности квеста
            selectedAimProperty.IsDoneProperty = isDoneProperty;

            var chAbs = from abilitiModel in persParam.Abilitis
                        from nA in abilitiModel.NeedAims
                        where nA.AimProperty == selectedAimProperty
                        select new { Ability = abilitiModel, ChangeValue = nA.KRel };

            // Меняем награду
            if (selectedAimProperty.IsDoneProperty)
            {
                StaticMetods.PlaySound(Resources.qwestDone);
                // Прибавляем к золоту
                persParam.GoldProperty += selectedAimProperty.GoldIfDoneProperty;
                //// Прибавляем к скиллам
                //foreach (var ab in chAbs)
                //{
                //    ab.Ability.ValueProperty += ab.ChangeValue * 20.0;
                //}

                // Прибавляем к скиллам
                foreach (var source in selectedAimProperty.UpUbilitys.ToList())
                {
                    source.Ability.ValueProperty += source.ValueToUp;
                    selectedAimProperty.UpUbilitys.Remove(source);
                }

                // Прибавляем к опыту
                persParam.PersExpFromeTasksAndQwests += selectedAimProperty.PlusExp;
            }
            else
            {
                persParam.GoldProperty -= selectedAimProperty.GoldIfDoneProperty;

                // Отнимаем от опыта
                persParam.PersExpFromeTasksAndQwests -= selectedAimProperty.PlusExp;

                //// Отнимаем от скиллов
                //foreach (var ab in chAbs)
                //{
                //    ab.Ability.ValueProperty -= ab.ChangeValue * 20.0;
                //}
            }

            StaticMetods.RecountPersExp();

            // Обновляем все квесты
            StaticMetods.RefreshAllQwests(persParam, false, false, true);

            // Обновляем все скиллы Расчет значений скиллов, на которые влияет этот квест
            selectedAimProperty.ChangeValuesOfRelaytedItems();
            StaticMetods.AbillitisRefresh(persParam);

            // Обновляем основные элементы игры
            StaticMetods.RefreshAllQwests(persParam, false, true, true);
            StaticMetods.AbillitisRefresh(persParam);
            StaticMetods.refreshShopItems(persParam);

            vc.GetValAfter();

            Brush col = isDoneProperty ? Brushes.Green : Brushes.Red;
            var itemImageProperty =
                StaticMetods.pathToImage(isDoneProperty
                    ? Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png")
                    : Path.Combine(Directory.GetCurrentDirectory(), "Images", "bad.png"));

            var header = isDoneProperty
                ? $"Квест выполнен: \"{selectedAimProperty.NameOfProperty}\""
                : $"Квест не выполнен: \"{selectedAimProperty.NameOfProperty}\"";
            vc.ShowChanges(header, col, itemImageProperty);
        }

        /// <summary>
        ///     The add new task command execute.
        /// </summary>
        /// <param name="_type">The _type.</param>
        public void AddNewTaskCommandExecute(TypeOfTask _type)
        {
            var selAim = SelectedAimProperty;

            IsOpenTaskNeedProperty = false;

            var add = Task.AddTask(_type, null, SelectedAimProperty);

            if (add.Item1)
            {
                add.Item2.ChangeValuesOfRelaytedItems();
                NeedsRefresh();
            }

            SelectedAimProperty = selAim;
            RefreshQwestInfo();
        }

        /// <summary>
        ///     Заполняем награды - то на что влияет этот квест! Что прокачивает и что дается!
        /// </summary>
        public void getRelaysItems()
        {
            var _qwest = SelectedAimProperty;
            var relaysItems = new List<RelaysItem>();
            if (_qwest == null)
            {
                RelaysItemsVm.RelaysItemsesProperty = relaysItems;
                return;
            }

            // Опыт
            if (SelectedAimProperty.PlusExp != 0)
            {
                relaysItems.Add(new RelaysItem
                {
                    IdProperty = "exp",
                    ElementToolTipProperty = "Опыт",
                    PictureProperty = StaticMetods.getImagePropertyFromImage(Pers.ExpImageProperty),
                    ReqvirementTextProperty = $"+\"{ExpForQwest}\""
                });
            }

            // скиллы
            //var abils = (from abilitiModel in Pers.Abilitis
            //             from needAims in abilitiModel.NeedAims
            //             where needAims.AimProperty == _qwest
            //             where needAims.KRelRound > 0
            //             select new RelaysItem
            //             {
            //                 IdProperty = abilitiModel.GUID,
            //                 ReqvirementTextProperty = $"+{needAims.KRelRound}",
            //                 KRelayProperty = needAims.KRelRound,
            //                 ElementToolTipProperty = $"Навык \"{abilitiModel.NameOfProperty}\"",
            //                 PictureProperty = abilitiModel.PictureProperty
            //             }).ToList();
            //relaysItems.AddRange(abils);

            // Золото
            if (GoldForQwest > 0)
            {
                relaysItems.Add(new RelaysItem
                {
                    IdProperty = "gold",
                    ElementToolTipProperty = "Золото",
                    PictureProperty = StaticMetods.getImagePropertyFromImage(Pers.GoldImageProperty),
                    ReqvirementTextProperty = $"+\"{GoldForQwest}\""
                });
            }

            // Артефакты
            var artefacts = from shopItem in Pers.ShopItems
                            where shopItem.NeedQwests.Any(q => q == _qwest)
                            select new RelaysItem
                            {
                                IdProperty = shopItem.GUID,
                                ElementToolTipProperty = $"Награда \"{shopItem.NameOfProperty}\"",
                                PictureProperty = shopItem.PictureProperty
                            };
            relaysItems.AddRange(artefacts);

            RelaysItemsVm.RelaysItemsesProperty = relaysItems;
        }

        /// <summary>
        ///     Обновляем требования для квеста
        /// </summary>
        public void NeedsRefresh()
        {
            SelectedNeedTaskProperty = null;

            // this.AbilitisFilterProperty = string.Empty; this.TasksFilterProperty = string.Empty;
            TasksNeedsProperty = GetQwestNeeds();

            if (SelectedAimProperty == null)
            {
                return;
            }

            // this.coundNeedsInQwests();
            SelectedAimProperty.CountAutoProgress();

            NeedQwestsView =
                (ListCollectionView)new CollectionViewSource { Source = SelectedAimProperty.Needs }.View;

            OnPropertyChanged(nameof(NeedQwestsView));

            NeedQwestsView.Refresh();
            OnPropertyChanged(nameof(NeedTasksView));

            MainViewModel.SaveLogTxt("Обновились требования для задач");

            OnPropertyChanged(nameof(QwNeedTasks));
            OnPropertyChanged(nameof(QwNeedQwests));
            OnPropertyChanged(nameof(Aim.LinksOfTasks));
            SelectedAimProperty.RefreshActiveSkills();
            SelectedAimProperty.RefreshLinks();
        }

        public void QwickAdd()
        {
            QwickAddTasksList = new List<QwickAdd>();

            var qw = new QwickAddTasksView { DataContext = this };
            qw.btnCansel.Click += (sender, args) => { qw.Close(); };

            qw.btnOk.Click += (sender, args) =>
            {
                qw.Close();
                QwickAddElement(QwickAddTasksList);
            };

            qw.ShowDialog();
        }

        public void QwickAddElement(List<QwickAdd> qwickAddTasksList)
        {
            OkQwickAddTasksCommand.Execute(null);
        }

        /// <summary>
        ///     Обновить информацию квеста
        /// </summary>
        public void RefreshQwestInfo()
        {
            OnPropertyChanged(nameof(SelectedAimProperty));
            if (SelectedAimProperty != null)
            {
                SelectedAimProperty.IsActiveProperty = AimsViewModel.IsQwestActive(SelectedAimProperty,
                    StaticMetods.PersProperty);
                SelectedAimProperty.RefreshDoneVisibillity();
                NeedsRefresh();
                OnPropertyChanged(nameof(colorProperty));
                OnPropertyChanged(nameof(taskTypeDefoultProperty));
                OnPropertyChanged(nameof(QwestRewardsProperty));
                OnPropertyChanged(nameof(AllNeeds));
                OnPropertyChanged(nameof(QwNeedTasks));
                OnPropertyChanged(nameof(QwNeedQwests));
                OnPropertyChanged(nameof(GoldForQwest));
                OnPropertyChanged(nameof(AbsLevelUp));
                SelectedAimProperty.RefreshActiveSkills();
                getRelaysItems();
                OnPropertyChanged(nameof(ucItemRevardsViewModel));
                SelectedAimProperty.RefreshRev();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void addChilds(ObservableCollection<NeedTasks> needsTasks,
                    NeedTasks needTaskse)
        {
            var prev = from need in needsTasks
                       from needNextAction in need.TaskProperty.NextActions
                       where needNextAction == needTaskse.TaskProperty
                       select need;

            foreach (var prevNeed in prev)
            {
                needTaskse.ChildNeeds.Add(prevNeed);
                addChilds(needsTasks, prevNeed);
            }
        }

        /// <summary>
        ///     Выбрать скилл для того, чтобы привязать его к квесту
        /// </summary>
        private void ChooseAbilityToQwestLink()
        {
            var abs = Pers.Abilitis.Except(SelectedAimProperty.AbilitiLinksOf).OrderBy(n => n.NameOfProperty).ToList();
            ListCollectionView lv = (ListCollectionView)new CollectionViewSource { Source = abs }.View;
            var cho = new ChooseAbility { txtHeader = { Text = "Выберите навык" } };
            lv.Filter = o =>
            {
                var ab = o as AbilitiModel;
                if (ab != null)
                {
                    if (ab.NameOfProperty.ToLower().Contains(cho.txtFilter.Text.ToLower()))
                    {
                        return true;
                    }
                }
                return false;
            };
            cho.lstAbbs.ItemsSource = lv;
            cho.txtFilter.TextChanged += (sender, args) => { lv.Refresh(); };
            cho.btnCansel.Click += (sender, args) => { cho.Close(); };
            cho.btnOk.Click += (sender, args) =>
            {
                var ab = cho.lstAbbs.SelectedValue as AbilitiModel;
                if (ab == null) return;

                SelectedAimProperty.AbilitiLinksOf.Add(ab);
                cho.Close();
            };

            FocusManager.SetFocusedElement(cho, cho.txtFilter);
            cho.ShowDialog();
        }

        private IOrderedEnumerable<Task> GetQwestNeeds()
        {
            return Pers?.Tasks.Where(
                n =>
                    n.IsDelProperty == false &&
                    MainViewModel.IsTaskVisibleInCurrentView(n, null, Pers, true, true, true)
                    && n.NameOfProperty.ToLower().Contains(TasksFilterProperty.ToLower()))
                .OrderBy(n => n.NameOfProperty);
        }

        /// <summary>
        ///     Скиллы для квеста
        /// </summary>
        /// <returns></returns>
        private IOrderedEnumerable<Task> GetQwestSkills()
        {
            var tsks = from abilitiModel in Pers.Abilitis
                       from skill in abilitiModel.Skills
                       where !skill.NeedTask.TaskProperty.IsDelProperty
                       where
                           skill.LevelProperty >= abilitiModel.CellValue &&
                           skill.ToLevelProperty <= abilitiModel.CellValue
                       select skill.NeedTask.TaskProperty;

            tsks = tsks.Distinct().Except(SelectedAimProperty.Skills);

            return tsks
                .Where(n => n.NameOfProperty.ToLower().Contains(TasksFilterProperty.ToLower()))
                .OrderBy(n => n.NameOfProperty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class QwickAdd : INotifyPropertyChanged
    {
        private int _counter;

        private bool _isQwest;

        private int _level;

        private string _name;

        private int _timer;

        /// <summary>
        ///     Счетчик
        /// </summary>
        public int counter
        {
            get { return _counter; }
            set
            {
                if (value == _counter) return;
                _counter = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Квест?
        /// </summary>
        public bool isQwest
        {
            get { return _isQwest; }
            set
            {
                if (value == _isQwest) return;
                _isQwest = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Уровень требования
        /// </summary>
        public int Level
        {
            get { return _level; }
            set
            {
                if (value == _level) return;
                _level = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Имя
        /// </summary>
        public string name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Таймер
        /// </summary>
        public int timer
        {
            get { return _timer; }
            set
            {
                if (value == _timer) return;
                _timer = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
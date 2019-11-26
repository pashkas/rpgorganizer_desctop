using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Sample.Annotations;
using Sample.Model;
using Sample.Properties;
using Sample.View;

namespace Sample.ViewModel
{
    /// <summary>
    /// Требования задачи
    /// </summary>
    public partial class AddOrEditAbilityViewModel : INotifyPropertyChanged
    {
        public RelayCommand addNeedOfQwestCommand;
        public RelayCommand addNeedTaskNoParamCommand;
        public RelayCommand addQwToUpAbCommand;
        public RelayCommand exportItCommand;
        public RelayCommand<object> showAbReq;

        /// <summary>
        /// Добавить ссылку на квест.
        /// </summary>
        private RelayCommand _addLinkToQwestCommand;

        /// <summary>
        /// Добавить задачу в требования скилла.
        /// </summary>
        private RelayCommand _addNeedTaskToAbilityCommand;

        /// <summary>
        /// Добавить новый квест в требования скилла.
        /// </summary>
        private RelayCommand _addReqAimCommand;

        private IEnumerable<Aim> _allAimsToAbUps = new List<Aim>();
        private string _chaRelsString;

        /// <summary>
        /// Выбрать квест для требований квестов для скилла.
        /// </summary>
        private RelayCommand _chooseAimReqCommand;

        /// <summary>
        /// Выбрать квест, который добавить для ссылки.
        /// </summary>
        private RelayCommand _chooseLinkToQwestCommand;

        /// <summary>
        /// Выбрать квест для добавления к скиллам
        /// </summary>
        private RelayCommand<object> _chooseQwestToSkill;

        /// <summary>
        /// Удалить условие
        /// </summary>
        private RelayCommand<object> _delObject;

        /// <summary>
        /// Удалить требования квеста для доступности скилла
        /// </summary>
        private RelayCommand<object> _delQwReq;

        /// <summary>
        /// Показать объект
        /// </summary>
        private RelayCommand<object> _editObject;

        private bool _isSetViz;

        /// <summary>
        /// Открыть редактируемую характеристику
        /// </summary>
        private RelayCommand<object> _openRelayCharacteristic;

        private List<UpUbility> _qwUpAb = new List<UpUbility>();
        private List<RelCha> _relaysToChas;
        private object _selAbNeed;

        /// <summary>
        /// Посмотреть квест из требований квеста
        /// </summary>
        private RelayCommand<object> _showQwestReq;

        /// <summary>
        /// Комманда Открыть редактирование всех требований скилла.
        /// </summary>
        private RelayCommand abNeedsEditorCommand;

        /// <summary>
        /// Gets the Добавить в квест прокачку этого навыка.
        /// </summary>
        private RelayCommand<Aim> addAbUpsAfterChooseCommand;

        

        /// <summary>
        /// Gets the Добавить ссылку на квест из выбранного квеста.
        /// </summary>
        private RelayCommand<Aim> addNodeCommand;

        /// <summary>
        /// Gets the Добавить квест к скиллу.
        /// </summary>
        private RelayCommand<object> addQwestToSkillCommand;

        /// <summary>
        /// Gets the Добавить требование задач.
        /// </summary>
        [field: NonSerialized]
        private RelayCommand<int> addTaskNeedCommand;

        /// <summary>
        /// Комманда Автозаполнение условий скилла.
        /// </summary>
        private RelayCommand autoFillCommand;

        /// <summary>
        /// Комманда Купить новый уровень для скилла!.
        /// </summary>
        private RelayCommand buyAbCommand;

        /// <summary>
        /// Комманда Отмена.
        /// </summary>
        private RelayCommand canselCommand;

        /// <summary>
        /// Gets the Изменить модификатор забывания скилла.
        /// </summary>
        private RelayCommand<double> changeModOfForgetCommand;

        /// <summary>
        /// Gets the Изменить модификатор скорости обучения.
        /// </summary>
        private RelayCommand<double> changeModOfLearnCommand;

        /// <summary>
        /// Комманда Очистить картинку.
        /// </summary>
        private RelayCommand clearImagePropertyCommand;

        /// <summary>
        /// Комманда Очистить скиллы.
        /// </summary>
        private RelayCommand clearSkillsCommand;

        /// <summary>
        /// Gets the Скопировать условия вниз.
        /// </summary>
        private RelayCommand<int> copyTaskUslDownCommand;

        /// <summary>
        /// Gets the Удалить ссылку на квест или сам квест.
        /// </summary>
        private RelayCommand<Aim> delQwestLinkCommand;

        /// <summary>
        /// Gets the Заполнить автоматом след. уровень.
        /// </summary>
        private RelayCommand<int> fillNextLevelCommand;

        /// <summary>
        /// Gets the Заполнить предыдущий уровень.
        /// </summary>
        private RelayCommand<int> fillPrevLevCommand;

        /// <summary>
        /// Видимость настройки начального уровня характеристики.
        /// </summary>
        private Visibility firstLevelVisible;


        /// <summary>
        /// Редактируется ли требование или добавляется?.
        /// </summary>
        private bool isEditNeed;

        /// <summary>
        /// Видимость добавления требования скилла.
        /// </summary>
        private bool isOpenAbilNeed;

        /// <summary>
        /// Комманда description.
        /// </summary>
        private RelayCommand<ComplecsNeed> linkToQwest;

        /// <summary>
        /// Комманда Ок в настройках скилла.
        /// </summary>
        private RelayCommand<string> okCommand;

        /// <summary>
        /// Комманда Открыть карту задач скилла.
        /// </summary>
        private RelayCommand openTaskMapCommand;

        /// <summary>
        /// Комманда Прибавляем уровень к требованию скилла.
        /// </summary>
        private RelayCommand plusAbilNeedLevelCommand;

        /// <summary>
        /// Gets the Прибавить уровень скилла.
        /// </summary>
        private RelayCommand<string> plusLevelCommand;

        /// <summary>
        /// Gets the + и - задать начальный уровень характеристики.
        /// </summary>
        private RelayCommand<string> plusMinusAbFirstLevelCommand;

        /// <summary>
        /// Gets the +1 к уровню скилла.
        /// </summary>
        private RelayCommand<ChangeAbilityModele> plusOneLevelAbilityRelayCommand;

        /// <summary>
        /// Gets the Плюс в задаче.
        /// </summary>
        private RelayCommand<Task> plusTaskCommand;

        /// <summary>
        /// Gets the Предыдущая/следующая характеристика.
        /// </summary>
        private RelayCommand<string> prevNextCommand;

        /// <summary>
        /// Лист коллекция для скиллов.
        /// </summary>
        private ListCollectionView qAbilView;

        /// <summary>
        /// Быстро задать значение влияния на характеристику.
        /// </summary>
        private Characteristic qwickCharacteristic;

        /// <summary>
        /// Gets the Быстро задать или снять влияние скилла на характеристику.
        /// </summary>
        private RelayCommand<Tuple<Characteristic, NeedAbility>> qwickSetAbToChaRelayCommand;

        /// <summary>
        /// Gets the Быстро задать значение влияния на характеристику.
        /// </summary>
        private RelayCommand<string> qwickSetCharactRelayCommand;

        /// <summary>
        /// Gets the Быстро задать сложность задачи (влияет на вероятность появления наград при выполнении).
        /// </summary>
        private RelayCommand<string> qwickSetHardnessCommand;

        /// <summary>
        /// Комманда Обновить активные задачи скилла.
        /// </summary>
        private RelayCommand refreshActiveAbTasksCommand;

        /// <summary>
        /// Комманда Обновление информации.
        /// </summary>
        private RelayCommand refreshInfoCommand;

        /// <summary>
        /// Комманда Сбросить скилл.
        /// </summary>
        private RelayCommand resetAbilityCommand;

        /// <summary>
        /// Выбранное изменение.
        /// </summary>
        private RelTaskToAb selChange;

        /// <summary>
        /// Выбранный квест для требований.
        /// </summary>
        private Aim selectedAimNeed;

        /// <summary>
        /// Выделенное требование для скиллов.
        /// </summary>
        private NeedAbility selectedNeedAbility;

        /// <summary>
        /// Выбранная связанная задача.
        /// </summary>
        private Task selectedTask;

        /// <summary>
        /// Gets the Показать квест для редактирования.
        /// </summary>
        private RelayCommand<Aim> showQwestCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditAbilityViewModel"/> class.
        /// </summary>
        public AddOrEditAbilityViewModel()
        {
            RelaysItemsVm = new ucRelaysItemsVM
            {
                IsNeedsProperty = false,
                IsReqvirementsProperty = false,
                ParrentDataContext = this
            };
            NeedsItemsVM = new ucRelaysItemsVM
            {
                IsNeedsProperty = true,
                IsReqvirementsProperty = false,
                ParrentDataContext = this
            };
            ReqvireItemsVm = new ucRelaysItemsVM
            {
                IsNeedsProperty = false,
                IsReqvirementsProperty = true,
                ParrentDataContext = this
            };

            Messenger.Default.Register<string>(
                this,
                comm =>
                {
                    if (comm == "Начальный или конечный уровень в опыте навыка изменен!")
                    {
                    }

                    if (comm == "Обновить задачи навыков!")
                    {
                    }
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets Скиллы для требований.
        /// </summary>
        public IOrderedEnumerable<AbilitiModel> AbilitisProperty
        {
            get { return PersProperty?.Abilitis.OrderBy(n => n.NameOfProperty); }
        }

        /// <summary>
        /// Gets the Открыть редактирование всех требований скилла.
        /// </summary>
        public RelayCommand AbNeedsEditorCommand
        {
            get
            {
                return abNeedsEditorCommand
                       ?? (abNeedsEditorCommand =
                           new RelayCommand(
                               () =>
                               {
                                   AbilityNeedSettingsView asv = new AbilityNeedSettingsView();
                                   asv.btnOk.Click += (sender, args) =>
                                   {
                                       asv.Close();
                                       SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                                       RefreshInfoCommand.Execute(null);
                                   };
                                   asv.DataContext = this;
                                   asv.ShowDialog();
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Добавить в квест прокачку этого навыка.
        /// </summary>
        public RelayCommand<Aim> AddAbUpsAfterChooseCommand
        {
            get
            {
                return addAbUpsAfterChooseCommand
                       ?? (addAbUpsAfterChooseCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               item.UpUbilitys.Add(new UpUbility() { Ability = SelectedAbilitiModelProperty, ValueToUp = 1 });
                               RefreshInfoCommand.Execute(null);
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
        /// Комманда Добавить картинку.
        /// </summary>
        private RelayCommand addImagePropertyCommand;

        /// <summary>
        /// Gets the комманда Добавить картинку.
        /// </summary>
        public RelayCommand AddImagePropertyCommand
        {
            get
            {
                return addImagePropertyCommand
                       ?? (addImagePropertyCommand =
                           new RelayCommand(
                               () =>
                               {
                                   SelectedAbilitiModelProperty.ImageProperty =
                                       StaticMetods.GetPathToImage(SelectedAbilitiModelProperty.ImageProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Добавить ссылку на квест.
        /// </summary>
        public RelayCommand AddLinkToQwestCommand
        {
            get
            {
                return _addLinkToQwestCommand
                       ?? (_addLinkToQwestCommand = new RelayCommand(
                           () =>
                           {
                               var childAim = new Aim(PersProperty)
                               {
                                   ImageProperty = SelectedAbilitiModelProperty.ImageProperty,
                                   MinLevelProperty = PersProperty.PersLevelProperty
                               };

                               //if (Keyboard.Modifiers == ModifierKeys.Alt)
                               //{
                               //    AddQwestUpAbilityToSelAbility(childAim);
                               //}
                               //else
                               //{
                               // Добавляем ссылку
                               childAim.AbilitiLinksOf.Add(SelectedAbilitiModelProperty);
                               //}

                               var editQwest = new EditQwestWindowView();
                               StaticMetods.Locator.QwestsVM.SelectedAimProperty = childAim;
                               editQwest.btnOk.Click += (sender, args) =>
                               {
                                   editQwest.Close();
                                   StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.КвестСоздан, editQwest);
                               };

                               editQwest.btnCansel.Click += (sender, args) =>
                               {
                                   StaticMetods.RemoveQwest(PersProperty, childAim);
                                   editQwest.Close();
                               };

                               editQwest.ShowDialog();
                               RefreshInfoCommand.Execute(null);
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Добавить условие - квест, или ссылку
        /// </summary>
        public RelayCommand AddNeedOfQwestCommand
        {
            get
            {
                return addNeedOfQwestCommand ?? (addNeedOfQwestCommand = new RelayCommand(
                    () =>
                    {
                        AddLinkToQwestCommand.Execute(null);
                        SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                        StaticMetods.RecountTaskLevels();
                    },
                    () => true));
            }
        }

        public RelayCommand AddNeedTaskNoParamCommand
        {
            get
            {
                return addNeedTaskNoParamCommand ?? (addNeedTaskNoParamCommand = new RelayCommand(
                    () => { AddTaskNeedCommand.Execute(SelectedAbilitiModelProperty.CellValue); },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Добавить задачу в требования скилла.
        /// </summary>
        public RelayCommand AddNeedTaskToAbilityCommand
        {
            get
            {
                return _addNeedTaskToAbilityCommand
                       ?? (_addNeedTaskToAbilityCommand = new RelayCommand(
                           () =>
                           {
                               if (Keyboard.Modifiers == ModifierKeys.Alt)
                               {
                                   var selTask = SelAbNeed as ComplecsNeed;
                                   if (selTask?.NeedTask != null)
                                   {
                                       CloneTask(selTask.NeedTask, -1, SelectedAbilitiModelProperty);
                                   }
                               }
                               else
                               {
                                   SelectedAbilitiModelProperty.AddNeedTaskCommand.Execute("+");
                               }
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Добавить ссылку на квест из выбранного квеста.
        /// </summary>
        public RelayCommand<Aim> AddNodeCommand
        {
            get
            {
                return addNodeCommand
                       ?? (addNodeCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               item.AbilitiLinksOf.Add(SelectedAbilitiModelProperty);
                               RefreshInfoCommand.Execute(null);
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
        /// Gets the Добавить квест к скиллу.
        /// </summary>
        public RelayCommand<object> AddQwestToSkillCommand
        {
            get
            {
                return addQwestToSkillCommand
                       ?? (addQwestToSkillCommand = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as ComplecsNeed;

                               var skills = new List<Task>();

                               if (Keyboard.Modifiers == ModifierKeys.Control)
                               {
                                   skills.AddRange(
                                       SelectedAbilitiModelProperty.Skills.Select(n => n.NeedTask.TaskProperty));
                               }
                               else
                               {
                                   skills.Add(it.NeedTask.TaskProperty);
                               }

                               StaticMetods.AddNewAim(PersProperty, -1, null, SelectedAbilitiModelProperty, 0, skills);

                               foreach (var complecsNeed in SelectedAbilitiModelProperty.Skills)
                               {
                                   complecsNeed.NeedTask.TaskProperty.RefreshSkillQwests();
                               }
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

        public RelayCommand AddQwToUpAbCommand
        {
            get
            {
                return addQwToUpAbCommand ?? (addQwToUpAbCommand = new RelayCommand(

                    () =>
                    {
                        UpUbility uU = new UpUbility() { Ability = SelectedAbilitiModelProperty, ValueToUp = 1 };
                        StaticMetods.AddNewAim(PersProperty, -1, null, null, 0, null, uU);
                        RefreshInfoCommand.Execute(null);
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Добавить новый квест в требования скилла.
        /// </summary>
        public RelayCommand AddReqAimCommand
        {
            get
            {
                return _addReqAimCommand
                       ?? (_addReqAimCommand = new RelayCommand(
                           () =>
                           {
                               var qw = StaticMetods.AddNewAim(PersProperty);
                               if (qw == null) return;
                               SelectedAbilitiModelProperty.ReqwireAims.Add(qw);
                               RefreshInfoCommand.Execute(null);
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Добавить требование задач.
        /// </summary>
        public RelayCommand<int> AddTaskNeedCommand
        {
            get
            {
                return addTaskNeedCommand
                       ?? (addTaskNeedCommand = new RelayCommand<int>(
                           item =>
                           {
                               SelectedAbilitiModelProperty.DefSelH = SelectedAbilitiModelProperty.HList[item];
                               var nt = SelectedAbilitiModelProperty.NeedTasks
                                  .OrderByDescending(n => n)
                                  .ToList();

                               // Все сразу

                               if (Keyboard.Modifiers == ModifierKeys.Shift)
                               {
                                   double count = nt.Count;
                                   for (int i = 0; i < count; i++)
                                   {
                                       nt[i].LevelProperty = 0;
                                       nt[i].ToLevelProperty = StaticMetods.PersProperty.PersSettings.AbRangs.Count;
                                   }

                                   SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                               }
                               // По очереди
                               else if (Keyboard.Modifiers == ModifierKeys.Control)
                               {
                                   SetNeedTasksQued(nt);

                                   SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                               }
                               // Постепенно
                               else if (Keyboard.Modifiers == ModifierKeys.Alt)
                               {
                                   SetNeedTasksParallel(nt);

                                   SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                               }

                               //else if (Keyboard.Modifiers == ModifierKeys.Control)
                               //{
                               //    foreach (var source in SelectedAbilitiModelProperty.NeedTasks.Where(n => n.LevelProperty < 4).ToList())
                               //    {
                               //        source.TaskProperty?.Delete(PersProperty);
                               //    }

                               // foreach (NeedTasks nt in
                               // SelectedAbilitiModelProperty.NeedTasks.Where(n => n.LevelProperty
                               // == 4 && n.ToLevelProperty == 4).ToList()) { var selTask = nt;
                               // List<Task> tsks = new List<Task>();

                               // for (int i = 0; i < 4; i++) { if (selTask.TaskProperty != null) {
                               // tsks.Add(CloneTask(selTask, i, SelectedAbilitiModelProperty,
                               // false)); } }

                               // var oneTime = selTask.TaskProperty.TimeMustProperty / 5.0; var
                               // onePom = selTask.TaskProperty.PomodorroMax / 5.0; var oneMes =
                               // selTask.TaskProperty.Mesure / 5.0; var oneSub =
                               // selTask.TaskProperty.SubTasks.Count / 5.0;

                               // for (int i = 0; i < 4; i++) { var mesure =
                               // Convert.ToInt32(Math.Floor((i + 1) * oneMes)); if (mesure < 1 &&
                               // selTask.TaskProperty.Mesure > 0) { mesure = 1; } tsks[i].Mesure =
                               // mesure; var pomodorroMax = Convert.ToInt32(Math.Floor((i + 1) *
                               // onePom)); if (pomodorroMax < 1 && selTask.TaskProperty.PomodorroMax
                               // > 0) { pomodorroMax = 1; } tsks[i].PomodorroMax = pomodorroMax; var
                               // timeMustProperty = Convert.ToInt32(Math.Floor((i + 1) * oneTime));
                               // if (timeMustProperty < 1 && selTask.TaskProperty.TimeMustProperty >
                               // 0) { timeMustProperty = 1; } tsks[i].TimeMustProperty =
                               // timeMustProperty; var sT = Convert.ToInt32(Math.Floor((i + 1) *
                               // oneSub)); if (oneSub > 0 && sT < 1) { sT = 1; } if (oneSub > 0) { tsks[i].SubTasks.Clear();

                               // for (int j = 0; j < sT; j++) { tsks[i].SubTasks.Add(new SubTask() {
                               // Guid = new Guid().ToString(), isDone = false, Tittle =
                               // selTask.TaskProperty.SubTasks[j].Tittle }); } } } }

                               //    foreach (Task source in SelectedAbilitiModelProperty.NeedTasks.Select(n => n.TaskProperty).ToList())
                               //    {
                               //        source.BeginDateProperty = MainViewModel.selectedTime;
                               //    }
                               //}
                               //else if (Keyboard.Modifiers == ModifierKeys.Shift)
                               //{
                               //    foreach (var source in SelectedAbilitiModelProperty.NeedTasks.ToList())
                               //    {
                               //        source.TaskProperty?.Delete(PersProperty);
                               //    }
                               //    SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                               //}
                               else
                               {
                                   var levelToNeed = item;
                                   var defType = SelectedAbilitiModelProperty.DefoultTaskType ??
                                                 StaticMetods.PersProperty.TasksTypes.FirstOrDefault(
                                                     n => n.IntervalForDefoult == TimeIntervals.День);
                                   NeedTasks need = null;
                                   // Новая задача
                                   var tsk = Task.AddTask(defType, SelectedAbilitiModelProperty, null, levelToNeed);
                                   need =
                                       SelectedAbilitiModelProperty.NeedTasks.LastOrDefault(
                                           n => n.TaskProperty == tsk.Item2);
                                   if (need != null)
                                   {
                                       var maxAbLev = PersProperty.PersSettings.MaxAbLev;
                                       need.LevelProperty = item < maxAbLev ? item : 0; //maxAbLev-1;
                                       need.ToLevelProperty = item; //GetNeedLev(); //GetNeedLev();
                                   }

                                   SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                                   StaticMetods.RecountTaskLevels();
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
        /// Gets Все цели.
        /// </summary>
        public IEnumerable<Aim> AllAimsProperty
        {
            get
            {
                return
                    PersProperty?.Aims
                        .Where(n => !n.IsDoneProperty)
                        .Where(n => !n.AbilitiLinksOf.Contains(SelectedAbilitiModelProperty))
                        .OrderBy(n => n.NameOfProperty);
            }
        }

        public IEnumerable<Aim> AllAimsToAbUps
        {
            get
            {
                return _allAimsToAbUps;
            }
            set
            {
                if (Equals(value, _allAimsToAbUps)) return;
                _allAimsToAbUps = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Все требования квеста
        /// </summary>
        public List<AllAimNeeds> AllNeeds
        {
            get
            {
                var selAbility = SelectedAbilitiModelProperty;

                if (selAbility == null)
                {
                    return null;
                }

                var qwests = (from compositeAimse in selAbility.NeedAims
                              let qwest = compositeAimse.AimProperty
                              let progressProperty = qwest.AutoProgressValueProperty
                              select
                                  new AllAimNeeds
                                  {
                                      Type = "Квест",
                                      NameOfNeed = compositeAimse.AimProperty.NameOfProperty,
                                      Progress = progressProperty,
                                      Image = compositeAimse.AimProperty.ImageProperty,
                                      GUID = compositeAimse.AimProperty.GUID
                                  }).OrderByDescending(n => n.Progress);

                var taskNeeds = (from needTasks in selAbility.NeedTasks
                                 let progress =
                                     Convert.ToDouble(needTasks.TaskProperty.ValueOfTaskProperty)
                                     / Convert.ToDouble(needTasks.TaskProperty.MaxValueOfTaskProperty) * 100.0
                                 select
                                     new AllAimNeeds
                                     {
                                         Type = "Задача",
                                         NameOfNeed = needTasks.TaskProperty.NameOfProperty,
                                         Progress = progress,
                                         Image = needTasks.TaskProperty.ImageProperty,
                                         GUID = needTasks.TaskProperty.GUID
                                     }).OrderByDescending(n => n.Progress);

                return qwests.Concat(taskNeeds).OrderByDescending(n => n.Progress).ToList();
            }
        }

        /// <summary>
        /// Gets the Купить скилл за очки скиллов.
        /// </summary>
        public RelayCommand BuyAbCommand
        {
            get
            {
                return buyAbCommand ?? (buyAbCommand = new RelayCommand(
                    () =>
                    {
                        var item = SelectedAbilitiModelProperty;
                        AbilitiModel.BuyAbLevel(item, PersProperty, true);
                        PersProperty.UpdateAbilityPoints();
                        RefreshInfoCommand.Execute(null);
                    },
                    () =>
                    {
                        if (SelectedAbilitiModelProperty == null ||
                            SelectedAbilitiModelProperty.IsBuyVisibility == Visibility.Collapsed)
                        {
                            return false;
                        }
                        return true;
                    }));
            }
        }

        /// <summary>
        /// Gets the комманда Отмена.
        /// </summary>
        public RelayCommand CanselCommand
        {
            get
            {
                return canselCommand ?? (canselCommand = new RelayCommand(
                    () =>
                    {
                        StaticMetods.DeleteAbility(PersProperty, SelectedAbilitiModelProperty);
                        SelectedAbilitiModelProperty = null;
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Изменения характеристик
        /// </summary>
        public IEnumerable<Tuple<Characteristic, NeedAbility>> ChangeCharacts
        {
            get
            {
                return from characteristic in PersProperty.Characteristics
                       from needAbility in characteristic.NeedAbilitisProperty
                       where needAbility.AbilProperty == SelectedAbilitiModelProperty
                       select new Tuple<Characteristic, NeedAbility>(characteristic, needAbility);
            }
        }

        /// <summary>
        /// Gets the Изменить модификатор забывания скилла.
        /// </summary>
        public RelayCommand<double> ChangeModOfForgetCommand
        {
            get
            {
                return changeModOfForgetCommand
                       ?? (changeModOfForgetCommand = new RelayCommand<double>(
                           item =>
                           {
                               SelectedAbilitiModelProperty.ModificatorOfForget = item;
                               OnPropertyChanged(nameof(ModificatorsOfForget));
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
        /// Gets the Изменить модификатор скорости обучения.
        /// </summary>
        public RelayCommand<double> ChangeModOfLearnCommand
        {
            get
            {
                return changeModOfLearnCommand
                       ?? (changeModOfLearnCommand = new RelayCommand<double>(
                           item =>
                           {
                               SelectedAbilitiModelProperty.ModificatorLearn = item;
                               OnPropertyChanged(nameof(ModificatorsLearn));
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

        public string ChaRelsString
        {
            get
            {
                return _chaRelsString;
            }
            set
            {
                if (value == _chaRelsString) return;
                _chaRelsString = value;
                OnPropertyChanged(nameof(ChaRelsString));
            }
        }

        /// <summary>
        /// Характеристика для предыдущих/следующих скиллов
        /// </summary>
        public Characteristic ChaToPrevNext { get; set; }

        /// <summary>
        /// Gets the Выбрать квест для требований квестов для скилла.
        /// </summary>
        public RelayCommand ChooseAimReqCommand
        {
            get
            {
                return _chooseAimReqCommand
                       ?? (_chooseAimReqCommand = new RelayCommand(
                           () =>
                           {
                               var cho = new ChooseAbility
                               {
                                   txtHeader = { Text = "Выберите квест" },
                                   lstAbbs =
                                   {
                                       ItemsSource = PersProperty.Aims.Except(SelectedAbilitiModelProperty.ReqwireAims)
                                           .Where(n => !n.IsDoneProperty)
                                           .OrderBy(n => n.NameOfProperty)
                                   }
                               };
                               cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                               cho.btnOk.Click += (sender, args) =>
                               {
                                   var aim = cho.lstAbbs.SelectedValue as Aim;
                                   if (aim == null) return;
                                   SelectedAbilitiModelProperty.ReqwireAims.Add(aim);
                                   cho.Close();
                               };
                               cho.ShowDialog();
                               RefreshInfoCommand.Execute(null);
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Выбрать квест, который добавить для ссылки.
        /// </summary>
        public RelayCommand ChooseLinkToQwestCommand
        {
            get
            {
                return _chooseLinkToQwestCommand
                       ?? (_chooseLinkToQwestCommand = new RelayCommand(
                           () =>
                           {
                               // Добавляем прокачку?
                               bool isAddUp = Keyboard.Modifiers == ModifierKeys.Alt;

                               var abs =
                                   PersProperty.Aims.Where(n => !n.IsDoneProperty)
                                       .Where(n => n.AbilitiLinksOf.All(q => q != SelectedAbilitiModelProperty))
                                       .OrderBy(n => n.NameOfProperty)
                                       .ToList();

                               var cho = new ChooseAbility
                               {
                                   txtHeader = { Text = "Выберите квест" },
                                   lstAbbs =
                                   {
                                       ItemsSource = abs
                                   }
                               };

                               cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                               cho.btnOk.Click += (sender, args) =>
                               {
                                   var qw = cho.lstAbbs.SelectedValue as Aim;
                                   if (qw == null) return;
                                   if (isAddUp)
                                   {
                                       AddQwestUpAbilityToSelAbility(qw);
                                   }
                                   else
                                   {
                                       qw.AbilitiLinksOf.Add(SelectedAbilitiModelProperty);
                                   }

                                   RefreshInfoCommand.Execute(null);
                                   cho.Close();
                               };
                               cho.ShowDialog();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Выбрать квест для добавления к скиллам
        /// </summary>
        public RelayCommand<object> ChooseQwestToSkillCommand
        {
            get
            {
                return _chooseQwestToSkill ??
                       (_chooseQwestToSkill = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as ComplecsNeed;
                               var skills = new List<Task>();

                               if (Keyboard.Modifiers == ModifierKeys.Control)
                               {
                                   skills.AddRange(
                                       SelectedAbilitiModelProperty.Skills.Select(n => n.NeedTask.TaskProperty));
                               }
                               else
                               {
                                   skills.Add(it.NeedTask.TaskProperty);
                               }

                               var cho = new ChooseAbility
                               {
                                   txtHeader = { Text = "Выберите квест" },
                                   lstAbbs =
                                   {
                                       ItemsSource = PersProperty.Aims
                                           .Where(n => !n.IsDoneProperty)
                                           .OrderBy(n => n.NameOfProperty)
                                   }
                               };
                               cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                               cho.btnOk.Click += (sender, args) =>
                               {
                                   var aim = cho.lstAbbs.SelectedValue as Aim;
                                   if (aim == null) return;

                                   foreach (var skill in skills)
                                   {
                                       aim.Spells.Add(skill);
                                   }

                                   foreach (var complecsNeed in SelectedAbilitiModelProperty.Skills)
                                   {
                                       complecsNeed.NeedTask.TaskProperty.RefreshSkillQwests();
                                   }

                                   cho.Close();
                               };
                               cho.ShowDialog();
                           },
                           item =>
                           {
                               if (!(item is ComplecsNeed))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        /// Gets the комманда Очистить картинку.
        /// </summary>
        public RelayCommand ClearImagePropertyCommand
        {
            get
            {
                return clearImagePropertyCommand
                       ?? (clearImagePropertyCommand =
                           new RelayCommand(
                               () => { SelectedAbilitiModelProperty.ImageProperty = null; },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Очистить скиллы.
        /// </summary>
        public RelayCommand ClearSkillsCommand
        {
            get
            {
                return clearSkillsCommand
                       ?? (clearSkillsCommand =
                           new RelayCommand(
                               () => { SelectedAbilitiModelProperty.NeedTasks.Clear(); },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Скопировать условия вниз.
        /// </summary>
        public RelayCommand<int> CopyTaskUslDownCommand
        {
            get
            {
                return copyTaskUslDownCommand
                       ?? (copyTaskUslDownCommand = new RelayCommand<int>(
                           item =>
                           {
                               var edit = Keyboard.Modifiers == ModifierKeys.Alt;
                               var selTask = SelAbNeed as ComplecsNeed;

                               for (int i = item + 1; i < 5; i++)
                               {
                                   if (selTask?.NeedTask != null)
                                   {
                                       SelectedAbilitiModelProperty.DefSelH = SelectedAbilitiModelProperty.HList[i];
                                       if (edit)
                                       {
                                           MessageBox.Show($"Уровень \"{SelectedAbilitiModelProperty.DefSelH}\"...");
                                       }
                                       var tsk = CloneTask(selTask.NeedTask, -1, SelectedAbilitiModelProperty, edit);
                                       selTask =
                                           SelectedAbilitiModelProperty.ComplecsNeeds.FirstOrDefault(
                                               n => n.NeedTask.TaskProperty == tsk);
                                   }
                               }
                           },
                           item =>
                           {
                               if (item >= 5)
                               {
                                   return false;
                               }
                               if (item == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Удалить условие
        /// </summary>
        public RelayCommand<object> DelObjectCommand
        {
            get
            {
                return _delObject ??
                       (_delObject = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as ComplecsNeed;
                               if (it == null) return;
                               if (it.NeedTask != null)
                               {
                                   SelectedAbilitiModelProperty.DeleteNeedTaskCommand.Execute(it.NeedTask);
                               }
                               else if (it.NeedQwest != null)
                               {
                                   SelectedAbilitiModelProperty.DelAimNeedCommand.Execute(it.NeedQwest);
                               }

                               SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                           },
                           item =>
                           {
                               if (!(item is ComplecsNeed))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        /// Gets the Удалить ссылку на квест или сам квест.
        /// </summary>
        public RelayCommand<Aim> DelQwestLinkCommand
        {
            get
            {
                return delQwestLinkCommand
                       ?? (delQwestLinkCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               var messageBoxResult =
                                   MessageBox.Show(
                                       "Удалить также сам квест?",
                                       "Внимание!",
                                       MessageBoxButton.OKCancel);
                               if (messageBoxResult == MessageBoxResult.OK)
                               {
                                   StaticMetods.RemoveQwest(StaticMetods.PersProperty, item);
                               }
                               else
                               {
                                   item.AbilitiLinksOf.Remove(
                                       item.AbilitiLinksOf.First(n => n == SelectedAbilitiModelProperty));
                               }

                               RefreshInfoCommand.Execute(null);
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
        /// Удалить требования квеста для доступности скилла
        /// </summary>
        public RelayCommand<object> DelQwReqCommand
        {
            get
            {
                return _delQwReq ??
                       (_delQwReq = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as Aim;
                               SelectedAbilitiModelProperty.ReqwireAims.Remove(it);
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
        /// Показать объект
        /// </summary>
        public RelayCommand<object> EditObjectCommand
        {
            get
            {
                return _editObject ??
                       (_editObject = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as ComplecsNeed;
                               if (it == null) return;
                               if (it.NeedTask != null)
                               {
                                   it.NeedTask.TaskProperty.EditTask();
                               }
                               else if (it.NeedQwest != null)
                               {
                                   StaticMetods.editAim(it.NeedQwest.AimProperty);
                                   foreach (var complecsNeed in SelectedAbilitiModelProperty.Skills)
                                   {
                                       complecsNeed.NeedTask.TaskProperty.RefreshSkillQwests();
                                   }
                               }

                               SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                           },
                           item =>
                           {
                               if (!(item is ComplecsNeed))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        public RelayCommand ExportItCommand
        {
            get
            {
                return exportItCommand ?? (exportItCommand = new RelayCommand(
                    () =>
                    {
                        string path;
                        if (AddOrEditCharacteristicViewModel.GetPathToFolderExp(out path)) return;
                        ExportAbToFolder(
                            Path.Combine(path, StaticMetods.GetClearString(SelectedAbilitiModelProperty.NameOfProperty)),
                            SelectedAbilitiModelProperty);
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Заполнить автоматом след. уровень.
        /// </summary>
        public RelayCommand<int> FillNextLevelCommand
        {
            get
            {
                return fillNextLevelCommand
                       ?? (fillNextLevelCommand = new RelayCommand<int>(
                           item =>
                           {
                               if (Keyboard.Modifiers == ModifierKeys.Shift)
                               {
                                   foreach (var source in SelectedAbilitiModelProperty.NeedTasks.ToList())
                                   {
                                       source.TaskProperty.Delete(PersProperty);
                                   }
                                   SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                               }
                               else
                               {
                                   AbilitiModel.CloneTasksForNextLevel(SelectedAbilitiModelProperty, item);
                               }
                               RefreshImagesOfTasksOfAb();
                           },
                           item =>
                           {
                               if (item >= 5)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Gets the Заполнить предыдущий уровень.
        /// </summary>
        public RelayCommand<int> FillPrevLevCommand
        {
            get
            {
                return fillPrevLevCommand
                       ?? (fillPrevLevCommand = new RelayCommand<int>(
                           item =>
                           {
                               if (Keyboard.Modifiers == ModifierKeys.Shift)
                               {
                                   foreach (var source in SelectedAbilitiModelProperty.NeedTasks.ToList())
                                   {
                                       source.TaskProperty.Delete(PersProperty);
                                   }
                                   SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                               }
                               else
                               {
                                   AbilitiModel.CloneTasksForNextLevel(SelectedAbilitiModelProperty, item, true);
                                   //for (int i = StaticMetods.MaxAbLevel; i > 0; i--)
                                   //{
                                   //    AbilitiModel.CloneTasksForNextLevel(SelectedAbilitiModelProperty, i, true);
                                   //}
                               }
                               RefreshImagesOfTasksOfAb();
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
        /// Sets and gets Видимость настройки начального уровня характеристики. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public Visibility FirstLevelVisibleProperty
        {
            get
            {
                return firstLevelVisible;
            }

            set
            {
                if (firstLevelVisible == value)
                {
                    return;
                }

                firstLevelVisible = value;
                OnPropertyChanged(nameof(FirstLevelVisibleProperty));
            }
        }

        /// <summary>
        /// Список сложностей и их значений
        /// </summary>
        public List<HardnessAbAndNames> HardnessList
        {
            get
            {
                return new List<HardnessAbAndNames>
                {
                    new HardnessAbAndNames("Легко", 1),
                    new HardnessAbAndNames("Норм", 2),
                    new HardnessAbAndNames("Сложно", 3)
                };
            }
        }

        private RelayCommand imgGenFromWord;


        public RelayCommand ImgGenFromWord
        {
            get
            {
                return imgGenFromWord ??
                    (imgGenFromWord = new RelayCommand(
                        () =>
                        {
                            var task = System.Threading.Tasks.Task<byte[]>.Factory.StartNew(() =>
                            {
                                return InetImageGen.ImageByWord(SelectedAbilitiModelProperty.NameOfProperty);
                            });
                            task.ContinueWith((img) =>
                            {
                                SelectedAbilitiModelProperty.ImageProperty = img.Result;
                            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());

                            //SelectedAbilitiModelProperty.ImageProperty =
                            //    InetImageGen.ImageByWord(SelectedAbilitiModelProperty.NameOfProperty);
                        }));
            }
        }

        public int IndexNeed
        {
            get
            {
                //if (SelectedAbilitiModelProperty.IsPayedProperty)
                //{
                //    return 1;
                //}
                return 0;
            }
        }

        public bool IsDevMode
        {
            get
            {
                return MainViewModel.IsDevelopmentMode;
            }
            set
            {
                if (MainViewModel.IsDevelopmentMode == value) return;
                MainViewModel.IsDevelopmentMode = value;
                OnPropertyChanged(nameof(IsDevMode));
            }
        }

        /// <summary>
        /// Sets and gets Редактируется ли требование или добавляется?. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool IsEditNeedProperty
        {
            get
            {
                return isEditNeed;
            }

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
        /// Sets and gets Видимость добавления требования скилла. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool IsOpenAbilNeedProperty
        {
            get
            {
                return isOpenAbilNeed;
            }

            set
            {
                isOpenAbilNeed = value;
                OnPropertyChanged(nameof(IsOpenAbilNeedProperty));
            }
        }

        /// <summary>
        /// Видимость настроек
        /// </summary>
        public bool IsSetViz
        {
            get
            {
                return _isSetViz;
            }
            set
            {
                if (value == _isSetViz) return;
                _isSetViz = value;
                OnPropertyChanged();
            }
        }

        public List<NeedK> KMissionsRelays
        {
            get
            {
                var needKs = new List<NeedK>
                {
                    new NeedK {KProperty = 0, NameProperty = "Нет"},
                    new NeedK {KProperty = 3, NameProperty = "Слабо"},
                    new NeedK {KProperty = 6, NameProperty = "Норм"},
                    new NeedK {KProperty = 10, NameProperty = "Сильно"}
                };
                return needKs;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public RelayCommand<ComplecsNeed> LinkToQwestCommand
        {
            get
            {
                return linkToQwest
                       ?? (linkToQwest =
                           new RelayCommand<ComplecsNeed>(
                               item =>
                               {
                                   var need = QwestsViewModel.GetDefoultNeedTask(item.NeedTask.TaskProperty);
                                   need.AsLinkProperty = true;

                                   if (Keyboard.Modifiers == ModifierKeys.Control)
                                   {
                                       // Выбор из списка
                                       AddOrEditNeedAimView av = new AddOrEditNeedAimView();
                                       AddOrEditAimNeedViewModel context = StaticMetods.Locator.AddOrEditAimNeedVM;

                                       context.SellectedNeedPropertyProperty = new CompositeAims
                                       {
                                           AimProperty = null,
                                           KoeficientProperty = 10,
                                           KRel = 6,
                                           LevelProperty = 0
                                       };

                                       av.btnOk.Click += (sender, args) =>
                                       {
                                           context.SellectedNeedPropertyProperty.AimProperty?.NeedsTasks.Add(need);
                                           av.Close();
                                           StaticMetods.editAim(context.SellectedNeedPropertyProperty.AimProperty);
                                       };

                                       av.btnCansel.Click += (sender, args) => { av.Close(); };
                                       av.ShowDialog();
                                   }
                                   else
                                   {
                                       var newAim = new Aim(PersProperty)
                                       {
                                           ImageProperty = SelectedAbilitiModelProperty.ImageProperty,
                                           MinLevelProperty = PersProperty.PersLevelProperty
                                       };

                                       newAim.NeedsTasks.Add(need);

                                       var editQwest = new EditQwestWindowView();
                                       var context = StaticMetods.Locator.AimsVM;

                                       context.SelectedAimProperty = newAim;
                                       FocusManager.SetFocusedElement(editQwest, editQwest.QwestsView.txtName);
                                       editQwest.btnOk.Click += (sender, args) => { editQwest.Close(); };
                                       editQwest.btnCansel.Click += (sender, args) =>
                                       {
                                           StaticMetods.RemoveQwest(StaticMetods.PersProperty, newAim, true, true);
                                           newAim = null;
                                           editQwest.Close();
                                       };

                                       StaticMetods.Locator.QwestsVM.RefreshInfoCommand.Execute(null);

                                       editQwest.ShowDialog();
                                   }

                                   StaticMetods.RefreshAllQwests(StaticMetods.PersProperty, true, true, true);
                                   SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                               },
                               item =>
                               {
                                   if (item == null || item.NeedTask == null)
                                   {
                                       return false;
                                   }
                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Значения и описания для скорости освоения скилла
        /// </summary>
        public List<ValuesAndDescriptions> ModificatorsLearn
        {
            get
            {
                var selVal = SelectedAbilitiModelProperty?.ModificatorLearn ?? 0;
                return new List<ValuesAndDescriptions>
                {
                    new ValuesAndDescriptions
                    {
                        Value = 5,
                        MinValue = 0,
                        MaxValue = 10,
                        Description = "Медленно",
                        SellectedValue = selVal
                    },
                    new ValuesAndDescriptions
                    {
                        Value = 10,
                        MinValue = 10,
                        MaxValue = 20,
                        Description = "Норм",
                        SellectedValue = selVal
                    },
                    new ValuesAndDescriptions
                    {
                        Value = 20,
                        MinValue = 20,
                        MaxValue = 101,
                        Description = "Быстро",
                        SellectedValue = selVal
                    }
                };
            }
        }

        /// <summary>
        /// Значения и описания для скорости забывания скилла
        /// </summary>
        public List<ValuesAndDescriptions> ModificatorsOfForget
        {
            get
            {
                var selVal = SelectedAbilitiModelProperty?.ModificatorOfForget ?? 0;
                return new List<ValuesAndDescriptions>
                {
                    new ValuesAndDescriptions
                    {
                        Value = 1,
                        MinValue = 0,
                        MaxValue = 2,
                        Description = "Нет",
                        SellectedValue = selVal
                    },
                    new ValuesAndDescriptions
                    {
                        Value = 5,
                        MinValue = 2,
                        MaxValue = 10,
                        Description = "Медленно",
                        SellectedValue = selVal
                    },
                    new ValuesAndDescriptions
                    {
                        Value = 10,
                        MinValue = 10,
                        MaxValue = 20,
                        Description = "Норм",
                        SellectedValue = selVal
                    },
                    new ValuesAndDescriptions
                    {
                        Value = 20,
                        MinValue = 20,
                        MaxValue = 101,
                        Description = "Быстро",
                        SellectedValue = selVal
                    }
                };
            }
        }

        public ucRelaysItemsVM NeedsItemsVM { get; set; }

        /// <summary>
        /// Gets the комманда Ок в настройках скилла.
        /// </summary>
        public RelayCommand<string> OkCommand
        {
            get
            {
                return okCommand ?? (okCommand = new RelayCommand<string>(
                    item =>
                    {
                        foreach (var characteristic in PersProperty.Characteristics)
                        {
                            characteristic.RefreshRelAbs();
                        }
                        StaticMetods.RefreshAllQwests(PersProperty, true, true, false);
                        StaticMetods.Locator.ucAbilitisVM.ChaAbilitises.Refresh();
                        PersProperty.RecountPlusAbPoints();
                        if (MainViewModel.IsEditModeAfterAbLevUp)
                        {
                            MainViewModel.IsEditModeAfterAbLevUp = false;
                            StaticMetods.PersProperty.IsSetViz = false;
                        }
                    },
                    item => { return true; }));
            }
        }

        /// <summary>
        /// Открыть редактируемую характеристику
        /// </summary>
        public RelayCommand<object> OpenRelayCharacteristicCommand
        {
            get
            {
                return _openRelayCharacteristic ??
                       (_openRelayCharacteristic = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as Characteristic;
                               if (it != null)
                               {
                                   it.EditCharacteristic();
                                   RefreshInfoCommand.Execute(null);
                                   OnPropertyChanged(nameof(ChangeCharacts));
                               }
                           },
                           item =>
                           {
                               if (!(item is Characteristic))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        /// Gets the Открыть карту задач скилла.
        /// </summary>
        public RelayCommand OpenTaskMapCommand
        {
            get
            {
                return openTaskMapCommand
                       ?? (openTaskMapCommand =
                           new RelayCommand(
                               () =>
                               {
                                   AbTaskMap ab = new AbTaskMap();
                                   ab.btnClose.Click += (sender, args) =>
                                   {
                                       ab.Close();
                                       SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                                       RefreshInfoCommand.Execute(null);
                                   };

                                   PersProperty.SellectedAbilityProperty = SelectedAbilitiModelProperty;

                                   ab.ShowDialog();
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Персонаж. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Pers PersProperty
        {
            get
            {
                return StaticMetods.PersProperty;
            }
            set
            {
                StaticMetods.PersProperty = value;
                OnPropertyChanged(nameof(Pers));
            }
        }

        /// <summary>
        /// Gets the комманда Прибавляем уровень к требованию скилла.
        /// </summary>
        public RelayCommand PlusAbilNeedLevelCommand
        {
            get
            {
                return plusAbilNeedLevelCommand
                       ?? (plusAbilNeedLevelCommand =
                           new RelayCommand(
                               () =>
                               {
                                   SelectedNeedAbilityProperty.ValueProperty =
                                       StaticMetods.plusNeedAbilityLevel(
                                           SelectedNeedAbilityProperty.ValueProperty,
                                           PersProperty.PersSettings.AbilMaxLevelProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Прибавить уровень скилла.
        /// </summary>
        public RelayCommand<string> PlusLevelCommand
        {
            get
            {
                return plusLevelCommand
                       ?? (plusLevelCommand = new RelayCommand<string>(
                           item =>
                           {
                               refreshRangs();
                               RefreshInfoCommand.Execute(null);
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
        /// Gets the + и - задать начальный уровень характеристики.
        /// </summary>
        public RelayCommand<string> PlusMinusAbFirstLevelCommand
        {
            get
            {
                return plusMinusAbFirstLevelCommand
                       ?? (plusMinusAbFirstLevelCommand =
                           new RelayCommand<string>(
                               item =>
                               {
                                   switch (item)
                                   {
                                       case "+":
                                           SelectedAbilitiModelProperty.FirstLevelProperty++;
                                           break;

                                       case "-":
                                           SelectedAbilitiModelProperty.FirstLevelProperty--;
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
        /// Gets the +1 к уровню скилла.
        /// </summary>
        public RelayCommand<ChangeAbilityModele> PlusOneLevelAbilityRelayCommand
        {
            get
            {
                return plusOneLevelAbilityRelayCommand
                       ?? (plusOneLevelAbilityRelayCommand =
                           new RelayCommand<ChangeAbilityModele>(
                               item =>
                               {
                                   var curValue = item.AbilityProperty.ValueProperty;
                                   var curLevel = item.AbilityProperty.LevelProperty;
                                   var needValue = Pers.ExpToLevel(curLevel + 1, RpgItemsTypes.ability);

                                   item.ChangeAbilityProperty = needValue - curValue;
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
        /// Gets the Плюс в задаче.
        /// </summary>
        public RelayCommand<Task> PlusTaskCommand
        {
            get
            {
                return plusTaskCommand
                       ?? (plusTaskCommand =
                           new RelayCommand<Task>(
                               item => { item.ClickPlusMinusTomorrowTask(PersProperty, true); },
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
        /// Gets the Предыдущая/следующая характеристика.
        /// </summary>
        public RelayCommand<string> PrevNextCommand
        {
            get
            {
                return prevNextCommand
                       ?? (prevNextCommand = new RelayCommand<string>(
                           item =>
                           {
                               AbilitiModel other = null;
                               AbilitiModel it = SelectedAbilitiModelProperty;

                               List<AbilitiModel> abCollection;

                               abCollection = ChaToPrevNext != null
                                   ? ChaToPrevNext.RelayAbilitys.Select(n => n.AbilProperty).ToList()
                                   : StaticMetods.Locator.ucAbilitisVM.ChaAbilitises.Cast<AbilitiModel>().ToList();

                               var ind = abCollection.IndexOf(it);

                               if (item == "next")
                               {
                                   ind = abCollection.Count > ind + 1 ? ind + 1 : 0;
                               }
                               else if (item == "prev")
                               {
                                   ind = ind - 1 >= 0 ? ind - 1 : abCollection.Count - 1;
                               }

                               other = abCollection.ElementAt(ind);

                               if (other != null && other != it)
                               {
                                   StaticMetods.Locator.AddOrEditAbilityVM.SetSelAbiliti(other);
                                   RefreshInfoCommand.Execute(null);
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
        /// Sets and gets Лист коллекция для скиллов. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ListCollectionView QAbilViewProperty
        {
            get
            {
                return qAbilView;
            }

            set
            {
                if (qAbilView == value)
                {
                    return;
                }

                qAbilView = value;
                OnPropertyChanged(nameof(QAbilViewProperty));
            }
        }

        /// <summary>
        /// Gets Квесты для требований.
        /// </summary>
        public IOrderedEnumerable<Aim> QwestsProperty
        {
            get { return PersProperty?.Aims.OrderBy(n => n.NameOfProperty); }
        }

        /// <summary>
        /// Sets and gets Быстро задать значение влияния на характеристику. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public Characteristic QwickCharacteristicProperty
        {
            get
            {
                return qwickCharacteristic;
            }

            set
            {
                if (qwickCharacteristic == value)
                {
                    return;
                }

                qwickCharacteristic = value;
                OnPropertyChanged(nameof(QwickCharacteristicProperty));
            }
        }

        /// <summary>
        /// Gets the Быстро задать или снять влияние скилла на характеристику.
        /// </summary>
        public RelayCommand<Tuple<Characteristic, NeedAbility>> QwickSetAbToChaRelayCommand
        {
            get
            {
                return qwickSetAbToChaRelayCommand
                       ?? (qwickSetAbToChaRelayCommand = new RelayCommand<Tuple<Characteristic, NeedAbility>>(
                           item =>
                           {
                               if (item.Item2.KoeficientProperty == 0)
                               {
                                   item.Item2.KoeficientProperty = 6;
                               }
                               else
                               {
                                   item.Item2.KoeficientProperty = 0;
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
        /// Gets the Быстро задать значение влияния на характеристику.
        /// </summary>
        public RelayCommand<string> QwickSetCharactRelayCommand
        {
            get
            {
                return qwickSetCharactRelayCommand
                       ?? (qwickSetCharactRelayCommand =
                           new RelayCommand<string>(
                               item => { refreshRangs(); },
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
        /// Gets the Быстро задать сложность задачи (влияет на вероятность появления наград при выполнении).
        /// </summary>
        public RelayCommand<string> QwickSetHardnessCommand
        {
            get
            {
                return qwickSetHardnessCommand
                       ?? (qwickSetHardnessCommand =
                           new RelayCommand<string>(
                               item => { SelectedAbilitiModelProperty.HardnessProperty = Convert.ToInt32(item); },
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

        public List<UpUbility> QwUpAb
        {
            get
            {
                return _qwUpAb;
            }
            set
            {
                if (Equals(value, _qwUpAb)) return;
                _qwUpAb = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the комманда Обновить активные задачи скилла.
        /// </summary>
        public RelayCommand RefreshActiveAbTasksCommand
        {
            get
            {
                return refreshActiveAbTasksCommand
                       ?? (refreshActiveAbTasksCommand =
                           new RelayCommand(
                               () => { StaticMetods.Locator.ActiveAbTasksVM.RefreshActiveTasks(); },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Обновление информации.
        /// </summary>
        public RelayCommand RefreshInfoCommand
        {
            get
            {
                return refreshInfoCommand
                       ?? (refreshInfoCommand = new RelayCommand(
                           () =>
                           {
                               OnPropertyChanged(nameof(SelectedAbilitiModelProperty.MinLevelProperty));
                               SelectedAbilitiModelProperty.RefreshEnabled();

                               OnPropertyChanged(nameof(RelaysTasks));
                               OnPropertyChanged(nameof(RelToAbQwests));
                               OnPropertyChanged(nameof(AllNeeds));

                               OnPropertyChanged(nameof(AbilityNeedCharacts));
                               OnPropertyChanged(nameof(AbilityNeedAbilities));
                               OnPropertyChanged(nameof(ReqwireAims));

                               getRelaysItems();
                               getNeedsItems();
                               getReqvireItems();

                               SelectedAbilitiModelProperty.RefreshLinkedQwests();
                               SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                               refreshQwAbUps();
                               refreshChaRels();
                           },
                           () => { return true; }));
            }
        }

        public ucRelaysItemsVM RelaysItemsVm { get; set; }

        /// <summary>
        /// Задачи, влияющие на скилл
        /// </summary>
        public IEnumerable<RelTaskToAb> RelaysTasks
        {
            get { return null; }
        }

        public List<RelCha> RelaysToChas
        {
            get
            {
                return _relaysToChas;
            }
            set
            {
                if (Equals(value, _relaysToChas)) return;
                _relaysToChas = value;
                OnPropertyChanged(nameof(RelaysToChas));
            }
        }

        /// <summary>
        /// Квесты, влияющие на скилл
        /// </summary>
        public IEnumerable<RelToAbQwests> RelToAbQwests
        {
            get { return null; }
        }

        public ucRelaysItemsVM ReqvireItemsVm { get; set; }

        /// <summary>
        /// Gets the Сбросить скилл.
        /// </summary>
        public RelayCommand ResetAbilityCommand
        {
            get
            {
                return resetAbilityCommand
                       ?? (resetAbilityCommand =
                           new RelayCommand(
                               () =>
                               {
                                   AbilitiModel.SaleAbLevel(SelectedAbilitiModelProperty, PersProperty);

                                   // Пересчет значений
                                   StaticMetods.RecauntAllValues();

                                   // Обновляем комплексные требования
                                   SelectedAbilitiModelProperty.RefreshComplecsNeeds();

                                   // Обновляем квесты
                                   StaticMetods.RefreshAllQwests(StaticMetods.PersProperty, true, true, true);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Выбранное требование
        /// </summary>
        public object SelAbNeed
        {
            get
            {
                return _selAbNeed;
            }
            set
            {
                if (Equals(value, _selAbNeed)) return;
                _selAbNeed = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets Выбранное изменение. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public RelTaskToAb SelChangeProperty
        {
            get
            {
                return selChange;
            }

            set
            {
                if (selChange == value)
                {
                    return;
                }

                selChange = value;
                OnPropertyChanged(nameof(SelChangeProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранный скилл. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public AbilitiModel SelectedAbilitiModelProperty
        {
            get
            {
                return PersProperty.SellectedAbilityProperty;
            }

            set
            {
                value?.RefreshComplecsNeeds();

                if (PersProperty.SellectedAbilityProperty == value)
                {
                    return;
                }
                PersProperty.SellectedAbilityProperty = value;
                sortRangs();
                OnPropertyChanged(nameof(SelectedAbilitiModelProperty));
                OnPropertyChanged(nameof(ModificatorsLearn));
                OnPropertyChanged(nameof(ModificatorsOfForget));
                OnPropertyChanged(nameof(IndexNeed));
                OnPropertyChanged(nameof(ChangeCharacts));
                OnPropertyChanged(nameof(ucItemRevardsViewModel));
                refreshQwAbUps();
            }
        }

        /// <summary>
        /// Sets and gets Выбранный квест для требований. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Aim SelectedAimNeedProperty
        {
            get
            {
                return selectedAimNeed;
            }

            set
            {
                if (selectedAimNeed == value)
                {
                    return;
                }

                selectedAimNeed = value;
                OnPropertyChanged(nameof(SelectedAimNeedProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выделенное требование для скиллов. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public NeedAbility SelectedNeedAbilityProperty
        {
            get
            {
                return selectedNeedAbility;
            }

            set
            {
                if (selectedNeedAbility == value)
                {
                    return;
                }

                selectedNeedAbility = value;
                OnPropertyChanged(nameof(SelectedNeedAbilityProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранная связанная задача. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Task SelectedTaskProperty
        {
            get
            {
                return selectedTask;
            }

            set
            {
                if (selectedTask == value)
                {
                    return;
                }

                selectedTask = value;
                OnPropertyChanged(nameof(SelectedTaskProperty));
            }
        }

        /// <summary>
        /// Команда на просмотр скилла из требований
        /// </summary>
        public RelayCommand<object> ShowAbReqCommand
        {
            get
            {
                return showAbReq ??
                       (showAbReq = new RelayCommand<object>(
                           item =>
                           {
                               var sA = SelectedAbilitiModelProperty;
                               var it = item as AbilReqwirement;
                               it.AbilityProperty.EditAbility();
                               SelectedAbilitiModelProperty = sA;
                           },
                           item =>
                           {
                               if (!(item is AbilReqwirement))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        /// Gets the Показать квест для редактирования.
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
                               RefreshInfoCommand.Execute(null);
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
        /// Посмотреть квест из требований квеста
        /// </summary>
        public RelayCommand<object> ShowQwestReqCommand
        {
            get
            {
                return _showQwestReq ??
                       (_showQwestReq = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as Aim;
                               StaticMetods.editAim(it);
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

        public ucItemRevardsViewModel ucItemRevardsViewModel => new ucItemRevardsViewModel(SelectedAbilitiModelProperty)
                            ;

        /// <summary>
        /// Обнуляем опыт для скилла
        /// </summary>
        /// <param name="selAbb"></param>
        public static void AbNullExperiance(AbilitiModel selAbb)
        {
            if (selAbb.Experiance > 0)
            {
                selAbb.Experiance = 0;
            }
            else
            {
                var ab = StaticMetods.PersProperty.Abilitis.FirstOrDefault(n => n.IsPayedProperty && n != selAbb);
                if (ab != null)
                {
                    ab.Experiance += selAbb.Experiance;
                }
            }
        }

        /// <summary>
        /// Клонировать задачу
        /// </summary>
        /// <param name="need"></param>
        /// <param name="item"></param>
        /// <param name="edit"></param>
        public static Task CloneTask(NeedTasks need, int lvl, AbilitiModel ab, bool edit = true)
        {
            var tsk = GetClonnedTask(need, lvl, StaticMetods.PersProperty, ab);
            if (edit)
            {
                tsk.EditTask();
            }

            //--------------------
            //var need = SelectedAbilitiModelProperty.NeedTasks.LastOrDefault(n => n.TaskProperty == tsk.Item2);
            //need.LevelProperty = SelectedAbilitiModelProperty.LevelProperty;
            //need.ToLevelProperty = SelectedAbilitiModelProperty.LevelProperty;

            ab.RefreshComplecsNeeds();
            StaticMetods.RecountTaskLevels();
            return tsk;
        }

        public static void ExportAbToFolder(string pathToSave, AbilitiModel ab)
        {
            // Сохраняем персонажа с его задачами
            using (var fs = new FileStream(pathToSave, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, ab);
            }
        }

        /// <summary>
        /// Открыть настройку требований редактируемого скилла
        /// </summary>
        /// <param name="item"></param>
        public static void OpenAbNeedsSettings(AbilitiModel item)
        {
            item.EditAbility();

            //if (item.PayedLevelProperty == 1)
            //{
            //    item.EditAbility();
            //}
            //else
            //{
            //    var editAbContext = StaticMetods.Locator.AddOrEditAbilityVM;

            // editAbContext.SetSelAbiliti(item);

            //    AbilityNeedSettingsView asv = new AbilityNeedSettingsView();
            //    asv.btnOk.Click += (sender, args) =>
            //    {
            //        asv.Close();
            //        item.RefreshComplecsNeeds();
            //    };
            //    asv.DataContext = editAbContext;
            //    asv.ShowDialog();
            //}
        }

        public static void SetNeedTasksAllNow(List<NeedTasks> nt)
        {
            foreach (var t in nt)
            {
                t.LevelProperty = 0;
                t.ToLevelProperty = StaticMetods.PersProperty.PersSettings.AbRangs.Count - 1;
            }
        }

        /// <summary>
        /// Задачи навыков сделать постепенно.
        /// </summary>
        /// <param name="nt"></param>
        public static void SetNeedTasksParallel(List<NeedTasks> nt)
        {
            double count = nt.Count;

            for (int i = 0; i < count; i++)
            {
                nt[i].LevelProperty = (int)Math.Ceiling((i / count) * (StaticMetods.PersProperty.PersSettings.AbRangs.Count - 1));
                nt[i].ToLevelProperty = StaticMetods.PersProperty.PersSettings.AbRangs.Count - 1;
            }
        }

        /// <summary>
        /// Задачи навыков сделать по очереди.
        /// </summary>
        /// <param name="nt"></param>
        public static void SetNeedTasksQued(List<NeedTasks> nt)
        {
            double count = nt.Count;

            for (int i = 0; i < count; i++)
            {
                nt[i].LevelProperty = (int)Math.Ceiling((i / count) * (StaticMetods.PersProperty.PersSettings.AbRangs.Count - 1));

                if (i + 1 < count)
                    nt[i].ToLevelProperty = (int)Math.Ceiling(((i + 1) / count) * (StaticMetods.PersProperty.PersSettings.AbRangs.Count - 1)) - 1;
                else
                    nt[i].ToLevelProperty = StaticMetods.PersProperty.PersSettings.AbRangs.Count - 1;
            }
        }

        /// <summary>
        /// Показать поднятие уровня
        /// </summary>
        /// <param name="headerText"></param>
        /// <param name="selAb"></param>
        /// <param name="color"></param>
        /// <param name="changeLVis"></param>
        public static void showAbLevelChange(string headerText, AbilitiModel selAb, SolidColorBrush color,
            bool down = false)
        {
            LevelsChangesView lv = new LevelsChangesView
            {
                Image = { Source = selAb.PictureProperty },
                down = { Visibility = Visibility.Collapsed },
                Header = { Text = headerText, Foreground = color }
            };

            if (down)
            {
                lv.down.Visibility = Visibility.Visible;
                lv.up.Visibility = Visibility.Collapsed;
                StaticMetods.PlaySound(Resources.Fail);
            }
            else
            {
                StaticMetods.PlaySound(Resources.abLevelUp);
            }

            lv.btnOk.Click += (sender, args) => { lv.Close(); };
            lv.ShowDialog();
        }

        /// <summary>
        /// Добавление нового скилла
        /// </summary>
        public void addAb()
        {
            SelectedAbilitiModelProperty = new AbilitiModel(PersProperty);
            FirstLevelVisibleProperty = Visibility.Visible;
            OnPropertyChanged(nameof(RelaysTasks));
            RefreshInfoCommand.Execute(null);
        }

        /// <summary>
        /// Экспорт скилла
        /// </summary>
        public void ExportAbility()
        {
            var ab = new AbilitiModel(SelectedAbilitiModelProperty);
            var of = new SaveFileDialog();
            of.ShowDialog();
            var pathToSave = of.FileName;
            if (string.IsNullOrWhiteSpace(pathToSave))
            {
                return;
            }
            try
            {
                ExportAbToFolder(pathToSave, ab);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при экспорте данных! Возможно проблема в правах доступа.");
            }
        }

        /// <summary>
        /// Получить элементы которые влияют на этот
        /// </summary>
        public void getNeedsItems()
        {
            var needsItems = new List<RelaysItem>();

            if (SelectedAbilitiModelProperty != null)
            {
                IEnumerable<RelaysItem> needQwests =
                    (from aimNeed in
                        SelectedAbilitiModelProperty.NeedAims.OrderByDescending(n => n.KoeficientProperty)
                     select
                         new RelaysItem
                         {
                             BorderColorProperty =
                                 StaticMetods.getBorderColorFromRelaysWithHardness(
                                     Convert.ToInt32(aimNeed.KoeficientProperty)),
                             ElementToolTipProperty =
                                 "Квест ''" + aimNeed.AimProperty.NameOfProperty + "''",
                             ReqvirementTextProperty =
                                 Math.Round(aimNeed.AimProperty.AutoProgressValueProperty, 0) + "%",
                             IdProperty = aimNeed.AimProperty.GUID,
                             PictureProperty = aimNeed.AimProperty.PictureProperty,
                             KRelayProperty = aimNeed.KoeficientProperty
                         }).OrderByDescending(
                                n => n.KRelayProperty).ToList();

                IEnumerable<RelaysItem> needTasks =
                    (from taskNeed in
                        SelectedAbilitiModelProperty.NeedTasks.OrderByDescending(n => n.KoeficientProperty)
                     select
                         new RelaysItem
                         {
                             BorderColorProperty =
                                 StaticMetods.getBorderColorFromRelaysWithHardness(
                                     Convert.ToInt32(taskNeed.KoeficientProperty)),
                             ElementToolTipProperty =
                                 "Задача ''" + taskNeed.TaskProperty.NameOfProperty + "''",
                             ReqvirementTextProperty = taskNeed.Progress + "%",
                             IdProperty = taskNeed.TaskProperty.GUID,
                             PictureProperty = taskNeed.TaskProperty.PictureProperty,
                             KRelayProperty = taskNeed.KoeficientProperty
                         }).OrderByDescending(
                                n => n.KRelayProperty).ToList();

                needsItems.AddRange(needQwests);
                needsItems.AddRange(needTasks);
            }

            NeedsItemsVM.RelaysItemsesProperty = needsItems;
        }

        /// <summary>
        /// Получить элементы на которые влияет элемент
        /// </summary>
        public void getRelaysItems()
        {
            var relaysItems = new List<RelaysItem>();

            if (SelectedAbilitiModelProperty != null)
            {
                if (PersProperty.PersSettings.IsFudgeModeProperty)
                {
                    var relToCha =
                        PersProperty.Characteristics.Any(
                            n =>
                                n.NeedAbilitisProperty.Any(
                                    q => q.AbilProperty == SelectedAbilitiModelProperty && q.KoeficientProperty != 0));
                    if (relToCha)
                    {
                        relaysItems.Add(
                            new RelaysItem
                            {
                                IdProperty = "exp",
                                ElementToolTipProperty = "Опыт",
                                PictureProperty =
                                    StaticMetods.getImagePropertyFromImage(Pers.ExpImageProperty)
                            });
                    }
                }

                IEnumerable<RelaysItem> relayses = (from characteristic in PersProperty.Characteristics
                                                    from needAbility in
                                                        characteristic.NeedAbilitisProperty.Where(
                                                            n => n.AbilProperty == SelectedAbilitiModelProperty && n.KoeficientProperty > 0)
                                                    select
                                                        new RelaysItem
                                                        {
                                                            BorderColorProperty =
                                                                StaticMetods.getBorderColorFromRelaysWithHardness(
                                                                    Convert.ToInt32(needAbility.KoeficientProperty)),
                                                            ReqvirementTextProperty =
                                                                StaticMetods.getTextFromRelaysWithHardness(
                                                                    Convert.ToInt32(needAbility.KoeficientProperty)),
                                                            ElementToolTipProperty =
                                                                "Характеристика ''" + characteristic.NameOfProperty + "''",
                                                            ProgressProperty = 0,
                                                            IdProperty = characteristic.GUID,
                                                            PictureProperty = characteristic.PictureProperty,
                                                            KRelayProperty = needAbility.KoeficientProperty
                                                        }).OrderByDescending(
                            n => n.KRelayProperty).ToList();

                relaysItems.AddRange(relayses);
            }

            RelaysItemsVm.RelaysItemsesProperty = relaysItems;
        }

        public void getReqvireItems()
        {
            var relaysItems = new List<RelaysItem>();

            if (SelectedAbilitiModelProperty != null)
            {
                // Уровень персонажа
                relaysItems.Add(
                    new RelaysItem
                    {
                        BorderColorProperty =
                            StaticMetods.GetLevelReqBorderColor(
                                PersProperty.PersLevelProperty,
                                SelectedAbilitiModelProperty.MinLevelProperty),
                        ElementToolTipProperty = "Минимальный уровень персонажа",
                        IdProperty = "уровень",
                        PictureProperty =
                            StaticMetods.getImagePropertyFromImage(Pers.LevelImageProperty),
                        ReqvirementTextProperty =
                            ">= " + SelectedAbilitiModelProperty.MinLevelProperty
                    });
            }

            ReqvireItemsVm.RelaysItemsesProperty = relaysItems;
        }

        public void RefreshAbTsks()
        {
            OnPropertyChanged(nameof(RelaysTasks));
        }

        public void RefreshKRelays()
        {
            OnPropertyChanged(nameof(ChangeCharacts));
        }

        /// <summary>
        /// The refresh rangs.
        /// </summary>
        public void refreshRangs()
        {
            var Cha = SelectedAbilitiModelProperty;
            SelectedAbilitiModelProperty = null;
            SelectedAbilitiModelProperty = Cha;
            OnPropertyChanged(nameof(SelectedAbilitiModelProperty));
        }

        /// <summary>
        /// Задать выделенный скилл
        /// </summary>
        /// <param name="ability">Выбранный скилл</param>
        public void SetSelAbiliti(AbilitiModel ability)
        {
            SelectedAbilitiModelProperty = ability;
            FirstLevelVisibleProperty = Visibility.Visible;
            OnPropertyChanged(nameof(RelaysTasks));
            RefreshInfoCommand.Execute(null);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static Task GetClonnedTask(NeedTasks need, int lvl, Pers prs, AbilitiModel ab)
        {
            var taskProperty = need.TaskProperty;
            TypeOfTask defType = StaticMetods.PersProperty.PersSettings.DefoultTaskTypeForAbills ??
                                 StaticMetods.PersProperty.TasksTypes.FirstOrDefault(
                                     n => n.IntervalForDefoult == TimeIntervals.День);

            var addTask = new AddOrEditTaskView();
            var context = (UcTasksSettingsViewModel)addTask.UcTasksSettingsView.DataContext;
            context.AddNewTask(defType);
            var tsk = context.SelectedTaskProperty;
            //--------------------
            tsk.NameOfProperty = taskProperty.NameOfProperty;
            tsk.DescriptionProperty = taskProperty.DescriptionProperty;
            tsk.BeginDateProperty = taskProperty.BeginDateProperty;
            tsk.BackGroundBrush = taskProperty.BackGroundBrush;
            tsk.BackGroundColor = taskProperty.BackGroundColor;
            tsk.TimeProperty = taskProperty.TimeProperty;
            tsk.Mesure = taskProperty.Mesure;
            tsk.TimeMustProperty = taskProperty.TimeMustProperty;
            tsk.Link = taskProperty.Link;
            tsk.PomodorroMax = taskProperty.PomodorroMax;
            tsk.Recurrense.TypeInterval = taskProperty.Recurrense.TypeInterval;
            tsk.Recurrense.Interval = taskProperty.Recurrense.Interval;
            tsk.PlusExp = taskProperty.PlusExp;
            tsk.PlusGold = taskProperty.PlusGold;
            tsk.AimTimerMax = taskProperty.AimTimerMax;
            tsk.AimMesure = taskProperty.AimMesure;
            tsk.IsWordHardness = taskProperty.IsWordHardness;
            tsk.MiliSecsDoneForSort = taskProperty.MiliSecsDoneForSort;

            foreach (var daysOfWeekRepeat in taskProperty.DaysOfWeekRepeats)
            {
                tsk.DaysOfWeekRepeats.First(n => n.Day == daysOfWeekRepeat.Day).CheckedProperty =
                    daysOfWeekRepeat.CheckedProperty;
            }
            tsk.IsBoss = taskProperty.IsBoss;

            // Копируем подзадачи
            foreach (var subTask in taskProperty.SubTasks)
            {
                tsk.SubTasks.Add(new SubTask { Guid = new Guid().ToString(), isDone = false, Tittle = subTask.Tittle });
            }

            tsk.SubTasksRec = taskProperty.SubTasksRec;

            //Клонируем в квесты
            foreach (Aim aim in prs.Aims.Where(n => n.NeedsTasks.Any(q => q.TaskProperty == taskProperty)).ToList())
            {
                var needTask = QwestsViewModel.GetDefoultNeedTask(tsk);
                aim.NeedsTasks.Add(needTask);
            }

            //Клонируем в скиллы
            var needsInAbAndAb = (from abilitiModel in prs.Abilitis
                                  from nt in abilitiModel.NeedTasks.Where(n => n.TaskProperty == taskProperty)
                                  select new { abilitiModel, nt }).ToList();

            if (needsInAbAndAb.Any())
            {
                foreach (var nIab in needsInAbAndAb)
                {
                    var to = lvl >= 0 ? lvl : need.ToLevelProperty;
                    var frome = lvl >= 0 ? lvl : nIab.abilitiModel.GetNeedLev();
                    var nt = Task.taskSettingForAbility(tsk, nIab.abilitiModel, frome, to);
                }
            }
            else
            {
                if (ab != null)
                {
                    var to = need.ToLevelProperty;
                    var frome = need.LevelProperty;
                    var nt = Task.taskSettingForAbility(tsk, ab, frome, to);
                }
            }

            tsk.RecountAutoValues();
            return tsk;
        }

        /// <summary>
        /// Задать чтобы квест прокачивал выбранный скилл
        /// </summary>
        /// <param name="qwest">Квест</param>
        private void AddQwestUpAbilityToSelAbility(Aim qwest)
        {
            // Добавляем влияение на скилл
            var ca = new CompositeAims
            {
                AimProperty = qwest,
                KoeficientProperty = 1,
                KRel = 1,
                LevelProperty = 0 //ab.NeedLevelForDefoult
            };

            ca.LevelProperty = 0; //ab.GetNeedLev();
            ca.ToLevelProperty = SelectedAbilitiModelProperty.GetNeedLev();
            SelectedAbilitiModelProperty.NeedAims.Add(ca);
        }

        private void refreshChaRels()
        {
            if (SelectedAbilitiModelProperty == null)
            {
                RelaysToChas = new List<RelCha>();
            }
            else
            {
                RelaysToChas = (from characteristic in PersProperty.Characteristics
                                from needAbility in characteristic.NeedAbilitisProperty
                                where needAbility.AbilProperty == SelectedAbilitiModelProperty
                                where IsSetViz || needAbility.KoeficientProperty > 0
                                orderby needAbility.KoeficientProperty descending
                                select new RelCha() { Charact = characteristic, NeedAbility = needAbility }).ToList();
            }

            ChaRelsString = string.Join("; ",
                RelaysToChas.Select(n => $"{n.Charact.NameOfProperty} {n.NeedAbility.GetKoefName}"));
        }

        private void RefreshImagesOfTasksOfAb()
        {
            StaticMetods.Locator.MainVM.RefreshRelations();
            foreach (var needTaskse in SelectedAbilitiModelProperty.NeedTasks)
            {
                Task.RecountTaskLevel(needTaskse.TaskProperty);
            }
        }

        /// <summary>
        /// Обновляем квесты, которые награждают
        /// </summary>
        private void refreshQwAbUps()
        {
            if (SelectedAbilitiModelProperty == null) return;
            QwUpAb = (from aim in PersProperty.Aims
                      from upUbility in aim.UpUbilitys
                      where upUbility.Ability == SelectedAbilitiModelProperty
                      select new UpUbility() { QwToUp = aim, ValueToUp = upUbility.ValueToUp }).ToList();

            AllAimsToAbUps =
                PersProperty.Aims.Where(n => !QwUpAb.Any(q => q.QwToUp == n)).OrderBy(n => n.NameOfProperty).ToList();

            //QwUpAb
            //AllAimsToAbUps
        }

        /// <summary>
        /// Сортируем ранги по уровню
        /// </summary>
        private void sortRangs()
        {
            if (SelectedAbilitiModelProperty != null)
            {
                ObservableCollection<Rangs> selRangs = SelectedAbilitiModelProperty.Rangs;
                IOrderedEnumerable<Rangs> orderedEnumerable = selRangs.OrderByDescending(n => n.LevelRang);
                foreach (Rangs rangse in orderedEnumerable)
                {
                    selRangs.Move(selRangs.IndexOf(rangse), 0);
                }
            }
        }

        /// <summary>
        /// Цели и требования скиллов
        /// </summary>
        public class AimAndNeeds
        {
            /// <summary>
            /// Квест
            /// </summary>
            public Aim Aim { get; set; }

            /// <summary>
            /// Требования скиллов
            /// </summary>
            public IEnumerable<NeedAbility> NeedAbilitis { get; set; }
        }
    }

    public class HardnessAbAndNames
    {
        public HardnessAbAndNames(string nameOfHardness, int valOfHardness)
        {
            NameOfHardness = nameOfHardness;
            ValOfHardness = valOfHardness;
        }

        /// <summary>
        /// Название сложности
        /// </summary>
        public string NameOfHardness { get; set; }

        /// <summary>
        /// Числовое обозначение сложности
        /// </summary>
        public int ValOfHardness { get; set; }
    }

    public class RelCha
    {
        public Characteristic Charact { get; set; }

        public NeedAbility NeedAbility { get; set; }
    }

    public class ValuesAndDescriptions : INotifyPropertyChanged
    {
        private double _sellectedValue;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Цвет фона
        /// </summary>
        public Brush Brushh
        {
            get
            {
                if (SellectedValue >= MinValue && SellectedValue < MaxValue)
                {
                    return Brushes.Yellow;
                }

                return Brushes.Transparent;
            }
        }

        /// <summary>
        /// Обозначение
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Максимальное значение
        /// </summary>
        public double MaxValue { get; set; }

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// Выбранное значение
        /// </summary>
        public double SellectedValue
        {
            get
            {
                return _sellectedValue;
            }
            set
            {
                if (value.Equals(_sellectedValue)) return;
                _sellectedValue = value;
                OnPropertyChanged(nameof(SellectedValue));
                OnPropertyChanged(nameof(Brushh));
            }
        }

        /// <summary>
        /// Значение
        /// </summary>
        public double Value { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
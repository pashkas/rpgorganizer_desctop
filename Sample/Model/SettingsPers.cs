// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsPers.cs" company="">
// </copyright>
// <summary>
// Настройки персонажа
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Sample.Model
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Sample.Annotations;
    using Sample.Properties;

    [Serializable]
    public class ChaRangs
    {
        public int Ind
        {
            get { return StaticMetods.PersProperty.PersSettings.AbRangs.IndexOf(this); }
        }

        #region Public Properties

        public string Name { get; set; }

        /// <summary>
        /// Много, чуть-чуть и.т.д. Умное словесное описание
        /// </summary>
        public string Smart { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Настройки персонажа
    /// </summary>
    [Serializable]
    public class SettingsPers : INotifyPropertyChanged
    {
        public double GlobalBooster
        {
            get
            {
                return _globalBooster;
            }
            set
            {
                if (value.Equals(_globalBooster)) return;
                if (value <= 0) return;
                _globalBooster = value;
                foreach (var abiliti in StaticMetods.PersProperty.Abilitis)
                {
                    abiliti.Booster = value;
                }
                OnPropertyChanged(nameof(GlobalBooster));
            }
        }

        public List<Tuple<int, string>> WaveNames
        {
            get
            {
                if (_waveNames == null || _waveNames.Count != 7)
                {
                    _waveNames = new List<Tuple<int, string>>
                    {
                        new Tuple<int, string>(0,"Целый день"),
                        new Tuple<int, string>(1,"Утро"),
                        new Tuple<int, string>(2,"Дорога из дома"),
                        new Tuple<int, string>(3,"День"),
                        new Tuple<int, string>(4,"Дорога домой"),
                        new Tuple<int, string>(5,"Вечер"),
                        new Tuple<int, string>(6,"Перед сном")
                    };
                }

                return _waveNames;
            }
            set
            {
                if (Equals(value, _waveNames)) return;
                _waveNames = value;
                OnPropertyChanged(nameof(WaveNames));
            }
        }

        #region Public Properties

        /// <summary>
        /// Sets and gets Число колонок в окне настройки скиллов. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int AbColumnsInAvViewProperty
        {
            get
            {
                return abColumnsInAvView;
            }

            set
            {
                if (abColumnsInAvView == value)
                {
                    return;
                }

                abColumnsInAvView = value;
                OnPropertyChanged(nameof(AbColumnsInAvViewProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальный уровень для скиллов. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public int AbilMaxLevelProperty
        {
            get
            {
                return MaxAbLev;
            }

            set
            {
                return;
                if (this.abilMaxLevel == value)
                {
                    return;
                }

                this.abilMaxLevel = value;
                Settings.Default.MaxAbilLevel = value;
                this.OnPropertyChanged(nameof(AbilMaxLevelProperty));
            }
        }

        /// <summary>
        /// Ранги для скиллов по умолчанию.
        /// </summary>
        public ObservableCollection<Rangs> AbilRangsForDefoultProperty
        {
            get
            {
                if (this.abilRangsForDefoult == null)
                {
                    this.abilRangsForDefoult = new ObservableCollection<Rangs>
                                               {
                                                   new Rangs()
                                                   {
                                                       LevelRang = 0,
                                                       NameOfRang = "Бездарь!"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 1,
                                                       NameOfRang = "Новичок"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 2,
                                                       NameOfRang = "Ученик"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 3,
                                                       NameOfRang = "Стажер"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 4,
                                                       NameOfRang =
                                                           "Подмастерье"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 5,
                                                       NameOfRang = "Адепт"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 6,
                                                       NameOfRang = "Знаток"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 7,
                                                       NameOfRang = "Эксперт"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 8,
                                                       NameOfRang = "Профи"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 9,
                                                       NameOfRang = "Мастер"
                                                   },
                                                   new Rangs()
                                                   {
                                                       LevelRang = 10,
                                                       NameOfRang = "Гуру!"
                                                   }
                                               };
                }

                return this.abilRangsForDefoult;
            }

            set
            {
                if (this.abilRangsForDefoult == value)
                {
                    return;
                }

                this.abilRangsForDefoult = value;
                this.OnPropertyChanged(nameof(AbilRangsForDefoultProperty));
            }
        }

        /// <summary>
        /// Sets and gets Очков прокачки персонажа для его первого уровня. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public int AbOpsForFirstPersLevelProperty
        {
            get
            {
                if (AbOpsForFirstPersLevel == 0)
                {
                    AbOpsForFirstPersLevel = 4;
                }
                return AbOpsForFirstPersLevel;
            }

            set
            {
                if (AbOpsForFirstPersLevel == value)
                {
                    return;
                }

                AbOpsForFirstPersLevel = value;
                OnPropertyChanged(nameof(AbOpsForFirstPersLevelProperty));
                OnPropertyChanged(nameof(RecomendedMaxPersLevel));
            }
        }

        public int AbPointsForEasyQwest
        {
            get
            {
                if (_abPointsForEasyQwest == 0)
                {
                    _abPointsForEasyQwest = 1;
                }
                return _abPointsForEasyQwest;
            }
            set
            {
                if (value == _abPointsForEasyQwest) return;
                _abPointsForEasyQwest = value;
                OnPropertyChanged(nameof(AbPointsForEasyQwest));
            }
        }

        /// <summary>
        /// Очков скиллов на первом уровне
        /// </summary>
        public int AbPointsForFirstLevel
        {
            get
            {
                return 0;
                if (_abPointsForFirstLevel == 0)
                {
                    _abPointsForFirstLevel = 2;
                }
                return _abPointsForFirstLevel;
            }
            set
            {
                return;
                if (value == _abPointsForFirstLevel) return;
                _abPointsForFirstLevel = value;
                StaticMetods.PersProperty.UpdateAbilityPoints();
                OnPropertyChanged(nameof(AbPointsForFirstLevel));
            }
        }

        public int AbPointsForHardQwest
        {
            get
            {
                if (_abPointsForHardQwest == 0)
                {
                    _abPointsForHardQwest = 4;
                }
                return _abPointsForHardQwest;
            }
            set
            {
                if (value == _abPointsForHardQwest) return;
                _abPointsForHardQwest = value;
                OnPropertyChanged(nameof(AbPointsForHardQwest));
            }
        }

        /// <summary>
        /// Число скиллов за уровень
        /// </summary>
        public int AbPointsForLevel
        {
            get
            {
                if (_abPointsForLevel == 0)
                {
                    _abPointsForLevel = 7;
                }

                return _abPointsForLevel;
            }
            set
            {
                if (value == _abPointsForLevel) return;
                _abPointsForLevel = value;
                OnPropertyChanged(nameof(AbPointsForLevel));
                OnPropertyChanged(nameof(MaxPersLevelProperty));
            }
        }

        public int AbPointsForNormalQwest
        {
            get
            {
                if (_abPointsForNormalQwest == 0)
                {
                    _abPointsForNormalQwest = 2;
                }
                return _abPointsForNormalQwest;
            }
            set
            {
                if (value == _abPointsForNormalQwest) return;
                _abPointsForNormalQwest = value;
                OnPropertyChanged(nameof(AbPointsForNormalQwest));
            }
        }

        /// <summary>
        /// Очков скиллов за 1 уровень
        /// </summary>
        public double AbPointsForOneLevel
        {
            get
            {
                if (AutoCountAbPointsForLev) return StaticMetods.GetAutoRecauntAbPointsPerLev();
                if (_abPointsForOneLevel <= 0) return _abPointsForOneLevel = 3;
                return _abPointsForOneLevel;
            }
            set
            {
                if (value == _abPointsForOneLevel) return;
                _abPointsForOneLevel = value;
                OnPropertyChanged(nameof(AbPointsForOneLevel));
                RefreshPointsAndRangse();
            }
        }

        /// <summary>
        /// Очки скиллов начисляются за выполнение квестов
        /// </summary>
        public bool AbPointsForQwests
        {
            get
            {
                return _abPointsForQwests;
            }
            set
            {
                if (value == _abPointsForQwests) return;
                _abPointsForQwests = value;
                OnPropertyChanged(nameof(AbPointsForQwests));
                OnPropertyChanged(nameof(MaxPersLevelProperty));
            }
        }

        public int AbPointsForVeryHardQwest
        {
            get
            {
                if (_abPointsForVeryHardQwest == 0)
                {
                    _abPointsForVeryHardQwest = 6;
                }
                return _abPointsForVeryHardQwest;
            }
            set
            {
                if (value == _abPointsForVeryHardQwest) return;
                _abPointsForVeryHardQwest = value;
                OnPropertyChanged(nameof(AbPointsForVeryHardQwest));
            }
        }

        /// <summary>
        /// Ранги характеристик
        /// </summary>
        public List<ChaRangs> AbRangs
        {
            get
            {
                checkAbRangse(false);
                return _abRangs;
            }
            set
            {
                if (Equals(value, _abRangs)) return;
                _abRangs = value;
                OnPropertyChanged(nameof(AbRangs));
            }
        }

        /// <summary>
        /// Sets and gets Активный вид задач персонажа. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int ActivePersViewProperty
        {
            get
            {
                return activePersView;
            }

            set
            {
                if (activePersView == value)
                {
                    return;
                }

                activePersView = value;
                OnPropertyChanged(nameof(ActivePersViewProperty));
            }
        }

        /// <summary>
        /// Авторасчет очков скиллов за 1 уровень. Так, чтобы подгонялось под 50-й уровень.
        /// </summary>
        public bool AutoCountAbPointsForLev
        {
            get
            {
                return _autoCountAbPointsForLev;
            }
            set
            {
                if (value == _autoCountAbPointsForLev) return;
                _autoCountAbPointsForLev = value;
                OnPropertyChanged(nameof(AutoCountAbPointsForLev));
                OnPropertyChanged(nameof(IsMaySetAbPoints));
                OnPropertyChanged(nameof(AbPointsForOneLevel));
                RefreshPointsAndRangse();
            }
        }

        /// <summary>
        /// Sets and gets Число колонок характеристик. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int ChaColumnsInChViewProperty
        {
            get
            {
                return chaColumnsInChView;
            }

            set
            {
                if (chaColumnsInChView == value)
                {
                    return;
                }

                chaColumnsInChView = value;
                OnPropertyChanged(nameof(ChaColumnsInChViewProperty));
            }
        }

        /// <summary>
        /// Ранги характеристик
        /// </summary>
        public List<ChaRangs> CharacteristicRangs
        {
            get
            {
                CheckChaRangse(false);
                return _characteristicRangs;
            }
            set
            {
                if (Equals(value, _characteristicRangs)) return;
                _characteristicRangs = value;
                OnPropertyChanged(nameof(CharacteristicRangs));
            }
        }

        /// <summary>
        /// Sets and gets Ранги для характеристик по умолчанию. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public ObservableCollection<Rangs> CharactRangsForDefoultProperty
        {
            get
            {
                if (this.charactRangsForDefoult == null)
                {
                    this.charactRangsForDefoult = new ObservableCollection<Rangs>
                                                  {
                                                      new Rangs()
                                                      {
                                                          LevelRang = 0,
                                                          NameOfRang =
                                                              "Отвратительно!"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 1,
                                                          NameOfRang =
                                                              "Ужасно"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 2,
                                                          NameOfRang = "Плохо"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 3,
                                                          NameOfRang = "Слабо"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 4,
                                                          NameOfRang =
                                                              "Посредственно"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 5,
                                                          NameOfRang =
                                                              "Нормально"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 6,
                                                          NameOfRang =
                                                              "Выше среднего"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 7,
                                                          NameOfRang =
                                                              "Хорошо"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 8,
                                                          NameOfRang =
                                                              "Отлично"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 9,
                                                          NameOfRang = "Супер"
                                                      },
                                                      new Rangs()
                                                      {
                                                          LevelRang = 10,
                                                          NameOfRang =
                                                              "Легендарно!"
                                                      }
                                                  };

                    // Добавляем ранги по умолчанию
                }

                return this.charactRangsForDefoult;
            }

            set
            {
                if (this.charactRangsForDefoult == value)
                {
                    return;
                }

                this.charactRangsForDefoult = value;
                this.OnPropertyChanged(nameof(CharactRangsForDefoultProperty));
            }
        }

        /// <summary>
        /// Sets and gets Цвет рамки для задач по умолчанию. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public string ColorTaskBorderProperty
        {
            get
            {
                return this.colorTaskBorder;
            }

            set
            {
                if (this.colorTaskBorder == value)
                {
                    return;
                }

                this.colorTaskBorder = value;
                this.OnPropertyChanged(nameof(ColorTaskBorderProperty));
            }
        }

        /// <summary>
        /// Sets and gets Число колонок в альтернативном виде. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public int ColumnsInAlternateViewProperty
        {
            get
            {
                return this.columnsInAlternateView;
            }

            set
            {
                if (this.columnsInAlternateView == value)
                {
                    return;
                }

                this.columnsInAlternateView = value;
                this.OnPropertyChanged(nameof(ColumnsInAlternateViewProperty));
            }
        }

        /// <summary>
        /// Sets and gets Количество колонок задач в главном окне. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int ColumnsTasksInMainViewProperty
        {
            get
            {
                return columnsTasksInMainView;
            }

            set
            {
                if (columnsTasksInMainView == value)
                {
                    return;
                }

                columnsTasksInMainView = value;
                OnPropertyChanged(nameof(ColumnsTasksInMainViewProperty));
            }
        }

        /// <summary>
        /// Урон за пропущенную задачу
        /// </summary>
        public int DamageFromeTask
        {
            get
            {
                return _damageFromeTask;
            }
            set
            {
                if (value == _damageFromeTask) return;
                if (value < 0) value = 0;
                _damageFromeTask = value;
                OnPropertyChanged(nameof(DamageFromeTask));
            }
        }

        /// <summary>
        /// Урон за привычку
        /// </summary>
        public int DamageFromHabbit
        {
            get
            {
                return _damageFromHabbit;
            }
            set
            {
                if (value == _damageFromHabbit) return;
                if (value < 0) value = 0;
                _damageFromHabbit = value;
                OnPropertyChanged(nameof(DamageFromHabbit));
            }
        }

        /// <summary>
        /// Тип задач по умолчанию для скиллов
        /// </summary>
        public TypeOfTask DefoultTaskTypeForAbills
        {
            get
            {
                return _defoultTaskTypeForAbills;
            }
            set
            {
                if (Equals(value, _defoultTaskTypeForAbills)) return;
                _defoultTaskTypeForAbills = value;
                OnPropertyChanged(nameof(DefoultTaskTypeForAbills));
            }
        }

        /// <summary>
        /// Тип задач по умолчанию для квестов
        /// </summary>
        public TypeOfTask DefoultTaskTypeForQwests
        {
            get { return _defoultTaskTypeForQwests; }
            set { _defoultTaskTypeForQwests = value; }
        }

        /// <summary>
        /// Отключить звуки
        /// </summary>
        public bool DisableSounds
        {
            get
            {
                return _disableSounds;
            }
            set
            {
                if (value == _disableSounds) return;
                _disableSounds = value;
                OnPropertyChanged(nameof(DisableSounds));
            }
        }

        /// <summary>
        /// Sets and gets Делить карту квестов на разные уровни. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool DivideByLevelsProperty
        {
            get
            {
                return this.divideByLevels;
            }

            set
            {
                if (this.divideByLevels == value)
                {
                    return;
                }

                this.divideByLevels = value;
                this.OnPropertyChanged(nameof(DivideByLevelsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активировать потребности. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool EnableNeednessProperty
        {
            get
            {
                return enableNeedness;
            }

            set
            {
                if (enableNeedness == value)
                {
                    return;
                }

                enableNeedness = value;
                OnPropertyChanged(nameof(EnableNeednessProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активировать очки прокачки?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool EnableOPProperty
        {
            get
            {
                return enableOP;
            }

            set
            {
                if (enableOP == value)
                {
                    return;
                }

                enableOP = value;
                OnPropertyChanged(nameof(EnableOPProperty));
            }
        }

        /// <summary>
        /// Sets and gets Опыта за сложную задачу. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int ExpForHardTaskProperty
        {
            get
            {
                return this.expForHardTask;
            }

            set
            {
                if (this.expForHardTask == value)
                {
                    return;
                }

                this.expForHardTask = value;
                this.OnPropertyChanged(nameof(ExpForHardTaskProperty));
            }
        }

        /// <summary>
        /// Sets and gets Опыта за нормальную задачу. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int ExpForNormalTaskProperty
        {
            get
            {
                return this.expForNormalTask;
            }

            set
            {
                if (this.expForNormalTask == value)
                {
                    return;
                }

                this.expForNormalTask = value;
                this.OnPropertyChanged(nameof(ExpForNormalTaskProperty));
            }
        }

        /// <summary>
        /// Sets and gets Опыта за очень сложную задачу. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int ExpForVeryHardTaskProperty
        {
            get
            {
                return this.expForVeryHardTask;
            }

            set
            {
                if (this.expForVeryHardTask == value)
                {
                    return;
                }

                this.expForVeryHardTask = value;
                this.OnPropertyChanged(nameof(ExpForVeryHardTaskProperty));
            }
        }

        /// <summary>
        /// Sets and gets Опыт также за выполнение задач и квестов. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool ExpFromTasksAndQwestsProperty
        {
            get
            {
                return expFromTasksAndQwests;
            }

            set
            {
                if (expFromTasksAndQwests == value)
                {
                    return;
                }

                expFromTasksAndQwests = value;
                OnPropertyChanged(nameof(ExpFromTasksAndQwestsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Количество строк в фокусе. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int FocusRowsProperty
        {
            get
            {
                return this.FocusRows;
            }

            set
            {
                if (this.FocusRows == value)
                {
                    return;
                }

                this.FocusRows = value;
                this.OnPropertyChanged(nameof(FocusRowsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Золото за сложную задачу. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int GoldForHardTaskProperty
        {
            get
            {
                return this.goldForHardTask;
            }

            set
            {
                if (this.goldForHardTask == value)
                {
                    return;
                }

                this.goldForHardTask = value;
                this.OnPropertyChanged(nameof(GoldForHardTaskProperty));
            }
        }

        /// <summary>
        /// Sets and gets Золото за выполнение нормальной задачи. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int GoldForNormalTaskProperty
        {
            get
            {
                return this.goldForNormalTask;
            }

            set
            {
                if (this.goldForNormalTask == value)
                {
                    return;
                }

                this.goldForNormalTask = value;
                this.OnPropertyChanged(nameof(GoldForNormalTaskProperty));
            }
        }

        /// <summary>
        /// Sets and gets Золото за очень сложную задачу. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int GoldForVeryHardTaskProperty
        {
            get
            {
                return this.goldForVeryHardTask;
            }

            set
            {
                if (this.goldForVeryHardTask == value)
                {
                    return;
                }

                this.goldForVeryHardTask = value;
                this.OnPropertyChanged(nameof(GoldForVeryHardTaskProperty));
            }
        }

        public int HabbitDays
        {
            get
            {
                double days = AbilMaxLevelProperty * (AbilMaxLevelProperty + 1) / 2.0;
                double all = days * TaskCountDefoultPrivichkaProperty;
                return Convert.ToInt32(all);
            }
        }

        /// <summary>
        /// Скрывать выполненные в карте
        /// </summary>
        public bool HideDoneQwestInMap { get; set; }

        /// <summary>
        /// Sets and gets Скрывать зону фокусировки. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool HideFocusFieldProperty
        {
            get
            {
                return hideFocusField;
            }

            set
            {
                if (hideFocusField == value)
                {
                    return;
                }

                hideFocusField = value;
                OnPropertyChanged(nameof(HideFocusFieldProperty));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать изображения во 2-м виде задач. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool HideImag2ViewProperty
        {
            get
            {
                return hideImag2View;
            }

            set
            {
                if (hideImag2View == value)
                {
                    return;
                }

                hideImag2View = value;
                OnPropertyChanged(nameof(HideImag2ViewProperty));
                OnPropertyChanged(nameof(Image2ViewVisibility));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать неактивные скиллы. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool HideNotActiveAbilitidProperty
        {
            get
            {
                return this.hideNotActiveAbilitid;
            }

            set
            {
                if (this.hideNotActiveAbilitid == value)
                {
                    return;
                }

                this.hideNotActiveAbilitid = value;
                this.OnPropertyChanged(nameof(HideNotActiveAbilitidProperty));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать неактивные скиллы в дереве. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool HideNotActiveAbilitisInTreeProperty
        {
            get
            {
                return this.hideNotActiveAbilitisInTree;
            }

            set
            {
                if (this.hideNotActiveAbilitisInTree == value)
                {
                    return;
                }

                this.hideNotActiveAbilitisInTree = value;
                this.OnPropertyChanged(nameof(HideNotActiveAbilitisInTreeProperty));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать неактивные перки?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool HideNotActivPerksProperty
        {
            get
            {
                return hideNotActivPerks;
            }

            set
            {
                if (hideNotActivPerks == value)
                {
                    return;
                }

                hideNotActivPerks = value;
                OnPropertyChanged(nameof(HideNotActivPerksProperty));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать не доступные скиллы в дереве. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool HideNotAllowAbilitisInTreeProperty
        {
            get
            {
                return this.hideNotAllowAbilitisInTree;
            }

            set
            {
                if (this.hideNotAllowAbilitisInTree == value)
                {
                    return;
                }

                this.hideNotAllowAbilitisInTree = value;
                this.OnPropertyChanged(nameof(HideNotAllowAbilitisInTreeProperty));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать квесты без активных задач из фокусировки. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool HideQwestsWithoutTasksInFocusProperty
        {
            get
            {
                return this.hideQwestsWithoutTasksInFocus;
            }

            set
            {
                if (this.hideQwestsWithoutTasksInFocus == value)
                {
                    return;
                }

                this.hideQwestsWithoutTasksInFocus = value;
                this.OnPropertyChanged(nameof(HideQwestsWithoutTasksInFocusProperty));
            }
        }

        /// <summary>
        /// Sets and gets Режим Героев меча и магии, нужно нажать кнопку для завершения хода. Changes
        /// to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool HOMMModeProperty
        {
            get
            {
                return HOMMMode;
            }

            set
            {
                if (HOMMMode == value)
                {
                    return;
                }

                HOMMMode = value;
                OnPropertyChanged(nameof(HOMMModeProperty));
            }
        }

        /// <summary>
        /// Sets and gets Минут до снижения ХП до нуля. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int HourseToNullHpProperty
        {
            get
            {
                return 0;
            }

            set
            {
                if (hourseToNullHP == value)
                {
                    return;
                }

                hourseToNullHP = value;
                OnPropertyChanged(nameof(HourseToNullHpProperty));
            }
        }

        /// <summary>
        /// Видимость картинок задач во втором виде
        /// </summary>
        public Visibility Image2ViewVisibility
        {
            get
            {
                if (HideImag2ViewProperty == true)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Авторасчет на 100 уровней?
        /// </summary>
        public bool Is100Levels
        {
            get
            {
                return _is100Levels;
            }
            set
            {
                if (value == _is100Levels) return;
                _is100Levels = value;
                OnPropertyChanged(nameof(Is100Levels));
                RefreshPointsAndRangse();
            }
        }

        /// <summary>
        /// Sets and gets Группы скиллов это характеристики. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsAbGroupsIsCharacteristicsProperty
        {
            get
            {
                return isAbGroupsIsCharacteristics;
            }

            set
            {
                if (isAbGroupsIsCharacteristics == value)
                {
                    return;
                }

                isAbGroupsIsCharacteristics = value;
                OnPropertyChanged(nameof(IsAbGroupsIsCharacteristicsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Автоначало активности скиллов по умолчанию?. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool IsAbilAutoStartProperty
        {
            get
            {
                return this.isAbilAutoStart;
            }

            set
            {
                if (this.isAbilAutoStart == value)
                {
                    return;
                }

                this.isAbilAutoStart = value;
                this.OnPropertyChanged(nameof(IsAbilAutoStartProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активировать очки скиллов?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsAbPointsActiveProperty
        {
            get
            {
                return isAbPointsActive;
            }

            set
            {
                if (isAbPointsActive == value)
                {
                    return;
                }

                isAbPointsActive = value;
                OnPropertyChanged(nameof(IsAbPointsActiveProperty));
            }
        }

        /// <summary>
        /// Активировать ранги?
        /// </summary>
        public bool IsActtivateRangse
        {
            get
            {
                return true;
                //!_isActtivateRangse;
            }
            set
            {
                if (!value == _isActtivateRangse) return;
                _isActtivateRangse = !value;
                OnPropertyChanged(nameof(IsActtivateRangse));
            }
        }

        /// <summary>
        /// Sets and gets Активированы элементы автофокуса?. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsAutofocusEnabledProperty
        {
            get
            {
                return isAutofocusEnabled;
            }

            set
            {
                if (isAutofocusEnabled == value)
                {
                    return;
                }

                isAutofocusEnabled = value;
                OnPropertyChanged(nameof(IsAutofocusEnabledProperty));
            }
        }

        /// <summary>
        /// Sets and gets Можно нажать плюс только если все подзадачи выполненны. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsCanDownPlusOnliIfSubtasksDoneProperty
        {
            get
            {
                return this.isCanDownPlusOnliIfSubtasksDone;
            }

            set
            {
                if (this.isCanDownPlusOnliIfSubtasksDone == value)
                {
                    return;
                }

                this.isCanDownPlusOnliIfSubtasksDone = value;
                this.OnPropertyChanged(nameof(IsCanDownPlusOnliIfSubtasksDoneProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активированна ли автофокусировка для характеристик. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsChaAvtofocusEnabledProperty
        {
            get
            {
                return this.isChaAvtofocusEnabled;
            }

            set
            {
                if (this.isChaAvtofocusEnabled == value)
                {
                    return;
                }

                this.isChaAvtofocusEnabled = value;
                this.OnPropertyChanged(nameof(IsChaAvtofocusEnabledProperty));
            }
        }

        /// <summary>
        /// Sets and gets Характеристики делятся на группы. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsChaGroupProperty
        {
            get
            {
                return this.isChaGroup;
            }

            set
            {
                if (this.isChaGroup == value)
                {
                    return;
                }

                this.isChaGroup = value;
                this.OnPropertyChanged(nameof(IsChaGroupProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активированна ли картинка с характеристиками в главном окне?. Changes to
        /// that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsChaPicMainWindowEnabledProperty
        {
            get
            {
                return this.isChaPicMainWindowEnabled;
            }

            set
            {
                if (this.isChaPicMainWindowEnabled == value)
                {
                    return;
                }

                this.isChaPicMainWindowEnabled = value;
                this.OnPropertyChanged(nameof(IsChaPicMainWindowEnabledProperty));
            }
        }

        /// <summary>
        /// Баланс характеристик (нельзя прокачивать самые сильные)
        /// </summary>
        public bool IsCharactBalanse
        {
            get
            {
                return _isCharactBalanse;
            }
            set
            {
                if (value == _isCharactBalanse) return;
                _isCharactBalanse = value;
                OnPropertyChanged(nameof(IsCharactBalanse));
            }
        }

        /// <summary>
        /// Отображать урон вместо ХП
        /// </summary>
        public bool IsDamageNotHP
        {
            get
            {
                return _isDamageNotHp;
            }
            set
            {
                if (value == _isDamageNotHp) return;
                _isDamageNotHp = value;
                OnPropertyChanged(nameof(IsDamageNotHP));
            }
        }

        /// <summary>
        /// Динамический расчет опыта
        /// </summary>
        public bool IsDynamicExpCount
        {
            get
            {
                return _isDynamicExpCount;
            }
            set
            {
                if (value == _isDynamicExpCount) return;
                _isDynamicExpCount = value;
                OnPropertyChanged(nameof(IsDynamicExpCount));
            }
        }

        /// <summary>
        /// Sets and gets Альтернативное отображение задач активировано?. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool IsEnabledAlterneteTaskViewProperty
        {
            get
            {
                return this.isEnabledAlterneteTaskView;
            }

            set
            {
                if (this.isEnabledAlterneteTaskView == value)
                {
                    return;
                }

                this.isEnabledAlterneteTaskView = value;
                this.OnPropertyChanged(nameof(IsEnabledAlterneteTaskViewProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активен первый вид отображения задач. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool IsEnabledFirstViewProperty
        {
            get
            {
                return isEnabledFirstView;
            }

            set
            {
                if (isEnabledFirstView == value)
                {
                    return;
                }

                isEnabledFirstView = value;
                OnPropertyChanged(nameof(IsEnabledFirstViewProperty));
            }
        }

        /// <summary>
        /// Фиксированный максимальный уровень персонажа?
        /// </summary>
        public bool IsFixMaxPLevel
        {
            get
            {
                return _isFixMaxPLevel;
            }
            set
            {
                if (value == _isFixMaxPLevel) return;
                _isFixMaxPLevel = value;
                StaticMetods.PersProperty.RefreshMaxPersLevel();
                OnPropertyChanged(nameof(IsFixMaxPLevel));
            }
        }

        /// <summary>
        /// Sets and gets Фокусировка для новых скиллов по умолчанию. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool IsFocusForDefoultForNewAbilityProperty
        {
            get
            {
                return this.isFocusForDefoultForNewAbility;
            }

            set
            {
                if (this.isFocusForDefoultForNewAbility == value)
                {
                    return;
                }

                this.isFocusForDefoultForNewAbility = value;
                this.OnPropertyChanged(nameof(IsFocusForDefoultForNewAbilityProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активирован 4 вид. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsFourViewEnabledProperty
        {
            get
            {
                return this.isFourViewEnabled;
            }

            set
            {
                if (this.isFourViewEnabled == value)
                {
                    return;
                }

                this.isFourViewEnabled = value;
                this.OnPropertyChanged(nameof(IsFourViewEnabledProperty));
            }
        }

        /// <summary>
        /// Sets and gets Режим FUDGE активирован. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsFudgeModeProperty
        {
            get
            {
                return isFudgeMode;
            }

            set
            {
                if (isFudgeMode == value)
                {
                    return;
                }

                isFudgeMode = value;
                OnPropertyChanged(nameof(IsFudgeModeProperty));
            }
        }

        /// <summary>
        /// Активирован вид, когда отображаются только картинки монстров в задачах?
        /// </summary>
        public bool IsGhostBastersMode
        {
            get
            {
                return false; //_isGhostBastersMode;
            }
            set
            {
                return;
                if (value == _isGhostBastersMode) return;
                _isGhostBastersMode = value;
                OnPropertyChanged(nameof(IsGhostBastersMode));
                //foreach (var task in StaticMetods.PersProperty.Tasks)
                //{
                //    task.RefreshBackGround();
                //}
                //StaticMetods.Locator.MainVM.RefreshTasksInMainView();
            }
        }

        /// <summary>
        /// Золото активно?
        /// </summary>
        public bool IsGoldEnabled
        {
            get
            {
                return _isGoldEnabled;
            }
            set
            {
                if (value == _isGoldEnabled) return;
                _isGoldEnabled = value;
                OnPropertyChanged(nameof(IsGoldEnabled));
            }
        }

        /// <summary>
        /// Sets and gets Можно покупать сложные скиллы только после приобретения простых. Changes to
        /// that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsHardAbAfterEasyProperty
        {
            get
            {
                return isHardAbAfterEasy;
            }

            set
            {
                if (isHardAbAfterEasy == value)
                {
                    return;
                }

                isHardAbAfterEasy = value;
                OnPropertyChanged(nameof(IsHardAbAfterEasyProperty));
            }
        }

        /// <summary>
        /// Задачи навыков одна за одной?
        /// </summary>
        public bool IsHideAbTasksOneByOne
        {
            get
            {
                return _isHideAbTasksOneByOne;
            }
            set
            {
                if (value == _isHideAbTasksOneByOne) return;
                _isHideAbTasksOneByOne = value;
                OnPropertyChanged(nameof(IsHideAbTasksOneByOne));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать характеристики в главном окне?. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool IsHideCharactsInMainWindowProperty
        {
            get
            {
                return isHideCharactsInMainWindow;
            }

            set
            {
                if (isHideCharactsInMainWindow == value)
                {
                    return;
                }

                isHideCharactsInMainWindow = value;
                OnPropertyChanged(nameof(IsHideCharactsInMainWindowProperty));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать класс персонажа. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsHideClassProperty
        {
            get
            {
                return this.isHideClass;
            }

            set
            {
                if (this.isHideClass == value)
                {
                    return;
                }

                this.isHideClass = value;
                this.OnPropertyChanged(nameof(IsHideClassProperty));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать дневник?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsHideDiaryProperty
        {
            get
            {
                return isHideDiary;
            }

            set
            {
                if (isHideDiary == value)
                {
                    return;
                }

                isHideDiary = value;
                OnPropertyChanged(nameof(IsHideDiaryProperty));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать картинки в скиллах. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsHideImagePropertysInChangesProperty
        {
            get
            {
                return this.isHideImagePropertysInChanges;
            }

            set
            {
                if (this.isHideImagePropertysInChanges == value)
                {
                    return;
                }

                this.isHideImagePropertysInChanges = value;
                this.OnPropertyChanged(nameof(IsHideImagePropertysInChangesProperty));
            }
        }

        /// <summary>
        /// Скрывать не выделенные ссылки?
        /// </summary>
        public bool IsHideNotSellectedLink
        {
            get
            {
                return true;
                //return _isHideNotSellectedLink;
            }
            set
            {
                if (value == _isHideNotSellectedLink) return;
                _isHideNotSellectedLink = value;
                OnPropertyChanged(nameof(IsHideNotSellectedLink));
            }
        }

        /// <summary>
        /// Делать квесты неактивными если задачи скиллов неактивны
        /// </summary>
        public bool IsHideQwestsFromAbLink
        {
            get
            {
                return false; //_isHideQwestsFromAbLink;
            }
            set
            {
                if (value == _isHideQwestsFromAbLink) return;
                _isHideQwestsFromAbLink = value;
                OnPropertyChanged(nameof(IsHideQwestsFromAbLink));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать рассу. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsHideRaseProperty
        {
            get
            {
                return this.isHideRase;
            }

            set
            {
                if (this.isHideRase == value)
                {
                    return;
                }

                this.isHideRase = value;
                this.OnPropertyChanged(nameof(IsHideRaseProperty));
            }
        }

        /// <summary>
        /// Задачи навыков одна за другой в Андроид?
        /// </summary>
        public bool IsHideTasksOneByOneAndroid
        {
            get
            {
                return _isHideTasksOneByOneAndroid;
            }
            set
            {
                if (value == _isHideTasksOneByOneAndroid) return;
                _isHideTasksOneByOneAndroid = value;
                OnPropertyChanged(nameof(IsHideTasksOneByOneAndroid));
            }
        }

        public bool IsHP
        {
            get
            {
                return false;
                //return _isHp;
            }
            set
            {
                //if (value == _isHp) return;
                //_isHp = value;
                //OnPropertyChanged(nameof(IsHP));
            }
        }

        /// <summary>
        /// Sets and gets Активировать здоровье?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsHPActiveteProperty
        {
            get
            {
                return false;
                //return isHPActivete;
            }

            set
            {
                if (isHPActivete == value)
                {
                    return;
                }

                if (isHPActivete == false && value == true)
                {
                    StaticMetods.PersProperty.HitPoints = 100;
                }

                isHPActivete = value;
                OnPropertyChanged(nameof(IsHPActiveteProperty));
            }
        }

        /// <summary>
        /// Sets and gets Хп снижаются по таймеру. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsHpMinusForTimerProperty
        {
            get
            {
                return isHpMinusForTimer;
            }

            set
            {
                if (isHpMinusForTimer == value)
                {
                    return;
                }

                isHpMinusForTimer = value;
                OnPropertyChanged(nameof(IsHpMinusForTimerProperty));
            }
        }

        public bool IsMaySetAbPoints => !AutoCountAbPointsForLev;

        /// <summary>
        /// Sets and gets При здоровье = 0 опыт отнимается до предыдущего уровня?. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsMinusExpHPProperty
        {
            get
            {
                return isMinusExpHP;
            }

            set
            {
                if (isMinusExpHP == value)
                {
                    return;
                }

                isMinusExpHP = value;
                OnPropertyChanged(nameof(IsMinusExpHPProperty));
            }
        }

        public bool IsNotOwerriteCursor
        {
            get
            {
                return Settings.Default.IsNotOwerriteCursor;
            }
            set
            {
                Settings.Default.IsNotOwerriteCursor = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(IsNotOwerriteCursor));
            }
        }

        /// <summary>
        /// Sets and gets Сразу открывать карту квестов из главного окна. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool IsOpenQwestMapProperty
        {
            get
            {
                return isOpenQwestMap;
            }

            set
            {
                if (isOpenQwestMap == value)
                {
                    return;
                }

                isOpenQwestMap = value;
                OnPropertyChanged(nameof(IsOpenQwestMapProperty));
            }
        }

        /// <summary>
        /// Sets and gets Включить защиту паролем?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsPassProtectProperty
        {
            get
            {
                return this.isPassProtect;
            }

            set
            {
                if (this.isPassProtect == value)
                {
                    return;
                }

                this.isPassProtect = value;
                Settings.Default.isPassProtect = this.IsPassProtectProperty;
                this.OnPropertyChanged(nameof(IsPassProtectProperty));
            }
        }

        /// <summary>
        /// Sets and gets Прогрессбары вместо звездочек. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsProgressWhithoutStarsProperty
        {
            get
            {
                return this.isProgressWhithoutStars;
            }

            set
            {
                if (this.isProgressWhithoutStars == value)
                {
                    return;
                }

                this.isProgressWhithoutStars = value;
                this.OnPropertyChanged(nameof(IsProgressWhithoutStarsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Автоактивность квестов по умолчанию. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsQwestAutoActiveForDefoultProperty
        {
            get
            {
                return isQwestAutoActiveForDefoult;
            }

            set
            {
                if (isQwestAutoActiveForDefoult == value)
                {
                    return;
                }

                isQwestAutoActiveForDefoult = value;
                OnPropertyChanged(nameof(IsQwestAutoActiveForDefoultProperty));
            }
        }

        /// <summary>
        /// Sets and gets Группы квестов равно характеристики. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsQwestGroupsIsCharacteristicProperty
        {
            get
            {
                return isQwestGroupsIsCharacteristic;
            }

            set
            {
                if (isQwestGroupsIsCharacteristic == value)
                {
                    return;
                }

                isQwestGroupsIsCharacteristic = value;
                OnPropertyChanged(nameof(IsQwestGroupsIsCharacteristicProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активированна ли автоматическая фокусировка для квестов?. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsQwestsAvtofocusEnabledProperty
        {
            get
            {
                return this.isQwestsAvtofocusEnabled;
            }

            set
            {
                if (this.isQwestsAvtofocusEnabled == value)
                {
                    return;
                }

                this.isQwestsAvtofocusEnabled = value;
                this.OnPropertyChanged(nameof(IsQwestsAvtofocusEnabledProperty));
            }
        }

        /// <summary>
        /// Sets and gets Квесты делятся на группы. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsQwestsGroupProperty
        {
            get
            {
                return this.isQwestsGroup;
            }

            set
            {
                if (this.isQwestsGroup == value)
                {
                    return;
                }

                this.isQwestsGroup = value;
                this.OnPropertyChanged(nameof(IsQwestsGroupProperty));
            }
        }

        /// <summary>
        /// Sets and gets Короткие сообщения если задача или квест выполненны. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsShortMesWhenTaskOrQwestDoneProperty
        {
            get
            {
                return _isShortMesWhenTaskOrQwestDone;
            }

            set
            {
                if (_isShortMesWhenTaskOrQwestDone == value)
                {
                    return;
                }

                _isShortMesWhenTaskOrQwestDone = value;
                OnPropertyChanged(nameof(IsShortMesWhenTaskOrQwestDoneProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показывать название группы скиллов. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsShowAbGroupNameProperty
        {
            get
            {
                return isShowAbGroupName;
            }

            set
            {
                if (isShowAbGroupName == value)
                {
                    return;
                }

                isShowAbGroupName = value;
                OnPropertyChanged(nameof(IsShowAbGroupNameProperty));
            }
        }

        /// <summary>
        /// Утро день вечер по очереди?
        /// </summary>
        public bool IsShowTasksByTime
        {
            get
            {
                //return true;
                return _isShowTasksByTime;
            }
            set
            {
                if (value == _isShowTasksByTime) return;
                _isShowTasksByTime = value;
                OnPropertyChanged(nameof(IsShowTasksByTime));
            }
        }

        /// <summary>
        /// Утро день вечер по очереди андроид
        /// </summary>
        public bool IsShowTasksByTimeAndroid
        {
            get
            {
                return _isShowTasksByTimeAndroid;
            }
            set
            {
                if (value == _isShowTasksByTimeAndroid) return;
                _isShowTasksByTimeAndroid = value;
                OnPropertyChanged(nameof(IsShowTasksByTimeAndroid));
            }
        }

        /// <summary>
        /// Умная сортировка
        /// </summary>
        public bool IsSmartTaskSort
        {
            get
            {
                return _isSmartTaskSort;
            }
            set
            {
                if (value == _isSmartTaskSort) return;
                _isSmartTaskSort = value;
                OnPropertyChanged(nameof(IsSmartTaskSort));
            }
        }

        /// <summary>
        /// Sets and gets Показывать мотиватор во время завершения задачи. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool IsTaskDoneMotivatorShowProperty
        {
            get
            {
                return isTaskDoneMotivatorShow;
            }

            set
            {
                if (isTaskDoneMotivatorShow == value)
                {
                    return;
                }

                isTaskDoneMotivatorShow = value;
                OnPropertyChanged(nameof(IsTaskDoneMotivatorShowProperty));
            }
        }

        /// <summary>
        /// Sets and gets Если здоровье меньше 25% функции программы становятся ограниченными?.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsTerribleDebufHPProperty
        {
            get
            {
                return isTerribleDebufHP;
            }

            set
            {
                if (isTerribleDebufHP == value)
                {
                    return;
                }

                isTerribleDebufHP = value;
                OnPropertyChanged(nameof(IsTerribleDebufHPProperty));
            }
        }

        /// <summary>
        /// Опыт расчитывается в зависимости от... Короче, как в Морровинде!
        /// </summary>
        public bool IsTesExp
        {
            get
            {
                return true;
                return _isTesExp;
            }
            set
            {
                if (value == _isTesExp) return;
                _isTesExp = value;
                OnPropertyChanged(nameof(IsTesExp));
            }
        }

        /// <summary>
        /// Sets and gets Третий вид активирован. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsThreedViewActiveProperty
        {
            get
            {
                return this.isThreedViewActive;
            }

            set
            {
                if (this.isThreedViewActive == value)
                {
                    return;
                }

                this.isThreedViewActive = value;
                this.OnPropertyChanged(nameof(IsThreedViewActiveProperty));
            }
        }

        /// <summary>
        /// Sets and gets Коэффициент влияния скиллов на опыт. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public double KAbToExpRelayProperty
        {
            get
            {
                if (Math.Abs(kAbToExpRelay) < 0.0001)
                {
                    kAbToExpRelay = 0.5;
                }

                return kAbToExpRelay;
            }

            set
            {
                if (Math.Abs(kAbToExpRelay - value) < 0.01)
                {
                    return;
                }

                kAbToExpRelay = value;
                OnPropertyChanged(nameof(KAbToExpRelayProperty));
            }
        }

        /// <summary>
        /// Коэффициент уровня персонажа
        /// </summary>
        public double KPersLevel
        {
            get
            {
                if (_kPersLevel < 1)
                {
                    _kPersLevel = 2;
                }
                return _kPersLevel;
            }
            set
            {
                if (value == _kPersLevel) return;
                _kPersLevel = value;
                var prs = StaticMetods.PersProperty;
                prs.RefreshMaxPersLevel();
                OnPropertyChanged(nameof(KPersLevel));
                OnPropertyChanged(nameof(MaxPLevel));
            }
        }

        /// <summary>
        /// Последний файл-андроид для импорта с телефона
        /// </summary>
        public string LastAndroidFile { get; set; }

        /// <summary>
        /// Sets and gets За сколько уровней дается одна звездочка. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int LevelsInStarProperty
        {
            get
            {
                return this.levelsInStar;
            }

            set
            {
                if (this.levelsInStar == value)
                {
                    return;
                }

                this.levelsInStar = value;
                Settings.Default.LevelsInStar = value;
                this.OnPropertyChanged(nameof(LevelsInStarProperty));
            }
        }

        /// <summary>
        /// Максимальный уровень навыков
        /// </summary>
        public int MaxAbLev
        {
            get
            {
                if (Is10AbLevels)
                    return 10;

                if (Is5_5_50)
                    return 5;

                if (IsFUDGE)
                    return 6;

                return 100;

            }
            set
            {
                return;
            }
        }

        /// <summary>
        /// Без навыков.
        /// </summary>
        public bool IsNoAbs
        {
            get
            {
                return _isNoAbs;
            }
            set
            {
                if (value == _isNoAbs) return;
                _isNoAbs = value;

                Dictionary<AbilitiModel, double> abProgress = GetAbProgDict();

                if (value)
                {
                    _is5_5_50 = false;
                    _is10AbLevels = false;
                    _isFUDGE = false;
                    _isNoAbs = true;
                }

                RefrAb(abProgress);

                OnPropertyChanged(nameof(Is5_5_50));
                OnPropertyChanged(nameof(Is10AbLevels));
                OnPropertyChanged(nameof(IsFUDGE));
                OnPropertyChanged(nameof(IsNoAbs));
            }
        }

        /// <summary>
        /// В стиле Fudge.
        /// </summary>
        public bool IsFUDGE
        {
            get
            {
                return _isFUDGE;
            }
            set
            {
                if (value == _isFUDGE) return;

                Dictionary<AbilitiModel, double> abProgress = GetAbProgDict();

                if (value)
                {
                    _is5_5_50 = false;
                    _is10AbLevels = false;
                    _isNoAbs = false;
                    _isFUDGE = true;
                }

                RefrAb(abProgress);

                OnPropertyChanged(nameof(Is5_5_50));
                OnPropertyChanged(nameof(Is10AbLevels));
                OnPropertyChanged(nameof(IsFUDGE));
                OnPropertyChanged(nameof(IsNoAbs));

            }
        }

        /// <summary>
        /// 5 уровней характеристик, 5 уровней навыков, 50 - макс уровень персонажа.
        /// </summary>
        public bool Is5_5_50
        {
            get
            {
                return _is5_5_50;
            }
            set
            {
                if (value == _is5_5_50) return;

                Dictionary<AbilitiModel, double> abProgress = GetAbProgDict();

                if (value)
                {
                    _is10AbLevels = false;
                    _isFUDGE = false;
                    _isNoAbs = false;
                    _is5_5_50 = true;
                }

                RefrAb(abProgress);

                OnPropertyChanged(nameof(Is5_5_50));
                OnPropertyChanged(nameof(Is10AbLevels));
                OnPropertyChanged(nameof(IsFUDGE));
                OnPropertyChanged(nameof(IsNoAbs));

            }
        }

        /// <summary>
        /// Только 10 уровней в навыках?
        /// </summary>
        public bool Is10AbLevels
        {
            get => _is10AbLevels;
            set
            {
                if (value == _is10AbLevels) return;

                Dictionary<AbilitiModel, double> abProgress = GetAbProgDict();

                if (value)
                {
                    _is5_5_50 = false;
                    _isFUDGE = false;
                    _is10AbLevels = true;
                    _isNoAbs = false;
                }

                RefrAb(abProgress);

                OnPropertyChanged(nameof(Is10AbLevels));
                OnPropertyChanged(nameof(Is5_5_50));
                OnPropertyChanged(nameof(IsFUDGE));
                OnPropertyChanged(nameof(IsNoAbs));

            }
        }

        private void RefrAb(Dictionary<AbilitiModel, double> abProgress)
        {
            foreach (var ab in StaticMetods.PersProperty.Abilitis)
            {
                foreach (var nt in ab.NeedTasks)
                {
                    nt.LevelProperty = 0;
                    nt.ToLevelProperty = MaxAbLev;
                }

                ab.ValueProperty = abProgress[ab] * ab.MaxValue;
            }
        }

        private static Dictionary<AbilitiModel, double> GetAbProgDict()
        {
            Dictionary<AbilitiModel, double> abProgress = new Dictionary<AbilitiModel, double>();

            foreach (var ab in StaticMetods.PersProperty.Abilitis)
            {
                abProgress.Add(ab, ab.ValueProperty / ab.MaxValue);
            }

            return abProgress;
        }

        /// <summary>
        /// Последний уровень скилла сложнее первого в... раз
        /// </summary>
        public int MaxAbLevelMoreThenFirst
        {
            get
            {
                if (_maxAbLevelMoreThenFirst == 0)
                {
                    _maxAbLevelMoreThenFirst = 10;
                }
                return _maxAbLevelMoreThenFirst;
            }
            set
            {
                if (value == _maxAbLevelMoreThenFirst) return;
                _maxAbLevelMoreThenFirst = value;
                OnPropertyChanged(nameof(MaxAbLevelMoreThenFirst));
            }
        }

        /// <summary>
        /// Максимальный уровень характеристик
        /// </summary>
        public int MaxChaLev
        {
            get
            {
                if (Is5_5_50)
                    return 5;

                if (IsFUDGE || IsNoAbs)
                    return 6;

                return 10;
            }
            set
            {

            }
        }

        /// <summary>
        /// Sets and gets Максимальный уровень для характеристик. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int MaxCharactLevelProperty
        {
            get
            {
                return 10;
            }

            set
            {
                return;
                if (this.maxCharactLevel == value)
                {
                    return;
                }

                this.maxCharactLevel = value;
                Settings.Default.MaxChaLevel = value;
                this.OnPropertyChanged(nameof(MaxCharactLevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальный уровень для новых скиллов?. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool MaxLevelForNewAbilitisProperty
        {
            get
            {
                return this.maxLevelForNewAbilitis;
            }

            set
            {
                if (this.maxLevelForNewAbilitis == value)
                {
                    return;
                }

                this.maxLevelForNewAbilitis = value;

                if (value == true)
                {
                    this.NullLevelForNewAbilitisProperty = false;
                }

                this.OnPropertyChanged(nameof(MaxLevelForNewAbilitisProperty));
            }
        }

        /// <summary>
        /// Sets and gets Новым квестам задается минимальный уровень. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool MaxLevelToNewQwestsProperty
        {
            get
            {
                return this.maxLevelToNewQwests;
            }

            set
            {
                if (this.maxLevelToNewQwests == value)
                {
                    return;
                }

                this.maxLevelToNewQwests = value;
                this.OnPropertyChanged(nameof(MaxLevelToNewQwestsProperty));
            }
        }

        /// <summary>
        /// Максимальный уровень скиллов для расчетов
        /// </summary>
        public int MaxLevOfAbForProg => MaxAbLev;

        /// <summary>
        /// Максимальный уровень характеристик для расчетов
        /// </summary>
        public int MaxLevOfChaForProg => MaxChaLev;

        /// <summary>
        /// Для достижения последнего уровня персонажа требуется опыта больше, раз
        /// </summary>
        public int MaxPersLevelMoreThenFirst
        {
            get
            {
                return 30;

                double maxAb = Convert.ToDouble(AbilMaxLevelProperty);
                double maxPoints = Pers.ExpToLevel(AbilMaxLevelProperty, RpgItemsTypes.ability);

                double mp = maxPoints / 100.0;

                //mp = Math.Round(mp / 5.0) * 5.0;

                return Convert.ToInt32(mp);
            }
        }

        /// <summary>
        /// Sets and gets Максимальный уровень для персонажа. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public int MaxPersLevelProperty
        {
            get
            {
                return maxPersLevel;
            }

            set
            {
                if (this.maxPersLevel == value)
                {
                    return;
                }

                this.maxPersLevel = value;
                this.OnPropertyChanged(nameof(MaxPersLevelProperty));
            }
        }

        /// <summary>
        /// Максимальный уровень персонажа (при галочке фиксированный)
        /// </summary>
        public int MaxPLevel
        {
            get
            {
                if (_maxPLevel < StaticMetods.PersProperty.Abilitis.Count + 1) _maxPLevel = StaticMetods.PersProperty.Abilitis.Count + 1;
                if (_maxPLevel < 5) _maxPLevel = 5;
                return _maxPLevel;
            }
            set
            {
                if (value < StaticMetods.PersProperty.Abilitis.Count + 1) value = StaticMetods.PersProperty.Abilitis.Count + 1;
                if (value < 5) value = 5;
                if (value == _maxPLevel) return;
                _maxPLevel = value;
                StaticMetods.PersProperty.RefreshMaxPersLevel();
                OnPropertyChanged(nameof(MaxPLevel));
            }
        }

        /// <summary>
        /// Sets and gets Максимальное влияние квестов на опыт. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int MaxQwestRelayToExpProperty
        {
            get
            {
                if (this.maxQwestRelayToExp == 0)
                {
                    this.maxQwestRelayToExp = 200;
                }

                return this.maxQwestRelayToExp;
            }

            set
            {
                if (this.maxQwestRelayToExp == value)
                {
                    return;
                }

                this.maxQwestRelayToExp = value;
                this.OnPropertyChanged(nameof(MaxQwestRelayToExpProperty));
            }
        }

        /// <summary>
        /// Sets and gets Квесты меньшего уровня должны быть сделаны для того чтобы были активны
        /// квесты бОльшего уровня. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool MinLevelQwestsMustDoneProperty
        {
            get
            {
                return minLevelQwestsMustDone;
            }

            set
            {
                if (minLevelQwestsMustDone == value)
                {
                    return;
                }

                minLevelQwestsMustDone = value;
                OnPropertyChanged(nameof(MinLevelQwestsMustDoneProperty));
            }
        }

        /// <summary>
        /// Sets and gets Минус к значению для привычки. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public double MinusForDefoultForPrivichkaProperty
        {
            get
            {
                return StaticMetods.Config.BuffTaskValue;
                //return minusForDefoultForPrivichka;
            }

            set
            {
                if (minusForDefoultForPrivichka == value)
                {
                    return;
                }

                minusForDefoultForPrivichka = value;
                OnPropertyChanged(nameof(MinusForDefoultForPrivichkaProperty));
            }
        }

        /// <summary>
        /// Sets and gets Минус опыта за час. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int MinusHPPerHourProperty
        {
            get
            {
                if (minusHPPerHour == 0)
                {
                    minusHPPerHour = 10;
                }

                return minusHPPerHour;
            }

            set
            {
                if (minusHPPerHour == value)
                {
                    return;
                }

                minusHPPerHour = value;
                OnPropertyChanged(nameof(MinusHPPerHourProperty));
            }
        }

        /// <summary>
        /// Sets and gets Нормальная вероятность появления награды. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int NormRevardProperty
        {
            get
            {
                return normRevard;
            }

            set
            {
                if (normRevard == value)
                {
                    return;
                }

                normRevard = value;
                OnPropertyChanged(nameof(NormRevardProperty));
            }
        }

        /// <summary>
        /// Sets and gets Нулевой уровень для новых скиллов. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool NullLevelForNewAbilitisProperty
        {
            get
            {
                return this.nullLevelForNewAbilitis;
            }

            set
            {
                if (this.nullLevelForNewAbilitis == value)
                {
                    return;
                }

                this.nullLevelForNewAbilitis = value;

                if (value == true)
                {
                    this.MaxLevelForNewAbilitisProperty = false;
                }

                this.OnPropertyChanged(nameof(NullLevelForNewAbilitisProperty));
            }
        }

        /// <summary>
        /// Sets and gets Количество скиллов которые можно добавить с каждым уровнем. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public int NumActiveAbilitisFromeLevelProperty
        {
            get
            {
                return this.numActiveAbilitisFromeLevel;
            }

            set
            {
                if (this.numActiveAbilitisFromeLevel == value)
                {
                    return;
                }

                this.numActiveAbilitisFromeLevel = value;
                this.OnPropertyChanged(nameof(NumActiveAbilitisFromeLevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Количество колонок с характеристиками в главном окне. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public int NumberChaColumnsMainWindowProperty
        {
            get
            {
                return this.numberChaColumnsMainWindow;
            }

            set
            {
                if (this.numberChaColumnsMainWindow == value)
                {
                    return;
                }

                this.numberChaColumnsMainWindow = value;
                this.OnPropertyChanged(nameof(NumberChaColumnsMainWindowProperty));
            }
        }

        /// <summary>
        /// Sets and gets Вероятность появления очень редкой награды. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public int OchRedcoRevardProperty
        {
            get
            {
                return ochRedcoRevard;
            }

            set
            {
                if (ochRedcoRevard == value)
                {
                    return;
                }

                ochRedcoRevard = value;
                OnPropertyChanged(nameof(OchRedcoRevardProperty));
            }
        }

        /// <summary>
        /// Sets and gets Частое появление награды. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int OftenRewardProperty
        {
            get
            {
                return oftenReward;
            }

            set
            {
                if (oftenReward == value)
                {
                    return;
                }

                oftenReward = value;
                OnPropertyChanged(nameof(OftenRewardProperty));
            }
        }

        /// <summary>
        /// Sets and gets Стоимость одного уровня характеристики. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int OpChaCostProperty
        {
            get
            {
                return opChaCost;
            }

            set
            {
                if (opChaCost == value)
                {
                    return;
                }

                opChaCost = value;
                OnPropertyChanged(nameof(OpChaCostProperty));
            }
        }

        /// <summary>
        /// Sets and gets Очков прокачки за уровень. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int OpFromLevelProperty
        {
            get
            {
                return opFromLevel;
            }

            set
            {
                if (opFromLevel == value)
                {
                    return;
                }

                opFromLevel = value;
                OnPropertyChanged(nameof(OpFromLevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Пароль для защиты. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string PassProperty
        {
            get
            {
                return this.pass;
            }

            set
            {
                if (this.pass == value)
                {
                    return;
                }

                this.pass = value;
                Settings.Default.Pass = this.PassProperty;
                this.OnPropertyChanged(nameof(PassProperty));
            }
        }

        public string PathToDrop
        {
            get
            {
                return Settings.Default.PathToDropBox;
            }
            set
            {
                if (!Directory.Exists(value))
                {
                    return;
                }
                Settings.Default.PathToDropBox = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(PathToDrop));
            }
        }

        /// <summary>
        /// Путь к папке dropbox
        /// </summary>
        public string PathToDropbox { get; set; }

        /// <summary>
        /// Папка для импорта-экспорта с телефона
        /// </summary>
        public string PathToExpImpFolder
        {
            get
            {
                return _pathToExpImpFolder;
            }
            set
            {
                if (value == _pathToExpImpFolder) return;
                _pathToExpImpFolder = value;
                OnPropertyChanged(nameof(PathToExpImpFolder));
            }
        }

        /// <summary>
        /// Путь к программе Graphviz
        /// </summary>
        public string PathToGraphviz
        {
            get
            {
                var toGraphviz = Path.Combine(Directory.GetCurrentDirectory(), "Graphviz", "bin");
                return toGraphviz;
                //return this.pathToGraphviz;
            }

            set
            {
                this.pathToGraphviz = value;
                this.OnPropertyChanged(nameof(PathToGraphviz));
            }
        }

        /// <summary>
        /// Sets and gets Путь к фону карты квестов. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string PathToMapBackgroundProperty
        {
            get
            {
                return this.pathToMapBackground;
            }

            set
            {
                if (this.pathToMapBackground == value)
                {
                    return;
                }

                this.pathToMapBackground = value;
                this.OnPropertyChanged(nameof(PathToMapBackgroundProperty));
            }
        }

        /// <summary>
        /// Sets and gets Путь к мотиваторам после завершения задачи. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public string PathToMotivatorsProperty
        {
            get
            {
                return pathToMotivators;
            }

            set
            {
                if (pathToMotivators == value)
                {
                    return;
                }

                pathToMotivators = value;
                OnPropertyChanged(nameof(PathToMotivatorsProperty));
            }
        }

        public string PathToPers
        {
            get
            {
                return Settings.Default.PathToPers;
            }
            set
            {
                if (!Directory.Exists(value))
                {
                    return;
                }
                Settings.Default.PathToPers = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(PathToPers));
            }
        }

        /// <summary>
        /// Sets and gets Путь к картинке ранга (звездочка по умолчанию). Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public string PathToRangImageProperty
        {
            get
            {
                return this.pathToRangImageProperty;
            }

            set
            {
                if (this.pathToRangImageProperty == value)
                {
                    return;
                }

                this.pathToRangImageProperty = value;
                this.OnPropertyChanged(nameof(PathToRangImageProperty));
            }
        }

        /// <summary>
        /// Sets and gets Плюс ХП за уровень персонажа. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int PlusHPPerLevelProperty
        {
            get
            {
                if (plusHPPerLevel == 0)
                {
                    plusHPPerLevel = 10;
                }

                return plusHPPerLevel;
            }

            set
            {
                if (plusHPPerLevel == value)
                {
                    return;
                }

                plusHPPerLevel = value;
                OnPropertyChanged(nameof(PlusHPPerLevelProperty));
            }
        }

        /// <summary>
        /// Сколько минут в таймере помидоров
        /// </summary>
        public int PomodorroTime
        {
            get
            {
                return 20;
                //return _pomodorroTime;
            }
            set
            {
                if (value == _pomodorroTime) return;
                _pomodorroTime = value;
                OnPropertyChanged(nameof(PomodorroTime));
            }
        }

        /// <summary>
        /// Рекомендуемый максимальный уровень персонажа
        /// </summary>
        public int RecomendedMaxPersLevel
        {
            get
            {
                int easyAbPoints = StaticMetods.PersProperty.Abilitis.Count(n => n.HardnessProperty == 0);
                int normalAbPoints = StaticMetods.PersProperty.Abilitis.Count(n => n.HardnessProperty == 1) * 2;
                int hardAbPoints = StaticMetods.PersProperty.Abilitis.Count(n => n.HardnessProperty == 2) * 3;

                int needAbPoints = easyAbPoints + normalAbPoints + hardAbPoints;

                var needLevel = System.Convert.ToDouble((needAbPoints - this.AbOpsForFirstPersLevelProperty)) / 2.0;
                needLevel = Math.Truncate(needLevel) + 1.0;
                return System.Convert.ToInt32(needLevel);
            }
        }

        /// <summary>
        /// Sets and gets Редкое появление награды. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int RedcoRewardProperty
        {
            get
            {
                return redcoReward;
            }

            set
            {
                if (redcoReward == value)
                {
                    return;
                }

                redcoReward = value;
                OnPropertyChanged(nameof(RedcoRewardProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показывать характеристики в главном окне. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool ShowCharacteristicsInMainWindowProperty
        {
            get
            {
                return this.showCharacteristicsInMainWindow;
            }

            set
            {
                if (this.showCharacteristicsInMainWindow == value)
                {
                    return;
                }

                this.showCharacteristicsInMainWindow = value;
                this.OnPropertyChanged(nameof(ShowCharacteristicsInMainWindowProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показывать повреждения вместо жизней. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool ShowDamageNotHPProperty
        {
            get
            {
                return false;
                //return showDamageNotHP;
            }

            set
            {
                //if (showDamageNotHP == value)
                //{
                //    return;
                //}

                //showDamageNotHP = value;
                //OnPropertyChanged(nameof(ShowDamageNotHPProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показывать имя файла. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool ShowFileSplashNameProperty
        {
            get
            {
                return this.showFileSplashName;
            }

            set
            {
                if (this.showFileSplashName == value)
                {
                    return;
                }

                this.showFileSplashName = value;
                Settings.Default.ShowFileSplashName = value;
                this.OnPropertyChanged(nameof(ShowFileSplashNameProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показывать ссылки между квестами разных уровней. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool ShowLinksWithNotSameLevelsProperty
        {
            get
            {
                return this.showLinksWithNotSameLevels;
            }

            set
            {
                if (this.showLinksWithNotSameLevels == value)
                {
                    return;
                }

                this.showLinksWithNotSameLevels = value;
                this.OnPropertyChanged(nameof(ShowLinksWithNotSameLevelsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показывать в зоне фокусировки не активные квесты. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool ShowNotActiveQwestsInFocusProperty
        {
            get
            {
                return this.showNotActiveQwestsInFocus;
            }

            set
            {
                if (this.showNotActiveQwestsInFocus == value)
                {
                    return;
                }

                this.showNotActiveQwestsInFocus = value;
                this.OnPropertyChanged(nameof(ShowNotActiveQwestsInFocusProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показывать заставку - случайная картинка из папки. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool ShowSplashesFromFolderProperty
        {
            get
            {
                return this.showSplashesFromFolder;
            }

            set
            {
                if (this.showSplashesFromFolder == value)
                {
                    return;
                }

                this.showSplashesFromFolder = value;
                Settings.Default.showUserSplashes = value;
                this.OnPropertyChanged(nameof(ShowSplashesFromFolderProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показывать заставку в начале?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool ShowSplashesProperty
        {
            get
            {
                return this.showSplashes;
            }

            set
            {
                if (this.showSplashes == value)
                {
                    return;
                }

                this.showSplashes = value;

                Settings.Default.showSplashes = this.showSplashes;

                this.OnPropertyChanged(nameof(ShowSplashesProperty));
            }
        }

        /// <summary>
        /// Sets and gets Авто сортировка характеристик. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool SortCharacteristicsProperty
        {
            get
            {
                return this.sortCharacteristics;
            }

            set
            {
                if (this.sortCharacteristics == value)
                {
                    return;
                }

                this.sortCharacteristics = value;
                this.OnPropertyChanged(nameof(SortCharacteristicsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Папка с картинками для заставок. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string SplashesFolderProperty
        {
            get
            {
                return this.splashesFolder;
            }

            set
            {
                if (this.splashesFolder == value)
                {
                    return;
                }

                Settings.Default.userSplashesFolder = value;

                this.splashesFolder = value;
                this.OnPropertyChanged(nameof(SplashesFolderProperty));
            }
        }

        /// <summary>
        /// Sets and gets Сколько раз надо выполнить задачу, чтобы прокачать скилл типа "привычка".
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int TaskCountDefoultPrivichkaProperty
        {
            get
            {
                return taskCountDefoultPrivichka;
            }

            set
            {
                if (taskCountDefoultPrivichka == value)
                {
                    return;
                }

                taskCountDefoultPrivichka = value;
                OnPropertyChanged(nameof(TaskCountDefoultPrivichkaProperty));
                OnPropertyChanged(nameof(HabbitDays));
            }
        }

        /// <summary>
        /// Sets and gets Сколько раз нужно выполнить задачу, чтобы прокачать скилл типа "обучение".
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int TaskCountForEducationProperty
        {
            get
            {
                return taskCountForEducation;
            }

            set
            {
                if (taskCountForEducation == value)
                {
                    return;
                }

                taskCountForEducation = value;
                OnPropertyChanged(nameof(TaskCountForEducationProperty));
            }
        }

        /// <summary>
        /// Sets and gets Для максимальной прокачки скилла задача должна быть выполненна раз, по
        /// умолчанию. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int TaskDoToMaxAbilityProperty
        {
            get
            {
                if (this.taskDoToMaxAbility == 0)
                {
                    this.taskDoToMaxAbility = 20;
                }

                return this.taskDoToMaxAbility;
            }

            set
            {
                if (this.taskDoToMaxAbility == value)
                {
                    return;
                }

                this.taskDoToMaxAbility = value;
                this.OnPropertyChanged(nameof(TaskDoToMaxAbilityProperty));
            }
        }

        /// <summary>
        /// Sets and gets Задачи не влияют на здоровье (выполнение и невыполнение задач). Changes to
        /// that property's value raise the PropertyChanged event.
        /// </summary>
        public bool TasksNotRelayToHPProperty
        {
            get
            {
                return tasksNotRelayToHP;
            }

            set
            {
                if (tasksNotRelayToHP == value)
                {
                    return;
                }

                tasksNotRelayToHP = value;
                OnPropertyChanged(nameof(TasksNotRelayToHPProperty));
            }
        }

        /// <summary>
        /// Sets and gets Время показа заставки в милисекундах. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int TimeShowSplashProperty
        {
            get
            {
                return this.timeShowSplash;
            }

            set
            {
                if (this.timeShowSplash == value)
                {
                    return;
                }

                this.timeShowSplash = value;
                Settings.Default.timeShowSplash = value;
                this.OnPropertyChanged(nameof(TimeShowSplashProperty));
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Ранги характеристик по умолчанию
        /// </summary>
        /// <returns></returns>
        public static List<ChaRangs> getDefoultAbRangse()
        {
            var chaRangse = new List<ChaRangs>();

            if (StaticMetods.PersProperty.PersSettings.IsFUDGE)
            {
                chaRangse = new List<ChaRangs>
                {
                    new ChaRangs() {Name = "Ужасно"},
                    new ChaRangs() {Name = "Плохо"},
                    new ChaRangs() {Name = "Посредственно"},
                    new ChaRangs() {Name = "Нормально"},
                    new ChaRangs() {Name = "Хорошо"},
                    new ChaRangs() {Name = "Отлично"},
                    new ChaRangs() {Name = "Супер"}
                };
            }
            else
            {
                for (int i = 0; i <= StaticMetods.PersProperty.PersSettings.MaxLevOfAbForProg; i++)
                {
                    chaRangse.Add(new ChaRangs() { Name = (i).ToString() });
                }
            }

            return chaRangse;
        }

        /// <summary>
        /// Ранги характеристик по умолчанию
        /// </summary>
        /// <returns></returns>
        public static List<ChaRangs> getDefoultChaRangse()
        {
            var chaRangse = new List<ChaRangs>();

            if (StaticMetods.PersProperty.PersSettings.IsFUDGE || StaticMetods.PersProperty.PersSettings.IsNoAbs)
            {
                chaRangse = new List<ChaRangs>
                {
                    new ChaRangs() {Name = "Ужасно"},
                    new ChaRangs() {Name = "Плохо"},
                    new ChaRangs() {Name = "Посредственно"},
                    new ChaRangs() {Name = "Нормально"},
                    new ChaRangs() {Name = "Хорошо"},
                    new ChaRangs() {Name = "Отлично"},
                    new ChaRangs() {Name = "Супер"}
                };
            }
            else
            {
                for (int i = 0; i <= StaticMetods.PersProperty.PersSettings.MaxLevOfChaForProg; i++)
                {
                    chaRangse.Add(new ChaRangs() { Name = (i).ToString() });
                }
            }

            return chaRangse;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private static void RefreshPointsAndRangse()
        {
            StaticMetods.PersProperty.RecountPlusAbPoints();
            StaticMetods.PersProperty.RecountRangLevels();
        }

        /// <summary>
        /// Проверка рангов скиллов
        /// </summary>
        private void checkAbRangse(bool propChanged)
        {
            if (_abRangs == null || _abRangs.Count != MaxAbLev + 1)
            {
                _abRangs = getDefoultAbRangse();
                if (propChanged)
                {
                    OnPropertyChanged(nameof(AbRangs));
                }
            }
        }

        /// <summary>
        /// Проверка рангов характеристик
        /// </summary>
        private void CheckChaRangse(bool propChange)
        {
            if (_characteristicRangs == null || _characteristicRangs.Count != MaxChaLev + 1)
            {
                _characteristicRangs = getDefoultChaRangse();
                if (propChange)
                {
                    OnPropertyChanged(nameof(CharacteristicRangs));
                }
            }
        }

        /// <summary>
        /// Когда количество уровней характеристик или скиллов меняется - корректируем прочие элементы
        /// </summary>
        private void CorrectValuesWhenRangseChanged()
        {
            checkAbRangse(true);
            CheckChaRangse(true);
            // Корректируем значения характеристик
            foreach (var characteristic in StaticMetods.PersProperty.Characteristics)
            {
                if (characteristic.FirstVal > MaxLevOfAbForProg) characteristic.FirstVal = MaxLevOfAbForProg;
                characteristic.RecountChaValue();
            }

            // Корректируем скиллы
            foreach (var abilitiModel in StaticMetods.PersProperty.Abilitis)
            {
                abilitiModel.CheckNeedsForLevels();

                // Меняем требования в скиллах
                foreach (var abiliti in StaticMetods.PersProperty.Abilitis)
                {
                    // Удаляем левелс вен ап!
                    var count = abiliti.LevelsWhenUp.Count;
                    var l = MaxLevOfAbForProg + 1;
                    if (count > l)
                    {
                        for (int i = 0; i <= count - l; i++)
                        {
                            abiliti.LevelsWhenUp.Remove(abiliti.LevelsWhenUp.Last());
                        }
                    }

                    // Требования других скиллов
                    foreach (var needAbility in abiliti.NeedAbilities.Where(n => n.ValueProperty > MaxLevOfAbForProg).ToList())
                    {
                        needAbility.ValueProperty = MaxLevOfAbForProg;
                    }

                    // Требования характеристик
                    foreach (
                        NeedCharact needCharact in
                            abiliti.NeedCharacts.Where(n => n.ValueProperty > MaxLevOfChaForProg).ToList())
                    {
                        needCharact.ValueProperty = MaxLevOfChaForProg;
                    }
                }

                // Удаляем задачи скиллов, которые превышают макс уровень
                foreach (NeedTasks needTasks in abilitiModel.NeedTasks.Where(n => n.LevelProperty > MaxLevOfAbForProg).ToList())
                {
                    needTasks.TaskProperty.Delete(StaticMetods.PersProperty);
                }
            }

            // Меняем требования в наградах
            foreach (var shopItem in StaticMetods.PersProperty.ShopItems)
            {
                // Требования других скиллов
                foreach (var needAbility in shopItem.AbilityNeeds.Where(n => n.ValueProperty > MaxLevOfAbForProg).ToList())
                {
                    needAbility.ValueProperty = MaxLevOfAbForProg;
                }

                // Требования характеристик
                foreach (
                    NeedCharact needCharact in shopItem.NeedCharacts.Where(n => n.ValueProperty > MaxLevOfChaForProg).ToList())
                {
                    needCharact.ValueProperty = MaxLevOfChaForProg;
                }
            }

            // Меняем требования в инвентаре
            foreach (var shopItem in StaticMetods.PersProperty.InventoryItems)
            {
                // Требования других скиллов
                foreach (var needAbility in shopItem.AbilityNeeds.Where(n => n.ValueProperty > MaxLevOfAbForProg).ToList())
                {
                    needAbility.ValueProperty = MaxLevOfAbForProg;
                }

                // Требования характеристик
                foreach (
                    NeedCharact needCharact in shopItem.NeedCharacts.Where(n => n.ValueProperty > MaxLevOfChaForProg).ToList())
                {
                    needCharact.ValueProperty = MaxLevOfChaForProg;
                }
            }

            OnPropertyChanged(nameof(AbPointsForOneLevel));
        }

        #endregion Private Methods

        #region Public Events

        /// <summary>
        /// The property changed.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Private Fields

        private int _abPointsForEasyQwest;

        private int _abPointsForFirstLevel;

        private int _abPointsForHardQwest;

        private int _abPointsForLevel;

        private int _abPointsForNormalQwest;

        private double _abPointsForOneLevel;
        private bool _abPointsForQwests;

        private int _abPointsForVeryHardQwest;

        private List<ChaRangs> _abRangs;

        private bool _autoCountAbPointsForLev;
        private List<ChaRangs> _characteristicRangs;

        private int _columnsOfCharacteristicsInMain;
        private int _damageFromeTask = 3;

        private int _damageFromHabbit = 10;

        private TypeOfTask _defoultTaskTypeForAbills;

        private TypeOfTask _defoultTaskTypeForQwests;

        private bool _disableSounds;

        private double _globalBooster;
        private bool _is100Levels;
        private bool _isActtivateRangse;

        private bool _isCharactBalanse;

        private bool _isDamageNotHp;

        private bool _isDynamicExpCount;

        private bool _isFixMaxPLevel;

        private bool _isGhostBastersMode;

        private bool _isGoldEnabled;
        private bool _isHideAbTasksOneByOne;

        private bool _isHideNotSellectedLink;

        private bool _isHideQwestsFromAbLink;

        private bool _isHideTasksOneByOneAndroid;

        private bool _isHp;

        private bool _isNotShowCharacteristicsInMain;

        private bool _isNotShowNotifications;

        /// <summary>
        /// Короткие сообщения если задача или квест выполненны.
        /// </summary>
        private bool _isShortMesWhenTaskOrQwestDone;

        private bool _isShowTasksByTime;

        private bool _isShowTasksByTimeAndroid;

        private bool _isSmartTaskSort;

        private bool _isTesExp;

        private double _kPersLevel;

        private int _maxAbLev;

        private int _maxAbLevelMoreThenFirst;

        private int _maxChaLev;

        private int _maxPersLevelMoreThenFirst;

        private int _maxPLevel;

        private string _pathToExpImpFolder;

        private int _pomodorroTime;

        private List<Tuple<int, string>> _waveNames;

        /// <summary>
        /// Число колонок в окне настройки скиллов.
        /// </summary>
        private int abColumnsInAvView;

        /// <summary>
        /// Максимальный уровень для скиллов.
        /// </summary>
        private int abilMaxLevel;

        /// <summary>
        /// Ранги для скиллов по умолчанию.
        /// </summary>
        private ObservableCollection<Rangs> abilRangsForDefoult;

        /// <summary>
        /// Очков прокачки персонажа для его первого уровня.
        /// </summary>
        private int AbOpsForFirstPersLevel;

        /// <summary>
        /// Активный вид задач персонажа.
        /// </summary>
        private int activePersView;

        /// <summary>
        /// Число колонок характеристик.
        /// </summary>
        private int chaColumnsInChView;

        /// <summary>
        /// Ранги для характеристик по умолчанию.
        /// </summary>
        private ObservableCollection<Rangs> charactRangsForDefoult;

        /// <summary>
        /// Цвет рамки для задач по умолчанию.
        /// </summary>
        private string colorTaskBorder;

        /// <summary>
        /// Число колонок в альтернативном виде.
        /// </summary>
        private int columnsInAlternateView;

        /// <summary>
        /// Количество колонок задач в главном окне.
        /// </summary>
        private int columnsTasksInMainView;

        /// <summary>
        /// Делить карту квестов на разные уровни.
        /// </summary>
        private bool divideByLevels = true;

        /// <summary>
        /// Активировать потребности.
        /// </summary>
        private bool enableNeedness;

        /// <summary>
        /// Активировать очки прокачки?.
        /// </summary>
        private bool enableOP;

        /// <summary>
        /// Опыта за сложную задачу.
        /// </summary>
        private int expForHardTask;

        /// <summary>
        /// Опыта за нормальную задачу.
        /// </summary>
        private int expForNormalTask;

        /// <summary>
        /// Опыта за очень сложную задачу.
        /// </summary>
        private int expForVeryHardTask;

        /// <summary>
        /// Опыт также за выполнение задач и квестов.
        /// </summary>
        private bool expFromTasksAndQwests;

        /// <summary>
        /// Количество строк в фокусе.
        /// </summary>
        private int FocusRows;

        /// <summary>
        /// Золото за сложную задачу.
        /// </summary>
        private int goldForHardTask;

        /// <summary>
        /// Золото за выполнение нормальной задачи.
        /// </summary>
        private int goldForNormalTask;

        /// <summary>
        /// Золото за очень сложную задачу.
        /// </summary>
        private int goldForVeryHardTask;

        /// <summary>
        /// Скрывать зону фокусировки.
        /// </summary>
        private bool hideFocusField;

        /// <summary>
        /// Скрывать изображения во 2-м виде задач.
        /// </summary>
        private bool hideImag2View;

        /// <summary>
        /// Скрывать неактивные скиллы.
        /// </summary>
        private bool hideNotActiveAbilitid = false;

        /// <summary>
        /// Скрывать неактивные скиллы в дереве.
        /// </summary>
        private bool hideNotActiveAbilitisInTree;

        /// <summary>
        /// Скрывать неактивные перки?.
        /// </summary>
        private bool hideNotActivPerks;

        /// <summary>
        /// Скрывать не доступные скиллы в дереве.
        /// </summary>
        private bool hideNotAllowAbilitisInTree;

        /// <summary>
        /// Скрывать квесты без активных задач из фокусировки.
        /// </summary>
        private bool hideQwestsWithoutTasksInFocus = true;

        /// <summary>
        /// Режим Героев меча и магии, нужно нажать кнопку для завершения хода.
        /// </summary>
        private bool HOMMMode;

        /// <summary>
        /// Минут до снижения ХП до нуля.
        /// </summary>
        private int hourseToNullHP;

        /// <summary>
        /// Группы скиллов это характеристики.
        /// </summary>
        private bool isAbGroupsIsCharacteristics;

        /// <summary>
        /// Автоначало активности скиллов по умолчанию?.
        /// </summary>
        private bool isAbilAutoStart;

        /// <summary>
        /// Активировать очки скиллов?.
        /// </summary>
        private bool isAbPointsActive;

        /// <summary>
        /// Активированы элементы автофокуса?.
        /// </summary>
        private bool isAutofocusEnabled;

        /// <summary>
        /// Можно нажать плюс только если все подзадачи выполненны.
        /// </summary>
        private bool isCanDownPlusOnliIfSubtasksDone = false;

        /// <summary>
        /// Активированна ли автофокусировка для характеристик.
        /// </summary>
        private bool isChaAvtofocusEnabled;

        /// <summary>
        /// Характеристики делятся на группы.
        /// </summary>
        private bool isChaGroup;

        /// <summary>
        /// Активированна ли картинка с характеристиками в главном окне?.
        /// </summary>
        private bool isChaPicMainWindowEnabled;

        /// <summary>
        /// Альтернативное отображение задач активировано?.
        /// </summary>
        private bool isEnabledAlterneteTaskView;

        /// <summary>
        /// Активен первый вид отображения задач.
        /// </summary>
        private bool isEnabledFirstView;

        /// <summary>
        /// Фокусировка для новых скиллов по умолчанию.
        /// </summary>
        private bool isFocusForDefoultForNewAbility;

        /// <summary>
        /// Активирован 4 вид.
        /// </summary>
        private bool isFourViewEnabled;

        /// <summary>
        /// Режим FUDGE активирован.
        /// </summary>
        private bool isFudgeMode;

        /// <summary>
        /// Можно покупать сложные скиллы только после приобретения простых.
        /// </summary>
        private bool isHardAbAfterEasy;

        /// <summary>
        /// Скрывать характеристики в главном окне?.
        /// </summary>
        private bool isHideCharactsInMainWindow;

        /// <summary>
        /// Скрывать класс персонажа.
        /// </summary>
        private bool isHideClass;

        /// <summary>
        /// Скрывать дневник?.
        /// </summary>
        private bool isHideDiary;

        /// <summary>
        /// Скрывать картинки в скиллах.
        /// </summary>
        private bool isHideImagePropertysInChanges;

        /// <summary>
        /// Скрывать рассу.
        /// </summary>
        private bool isHideRase;

        /// <summary>
        /// Активировать здоровье?.
        /// </summary>
        private bool isHPActivete;

        /// <summary>
        /// Хп снижаются по таймеру.
        /// </summary>
        private bool isHpMinusForTimer;

        /// <summary>
        /// При здоровье = 0 опыт отнимается до предыдущего уровня?.
        /// </summary>
        private bool isMinusExpHP;

        /// <summary>
        /// Сразу открывать карту квестов из главного окна.
        /// </summary>
        private bool isOpenQwestMap;

        /// <summary>
        /// Включить защиту паролем?.
        /// </summary>
        private bool isPassProtect = false;

        /// <summary>
        /// Прогрессбары вместо звездочек.
        /// </summary>
        private bool isProgressWhithoutStars;

        /// <summary>
        /// Автоактивность квестов по умолчанию.
        /// </summary>
        private bool isQwestAutoActiveForDefoult;

        /// <summary>
        /// Группы квестов равно характеристики.
        /// </summary>
        private bool isQwestGroupsIsCharacteristic;

        /// <summary>
        /// Активированна ли автоматическая фокусировка для квестов?.
        /// </summary>
        private bool isQwestsAvtofocusEnabled;

        /// <summary>
        /// Квесты делятся на группы.
        /// </summary>
        private bool isQwestsGroup;

        /// <summary>
        /// Показывать название группы скиллов.
        /// </summary>
        private bool isShowAbGroupName;

        /// <summary>
        /// Показывать мотиватор во время завершения задачи.
        /// </summary>
        private bool isTaskDoneMotivatorShow;

        /// <summary>
        /// Если здоровье меньше 25% функции программы становятся ограниченными?.
        /// </summary>
        private bool isTerribleDebufHP;

        /// <summary>
        /// Третий вид активирован.
        /// </summary>
        private bool isThreedViewActive = false;

        /// <summary>
        /// Коэффициент влияния скиллов и характеристик на опыт.
        /// </summary>
        private double kAbilChaExpRelay;

        /// <summary>
        /// Коэффициент влияния скиллов на опыт.
        /// </summary>
        private double kAbToExpRelay;

        /// <summary>
        /// Уровней в звездочке.
        /// </summary>
        private int levelsInStar;

        /// <summary>
        /// Максимальный уровень для характеристик.
        /// </summary>
        private int maxCharactLevel;

        /// <summary>
        /// Максимальный уровень для новых скиллов?.
        /// </summary>
        private bool maxLevelForNewAbilitis;

        /// <summary>
        /// Новым квестам задается минимальный уровень.
        /// </summary>
        private bool maxLevelToNewQwests = false;

        /// <summary>
        /// Максимальный уровень для персонажа.
        /// </summary>
        private int maxPersLevel;

        /// <summary>
        /// Максимальное влияние квестов на опыт.
        /// </summary>
        private int maxQwestRelayToExp;

        /// <summary>
        /// Квесты меньшего уровня должны быть сделаны для того чтобы были активны квесты бОльшего уровня.
        /// </summary>
        private bool minLevelQwestsMustDone;

        /// <summary>
        /// Минус к значению для привычки.
        /// </summary>
        private double minusForDefoultForPrivichka;

        /// <summary>
        /// Минус опыта за час.
        /// </summary>
        private int minusHPPerHour;

        /// <summary>
        /// Нормальная вероятность появления награды.
        /// </summary>
        private int normRevard = 9;

        /// <summary>
        /// Нулевой уровень для новых скиллов.
        /// </summary>
        private bool nullLevelForNewAbilitis;

        /// <summary>
        /// Количество скиллов которые можно добавить с каждым уровнем.
        /// </summary>
        private int numActiveAbilitisFromeLevel;

        /// <summary>
        /// Количество колонок с характеристиками в главном окне.
        /// </summary>
        private int numberChaColumnsMainWindow = 1;

        /// <summary>
        /// Вероятность появления очень редкой награды.
        /// </summary>
        private int ochRedcoRevard = 3;

        /// <summary>
        /// Частое появление награды.
        /// </summary>
        private int oftenReward = 15;

        /// <summary>
        /// Стоимость одного уровня характеристики.
        /// </summary>
        private int opChaCost;

        /// <summary>
        /// Очков прокачки за уровень.
        /// </summary>
        private int opFromLevel;

        /// <summary>
        /// Пароль для защиты.
        /// </summary>
        private string pass;

        /// <summary>
        /// Путь к программе Graphviz
        /// </summary>
        private string pathToGraphviz = @"C:\Program Files (x86)\Graphviz2.38\bin";

        /// <summary>
        /// Путь к фону карты квестов.
        /// </summary>
        private string pathToMapBackground = Path.Combine(Directory.GetCurrentDirectory(), "Images", "map.jpg");

        /// <summary>
        /// Путь к мотиваторам после завершения задачи.
        /// </summary>
        private string pathToMotivators;

        /// <summary>
        /// Путь к картинке ранга (звездочка по умолчанию).
        /// </summary>
        private string pathToRangImageProperty;

        /// <summary>
        /// Плюс ХП за уровень персонажа.
        /// </summary>
        private int plusHPPerLevel;

        /// <summary>
        /// Редкое появление награды.
        /// </summary>
        private int redcoReward = 5;

        /// <summary>
        /// Показывать характеристики в главном окне.
        /// </summary>
        private bool showCharacteristicsInMainWindow;

        /// <summary>
        /// Показывать повреждения вместо жизней.
        /// </summary>
        private bool showDamageNotHP;

        /// <summary>
        /// Показывать имя файла.
        /// </summary>
        private bool showFileSplashName;

        /// <summary>
        /// Показывать ссылки между квестами разных уровней.
        /// </summary>
        private bool showLinksWithNotSameLevels = false;

        /// <summary>
        /// Показывать в зоне фокусировки не активные квесты.
        /// </summary>
        private bool showNotActiveQwestsInFocus;

        /// <summary>
        /// Показывать заставку в начале?.
        /// </summary>
        private bool showSplashes;

        /// <summary>
        /// Показывать заставку - случайная картинка из папки.
        /// </summary>
        private bool showSplashesFromFolder;

        /// <summary>
        /// Авто сортировка характеристик.
        /// </summary>
        private bool sortCharacteristics;

        /// <summary>
        /// Папка с картинками для заставок.
        /// </summary>
        private string splashesFolder;

        /// <summary>
        /// Сколько раз надо выполнить задачу, чтобы прокачать скилл типа "привычка".
        /// </summary>
        private int taskCountDefoultPrivichka;

        /// <summary>
        /// Сколько раз нужно выполнить задачу, чтобы прокачать скилл типа "обучение".
        /// </summary>
        private int taskCountForEducation;

        /// <summary>
        /// Для максимальной прокачки скилла задача должна быть выполненна раз, по умолчанию.
        /// </summary>
        private int taskDoToMaxAbility;

        /// <summary>
        /// Задачи не влияют на здоровье (выполнение и невыполнение задач).
        /// </summary>
        private bool tasksNotRelayToHP;

        /// <summary>
        /// Время показа заставки в милисекундах.
        /// </summary>
        private int timeShowSplash;

        private bool _is10AbLevels;
        private bool _is5_5_50;
        private bool _isFUDGE;
        private bool _isNoAbs;

        /// <summary>
        /// Колонок характеристик в главном окне.
        /// </summary>
        public int ColumnsOfCharacteristicsInMain
        {
            get => _columnsOfCharacteristicsInMain; set
            {
                if (_columnsOfCharacteristicsInMain == value)
                    return;

                _columnsOfCharacteristicsInMain = value;

                OnPropertyChanged(nameof(ColumnsOfCharacteristicsInMain));
            }
        }

        /// <summary>
        /// Не показывать характеристики в главном окне.
        /// </summary>
        public bool IsNotShowCharacteristicsInMain
        {
            get => _isNotShowCharacteristicsInMain; set
            {
                if (_isNotShowCharacteristicsInMain == value)
                    return;

                _isNotShowCharacteristicsInMain = value;

                OnPropertyChanged(nameof(IsNotShowCharacteristicsInMain));
            }
        }

        /// <summary>
        /// Не показывать оповещения изменения опыта и навыков. Только характеристик и уровня.
        /// </summary>
        public bool IsNotShowNotifications
        {
            get => _isNotShowNotifications;
            set
            {
                if (_isNotShowNotifications == value)
                    return;

                _isNotShowNotifications = value;

                OnPropertyChanged(nameof(IsNotShowNotifications));
            }
        }

        #endregion Private Fields
    }
}
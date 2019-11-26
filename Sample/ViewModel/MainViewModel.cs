using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommonMark;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Sample.Model;
using Sample.Properties;
using Sample.View;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Sample.ViewModel
{
    public class ElementForInterface
    {
        /// <summary>
        /// Изображение
        /// </summary>
        public BitmapImage PictureProperty { get; set; }

        /// <summary>
        /// Задача
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// Подсказка
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// Ид.
        /// </summary>
        public string Guid { get; set; }
    }

    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The exp after.
        /// </summary>
        public static int expAfter;

        /// <summary>
        /// The exp before.
        /// </summary>
        public static int expBefore;

        /// <summary>
        /// The rnd.
        /// </summary>
        public static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// The опыта доп ервогоуровня.
        /// </summary>
        public static int опытаДоПервогоУровня = 10;

        //new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), NumberStyles.HexNumber));
        public List<FocusModel> _activeQwestsWithTasks;

        public RelayCommand beginNewGameCommand;

        public RelayCommand focModeEnabledDisabledCommand;

        public RelayCommand helpForProjectCommand;

        public RelayCommand nextFocViewTasksCommand;

        public RelayCommand openAllTasksCommand;

        public RelayCommand openPlaningWindow;

        public RelayCommand prevFocViewTasksCommand;

        public RelayCommand setPlaningModeCommand;

        public RelayCommand sortByPriorityCommand;

        private static DateTime _selectedTime;

        private static double s_sizeDivider;

        /// <summary>
        /// The timer hp.
        /// </summary>
        private readonly Timer timerHP = new Timer();

        /// <summary>
        /// The app folder.
        /// </summary>
        //private readonly string appFolder = Path.Combine(
        //    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        //    Resources.NameOfTheGame);
        /// <summary>
        /// The timer hp.
        /// </summary>
        private readonly Timer timerNeedness = new Timer();

        private List<FocusModel> _activeAbilitys;

        private List<FocusModel> _activeQwests;

        /// <summary>
        /// Добавить новую задачу.
        /// </summary>
        private RelayCommand _addNewTaskCommand;

        private List<FocusModel> _allSpells;

        private int _colsInPlaning;

        /// <summary>
        /// Комманда Конец хода.
        /// </summary>
        private RelayCommand _endOfTurnCommand;

        /// <summary>
        /// Экспортировать информацию для ВИКИ.
        /// </summary>
        private RelayCommand _exportToWikiCommand;

        private FocusModel _firstFocus;

        private bool _interfaceActiveGroupsVisibillity;

        private bool _isFocMode;

        private Visibility _isFocTaksVisibility = Visibility.Collapsed;

        private string _isMoreHack;

        private bool _isPlaningMode;

        private Task _lastParTask;

        /// <summary>
        /// Комманда Загрузить пустой персонаж.
        /// </summary>
        private RelayCommand _loadClearPersCommand;

        /// <summary>
        /// Комманда Загрузить обучающую компанию.
        /// </summary>
        private RelayCommand _loadLearningTourCommand;

        /// <summary>
        /// Комманда открыть дневник.
        /// </summary>
        private RelayCommand _openDiary;

        private Task _prevLastParTask;

        /// <summary>
        /// Gets the Клик по элементу из зоны фокусировки.
        /// </summary>
        private RelayCommand<string> _selectFocusCommand;

        private FocusModel _selFoc;

        /// <summary>
        /// Gets the Показать характеристику.
        /// </summary>
        private RelayCommand<Characteristic> _showCharactCommand;

        private int _tasksCount;

        private List<Task> _tasksForFoc;

        /// <summary>
        /// Число колонок в униформгриде задач.
        /// </summary>
        private int _tasksNumOfColumn;

        /// <summary>
        /// Добавление новой задачи.
        /// </summary>
        private RelayCommand<TypeOfTask> addNewTaskCommand;

        /// <summary>
        /// Gets the Редактирование задачи из альтернативного режима.
        /// </summary>
        private RelayCommand<Task> alterEditTaskCommand;

        /// <summary>
        /// Комманда альтернативное добавление задачи.
        /// </summary>
        private RelayCommand alternateAddTaskCommand;

        /// <summary>
        /// Gets the Альтернативное "Задача не сделана".
        /// </summary>
        private RelayCommand<Task> alternateMinusTaskCommand;

        /// <summary>
        /// Gets the Альтернативно сдвинуть задачу вниз.
        /// </summary>
        private RelayCommand<Task> alternateMoveDownCommand;

        /// <summary>
        /// Gets the Альтернативный сдвиг задачи вверх.
        /// </summary>
        private RelayCommand<Task> alternateMoveUpCommand;

        /// <summary>
        /// Gets the Альтернативное сделание задачи.
        /// </summary>
        private RelayCommand<object> alternatePlusTaskCommand;

        /// <summary>
        /// Gets the Удаление задачи из альтернативного режима.
        /// </summary>
        private RelayCommand<Task> alternateRemoveTaskCommand;

        /// <summary>
        /// Комманда Анонимный экспорт персонажа.
        /// </summary>
        private RelayCommand anonimExportCommand;

        /// <summary>
        /// Gets the Клик по счетчику.
        /// </summary>
        private RelayCommand<Task> clickCounterCommand;

        /// <summary>
        /// Gets the Клик минус в задаче (обычный режим).
        /// </summary>
        private RelayCommand<Task> clickMinusCommand;

        /// <summary>
        /// Gets the Комманда клик плюс в задаче.
        /// </summary>
        private RelayCommand<Task> clickPlusCommand;

        /// <summary>
        /// Комманда Закрыть вид - фокусировка на квесте или скилле задачи.
        /// </summary>
        private RelayCommand closeFocCommand;

        /// <summary>
        /// Скопированная задача.
        /// </summary>
        private Task copiedTask;

        /// <summary>
        /// Gets the Копировать задачу или перенести.
        /// </summary>
        private RelayCommand<string> cutOrCopyTaskCommand;

        /// <summary>
        /// Gets the Удалить задачу.
        /// </summary>
        private RelayCommand<TypeOfTask> deleteTaskCommand;

        /// <summary>
        /// Видимомть редактирования задачи.
        /// </summary>
        private Visibility editVisibility = Visibility.Visible;

        /// <summary>
        /// Gets the Скрыть/показать шапку персонажа.
        /// </summary>
        private RelayCommand<string> expandShrinkHUDCommand;

        /// <summary>
        /// Комманда Экспорт для андроид.
        /// </summary>
        private RelayCommand exportToAndroidCommand;

        /// <summary>
        /// Комманда Все задачи во втором виде перенести во конец списка.
        /// </summary>
        private RelayCommand focusViewAllToEndCommand;

        /// <summary>
        /// Комманда В виде фокусировки перенос всех задач в начало.
        /// </summary>
        private RelayCommand fovusedViewAllToBeginCommand;

        /// <summary>
        /// Gets the Команда - перейти на веб сайт по ссылке.
        /// </summary>
        private RelayCommand<string> goToURLCommand;

        /// <summary>
        /// Комманда Импортировать из андроида.
        /// </summary>
        private RelayCommand importFromAndroidCommand;

        /// <summary>
        /// Задача вырезана?.
        /// </summary>
        private bool isCutTask;

        /// <summary>
        /// Открыть добавление или редактирование?.
        /// </summary>
        private bool isEditOrAddOpen;

        /// <summary>
        /// Активирован режим переноса?.
        /// </summary>
        private bool isMoveOrCopyEnabled;

        /// <summary>
        /// Открыть окно показать изменения?.
        /// </summary>
        private bool isViewChangesOpenProperty;

        /// <summary>
        /// Комманда Комманда - начать с начала.
        /// </summary>
        private RelayCommand letSBeginCommand;

        /// <summary>
        /// Загрузить продвинутый шаблон.
        /// </summary>
        private RelayCommand loadAdvansedTemplateCommand;

        /// <summary>
        /// Комманда Загрузить обучающую компанию.
        /// </summary>
        private RelayCommand loadLearningCompanyCommand;

        /// <summary>
        /// Загрузить простую версию шаблона.
        /// </summary>
        private RelayCommand loadSampleTemplateCommand;

        /// <summary>
        /// Минуты таймера для снижения ХП.
        /// </summary>
        private int minutes;

        /// <summary>
        /// Сдвинуть квест из третьего вида вниз.
        /// </summary>
        private RelayCommand<FocusModel> moveQwestDownCommand;

        /// <summary>
        /// Сдвинуть квест вверх.
        /// </summary>
        private RelayCommand<FocusModel> moveQwestUpCommand;

        /// <summary>
        /// Gets the Сдвинуть задачу в начало списка.
        /// </summary>
        private RelayCommand<Task> moveTaskToBeginOfListCommand;

        /// <summary>
        /// Gets the Сдвинуть задачу в конец списка.
        /// </summary>
        private RelayCommand<Task> moveTaskToEndOfListCommand;

        /// <summary>
        /// Gets the Сдвинуть волну задачи влево.
        /// </summary>
        private RelayCommand<Task> moveTaskWaveLeftCommand;

        /// <summary>
        /// Gets the Сдвинуть волну задачи вправо.
        /// </summary>
        private RelayCommand<Task> moveTaskWaveRightCommand;

        /// <summary>
        /// Gets the Сдвинуть вид для третьего отображения задач.
        /// </summary>
        private RelayCommand<string> moveViewCommand;

        /// <summary>
        /// Gets the Сдвинуть волну влево.
        /// </summary>
        private RelayCommand<int> moveWaveLeftCommand;

        /// <summary>
        /// Gets the Сдвинуть волну вправо.
        /// </summary>
        private RelayCommand<int> moveWaveRightCommand;

        /// <summary>
        /// Комманда Начать новую игру.
        /// </summary>
        private RelayCommand newGameCommand;

        /// <summary>
        /// Комманда Открыть общие настройки.
        /// </summary>
        private RelayCommand openAllSettingsCommand;

        /// <summary>
        /// Открыть окно Автофокус!!!.
        /// </summary>
        private RelayCommand openAutofocusCommand;

        /// <summary>
        /// Комманда Открыть первый квест, связанный с задачей.
        /// </summary>
        private RelayCommand openFirstLinkQwestCommand;

        /// <summary>
        /// Комманда Зайти вглубь задачи и открыть самый первый элемент фокусировки.
        /// </summary>
        private RelayCommand openFirstLinkTaskCommand;

        /// <summary>
        /// Gets the Открыть связанный скилл.
        /// </summary>
        private RelayCommand<TaskRelaysItem> openLinkedAbilityCommand;

        /// <summary>
        /// Gets the Открыть элемент для редактирования.
        /// </summary>
        private RelayCommand<object> openLinkElementForEditCommand;

        /// <summary>
        /// Gets the Открыть элемент с которым связана задача.
        /// </summary>
        private RelayCommand<object> openLinksThisTaskCommand;

        /// <summary>
        /// Открыть лог.
        /// </summary>
        private RelayCommand openLogCommand;

        /// <summary>
        /// Открыть завершенные задачи.
        /// </summary>
        private RelayCommand openLogWindowCommand;

        /// <summary>
        /// Комманда SUMMARY.
        /// </summary>
        private RelayCommand openNewPersWizzardCommand;

        /// <summary>
        /// Gets the Открыть активные задачи квеста для 3-го вида.
        /// </summary>
        private RelayCommand<Aim> openQwestTasksCommand;

        /// <summary>
        /// Gets the Открыть быстрый вызов свойств персонажа.
        /// </summary>
        private RelayCommand<string> openQwickButtonCommand;

        /// <summary>
        /// Комманда Открыть статистику.
        /// </summary>
        private RelayCommand openStatisticCommand;

        /// <summary>
        /// Комманда Открыть карту задач.
        /// </summary>
        private RelayCommand openTasksMapCommand;

        /// <summary>
        /// Комманда Вставить задачу.
        /// </summary>
        private RelayCommand pasteTaskCommand;

        /// <summary>
        /// Путь к шаблону по умолчанию
        /// </summary>
        private string pathToTemplate = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Templates", "demo");

        /// <summary>
        /// Gets the Быстро задать дату.
        /// </summary>
        private RelayCommand<string> qwickSetDateCommand;

        /// <summary>
        /// Gets the Быстрый выбор вида.
        /// </summary>
        private RelayCommand<string> qwickSetViewCommand;

        /// <summary>
        /// Комманда Экспорт для андроид.
        /// </summary>
        private RelayCommand refreshSubTasksCommand;

        private DispatcherTimer SaveTimer = new DispatcherTimer { Interval = StaticMetods.timeSpan };

        /// <summary>
        /// The selected task type.
        /// </summary>
        private TypeOfTask selectedTaskType;

        /// <summary>
        /// The sellected task.
        /// </summary>
        private Task sellectedTask;

        /// <summary>
        /// Gets the Перенести задачу на завтра.
        /// </summary>
        private RelayCommand<Task> sendTaskToTomorowCommand;

        /// <summary>
        /// Gets the Быстрая установка даты (вчера, сегодня, завтра).
        /// </summary>
        private RelayCommand<string> setDateCommand;

        /// <summary>
        /// Gets the Задать последнюю родительскую задача.
        /// </summary>
        private RelayCommand<Task> setLastParCommand;

        /// <summary>
        /// Gets the Задать пресеты.
        /// </summary>
        private RelayCommand<string> setPresetCommand;

        /// <summary>
        /// Gets the Задать по клику выделенную задачу.
        /// </summary>
        private RelayCommand<Task> setSellectedTaskCommand;

        /// <summary>
        /// Комманда Показать счетчики.
        /// </summary>
        private RelayCommand showCountersCommand;

        /// <summary>
        /// Показать экран приветствия.
        /// </summary>
        private RelayCommand showGreetingsCommand;

        /// <summary>
        /// Комманда Комманда - начать с начала.
        /// </summary>
        private RelayCommand softBeginCommand;

        /// <summary>
        /// Видимость контекстного меню для задач..
        /// </summary>
        private bool taskContextVisible;

        /// <summary>
        /// Создать квест на основе задачи.
        /// </summary>
        private RelayCommand<Task> taskToQwestCommand;

        /// <summary>
        /// Gets the Пауза в таймере.
        /// </summary>
        private RelayCommand<Task> timerPauseCommand;

        /// <summary>
        /// Gets the Запуск таймера.
        /// </summary>
        private RelayCommand<Task> timerStartCommand;

        /// <summary>
        /// Gets the остановка таймера.
        /// </summary>
        private RelayCommand<Task> timerStopCommand;

        private Task tskSelFoc;

        /// <summary>
        /// Вьюмодель для активных целей.
        /// </summary>
        private ucMainAimsViewModel ucMainAimsVM;

        /// <summary>
        /// Gets the Эту и следующие все вверх.
        /// </summary>
        private RelayCommand<int> waveUpCommand;

        /// <summary>
        /// Gets the Этой и всем вниз.
        /// </summary>
        private RelayCommand<int> waweDownCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // Моделирование прокачки навыка
            //AbilitiModel ab = new AbilitiModel();
            //List<double> valls = new List<double>();
            //int count = 0;

            //while (ab.ValueProperty < 5)
            //{
            //    ab.ValueIncrease(10, ab.ValueProperty);
            //    valls.Add(ab.ValueProperty);
            //    count++;
            //}

            //Stopwatch sw = new Stopwatch();
            //List<long> times = new List<long>();
            //sw.Start();

            Pers.IsPlanningModeMain = false;
            Pers.IsFocTasks = true;
            Pers.IsPlaningMode = false;
            Pers.IsSetViz = false;
            Pers.IsParetto = false;
            Pers.IsSortByPrioryty = false;
            Pers.IsSortByBalance = false;

            FillTaskLinkDic();

            // Подстраиваем влияния характеристик
            foreach (var cha in Pers.Characteristics)
            {
                foreach (var na in cha.NeedAbilitisProperty)
                {
                    if (na.KoeficientProperty > 6)
                        na.KoeficientProperty = 10;
                    else if (na.KoeficientProperty > 3)
                        na.KoeficientProperty = 6;
                    else if (na.KoeficientProperty > 0)
                        na.KoeficientProperty = 3;
                }
            }

            // Пересчитываем все значения после загрузки
            Pers.SetMinMaxValue();
            RefreshRelations();
            InizializeVariables();
            InizializeMesseges();
            //SetSW(sw, times, 0);

            // Если бустеры вообще не заданы
            if (Pers.Tasks.All(n => n.BoosterOfDone == 0 && n.BoosterOfFail == 0))
            {
                foreach (var tsk in Pers.Tasks)
                {
                    tsk.BoosterOfDone = 1;
                    tsk.BoosterOfFail = 1;
                }
            }

            // Чуток меняем время в задачах
            foreach (var task in Pers.Tasks)
            {
                if (task.TimeProperty == new DateTime(2001, 1, 1, 6, 0, 1))
                {
                    task.TimeProperty = new DateTime(2001, 1, 1, 11, 59, 1);
                }
                else if (task.TimeProperty == new DateTime(2001, 1, 1, 12, 0, 1))
                {
                    task.TimeProperty = new DateTime(2001, 1, 1, 17, 59, 1);
                }
                else if (task.TimeProperty == new DateTime(2001, 1, 1, 18, 0, 1))
                {
                    task.TimeProperty = new DateTime(2001, 1, 1, 23, 58, 1);
                }
            }

            foreach (var characteristic in StaticMetods.PersProperty.Characteristics)
            {
                foreach (var needAbility in characteristic.NeedAbilitisProperty)
                {
                    needAbility.GetRifhtK();
                }
            }

            ForOld();

            setNeednessTimer();
            viewLevelRefresh();
            CleardoneTasksImages();
            ResetTasksTimers();
            ReorderAbilitisIfPointsEnable();
            cleanAbNeeds(Pers.Abilitis);

            // Пересчитывем минимальные значения автоматом
            foreach (var abilitiModel in Pers.Abilitis)
            {
                abilitiModel.RecountMinValues();
            }

            Pers.RecountRangLevels();
            StaticMetods.RecountPersExp();
            StaticMetods.RecauntAllValues();
            StaticMetods.RefreshAllQwests(Pers, true, false, true);
            StaticMetods.AbillitisRefresh(Pers);

            SetTimer();
            Refresh();

            StaticMetods.RecountTaskLevels();

            Tasks.MoveCurrentToFirst();
            SellectedTask = (Task)Tasks.CurrentItem;

            // Глобальный бустер
            if (Pers.PersSettings.GlobalBooster <= 0)
            {
                Pers.PersSettings.GlobalBooster = 1.0;
                foreach (var abilitiModel in Pers.Abilitis)
                {
                    abilitiModel.Booster = Pers.PersSettings.GlobalBooster;
                }
            }

            //---------------------------------------------------------
            // Небольшое ускорение
            // Для задач скиллов - делаем пройденным IsDel, не пройденным не IsDel
            var abilitiesPers = Pers.Abilitis;
            foreach (var abilitiesPer in abilitiesPers)
            {
                abilitiesPer.RecountValue();
            }

            // Выполненным задачам квестов задаем IsDel
            foreach (var aim in Pers.Aims)
            {
                if (aim.IsDoneProperty)
                {
                    aim.NeedsTasks.ToList().ForEach(n => n.TaskProperty.IsDelProperty = true);
                }
            }

            if (Pers.isCheckMinLev == false)
            {
                foreach (var abilitiModel in Pers.Abilitis)
                {
                    abilitiModel.MinLevelProperty = 0;
                }

                Pers.isCheckMinLev = true;
            }

            // Всем скиллам задаем сложность
            foreach (var abilitiModel in Pers.Abilitis.Where(n => n.Hardness <= 0))
            {
                abilitiModel.Hardness = 3;
            }

            // Таймер помидоров ))
            if (Pers.PersSettings.PomodorroTime == 0)
            {
                Pers.PersSettings.PomodorroTime = 25;
            }

            SaveTimerStart();
            Pers.SellectedAbilityProperty = Pers.Abilitis.FirstOrDefault();
            Pers.SellectedAimProperty = Pers.Aims.FirstOrDefault();

            foreach (var task in Pers.Tasks)
            {
                task.RecountAutoValues();
            }

            foreach (var abilitiModel in Pers.Abilitis)
            {
                if (abilitiModel.ValueProperty < abilitiModel.FirstVal)
                    abilitiModel.ValueProperty = abilitiModel.FirstVal;
                abilitiModel.CheckNeedsForLevels();
            }

            Pers.Characteristics.ToList().ForEach(n => n.RecountChaValue());
            Pers.NewRecountExp();
            Pers.RecountPersLevel();
            RefreshTasksInMainView();

            // var exp1 = Pers.Abilitis.Sum(n => n.getPriorOfAb(n) * n.ValueProperty);

            // SetSW(sw, times, 0);
            // sw.Stop();
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The files to load save.
        /// </summary>
        private enum FilesToLoadSave
        {
            /// <summary>
            /// The pers.
            /// </summary>
            Pers
        }

        /// <summary>
        /// Режим разработчика
        /// </summary>
        public static bool IsDevelopmentMode { get; set; }

        public static bool IsEditModeAfterAbLevUp { get; set; }

        public static string PathToAndroidExpImp
        {
            get
            {
                return Path.Combine(Settings.Default.PathToDropBox, "!RpgOrganizer");
            }
        }

        /// <summary>
        /// The selected time.
        /// </summary>
        public static DateTime selectedTime
        {
            get { return _selectedTime.Date; }
            set { _selectedTime = value.Date; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show changes.
        /// </summary>
        public static bool showChanges { get; set; }

        public static double SizeDivider
        {
            get
            {
                if (s_sizeDivider < 1)
                {
                    s_sizeDivider = 1;
                }

                //return s_sizeDivider;
                return 2;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }

                s_sizeDivider = value;
            }
        }

        /// <summary>
        /// Словарь со ссылающимися из задачи элементами.
        /// </summary>
        public static Dictionary<Task, TaskLinkDictItem> TaskLinkDic { get; set; }

        /// <summary>
        /// Все активные скиллы
        /// </summary>
        public List<FocusModel> ActiveAbilitys
        {
            get
            {
                return _activeAbilitys;
            }
            set
            {
                _activeAbilitys = value;
                OnPropertyChanged(nameof(ActiveAbilitys));
            }
        }

        /// <summary>
        /// Активные квесты
        /// </summary>
        public List<FocusModel> ActiveQwests
        {
            get
            {
                return _activeQwests;
            }
            set
            {
                _activeQwests = value;
                OnPropertyChanged(nameof(ActiveQwests));
            }
        }

        /// <summary>
        /// Активные квесты
        /// </summary>
        public List<FocusModel> ActiveQwestsCollection
        {
            get
            {
                var focus = new List<FocusModel>();

                foreach (
                    var aim in Pers.Aims.Where(n => n.IsDoneProperty == false && n.AutoProgressValueProperty >= 99.9))
                {
                    focus.Add(GetItemToFocus(aim, Colors.Transparent, Visibility.Visible, true));
                }

                return focus;
            }
        }

        /// <summary>
        /// Третий вид отобажения задач
        /// </summary>
        public List<FocusModel> ActiveQwestsWithTasks
        {
            get
            {
                return _activeQwestsWithTasks;
            }

            set
            {
                var id = FirstFocus?.IdProperty;
                _activeQwestsWithTasks = value;
                FirstFocus = _activeQwestsWithTasks.FirstOrDefault(n => n.IdProperty == id);
                FirstFocus = FirstFocus ?? _activeQwestsWithTasks.FirstOrDefault();
                OnPropertyChanged(nameof(ActiveQwestsWithTasks));
            }
        }

        /// <summary>
        /// Gets the Добавить новую задачу.
        /// </summary>
        public RelayCommand AddNewTask
        {
            get
            {
                return _addNewTaskCommand
                       ?? (_addNewTaskCommand = new RelayCommand(
                           () =>
                           {
                               var thisTask = SellectedTask;
                               var isNormView = IsFocTaksVisibility == Visibility.Collapsed;
                               if (isNormView)
                               {
                                   var qqq = Task.AddTask(Pers.TasksTypes.FirstOrDefault());
                                   thisTask = qqq.Item1 ? qqq.Item2 : thisTask;
                               }
                               else
                               {
                                   var abil =
                                       Pers.Abilitis.FirstOrDefault(
                                           n => n.GUID == StaticMetods.FocsString.Last().GuidOfElement);
                                   var qwest =
                                       Pers.Aims.FirstOrDefault(
                                           n => n.GUID == StaticMetods.FocsString.Last().GuidOfElement);
                                   abil?.AddNeedTaskCommand.Execute("+");
                                   if (qwest != null)
                                   {
                                       var typeOfTaskDefoultProperty = qwest.TypeOfTaskDefoultProperty;
                                       var qqq = Task.AddTask(typeOfTaskDefoultProperty, null, qwest);
                                       thisTask = qqq.Item1 ? qqq.Item2 : thisTask;
                                   }
                               }

                               StaticMetods.RefreshAllQwests(Pers, true, true, true);
                               //reorderTasks();
                               RefreshTasksInMainView();
                               SellectedTask = thisTask;
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Добавление новой задачи.
        /// </summary>
        public RelayCommand<TypeOfTask> AddNewTaskCommand
        {
            get
            {
                return addNewTaskCommand
                       ?? (addNewTaskCommand =
                           new RelayCommand<TypeOfTask>(
                               AddNewTaskCommandExecute,
                               _type =>
                               {
                                   if (isTerribleBuff(Pers))
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        public List<FocusModel> AllSpells
        {
            get
            {
                return _allSpells;
            }
            set
            {
                _allSpells = value;
                OnPropertyChanged(nameof(AllSpells));
            }
        }

        /// <summary>
        /// Gets the Редактирование задачи из альтернативного режима.
        /// </summary>
        public RelayCommand<Task> AlterEditTaskCommand
        {
            get
            {
                return alterEditTaskCommand
                       ?? (alterEditTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               if (Pers.Tasks.FirstOrDefault(n => n.GUID == item.GUID) == null)
                               {
                                   return;
                               }

                               var thisTask = item;
                               var lastPar = LastParTask;
                               SellectedTask = item;
                               SellectedTask.EditTask();
                               RefreshTasksInMainView();
                               SellectedTask = thisTask;
                               LastParTask = lastPar;
                               //Tasks.MoveCurrentTo(lastPar);
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
        /// Gets the комманда альтернативное добавление задачи.
        /// </summary>
        public RelayCommand AlternateAddTaskCommand
        {
            get
            {
                return alternateAddTaskCommand
                       ?? (alternateAddTaskCommand = new RelayCommand(
                           () =>
                           {
                               var typeOfTask = Pers.TasksTypes.FirstOrDefault();
                               var viewVisibleTypes =
                                   SelectedView?.ViewTypesOfTasks.FirstOrDefault(n => n.isVisible);
                               if (viewVisibleTypes != null)
                               {
                                   typeOfTask = viewVisibleTypes.taskType;
                               }

                               AddNewTaskCommandExecute(typeOfTask);
                           },
                           () =>
                           {
                               if (isTerribleBuff(Pers))
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Gets the Альтернативное "Задача не сделана".
        /// </summary>
        public RelayCommand<Task> AlternateMinusTaskCommand
        {
            get
            {
                return alternateMinusTaskCommand
                       ?? (alternateMinusTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               int tInd;
                               Task t;
                               beforeClickForSmartSelect(out tInd, item, out t);

                               var isAlt = Keyboard.Modifiers == ModifierKeys.Alt;
                               if (!isAlt)
                               {
                                   item.ClickPlusMinusTomorrowTask(Pers, false, false, false, false);
                               }
                               else
                               {
                                   item.ClickPlusMinusTomorrowTask(Pers, false, true, false, false);
                               }
                               //var l = LastParTask;
                               RefreshTasksInMainView(true);
                               //LastParTask = l;
                               Task.RecountTaskLevel(item);
                               item.GetEnamyImage();
                               AsinchSaveData(Pers);
                               //SellectedTask = LastParTask;
                               afterClickForSmartSellect(t, tInd, item);
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
        /// Gets the Альтернативно сдвинуть задачу вниз.
        /// </summary>
        public RelayCommand<Task> AlternateMoveDownCommand
        {
            get
            {
                return alternateMoveDownCommand
                       ?? (alternateMoveDownCommand = new RelayCommand<Task>(
                           item =>
                           {
                               SellectedTask = item;
                               moveTaskDown(SellectedTask.TaskType);
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
        /// Gets the Альтернативный сдвиг задачи вверх.
        /// </summary>
        public RelayCommand<Task> AlternateMoveUpCommand
        {
            get
            {
                return alternateMoveUpCommand
                       ?? (alternateMoveUpCommand = new RelayCommand<Task>(
                           item =>
                           {
                               SellectedTask = item;
                               moveUpTask(SellectedTask.TaskType);
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
        /// Gets the Альтернативное сделание задачи.
        /// </summary>
        public RelayCommand<object> AlternatePlusTaskCommand
        {
            get
            {
                return alternatePlusTaskCommand
                       ?? (alternatePlusTaskCommand = new RelayCommand<object>(
                           item =>
                           {
                               //Task nextTask = null;
                               //GetNextTaskForSmartSelect(item, out nextTask);

                               var asTask = item as Task;

                               if (asTask == null)
                               {
                                   return;
                               }

                               var tsk = Pers.Tasks.FirstOrDefault(n => n.GUID == asTask.GUID);

                               if (tsk != null)
                               {
                                   int tInd;
                                   Task t;
                                   beforeClickForSmartSelect(out tInd, tsk, out t);
                                   if (Keyboard.Modifiers == ModifierKeys.Alt)
                                   {
                                       tsk.ClickPlusMinusTomorrowTask(Pers, true, true, false, false);
                                       RefreshTasksInMainView(true);
                                   }
                                   else
                                   {
                                       //var isAlt = Keyboard.Modifiers == ModifierKeys.Alt;
                                       tsk.ClickPlusMinusTomorrowTask(Pers, true, false, false, false);
                                       //var l = LastParTask;
                                       RefreshTasksInMainView(true);
                                       //LastParTask = l;
                                       Task.RecountTaskLevel(tsk);
                                       tsk.GetEnamyImage();
                                       //if (isAlt)
                                       //{
                                       //    item.BeginDateProperty = selectedTime;
                                       //    RefreshTasksInMainView(true);
                                       //}
                                   }

                                   AsinchSaveData(Pers);
                                   afterClickForSmartSellect(t, tInd, tsk);
                               }
                               else
                               {
                                   var selectMany = Pers.Tasks.SelectMany(n => n.SubTasks);
                                   var sub = selectMany.FirstOrDefault(n => n.Guid == asTask.GUID);
                                   if (sub != null) sub.isDone = true;
                                   RefreshTasksInMainView(true);
                               }
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               if (Pers == null)
                               {
                                   return false;
                               }

                               //if (Pers.PersSettings.IsCanDownPlusOnliIfSubtasksDoneProperty)
                               //{
                               //    if (item.SubTasks == null)
                               //    {
                               //        return true;
                               //    }

                               //    if (item.SubTasks.Count == 0)
                               //    {
                               //        return true;
                               //    }

                               //    var count = item.SubTasks.Count(n => n.isDone == false);
                               //    if (count > 0)
                               //    {
                               //        return false;
                               //    }
                               //}
                               return true;
                               //return StaticMetods.IsAllChildrenTasksDone(item);
                           }));
            }
        }

        /// <summary>
        /// Gets the Удаление задачи из альтернативного режима.
        /// </summary>
        public RelayCommand<Task> AlternateRemoveTaskCommand
        {
            get
            {
                return alternateRemoveTaskCommand
                       ?? (alternateRemoveTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               SellectedTask = item;
                               removeTask(item);
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
        /// Gets the комманда Анонимный экспорт персонажа.
        /// </summary>
        public RelayCommand AnonimExportCommand
        {
            get
            {
                return anonimExportCommand ?? (anonimExportCommand = new RelayCommand(
                    () =>
                    {
                        var of = new SaveFileDialog();
                        of.ShowDialog();
                        var pathToSave = of.FileName;
                        if (string.IsNullOrEmpty(pathToSave) == false)
                        {
                            try
                            {
                                // Сохраняем персонажа с его задачами
                                using (var fs = new FileStream(pathToSave, FileMode.Create))
                                {
                                    var formatter = new BinaryFormatter();
                                    formatter.Serialize(fs, Pers);
                                }

                                // Загружаем персонажа
                                var pers2 = Pers.LoadPers(pathToSave);

                                // Обнуляем все
                                pers2.ImageProperty = null;
                                foreach (var rangse in pers2.Rangs)
                                {
                                    rangse.ImageProperty = null;
                                }
                                pers2.NameOfProperty = "Искатель приключений";
                                pers2.DescriptionProperty = string.Empty;
                                pers2.History = string.Empty;
                                pers2.DiaryProperty.Clear();

                                // Обнуляем характеристики
                                for (var i = 0; i < pers2.Characteristics.Count; i++)
                                {
                                    var characteristic = pers2.Characteristics[i];
                                    characteristic.NameOfProperty = "Характеристика " + i;
                                    characteristic.ImageProperty = null;
                                    characteristic.DescriptionProperty = string.Empty;
                                }

                                // Обнуляем скиллы
                                for (var i = 0; i < pers2.Abilitis.Count; i++)
                                {
                                    var abilitiModel = pers2.Abilitis[i];
                                    abilitiModel.NameOfProperty = "Навык " + i;
                                    abilitiModel.ImageProperty = null;
                                    abilitiModel.DescriptionProperty = string.Empty;
                                }

                                // Обнуляем квесты
                                for (var i = 0; i < pers2.Aims.Count; i++)
                                {
                                    var qwest = pers2.Aims[i];
                                    qwest.NameOfProperty = "Квест " + i;
                                    qwest.ImageProperty = null;
                                    qwest.DescriptionProperty = string.Empty;
                                }

                                // Обнуляем награды
                                for (var i = 0; i < pers2.ShopItems.Count; i++)
                                {
                                    var rev = pers2.ShopItems[i];
                                    rev.NameOfProperty = "Награда " + i;
                                    rev.ImageProperty = null;
                                    rev.DescriptionProperty = string.Empty;
                                }

                                // Обнуляем задачи
                                for (var i = 0; i < pers2.Tasks.Count; i++)
                                {
                                    var task = pers2.Tasks[i];
                                    task.NameOfProperty = "Задача " + i;
                                    task.ImageProperty = null;
                                    task.DescriptionProperty = string.Empty;
                                }

                                // Сохраняем
                                using (var fs = new FileStream(pathToSave, FileMode.Create))
                                {
                                    var formatter = new BinaryFormatter();
                                    formatter.Serialize(fs, pers2);
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Ошибка при экспорте данных! Возможно проблема в правах доступа.");
                            }
                        }
                    },
                    () => { return true; }));
            }
        }

        public Visibility AntyFocTasksVisibility
        {
            get { return IsFocTaksVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Начать новую игру - удаление всего, кроме настроек
        /// </summary>
        public RelayCommand BeginNewGameCommand
        {
            get
            {
                return beginNewGameCommand ?? (beginNewGameCommand = new RelayCommand(

                    () =>
                    {
                        var hh = MessageBox.Show(
                            "Вы уверены, что хотите полностью удалить все данные и начать новую игру?",
                            "Новая игра",
                            MessageBoxButton.OKCancel);
                        if (hh == MessageBoxResult.OK)
                        {
                            Pers = Pers.LoadLerningTour();

                            // Сначала удаляем характеристики
                            foreach (var characteristic in Pers.Characteristics.ToList())
                            {
                                characteristic.RemoveCharacteristic(Pers);
                            }

                            // Теперь все скиллы
                            foreach (var abilitiModel in Pers.Abilitis.ToList())
                            {
                                StaticMetods.DeleteAbility(Pers, abilitiModel);
                            }

                            // Теперь этим методом подчищаем все остальное и перезагружаем прогу
                            Pers.ShopItems.Clear();
                            LetSBegin();
                        }
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Клик по счетчику.
        /// </summary>
        public RelayCommand<Task> ClickCounterCommand
        {
            get
            {
                return clickCounterCommand
                       ?? (clickCounterCommand =
                           new RelayCommand<Task>(
                               item => { item.CounterValueProperty++; },
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
        /// Gets the Клик минус в задаче (обычный режим).
        /// </summary>
        public RelayCommand<Task> ClickMinusCommand
        {
            get
            {
                return clickMinusCommand
                       ?? (clickMinusCommand = new RelayCommand<Task>(
                           item =>
                           {
                               SellectedTask = item;
                               item.ClickPlusMinusTomorrowTask(Pers, false);
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
        /// Gets the Комманда клик плюс в задаче.
        /// </summary>
        public RelayCommand<Task> ClickPlusCommand
        {
            get
            {
                return clickPlusCommand
                       ?? (clickPlusCommand = new RelayCommand<Task>(
                           item =>
                           {
                               SellectedTask = item;
                               item.ClickPlusMinusTomorrowTask(Pers, true);
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               if (Pers.PersSettings.IsCanDownPlusOnliIfSubtasksDoneProperty)
                               {
                                   if (item.SubTasks == null)
                                   {
                                       return true;
                                   }

                                   if (item.SubTasks.Count == 0)
                                   {
                                       return true;
                                   }

                                   if (item.SubTasks.Count(n => n.isDone == false) > 0)
                                   {
                                       return false;
                                   }
                               }

                               return StaticMetods.IsAllChildrenTasksDone(item);
                           }));
            }
        }

        /// <summary>
        /// Gets the Закрыть вид - фокусировка на квесте или скилле задачи.
        /// </summary>
        public RelayCommand CloseFocCommand
        {
            get
            {
                return closeFocCommand
                       ?? (closeFocCommand =
                           new RelayCommand(
                               () =>
                               {
                                   StaticMetods.FocsString.Remove(
                                       StaticMetods.FocsString.FirstOrDefault(n => n.GuidOfElement == SelFoc.IdProperty));
                                   var lastOrDefault = StaticMetods.FocsString.LastOrDefault();
                                   RefreshSelFoc(lastOrDefault?.GuidOfElement);
                                   if (lastOrDefault == null)
                                   {
                                       HideGhostBastersMode();
                                   }
                                   RefreshTasksInMainView(true);
                               },
                               () => { return true; }));
            }
        }

        public int ColsInPlaning
        {
            get
            {
                return _colsInPlaning;
            }
            set
            {
                _colsInPlaning = value;
                OnPropertyChanged(nameof(ColsInPlaning));
            }
        }

        /// <summary>
        /// Sets and gets Скопированная задача. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Task CopiedTaskProperty
        {
            get
            {
                return copiedTask;
            }

            set
            {
                if (copiedTask == value)
                {
                    return;
                }

                copiedTask = value;
                OnPropertyChanged("CopiedTaskProperty");
            }
        }

        /// <summary>
        /// Gets the Копировать задачу или перенести.
        /// </summary>
        public RelayCommand<string> CutOrCopyTaskCommand
        {
            get
            {
                return cutOrCopyTaskCommand
                       ?? (cutOrCopyTaskCommand = new RelayCommand<string>(
                           item =>
                           {
                               switch (item)
                               {
                                   case "вырезать":
                                       IsCutTaskProperty = true;
                                       break;

                                   case "копировать":
                                       IsCutTaskProperty = false;
                                       break;
                               }

                               CopiedTaskProperty = SellectedTask;
                               TaskContextVisibleProperty = false;
                               IsMoveOrCopyEnabledProperty = true;
                           },
                           item => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Видимомть редактирования задачи. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Visibility EditVisibilityProperty
        {
            get
            {
                return editVisibility;
            }

            set
            {
                if (editVisibility == value)
                {
                    return;
                }

                editVisibility = value;
                OnPropertyChanged("EditVisibilityProperty");
            }
        }

        /// <summary>
        /// Gets the комманда Конец хода.
        /// </summary>
        public RelayCommand EndOfTurnCommand
        {
            get
            {
                return _endOfTurnCommand ?? (_endOfTurnCommand = new RelayCommand(
                    () =>
                    {
                        EndOfTurn();
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Скрыть/показать шапку персонажа.
        /// </summary>
        public RelayCommand<string> ExpandShrinkHUDCommand
        {
            get
            {
                return expandShrinkHUDCommand
                       ?? (expandShrinkHUDCommand = new RelayCommand<string>(
                           item =>
                           {
                               shrinkExpand(item);
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
        /// Текст для отображения в кнопку свернуть/развернуть
        /// </summary>
        public string ExpandTaskPanelText
        {
            get
            {
                if (Pers.IsHideHUD)
                {
                    return "   Свернуть панель задач   ";
                }
                return "   Развернуть панель задач   ";
            }
        }

        /// <summary>
        /// Gets the Экспорт для андроид.
        /// </summary>
        public RelayCommand ExportToAndroidCommand
        {
            get
            {
                return exportToAndroidCommand
                       ?? (exportToAndroidCommand =
                           new RelayCommand(
                               () => { ExportToAndroid(null); },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Экспортировать информацию для ВИКИ.
        /// </summary>
        public RelayCommand ExportToWikiCommand
        {
            get
            {
                return _exportToWikiCommand
                       ?? (_exportToWikiCommand = new RelayCommand(
                           () =>
                           {
                               Pers pers = Pers;

                               ExportWiki(pers);
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Экспортировать в Wiki.
        /// </summary>
        /// <param name="pers"></param>
        public static void ExportWiki(Pers pers)
        {
            Func<string, string> getRightName = s =>
            {
                s = s.Replace(':', '_');
                s = s.Replace('*', ' ');
                s = s.Replace('"', '@');
                s = s.Replace(':', '-');
                s = s.Replace('\\', ' ');
                s = s.Replace('<', ' ');
                s = s.Replace('>', ' ');
                s = s.Replace('|', ' ');
                return s;
            };

            var fb = new FolderBrowserDialog();
            fb.ShowDialog();
            var path = fb.SelectedPath;
            if (!Directory.Exists(path)) return;
            var name = $"РПГ Органайзер Вики {DateTime.Now}";
            path = Path.Combine(path, getRightName(name));
            Directory.CreateDirectory(path);

            // Экспортируем Характеристики
            var chaPath = Path.Combine(path, "Характеристики");
            Directory.CreateDirectory(chaPath);
            var imageFormat = ImageFormat.Png;
            foreach (var characteristic in pers.Characteristics)
            {
                var ms = new MemoryStream(characteristic.ImageProperty);
                var i = Image.FromStream(ms);
                i.Save(Path.Combine(chaPath, $"{getRightName(characteristic.NameOfProperty)}.png"),
                    imageFormat);
                string txt =
                    $"[[Файл:{getRightName(characteristic.NameOfProperty)}.png|thumb|240px]]";
                txt += $"{Environment.NewLine}== Описание ==";
                txt += $"{Environment.NewLine}''{characteristic.DescriptionProperty}''";
                txt += $"{Environment.NewLine}== Ассоциации ==";
                txt += $"{Environment.NewLine}== Навыки ==";
                foreach (
                    var needAbility in
                        characteristic.NeedAbilitisProperty.Where(n => n.KoeficientProperty > 0)
                            .OrderByDescending(n => n.KoeficientProperty)
                            .ToList())
                {
                    txt += $"{Environment.NewLine}* {needAbility.AbilProperty.NameOfProperty}";
                }
                File.WriteAllText(
                    Path.Combine(chaPath, getRightName(characteristic.NameOfProperty + ".txt")), txt);
            }
            // скиллы
            var abPath = Path.Combine(path, "Навыки");
            Directory.CreateDirectory(abPath);
            foreach (var ab in pers.Abilitis)
            {
                var ms = new MemoryStream(ab.ImageProperty);
                var i = Image.FromStream(ms);
                i.Save(Path.Combine(abPath, $"{getRightName(ab.NameOfProperty)}.png"), imageFormat);
                string txt = $"[[Файл:{getRightName(ab.NameOfProperty)}.png|thumb|240px]]";
                txt += $"{Environment.NewLine}== Описание ==";
                txt += $"{Environment.NewLine}''{ab.DescriptionProperty}''";
                txt += $"{Environment.NewLine}== Ассоциации ==";
                txt += $"{Environment.NewLine}== Характеристики ==";
                // Требования скиллов
                foreach (
                    var cha in
                        pers.Characteristics.Where(
                            n =>
                                n.NeedAbilitisProperty.Any(
                                    q => q.KoeficientProperty > 0 && q.AbilProperty == ab)))
                {
                    txt += $"{Environment.NewLine}* {cha.NameOfProperty}";
                }
                txt += $"{Environment.NewLine}== Условия ==";
                foreach (var complecsNeed in ab.ComplecsNeeds)
                {
                    var nType = complecsNeed.IsQwest
                        ? $"Квест \"{complecsNeed.NeedQwest.AimProperty.NameOfProperty}\""
                        : $"{complecsNeed.NeedTask.TaskProperty.NameOfProperty}{complecsNeed.NeedTask.TaskProperty.SubTasksString}{complecsNeed.NeedTask.TaskProperty.PlusNameOf2}";
                    txt +=
                        $"{Environment.NewLine}* [{complecsNeed.LevelProperty} - {complecsNeed.ToLevelProperty}] {nType}";
                }

                File.WriteAllText(Path.Combine(abPath, getRightName(ab.NameOfProperty + ".txt")), txt);
            }

            // награды
            var revPath = Path.Combine(path, "Награды");
            Directory.CreateDirectory(revPath);
            foreach (var revard in pers.ShopItems)
            {
                var ms = new MemoryStream(revard.ImageProperty);
                var i = Image.FromStream(ms);
                i.Save(Path.Combine(revPath, $"{getRightName(revard.NameOfProperty)}.png"),
                    imageFormat);
                string txt = $"[[Файл:{getRightName(revard.NameOfProperty)}.png|thumb|240px]]";
                txt += $"{Environment.NewLine}== Описание ==";
                txt += $"{Environment.NewLine}''{revard.DescriptionProperty}''";
                txt += $"{Environment.NewLine}== Дополнительно ==";
                if (revard.IsArtefact)
                {
                    txt += $"{Environment.NewLine}* Артефакт";
                }
                if (revard.IsFromeTasksProperty)
                {
                    txt +=
                        $"{Environment.NewLine}* Выпадает из задач с вероятностью {revard.VeroyatnostProperty}%";
                }
                txt += $"{Environment.NewLine}== Условия ==";
                // Стоимость золотых
                if (revard.CostProperty > 0)
                {
                    txt += $"{Environment.NewLine}* Стоимость {revard.CostProperty} золотых";
                }
                if (revard.NeedLevelProperty > 0)
                {
                    txt += $"{Environment.NewLine}* Уровень персонажа >= {revard.NeedLevelProperty}";
                }
                foreach (var abilityNeed in revard.NeedCharacts)
                {
                    txt +=
                        $"{Environment.NewLine}* Характеристика \"{abilityNeed.CharactProperty.NameOfProperty}\"";
                }
                foreach (var abilityNeed in revard.AbilityNeeds)
                {
                    txt +=
                        $"{Environment.NewLine}* Навык \"{abilityNeed.AbilProperty.NameOfProperty}\"";
                }
                foreach (var abilityNeed in revard.NeedQwests)
                {
                    txt += $"{Environment.NewLine}* Квест \"{abilityNeed.NameOfProperty}\"";
                }

                File.WriteAllText(
                    Path.Combine(revPath, getRightName(revard.NameOfProperty + ".txt")), txt);
            }
        }

        /// <summary>
        /// Первый элемент фокусировки
        /// </summary>
        public FocusModel FirstFocus
        {
            get
            {
                return _firstFocus;
            }
            set
            {
                _firstFocus = value;
                OnPropertyChanged(nameof(FirstFocus));
            }
        }

        public RelayCommand FocModeEnabledDisabledCommand
        {
            get
            {
                return focModeEnabledDisabledCommand ?? (focModeEnabledDisabledCommand = new RelayCommand(

                    () =>
                    {
                        IsPlaningMode = false;
                        OnPropertyChanged(nameof(IsPlaningMode));
                        IsFocMode = !IsFocMode;
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets Задачи из фокусировки
        /// </summary>
        public ListCollectionView FocTasks { get; set; }

        /// <summary>
        /// Gets the Все задачи во втором виде перенести во конец списка.
        /// </summary>
        public RelayCommand FocusViewAllToEndCommand
        {
            get
            {
                return focusViewAllToEndCommand
                       ?? (focusViewAllToEndCommand =
                           new RelayCommand(
                               () =>
                               {
                                   var firstFocusTasks = FirstFocus.Tasks;

                                   var focusCollection = ActiveQwestsWithTasks;
                                   var indexOf = focusCollection.IndexOf(FirstFocus);
                                   var secondFocus = focusCollection.Count > indexOf + 1
                                       ? focusCollection[indexOf + 1]
                                       : focusCollection[0];

                                   Action tasksToEnd = () =>
                                   {
                                       var enumerable = (from tsk in Tasks.Cast<Task>()
                                                         join task1 in firstFocusTasks on tsk equals task1
                                                         select tsk).ToList();

                                       foreach (var task in enumerable)
                                       {
                                           task.ToEndOfList();
                                       }

                                       FirstFocus = null;
                                       RefreshTasksInMainView();
                                   };

                                   tasksToEnd();
                               },
                               () =>
                               {
                                   if (FirstFocus == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the В виде фокусировки перенос всех задач в начало.
        /// </summary>
        public RelayCommand FovusedViewAllToBeginCommand
        {
            get
            {
                return fovusedViewAllToBeginCommand
                       ?? (fovusedViewAllToBeginCommand =
                           new RelayCommand(
                               () =>
                               {
                                   var firstFocusTasks = FirstFocus.Tasks;

                                   var enumerable = (from tsk in Tasks.Cast<Task>()
                                                     join task1 in firstFocusTasks on tsk equals task1
                                                     select tsk).Reverse().ToList();

                                   foreach (var task in enumerable)
                                   {
                                       var taskA = task;
                                       Tasks.MoveCurrentToPosition(0);
                                       if (Tasks.CurrentItem != null)
                                       {
                                           var taskB = (Task)Tasks.CurrentItem;
                                           taskA.SecondOfDone = taskB.SecondOfDone - 1;
                                           var oldIndex = Pers.Tasks.IndexOf(taskA);
                                           var newIndex = Pers.Tasks.IndexOf(taskB);
                                           Pers.Tasks.Move(oldIndex, newIndex);
                                       }
                                   }

                                   FirstFocus = null;

                                   RefreshTasksInMainView();
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Команда - перейти на веб сайт по ссылке.
        /// </summary>
        public RelayCommand<string> GoToURL
        {
            get
            {
                return goToURLCommand
                       ?? (goToURLCommand =
                           new RelayCommand<string>(
                               item => { OpenLink(item); },
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
        /// Высота строки с данными о персе
        /// </summary>
        public GridLength HeightOfDataGrid
        {
            get
            {
                if (Pers != null && Pers.IsHideHUD)
                {
                    return new GridLength(3);
                }

                return new GridLength(Pers.HeightPersRow);
            }

            set
            {
                Pers.HeightPersRow = value.Value;
            }
        }

        /// <summary>
        /// Поддержать проект
        /// </summary>
        public RelayCommand HelpForProjectCommand
        {
            get
            {
                return helpForProjectCommand ?? (helpForProjectCommand = new RelayCommand(

                    () =>
                    {
                        HelpProject hp = new HelpProject();
                        hp.ShowDialog();
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Импортировать из андроида.
        /// </summary>
        public RelayCommand ImportFromAndroidCommand
        {
            get
            {
                return importFromAndroidCommand
                       ?? (importFromAndroidCommand =
                           new RelayCommand(
                               () =>
                               {
                                   var of = new OpenFileDialog { Filter = "Файл данных|*.json" };
                                   of.ShowDialog();
                                   //var path = of.FileName;
                                   if (File.Exists(of.FileName))
                                   {
                                       ImportFromAndroid(of.FileName);
                                   }
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Видимость панели интерфейса - групп
        /// </summary>
        public bool InterfaceActiveGroupsVisibillity
        {
            get
            {
                return !IsFocMode;
                //return _interfaceActiveGroupsVisibillity;
            }
            set
            {
                return;
                //if (_interfaceActiveGroupsVisibillity == value) return;
                //_interfaceActiveGroupsVisibillity = value;
                //OnPropertyChanged(nameof(InterfaceActiveGroupsVisibillity));
            }
        }

        /// <summary>
        /// Sets and gets Задача вырезана?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsCutTaskProperty
        {
            get
            {
                return isCutTask;
            }

            set
            {
                if (isCutTask == value)
                {
                    return;
                }

                isCutTask = value;
                OnPropertyChanged("IsCutTaskProperty");
            }
        }

        /// <summary>
        /// Sets and gets Открыть добавление или редактирование?. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool IsEditOrAddOpenProperty
        {
            get
            {
                return isEditOrAddOpen;
            }

            set
            {
                isEditOrAddOpen = value;
                OnPropertyChanged("IsEditOrAddOpenProperty");
            }
        }

        /// <summary>
        /// Режим фокусировки активирован?
        /// </summary>
        public bool IsFocMode
        {
            get
            {
                return Pers.IsFocTasks;
            }
            set
            {
                if (Pers.IsFocTasks == value) return;
                Pers.IsFocTasks = value;
                OnPropertyChanged(nameof(IsFocMode));
                OnPropertyChanged(nameof(InterfaceActiveGroupsVisibillity));
                if (value == true)
                {
                    IsPlaningMode = false;
                }
                FillFoc();
                if (!IsPlaningMode)
                {
                    Pers.focBeforePlanning = IsFocMode;
                }
            }
        }

        /// <summary>
        /// Видна ли сфокусированность на элементе на который влияет задача?
        /// </summary>
        public Visibility IsFocTaksVisibility
        {
            get
            {
                return _isFocTaksVisibility;
            }
            set
            {
                _isFocTaksVisibility = value;

                if (value == Visibility.Collapsed)
                {
                    StaticMetods.FocsString.Clear();
                    RefreshTasksInMainView(true);
                }
                OnPropertyChanged(nameof(IsFocTaksVisibility));
                OnPropertyChanged(nameof(IsSkillsVisible));
                OnPropertyChanged(nameof(IsTaskPanelVisibility));
                OnPropertyChanged(nameof(IsPipBoyVisibility));
                OnPropertyChanged(nameof(AntyFocTasksVisibility));
            }
        }

        /// <summary>
        /// Sets and gets Активирован режим переноса?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsMoveOrCopyEnabledProperty
        {
            get
            {
                return isMoveOrCopyEnabled;
            }

            set
            {
                if (isMoveOrCopyEnabled == value)
                {
                    return;
                }

                isMoveOrCopyEnabled = value;
                OnPropertyChanged("IsMoveOrCopyEnabledProperty");
            }
        }

        /// <summary>
        /// Видимость пипбоя
        /// </summary>
        public Visibility IsPipBoyVisibility
        {
            get
            {
                if (IsTaskPanelVisibility == Visibility.Visible || IsSkillsVisible == Visibility.Visible)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Режим планирования
        /// </summary>
        public bool IsPlaningMode
        {
            get
            {
                return Pers.IsPlanningModeMain;
                //return Pers.IsPlaningMode;
            }
            set
            {
                //if (Pers.IsPlaningMode == value) return;

                Pers.IsPlanningModeMain = value;
                OnPropertyChanged(nameof(IsPlaningMode));
                if (value == true)
                {
                    Pers.focBeforePlanning = IsFocMode;
                    IsFocMode = false;
                }
                else
                {
                    IsFocMode = Pers.focBeforePlanning;
                }
                StaticMetods.Locator.AimsVM.QCollectionViewProperty.Refresh();
                //reorderTasks();
                RefreshTasksInMainView(false);
                RefreshLocationsVisibility();

                //if (Pers.IsPlaningMode == value) return;

                //Pers.IsPlaningMode = value;
                //OnPropertyChanged(nameof(IsPlaningMode));
                //if (value == true)
                //{
                //    Pers.focBeforePlanning = IsFocMode;
                //    IsFocMode = false;
                //}
                //else
                //{
                //    IsFocMode = Pers.focBeforePlanning;
                //}
                //StaticMetods.Locator.AimsVM.QCollectionViewProperty.Refresh();
                ////reorderTasks();
                //RefreshTasksInMainView(false);
                //RefreshLocationsVisibility();
            }
        }

        /// <summary>
        /// Видимость панели скиллов
        /// </summary>
        public Visibility IsSkillsVisible
        {
            get
            {
                if (IsFocTaksVisibility == Visibility.Collapsed || SelFoc == null)
                {
                    return Visibility.Collapsed;
                }

                if (SelFoc.Skills == null)
                {
                    return Visibility.Collapsed;
                }

                return SelFoc.Skills.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Видимость панели задач
        /// </summary>
        public Visibility IsTaskPanelVisibility
        {
            get
            {
                if (IsFocTaksVisibility == Visibility.Visible)
                {
                    return SelFoc?.Tasks?.FirstOrDefault() == null ? Visibility.Collapsed : Visibility.Visible;
                }
                return Tasks == null || Tasks.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Sets and gets Открыть окно показать изменения?. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsViewChangesOpenPropertyProperty
        {
            get
            {
                return isViewChangesOpenProperty;
            }

            set
            {
                if (isViewChangesOpenProperty == value)
                {
                    return;
                }

                showChanges = value;
                isViewChangesOpenProperty = value;
                OnPropertyChanged("IsViewChangesOpenPropertyProperty");
            }
        }

        /// <summary>
        /// Последняя родительская задача
        /// </summary>
        public Task LastParTask
        {
            get
            {
                return _lastParTask;
            }
            set
            {
                if (_lastParTask == value) return;
                if (!Tasks.Contains(value)) return;
                _lastParTask = value;
                FillFoc();
                OnPropertyChanged(nameof(LastParTask));
                RefreshSellectedInFoc(value, ActiveQwests);
            }
        }

        /// <summary>
        /// Gets the Комманда - начать с начала.
        /// </summary>
        public RelayCommand LetSBeginCommand
        {
            get
            {
                return letSBeginCommand ?? (letSBeginCommand = new RelayCommand(
                    () =>
                    {
                        var hh = MessageBox.Show(
                            "Вы уверены, что хотите очистить все достижения?",
                            "Начать все с начала",
                            MessageBoxButton.OKCancel);
                        if (hh == MessageBoxResult.OK)
                        {
                            // Удаляем все задачи
                            LetSBegin();
                        }
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Загрузить продвинутый шаблон.
        /// </summary>
        public RelayCommand LoadAdvansedTemplateCommand
        {
            get
            {
                return loadAdvansedTemplateCommand
                       ?? (loadAdvansedTemplateCommand =
                           new RelayCommand(
                               loadAdvensedTemplateVoid,
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Загрузить пустой персонаж.
        /// </summary>
        public RelayCommand LoadClearPersCommand
        {
            get
            {
                return _loadClearPersCommand
                       ?? (_loadClearPersCommand =
                           new RelayCommand(
                               () =>
                               {
                                   StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.ПолностьюСНачала, null);
                                   LoadPersFromPath(
                                       Path.Combine(Directory.GetCurrentDirectory(), "Templates", "LearningPers"));
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Загрузить обучающую компанию.
        /// </summary>
        public RelayCommand LoadLearningCompanyCommand
        {
            get
            {
                return loadLearningCompanyCommand
                       ?? (loadLearningCompanyCommand =
                           new RelayCommand(
                               () =>
                               {
                                   var hh = MessageBox.Show(
                            "Все данные будут удалены и загружена обучающая компания. Вы уверены?",
                            "Загрузка обучающей компании",
                            MessageBoxButton.OKCancel);
                                   if (hh == MessageBoxResult.OK)
                                   {
                                       StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.ПолностьюСНачала, null);
                                       LoadPersFromPath(
                                           Path.Combine(Directory.GetCurrentDirectory(), "Templates", "LearningPers"));
                                   }
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Загрузить обучающую компанию.
        /// </summary>
        public RelayCommand LoadLearningTourCommand
        {
            get
            {
                return _loadLearningTourCommand
                       ?? (_loadLearningTourCommand = new RelayCommand(
                           () =>
                           {
                               var perrs = Pers.LoadLerningTour();

                               if (perrs == null)
                               {
                                   return;
                               }

                               Pers = perrs;
                               Pers.LastDateOfUseProperty = DateTime.Now.ToString();

                               if (Pers != null)
                               {
                                   Restart(Pers);
                               }
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Загрузить простую версию шаблона.
        /// </summary>
        public RelayCommand LoadSampleTemplateCommand
        {
            get
            {
                return loadSampleTemplateCommand
                       ?? (loadSampleTemplateCommand =
                           new RelayCommand(
                               SampleTemplateLoadVoid,
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Минуты таймера для снижения ХП. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int MinutesProperty
        {
            get
            {
                return minutes;
            }

            set
            {
                if (minutes == value)
                {
                    return;
                }

                minutes = value;
                OnPropertyChanged("MinutesProperty");
            }
        }

        /// <summary>
        /// Gets the Сдвинуть задачу в начало списка.
        /// </summary>
        public RelayCommand<Task> MoveTaskToBeginOfListCommand
        {
            get
            {
                return moveTaskToBeginOfListCommand
                       ?? (moveTaskToBeginOfListCommand = new RelayCommand<Task>(
                           item =>
                           {
                               Action doAct = () =>
                               {
                                   Pers.Tasks.Move(Pers.Tasks.IndexOf(item), 0);
                                   RefreshTasksInMainView();
                               };

                               var position1 = ActiveQwestsWithTasks.IndexOf(FirstFocus);

                               doAct.Invoke();

                               var position2 = ActiveQwestsWithTasks.IndexOf(FirstFocus);
                               if (position1 != position2)
                               {
                                   FirstFocus = ActiveQwestsWithTasks.FirstOrDefault();
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
        /// Gets the Сдвинуть задачу в конец списка.
        /// </summary>
        public RelayCommand<Task> MoveTaskToEndOfListCommand
        {
            get
            {
                return moveTaskToEndOfListCommand
                       ?? (moveTaskToEndOfListCommand = new RelayCommand<Task>(
                           item =>
                           {
                               var position1 = ActiveQwestsWithTasks.IndexOf(FirstFocus);

                               //item.SecondOfDone =Task.GetSecOfDone();
                               Pers.Tasks.Move(Pers.Tasks.IndexOf(item), Pers.Tasks.Count - 1);
                               RefreshTasksInMainView();

                               var position2 = ActiveQwestsWithTasks.IndexOf(FirstFocus);
                               if (position1 != position2)
                               {
                                   FirstFocus = ActiveQwestsWithTasks.FirstOrDefault();
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
        /// Gets the Сдвинуть волну задачи влево.
        /// </summary>
        public RelayCommand<Task> MoveTaskWaveLeftCommand
        {
            get
            {
                return moveTaskWaveLeftCommand
                       ?? (moveTaskWaveLeftCommand = new RelayCommand<Task>(
                           item =>
                           {
                               //item.Wave--;
                               //Tasks.MoveCurrentTo(item);
                               Tasks.MoveCurrentTo(item);

                               if (Tasks.IndexOf(item) == 0)
                                   return;

                               var taskA = (Task)Tasks.CurrentItem;
                               Tasks.MoveCurrentToPrevious();
                               var taskB = (Task)Tasks.CurrentItem;
                               MoveTask(taskA, taskB);
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
        /// Gets the Сдвинуть задачу вправо.
        /// </summary>
        public RelayCommand<Task> MoveTaskWaveRightCommand
        {
            get
            {
                return moveTaskWaveRightCommand
                       ?? (moveTaskWaveRightCommand = new RelayCommand<Task>(
                           item =>
                           {
                               Tasks.MoveCurrentTo(item);

                               if (Tasks.IndexOf(item) == Tasks.Count - 1)
                                   return;

                               var taskA = (Task)Tasks.CurrentItem;
                               Tasks.MoveCurrentToNext();
                               var taskB = (Task)Tasks.CurrentItem;
                               MoveTask(taskA, taskB);
                               // item.Wave++;
                               // Tasks.MoveCurrentTo(item);
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
        /// Gets the Сдвинуть вид для третьего отображения задач.
        /// </summary>
        public RelayCommand<string> MoveViewCommand
        {
            get
            {
                return moveViewCommand
                       ?? (moveViewCommand = new RelayCommand<string>(
                           item =>
                           {
                               switch (item)
                               {
                                   case "left":
                                       Tasks.MoveCurrentToPrevious();
                                       if (Keyboard.Modifiers == ModifierKeys.Control)
                                       {
                                           Tasks.MoveCurrentToFirst();
                                       }

                                       break;

                                   case "right":
                                       Tasks.MoveCurrentToNext();
                                       if (Keyboard.Modifiers == ModifierKeys.Control)
                                       {
                                           Tasks.MoveCurrentToLast();
                                       }
                                       break;
                               }

                               SellectedTask = (Task)Tasks.CurrentItem;

                               OnPropertyChanged("SellectedTask");

                               Messenger.Default.Send("!!!");
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
        /// Gets the Сдвинуть волну влево.
        /// </summary>
        public RelayCommand<int> MoveWaveLeftCommand
        {
            get
            {
                return moveWaveLeftCommand
                       ?? (moveWaveLeftCommand = new RelayCommand<int>(
                           item =>
                           {
                               var t = Tasks.Cast<Task>().Where(n => n.Wave == item).ToList();
                               foreach (var source in t)
                               {
                                   source.Wave--;
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
        /// Gets the Сдвинуть волну вправо.
        /// </summary>
        public RelayCommand<int> MoveWaveRightCommand
        {
            get
            {
                return moveWaveRightCommand
                       ?? (moveWaveRightCommand = new RelayCommand<int>(
                           item =>
                           {
                               var t = Tasks.Cast<Task>().Where(n => n.Wave == item).ToList();
                               foreach (var source in t)
                               {
                                   source.Wave++;
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
        /// Gets the комманда Начать новую игру.
        /// </summary>
        public RelayCommand NewGameCommand
        {
            get
            {
                return newGameCommand ?? (newGameCommand = new RelayCommand(
                    () =>
                    {
                        var ng = new NewGameWindow();
                        ng.ShowDialog();
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Новая задача
        /// </summary>
        public Task NewTask { get; set; }

        /// <summary>
        /// Фокусировка - следующие задачи
        /// </summary>
        public RelayCommand NextFocViewTasksCommand
        {
            get
            {
                return nextFocViewTasksCommand ?? (nextFocViewTasksCommand = new RelayCommand(

                    () =>
                    {
                        MoveTask(SellectedTask, TryGetTaskByIndex(Tasks.IndexOf(SellectedTask) + 1));
                        //if (LastParTask == null)
                        //{
                        //    return;
                        //}
                        //if (!Tasks.Contains(LastParTask))
                        //{
                        //    return;
                        //}

                        //var indOf = Tasks.IndexOf(LastParTask);
                        //if (indOf >= Tasks.Count - 1)
                        //{
                        //    SellectedTask = TryGetTaskByIndex(0);
                        //    return;
                        //}

                        //SellectedTask = TryGetTaskByIndex(indOf + 1);
                    },
                    () =>
                    {
                        return true;
                    }));
            }
        }

        /// <summary>
        /// Gets the комманда Открыть общие настройки.
        /// </summary>
        public RelayCommand OpenAllSettingsCommand
        {
            get
            {
                return openAllSettingsCommand
                       ?? (openAllSettingsCommand = new RelayCommand(
                           () =>
                           {
                               var pref = new AllPreferenses { DataContext = Pers.PersSettings };
                               pref.btnOk.Click += (sender, args) =>
                               {
                                   pref.Close();
                                   OnPropertyChanged("FocusCollectionProperty");
                               };

                               Messenger.Default.Send<Window>(pref);
                               pref.ShowDialog();
                               StaticMetods.RecauntAllValues();
                               ReorderAbilitisIfPointsEnable();
                               if (Pers.PersSettings.IsHpMinusForTimerProperty)
                               {
                                   timerHP.Start();
                               }
                               else
                               {
                                   timerHP.Stop();
                               }
                               RefreshTasksInMainView(true);
                               Pers.RefreshAllAboutHp();
                           },
                           () => { return true; }));
            }
        }

        public RelayCommand OpenAllTasksCommand
        {
            get
            {
                return openAllTasksCommand ?? (openAllTasksCommand = new RelayCommand(
                    () =>
                    {
                        var at = new AllTasksWindowView();
                        at.ShowDialog();
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Открыть окно Автофокус!!!.
        /// </summary>
        public RelayCommand OpenAutofocusCommand
        {
            get
            {
                return openAutofocusCommand
                       ?? (openAutofocusCommand = new RelayCommand(
                           () =>
                           {
                               var av = new AvtofocusView();
                               var vm = new AutofocusViewModel(Pers, SelectedView);
                               av.DataContext = vm;
                               av.ShowDialog();

                               // this.Tasks = CollectionViewSource.GetDefaultView(this.Pers.Tasks);
                               Tasks.SortDescriptions.Clear();
                               Tasks.Filter = null;
                               Refresh();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Команда - перейти на веб сайт по ссылке.
        /// </summary>
        public RelayCommand OpenDiaryCommand
        {
            get
            {
                return _openDiary
                       ?? (_openDiary =
                           new RelayCommand(() =>
                           {
                               var path = Path.Combine(Settings.Default.PathToPers, "TheBookOfSuccess.md");
                               var path2 = Path.Combine(Settings.Default.PathToPers, "TheBookOfSuccess.html");

                               if (File.Exists(path))
                               {
                                   File.Delete(path);
                               }

                               if (File.Exists(path2))
                               {
                                   File.Delete(path2);
                               }

                               var book = Pers.BookOfSuccess;
                               using (StreamWriter w = File.AppendText(path))
                               {
                                   for (int i = book.Count - 1; i >= 0; i--)
                                   {
                                       w.WriteLine(book[i]);
                                   }
                               }

                               if (File.Exists(path))
                               {
                                   using (var reader = new StreamReader(path, Encoding.UTF8))
                                   using (
                                       var writer = new StreamWriter(new FileStream(path2, FileMode.OpenOrCreate),
                                           Encoding.UTF8))
                                   {
                                       CommonMarkConverter.Convert(reader, writer);
                                   }

                                   Process.Start(path2);
                               }
                           }));
            }
        }

        /// <summary>
        /// Gets the Открыть первый квест, связанный с задачей.
        /// </summary>
        public RelayCommand OpenFirstLinkQwestCommand
        {
            get
            {
                return openFirstLinkQwestCommand
                       ?? (openFirstLinkQwestCommand =
                           new RelayCommand(
                               () => { },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Зайти вглубь задачи и открыть самый первый элемент фокусировки.
        /// </summary>
        public RelayCommand OpenFirstLinkTaskCommand
        {
            get
            {
                return openFirstLinkTaskCommand
                       ?? (openFirstLinkTaskCommand =
                           new RelayCommand(
                               () => { },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Открыть связанный скилл.
        /// </summary>
        public RelayCommand<TaskRelaysItem> OpenLinkedAbilityCommand
        {
            get
            {
                return openLinkedAbilityCommand
                       ?? (openLinkedAbilityCommand =
                           new RelayCommand<TaskRelaysItem>(
                               item =>
                               {
                                   switch (item.TypeProperty)
                                   {
                                       case "навык":
                                           SelectFocusCommand.Execute(item.GuidProperty);
                                           break;

                                       case "характеристика":
                                           showCharacteristic(
                                               Pers.Characteristics.First(n => n.GUID == item.GuidProperty));
                                           break;

                                       case "квест":
                                           SelectFocusCommand.Execute(item.GuidProperty);
                                           break;
                                   }

                                   Messenger.Default.Send("Обновить уровни!");

                                   Messenger.Default.Send("ПлюсМинусНажат!");

                                   RefreshTasksInMainView();

                                   var _task = SellectedTask;
                                   if (_task == null || _task.IsDelProperty
                                       || IsTaskVisibleInCurrentView(_task, SelectedView, Pers) == false)
                                   {
                                       Tasks.MoveCurrentToFirst();
                                       SellectedTask = (Task)Tasks.CurrentItem;
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
        /// Gets the Открыть элемент для редактирования.
        /// </summary>
        public RelayCommand<object> OpenLinkElementForEditCommand
        {
            get
            {
                return openLinkElementForEditCommand
                       ?? (openLinkElementForEditCommand = new RelayCommand<object>(
                           item =>
                           {
                               // Редактировать элемент фокусировки
                               Action<string> editFocusItemInfo = s =>
                               {
                                   SelectFocusCommand.Execute(s);
                                   if (IsFocTaksVisibility == Visibility.Visible && !FocTasks.Contains(SellectedTask))
                                   {
                                       FocTasks.MoveCurrentToFirst();
                                   }
                                   else
                                   {
                                       //if (!Tasks.Contains(SellectedTask))
                                       //{
                                       //    Tasks.MoveCurrentToFirst();
                                       //}
                                   }
                               };

                               var lll = item as LinkThisTask;
                               var it = lll == null ? item.ToString() : lll.GuidOfElement;
                               if (!string.IsNullOrEmpty(it))
                               {
                                   editFocusItemInfo.Invoke(it);
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
        /// Gets the Открыть элемент с которым связана задача.
        /// </summary>
        public RelayCommand<object> OpenLinksThisTaskCommand
        {
            get
            {
                return openLinksThisTaskCommand
                       ?? (openLinksThisTaskCommand = new RelayCommand<object>(
                           item =>
                           {
                               // Открыть задачи этого элемента
                               Action openTasksOfFoc = () =>
                               {
                                   IsFocTaksVisibility = Visibility.Visible;
                                   var lastOrDefault = StaticMetods.FocsString.LastOrDefault();
                                   if (lastOrDefault != null)
                                       RefreshSelFoc(lastOrDefault.GuidOfElement);
                                   FocTasks.MoveCurrentToFirst();
                                   ShowGhostBastersMode();
                                   InterfaceActiveGroupsVisibillity = true;
                                   SelectFirstQwestTaskInFoc(true);
                               };

                               var lll = item as LinkThisTask;

                               if (lll == null || lll.NameElement != lll.ShortName)
                               {
                                   var it = lll == null ? item.ToString() : lll.GuidOfElement;

                                   if (!string.IsNullOrEmpty(it))
                                   {
                                       StaticMetods.FocsString.Add(new LinkThisTask { GuidOfElement = it });
                                   }
                                   tskSelFoc = SellectedTask;
                                   openTasksOfFoc.Invoke();
                               }
                               else
                               {
                                   StaticMetods.FocsString.Add(new LinkThisTask { GuidOfElement = lll.GuidOfElement });
                                   tskSelFoc = SellectedTask;
                                   openTasksOfFoc.Invoke();
                               }
                           },
                           item => true));
            }
        }

        /// <summary>
        /// Gets the Открыть лог.
        /// </summary>
        public RelayCommand OpenLogCommand
        {
            get
            {
                return openLogCommand
                       ?? (openLogCommand =
                           new RelayCommand(
                               () =>
                               {
                                   Messenger.Default.Send(
                                       new PopupInformationMessege(
                                           "Функция отображения выполненных задач еще не готова, ожидается в следующей версии",
                                           "Проверить новую версию",
                                           "http..."));
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Открыть завершенные задачи.
        /// </summary>
        public RelayCommand OpenLogWindowCommand
        {
            get
            {
                return openLogWindowCommand
                       ?? (openLogWindowCommand = new RelayCommand(
                           () => { },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Открыть окно - мастер создания персонажа.
        /// </summary>
        public RelayCommand OpenNewPersWizzardCommand
        {
            get
            {
                return openNewPersWizzardCommand
                       ?? (openNewPersWizzardCommand = new RelayCommand(
                           () =>
                           {
                               var mv = new MasterPageView();
                               mv.ShowDialog();

                               //var context = (MasterPageViewModel)mv.DataContext;
                               //if (context.isFinish)
                               //{
                               //    Pers = StaticMetods.PersProperty;
                               //    Pers.ToNullPers();
                               //    Restart(Pers);
                               //}
                           },
                           () => { return true; }));
            }
        }

        public RelayCommand OpenPlaningWindow
        {
            get
            {
                return openPlaningWindow ?? (openPlaningWindow = new RelayCommand(
                    () =>
                    {
                        var pv = new PlaningWindowView();
                        pv.btnOk.Click += (sender, args) =>
                        {
                            pv.Close();
                            StaticMetods.Locator.ucAbilitisVM.RefreshAbilitis();
                            foreach (var characteristic in Pers.Characteristics)
                            {
                                characteristic.RefreshRelAbs();
                            }
                        };
                        pv.ShowDialog();
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Открыть активные задачи квеста для 3-го вида.
        /// </summary>
        public RelayCommand<Aim> OpenQwestTasksCommand
        {
            get
            {
                return openQwestTasksCommand
                       ?? (openQwestTasksCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               OpenQwestActiveTasks(Pers, item, null);

                               RefreshTasksInMainView();
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
        /// Gets the Открыть быстрый вызов свойств персонажа.
        /// </summary>
        public RelayCommand<string> OpenQwickButtonCommand
        {
            get
            {
                return openQwickButtonCommand
                       ?? (openQwickButtonCommand =
                           new RelayCommand<string>(
                               item => { OpenPersWindow(new Tuple<string, string>("Окно персонажа", item)); },
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
        /// Gets the комманда Открыть статистику.
        /// </summary>
        public RelayCommand OpenStatisticCommand
        {
            get
            {
                return openStatisticCommand
                       ?? (openStatisticCommand = new RelayCommand(
                           () =>
                           {
                               var sw = new taskStatisticView
                               {
                                   DataContext = new StatisticViewModel(Pers.Tasks)
                               };
                               Messenger.Default.Send<Window>(sw);
                               sw.ShowDialog();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Открыть карту задач.
        /// </summary>
        public RelayCommand OpenTasksMapCommand
        {
            get
            {
                return openTasksMapCommand
                       ?? (openTasksMapCommand = new RelayCommand(
                           () =>
                           {
                               var tm = new TaskMap();
                               var context = (MainViewTasksMapVM)tm.MapUserControl.DataContext;
                               context.SelectedViewProperty = SelectedView;
                               context.buildGraph(context.TasksForMap.ToList());
                               tm.ShowDialog();
                               RefreshTasksInMainView();
                               OnPropertyChanged("FocusCollectionProperty");
                           },
                           () =>
                           {
                               if (isTerribleBuff(Pers))
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Gets the комманда Вставить задачу.
        /// </summary>
        public RelayCommand PasteTaskCommand
        {
            get
            {
                return pasteTaskCommand
                       ?? (pasteTaskCommand = new RelayCommand(
                           () =>
                           {
                               // Копируем задачу
                               if (IsCutTaskProperty)
                               {
                                   CopiedTaskProperty.TaskType = SellectedTask.TaskType;
                                   Pers.Tasks.Move(
                                       Pers.Tasks.IndexOf(CopiedTaskProperty),
                                       Pers.Tasks.IndexOf(SellectedTask));
                               }

                               // Переносим задачу
                               if (IsCutTaskProperty == false)
                               {
                                   var task = new Func<Task, Task>(
                                       copiTask =>
                                       {
                                           var newTask = new Task
                                           {
                                               NameOfProperty = copiTask.NameOfProperty,
                                               GUID = Guid.NewGuid().ToString(),
                                               Cvet = copiTask.Cvet,
                                               BeginDateProperty = copiTask.BeginDateProperty,
                                               DateOfDone = copiTask.DateOfDone,
                                               Recurrense = copiTask.Recurrense,
                                               SubTasks = copiTask.SubTasks,
                                               TaskContext = copiTask.TaskContext,
                                               TaskStatus = copiTask.TaskStatus,
                                               TaskType = SellectedTask.TaskType,
                                               NextActions = copiTask.NextActions
                                           };
                                           return newTask;
                                       })(CopiedTaskProperty);
                                   Pers.Tasks.Add(task);
                                   Pers.Tasks.Move(
                                       Pers.Tasks.IndexOf(task),
                                       Pers.Tasks.IndexOf(SellectedTask));
                               }

                               TaskContextVisibleProperty = false;
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// The pers.
        /// </summary>
        public Pers Pers
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
        /// Фокусировка - предыдущие задачи
        /// </summary>
        public RelayCommand PrevFocViewTasksCommand
        {
            get
            {
                return prevFocViewTasksCommand ?? (prevFocViewTasksCommand = new RelayCommand(

                    () =>
                    {
                        MoveTask(SellectedTask, TryGetTaskByIndex(Tasks.IndexOf(SellectedTask) - 1));
                        //if (LastParTask == null)
                        //{
                        //    return;
                        //}
                        //if (!Tasks.Contains(LastParTask))
                        //{
                        //    return;
                        //}

                        //var indOf = Tasks.IndexOf(LastParTask);
                        //if (indOf <= 0)
                        //{
                        //    SellectedTask = TryGetTaskByIndex(Tasks.Count - 1);
                        //    return;
                        //}

                        //SellectedTask = TryGetTaskByIndex(indOf - 1);
                    },
                    () =>
                    {
                        return true;
                    }));
            }
        }

        /// <summary>
        /// Предыдущая родительская задача
        /// </summary>
        public Task PrevLastParTask
        {
            get { return _prevLastParTask; }
            set { _prevLastParTask = value; }
        }

        /// <summary>
        /// Gets the Быстро задать дату.
        /// </summary>
        public RelayCommand<string> QwickSetDateCommand
        {
            get
            {
                return qwickSetDateCommand
                       ?? (qwickSetDateCommand = new RelayCommand<string>(
                           item =>
                           {
                               var newData = selectedDateTime;

                               if (item == "today")
                               {
                                   newData = DateTime.Today.Date;
                               }

                               if (item == "tommorow")
                               {
                                   newData = DateTime.Today.Date.AddDays(1);
                               }

                               selectedDateTime = newData;
                               Pers.updateDateString();

                               RefreshTasksInMainView();
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
        /// Gets the Быстрый выбор вида.
        /// </summary>
        public RelayCommand<string> QwickSetViewCommand
        {
            get
            {
                return qwickSetViewCommand
                       ?? (qwickSetViewCommand = new RelayCommand<string>(
                           item =>
                           {
                               // Отменяем не работающие виды отображения задач!!!
                               if (item != "2" && item != "4")
                               {
                                   item = "2";
                               }

                               switch (item)
                               {
                                   case "1":
                                       OnPropertyChanged(nameof(HeightOfDataGrid));
                                       Pers.PersSettings.ActivePersViewProperty = 1;
                                       if (SelectedView != null)
                                       {
                                           SelectedView.DefoultTaskViewProperty = 1;
                                       }
                                       break;

                                   case "2":
                                       OnPropertyChanged(nameof(HeightOfDataGrid));
                                       Pers.PersSettings.ActivePersViewProperty = 2;
                                       if (SelectedView != null)
                                       {
                                           SelectedView.DefoultTaskViewProperty = 2;
                                       }
                                       break;

                                   case "3":
                                       Pers.IsHideHUD = !Pers.IsHideHUD;
                                       OnPropertyChanged(nameof(HeightOfDataGrid));
                                       OnPropertyChanged(nameof(ExpandTaskPanelText));
                                       break;

                                   case "4":
                                       OnPropertyChanged(nameof(HeightOfDataGrid));
                                       Pers.PersSettings.ActivePersViewProperty = 4;

                                       if (SelectedView != null)
                                       {
                                           SelectedView.DefoultTaskViewProperty = 4;
                                       }

                                       break;
                               }

                               var selTask = SellectedTask;
                               //FirstFocus = ActiveQwestsWithTasks.FirstOrDefault(n => n.Tasks.Any(q => q == SellectedTask));

                               RefreshTasksInMainView();
                               SellectedTask = selTask;
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
        /// Gets the Экспорт для андроид.
        /// </summary>
        public RelayCommand RefreshSubTasksCommand
        {
            get
            {
                return refreshSubTasksCommand
                       ?? (refreshSubTasksCommand =
                           new RelayCommand(
                               () => { SellectedTask?.RefreshSubtasks(); },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets or sets the selected date time.
        /// </summary>
        public DateTime selectedDateTime
        {
            get
            {
                if (Pers.PersSettings.HOMMModeProperty)
                {
                    return Pers.DateOfLastUse.Date;
                }

                return selectedTime.Date;
            }

            set
            {
                selectedTime = value;

                Pers.LastDateOfUseProperty = selectedTime.ToString();
                OnPropertyChanged(nameof(selectedDateTime));

                // Обновляем активности квестов
                StaticMetods.RefreshAllQwests(Pers, false, true, false);

                Refresh();
                Tasks.MoveCurrentToFirst();
                SellectedTask = (Task)Tasks.CurrentItem;

                foreach (var task in Pers.Tasks)
                {
                    task.RefreshTaskEndDateForeground();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected task type.
        /// </summary>
        public TypeOfTask SelectedTaskType
        {
            get
            {
                return selectedTaskType;
            }

            set
            {
                selectedTaskType = value;
                NewTask.TaskType = selectedTaskType;
                OnPropertyChanged("NewTask");
            }
        }

        /// <summary>
        /// Gets or sets the selected view.
        /// </summary>
        public ViewsModel SelectedView
        {
            get
            {
                //return Pers.Views.FirstOrDefault();
                if (Pers.ViewForDefoult == null || !Pers.Views.Contains(Pers.ViewForDefoult))
                    Pers.ViewForDefoult = Pers.Views.FirstOrDefault();

                return Pers.ViewForDefoult;
            }

            set
            {
                if (Pers.ViewForDefoult == value)
                    return;

                FirstFocus = null;
                SellectedTask = null;
                Pers.ViewForDefoult = value;
                OnPropertyChanged("SelectedView");

                // Меняем вид отображения задач
                if (value == null)
                    return;

                //QwickSetViewCommand.Execute(
                //    value.DefoultTaskViewProperty == 0 ? 1.ToString() : value.DefoultTaskViewProperty.ToString());

                RefreshTasksInMainView();

                //Tasks.MoveCurrentToFirst();
                //SellectedTask = (Task)Tasks.CurrentItem;
            }
        }

        /// <summary>
        /// Gets the Клик по элементу из зоны фокусировки.
        /// </summary>
        public RelayCommand<string> SelectFocusCommand
        {
            get
            {
                return _selectFocusCommand
                       ?? (_selectFocusCommand = new RelayCommand<string>(
                           item =>
                           {
                               var selTask = SellectedTask;
                               var lp = LastParTask;

                               var abil = Pers.Abilitis.FirstOrDefault(n => n.GUID == item);
                               var qwest = Pers.Aims.FirstOrDefault(n => n.GUID == item);
                               var tsk = Pers.Tasks.FirstOrDefault(n => n.GUID == item);

                               if (abil != null)
                               {
                                   abil.EditAbility();
                               }
                               else if (qwest != null)
                               {
                                   StaticMetods.editAim(qwest);
                               }
                               else if (tsk != null)
                               {
                                   AlterEditTaskCommand.Execute(tsk);
                               }

                               if (abil != null || qwest != null)
                               {
                                   RefreshTasksInMainView();
                                   SellectedTask = selTask;
                                   LastParTask = lp;
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
        /// Выбранный фокус
        /// </summary>
        public FocusModel SelFoc
        {
            get
            {
                return _selFoc;
            }
            set
            {
                if (_selFoc == value) return;
                _selFoc = value;
                OnPropertyChanged(nameof(SelFoc));
                OnPropertyChanged(nameof(IsSkillsVisible));
            }
        }

        /// <summary>
        /// Кликнутая задача
        /// </summary>
        public Task SellectedTask
        {
            get
            {
                return sellectedTask;
            }

            set
            {
                if (value == null || sellectedTask == value)
                {
                    return;
                }
                sellectedTask = value;
                if (Tasks.Contains(value))
                {
                    LastParTask = value;
                }
                sellectedTask?.RefreshValuesForMainWindow();
                OnPropertyChanged(nameof(SellectedTask));
                sellectedTask?.RefreshLinkedTasksNotify();
            }
        }

        public Context SelTaskContext
        {
            get
            {
                return SellectedTask?.TaskContext;
            }
            set
            {
                if (SellectedTask == null || SellectedTask.TaskContext == value)
                {
                    return;
                }

                SellectedTask.TaskContext = value;
                OnPropertyChanged(nameof(SelTaskContext));

                RefreshTasksInMainView();
            }
        }

        /// <summary>
        /// Gets the Перенести задачу на завтра.
        /// </summary>
        public RelayCommand<Task> SendTaskToTomorowCommand
        {
            get
            {
                return sendTaskToTomorowCommand
                       ?? (sendTaskToTomorowCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.ClickPlusMinusTomorrowTask(Pers, false, true);
                               RefreshTasksInMainView();
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
        /// Gets the Быстрая установка даты (вчера, сегодня, завтра).
        /// </summary>
        public RelayCommand<string> SetDateCommand
        {
            get
            {
                return setDateCommand
                       ?? (setDateCommand = new RelayCommand<string>(
                           item =>
                           {
                               switch (item)
                               {
                                   case "вчера":
                                       selectedDateTime = DateTime.Today.AddDays(-1);
                                       break;

                                   case "сегодня":
                                       selectedDateTime = DateTime.Today;
                                       break;

                                   case "завтра":
                                       selectedDateTime = DateTime.Today.AddDays(1);
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
        /// Gets the Задать последнюю родительскую задача.
        /// </summary>
        public RelayCommand<Task> SetLastParCommand
        {
            get
            {
                return setLastParCommand
                       ?? (setLastParCommand = new RelayCommand<Task>(
                           item =>
                           {
                               PrevLastParTask = LastParTask;
                               LastParTask = item;
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
        /// Режим планирования
        /// </summary>
        public RelayCommand SetPlaningModeCommand
        {
            get
            {
                return setPlaningModeCommand ?? (setPlaningModeCommand = new RelayCommand(
                    () =>
                    {
                        // Pers.IsSetViz = !Pers.IsSetViz;
                        // IsPlaningMode = Pers.IsSetViz;

                        IsPlaningMode = !IsPlaningMode;

                        ForPriorPlaning();
                        //Tasks.Refresh();
                        RefreshTasksInMainView();
                        RefreshSellectedInFoc(LastParTask, ActiveQwests);

                        //IsPlaningMode = !IsPlaningMode;
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the Задать пресеты.
        /// </summary>
        public RelayCommand<string> SetPresetCommand
        {
            get
            {
                return setPresetCommand
                       ?? (setPresetCommand = new RelayCommand<string>(
                           item =>
                           {
                               Pers.SetPreset(Pers, item);
                               RefreshTasksInMainView();
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
        /// Gets the Задать по клику выделенную задачу.
        /// </summary>
        public RelayCommand<Task> SetSellectedTaskCommand
        {
            get
            {
                return setSellectedTaskCommand
                       ?? (setSellectedTaskCommand =
                           new RelayCommand<Task>(
                               item => { SellectedTask = item; },
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
        /// Gets the Показать характеристику.
        /// </summary>
        public RelayCommand<Characteristic> ShowCharactCommand
        {
            get
            {
                return _showCharactCommand
                       ?? (_showCharactCommand =
                           new RelayCommand<Characteristic>(
                               item => { showCharacteristic(item); },
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
        /// Gets the комманда Показать счетчики.
        /// </summary>
        public RelayCommand ShowCountersCommand
        {
            get
            {
                return showCountersCommand
                       ?? (showCountersCommand = new RelayCommand(
                           () =>
                           {
                               var cvm = new CountersViewModel(Pers.CountersCollection);
                               var ucv = new ucCountersView { DataContext = cvm };
                               ucv.ShowDialog();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Показать экран приветствия.
        /// </summary>
        public RelayCommand ShowGreetingsCommand
        {
            get
            {
                return showGreetingsCommand
                       ?? (showGreetingsCommand =
                           new RelayCommand(
                               () => { new firstViewView().ShowDialog(); },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Показывать только задачи с завершением сегодня?
        /// </summary>
        public bool ShowOnlyTodayTasks
        {
            get
            {
                return Pers.ShowOnlyTodayTasksProperty;
            }
            set
            {
                if (Pers.ShowOnlyTodayTasksProperty == value)
                {
                    return;
                }

                Pers.ShowOnlyTodayTasksProperty = value;
                RefreshTasksInMainView();
            }
        }

        /// <summary>
        /// Gets the Мягкий рестарт.
        /// </summary>
        public RelayCommand SoftBeginCommand
        {
            get
            {
                return softBeginCommand ?? (softBeginCommand = new RelayCommand(
                    () =>
                    {
                        var hh = MessageBox.Show(
                            "Вы уверены, что хотите очистить все достижения с сохранением задач и квестов?",
                            "Начать все с начала",
                            MessageBoxButton.OKCancel);
                        if (hh == MessageBoxResult.OK)
                        {
                            // Удаляем все задачи
                            LetSBegin(true);
                        }
                    },
                    () => true));
            }
        }

        public RelayCommand SortByPriorityCommand
        {
            get
            {
                return sortByPriorityCommand ?? (sortByPriorityCommand = new RelayCommand(

                    () =>
                    {
                        if (Pers.IsParetto)
                        {
                            Pers.IsParetto = false;
                            Pers.IsSortByPrioryty = false;

                            Pers.IsSortByBalance = false;
                        }
                        else if (Pers.IsSortByPrioryty)
                        {
                            Pers.IsSortByPrioryty = false;
                            Pers.IsParetto = true;

                            Pers.IsSortByBalance = false;
                        }
                        else
                        {
                            Pers.IsSortByPrioryty = true;
                            Pers.IsParetto = false;

                            Pers.IsSortByBalance = false;
                        }

                        //Pers.IsSortByPrioryty = !Pers.IsSortByPrioryty;

                        ForPriorPlaning();
                        OnPropertyChanged(nameof(WhatSort));
                        //reorderTasks();
                        RefreshTasksInMainView();
                    },
                    () => true));
            }
        }

        public List<StatusTask> Statuses => Pers.VisibleStatuses;

        /// <summary>
        /// Sets and gets Видимость контекстного меню для задач.. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool TaskContextVisibleProperty
        {
            get
            {
                return taskContextVisible;
            }

            set
            {
                if (taskContextVisible == value)
                {
                    return;
                }

                taskContextVisible = value;
                OnPropertyChanged("TaskContextVisibleProperty");
            }
        }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        public ListCollectionView Tasks { get; set; }

        public int TasksCount
        {
            get
            {
                return _tasksCount;
            }
            set
            {
                if (_tasksCount == value) return;
                _tasksCount = value;
                OnPropertyChanged(nameof(TasksCount));
            }
        }

        /// <summary>
        /// Задачи для вида фокусировки
        /// </summary>
        public List<Task> TasksForFoc
        {
            get
            {
                return _tasksForFoc;
            }
            set
            {
                if (_tasksForFoc == value) return;
                _tasksForFoc = value;
                OnPropertyChanged(nameof(TasksForFoc));
            }
        }

        public ObservableCollection<ElementForInterface> TasksForInterfacePanel { get; set; } = new ObservableCollection<ElementForInterface>();

        /// <summary>
        /// Sets and gets Число колонок в униформгриде задач. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public int TasksNumOfColumnProperty
        {
            get
            {
                return _tasksNumOfColumn;
            }

            set
            {
                if (_tasksNumOfColumn == value)
                {
                    return;
                }

                _tasksNumOfColumn = value;
                OnPropertyChanged(nameof(TasksNumOfColumnProperty));
            }
        }


        /// <summary>
        /// Открыть просмотр характеристики.
        /// </summary>
        private RelayCommand<Characteristic> openChaCommand;

        /// <summary>
        /// Gets the Открыть просмотр характеристики.
        /// </summary>
        public RelayCommand<Characteristic> OpenChaCommand
        {
            get
            {
                return openChaCommand
                       ?? (openChaCommand = new RelayCommand<Characteristic>(
                           item =>
                           {
                               item.EditCharacteristic();
                               RefreshTasksInMainView();
                           },
                           item =>
                           {
                               if (item == null)
                                   return false;

                               return true;
                           }));
            }
        }


        /// <summary>
        /// Gets the Создать квест на основе задачи.
        /// </summary>
        public RelayCommand<Task> TaskToQwestCommand
        {
            get
            {
                return taskToQwestCommand
                       ?? (taskToQwestCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.ToQwest(Pers);
                               Tasks.Remove(item);
                               RefreshTasksInMainView();
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
        /// Gets the Пауза в таймере.
        /// </summary>
        public RelayCommand<Task> TimerPauseCommand
        {
            get
            {
                return timerPauseCommand
                       ?? (timerPauseCommand =
                           new RelayCommand<Task>(
                               item => { item.TimerPause(); },
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
        /// Gets the Запуск таймера.
        /// </summary>
        public RelayCommand<Task> TimerStartCommand
        {
            get
            {
                return timerStartCommand
                       ?? (timerStartCommand =
                           new RelayCommand<Task>(
                               item => { item.TimerStart(); },
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
        /// Gets the остановка таймера.
        /// </summary>
        public RelayCommand<Task> TimerStopCommand
        {
            get
            {
                return timerStopCommand
                       ?? (timerStopCommand =
                           new RelayCommand<Task>(
                               item => { item.TimerStop(); },
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
        /// Sets and gets Вьюмодель для активных целей. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ucMainAimsViewModel UcMainAimsVmProperty
        {
            get
            {
                return ucMainAimsVM;
            }

            set
            {
                if (ucMainAimsVM == value)
                {
                    return;
                }

                ucMainAimsVM = value;
                OnPropertyChanged("UcMainAimsVmProperty");
            }
        }

        /// <summary>
        /// Gets the Эту и следующие все вверх.
        /// </summary>
        public RelayCommand<int> WaveUpCommand
        {
            get
            {
                return waveUpCommand
                       ?? (waveUpCommand = new RelayCommand<int>(
                           item =>
                           {
                               var t = Tasks.Cast<Task>().Where(n => n.Wave > item).ToList();
                               foreach (var source in t)
                               {
                                   source.Wave++;
                               }
                               //Tasks.MoveCurrentTo(item);
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
        /// Gets the Этой и всем вниз.
        /// </summary>
        public RelayCommand<int> WaweDownCommand
        {
            get
            {
                return waweDownCommand
                       ?? (waweDownCommand = new RelayCommand<int>(
                           item =>
                           {
                               var t = Tasks.Cast<Task>().Where(n => n.Wave >= item).ToList();
                               foreach (var source in t)
                               {
                                   source.Wave++;
                               }
                               //Tasks.MoveCurrentTo(item);
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

        public string WhatSort
        {
            get
            {
                return "План";
                //if (Pers.IsParetto)
                //{
                //    return "Баланс";
                //}
                //else if (Pers.IsSortByPrioryty)
                //{
                //    return "Приоритет";
                //}
                //else
                //{
                //    return "План";
                //}
            }
        }

        /// <summary>
        /// Асинхронное сохранение данных
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        public static void AsinchSaveData(Pers _pers)
        {
            //if (_pers.IsAutosaveProperty == false)
            //{
            //    return;
            //}

            //System.Threading.Tasks.Task.Factory.StartNew(
            //    () =>
            //    {
            //        SaveData(
            //            Path.Combine(
            //                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            //                Resources.NameOfTheGame),
            //            _pers);
            //    });
        }

        /// <summary>
        /// Получить ранг скилла по уровню
        /// </summary>
        /// <param name="abil">скилл</param>
        /// <param name="levelAfter">уровень</param>
        /// <param name="rang">ранг</param>
        public static void getAbilRang(AbilitiModel abil, int levelAfter, out string rang)
        {
            var lastOrDefault =
                abil.Rangs.Where(n => n.LevelRang <= levelAfter).OrderBy(n => n.LevelRang).LastOrDefault();
            rang = lastOrDefault != null ? lastOrDefault.NameOfRang : string.Empty;
        }

        /// <summary>
        /// Получить ранг характеристики по уровню
        /// </summary>
        /// <param name="cha">характеристика</param>
        /// <param name="levelAfter">уровень</param>
        /// <param name="rang">ранг</param>
        public static void getChaRang(Characteristic cha, int levelAfter, out string rang)
        {
            var lastOrDefault =
                cha.Rangs.Where(n => n.LevelRang <= levelAfter).OrderBy(n => n.LevelRang).LastOrDefault();
            rang = lastOrDefault != null ? lastOrDefault.NameOfRang : string.Empty;
        }

        /// <summary>
        /// The get days of week collection.
        /// </summary>
        /// <returns>The /&gt;.</returns>
        public static ObservableCollection<DaysOfWeekRepeat> GetDaysOfWeekCollection()
        {
            var daysOfWeekCollection = new ObservableCollection<DaysOfWeekRepeat>
            {
                new DaysOfWeekRepeat
                {
                    Day
                        =
                        DayOfWeek
                            .Monday,
                    NameDayOfWeekProperty
                        =
                        "Пн",
                    CheckedProperty
                        =
                        false
                },
                new DaysOfWeekRepeat
                {
                    Day
                        =
                        DayOfWeek
                            .Tuesday,
                    NameDayOfWeekProperty
                        =
                        "Вт",
                    CheckedProperty
                        =
                        false
                },
                new DaysOfWeekRepeat
                {
                    Day
                        =
                        DayOfWeek
                            .Wednesday,
                    NameDayOfWeekProperty
                        =
                        "Ср",
                    CheckedProperty
                        =
                        false
                },
                new DaysOfWeekRepeat
                {
                    Day
                        =
                        DayOfWeek
                            .Thursday,
                    NameDayOfWeekProperty
                        =
                        "Чт",
                    CheckedProperty
                        =
                        false
                },
                new DaysOfWeekRepeat
                {
                    Day
                        =
                        DayOfWeek
                            .Friday,
                    NameDayOfWeekProperty
                        =
                        "Пт",
                    CheckedProperty
                        =
                        false
                },
                new DaysOfWeekRepeat
                {
                    Day
                        =
                        DayOfWeek
                            .Saturday,
                    NameDayOfWeekProperty
                        =
                        "Сб",
                    CheckedProperty
                        =
                        false
                },
                new DaysOfWeekRepeat
                {
                    Day
                        =
                        DayOfWeek
                            .Sunday,
                    NameDayOfWeekProperty
                        =
                        "Вс",
                    CheckedProperty
                        =
                        false
                }
            };
            return daysOfWeekCollection;
        }

        /// <summary>
        /// Получаем ранги по умолчанию для скиллов
        /// </summary>
        /// <param name="abilRangsDefoult">The abil Rangs Defoult.</param>
        /// <returns>/&gt;.</returns>
        public static Func<ObservableCollection<Rangs>> GetDefoultRangsForAbilitis(
            ObservableCollection<Rangs> abilRangsDefoult)
        {
            return () =>
            {
                var rangs = new ObservableCollection<Rangs>();
                foreach (var rangse in abilRangsDefoult)
                {
                    rangs.Add(new Rangs { LevelRang = rangse.LevelRang, NameOfRang = rangse.NameOfRang });
                }

                return rangs;
            };
        }

        /// <summary>
        /// Расчет коэффициента для ФУДЖ режима
        /// </summary>
        /// <param name="abilMaxLevelProperty">Максимальный уровень скилла</param>
        /// <param name="taskCountDefoultPrivichkaProperty">
        /// Сколько раз должна быть выполнена задача для развития привычки
        /// </param>
        /// <returns></returns>
        public static double getKAbToExp(int abilMaxLevelProperty, int taskCountDefoultPrivichkaProperty)
        {
            var maxAbValue = Pers.ExpToLevel(abilMaxLevelProperty, RpgItemsTypes.ability);

            var oneDo = maxAbValue / Convert.ToDouble(taskCountDefoultPrivichkaProperty);

            var kRel = 30.0 / oneDo;

            return kRel;
        }

        public static List<Task> GetNotDonnedTodayTasks()
        {
            return StaticMetods.PersProperty.Tasks.Where(n => n.IsDelProperty == false)
                .Where(
                    n =>
                        n.Recurrense.TypeInterval != TimeIntervals.Нет &&
                        n.Recurrense.TypeInterval != TimeIntervals.Сразу)
                .Where(n => IsTaskVisibleInCurrentView(n, null, StaticMetods.PersProperty, false, true, false, true))
                .OrderBy(n => n)
                .ToList();
        }

        /// <summary>
        /// The get pers frome template.
        /// </summary>
        /// <param name="combine">The combine.</param>
        /// <returns>The <see cref="Pers"/>.</returns>
        public static Pers GetPersFromeTemplate(string combine)
        {
            using (var fs = new FileStream(combine, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Pers)formatter.Deserialize(fs);
            }
        }

        /// <summary>
        /// Вид окна персонажа
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        public static void GetPersSettingsView(Pers _pers)
        {
            // Сообщения
            Messenger.Default.Send(Visibility.Collapsed);
            Messenger.Default.Send(
                new Func<ObservableCollection<Aim>, Aim>(
                    aims =>
                        aims.Where(
                            n =>
                                n.IsDoneProperty == false
                                && n.MinLevelProperty <= StaticMetods.GetLevel(_pers.PersExpProperty, RpgItemsTypes.exp))
                            .OrderBy(n => n.GoldIfDoneProperty)
                            .ThenBy(q => q.MinLevelProperty)
                            .ThenBy(n => n.NameOfProperty)
                            .FirstOrDefault()).Invoke(_pers.Aims));
            StaticMetods.RefreshAllQwests(_pers, false, true, false);
            Messenger.Default.Send("Обновить настройки персонажа!");
        }

        /// <summary>
        /// А есть ли у задачи предыдущие действия, для каких она является следующей?
        /// </summary>
        /// <param name="task">задача</param>
        /// <returns>список предыдущих действий</returns>
        public static List<Task> GetPreviewsTasks(
            Task task)
        {
            var previewTasks = StaticMetods.GetPrevActionsForTask(task);

            return previewTasks.ToList();
        }

        /// <summary>
        /// Из строки в дату с милисекундами
        /// </summary>
        /// <param name="dateOfClick"></param>
        /// <returns></returns>
        public static DateTime GetTimeFromeStringMiliseconds(string dateOfClick)
        {
            DateTime lastClick = DateTime.MinValue;
            DateTime.TryParseExact(dateOfClick, "yyyy/MM/dd. HH:mm:ss:FFF", null,
                DateTimeStyles.None, out lastClick);
            return lastClick;
        }

        /// <summary>
        /// Из даты в строку с милисекундами
        /// </summary>
        /// <param name="dateOfClick"></param>
        /// <returns></returns>
        public static string GetTimeStringMilisecondsFromeDate(DateTime dateOfClick)
        {
            return dateOfClick.ToString("yyyy/MM/dd. HH:mm:ss:FFF");
        }

        public static Func<DateTime, bool> IsDay()
        {
            Func<DateTime, bool> isDay = task =>
            {
                if (task.Hour >= 12 && task.Hour < 18)
                {
                    return true;
                }
                return false;
            };
            return isDay;
        }

        public static Func<DateTime, bool> IsEvening()
        {
            Func<DateTime, bool> isEvening = task =>
            {
                if (task.Hour >= 18 && task.Hour <= 23 && task.Minute < 59)
                {
                    return true;
                }
                return false;
            };
            return isEvening;
        }

        public static Func<DateTime, bool> IsMorning()
        {
            Func<DateTime, bool> isMorning = task =>
            {
                if (task.Hour < 12)
                {
                    return true;
                }
                return false;
            };
            return isMorning;
        }

        /// <summary>
        /// The is task visible in current view.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="selView">Выбранный вид</param>
        /// <param name="pers">Персонаж</param>
        /// <param name="showNextActions">The show next actions.</param>
        /// <param name="ignoreSellectedView">Игнорировать выбранный вид - выбираются</param>
        /// <param name="ignoreDate">Игнорировать дату начала и дату завершения</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsTaskVisibleInCurrentView(
            Task task,
            ViewsModel selView,
            Pers pers,
            bool showNextActions = false,
            bool ignoreSellectedView = false, bool ignoreDate = false, bool ignoreStatus = false)
        {
            if (task == null || task.IsDelProperty)
                return false;

            var req = "";

            // Если стоит галочка - только завершающиеся сегодня задачи
            if (pers.ShowOnlyTodayTasksProperty)
                if (task.EndDate > selectedTime)
                    return false;

            if (ignoreDate == false)
                if (IsTaskTodayEnabled(task) == false)
                    return false;

            // Проверка по видимости в текущем виде
            if (!ignoreSellectedView)
                if (GetRightRels(task) == null)
                    return false;

            // Проверка по активности скиллов и квестов
            var inAbQwests = IsTaskActiveInAnyNeeds(task, ref req);
            if (inAbQwests)
                return false;

            return true;
        }

        /// <summary>
        /// Активен страшный бафф?
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        /// <returns>Активен баф?</returns>
        public static bool isTerribleBuff(Pers _pers)
        {
            return false;

            //var isTerBuff = false;

            //if (_pers.PersSettings.IsHPActiveteProperty && _pers.PersSettings.IsTerribleDebufHPProperty)
            //{
            //    var twentiFive = Convert.ToInt32(Convert.ToDouble(_pers.HPProperty.MaxHPProperty) * 0.25);
            //    if (_pers.HPProperty.CurrentHPProperty < twentiFive)
            //    {
            //        isTerBuff = true;
            //    }
            //}

            //if (_pers.PersSettings.EnableNeednessProperty)
            //{
            //    if (_pers.NeednessCollection.Any(n => Math.Abs(n.ValueOfNeednessProperty) < 0.001))
            //    {
            //        isTerBuff = true;
            //    }
            //}

            //return isTerBuff;
        }

        /// <summary>
        /// Выполненна ли задача на данный момент?
        /// </summary>
        /// <param name="task">Задача</param>
        /// <returns>Задача выполнена?</returns>
        public static bool isThisTaskDone(Task task)
        {
            if (task.IsDelProperty)
            {
                return true;
            }

            switch (task.Recurrense.TypeInterval)
            {
                case TimeIntervals.Сразу:
                    return task.IsDelProperty;

                case TimeIntervals.Нет:
                    return task.IsDelProperty;
            }

            var dateOfBegin = task.BeginDateProperty;

            var sellectedTime = selectedTime;

            return dateOfBegin > sellectedTime;
        }

        /// <summary>
        /// Открыть активные задачи скилла
        /// </summary>
        /// <param name="pers">Перс</param>
        /// <param name="ab">скилл</param>
        /// <param name="nextAbs"></param>
        public static void OpenActiveAbTasks(Pers pers, AbilitiModel ab, List<FocusModel> nextAbs = null)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt)
            {
                ab.ImageProperty = StaticMetods.GetPathToImage(ab.ImageProperty);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                ab.EditAbility();
            }
            else
            {
                var activeAbTasks = new ActiveAbTasksWindow();
                var context = StaticMetods.Locator.ActiveAbTasksVM;
                context.SelectedAbilityProperty = ab;

                if (nextAbs != null)
                {
                    var prevN = nextAbs.Join(pers.Abilitis, n => n.IdProperty, q => q.GUID,
                        (model, abilitiModel) => abilitiModel).ToList();

                    context.SelectedAbilityProperty.PrevNextAbProperty = prevN;
                }

                Messenger.Default.Send<Window>(activeAbTasks);

                activeAbTasks.ShowDialog();
            }
        }

        /// <summary>
        /// Метод для того, чтобы открыть ссылку
        /// </summary>
        /// <param name="item">путь к ссылке</param>
        public static void OpenLink(string item)
        {
            try
            {
                switch (item)
                {
                    case "???Инструкция???":
                        item = Path.Combine(App.InstrPath);
                        break;
                }

                Process.Start(item);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Открыть активные задачи квеста из фокусировки
        /// </summary>
        /// <param name="pers">Перс</param>
        /// <param name="qwest">Квест</param>
        /// <param name="nextQwests"></param>
        public static void OpenQwestActiveTasks(Pers pers, Aim qwest, List<FocusModel> nextQwests)
        {
            pers.SellectedAimProperty = qwest;
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                pers.SellectedAimProperty.TasksToTommorow();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Alt)
            {
                pers.SellectedAimProperty.ImageProperty =
                    StaticMetods.GetPathToImage(pers.SellectedAimProperty.ImageProperty);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                StaticMetods.editAim(qwest);
            }
            else
            {
                var atWindow = new ActiveQwestTasksWindow();

                if (nextQwests != null)
                {
                    var qwL =
                        nextQwests.Join(pers.Aims, n => n.IdProperty, q => q.GUID, (model, aim) => aim).ToList();
                    pers.SellectedAimProperty.PrevNextQwestsProperty = qwL;
                }

                Messenger.Default.Send<Window>(atWindow);
                atWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Ранги для характеристики по умолчанию
        /// </summary>
        /// <param name="chaRangses">The cha Rangses.</param>
        /// <returns>The.</returns>
        public static Func<ObservableCollection<Rangs>> RangsForCharacteristucDefoult(
            ObservableCollection<Rangs> chaRangses)
        {
            return () =>
            {
                var rangs = new ObservableCollection<Rangs>();
                foreach (var rangse in chaRangses)
                {
                    rangs.Add(new Rangs { LevelRang = rangse.LevelRang, NameOfRang = rangse.NameOfRang });
                }

                return rangs;
            };
        }

        /// <summary>
        /// The restart.
        /// </summary>
        public static void Restart(Pers _pers)
        {
            SaveData(
                Settings.Default.PathToPers,
                _pers);

            try
            {
                Process.Start(Application.ResourceAssembly.Location);
                Environment.Exit(0);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <param name="appFolder">Папка куда сохранять</param>
        /// <param name="_pers">Персонаж</param>
        public static void SaveData(string appFolder, Pers _pers)
        {
            if (Directory.Exists(appFolder) == false)
            {
                Directory.CreateDirectory(appFolder);
            }

            // Сохраняем персонажа с его задачами
            _pers.SetDateOfLastsave();
            var pathToPers = Path.Combine(appFolder, FilesToLoadSave.Pers.ToString());
            using (var fs = new FileStream(pathToPers, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, _pers);
            }

            Settings.Default.Save();
        }

        /// <summary>
        /// Записываем лог кто что сделал
        /// </summary>
        /// <param name="whatDo"></param>
        public static void SaveLogTxt(string whatDo)
        {
            var appFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Resources.NameOfTheGame);

            if (Directory.Exists(appFolder) == false)
            {
                Directory.CreateDirectory(appFolder);
            }

            using (var br = new StreamWriter(Path.Combine(appFolder, "log.txt"), true))
            {
                br.WriteLine(whatDo + ";");
            }
        }

        /// <summary>
        /// The add new task command execute.
        /// </summary>
        /// <param name="_type">The _type.</param>
        public void AddNewTaskCommandExecute(TypeOfTask _type)
        {
            var addTask = Task.AddTask(_type);
            if (addTask.Item1)
            {
                RefreshTasksInMainView();
            }
        }

        /// <summary>
        /// Пробуем загрузить из андроида
        /// </summary>
        public void AutoLoadFromeAndro()
        {
            var pathToExpImpFolder = PathToAndroidExpImp;

            try
            {
                if (Directory.Exists(pathToExpImpFolder))
                {
                    var files =
                        new DirectoryInfo(pathToExpImpFolder).GetFiles()
                            .Where(n => n.Name == "FromePhone_RpgOrganizer.json")
                            .ToList();
                    if (files.Any())
                    {
                        var file = files.FirstOrDefault(n => n.Name == "FromePhone_RpgOrganizer" + ".json");
                        if (file != null)
                        {
                            ImportFromAndroid(file.FullName, true);
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("При синхронизации что-то пошло не так!");
            }
        }

        /// <summary>
        /// The clear log.
        /// </summary>
        public void ClearLog()
        {
        }

        /// <summary>
        /// Настройка колонок
        /// </summary>
        public void ColumnsSettings()
        {
            var csvm = new ColumnsSettingsViewModel(Pers);
            var csv = new ColumnsSettingsView { DataContext = csvm };
            csv.buttonOk.Click += (e, args) => csv.Close();

            // SaveLogTxt("Настраиваем столбцы");
            Messenger.Default.Send<Window>(csv);
            csv.ShowDialog();

            Messenger.Default.Send("Обновить уровни!");
            Messenger.Default.Send("ПлюсМинусНажат!");

            OnPropertyChanged("PersTypeOfTasks");
            OnPropertyChanged("Pers.Statuses");
            OnPropertyChanged("Pers.Contexts");
            // this.TaskSTypes.Refresh();
            AsinchSaveData(Pers);
            RefreshTasksInMainView();

            // this.RefreshAll();
        }

        /// <summary>
        /// Выход
        /// </summary>
        public void Exit()
        {
            // Останавливаем все таймеры
            foreach (var task in Pers.Tasks)
            {
                task.TimerStop();
                task.PomodorroTimerStop();
            }

            if (!Pers.PersSettings.DisableSounds)
            {
                StaticMetods.NotificationSound.Stream = Resources.doorClose;
                StaticMetods.NotificationSound.PlaySync();
            }

            Pers.DateLastUseProgram = DateTime.Now;
            var pathToExpImpFolder = Pers.PersSettings.PathToExpImpFolder;
            if (!string.IsNullOrWhiteSpace(Settings.Default.PathToDropBox) && Directory.Exists(Settings.Default.PathToDropBox))
            {
                if (!Directory.Exists(PathToAndroidExpImp))
                {
                    Directory.CreateDirectory(PathToAndroidExpImp);
                }
                ExportToAndroid(PathToAndroidExpImp);
            }
            SaveData(Settings.Default.PathToPers,
                Pers);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Все задачи для скилла
        /// </summary>
        /// <param name="abil"></param>
        /// <param name="abFocusItem"></param>
        /// <returns></returns>
        public List<Task> GetAllTasksToAbility(AbilitiModel abil, ref FocusModel abFocusItem)
        {
            abFocusItem.SubFocusItems = new List<FocusModel>();
            abFocusItem.SubFocusItems.Add(GetItemToFocus(abil, Colors.White, Visibility.Collapsed, true));

            var allTsks = new List<Task>();

            foreach (
                var task in
                    abil.SkillsActive.Where(n => IsTaskVisibleInCurrentView(n, SelectedView, Pers)).OrderBy(n => n))
            {
                var tsks = new List<Task>();

                foreach (var aim in Pers.Aims.Where(n => n.IsActiveProperty).Where(n => n.Spells.Any(q => q == task)))
                {
                    tsks.AddRange(aim.NeedsTasks
                        .Where(n => IsTaskVisibleInCurrentView(n.TaskProperty, null, Pers, false, true, false, true))
                        .Select(n => n.TaskProperty).OrderBy(n => n));

                    abFocusItem.SubFocusItems.Add(GetItemToFocus(aim, Colors.White, Visibility.Collapsed, true));
                }

                allTsks.Add(task);
                allTsks.AddRange(tsks.OrderBy(n => n));
            }

            return allTsks.Distinct().ToList();
        }

        /// <summary>
        /// Получить дату последнего клика в андроиде
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public DateTime GetTimeOfClick(string s)
        {
            DateTime dateAndro;
            DateTime.TryParseExact(s, "yyyy/MM/dd. HH:mm:ss:SSS", null, System.Globalization.DateTimeStyles.None, out dateAndro);
            return dateAndro;
        }

        /// <summary>
        /// Начать все с начала
        /// </summary>
        public void LetSBegin(bool isSoftBegin = false)
        {
            if (!isSoftBegin)
            {
                // Удаляем все задачи
                foreach (var task in Pers.Tasks.ToList())
                {
                    task.Delete(Pers);
                }

                // Удаляем все квесты
                foreach (var aim in Pers.Aims.ToList())
                {
                    StaticMetods.RemoveQwest(Pers, aim, false);
                }
            }

            // Обнуляем золото
            Pers.GoldProperty = 0;

            // Обнуляем инвентарь
            Pers.InventoryItems.Clear();
            //Pers.ShopItems.Clear();
            foreach (var source in Pers.ShopItems.Where(n => n.IsArtefact && !n.IsBaige).ToList())
            {
                Pers.ShopItems.Remove(source);
            }

            // Обнуляем скиллы
            foreach (var abilitiModel in Pers.Abilitis)
            {
                abilitiModel.FirstVal = 0;
                abilitiModel.ValueProperty = 0;
                abilitiModel.CellValue = 0;

                // Обнуляем опыт в скиллах
                abilitiModel._experiance = 0;
                //abilitiModel.MaxValue = 0;

                if (!isSoftBegin)
                {
                    abilitiModel.NeedTasks.Clear();
                    abilitiModel.NeedAims.Clear();
                    abilitiModel.ReqwireAims.Clear();
                }

                abilitiModel.ClearValue();
                abilitiModel.NeedLevelForDefoult = abilitiModel.MaxLevelProperty - 1;
            }

            StaticMetods.PersProperty = Pers;
            selectedDateTime = DateTime.Now.Date;

            foreach (var abilitiModel in Pers.Abilitis)
            {
                abilitiModel.IsPayedProperty = false;
                abilitiModel.RefreshEnabled();
            }

            foreach (var cha in Pers.Characteristics)
            {
                cha.RecountChaValue();
            }

            StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.СНачала, null);
            Pers.ToNullPers(isSoftBegin);

            Restart(Pers);
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        public void LoadPers()
        {
            var of = new OpenFileDialog();
            of.ShowDialog();
            var pathToSave = of.FileName;
            StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.ПолностьюСНачала, null);
            LoadPersFromPath(pathToSave);
        }

        public void LoadPersFromPath(string pathToSave)
        {
            var perrs = Pers.LoadPers(pathToSave);
            if (perrs == null)
            {
                return;
            }

            Pers = perrs;
            Pers.LastDateOfUseProperty = DateTime.Now.ToString();
            Pers.ViewForDefoult = Pers.Views.FirstOrDefault();
            StaticMetods.PersProperty = Pers;

            if (Pers != null)
            {
                Restart(Pers);
            }
        }

        /// <summary>
        /// Сдвинуть задачи
        /// </summary>
        /// <param name="taskA">Первая</param>
        /// <param name="taskB">Вторая</param>
        /// <param name="notRefresh">Не обновлять задачи в главном окне</param>
        public void MoveTask(Task taskA, Task taskB, bool notRefresh = false, bool ignorePlan = false)
        {
            if (!Pers.IsFocTasks && !Pers.IsSetViz)
            {
                return;
            }

            //int i1 = Tasks.IndexOf(taskA);
            //int i2 = Tasks.IndexOf(taskB);

            //Tasks.EditItem(taskA);
            ////taskA.MiliSecsDoneForSort = taskB.MiliSecsDoneForSort;
            ////taskA.TimeLastDone = taskB.TimeLastDone;
            //Tasks.CommitEdit();

            //if (i1 < 0 && i2 < 0)
            //{
            //    var ind1 = Pers.Tasks.IndexOf(taskA);
            //    var ind2 = Pers.Tasks.IndexOf(taskB);
            //    Pers.Tasks.Move(ind1, ind2);
            //}
            //else if (i2 < i1)
            //{
            //    // Тянем вверх
            //    var ind1 = Pers.Tasks.IndexOf(taskA);
            //    var ind2 = Pers.Tasks.IndexOf(taskB);
            //    if (ind2>ind1)
            //    {
            //        var tmp = ind2;
            //        ind2 = ind1;
            //        ind1 = tmp;
            //    }

            // Pers.Tasks.Move(ind1, ind2);

            //    var indA = Pers.Tasks.IndexOf(taskA);
            //    var indB = Pers.Tasks.IndexOf(taskB);
            //    if (indB<indA)
            //    {
            //        Pers.Tasks.Move(indA, indB);
            //    }
            //}
            //else
            //{
            //    // Тянем вниз
            //    // Тянем вверх
            //    var ind1 = Pers.Tasks.IndexOf(taskA);
            //    var ind2 = Pers.Tasks.IndexOf(taskB);
            //    if (ind1 > ind2)
            //    {
            //        var tmp = ind2;
            //        ind2 = ind1;
            //        ind1 = tmp;
            //    }

            // Pers.Tasks.Move(ind1, ind2);

            //    var indA = Pers.Tasks.IndexOf(taskA);
            //    var indB = Pers.Tasks.IndexOf(taskB);
            //    if (indB > indA)
            //    {
            //        Pers.Tasks.Move(indA, indB);
            //    }
            //}

            //Tasks.EditItem(taskA);
            //taskA.TimeLastDone = taskB.TimeLastDone;
            //Tasks.CommitEdit();

            var ind1 = Pers.Tasks.IndexOf(taskA);
            var ind2 = Pers.Tasks.IndexOf(taskB);
            // taskA.Wave = taskB.Wave;
            //if (ind2 < ind1)
            //{
            //    ind2 = ind2 - 1;
            //}
            //else
            //{
            //    ind2 = ind2 + 1;
            //}
            Pers.Tasks.Move(ind1, ind2);

            if (!notRefresh)
            {
                RefreshTasksInMainView();
                Tasks.EditItem(taskA);
                Tasks.CommitEdit();
                Tasks.EditItem(taskB);
                Tasks.CommitEdit();

                //FocTasks.EditItem(taskA);
                //FocTasks.CommitEdit();
                //FocTasks.EditItem(taskB);
                //FocTasks.CommitEdit();
            }

            Tasks.MoveCurrentTo(taskA);
            LastParTask = taskA;
            SellectedTask = taskA;
        }

        /// <summary>
        /// Открываем окно персонажа
        /// </summary>
        /// <param name="whatTabOpen"></param>
        /// <param name="plusSendMessege"></param>
        public void OpenPersWindow(Tuple<string, string> whatTabOpen, string plusSendMessege = null)
        {
            if (isTerribleBuff(Pers))
            {
                return;
            }

            StaticMetods.PlaySound(Resources.openDoor);

            var qCollectionViewProperty = StaticMetods.Locator.AimsVM.QCollectionViewProperty;
            qCollectionViewProperty.Refresh();
            StaticMetods.Locator.AimsVM.SelectedAimProperty = qCollectionViewProperty.Cast<Aim>().FirstOrDefault();

            //var s = new Style();
            //s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));

            var _psv = new PersSettingsView();
            Messenger.Default.Send("Обновить информацию!");

            if (whatTabOpen.Item2 == "Квесты")
            {
                openQwestsWindow(whatTabOpen);
            }
            else if (whatTabOpen.Item2 == "Арсенал")
            {
                Messenger.Default.Send(whatTabOpen);
            }
            else
            {
                Messenger.Default.Send(whatTabOpen);
            }

            if (string.IsNullOrEmpty(plusSendMessege) == false)
            {
                Messenger.Default.Send(plusSendMessege);
            }

            //_psv.Dispatcher.BeginInvoke(new Action(() => _psv.ShowDialog()), DispatcherPriority.ContextIdle, null);

            _psv.ShowDialog();
            Messenger.Default.Send("Обновить уровни!");
            Messenger.Default.Send("ПлюсМинусНажат!");

            RefreshTasksInMainView();
        }

        /// <summary>
        /// The open view settings.
        /// </summary>
        public void OpenViewSettings()
        {
            var vv = new ViewsViewModel(
                Pers.Views,
                Pers.TasksTypes,
                Pers.Statuses,
                Pers.Contexts);
            var vvv = new ViewsView { DataContext = vv };

            // SaveLogTxt("Настраиваем виды");
            Messenger.Default.Send<Window>(vvv);
            vvv.ShowDialog();

            SelectedView = SelectedView;

            AsinchSaveData(Pers);

            RefreshTasksInMainView();
        }

        /// <summary>
        /// Обновляем отображение свойств и задач персонажа
        /// </summary>
        public void Refresh()
        {
            viewLevelRefresh();
            UcMainAimsVmProperty = new ucMainAimsViewModel(Pers.Aims);
            OnPropertyChanged("PersNameProperty");
            RefreshTasksInMainView();
            Tasks.MoveCurrentToFirst();
        }

        public void RefreshRelations()
        {
            foreach (var task in Pers.Tasks)
            {
                task.TaskInAbilitis = new List<AbilitiModel>();

                if (TaskLinkDic.ContainsKey(task))
                    if(TaskLinkDic[task].Ability != null)
                        task.TaskInAbilitis.Add(TaskLinkDic[task].Ability);
            }
        }

        public void RefreshSellectedInFoc(Task tsk, List<FocusModel> interFoc)
        {
            if (!Pers.IsPlanningModeMain || tsk == null)
                return;

            List<FocusModel> fm = new List<FocusModel>();

            var ab = Pers.Abilitis.FirstOrDefault(n => n.NeedTasks.Any(q => q.TaskProperty == tsk));
            var notDoneQwests = Pers.Aims.Where(n => n.IsDoneProperty == false).ToList();

            if(!StaticMetods.PersProperty.PersSettings.IsNoAbs && ab != null)
                fm.Add(new FocusModel() { IdProperty = tsk.GUID, ImageProperty = StaticMetods.pathToImageProperty(tsk.PathToIm), ToolTippProperty = tsk.NameOfProperty });

            foreach (var qw in notDoneQwests.Where(n => n.NeedsTasks.Any(q => q.TaskProperty == tsk)).ToList())
            {
                fm.Add(new FocusModel()
                {
                    IdProperty = qw.GUID,
                    ImageProperty = qw.PictureProperty,
                    ToolTippProperty = qw.NameOfProperty + " (Квест)"
                });
            }

            if (ab != null)
            {
                FocusModel item = new FocusModel() { IdProperty = ab.GUID, ImageProperty = ab.PictureProperty, ToolTippProperty = ab.NameOfProperty };

                if (!Pers.PersSettings.IsNoAbs)
                    item.ToolTippProperty = item.ToolTippProperty + " (Навык)";

                fm.Add(item);

                foreach (var qw in notDoneQwests.Where(n => n.AbilitiLinksOf.Any(q => q == ab)).ToList())
                {
                    fm.Add(new FocusModel() { IdProperty = qw.GUID, ImageProperty = qw.PictureProperty, ToolTippProperty = qw.NameOfProperty + " (Квест)" });
                }
            }

            //AllSpells = new List<FocusModel>();

            //foreach (var focusModel in interFoc)
            //{
            //    if (lnks.Any(q => q == focusModel.IdProperty)) fm.Add(focusModel);
            //    //focusModel.ColorItemProperty = lnks.Any(q => q == focusModel.IdProperty)
            //    //    ? Colors.White.ToString()
            //    //    : Colors.Transparent.ToString();
            //}

            AllSpells = fm;
        }

        /// <summary>
        /// Обновляем все задачи в главном окне
        /// </summary>
        public void RefreshTasksInMainView(bool isOnlyAddOrdelete = false)
        {
            FillTaskLinkDic();

            var st = SellectedTask;
            var lp = LastParTask;

            isOnlyAddOrdelete = true;
            StaticMetods.RefreshAllQwests(Pers, false, false, false);
            RefreshRelations();

            RefreshTasksInMain(isOnlyAddOrdelete);

            foreach (Task task in Tasks)
            {
                if (task.LinkedTasks?.FirstOrDefault() != null)
                {
                    task.LinkedTasks = task.LinkedTasks;
                }
                task.RefreshBackGround();
            }

            RecountActiveQwests();

            if (IsFocMode)
            {
                if (!Tasks.Contains(lp))
                {
                    st = TryGetTaskByIndex(0);
                }
            }

            FillInterfacePanel();

            if (TasksForInterfacePanel.Count > 0 && !TasksForInterfacePanel.Any(n => n.Task == st))
            {
                st = TasksForInterfacePanel.FirstOrDefault().Task;
                lp = st;
            }

            SellectedTask = st;
            LastParTask = lp;

            FillFoc();

            SellectedTask?.RefreshLinkedTasksNotify();
            OnPropertyChanged(nameof(IsSkillsVisible));
            OnPropertyChanged(nameof(IsTaskPanelVisibility));
            OnPropertyChanged(nameof(IsPipBoyVisibility));
            OnPropertyChanged(nameof(LastParTask));
        }

        /// <summary>
        /// Заполняет панель интерфейса (активными квестами и навыками).
        /// </summary>
        private void FillInterfacePanel()
        {
            TasksForInterfacePanel.Clear();

            if (IsFocMode)
            {
                IEnumerable<Task> enumerable = Tasks.Cast<Task>();
                var enumerable2 = enumerable.Select(n => new { tsk = n, relObj = GetRightRels(n) })
                                    .Where(n => n.relObj != null);
                var distinkt = enumerable2.DistinctBy(n => n.relObj.Guid).ToList();

                foreach (var task in distinkt)
                {
                    task.tsk.IsMoreHack = string.Empty;

                    TasksForInterfacePanel.Add(new ElementForInterface()
                    {
                        Task = task.tsk,
                        PictureProperty = task.relObj.PictureProperty,
                        ToolTip = task.relObj.Name,
                        Guid = task.relObj.Guid
                    });
                }

                TasksCount = TasksForInterfacePanel.Count;

                if (SellectedTask != null && TasksForInterfacePanel.All(q => q.Task != SellectedTask))
                    SellectedTask = TryGetTaskByIndex(0);
                else
                    OnPropertyChanged(nameof(LastParTask));
            }

            OnPropertyChanged(nameof(TasksForInterfacePanel));
        }

        public void RefreshTasksPriority(bool isSetParretto)
        {
            foreach (var item in TaskLinkDic)
            {
                if (item.Value.Ability != null)
                {
                    AbilitiModel ab;
                    item.Key.Priority = Task.getPrioryty(Pers, item.Key, out ab, item.Value.Ability);
                }
            }

            //var enabledTasks = Pers.Tasks
            //    .Where(n => n.Recurrense.TypeInterval != TimeIntervals.Нет
            //                 && n.IsEnabled).ToList();

            //foreach (var task in enabledTasks)
            //{
            //    AbilitiModel ab;
            //    task.Priority = Task.getPrioryty(Pers, task, out ab);
            //    if (isSetParretto)
            //        task.isSuper = false;
            //}

            //if (isSetParretto && Pers.Characteristics.Any())
            //{
            //    enabledTasks = enabledTasks
            //        .Where(n => (n.BeginDateProperty.Date <= MainViewModel.selectedTime.Date
            //                     || n.TimeLastDone == MainViewModel.selectedTime.Date)).ToList();

            //    var minCharactVal = Pers.Characteristics.Min(n => n.CellValue);

            //    var lowCharacts = Pers.Characteristics.Where(n => n.CellValue == minCharactVal).ToList();

            //    foreach (var enabledTask in enabledTasks)
            //    {
            //        var tskCharacts = enabledTask.AbilityLink.GetRelCharacteristics();

            //        if (tskCharacts.Intersect(lowCharacts).Any())
            //            enabledTask.isSuper = true;
            //    }

            //    /*
            //    enabledTasks = enabledTasks
            //        .Where(n => (n.BeginDateProperty.Date <= MainViewModel.selectedTime.Date
            //                        || n.TimeLastDone == MainViewModel.selectedTime.Date)).ToList();

            //    var orderByDescending = enabledTasks.OrderByDescending(n => n.Priority).ThenBy(n => n).ToList();

            //    double perc80 = enabledTasks.Sum(n => n.Priority) * 0.75;
            //    double exp = 0;
            //    double minExpParetto = 0;
            //    int count = 0;

            //    foreach (var task in orderByDescending)
            //    {
            //        exp += task.Priority;
            //        task.isSuper = true;
            //        count++;
            //        if (exp >= perc80)
            //        {
            //            minExpParetto = task.Priority;
            //            break;
            //        }
            //    }
            //    */
            //}
        }

        /// <summary>
        /// Сохранить данные о персонаже
        /// </summary>
        public void SavePers()
        {
            var of = new Microsoft.Win32.SaveFileDialog();
            of.ShowDialog();
            var pathToSave = of.FileName;
            savePersToPath(pathToSave);
        }

        /// <summary>
        /// Сохраняем по 5 минут
        /// </summary>
        public void SaveTimerStart()
        {
            SaveTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5, 0) };

            SaveTimer.Tick += (sender, e) =>
            {
                System.Threading.Tasks.Task.Factory.StartNew(
                    () =>
                    {
                        SaveData(
                            Settings.Default.PathToPers,
                            Pers);
                    });
            };

            SaveTimer.Start();
        }

        public void SaveWhenClosing()
        {
            //SaveData(appFolder, Pers);
        }

        /// <summary>
        /// Открыть окно с достижениями
        /// </summary>
        public void ShowDostijeniya()
        {
            if (isTerribleBuff(Pers))
            {
                return;
            }

            var dv = new DostijeniyaView();
            Messenger.Default.Send<Window>(dv);
            dv.ShowDialog();
        }

        public void SyncToAndroid()
        {
            StaticMetods.Locator.MainVM.AutoLoadFromeAndro();
        }

        public bool TasksFilter(object o)
        {
            var tsk = (Task)o;

            if (tsk == null)
            {
                return false;
            }

            var isPlaningMode = IsPlaningMode;

            var visibleInCurView = IsTaskVisibleInCurrentView(tsk, SelectedView, Pers, false, false, isPlaningMode,
                false);

            if (visibleInCurView == false) return false;

            if (!isPlaningMode)
            {
                if (TaskLinkDic.ContainsKey(tsk))
                {
                    var tdi = TaskLinkDic[tsk];

                    // Проверка по квестам
                    var qwest = tdi.Qwest;
                    if (qwest != null)
                    {
                        // Проверка по задаче - запихнута ли она уже в ссылки?
                        if (SelectedView.NameOfView != "Квесты" 
                                && SelectedView.NameOfView != "Все")
                                return false;

                        // Проверка по задаче - является ли она первой в списке квестов
                        var tasksFilter = qwest.NeedsTasks.OrderBy(q => q.TaskProperty)
                                            .Select(q => q.TaskProperty).FirstOrDefault() == tsk;

                        return tasksFilter;
                    }

                    var abil = tdi.Ability;
                    
                    // Проверка по задаче - является ли она первой в списке скиллов
                    if (abil != null)
                    {
                        if (SelectedView.NameOfView != "Навыки"
                                && SelectedView.NameOfView != "Все")
                            return false;

                        if (!abil.IsMonstersall && Pers.PersSettings.IsHideTasksOneByOneAndroid)
                            return abil.NeedTasks.OrderBy(q => q.TaskProperty)
                               .FirstOrDefault(q => IsTaskVisibleInCurrentView(q.TaskProperty, null, Pers, false, true, false, true)).TaskProperty
                               == tsk;
                    }

                    if (tsk.Wave > 0)
                    {
                        var tasksList = Tasks.Cast<Task>().ToList();

                        if (tasksList.Any(n => n != tsk && n.Wave != 0 && n.Wave < tsk.Wave))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (SelectedView.NameOfView != "Монстры"
                                && SelectedView.NameOfView != "Все")
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static Color BoldColor()
        {
            return Colors.White;
        }

        private static void clearShopNeeds(Revard shopItem)
        {
            shopItem.NeedCharacts.Clear();
            shopItem.NeedQwests.Clear();
            shopItem.AbilityNeeds.Clear();
        }

        /// <summary>
        /// Сжать файл
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <returns></returns>
        private static string CompressFile(string sourceFileName)
        {
            using (var archive = ZipFile.Open(Path.ChangeExtension(sourceFileName, ".zip"), ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(sourceFileName, Path.GetFileName(sourceFileName));
            }
            return Path.ChangeExtension(sourceFileName, ".zip");
        }

        /// <summary>
        /// Цвет скилла для автофокуса
        /// </summary>
        /// <param name="abilitiModel"></param>
        /// <returns></returns>
        private static Color GetAbColor(AbilitiModel abilitiModel)
        {
            var any =
                abilitiModel.NeedTasks.Any(
                    n =>
                        n.TaskProperty.EndDate.Date <= selectedTime && !n.TaskProperty.IsDelProperty &&
                        n.LevelProperty == abilitiModel.CellValue - 1);
            var color = any ? BoldColor() : Colors.White;
            return color;
        }

        /// <summary>
        /// Получение квеста для зоны фокусировки
        /// </summary>
        /// <param name="focusObj">Объект для добавления в зону фокусировки</param>
        /// <param name="color">Фон бэкграунда</param>
        /// <param name="progressVisibility">Отображать прогресс?</param>
        /// <param name="isActive">Активен?</param>
        /// <returns></returns>
        private static FocusModel GetItemToFocus(object focusObj, Color color, Visibility progressVisibility,
            bool isActive)
        {
            var type = string.Empty;
            var guid = string.Empty;
            var name = string.Empty;
            BitmapImage im = null;
            double val = 0;
            var progress = string.Empty;
            var toolTip = string.Empty;
            var qwest = focusObj as Aim;

            if (qwest != null)
            {
                type = "Квест";
                guid = qwest.GUID;
                name = qwest.NameOfProperty;
                im = qwest.PictureProperty;
                val = qwest.AutoProgressValueProperty;
                progress = qwest.AutoProgressValueProperty + " %";
                toolTip =
                    $"Квест: \"{name}\"";
            }

            var ab = focusObj as AbilitiModel;
            if (ab != null)
            {
                type = "Навык";
                guid = ab.GUID;
                name = ab.NameOfProperty;
                im = ab.PictureProperty;
                val = ab.LevelProperty;
                progress = $" {ab.LevelProperty}/{ab.MaxLevelProperty}";
                toolTip =
                    $"Навык: \"{name}\";";
            }

            //toolTip += "\n* ЛКМ активные задачи";
            //toolTip += "\n* ПКМ открыть для настройки";

            return new FocusModel
            {
                TittleProperty = name,
                IdProperty = guid,
                TypeProperty = type,
                ImageProperty = im,
                LevelProperty =
                    progress,
                ValueProperty = val,
                MinValueProperty = 0,
                MaxValueProperty = 100.0,
                IsActiveProperty = isActive,
                ColorItemProperty = color.ToString(),
                ProgressVisibility = progressVisibility,
                ToolTippProperty = toolTip,
                IsMoveVisibility = Visibility.Collapsed,
                Qwest = qwest,
                Abillity = ab
            };
        }

        /// <summary>
        /// Получить элементы на которые влияет задача напрямую.
        /// </summary>
        /// <param name="tsk"></param>
        /// <returns></returns>
        private static RightRells GetRightRels(Task tsk)
        {
            Aim withoutTasks = new Aim()
            {
                NameOfProperty = "Просто задачи",
                PictureProperty = DefoultPicsAndImages.DefoultTaskPic
            };

            TaskLinkDictItem tdi = null;
            ViewsModel sellectedView = StaticMetods.PersProperty.ViewForDefoult;

            if (TaskLinkDic.TryGetValue(tsk, out tdi))
            {
                if (sellectedView.NameOfView != "Задачи")
                {
                    if (tdi.Ability != null
                        && (sellectedView.NameOfView == "Навыки" || sellectedView.NameOfView == "Все"))
                        return new RightRells()
                        {
                            Name = tdi.Ability.NameOfProperty,
                            PictureProperty = tdi.Ability.PictureProperty,
                            Guid = tdi.Ability.GUID
                        };
                    else if (tdi.Qwest != null
                        && (sellectedView.NameOfView == "Квесты"
                            || sellectedView.NameOfView == "Все"))
                        if (sellectedView.NameOfView == "Все" && tdi.Skills.Any()) // Если квест запихнут в ссылку навыка
                            return null;
                    else
                        return new RightRells()
                        {
                            Name = tdi.Qwest.NameOfProperty,
                            PictureProperty = tdi.Qwest.PictureProperty,
                            Guid = tdi.Qwest.GUID
                        };
                }
            }
            else
            {
                if(sellectedView.NameOfView == "Монстры" || sellectedView.NameOfView == "Все")
                    return new RightRells()
                    {
                        Name = "Монстры",
                        PictureProperty = DefoultPicsAndImages.DefoultTaskPic,
                        Guid = "Монстры"
                    };
            }

            return null;
        }

        /// <summary>
        /// Задача не активна в связи с требованиями
        /// </summary>
        /// <param name="task"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private static bool IsTaskActiveInAnyNeeds(Task task, ref string req)
        {
            if (task.IsDelProperty)
                return false;

            // По навыкам
            if (TaskLinkDic.ContainsKey(task))
            {
                var tdi = TaskLinkDic[task];

                if (tdi.Ability != null)
                {
                    if (!tdi.Ability.IsEnebledProperty)
                        return true;

                    if (tdi.Ability.CellValue < tdi.AbilityLevelFrom)
                        return true;

                    if (tdi.Ability.CellValue > tdi.AbilityLevelTo)
                        return true;
                }

                if (tdi.Qwest != null)
                {
                    if (!tdi.Qwest.IsActiveProperty)
                        return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        private static bool IsTaskTodayEnabled(Task task)
        {
            var isTaskTodayEnabled = true;

            // Проверка по дате
            var dateOfBegin = task.BeginDateProperty;
            if (dateOfBegin > selectedTime)
            {
                isTaskTodayEnabled = false;
            }

            // Задача выполнена на текущий момент?
            if (isThisTaskDone(task))
            {
                isTaskTodayEnabled = false;
            }

            return isTaskTodayEnabled;
        }

        /// <summary>
        /// А видна ли задача в выбранном виде?
        /// </summary>
        /// <param name="task">Задача</param>
        /// <param name="_selView">Выбранный вид</param>
        /// <param name="ignoreSellectedView">Игнорировать выбранный вид</param>
        /// <param name="ignoreStatus"></param>
        /// <returns>Видимость задачи</returns>
        private static bool isVisibleInView(Task task, ViewsModel _selView, bool ignoreSellectedView, bool ignoreStatus)
        {
            if (ignoreSellectedView == false)
            {
                if (_selView != null)
                {
                    if (_selView.NameOfView == "План")
                    {
                        return true;
                    }
                    if (task.Recurrense.TypeInterval != TimeIntervals.Нет && _selView.NameOfView == "Навыки")
                    {
                        return true;
                    }
                    if (task.Recurrense.TypeInterval == TimeIntervals.Нет && _selView.NameOfView == "Монстры")
                    {
                        return true;
                    }
                    if (task.Recurrense.TypeInterval == TimeIntervals.Нет && _selView.NameOfView == "Квесты")
                    {
                        return true;
                    }

                    return false;
                    //// Проверка по видимости статуса
                    //if (!ignoreStatus)
                    //{
                    //    if (!_selView.ViewStatusOfTasks.Any(n => n.taskStatus == task.TaskStatus && n.isVisible))
                    //    {
                    //        return false;
                    //    }
                    //}

                    //// Проверка по видимости контекста
                    //var viewVisibleContextses = _selView.ViewContextsOfTasks.Where(n => n.taskContext == task.TaskContext && n.isVisible);
                    //if (!viewVisibleContextses.Any())
                    //{
                    //    return false;
                    //}

                    //return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Распаковать зип файл
        /// </summary>
        /// <param name="sourceFileName">Путь с зип</param>
        /// <returns></returns>
        private static string UncompressFile(string sourceFileName)
        {
            var zipPath = sourceFileName;
            var extractPath = Path.GetDirectoryName(zipPath);
            var extracted = new List<string>();
            using (var archive = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        var destinationFileName = Path.Combine(extractPath, entry.FullName);
                        entry.ExtractToFile(destinationFileName, true);
                        extracted.Add(destinationFileName);
                    }
                }
            }

            return extracted.LastOrDefault();
        }

        private void addCompositeFocus(ref FocusModel fm, Aim qwest, ref List<Aim> blackList)
        {
            var qwestfocusItem = GetItemToFocus(qwest, QwestColorForFocus(qwest), Visibility.Collapsed, true);

            if (qwest.NeedsTasks.All(n => !IsTaskVisibleInCurrentView(n.TaskProperty, SelectedView, Pers)))
            {
                qwestfocusItem.Visible = Visibility.Collapsed;
            }

            if (!qwest.IsActiveProperty || qwest.IsDoneProperty)
            {
                return;
            }

            foreach (var compositeAimse in qwest.CompositeAims)
            {
                addCompositeFocus(ref qwestfocusItem, compositeAimse.AimProperty, ref blackList);
            }

            if (qwest.RelatedTasks().Any(q => IsTaskVisibleInCurrentView(q, SelectedView, Pers)))
            {
                fm.SubFocusItems.Add(qwestfocusItem);
            }

            blackList.Add(qwest);
        }

        //    if (abBlackList.All(n => n != abilitiModel))
        //    {
        //        activeQwestsWithTasks.Add(abFocusItem);
        //        abBlackList.Add(abilitiModel);
        //    }
        //}
        /// <summary>
        /// Добавить квест и его подквесты в фокусировку
        /// </summary>
        /// <param name="qwest">Квест</param>
        /// <param name="focusList">Список фокусировки</param>
        /// <param name="task">Задача, которая содержится в квестах или подквестах</param>
        /// <param name="col">Цвет</param>
        private void AddQwestAndItCompositeToFocus(Aim qwest, List<FocusModel> focusList, Task task, Color col)
        {
            foreach (var compositeAimse in qwest.CompositeAims)
            {
                AddQwestAndItCompositeToFocus(compositeAimse.AimProperty, focusList, task, col);
            }

            if (task != null)
            {
                if (qwest.IsActiveProperty == false || qwest.NeedsTasks.Any(n => n.TaskProperty == task) == false)
                {
                    return;
                }
            }
            else
            {
                if (qwest.IsActiveProperty == false ||
                    qwest.NeedsTasks.Any(n => IsTaskVisibleInCurrentView(n.TaskProperty, null, Pers)) == false)
                {
                    return;
                }
            }

            focusList.Add(GetItemToFocus(qwest, col, Visibility.Collapsed, true));
        }

        // if (!except.Any()) { abFocusItem.Visible = Visibility.Collapsed; }
        private void afterClickForSmartSellect(Task t, int tInd, Task item)
        {
            if (t == null)
            {
                //t = TryGetTaskByIndex(tInd);
                t = TryGetTaskByIndex(0);
            }
            SmartSelect(item, t);
        }

        // var except = abilitiModel.NeedTasks.Where(n => n.LevelProperty == curAbNeedsLevel)
        // .Select(n => n.TaskProperty) .Except(allTaskInQwests);
        private void autoDebuffAbs()
        {
            // Неактивные навыки понижаем
            // Автопонижение значений всех неактивных навыков
            foreach (var v in Pers.Abilitis.Where(q => q.IsEnebledProperty == false && q.ValueProperty > 0))
            {
                var minus = v.ValueProperty - ((1.0 / (v.CellValue + 1.0)) * v.Booster);
                v.ValueProperty = minus;
            }
        }

        // var allTaskInQwests = from firstLevelQwest in firstLevelQwests from needTaskse in
        // firstLevelQwest.NeedsTasks select needTaskse.TaskProperty;
        private void beforeClickForSmartSelect(out int tInd, Task item, out Task t)
        {
            tInd = Tasks.IndexOf(item);
            t = null;
            var qw = item.RelToQwests.FirstOrDefault();
            if (qw != null)
            {
                GetNextQwestTask(item, out t);
            }
        }

        // foreach ( var compAim in firstLevelQwests ) { addCompositeFocus(ref abFocusItem, compAim,
        // ref qwBlackList); }
        /// <summary>
        /// Очищаем из требований скиллов требования с удаленными скиллами
        /// </summary>
        /// <param name="abilitis"></param>
        private void cleanAbNeeds(ObservableCollection<AbilitiModel> abilitis)
        {
            foreach (var abilitiModel in abilitis)
            {
                var abilReqwirementsNotInAbilities =
                    abilitiModel.NeedAbilities.Where(n => abilitis.All(q => q != n.AbilProperty)).ToList();
                foreach (var abilReqwirement in abilReqwirementsNotInAbilities)
                {
                    abilitiModel.NeedAbilities.Remove(abilReqwirement);
                }
            }
        }

        // var firstLevelQwests = all.Where(n => all.All(q => q.CompositeAims.All(w => w.AimProperty
        // != n))).ToList();
        private void CleardoneTasksImages()
        {
            // Изображения
            foreach (var source in Pers.Tasks.Where(n => n.IsDelProperty))
            {
                source.ImageProperty = null;
            }
        }

        // var all = compositeAimses.Concat(aimsWithTask).Distinct().ToList();
        /// <summary>
        /// Завершение хода
        /// </summary>
        private void EndOfTurn(bool isFromeAndro = false)
        {
            var beforeAbPoints = Pers.AbilitisPoints;

            // Обнуляем таймеры
            foreach (var task in Pers.Tasks)
            {
                task.TimerRefresh();
                task.CounterRefresh();
            }

            if (isFromeAndro)
            {
                autoDebuffAbs();

                var notToDone =
                    GetNotDonnedTodayTasks();
                foreach (
                    var task in
                    notToDone)
                {
                    task.ClickPlusMinusTomorrowTask(Pers, false, false, true);
                }
            }

            // Завершение хода
            if (isFromeAndro == false)
            {
                var vc = new ViewChangesClass(Pers.InventoryItems.Union(Pers.ShopItems).ToList());
                vc.GetValBefore();
                // Завершение всех просроченных задач
                var notToDone =
                    GetNotDonnedTodayTasks();
                foreach (
                    var task in
                        notToDone)
                {
                    task.ClickPlusMinusTomorrowTask(Pers, false, false, true);
                }
                // Если здоровье активно
                if (Pers.PersSettings.IsHPActiveteProperty)
                {
                    Pers.HitPoints += 10;
                }

                autoDebuffAbs();

                vc.GetValAfter();
                double a;
                double b;
                vc.GetChanges(out a, out b);
                var showEndTurn = !vc.ViewChanges.Any();
                StaticMetods.PlaySound(showEndTurn
                    ? Resources.abLevelUp
                    : Resources.Fail);
                vc.ViewChanges.Clear();
                if (!showEndTurn)
                {
                    vc.ShowChanges("Следующий ход!", Brushes.Green, null, selectedTime.ToShortDateString(),
                        showEndTurn);
                }
                FirstFocus = null;
            }

            // Добавляем дату
            var newData = selectedDateTime.AddDays(1);
            Pers.LastDateOfUseProperty = newData.ToString();
            selectedDateTime = newData;
            Pers.updateDateString();
            SelectedView = Pers.Views.FirstOrDefault();

            Pers.NewLevelAfterSleep(Pers);

            if (isFromeAndro == false)
            {
                Pers.ShowChangeAbPoints(beforeAbPoints, Pers.AbilitisPoints);
                RefreshTasksInMainView();
            }

            RefreshTasksPriority(true);

            StaticMetods.WriteAutoBard(StaticMetods.AutoBardOperations.КонецХода, null);
        }

        // var aimsWithTask = Pers.Aims.Where(n => n.IsActiveProperty).Where(n => n.NeedsTasks.Any(q
        // => q.TaskProperty == tsk));
        private void ExportToAndroid(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                var fb = new FolderBrowserDialog();
                fb.ShowDialog();
                path = fb.SelectedPath;
            }

            if (string.IsNullOrEmpty(path))
                return;

            Pers.IsPlanningModeMain = false;
            var pers = Pers;
            var root = RPGAndroidJSON.SerializeToAndroidJSON(pers, path);
            var serialized = JsonConvert.SerializeObject(root);
            path = Path.Combine(path, "!ToPhone_RpgOrganizer.json");

            File.WriteAllText(path, serialized);
        }

        /// <summary>
        /// Выбирает задачи, которые отображаются при выборе чего-то из панели интерфейса.
        /// </summary>
        private void FillFoc()
        {
            if (TasksForInterfacePanel.Count == 0)
            {
                TasksForFoc = new List<Task>();
                return;
            }

            if (_lastParTask == null || !Tasks.Contains(_lastParTask))
            {
                _lastParTask = TryGetTaskByIndex(0);
            }
            if (_lastParTask == null)
            {
                TasksForFoc = new List<Task>();
                return;
            }

            var tsk = LastParTask;
            var lt = new List<Task>();
            SortedSet<Task> tl = new SortedSet<Task> { tsk };

            if (TaskLinkDic.ContainsKey(tsk))
            {
                var tdi = TaskLinkDic[tsk];
                var abil = tdi.Ability;

                if (abil != null)
                {
                    if(abil.IsMonstersall)
                        foreach (var nt in abil.NeedTasks)
                            if (TasksFilter(nt.TaskProperty))
                                tl.Add(nt.TaskProperty);

                    // Добавляем задачи из квестов
                    foreach (var qt in tdi.QwestTasks)
                        tl.Add(qt);
                }

                var qw = tdi.Qwest;
                if (qw != null)
                    foreach (var sk in tdi.Skills)
                        tl.Add(sk);

            }
            else
            {
                // Если это просто задача, то добавляем все просто задачи
                foreach (var ptsk in Pers.Tasks)
                {
                    if (!ptsk.IsDelProperty
                         && !TaskLinkDic.ContainsKey(ptsk)
                           && TasksFilter(ptsk))
                        tl.Add(ptsk);
                }
            }

            // Добвляем подзадачи
            List<Task> newTList = new List<Task>();
            foreach (var source in tl)
            {
                newTList.Add(source);
                var firstSub = source.NotDoneSubtasksFirstTasks.FirstOrDefault();
                if (firstSub != null)
                {
                    var sub = new Task();
                    sub.TaskLevel = source.TaskLevel;
                    sub.NameOfProperty = firstSub.Tittle;
                    sub.GUID = firstSub.Guid;
                    sub.PathToIm = firstSub.PathToIm;
                    sub.Recurrense = new TypeOfRecurrense();
                    sub.Recurrense.TypeInterval = TimeIntervals.Нет;
                    newTList.Add(sub);
                }
            }

            TasksForFoc = newTList;
        }

        /// <summary>
        /// Заполняем словарь с задачами и их ссылками.
        /// </summary>
        private void FillTaskLinkDic()
        {
            TaskLinkDic = new Dictionary<Task, TaskLinkDictItem>();

            // Пробегаем по навыкам
            foreach (var ab in Pers.Abilitis)
            {
                TaskLinkDictItem abTdi = null;

                foreach (var nt in ab.NeedTasks)
                {
                    bool isExists = TaskLinkDic.TryGetValue(nt.TaskProperty, out abTdi);

                    if (!isExists)
                        abTdi = new TaskLinkDictItem();

                    abTdi.Ability = ab;
                    abTdi.AbilityLevelFrom = nt.LevelProperty;
                    abTdi.AbilityLevelTo = nt.ToLevelProperty;

                    if (!isExists)
                        TaskLinkDic.Add(nt.TaskProperty, abTdi);
                }

                // Ссылки на квесты
                foreach (var qw in ab.LinkedQwests)
                {
                    if (qw.IsActiveProperty)
                    {
                        var fod = qw.NeedsTasks.OrderBy(n => n.TaskProperty)
                            .FirstOrDefault(n => n.TaskProperty.IsDelProperty == false);

                        if (fod != null)
                            abTdi.QwestTasks.Add(fod.TaskProperty);
                    }

                    foreach (var nt in qw.NeedsTasks)
                    {
                        TaskLinkDictItem tdi = null;
                        bool isExists = TaskLinkDic.TryGetValue(nt.TaskProperty, out tdi);

                        if (!isExists)
                            tdi = new TaskLinkDictItem();

                        tdi.HasSkills = true;
                        tdi.Qwest = qw;
                        int count = 0;
                        foreach (var abnt in ab.NeedTasks)
                        {
                            if ((count == 0 || ab.IsMonstersall)
                                && MainViewModel.IsTaskVisibleInCurrentView(abnt.TaskProperty,
                                Pers.ViewForDefoult, Pers, false, true))
                            {
                                tdi.Skills.Add(abnt.TaskProperty);
                                count++;
                            }
                        }

                        if (!isExists)
                            TaskLinkDic.Add(nt.TaskProperty, tdi);
                    }
                }
            }

            // Пробегаем по квестам
            foreach (var qw in Pers.Aims)
            {
                foreach (var nt in qw.NeedsTasks)
                {
                    TaskLinkDictItem tdi = null;
                    bool isExists = TaskLinkDic.TryGetValue(nt.TaskProperty, out tdi);

                    if (!isExists)
                        tdi = new TaskLinkDictItem();

                    tdi.Qwest = qw;

                    if (!isExists)
                        TaskLinkDic.Add(nt.TaskProperty, tdi);
                }
            }

        }

        // Фильтр для задач фокусировки
        private bool FocFilter(object o)
        {
            var tsk = (Task)o;
            if (IsFocTaksVisibility == Visibility.Collapsed)
            {
                return false;
            }

            if (SelFoc?.Tasks != null && SelFoc.Tasks.Contains(tsk))
            {
                return true;
            }

            return false;
        }

        private void ForOld()
        {
            // Для видов задач экспорта в андроид
            foreach (var source in Pers.Views.Where(n => string.IsNullOrEmpty(n.GUID)))
            {
                source.GUID = Guid.NewGuid().ToString();
            }

            if (Pers.Version == 0)
            {
                Pers.Statuses.Clear();
                Pers.Statuses = new ObservableCollection<StatusTask>
                {
                    new StatusTask {NameOfStatus = "Нет", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Первым делом", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "В работе", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Планируется", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Поручено", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Ожидается", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Задержано", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Отложено", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Когда-нибудь", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Отменено", Uid = Guid.NewGuid().ToString()},
                    new StatusTask {NameOfStatus = "Памятка", Uid = Guid.NewGuid().ToString()}
                };

                foreach (var typeOfTask in Pers.TasksTypes)
                {
                    typeOfTask.StatusForDefoult = Pers.Statuses[0];
                }

                foreach (var view in Pers.Views)
                {
                    view.ViewStatusOfTasks.Clear();

                    foreach (var statusTask in Pers.Statuses)
                    {
                        view.ViewStatusOfTasks.Add(new ViewVisibleStatuses { taskStatus = statusTask, isVisible = false });
                    }

                    view.ViewStatusOfTasks.First().isVisible = true;
                }

                Pers.Version = 1;
            }
        }

        private void ForPriorPlaning()
        {
            if (IsPlaningMode)
            {
                Tasks.GroupDescriptions.Clear();
                Tasks.GroupDescriptions.Add(new PropertyGroupDescription("Wave"));
                ColsInPlaning = 1;
            }
            else
            {
                Tasks.GroupDescriptions.Clear();
            }
        }

        private List<Task> GetAllTasksForQwest(Aim qwest)
        {
            var allThisTasks = qwest.NeedsTasks
                .Where(n => IsTaskVisibleInCurrentView(n.TaskProperty, null, Pers, false, true, false, true))
                .Select(n => n.TaskProperty)
                .ToList();

            return allThisTasks.Distinct().ToList();
        }

        /// <summary>
        /// Заполнить коллекцию для зоны фокусировки
        /// </summary>
        /// <param name="abFocusCollectionCollection">Фокус для скиллов</param>
        /// <param name="qwestsInAbils">Фокус для квестов, входящих в скиллы</param>
        /// <param name="qwestsWithoutAbils">Квесты без скиллов</param>
        /// <param name="col">Цвет фона</param>
        private void GetFocusCollectionsFromTasks(List<FocusModel> abFocusCollectionCollection,
            List<FocusModel> qwestsInAbils, List<FocusModel> qwestsWithoutAbils, Color col)
        {
            foreach (var taskk in Tasks)
            {
                var task = taskk as Task;

                foreach (
                    var abilitiModel in
                        Pers.Abilitis.Where(n => n.IsEnebledProperty && n.RelatedTasks().Any(q => q == task))
                    )
                {
                    if (abilitiModel.NeedTasks.Any(q => q.TaskProperty == task))
                    {
                        abFocusCollectionCollection.Add(GetItemToFocus(abilitiModel, col, Visibility.Collapsed,
                            true));

                        foreach (var compositeAimse in abilitiModel.NeedAims)
                        {
                            AddQwestAndItCompositeToFocus(compositeAimse.AimProperty, qwestsInAbils, null, col);
                        }
                    }
                }

                foreach (
                    var aim in
                        Pers.Aims.Where(n => n.IsActiveProperty && n.NeedsTasks.Any(q => q.TaskProperty == task)))
                {
                    AddQwestAndItCompositeToFocus(aim, qwestsWithoutAbils, task, col);
                }
            }
        }

        private bool GetNextQwestTask(Task thisTask, out Task nextTask)
        {
            var qw = thisTask.RelToQwests.FirstOrDefault();
            if (qw != null)
            {
                // Выбираем вторую активную задачу
                var tsks =
                    qw.NeedsTasks.Where(
                        n => IsTaskVisibleInCurrentView(n.TaskProperty, null, Pers, false, false, false, true))
                        .Select(n => n.TaskProperty)
                        .OrderBy(n => n)
                        .ToList();
                var nt = tsks.Skip(tsks.IndexOf(thisTask) + 1).FirstOrDefault();
                if (nt != null)
                {
                    nextTask = nt;
                    return true;
                }
            }
            nextTask = null;
            return false;
        }

        private void GetNextTaskForSmartSelect(Task thisTask, out Task nextTask)
        {
            // Если она в списке задач
            if (Tasks.Contains(thisTask))
            {
                var indThis = Tasks.IndexOf(thisTask);

                var nextInd = indThis + 1;
                if (Tasks.Count == 1)
                {
                    nextTask = null;
                    return;
                }

                // Если квест - выбираем следующую квеста Если в каком-то квесте
                if (GetNextQwestTask(thisTask, out nextTask)) return;

                // Если эта последняя - выбираем первую
                if (Tasks.Count <= nextInd)
                {
                    nextTask = null;
                    return;
                }

                // Выбираем следующую
                var nt = Tasks.GetItemAt(nextInd);
                if (nt != null)
                {
                    nextTask = (Task)nt;
                    return;
                }
                nextTask = (Task)Tasks.GetItemAt(0);
            }
            // Если она в списке ссылок
            else
            {
                // Если в каком-то квесте
                if (GetNextQwestTask(thisTask, out nextTask)) return;
                nextTask = LastParTask;
            }
        }

        private List<Aim> GetQwestsToFocus(List<AbilitiModel> abs, Task tsk)
        {
            var qw = new List<Aim>();

            foreach (var abilitiModel in abs)
            {
                qw.AddRange(
                    Pers.Aims.Where(n => abilitiModel.NeedTasks.Select(q => q.TaskProperty).Intersect(n.Spells).Any()));

                foreach (
                    var qww in
                        abilitiModel.QwestsActive.Where(
                            n => n.NeedsTasks.Any(q => IsTaskVisibleInCurrentView(q.TaskProperty, SelectedView, Pers)))
                            .ToList())
                {
                    qw.Add(qww);
                    qw.AddRange(
                        qww.AllCompositeQwests.Where(
                            n => n.NeedsTasks.Any(q => IsTaskVisibleInCurrentView(q.TaskProperty, SelectedView, Pers))));
                }
            }

            qw.AddRange(Pers.Aims.Where(n => n.NeedsTasks.Any(q => q.TaskProperty == tsk)));

            qw =
                qw.Where(n => n.IsActiveProperty)
                    .Distinct()
                    .OrderByDescending(
                        n => n.NeedsTasks.Count(q => IsTaskVisibleInCurrentView(q.TaskProperty, SelectedView,
                            Pers, false, false, false,
                            true))).ToList();
            return qw;
        }

        /// <summary>
        /// Получить все кликнутые задачи на текущую дату
        /// </summary>
        /// <param name="tasks">Все задачи андроида</param>
        /// <param name="qwests">Все квесты андроида</param>
        /// <param name="dateTime">Дата текущая</param>
        /// <returns></returns>
        private List<RPGAndroidJSON.Task> getSortedAndroTasksByDate(List<RPGAndroidJSON.Task> tasks, List<RPGAndroidJSON.Qwest> qwests, DateTime dateTime)
        {
            var dt = dateTime.Date.ToString("dd.MM.yyyy");
            var tsks = tasks.Where(n => n.IsDone).Where(n => n.DateOfDone == dt);
            var qwTasks = qwests.SelectMany(n => n.Tasks);
            var qw = qwTasks
                .Where(n => n.IsDone).Where(n => n.DateOfDone == dt);
            var all = tsks.Union(qw);

            return all.OrderBy(n => n.DateOfClick).ToList();
        }

        /// <summary>
        /// Получить все не кликнутые задачи на текущую дату
        /// </summary>
        /// <param name="tasks">Все задачи андроида</param>
        /// <param name="qwests">Все квесты андроида</param>
        /// <param name="dateTime">Дата текущая</param>
        /// <returns></returns>
        private List<RPGAndroidJSON.Task> getSortedNotClickedAndroTasksByDate(List<RPGAndroidJSON.Task> tasks, List<RPGAndroidJSON.Qwest> qwests, DateTime dateTime)
        {
            var dt = dateTime.Date.ToString("dd.MM.yyyy");
            var tsks = tasks.Where(n => n.IsDone == false && n.Date == dt);
            var qwTasks = qwests.SelectMany(n => n.Tasks);
            var qw = qwTasks
                .Where(n => n.IsDone == false && n.Date == dt);
            var all = tsks.Union(qw).ToList();
            return all;
        }

        private void HideGhostBastersMode()
        {
            //Pers.PersSettings.IsGhostBastersMode = false;
            //foreach (Task focTask in Tasks)
            //{
            //    focTask.RefreshBackGround();
            //}
        }

        /// <summary>
        /// Импорт из андроида
        /// </summary>
        /// <param name="path"></param>
        private void ImportFromAndroid(string path, bool catchDate = false)
        {
            var abPointsBefore = StaticMetods.PersProperty.AbilitisPoints;

            #region Получить файл с инфой

            if (!File.Exists(path))
            {
                return;
            }

            var text = File.ReadAllText(path);
            RPGAndroidJSON.RootObject root;

            try
            {
                root = JsonConvert.DeserializeObject<RPGAndroidJSON.RootObject>(text);
            }
            catch (Exception)
            {
                return;
            }

            if (catchDate)
            {
                var dateLastUse = Pers.GetDateOfLastSave();
                var timeOfLastUse = root.TimeOfLastUse;
                DateTime dateAndro = DateTime.MinValue;
                DateTime.TryParseExact(timeOfLastUse, "yyyyMMdd_HHmmss", null,
                    DateTimeStyles.None, out dateAndro);

                if (dateAndro <= dateLastUse)
                {
                    return;
                }
            }

            #endregion Получить файл с инфой

            var minDate = Pers.DateLastUseProgram;
            var maxDate = RPGAndroidJSON.getDateFromTaskAndroDateString(root.DateOfLastUse);//DateTime.Today.Date;

            selectedDateTime = selectedDateTime <= minDate ? selectedDateTime : minDate;

            var end = false;
            var vc = new ViewChangesClass(Pers.InventoryItems.Union(Pers.ShopItems).ToList());
            vc.GetValBefore();

            #region Отмечаем задачи

            var allSubTasks = Pers.Tasks.SelectMany(n => n.SubTasks).ToList();
            int maxDoingWave = 0;

            do
            {
                var sortedTasksByDate = getSortedAndroTasksByDate(root.Tasks, root.Qwests, selectedDateTime);

                foreach (var task in sortedTasksByDate)
                {
                    var taskInPers = Pers.Tasks.FirstOrDefault(n => n.GUID == task.UUID);

                    if (taskInPers == null)
                    {
                        // Подзадачи
                        var subInPers = allSubTasks.FirstOrDefault(n => n.Guid == task.UUID);
                        if (subInPers != null) subInPers.isDone = true;
                    }
                    else
                    {
                        var lastClick = GetTimeFromeStringMiliseconds(task.TimeLastDone);

                        if (!task.IsMinus) // Если сделана
                            taskInPers.ClickPlusMinusTomorrowTask(Pers, true, false, true);
                        else // Если не сделана
                            taskInPers.ClickPlusMinusTomorrowTask(Pers, false, false, true);

                        if (maxDoingWave < taskInPers.Wave)
                            maxDoingWave = taskInPers.Wave;
                    }
                }

                if (selectedDateTime.Date >= maxDate.Date)
                    end = true;

                if (selectedDateTime.Date < maxDate.Date)
                {
                    EndOfTurn(true);
                    maxDoingWave = 0;
                }
            } while (end == false);

            #endregion Отмечаем задачи

            RefreshTasksInMainView();
            Pers.PersSettings.LastAndroidFile = path;
            vc.GetValAfter();
            vc.ShowChanges($"Сеанс сверки с Пип-бой прошел успешно!\nТекущая дата: {selectedTime.Date.ToString("D")}", Brushes.Green,
                null, selectedTime.ToShortDateString(), false);

            if (maxDate > minDate)
            {
                Pers.ShowChangeAbPoints(abPointsBefore, Pers.AbilitisPoints);
            }

            // Теперь удаляем файл
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
            }

            RefreshTasksInMainView();
            OnPropertyChanged(nameof(Tasks));
        }

        /// <summary>
        /// Регистрация сообщений
        /// </summary>
        private void InizializeMesseges()
        {
            Messenger.Default.Register<string>(
                this,
                _string =>
                {
                    switch (_string)
                    {
                        case "!Начать заново!":
                            LetSBegin();
                            break;

                        case "!RIP!":
                            RIP();
                            break;

                        case "Обновить активные задачи!":
                            RefreshTasksInMainView();
                            break;

                        case "Закрыть показ изменений!":
                            break;

                        case "Создать пустой персонаж!":
                            if (Pers != null)
                            {
                                Pers = Pers.LoadPers(
                                    Path.Combine(Directory.GetCurrentDirectory(), "Templates", "LearningPers"));
                            }
                            break;

                        case "Загрузить простой пример!":
                            SampleTemplateLoadVoid();
                            break;

                        case "Загрузить продвинутый пример!":
                            loadAdvensedTemplateVoid();
                            break;

                        case "Выбираем картинку":
                            IsEditOrAddOpenProperty = false;
                            break;

                        case "Выбрали картинку":
                            IsEditOrAddOpenProperty = true;
                            break;

                        case "Таймер тикнул!":
                            break;

                        case "Обновить уровни!":
                            viewLevelRefresh();
                            break;

                        case "Показать изменения!":
                            IsViewChangesOpenPropertyProperty = true;
                            break;

                        case "Обновить карту всех задач!":
                            {
                                var onlyTaskForTaskMap =
                                    Pers.Tasks.Where(
                                        task =>
                                            IsTaskVisibleInCurrentView(task, SelectedView, Pers, true)).ToList();

                                Messenger.Default.Send(
                                    new TaskMapMessege
                                    {
                                        PersProperty = Pers,
                                        isFromeMainWindow = true,
                                        SellectedViewProperty = SelectedView,
                                        OnlyThisTasks = onlyTaskForTaskMap
                                    });
                            }

                            break;

                        case "Обновить ранги характеристик!":
                            foreach (var characteristic in Pers.Characteristics)
                            {
                                characteristic.Rangs =
                                    RangsForCharacteristucDefoult(
                                        Pers.PersSettings.CharactRangsForDefoultProperty)();
                            }

                            break;

                        case "Обновить ранги навыков!":
                            UpdateAbilitisRangs();
                            UpdateTasksRangs();
                            break;

                        case "Обновить после карты активных задач!":

                            Messenger.Default.Send("Обновить уровни!");
                            RefreshTasksInMainView();

                            break;
                        case "Выход!!!":
                            Exit();

                            break;
                    }
                });

            Messenger.Default.Register<MoveTaskMessege>(
                this,
                item =>
                {
                    var taskA = item.taskA;
                    var taskB = item.taskB;

                    MoveTask(taskA, taskB, false, item.IgnorePlaningMode);
                });
        }

        /// <summary>
        /// Инициализируем необходимые переменные
        /// </summary>
        private void InizializeVariables()
        {
            //NewTask = new Task();
            selectedTime = Pers.PersSettings.HOMMModeProperty ? Pers.DateOfLastUse : DateTime.Today;

            // Объявляем вид для задач
            Tasks = (ListCollectionView)new CollectionViewSource { Source = Pers.Tasks }.View;
            //Tasks = (ListCollectionView)new CollectionViewSource { Source = new List<Task>() }.View;
            Tasks.Filter = TasksFilter;
            Tasks.CustomSort = new TaskForTypesSorter(Pers.TasksTypes, Pers.Tasks);

            // Вид задач фокусировки
            FocTasks = (ListCollectionView)new CollectionViewSource { Source = new List<Task>() }.View;
            FocTasks.Filter = FocFilter;
            FocTasks.SortDescriptions.Add(new SortDescription(nameof(Task.SelFocTasksIndex), ListSortDirection.Ascending));
            //FocTasks.CustomSort = new TaskForTypesSorter(Pers.TasksTypes, Pers.Tasks);

            // Объявляем временные интервалы
            StaticMetods.GetRepeatIntervals();

            // Типы задач
            //Tasks.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Task.AbilityLink)));
            Pers.IsHideHUD = true;
        }

        /// <summary>
        /// The load advensed template void.
        /// </summary>
        private void loadAdvensedTemplateVoid()
        {
            pathToTemplate = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Images",
                "Templates",
                "AdvancedTemplate");

            Pers = GetPersFromeTemplate(pathToTemplate);
            //SaveData(appFolder, Pers);
            Restart(Pers);
        }

        /// <summary>
        /// The move task down.
        /// </summary>
        /// <param name="item">The item.</param>
        private void moveTaskDown(TypeOfTask item)
        {
            var oldIndex = Pers.Tasks.IndexOf(SellectedTask);
            var tasksOfThisType = Pers.Tasks.Where(
                n =>
                {
                    var dateOfBegin = n.BeginDateProperty;

                    var dateOfDone = string.IsNullOrEmpty(n.DateOfDone)
                        ? DateTime.MinValue
                        : DateTime.Parse(n.DateOfDone);

                    return n.TaskType == item && dateOfBegin <= selectedTime
                           && (dateOfDone <= selectedTime || dateOfDone == DateTime.MinValue);
                }).ToList();
            var index = tasksOfThisType.IndexOf(SellectedTask) + 1;
            if (index > tasksOfThisType.Count - 1)
            {
                return;
            }
            var nextTask = tasksOfThisType[index];
            var indexOf = Pers.Tasks.IndexOf(nextTask);
            Pers.Tasks.Move(oldIndex, indexOf);
            RefreshTasksInMainView();
            OnPropertyChanged("FocusCollectionProperty");
        }

        /// <summary>
        /// The move up task.
        /// </summary>
        /// <param name="item">The item.</param>
        private void moveUpTask(TypeOfTask item)
        {
            var oldIndex = Pers.Tasks.IndexOf(SellectedTask);

            var tasksOfThisType = Pers.Tasks.Where(
                n =>
                {
                    var dateOfBegin = n.BeginDateProperty;

                    var dateOfDone = string.IsNullOrEmpty(n.DateOfDone)
                        ? DateTime.MinValue
                        : DateTime.Parse(n.DateOfDone);

                    return n.TaskType == item && dateOfBegin <= selectedTime
                           && (dateOfDone <= selectedTime || dateOfDone == DateTime.MinValue);
                }).ToList();

            var index = tasksOfThisType.IndexOf(SellectedTask) - 1;
            if (index < 0)
            {
                return;
            }

            var previewsTask = tasksOfThisType[index];
            Pers.Tasks.Move(oldIndex, Pers.Tasks.IndexOf(previewsTask));
            RefreshTasksInMainView();
            OnPropertyChanged("FocusCollectionProperty");
        }

        private void openQwestsWindow(Tuple<string, string> whatTabOpen)
        {
            //StaticMetods.Locator.AimsVM.SelectedAimProperty = StaticMetods.PersProperty.Aims.FirstOrDefault();

            Messenger.Default.Send(whatTabOpen);
        }

        private Color QwestColorForFocus(Aim qwest)
        {
            var col = Colors.White;

            if (qwest.NeedsTasks.Any(n => n.TaskProperty.EndDate <= selectedTime && !n.TaskProperty.IsDelProperty))
            {
                col = BoldColor();
            }

            return col;
        }

        private void QwToFocus(List<FocusModel> activeQwestsWithTasks, Aim qwest, ref List<Aim> qwBlackList)
        {
            var qwFocus = GetItemToFocus(qwest, QwestColorForFocus(qwest), Visibility.Collapsed, true);
            if (qwest.NeedsTasks.All(n => !IsTaskVisibleInCurrentView(n.TaskProperty, SelectedView, Pers)) ||
                !qwest.IsActiveProperty || qwest.IsDoneProperty)
            {
                qwFocus.Visible = Visibility.Collapsed;
            }

            foreach (var compositeAimse in qwest.CompositeAims)
            {
                addCompositeFocus(ref qwFocus, compositeAimse.AimProperty, ref qwBlackList);
            }

            // var abWithThisQwest = Pers.Abilitis.SelectMany(n => n.NeedAims.Where(q =>
            // q.AimProperty == qwest)).ToList();

            if (qwBlackList.All(n => n != qwest))
            {
                activeQwestsWithTasks.Add(qwFocus);
                qwBlackList.Add(qwest);
            }
        }

        /// <summary>
        /// Пересчитать активные скиллы
        /// </summary>
        private void RecountActiveQwests()
        {
            return;

            //if (SelectedView == null)
            //{
            //    ActiveQwests = new List<FocusModel>();
            //    return;
            //}

            //var tsks =
            //    Tasks.Cast<Task>().ToList();

            //var allWithLinkedTasks = tsks;
            //allWithLinkedTasks.AddRange(tsks.SelectMany(n => n.LinkedTasks).ToList());

            //Func<IEnumerable<Task>, bool> IsIn = list =>
            //{
            //    if (list.Intersect(allWithLinkedTasks).Any())
            //    {
            //        return true;
            //    }
            //    return false;
            //};

            //var activeQw = new List<FocusModel>();

            //if (SelectedView.NameOfView == "План")
            //{
            //    foreach (var task in tsks)
            //    {
            //        var abilitiModels = task.TaskInAbilitis.Where(n => n.IsEnebledProperty)
            //            .Where(
            //                n =>
            //                    n.NeedTasks.Any(
            //                        q =>
            //                            IsTaskVisibleInCurrentView(q.TaskProperty, SelectedView, Pers, false, true,
            //                                IsPlaningMode))).ToList();

            //        var qwestsModeles = from aim in Pers.Aims
            //                            where aim.NeedsTasks.Select(n => n.TaskProperty).Intersect(task.LinkedTasks).Any()
            //                            select aim;

            //        activeQw.AddRange(
            //            qwestsModeles.Select(
            //                qwestsModele => GetItemToFocus(qwestsModele, Colors.Transparent, Visibility.Visible, true)));

            //        activeQw.AddRange(
            //            abilitiModels.Select(
            //                abilitiModel => GetItemToFocus(abilitiModel, Colors.Transparent, Visibility.Visible, true)));

            //        var qw = new List<Aim>();
            //        qw.AddRange(task.RelToQwests);
            //        qw.AddRange(task.TaskInAbilitis.SelectMany(n => n.LinkedQwests));
            //        qw = qw.Where(n => n.IsActiveProperty).ToList();
            //        activeQw.AddRange(
            //            qw.Select(qww => GetItemToFocus(qww, Colors.Transparent, Visibility.Visible, true)).ToList());
            //        activeQw.AddRange(qw.Select(qww => GetItemToFocus(qww, Colors.Transparent, Visibility.Visible, true)));
            //    }

            //    //var allSpells = new List<FocusModel>();
            //    //allSpells.AddRange(
            //    //    Pers.Abilitis.Where(n=>n.IsEnebledProperty).Select(
            //    //        abilitiModel => GetItemToFocus(abilitiModel, Colors.Transparent, Visibility.Visible, true)));
            //    //allSpells.AddRange(
            //    //    Pers.Aims.Where(n=>n.IsActiveProperty).Select(
            //    //        qwestsModele => GetItemToFocus(qwestsModele, Colors.Transparent, Visibility.Visible, true)));
            //    //AllSpells = allSpells;
            //}
            //else if (SelectedView.NameOfView == "Квесты")
            //{
            //    foreach (var task in tsks)
            //    {
            //        var qw = new List<Aim>();
            //        qw.AddRange(task.RelToQwests);
            //        qw.AddRange(task.TaskInAbilitis.SelectMany(n => n.LinkedQwests));
            //        qw = qw.Where(n => n.IsActiveProperty).ToList();
            //        activeQw.AddRange(
            //            qw.Select(qww => GetItemToFocus(qww, Colors.Transparent, Visibility.Visible, true)).ToList());
            //        activeQw.AddRange(qw.Select(qww => GetItemToFocus(qww, Colors.Transparent, Visibility.Visible, true)));
            //    }
            //}
            //else if (SelectedView.NameOfView == "Навыки")
            //{
            //    foreach (var task in tsks)
            //    {
            //        var abilitiModels = task.TaskInAbilitis.Where(n => n.IsEnebledProperty)
            //            .Where(
            //                n =>
            //                    n.NeedTasks.Any(
            //                        q =>
            //                            IsTaskVisibleInCurrentView(q.TaskProperty, SelectedView, Pers, false, true,
            //                                false))).ToList();
            //        activeQw.AddRange(
            //            abilitiModels.Select(
            //                abilitiModel => GetItemToFocus(abilitiModel, Colors.Transparent, Visibility.Visible, true)));
            //    }
            //}

            //ActiveQwests = activeQw.Distinct(new FocusModelComparer()).ToList();
            ////RefreshSellectedInFoc(SellectedTask, ActiveQwests);
            //InterfaceActiveGroupsVisibillity = SelectedView.NameOfView == "План" || SelectedView.NameOfView == "Задачи";

            //if (IsPlaningMode)
            //{
            //    InterfaceActiveGroupsVisibillity = true;
            //}

            //ActiveQwests = ActiveQwests.Where(n =>
            //{
            //    var qw = Pers.Aims.FirstOrDefault(q => q.GUID == n.IdProperty);
            //    var ab = Pers.Abilitis.FirstOrDefault(q => q.GUID == n.IdProperty);
            //    if (qw != null)
            //    {
            //        return IsIn(qw.NeedsTasks.Select(q => q.TaskProperty));
            //    }
            //    if (ab != null)
            //    {
            //        return IsIn(ab.NeedTasks.Select(q => q.TaskProperty));
            //    }
            //    return false;
            //}).ToList();
        }

        /// <summary>
        /// Заполняем список с фокусировкой
        /// </summary>
        private void RecountFocus()
        {
            if (Pers == null)
            {
                return;
            }

            var activeQwestsWithTasks = new List<FocusModel>();
            var abBlackList = new List<AbilitiModel>();
            var qwBlackList = new List<Aim>();
            var tskBlackList = new List<Task>();

            foreach (var taskk in Tasks)
            {
                var tsk = (Task)taskk;
                // скилла
                var abs =
                    Pers.Abilitis.Where(n => n.IsEnebledProperty)
                        .Where(n => n.SkillsActive.Any(q => q == taskk))
                        .ToList();

                foreach (var abil in abs)
                {
                    if (abBlackList.Any(n => n == abil))
                    {
                        continue;
                    }

                    var abFocusItem = GetItemToFocus(abil, Colors.White, Visibility.Visible, true);
                    var allTsks = GetAllTasksToAbility(abil, ref abFocusItem);
                    allTsks = allTsks.Distinct().ToList();
                    var activeNoBlackList =
                        allTsks.Except(tskBlackList).ToList();
                    if (!activeNoBlackList.Any())
                    {
                        continue;
                    }
                    tskBlackList.AddRange(allTsks);
                    abFocusItem.Tasks = allTsks;
                    abBlackList.Add(abil);
                    abFocusItem.SubFocusItems = abFocusItem.SubFocusItems.Distinct(new FocusModelComparer()).ToList();
                    activeQwestsWithTasks.Add(abFocusItem);
                }

                foreach (
                    var qwest in
                        Pers.Aims.Where(n => n.IsActiveProperty)
                            .Where(n => n.NeedsTasks.Any(q => q.TaskProperty == tsk)))
                {
                    if (qwBlackList.Any(n => n == qwest))
                    {
                        continue;
                    }
                    var qwestfocusItem = GetItemToFocus(qwest, Colors.White, Visibility.Visible, true);
                    qwestfocusItem.SubFocusItems = new List<FocusModel>();
                    qwestfocusItem.SubFocusItems.Add(qwestfocusItem);
                    var allThisTasks = GetAllTasksForQwest(qwest);
                    var activeNoBlackList =
                        allThisTasks.Except(tskBlackList).ToList();
                    if (!activeNoBlackList.Any())
                    {
                        continue;
                    }
                    tskBlackList.AddRange(activeNoBlackList);
                    qwestfocusItem.Tasks = allThisTasks.OrderBy(n => n).ToList();
                    qwBlackList.Add(qwest);
                    qwestfocusItem.SubFocusItems =
                        qwestfocusItem.SubFocusItems.Distinct(new FocusModelComparer()).ToList();
                    activeQwestsWithTasks.Add(qwestfocusItem);
                }
            }

            ActiveQwestsWithTasks = activeQwestsWithTasks;
        }

        /// <summary>
        /// Обновляем видимость локаций
        /// </summary>
        private void RefreshLocationsVisibility()
        {
            var orderedTasks = Pers.Tasks.OrderBy(n => n).ToList();

            foreach (var viewsModel in Pers.Views)
            {
                var tasksThisView =
                    Pers.Tasks.Where(
                        n => IsTaskVisibleInCurrentView(n, viewsModel, Pers, false, false, IsPlaningMode, false))
                        .ToList();

                viewsModel.IsVisible = tasksThisView.Any();
            }

            Pers.RefreshOrderedLocations();

            if (SelectedView == null || SelectedView.IsVisible == false)
            {
                SelectedView = Pers.OrderedViewsModel.FirstOrDefault();
            }
        }

        /// <summary>
        /// Обновить выбранный фокус
        /// </summary>
        /// <param name="item"></param>
        private void RefreshSelFoc(string item, bool isOnlyAddOrEdit = false)
        {
            if (string.IsNullOrEmpty(item))
            {
                IsFocTaksVisibility = Visibility.Collapsed;
                if (Tasks.Contains(tskSelFoc))
                {
                    SellectedTask = tskSelFoc;
                }
                foreach (Task task in Tasks)
                {
                    task.RefreshBackGround();
                }
                //Pers.PersSettings.IsGhostBastersMode = false;
                return;
            }

            var abil = Pers.Abilitis.FirstOrDefault(n => n.GUID == item);
            var qwest = Pers.Aims.FirstOrDefault(n => n.GUID == item);
            var selFoc = new FocusModel();

            if (abil != null)
            {
                selFoc = GetItemToFocus(abil, Colors.Transparent, Visibility.Visible, true);
                var skills =
                    abil.Skills.Select(n => n.NeedTask.TaskProperty)
                        .OrderBy(n => n)
                        .ToList();

                //if (!IsPlaningMode)
                //{
                //    skills = new List<Task> { skills.FirstOrDefault() };
                //}

                selFoc.Skills = null;
                selFoc.Tasks = new List<Task>();

                // Задачи скиллов
                foreach (var skill in skills)
                {
                    selFoc.Tasks.Add(skill);
                }

                // В ссылках на квесты
                if (!IsPlaningMode)
                {
                    var needTaskses = abil.LinkedQwests.Where(n => n.IsActiveProperty)
                        .ToList().Select(aim => aim.NeedsTasks.OrderBy(
                            n => n.TaskProperty).FirstOrDefault(n => IsTaskVisibleInCurrentView(n.TaskProperty, null, Pers, false, true, false, true)));
                    var firstOrDefault = needTaskses.Where(n => n?.TaskProperty != null).OrderBy(n => n.TaskProperty).ToList();
                    selFoc.Tasks.AddRange(firstOrDefault.Select(n => n.TaskProperty));
                }
                else
                {
                    foreach (var firstActive in abil.LinkedQwests.Where(n => n.IsActiveProperty)
                        .ToList().SelectMany(aim => aim.NeedsTasks.Where(
                            n => IsTaskVisibleInCurrentView(n.TaskProperty, null, Pers, false, true, false, true)))
                            .OrderBy(n => n.TaskProperty)
                        .ToList())
                    {
                        if (!selFoc.Tasks.Contains(firstActive.TaskProperty))
                        {
                            selFoc.Tasks.Add(firstActive.TaskProperty);
                        }
                    }
                }

                // В ссылках-задачах
                //foreach (var skill in skills)
                //{
                //    var qwestWithLinks =
                //        Pers.Aims.Where(n => n.IsActiveProperty).Where(n => n.LinksOfTasks.Any(q => q == skill));

                // if (!IsPlaningMode) { foreach ( var tsk in qwestWithLinks.Select( j =>
                // j.NeedsTasks.OrderBy(z => z.TaskProperty) .FirstOrDefault( n =>
                // IsTaskVisibleInCurrentView(n.TaskProperty, null, Pers, false, true, false, true)))
                // .Where(firstActive => firstActive != null) .Select(n => n.TaskProperty)) { if
                // (!selFoc.Tasks.Contains(tsk)) { selFoc.Tasks.Add(tsk); } } } else { foreach ( var
                // tsk in qwestWithLinks.SelectMany( j => j.NeedsTasks.OrderBy(z => z.TaskProperty)
                // .Where( n => IsTaskVisibleInCurrentView(n.TaskProperty, null, Pers, false, true,
                // IsPlaningMode, true)).Select(n => n.TaskProperty))) { if
                // (!selFoc.Tasks.Contains(tsk)) { selFoc.Tasks.Add(tsk); } } }

                //    selFoc.Tasks.Add(skill);
                //}
            }

            //---------------------------------------------------------------------------------
            if (qwest != null)
            {
                selFoc = GetItemToFocus(qwest, Colors.Transparent, Visibility.Visible, true);
                selFoc.Skills = null;
                var qwTsks = qwest.NeedsTasks.Select(n => n.TaskProperty).ToList();
                var tasks =
                    qwTsks
                        .Select(n => n)
                        .OrderBy(n => n)
                        .ToList();
                selFoc.Tasks = new List<Task>();

                if (!IsPlaningMode)
                {
                    var firstOrDefault = tasks.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        var qwestsWithThisTask =
                            Pers.Aims.Where(n => n.NeedsTasks.Any(q => q.TaskProperty == firstOrDefault)).ToList();
                        if (qwestsWithThisTask.Any() && qwestsWithThisTask.Any(
                            n =>
                                n.NeedsTasks.OrderBy(q => q.TaskProperty).Select(q => q.TaskProperty).FirstOrDefault() ==
                                firstOrDefault))
                        {
                            selFoc.Tasks.Add(firstOrDefault);
                        }
                    }
                }
                else
                {
                    selFoc.Tasks.AddRange(tasks);
                }

                var relAbs = Pers.Abilitis.Where(n => n.NeedAims.Any(q => q.AimProperty == qwest));

                //var relAbsLinks = (from abilitiModel in Pers.Abilitis
                //                   where abilitiModel.IsEnebledProperty
                //                   let sk = (from skill in abilitiModel.Skills.Select(n => n.NeedTask.TaskProperty)
                //                   where qwest.LinksOfTasks.Contains(skill)
                //                   where IsTaskVisibleInCurrentView(skill, null, Pers, true, true, false, true)
                //                   select skill).ToList()
                //                   where sk.FirstOrDefault()!=null
                //                   select sk.FirstOrDefault()).ToList();

                //foreach (var relAbsLink in relAbsLinks)
                //{
                //    selFoc.Tasks.Add(relAbsLink);
                //}

                selFoc.Tasks.AddRange(
                       qwest.AbilitiLinksOf.Union(relAbs)
                           .SelectMany(n => n.NeedTasks.Select(q => q.TaskProperty).Where(q => IsTaskVisibleInCurrentView(q, null, Pers, false, true, false, true)).OrderBy(q => q))
                           );

                //selFoc.Tasks.AddRange(
                //    qwest.AbilitiLinksOf.Union(relAbs)
                //        .Select(n => n.NeedTasks.Select(q => q.TaskProperty).OrderBy(q => q).FirstOrDefault(q => IsTaskVisibleInCurrentView(q, null, Pers, false, true, false, true)))
                //        );
            }

            selFoc.Tasks = selFoc.Tasks.Where(TasksFilter).ToList();

            SelFoc = selFoc;
            SelFoc.Tasks = SelFoc.Tasks.OrderBy(n => n).ToList();
            for (var i = 0; i < SelFoc.Tasks.Count; i++)
            {
                SelFoc.Tasks[i].SelFocTasksIndex = i;
            }

            OnPropertyChanged(nameof(IsSkillsVisible));
            OnPropertyChanged(nameof(IsTaskPanelVisibility));
            OnPropertyChanged(nameof(IsPipBoyVisibility));

            var tsks = FocTasks.Cast<Task>().ToList();
            var toDel = tsks.Except(SelFoc.Tasks).ToList();
            var toNew = SelFoc.Tasks.Except(tsks).ToList();
            foreach (var tt in toDel)
            {
                FocTasks.Remove(tt);
            }
            foreach (var tt in toNew)
            {
                FocTasks.AddNewItem(tt);
            }
            FocTasks.CommitNew();

            Task.GetLinksOfTasks(FocTasks.Cast<Task>());
            foreach (Task focTask in FocTasks)
            {
                focTask.LinkedTasks = new List<Task>();
            }
            SelectFirstQwestTaskInFoc();
        }

        private void RefreshTasksInMain(bool isOnlyAddOrdelete)
        {
            RefreshTasksPriority(false);
            reorderTasks();
            Tasks.Refresh();
            reorderTasks();

            var enumerable = Tasks.Cast<Task>().ToList();
            Task.GetLinksOfTasks(enumerable);
            if (IsPlaningMode)
            {
                foreach (Task task in Tasks)
                {
                    task.LinkedTasks = new List<Task>();
                }
            }
            if (IsFocTaksVisibility == Visibility.Collapsed &&
                (Tasks.CurrentItem == null || (Task)Tasks.CurrentItem != SellectedTask))
            {
                Tasks.MoveCurrentToFirst();
            }
        }

        /// <summary>
        /// The remove task.
        /// </summary>
        private void removeTask(Task tsk)
        {
            tsk.Delete(Pers);
            RefreshTasksInMainView();
        }

        /// <summary>
        /// Перераспределить скиллы если активны очки скиллов
        /// </summary>
        private void ReorderAbilitisIfPointsEnable()
        {
            // Для очков скиллов
            if (Pers.PersSettings.IsAbPointsActiveProperty)
            {
                StaticMetods.AbillitisRefresh(Pers);
            }
        }

        private void reorderTasks()
        {
            foreach (var t in Tasks.Cast<Task>().ToList())
            {
                Tasks.EditItem(t);
                Tasks.CommitEdit();
            }
        }

        private void ResetTasksTimers()
        {
            // Сбрасываем таймер!
            foreach (var task in Pers.Tasks)
            {
                task.TimerActiveProperty = Visibility.Collapsed;
                foreach (var subTask in task.SubTasks)
                {
                    subTask.TimerActiveProperty = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Штраф для перса за 0 хп!
        /// </summary>
        private void RIP()
        {
            var vc = new ViewChangesClass(Pers.InventoryItems.Union(Pers.ShopItems).ToList());
            vc.GetValBefore();

            foreach (
                var task in
                    Pers.Tasks.Where(n => n.IsDelProperty == false && n.Recurrense.TypeInterval != TimeIntervals.Нет))
            {
                var d = task.ValueOfTaskProperty * 0.67;
                task.ValueOfTaskProperty = Math.Floor(d);
            }

            Pers.HPProperty.MaxHPProperty = StaticMetods.GetMaxHP(Pers);
            Pers.HPProperty.CurrentHPProperty = Pers.HPProperty.MaxHPProperty;

            // Штрафуем на 1 уровень
            if (Pers.PersLevelProperty >= 1)
            {
                Pers.PlusExpFromTasksProperty = Pers.ExpToLevel(Pers.PersLevelProperty - 1, RpgItemsTypes.exp);
            }

            Pers.GoldProperty = 0;
            Pers.InventoryItems.Clear();
            StaticMetods.RecauntAllValues();

            vc.GetValAfter();

            var header = "Ой! Критические повреждения!";
            Brush col = Brushes.Red;
            var itemImageProperty =
                StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "bad.png"));

            StaticMetods.PlaySound(Resources.death);
            vc.ShowChanges(header, col, itemImageProperty);

            Pers.UpdateAbilityPoints();
            RefreshTasksInMainView();
        }

        /// <summary>
        /// Метод для загрузки простого примера
        /// </summary>
        private void SampleTemplateLoadVoid()
        {
            LoadPersFromPath(
                Path.Combine(Directory.GetCurrentDirectory(), "Images", "Templates", "SampleTemplate"));
        }

        private void savePersToPath(string pathToSave)
        {
            if (string.IsNullOrEmpty(pathToSave) == false)
            {
                try
                {
                    // Сохраняем персонажа с его задачами
                    using (var fs = new FileStream(pathToSave, FileMode.Create))
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(fs, Pers);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при экспорте данных! Возможно проблема в правах доступа.");
                }
            }
        }

        ///// <summary>
        ///// Сохраняем персонаж для архива
        ///// </summary>
        //private void saveToArchive()
        //{
        //    var pathToPers = Path.Combine(appFolder, DateTime.Now.DayOfWeek.ToString());

        // if (!Directory.Exists(appFolder)) { Directory.CreateDirectory(appFolder); }

        //    savePersToPath(pathToPers);
        //}

        private void SelectFirstQwestTaskInFoc(bool notCheck = false)
        {
            if (notCheck || (IsFocTaksVisibility == Visibility.Visible &&
                (FocTasks.CurrentItem == null || (Task)FocTasks.CurrentItem != SellectedTask)))
            {
                var firstOrDefault = FocTasks.Cast<Task>().FirstOrDefault(n => n.Recurrense.TypeInterval == TimeIntervals.Нет);
                if (firstOrDefault != null)
                {
                    FocTasks.MoveCurrentTo(firstOrDefault);
                }
                else
                {
                    FocTasks.MoveCurrentToFirst();
                }
                SellectedTask = (Task)FocTasks.CurrentItem;
            }
        }

        private void SelectFirstTask()
        {
            if (IsFocTaksVisibility == Visibility.Visible)
            {
                SelectFirstQwestTaskInFoc();
                //FocTasks.MoveCurrentToFirst();
                //SellectedTask = (Task) FocTasks.CurrentItem;
            }
            else
            {
                if (LastParTask != null && Tasks.Contains(LastParTask))
                {
                    SellectedTask = LastParTask;
                }
                else
                {
                    Tasks.MoveCurrentToFirst();
                    SellectedTask = (Task)Tasks.CurrentItem;
                    //if (Tasks.Contains(SellectedTask))
                    //{
                    //    LastParTask = SellectedTask;
                    //}
                }
            }

            // OnPropertyChanged(nameof(LastParTask)); OnPropertyChanged(nameof(SellectedTask));
        }

        /// <summary>
        /// Активировать таймер потребностей
        /// </summary>
        private void setNeednessTimer()
        {
            timerNeedness.Interval = 60000;
            timerNeedness.Tick += (sender, args) =>
            {
                if (Pers.PersSettings.EnableNeednessProperty)
                {
                    foreach (var needness in Pers.NeednessCollection)
                    {
                        needness.minusNeedness();
                        StaticMetods.terribleBuffIfNeed(Pers);
                    }
                }
            };

            timerNeedness.Start();
        }

        private void SetSW(Stopwatch sw, List<long> times, int i)
        {
            if (times.Count != 0)
            {
                var last = times.Last();
                times.Add(sw.ElapsedMilliseconds - last);
            }
            else
            {
                times.Add(sw.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// The set timer.
        /// </summary>
        private void SetTimer()
        {
            timerHP.Interval = 60000;
            timerHP.Tick += (sender, args) =>
            {
                MinutesProperty--;

                if (MinutesProperty < 0)
                {
                    if (Pers.PersSettings.HourseToNullHpProperty == 0)
                    {
                        Pers.HPProperty.CurrentHPProperty -= Pers.PersSettings.MinusHPPerHourProperty;
                    }
                    else
                    {
                        var minusHP = Convert.ToDouble(Pers.HPProperty.MaxHPProperty)
                                      / Pers.PersSettings.HourseToNullHpProperty;

                        Pers.HPProperty.CurrentHPProperty -= Convert.ToInt32(minusHP);
                    }

                    MinutesProperty = 60;
                }
            };

            MinutesProperty = 60;
            if (Pers.PersSettings.IsHpMinusForTimerProperty)
            {
                timerHP.Start();
            }
        }

        /// <summary>
        /// The show characteristic.
        /// </summary>
        /// <param name="_cha">The _cha.</param>
        private void showCharacteristic(Characteristic _cha)
        {
            _cha.EditCharacteristic();
            Refresh();
        }

        private void ShowGhostBastersMode()
        {
            //Pers.PersSettings.IsGhostBastersMode = true;
            //foreach (Task focTask in FocTasks)
            //{
            //    focTask.RefreshBackGround();
            //}
        }

        private void shrinkExpand(string item)
        {
            if (item == "показать")
            {
                if (Pers.IsHideHUD == false) return;
                Pers.IsHideHUD = false;
            }
            else
            {
                if (Pers.IsHideHUD) return;
                Pers.IsHideHUD = true;
            }
            OnPropertyChanged(nameof(HeightOfDataGrid));
            OnPropertyChanged(nameof(ExpandTaskPanelText));
        }

        private void SmartSelect(Task thisTask, Task nextTask)
        {
            if (Tasks.Contains(thisTask) && TasksForInterfacePanel.Any(n => n.Task == thisTask))
            {
                SellectedTask = thisTask;
            }
            else if (Tasks.Contains(nextTask) && TasksForInterfacePanel.Any(n => n.Task == nextTask))
            {
                SellectedTask = nextTask;
            }
            else
            {
                if (IsFocMode)
                {
                    if (TasksFilter(LastParTask) && TasksForInterfacePanel.Any(n => n.Task == LastParTask))
                    {
                        SellectedTask = LastParTask;
                    }
                    else if (Tasks.Contains(nextTask) && TasksForInterfacePanel.Any(n => n.Task == nextTask))
                    {
                        SellectedTask = nextTask;
                    }
                    else
                    {
                        SelectFirstTask();
                    }
                }
                else
                {
                    if (nextTask == null)
                    {
                        SelectFirstTask();
                    }
                    else
                    {
                        SellectedTask = nextTask;
                    }
                }

                // Обновить здесь ссылки
                LastParTask?.RefreshLinkedTasksNotify();
            }
        }

        private Task TryGetTaskByIndex(int tInd)
        {
            if (Tasks.Count > tInd && tInd >= 0)
            {
                return (Task)Tasks.GetItemAt(tInd);
            }

            return null;
        }

        /// <summary>
        /// Обновить ранги скиллов
        /// </summary>
        private void UpdateAbilitisRangs()
        {
            foreach (var abilitiModel in Pers.Abilitis)
            {
                abilitiModel.Rangs =
                    GetDefoultRangsForAbilitis(Pers.PersSettings.AbilRangsForDefoultProperty)();
            }
        }

        /// <summary>
        /// Обновить ранги всех задач
        /// </summary>
        private void UpdateTasksRangs()
        {
            var privichki = from task in Pers.Tasks
                            where
                                task.Recurrense.TypeInterval != TimeIntervals.Сразу
                                && task.Recurrense.TypeInterval != TimeIntervals.Нет
                            select task;

            var obuchenie = from task in Pers.Tasks
                            where task.Recurrense.TypeInterval == TimeIntervals.Сразу
                            select task;

            foreach (var taskk in privichki)
            {
                taskk.MaxValueOfTaskProperty = StaticMetods.Config.AbOneLevelDays;
                taskk.ChangeValueIfNotDoneProperty =
                    StaticMetods.PersProperty.PersSettings.MinusForDefoultForPrivichkaProperty;
                taskk.setTaskRangse();
            }

            foreach (var taskk in obuchenie)
            {
                taskk.MaxValueOfTaskProperty = StaticMetods.PersProperty.PersSettings.TaskCountForEducationProperty;
                taskk.ChangeValueIfNotDoneProperty = 0;
                taskk.setTaskRangse();
            }
        }

        /// <summary>
        /// Автоиспользование бутылочек здоровья
        /// </summary>
        private void UseBottles()
        {
            if (Pers.HPProperty.CurrentHPProperty > 0 ||
                (Pers.BigHpBottles <= 0 && Pers.MiddleHpBottles <= 0 && Pers.SmallHpBottles <= 0))
            {
                return;
            }

            if (Pers.SmallHpBottles > 0 && Pers.HPProperty.CurrentHPProperty <= 0)
            {
                StaticMetods.UseHPBottle(Pers, 10);
                UseBottles();
            }

            if (Pers.MiddleHpBottles > 0 && Pers.HPProperty.CurrentHPProperty <= 0)
            {
                StaticMetods.UseHPBottle(Pers, 20);
                UseBottles();
            }

            if (Pers.BigHpBottles > 0 && Pers.HPProperty.CurrentHPProperty <= 0)
            {
                StaticMetods.UseHPBottle(Pers, 40);
                UseBottles();
            }
        }

        /// <summary>
        /// Обновляем данные для отображения уровня и опыта персонажа
        /// </summary>
        private void viewLevelRefresh()
        {
            OnPropertyChanged("ExpProperty");
            OnPropertyChanged("MinExpProperty");
            OnPropertyChanged("MaxExpProperty");
            OnPropertyChanged("ExperenceProperty");
            OnPropertyChanged("LevelProperty");
            OnPropertyChanged("MaxHpProperty");
            OnPropertyChanged("HpProperty");
        }

        private void WithAbilitis(List<FocusModel> activeQwestsWithTasks)
        {
            var blackList = new List<string>();

            foreach (var taskk in Tasks)
            {
                var task = taskk as Task;

                foreach (
                    var abilitiModel in
                        Pers.Abilitis.Where(n => blackList.All(q => q != n.GUID))
                            .Where(n => n.RelatedTasks().Any(q => q == task)))
                {
                    var abFocusItem = GetItemToFocus(abilitiModel, Colors.White, Visibility.Collapsed,
                        true);

                    activeQwestsWithTasks.Add(abFocusItem);

                    foreach (
                        var aim in
                            abilitiModel.ComplecsNeeds.Where(n => n.NeedQwest != null)
                                .Select(n => n.NeedQwest.AimProperty)
                                .Where(n => n.RelatedTasks().Any(q => IsTaskVisibleInCurrentView(q, null, Pers))))
                    {
                        var qwestFocusItem = GetItemToFocus(aim, Colors.White, Visibility.Collapsed,
                            true);

                        abFocusItem.SubFocusItems.Add(qwestFocusItem);
                    }

                    blackList.Add(abilitiModel.GUID);
                }
            }
        }
    }

    internal static class StringExtensions
    {
        public static string SplitInParts(this string s, int maxChunkSize)
        {
            var cc = Convert.ToDouble(s).ToString("N0", new NumberFormatInfo
            {
                NumberGroupSizes = new[] { maxChunkSize },
                NumberGroupSeparator = "'"
            });

            return cc;
        }
    }

    internal class RightRells
    {
        public string Guid { get; set; }
        public string Name { get; set; }

        public BitmapImage PictureProperty { get; set; }
    }
}
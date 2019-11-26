using Sample.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sample.Model
{
    public class CONTEXT
    {
        public int ID { get; set; }
        public string UUID { get; set; }
        public int PARENT { get; set; }
        public int CHILDREN { get; set; }
        public string CREATED { get; set; }
        public string MODIFIED { get; set; }
        public string TITLE { get; set; }
        public int COLOR { get; set; }
        public int VISIBLE { get; set; }
    }

    public class TAG
    {
        public int ID { get; set; }
        public string UUID { get; set; }
        public string CREATED { get; set; }
        public string MODIFIED { get; set; }
        public string TITLE { get; set; }
        public int COLOR { get; set; }
        public int VISIBLE { get; set; }
    }

    public class TASK
    {
        private string _dueDate = "";
        private string _startDate = "";
        private string _completed = "";
        public int ID { get; set; }
        public string UUID { get; set; } = "";
        public int PARENT { get; set; } = 0;
        public string CREATED { get; set; }
        public string MODIFIED { get; set; }
        public string TITLE { get; set; }

        public string START_DATE
        {
            get { return _startDate; }
            set
            {
                value = value.Replace(".", "-");
                _startDate = value;
            }
        }

        public int START_TIME_SET { get; set; }

        public string DUE_DATE
        {
            get { return _dueDate; }
            set
            {
                value = value.Replace(".", "-");
                _dueDate = value;
            }
        }

        public string DUE_DATE_PROJECT { get; set; } = "";
        public int DUE_TIME_SET { get; set; } = 0;
        public string DUE_DATE_MODIFIER { get; set; } = "0";
        public int REMINDER { get; set; } = -1;
        public string ALARM { get; set; } = "";
        public string REPEAT_NEW { get; set; } = "";
        public int REPEAT_FROM { get; set; } = 1;
        public int DURATION { get; set; } = 0;
        public int STATUS { get; set; } = 0;
        public int CONTEXT { get; set; }
        public int GOAL { get; set; } = 0;
        public int FOLDER { get; set; } = 0;
        public List<object> TAG { get; set; } = new List<object>();
        public int STARRED { get; set; } = 0;
        public int PRIORITY { get; set; }
        public string NOTE { get; set; } = "";

        public string COMPLETED
        {
            get
            {
                if (STARRED == 1)
                {
                    return "";
                }

                if (!string.IsNullOrEmpty(_completed))
                {
                    var task = StaticMetods.PersProperty.Tasks.FirstOrDefault(n => n.GUID == NOTE);
                    if (task == null)
                    {
                        return "";
                    }

                    if (task.Recurrense.TypeInterval == TimeIntervals.Нет)
                    {
                        return _completed;
                    }
                    else
                    {
                        return DUE_DATE;
                    }
                }

                return _completed;
            }
            set { _completed = value; }
        }

        public int TYPE { get; set; } = 0;
        public string TRASH_BIN { get; set; } = "";
        public int IMPORTANCE { get; set; } = 0;
        public string METAINF { get; set; } = "";
        public int FLOATING { get; set; } = 0;
        public int HIDE { get; set; }
        public long HIDE_UNTIL { get; set; }
    }

    public class RootObject
    {
        public int version { get; set; }
        public List<CONTEXT> CONTEXT { get; set; }
        public List<TAG> TAG { get; set; }
        public List<TASK> TASK { get; set; }

        /// <summary>
        ///     Сгенерить задачу для json
        /// </summary>
        /// <param name="root"></param>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static TASK GetTaskJSON(RootObject root, ref int id, Task task)
        {
            var repeatNew = "";
            var repeatFrom = 0;
            var dueDateModifier = "0";
            var dueDate = task.EndDate;
            var priority = 0;

            switch (task.Recurrense.TypeInterval)
            {
                case TimeIntervals.Нет:
                    dueDateModifier = "2";

                    repeatNew = "";
                    repeatFrom = 0;
                    dueDate = task.BeginDateProperty == DateTime.MinValue
                        ? MainViewModel.selectedTime
                        : task.BeginDateProperty;
                    priority = -1;
                    break;

                case TimeIntervals.Сразу:
                    dueDateModifier = "1";
                    repeatFrom = 1;

                    repeatNew = "Every 1 day";
                    dueDate = task.EndDate;
                    priority = 1;
                    break;
                case TimeIntervals.Ежедневно:
                    dueDateModifier = "1";
                    repeatFrom = 1;

                    repeatNew = "Daily";
                    dueDate = task.EndDate;
                    priority = 1;
                    break;
                case TimeIntervals.Будни:
                    dueDateModifier = "1";
                    repeatFrom = 1;

                    repeatNew = "BusinessDay";
                    dueDate = task.EndDate;
                    priority = 1;
                    break;

                case TimeIntervals.Выходные:
                    dueDateModifier = "1";
                    repeatFrom = 1;

                    repeatNew = "Weekend";
                    dueDate = task.EndDate;
                    priority = 1;
                    break;

                case TimeIntervals.День:
                case TimeIntervals.ДниСначала:
                    dueDateModifier = "1";
                    repeatFrom = 1;

                    repeatNew = $"Every {task.Recurrense.Interval} day";
                    dueDate = task.BeginDateProperty == DateTime.MinValue
                        ? MainViewModel.selectedTime
                        : task.BeginDateProperty;
                    priority = 1;
                    break;
                case TimeIntervals.Неделя:
                case TimeIntervals.НеделиСНачала:
                    dueDateModifier = "1";
                    repeatFrom = 1;

                    repeatNew = $"Every {task.Recurrense.Interval} Weeks";
                    dueDate = task.BeginDateProperty == DateTime.MinValue
                        ? MainViewModel.selectedTime
                        : task.BeginDateProperty;
                    priority = 2;
                    break;
                case TimeIntervals.МесяцыСНачала:
                case TimeIntervals.Месяц:
                    dueDateModifier = "1";
                    repeatFrom = 1;

                    repeatNew = $"Every {task.Recurrense.Interval} Month";
                    dueDate = task.BeginDateProperty == DateTime.MinValue
                        ? MainViewModel.selectedTime
                        : task.BeginDateProperty;
                    priority = 2;
                    break;
                case TimeIntervals.ДниНедели:
                case TimeIntervals.ДниНеделиСНачала:
                    dueDateModifier = "1";
                    repeatFrom = 1;

                    var weekDays = string.Empty;
                    foreach (var source in task.DaysOfWeekRepeats.Where(n => n.CheckedProperty))
                    {
                        if (!string.IsNullOrEmpty(weekDays))
                        {
                            weekDays += ", ";
                        }

                        switch (source.Day)
                        {
                            case DayOfWeek.Monday:
                                weekDays += "Mon";
                                break;

                            case DayOfWeek.Tuesday:
                                weekDays += "Tue";
                                break;

                            case DayOfWeek.Wednesday:
                                weekDays += "Wed";
                                break;

                            case DayOfWeek.Thursday:
                                weekDays += "Thu";
                                break;

                            case DayOfWeek.Friday:
                                weekDays += "Fri";
                                break;

                            case DayOfWeek.Saturday:
                                weekDays += "Sat";
                                break;

                            case DayOfWeek.Sunday:
                                weekDays += "Sun";
                                break;
                        }
                    }

                    repeatNew = $"Every {weekDays}";
                    dueDate = task.BeginDateProperty == DateTime.MinValue
                        ? MainViewModel.selectedTime
                        : task.BeginDateProperty;

                    priority = 2;
                    break;
            }

            // Контексты для задачи
            var context = task.Recurrense.TypeInterval==TimeIntervals.Нет? root.CONTEXT[1]: root.CONTEXT[0];

            var tag = GetTagsForTask(root, task);
            dueDate = dueDate >= task.BeginDateProperty ? dueDate : task.BeginDateProperty;
            dueDate = dueDate >= MainViewModel.selectedTime ? dueDate : MainViewModel.selectedTime;
            
            var taskJson = new TASK
            {
                ID = id,
                NOTE = task.GUID,
                TITLE = task.NameOfProperty + task.GetPlusName(true),
                PRIORITY = priority,
                DUE_DATE = GetDateJSON(dueDate),
                DUE_DATE_MODIFIER = dueDateModifier,
                CONTEXT = context.ID,
                TAG = tag,
                CREATED = GetDateJSON(MainViewModel.selectedTime),
                MODIFIED = GetDateJSON(MainViewModel.selectedTime),
                REMINDER = -1,
                START_DATE = GetDateJSON(DateTime.MinValue),
                HIDE = 0,
                HIDE_UNTIL = 0,
                REPEAT_FROM = 0,
                REPEAT_NEW = repeatNew,
                START_TIME_SET = 0,
                STATUS = StaticMetods.PersProperty.Statuses.IndexOf(task.TaskStatus)
            };


            return taskJson;
        }

        /// <summary>
        /// Получить теги для задачи
        /// </summary>
        /// <param name="root"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private static List<object> GetTagsForTask(RootObject root, Task task)
        {
            var tag = new List<object>();
            var taskInQwests = task.TaskInQwests().Where(n => n.IsActiveProperty).ToList();
            var taskInAbilitis = task.TaskInAbilitis.Where(n => n.IsEnebledProperty).ToList();
            if (taskInQwests.Any() || taskInAbilitis.Any())
            {
                tag.AddRange(taskInAbilitis.Select(ab => root.TAG.First(n => n.UUID == ab.GUID).ID).Cast<object>());
                tag.AddRange(taskInQwests.Select(qwest => root.TAG.First(n => n.UUID == qwest.GUID).ID).Cast<object>());
            }

            return tag;
        }

        /// <summary>
        ///     Получить дату для JSON
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetDateJSON(DateTime dateTime)
        {
            var date = string.Empty;

            date += dateTime.Year;
            date += $"-{dateTime.Month}";
            date += $"-{dateTime.Day}";
            date += $" {dateTime.Hour}:{dateTime.Minute}";

            return date;
        }

        public static RootObject SerializeToAndroidJSON(Pers pers)
        {
            var id = 1;

            // рут
            var root = new RootObject
            {
                version = 3,
                CONTEXT = new List<CONTEXT>(),
                TAG = new List<TAG>(),
                TASK = new List<TASK>()
            };

            // Контексты
            GetContexts(pers, ref id, root);

            // Тэги
            GetTags(pers, ref id, root);

            // Задачи
            GetTasks(pers, root, id);

            return root;
        }

        private static void GetTasks(Pers pers, RootObject root, int id)
        {
            var tasks = pers.Tasks.Where(n => n.IsEnabled).OrderBy(n=>n);


            Action<Sample.Model.TASK, Task> setDateAndro = (TASK, tsk) =>
            {
                DateTime dt = new DateTime(tsk.BeginDateProperty.Year, tsk.BeginDateProperty.Month,
                   tsk.BeginDateProperty.Day);
                dt = dt.AddMinutes(tsk.EndMinutesAndroid);
                TASK.DUE_DATE = GetDateJSON(dt);
                TASK.DUE_TIME_SET = 1;
            };

            DateTime dtt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            foreach (var task in tasks)
            {
                task.EndMinutesAndroid = Convert.ToInt32(task.TimeProperty.TimeOfDay.TotalMinutes);
            }

            int counter = 100;

            // Задачи скиллов
            foreach (var task in tasks.Where(n=>n.IsSkill))
            {
                var taskJson = GetTaskJSON(root, ref id, task);
                var ctr = counter.ToString().Remove(0,1);
                taskJson.TITLE = $"{ctr}) {taskJson.TITLE}";
                counter++;
                setDateAndro(taskJson, task);
                taskJson.STATUS = 1;
                root.TASK.Add(taskJson);
                id++;
            }

            // Квесты
            var qw = pers.Aims.Where(n=>n.IsActiveProperty && !n.IsDoneProperty).OrderBy(n=>n).ToList();
            foreach (var qwest in qw)
            {
                int status = 3;

                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dt = dt.AddMinutes(qw.IndexOf(qwest));

                var qwNeeds = qwest.NeedsTasks.Select(n => n.TaskProperty).Where(n => n.IsEnabled).ToList();
                if (!qwNeeds.Any()) continue;

                // Добавляем квест
                var taskJson = new TASK
                {
                    ID = id,
                    NOTE = qwest.GUID,
                    TITLE = $"Квест \"{qwest.NameOfProperty}\"",
                    PRIORITY = 1,
                    DUE_DATE = GetDateJSON(MainViewModel.selectedTime.AddHours(23).AddMinutes(59)),
                    DUE_TIME_SET = 1,
                    DUE_DATE_PROJECT = GetDateJSON(MainViewModel.selectedTime.AddHours(23).AddMinutes(59)),
                    CREATED = GetDateJSON(MainViewModel.selectedTime),
                    MODIFIED = GetDateJSON(MainViewModel.selectedTime),
                    REMINDER = -1,
                    START_DATE = GetDateJSON(DateTime.MinValue),
                    HIDE = 0,
                    HIDE_UNTIL = 0,
                    REPEAT_FROM = 0,
                    START_TIME_SET = 0,
                    TYPE = 1,
                    STATUS = status
                };

                taskJson.TITLE = $"{counter.ToString().Remove(0,1)}) {taskJson.TITLE}";
                counter++;

                List<object> tag = qwest.AbilitiLinksOf.Select(abilitiModel => root.TAG.First(n => n.UUID == abilitiModel.GUID).ID).Cast<object>().ToList();

                foreach (var source in qwest.NeedsTasks.Select(n=>n.TaskProperty))
                {
                    tag.AddRange(GetTagsForTask(root, source));
                }

                tag = tag.Distinct().ToList();
                taskJson.CONTEXT = root.CONTEXT[1].ID;
                taskJson.STATUS = status;
                taskJson.TAG = tag;
                root.TASK.Add(taskJson);
                id++;

                int counter2 = 100;

                //Задачи квеста
                
                foreach (var tsk in qwNeeds.OrderBy(n=>n))
                {
                    var taskJson2 = new TASK
                    {
                        ID = id,
                        NOTE = tsk.GUID,
                        TITLE = tsk.NameOfProperty,
                        PRIORITY = -1,
                        CREATED = GetDateJSON(MainViewModel.selectedTime),
                        MODIFIED = GetDateJSON(MainViewModel.selectedTime),
                        REMINDER = -1,
                        START_DATE = GetDateJSON(DateTime.MinValue),
                        HIDE = 0,
                        HIDE_UNTIL = 0,
                        REPEAT_FROM = 0,
                        START_TIME_SET = 0,
                        PARENT = taskJson.ID,
                        STATUS = status
                    };

                    taskJson2.CONTEXT = root.CONTEXT[1].ID;
                    taskJson2.TITLE = $"{counter2.ToString().Remove(0,1)}) {taskJson2.TITLE}";
                    counter2++;
                    setDateAndro(taskJson2, tsk);
                    root.TASK.Add(taskJson2);
                    id++;
                }


            }
        }

        private static void GetTags(Pers pers, ref int id, RootObject root)
        {
            // Скиллы
            foreach (var abilitiModel in pers.Abilitis.Where(n => n.IsEnebledProperty).OrderBy(n=>n.NameOfProperty))
            {
                var tag = new TAG
                {
                    ID = id,
                    UUID = abilitiModel.GUID,
                    TITLE = $"__{abilitiModel.NameOfProperty}",
                    COLOR = -1048832,
                    VISIBLE = 1,
                    CREATED = GetDateJSON(MainViewModel.selectedTime),
                    MODIFIED = GetDateJSON(MainViewModel.selectedTime)
                };

                root.TAG.Add(tag);
                id++;
            }

            // Квесты
            foreach (var qwest in pers.Aims.Where(n => n.IsActiveProperty).OrderBy(n=>n.NameOfProperty))
            {
                var tag = new TAG
                {
                    ID = id,
                    UUID = qwest.GUID,
                    TITLE = $"_{qwest.NameOfProperty}",
                    COLOR = -1048832,
                    VISIBLE = 1,
                    CREATED = GetDateJSON(MainViewModel.selectedTime),
                    MODIFIED = GetDateJSON(MainViewModel.selectedTime)
                };

                root.TAG.Add(tag);
                id++;
            }
        }

        private static void GetContexts(Pers pers, ref int id, RootObject root)
        {
            var context = new CONTEXT
            {
                ID = id,
                TITLE = "Привычки",
                UUID = "0011",
                COLOR = -8876889,
                VISIBLE = 1,
                CREATED = GetDateJSON(MainViewModel.selectedTime),
                MODIFIED = GetDateJSON(MainViewModel.selectedTime)
            };

            root.CONTEXT.Add(context);

            id++;

            var context2 = new CONTEXT
            {
                ID = id,
                TITLE = "Задачи",
                UUID = "0012",
                COLOR = -8876889,
                VISIBLE = 1,
                CREATED = GetDateJSON(MainViewModel.selectedTime),
                MODIFIED = GetDateJSON(MainViewModel.selectedTime)
            };

            root.CONTEXT.Add(context2);

            id++;
        }
    }
}
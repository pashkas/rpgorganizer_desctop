using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Sample.ViewModel;

namespace Sample.Model
{
    public class CharacteristicAndroid
    {
        public CharacteristicAndroid(string name, double value, List<RPGAndroidJSON.AbilityAndroid> abilities)
        {
            Name = name;
            Value = value;
            Abilities = abilities;
        }

        public List<RPGAndroidJSON.AbilityAndroid> Abilities { get; set; }
        public string Name { get; set; }

        public double Value { get; set; }
    }

    public class RPGAndroidJSON
    {
        public static string getAndroTaskDateString(DateTime dt)
        {
            return dt.ToString("dd.MM.yyyy");
        }

        /// <summary>
        ///     Из строки дата на андроиде получить дату обычную
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime getDateFromTaskAndroDateString(string str)
        {
            DateTime dat = DateTime.MinValue;
            DateTime.TryParseExact(str, "dd.MM.yyyy", null,
                DateTimeStyles.None, out dat);
            return dat;
        }

        public static RootObject SerializeToAndroidJSON(Pers pers, string path)
        {
            StaticMetods.Locator.MainVM.RefreshTasksPriority(false);
            // Задаем индекс для задач
            int i = 0;
            var ordered = pers.Tasks.OrderByDescending(n => n).ToList();//pers.Tasks.OrderBy(n => n).ToList();

            foreach (var task in ordered)
            {
                task.TaskIndex = i;
                i++;
            }

            RootObject ro = new RootObject
            {
                PersName = pers.NameOfProperty,
                PersRang = pers.RangName,
                PersExp = pers.PersExpProperty,
                Is10AbLevels = pers.PersSettings.Is10AbLevels,
                Is5_5_50 = pers.PersSettings.Is5_5_50,
                IsFUDGE = pers.PersSettings.IsFUDGE,
                IsParetto = pers.IsParetto,
                DateOfLastUse = pers.DateOfLastUse.ToString("dd.MM.yyyy"),
                IsShowTasksByTimeAndroid = pers.PersSettings.IsShowTasksByTimeAndroid,
                TimeOfLastUse = string.Empty,
                IsSmartSort = pers.PersSettings.IsSmartTaskSort,
                IsSortByPrioryty = pers.IsSortByPrioryty,
                MaxGettedExp = pers.MaxGettedExp,
                HP = pers.HP,
                MaxHP = pers.MaxHP,
                BalanceIs50Levels = pers.BalanceIs50Levels,
                BalanceForFirstLevel = pers.BalanceForFirstLevel,
                BalanceForLastLevel = pers.BalanceForLastLevel,
                BalanceLevels = pers.BalanceLevels
            };

            var tasks =
                pers.Tasks.Where(n => n.IsEnabled)
                    .Where(n => !n.RelToQwests.Any() && !n.TaskInAbilitis.Any())
                    .OrderBy(n => n)
                    .ToList();

            List<Task> tsks = tasks.Select(GetTask).ToList();
            ro.Tasks = new List<Task>();
            ro.Qwests = new List<Qwest>();
            ro.Abils = new List<AbilityAndroid>();
            ro.Characts = new List<CharacteristicAndroid>();

            foreach (var cha in pers.Characteristics)
            {
                CharacteristicAndroid chaA = new CharacteristicAndroid(cha.NameOfProperty, cha.CellValue,
                    cha.RelayAbilitys.Select(n => new AbilityAndroid(n.AbilProperty.NameOfProperty, n.AbilProperty.CellValue)).ToList());

                ro.Characts.Add(chaA);
            }

            // скиллы заполняем
            var ab = pers.Abilitis.Where(n => n.IsEnebledProperty).OrderBy(n => n).ToList();

            foreach (var abilitiModel in ab)
            {
                ro.Abils.Add(new AbilityAndroid()
                {
                    Guid = abilitiModel.GUID,
                    Name = abilitiModel.NameOfProperty,
                    Modificator = abilitiModel.Booster,
                    Value = abilitiModel.ValueProperty,
                    LastValue = abilitiModel.LastValue,
                    ExpOneLevel = abilitiModel.getPriorOfAb(abilitiModel)
                });

                Qwest qww = new Qwest();
                qww.NotOneByOne = !pers.PersSettings.IsHideTasksOneByOneAndroid;
                qww.NameOfQwest = abilitiModel.NameOfProperty;
                qww.Guid = abilitiModel.GUID;

                var tskss =
                    abilitiModel.NeedTasks
                        .Where(n => n.TaskProperty.IsEnabled)
                        .Where(n =>
                        {
                            if (StaticMetods.PersProperty.IsParetto && n.TaskProperty.Recurrense.TypeInterval != TimeIntervals.Нет && n.TaskProperty.isSuper == false)
                            {
                                return false;
                            }

                            return true;
                        })
                        .Select(n => n.TaskProperty)
                        .OrderBy(n => n)
                        .ToList();

                var tsk = tskss.Select(GetTask).ToList();
                //tsk.ForEach(n => n.LinksOf = abilitiModel.NameOfProperty);

                foreach (Task task in tsk)
                {
                    task.SubTasksByQwests = GetSubTasksByQwests(abilitiModel, pers, task);
                }

                qww.Tasks = tsk;
                ro.Qwests.Add(qww);
            }

            // А теперь квесты заполняем
            var qw = pers.Aims.Where(QwestsActiveForAndroid()).OrderBy(n => n).ToList();

            foreach (var qwest in qw)
            {
                Qwest qww = new Qwest();
                qww.NameOfQwest = qwest.NameOfProperty;
                qww.Guid = qwest.GUID;

                var tskss =
                    qwest.NeedsTasks.Where(n => !n.TaskProperty.IsDelProperty)
                        .Select(n => n.TaskProperty)
                        .OrderBy(n => n)
                        .ToList();

                qww.Tasks = tskss.Select(GetTask).ToList();

                foreach (var task in qww.Tasks.Skip(1))
                {
                    var fod = qww.Tasks.FirstOrDefault();
                    if (fod != null)
                    {
                        if (getDateFromTaskAndroDateString(task.Date) < getDateFromTaskAndroDateString(fod.Date))
                        {
                            task.Date = fod.Date;
                        }
                    }
                }

                //List<string> lnkLst = new List<string>();
                //lnkLst.AddRange(qwest.AbilitiLinksOf.Select(n=>n.NameOfProperty));

                string lnkOf = qwest.NameOfProperty;

                if (qwest.AbilitiLinksOf.Any())
                {
                    lnkOf = " ⇉ ";
                }
                // + " ⇉ " + string.Join(", ", lnkLst);

                qww.Tasks.ForEach(n => n.LinksOf = lnkOf);

                if (qwest.AbilitiLinksOf.Any())
                {
                    foreach (var task in qww.Tasks)
                    {
                        task.LinkedQwests = qwest.AbilitiLinksOf.Select(n => n.GUID).ToList();
                    }
                }

                foreach (var task in qww.Tasks)
                {
                    task.SubTasksByQwests = GetSubTasksByQwests(null, pers, task);
                }

                ro.Qwests.Add(qww);
            }

            // Заполняем просто задачи, но запихиваем их в квест
            Qwest qw1 = new Qwest();
            qw1.NameOfQwest = "Прочее";
            qw1.Guid = Guid.NewGuid().ToString();
            qw1.Tasks = tsks.Distinct().ToList();
            qw1.NotOneByOne = true;
            foreach (var n in qw1.Tasks)
            {
                n.LinksOf = qw1.NameOfQwest;
            }
            ro.Qwests.Add(qw1);

            foreach (var task in ro.Tasks.ToList())
            {
                AndroSub(pers, task, ro);
                AndroSetTask(task, path);
            }

            foreach (var qwest in ro.Qwests.ToList())
            {
                foreach (var task in qwest.Tasks)
                {
                    AndroSub(pers, task, ro);
                    AndroSetTask(task, path);
                }
            }

            return ro;
        }

        private static void AndroSetTask(Task task, string path)
        {
            var fi = new FileInfo(task.Image);
            var fiName = fi.Name;
            var combine = Path.Combine(path, fiName);
            var exists = File.Exists(combine);
            if (!exists)
            {
                File.Copy(task.Image, combine);
            }

            task.Image = fiName;
        }

        /// <summary>
        /// Подзадачи для андроида
        /// </summary>
        /// <param name="pers"></param>
        /// <param name="task"></param>
        /// <param name="ro"></param>
        private static void AndroSub(Pers pers, Task task, RootObject ro)
        {
            var tskInMain = pers.Tasks.FirstOrDefault(n => n.GUID == task.UUID);
            if (tskInMain != null)
            {
                var subNotDone = tskInMain.SubTasks.Where(n => !n.isDone).ToList();
                if (subNotDone.Any())
                {
                    Qwest qww = new Qwest();
                    qww.NameOfQwest = "↑↑↑";
                    qww.Guid = task.UUID;
                    qww.Tasks = new List<Task>();
                    foreach (var sub in subNotDone)
                    {
                        var item = GetTask(tskInMain);
                        item.UUID = sub.Guid;
                        item.NameOf = sub.Tittle;
                        item.Reccurrense = "NONE";
                        item.Wave = task.Wave;
                        item.isSuper = task.isSuper;
                        item.Index = Int32.MaxValue;
                        item.LinksOf = "↑↑↑";
                        qww.Tasks.Add(item);
                    }

                    foreach (var t in qww.Tasks)
                    {
                        var linkQwest = ro.Qwests.Where(n => n.Tasks.Any(q => q == task)).Select(n => n.Guid).ToList();
                        t.LinkedQwests = linkQwest;
                    }

                    var subTasks = subNotDone.Select(n => n.Guid);
                    task.SubTasksByQwests.AddRange(subTasks);
                    ro.Qwests.Add(qww);
                }
            }
        }

        /// <summary>
        ///     Узнаем задачи-ссылки от квестов
        /// </summary>
        /// <param name="ab">навык</param>
        /// <param name="prs"></param>
        /// <param name="task">задача</param>
        /// <returns></returns>
        private static List<string> GetSubTasksByQwests(AbilitiModel ab, Pers prs, Task task)
        {
            List<string> subTasksByQwests = new List<string>();
            if (ab != null)
            {
                subTasksByQwests = prs.Aims.Where(
                QwestsActiveForAndroid()).Where(n => n.AbilitiLinksOf.Any(q => q == ab))
                .SelectMany(n => n.NeedsTasks.Select(q => q.TaskProperty))
                .Where(n => !n.IsDelProperty)
                .Select(n => n.GUID)
                .ToList();
            }

            return
                subTasksByQwests;
        }

        private static Task GetTask(Model.Task task)
        {
            var gTaskDate = SetTaskDateAndroFromNormal();
            Task nTask = new Task();

            // Новые свойства
            nTask.Image = task.PathToIm;
            var ab = task.TaskAbiliti();
            nTask.Ability = ab != null ? ab.GUID : "-1";

            nTask.BaseChangeAbility = 0;
            nTask.BaseChangeExp = 0;
            nTask.ChangeAbility = 0;
            nTask.BoosterOfDone = task.BoosterOfDone;
            nTask.BoosterOfFail = task.BoosterOfFail;

            if (ab != null)
            {
                var chAb = task.TaskAbylityChangeVal(ab);

                nTask.BaseChangeAbility = chAb;
                nTask.BaseChangeExp = task.Priority / Model.Task.AbIncreaseFormula(ab);
                nTask.ChangeAbility = chAb;
            }
            //----------------

            nTask.Exp = task.Priority;
            nTask.TimeLastDone = task.TimeLastDone.ToString("yyyy/MM/dd. HH:mm:ss:SSS");
            nTask.MiliSecsDoneForSort = task.MiliSecsDoneForSort;
            nTask.Wave = task.Wave;
            nTask.Priority = task.Priority;
            nTask.isSuper = task.isSuper;

            nTask.SubTasksByQwests = new List<string>();
            nTask.LinkedQwests = new List<string>();
            //nTask.TimeLastDone = MainViewModel.GetTimeStringMilisecondsFromeDate(new DateTime(task.MiliSecsDoneForSort));
            // todo сюда записать навык на который влияет, его значение и на сколько меняет этот навык, чтобы потом в андроид обработать

            if (MainViewModel.IsMorning().Invoke(task.TimeProperty))
            {
                nTask.TimeOfTheDay = 1;
            }
            else if (MainViewModel.IsDay().Invoke(task.TimeProperty))
            {
                nTask.TimeOfTheDay = 2;
            }
            else if (MainViewModel.IsEvening().Invoke(task.TimeProperty))
            {
                nTask.TimeOfTheDay = 3;
            }

            switch (task.Recurrense.TypeInterval)
            {
                case TimeIntervals.Нет:
                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = "NONE";
                    break;

                case TimeIntervals.Сразу:
                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = "NOW";
                    break;

                case TimeIntervals.Ежедневно:
                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = "Daily";
                    break;

                case TimeIntervals.Будни:
                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = "BusinessDay";
                    break;

                case TimeIntervals.Выходные:
                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = "Weekend";
                    break;

                case TimeIntervals.День:
                case TimeIntervals.ДниСначала:
                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = $"Every day&{task.Recurrense.Interval}";
                    break;

                case TimeIntervals.Неделя:
                case TimeIntervals.НеделиСНачала:
                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = $"Every Weeks&{task.Recurrense.Interval}";
                    break;

                case TimeIntervals.МесяцыСНачала:
                case TimeIntervals.Месяц:
                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = $"Every Month&{task.Recurrense.Interval}";
                    break;

                case TimeIntervals.ДниНедели:
                case TimeIntervals.ДниНеделиСНачала:
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

                    nTask.Date = gTaskDate(task);
                    nTask.Reccurrense = $"Every WeekDays&{weekDays}";
                    break;
            }

            nTask.UUID = task.GUID;

            string issuper = string.Empty;
            if (task.isSuper)
            {
                issuper = "👑 ";
            }

            var plusName1 = task.PlusNameOf2;
            string plusName2 = "";

            var count = (task.NameOfProperty + plusName1).Count();

            if (!string.IsNullOrWhiteSpace(plusName1) && task.Priority > 0 && plusName1.Contains("("))
            {
                var ind = plusName1.IndexOf('(');
                plusName2 = plusName1.Substring(ind).Trim();
                plusName1 = plusName1.Substring(0, ind).Trim();
            }

            if (string.IsNullOrWhiteSpace(plusName1))
            {
                nTask.NameOf = task.NameOfProperty.ToUpper();
            }
            else
            {
                string pl = string.Empty;
                if (!string.IsNullOrWhiteSpace(task.State) && !string.IsNullOrWhiteSpace(task.NameOfProperty))
                {
                    pl = " ";
                }

                nTask.NameOf = task.NameOfProperty.ToUpper() + pl + plusName1.ToUpper(); // + task.SubTasksString;
            }

            if (task.IsSkill)
            {
                nTask.TypeOf = "Навыки";
                //nTask.LinksOf = plusName2.Replace("(", "").Replace(")", "");
                var expName = plusName2.Replace("(", "").Replace(")", "");
                if (!string.IsNullOrWhiteSpace(expName))
                {
                    nTask.LinksOf = expName;
                    // nTask.NameOf += "\n" + expName;
                }
                //nTask.LinksOf = task.TaskInAbilitis.Aggregate("", (s, model) => s + model.NameOfProperty + "; ");
            }
            else if (task.RelToQwests.Any())
            {
                nTask.TypeOf = "Квесты";
            }
            else
            {
                nTask.TypeOf = "Задачи";
            }
            nTask.Time = task.TimeString;
            nTask.Index = task.TaskIndex;
            return nTask;
        }

        private static Func<Aim, bool> QwestsActiveForAndroid()
        {
            return n =>
                n.IsActiveProperty ||
                n.NotAllowReqwirements == "Хотя бы одна задача из связанных навыков должна быть активна; ";
        }

        /// <summary>
        ///     Дата из обычной задачи РПГ в задачу Андроид
        /// </summary>
        /// <returns></returns>
        private static Func<Model.Task, string> SetTaskDateAndroFromNormal()
        {
            Func<Model.Task, string> gTaskDate = task1 =>
            {
                return task1.BeginDateProperty == DateTime.MinValue
                    ? MainViewModel.selectedTime.ToString("dd.MM.yyyy")
                    : task1.BeginDateProperty.ToString("dd.MM.yyyy");
            };
            return gTaskDate;
        }

        public class AbilityAndroid
        {
            public AbilityAndroid(string name, double value)
            {
                Name = name;
                Value = value;
            }

            public AbilityAndroid()
            {
            }

            /// <summary>
            /// Количество опыта, которое дает 1 уровень навыка.
            /// </summary>
            public double ExpOneLevel { get; set; }

            public string Guid { get; set; }

            public double LastValue { get; set; }
            public double Modificator { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }

        public class Qwest
        {
            /// <summary>
            ///     Ид элемента
            /// </summary>
            /// \
            public string Guid { get; set; }

            /// <summary>
            ///     Название
            /// </summary>
            public string NameOfQwest { get; set; }

            /// <summary>
            ///     Задачи показываются одна за другой?
            /// </summary>
            public bool NotOneByOne { get; set; }

            /// <summary>
            ///     Задачи квеста
            /// </summary>
            public List<Task> Tasks { get; set; }
        }

        public class RootObject
        {
            /// <summary>
            /// Навыки
            /// </summary>
            public List<AbilityAndroid> Abils { get; set; }

            public List<CharacteristicAndroid> Characts { get; set; }

            /// <summary>
            ///     Дата последнего использования
            /// </summary>
            public string DateOfLastUse { get; set; }

            public double HP { get; set; }

            /// <summary>
            /// Правило паретто
            /// </summary>
            public bool IsParetto { get; set; }

            /// <summary>
            ///     Утро день вечер по очереди в андроиде
            /// </summary>
            public bool IsShowTasksByTimeAndroid { get; set; }

            /// <summary>
            ///     Умная фокусировка?
            /// </summary>
            public bool IsSmartSort { get; set; }

            /// <summary>
            /// Сортировка по приоритету
            /// </summary>
            public bool IsSortByPrioryty { get; set; }

            public int MaxGettedExp { get; set; }
            public double MaxHP { get; set; }
            public int PersExp { get; set; }

            public string PersName { get; set; }

            public string PersRang { get; set; }

            /// <summary>
            ///     Квесты
            /// </summary>
            public List<Qwest> Qwests { get; set; }

            /// <summary>
            ///     Задачи
            /// </summary>
            public List<Task> Tasks { get; set; }

            /// <summary>
            ///     Дата последнего использования
            /// </summary>
            public string TimeOfLastUse { get; set; }

            /// <summary>
            /// Только 10 уровней навыков.
            /// </summary>
            public bool Is10AbLevels { get; set; }

            /// <summary>
            /// 5-5-50.
            /// </summary>
            public bool Is5_5_50 { get; set; }

            /// <summary>
            /// FUDGE.
            /// </summary>
            public bool IsFUDGE { get; set; }

            public bool BalanceIs50Levels { get; set; }
            public int BalanceForFirstLevel { get; set; }
            public int BalanceForLastLevel { get; set; }
            public int BalanceLevels { get; set; }
        }

        public class Task
        {
            public string Ability { get; set; }

            public double BaseChangeAbility { get; set; }

            public double BaseChangeExp { get; set; }

            /// <summary>
            /// Множитель если задача выполнена.
            /// </summary>
            public double BoosterOfDone { get; set; }

            /// <summary>
            /// Множитель если задача провалена.
            /// </summary>
            public double BoosterOfFail { get; set; }

            public double ChangeAbility { get; set; }

            /// <summary>
            ///     Дата начала задачи
            /// </summary>
            public string Date { get; set; }

            /// <summary>
            ///     Время когда кликнули на сделано/не сделано
            /// </summary>
            public string DateOfClick { get; set; }

            /// <summary>
            ///     Дата когда она была выполнена
            /// </summary>
            public string DateOfDone { get; set; }

            public double Exp { get; set; }

            /// <summary>
            ///     ИД
            /// </summary>
            public int ID { get; set; }

            public string Image { get; set; }

            /// <summary>
            ///     Индекс задачи
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            ///     Выполнена
            /// </summary>
            public bool IsDone { get; set; }

            /// <summary>
            ///     Был нажат минус?
            /// </summary>
            public bool IsMinus { get; set; }

            public bool isSuper { get; set; }

            /// <summary>
            ///     Ссылки на квесты. Если все задачи этих квестов неактивны, то и эта задача тоже неактивна.
            /// </summary>
            public List<string> LinkedQwests { get; set; }

            /// <summary>
            ///     На че влияет задача
            /// </summary>
            public string LinksOf { get; set; }

            /// <summary>
            /// Милисекунды для сортировки
            /// </summary>
            public double MiliSecsDoneForSort { get; set; }

            /// <summary>
            ///     Название задачи
            /// </summary>
            public string NameOf { get; set; }

            /// <summary>
            ///     Доп. заметки
            /// </summary>
            public string Note { get; set; }

            public double Priority { get; set; }

            /// <summary>
            ///     Повторение
            /// </summary>
            public string Reccurrense { get; set; }

            /// <summary>
            ///     Подзадачи, как правило у навыков от квестов
            /// </summary>
            public List<string> SubTasksByQwests { get; set; }

            /// <summary>
            ///     Время
            /// </summary>
            public string Time { get; set; }

            /// <summary>
            ///     Время когда последний раз кликали по задачи (для умной сортировки)
            /// </summary>
            public string TimeLastDone { get; set; }

            /// <summary>
            ///     Время дня. 0-все, 1-утро, 2-день, 3-вечер.
            /// </summary>
            public int TimeOfTheDay { get; set; }

            /// <summary>
            ///     Тип задачи - скиллы, квесты, задачи - в каком списке?
            /// </summary>
            public string TypeOf { get; set; }

            /// <summary>
            ///     Уникальный номер
            /// </summary>
            public string UUID { get; set; }

            /// <summary>
            /// Волна для сортировки по приоритету
            /// </summary>
            public int Wave { get; set; }
        }
    }
}
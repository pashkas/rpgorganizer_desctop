using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Sample.Properties;
using Sample.View;
using Sample.ViewModel;

namespace Sample.Model
{
    /// <summary>
    /// Класс со статическими методами, к которым надо обращаться из всех форм
    /// </summary>
    public static class StaticMetods
    {
        public static void ShowGameOver()
        {
            GameOverWindow go = new GameOverWindow();
            go.btnExit.Click += (a, b) => {
                go.Close();
                StaticMetods.Locator.MainVM.Exit();
            };
            go.ShowDialog();
        }

        /// <summary>
        /// Максимальное количество очков жизней.
        /// </summary>
        public static int MaxHitPoints = 10;

        public static ObservableCollection<LinkThisTask> FocsString { get; set; } =
            new ObservableCollection<LinkThisTask>();

        /// <summary>
        /// Получить локатор
        /// </summary>
        /// <returns></returns>
        public static ViewModelLocator Locator
        {
            get { return (ViewModelLocator)Application.Current.FindResource("Locator"); }
        }

        /// <summary>
        /// Персонаж
        /// </summary>
        public static Pers PersProperty { get; set; }

        /// <summary>
        /// Максимальный уровень для навыков
        /// </summary>
        public static int MaxAbLevel => PersProperty.PersSettings.MaxLevOfAbForProg;

        /// <summary>
        /// Максимальный уровень для характеристик
        /// </summary>
        public static int MaxChaLevel => PersProperty.PersSettings.MaxLevOfChaForProg;

        /// <summary>
        /// Максимальный ранг персонажа и мостров
        /// </summary>
        public static int MaxPersAndMonstersRangs => 6;

        public static int PlusExpForSomeTasks => 5;

        /// <summary>
        /// Пересчет всех скиллов
        /// </summary>
        /// <param name="persProperty">Персонаж</param>
        public static void AbillitisRefresh(Pers persProperty)
        {
            // Пересчитываем требования для скиллов
            var persAbilitis = persProperty.Abilitis;

            // Проверяем все условия для активности скиллов
            foreach (var abilitiModel in persAbilitis)
            {
                abilitiModel.RefreshEnabled();
            }
        }

        /// <summary>
        /// Добавление дочернего квеста
        /// </summary>
        /// <param name="_pers">персонаж</param>
        /// <param name="parrentAim">родительский квест</param>
        /// <returns>возвращает добавленный дочерний квест</returns>
        public static Aim addChildAim(Pers _pers, Aim parrentAim, Aim child = null)
        {
            Aim childAim;
            if (child != null)
            {
                childAim = child;
            }
            else
            {
                childAim = new Aim(_pers);
            }

            parrentAim.Needs.Add(childAim);

            Locator.AimsVM.SelectedAimProperty = childAim;
            Messenger.Default.Send("Квест добавлен");

            childAim.MinLevelProperty = parrentAim.MinLevelProperty > _pers.PersLevelProperty
                ? parrentAim.MinLevelProperty
                : _pers.PersLevelProperty;
            childAim.ImageProperty = parrentAim.ImageProperty;

            CopyAimSkills(parrentAim, childAim);

            if (child == null)
            {
                editAim(childAim, true);
            }

            return childAim;
        }

        /// <summary>
        /// Добавляем составной квест для квеста
        /// </summary>
        /// <param name="pers">Персонаж</param>
        /// <param name="selectedAimProperty">Составной квест</param>
        public static Aim AddCompositeQwest(Pers pers, Aim selectedAimProperty, Task fromTask = null)
        {
            var childAim = new Aim(pers);

            if (fromTask != null)
            {
                childAim.NameOfProperty = $"{fromTask.NameOfProperty}";
            }

            selectedAimProperty.CompositeAims.Add(
                new CompositeAims { AimProperty = childAim, KoeficientProperty = 10 });

            childAim.ImageProperty = selectedAimProperty.ImageProperty;
            childAim.MinLevelProperty = selectedAimProperty.MinLevelProperty > pers.PersLevelProperty
                ? selectedAimProperty.MinLevelProperty
                : pers.PersLevelProperty;

            CopyAimSkills(selectedAimProperty, childAim);

            var editQwest = new EditQwestWindowView();
            CollapsePrevNextQwests(editQwest);
            Locator.QwestsVM.SelectedAimProperty = childAim;
            editQwest.btnOk.Click += (sender, args) =>
            {
                if (fromTask != null)
                {
                    foreach (
                        var needTasks in selectedAimProperty.NeedsTasks.Where(n => n.TaskProperty == fromTask).ToList())
                    {
                        selectedAimProperty.NeedsTasks.Remove(needTasks);
                    }
                }
                editQwest.Close();
            };

            editQwest.btnCansel.Click += (sender, args) =>
            {
                RemoveQwest(PersProperty, childAim);
                editQwest.Close();
                childAim = null;
            };

            editQwest.ShowDialog();
            Locator.QwestsVM.SelectedAimProperty = selectedAimProperty;
            return childAim;
        }

        /// <summary>
        /// Добавить новый квест (в отдельном окне)
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        /// <param name="minLevel"></param>
        /// <param name="NeedTasks"></param>
        /// <param name="_abil"></param>
        /// <param name="needLevel"></param>
        /// <param name="skills"></param>
        /// <param name="uU"></param>
        /// <returns></returns>
        public static Aim AddNewAim(Pers _pers, int minLevel = -1, ObservableCollection<NeedTasks> NeedTasks = null, AbilitiModel _abil = null, int needLevel = 0, List<Task> skills = null, UpUbility uU = null)
        {
            var newAim = new Aim(_pers);

            if (minLevel != -1)
            {
                newAim.MinLevelProperty = minLevel;
            }

            var editQwest = new EditQwestWindowView();

            var context = Locator.AimsVM;
            context.SelectedAimProperty = newAim;

            if (NeedTasks != null)
            {
                foreach (var needTaskse in NeedTasks)
                {
                    newAim.NeedsTasks.Add(needTaskse);
                }
            }

            FocusManager.SetFocusedElement(editQwest, editQwest.QwestsView.txtName);

            editQwest.btnOk.Click += (sender, args) =>
            {
                editQwest.Close();
                WriteAutoBard(AutoBardOperations.КвестСоздан, newAim);
            };

            editQwest.btnCansel.Click += (sender, args) =>
            {
                RemoveQwest(_pers, newAim);
                newAim = null;
                editQwest.Close();
            };

            if (_abil != null)
            {
                var hardName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_abil.SelH);
                var abName = CultureInfo.CurrentCulture.TextInfo.ToLower(_abil.NameOfProperty);
                newAim.NameOfProperty = $"{hardName} \"{abName}\"";
                newAim.ImageProperty = _abil.ImageProperty;
            }

            if (skills != null)
            {
                foreach (var skill in skills)
                {
                    newAim.Spells.Add(skill);
                }
            }

            newAim.CountAutoProgress();

            Locator.QwestsVM.RefreshInfoCommand.Execute(null);

            newAim.UcSetGoldExpRevardViewModel.SetRewCommand.Execute("простоGold");

            if (uU != null)
            {
                newAim.UpUbilitys.Add(uU);
            }

            editQwest.ShowDialog();

            RefreshAllQwests(PersProperty, true, true, true);

            RefreshSelAimInAimsList(newAim);
            return newAim;
        }

        /// <summary>
        /// Добавить родительский квест к квесту (следующий)
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        /// <param name="_childAim">дочерний квест, к которому нужно добавить</param>
        /// <param name="isNext">Следующий?</param>
        /// <returns>Новый родительский квест</returns>
        public static Aim addParrentQwest(Pers _pers, Aim _childAim, bool isNext = false)
        {
            var _parrentAim = new Aim(_pers);

            // Сохранение связей
            var reqvire = _pers.Aims.Where(n => n.Needs.Any(q => q == _childAim)).ToList();
            var combine = _pers.Aims.Where(n => n.CompositeAims.Any(q => q.AimProperty == _childAim)).ToList();

            if (isNext)
            {
                foreach (var aim in reqvire)
                {
                    aim.Needs.Add(_parrentAim);
                }

                // Удаление "следующих" из childAim
                foreach (var aim in reqvire)
                {
                    aim.Needs.Remove(_childAim);
                }
            }

            foreach (var aim in combine)
            {
                aim.CompositeAims.Add(new CompositeAims { AimProperty = _parrentAim, KoeficientProperty = 10 });
            }

            _parrentAim.Needs.Add(_childAim);

            foreach (var spell in _parrentAim.Spells.Where(n => _childAim.Spells.All(q => q != n)).ToList())
            {
                _childAim.Spells.Add(spell);
            }

            Locator.AimsVM.SelectedAimProperty = _parrentAim;
            Messenger.Default.Send("Квест добавлен");
            _parrentAim.MinLevelProperty = _childAim.MinLevelProperty > _pers.PersLevelProperty
                ? _childAim.MinLevelProperty
                : _pers.PersLevelProperty;
            _parrentAim.ImageProperty = _childAim.ImageProperty;
            CopyAimSkills(_childAim, _parrentAim);

            editAim(_parrentAim, true);

            return _parrentAim;
        }

        public static void CenterWindow(Window window)
        {
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = (SystemParameters.WorkArea.Width - window.ActualWidth) / 2 + SystemParameters.WorkArea.Left;
            window.Top = (SystemParameters.WorkArea.Height - window.ActualHeight) / 2 + SystemParameters.WorkArea.Top;
        }

        /// <summary>
        /// Копируем скиллы цели
        /// </summary>
        /// <param name="firstAim"></param>
        /// <param name="secondAim"></param>
        public static void CopyAimSkills(Aim firstAim, Aim secondAim)
        {
            var sk = secondAim.AbilitiLinksOf.Union(firstAim.AbilitiLinksOf);
            secondAim.AbilitiLinksOf.Clear();
            foreach (var abilitiModel in sk)
            {
                secondAim.AbilitiLinksOf.Add(abilitiModel);
            }

            // Ссылки на задачи
            var sl = secondAim.LinksOfTasks.Union(firstAim.LinksOfTasks);
            secondAim.LinksOfTasks.Clear();
            foreach (var task in sl)
            {
                secondAim.LinksOfTasks.Add(task);
            }
        }

        /// <summary>
        /// Удалить скилл
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        /// <param name="_ability">Скилл</param>
        public static void DeleteAbility(Pers _pers, AbilitiModel _ability)
        {
            // Есть ли связанные задачи?
            var linkedTasks = getLinkedToAbilityTasks(_ability);

            var linkTaskName = getLinkedToAbilitiTasksNames(linkedTasks);

            showRemoveLinkedTasksMessege(_pers, linkedTasks, linkTaskName);

            // Удаляем связанные квесты
            removeLinkedWithAbQwests(_pers, _ability, linkTaskName);

            // Удаляем из характеристик
            removeAbFromCharacteristics(_pers, _ability);

            // Удаляем из требований навыков
            foreach (var abilitiModel in _pers.Abilitis)
            {
                foreach (var needsForLevel in abilitiModel.NeedsForLevels)
                {
                    foreach (var source in needsForLevel.NeedAbilities.Where(n => n.AbilProperty == _ability).ToList())
                    {
                        needsForLevel.NeedAbilities.Remove(source);
                    }
                }

                foreach (var na in abilitiModel.NeedAbilities.Where(n => n.AbilProperty == _ability).ToList())
                {
                    abilitiModel.NeedAbilities.Remove(na);
                }
            }

            // Удаляем из требований квестов
            foreach (var qw in _pers.Aims)
            {
                foreach (var na in qw.NeedAbilities.Where(n => n.AbilProperty == _ability).ToList())
                {
                    qw.NeedAbilities.Remove(na);
                }

                // Удаляем из прокачки квестов
                foreach (var na in qw.UpUbilitys.Where(n => n.Ability == _ability).ToList())
                {
                    qw.UpUbilitys.Remove(na);
                }
            }

            // Удаляем из условий скиллов
            foreach (var abilitiModel in PersProperty.Abilitis)
            {
                foreach (var reqwirement in abilitiModel.NeedAbilities.Where(n => n.AbilProperty == _ability).ToList())
                {
                    abilitiModel.NeedAbilities.Remove(reqwirement);
                }
            }

            // Удаляем из магазина и инвентаря
            removeAbFromInventoryAndShop(_pers, _ability);

            // Удаляем из ссылок квестов
            foreach (var aim in PersProperty.Aims)
            {
                foreach (var source in aim.AbilitiLinksOf.Where(n => n == _ability).ToList())
                {
                    aim.AbilitiLinksOf.Remove(source);
                }
            }

            // Обнуляем опыт
            AddOrEditAbilityViewModel.AbNullExperiance(_ability);

            var ruleCha = _ability.RuleCharacterisic;

            // Удаляем скилл
            _pers.Abilitis.Remove(_ability);

            // Обновляем основные элементы игры
            RecountPersExp();
            RecauntAllValues();
            RecountTaskLevels();
            PersProperty.RefreshMaxPersLevel();
            PersProperty.SellectedAbilityProperty = PersProperty.Abilitis.FirstOrDefault();
            PersProperty.RecountPlusAbPoints();

            ruleCha?.RecountChaValue();
            PersProperty.NewRecountExp();
        }

        /// <summary>
        /// Открыть окно с редактированием квеста
        /// </summary>
        /// <param name="aimProperty">Квест для редактирования</param>
        /// <param name="isShowCansel">Показывать кнопку отмены?</param>
        public static void editAim(Aim aimProperty, bool isShowCansel = false)
        {
            var editQwest = new EditQwestWindowView();
            Locator.AimsVM.SelectedAimProperty = aimProperty;
            Locator.QwestsVM.NeedsRefresh();
            Locator.QwestsVM.RefreshInfoCommand.Execute(null);

            Action okComa = () =>
            {
                Locator.AimsVM.SelectedAimProperty?.OnPropertyChanged(nameof(Aim.ActiveMissions));
                RefreshAllQwests(PersProperty, true, true, true);
                Locator.MainVM.RefreshTasksInMainView();
                editQwest.Close();
                RefreshSelAimInAimsList(aimProperty);
            };

            editQwest.btnOk.Click += (sender, args) => { okComa.Invoke(); };

            var HotSaveCommand = new RelayCommand(() => { okComa.Invoke(); });
            editQwest.InputBindings.Add(new KeyBinding(HotSaveCommand,
                new KeyGesture(Key.S,
                    ModifierKeys.Control)));

            if (isShowCansel == false)
            {
                editQwest.btnCansel.Visibility = Visibility.Collapsed;
            }
            else
            {
                editQwest.btnCansel.Visibility = Visibility.Visible;
                CollapsePrevNextQwests(editQwest);
                editQwest.btnCansel.Click += (sender, args) =>
                {
                    RemoveQwest(PersProperty, aimProperty);
                    editQwest.Close();
                };
            }

            editQwest.ShowDialog();
        }

        /// <summary>
        /// Проверка на доступность по требованиям скиллов
        /// </summary>
        /// <param name="needOfAbilities">Требования скиллов</param>
        /// <param name="reqwirements">Строка с требованиями</param>
        /// <param name="isEnabled">Булевая переменная - доступно?</param>
        /// <returns></returns>
        public static void GetAbillsReq(ObservableCollection<NeedAbility> needOfAbilities, ref string reqwirements,
            ref bool isEnabled)
        {
            var inAbNeed = (from ab in needOfAbilities
                            where ab.AbilProperty.CellValue < ab.ValueProperty || !ab.AbilProperty.IsEnebledProperty
                            select new { ab.AbilProperty, ab.ValueProperty }).ToList();
            if (inAbNeed.Any())
            {
                foreach (var vvv in inAbNeed)
                {
                    reqwirements +=
                        $"{vvv.AbilProperty.NameOfProperty} >= {PersProperty.PersSettings.AbRangs[Convert.ToInt32(vvv.ValueProperty)].Name}; ";
                    isEnabled = false;
                }
            }
        }

        /// <summary>
        /// Автоматически расчитанное количество очков скиллов для того, чтобы подогнать под 50 уровней
        /// </summary>
        /// <returns></returns>
        public static double GetAutoRecauntAbPointsPerLev()
        {
            if (PersProperty == null) return 1;
            if (PersProperty.PersSettings.AutoCountAbPointsForLev == false)
                return PersProperty.PersSettings.AbPointsForOneLevel;
            double maxAbCost = Pers.AbCostByLev(PersProperty.PersSettings.MaxLevOfAbForProg);
            double allNeedPoints = PersProperty.Abilitis.Count * maxAbCost;
            double ap = allNeedPoints / 50.0;
            //if ((int) Math.Floor(ap) > PersProperty.PersSettings.MaxLevOfAbForProg)
            //{
            //    ap = PersProperty.PersSettings.MaxLevOfAbForProg; //allNeedPoints/100.0;
            //    PersProperty.PersSettings.Is100Levels = true;
            //}
            //else
            //{
            //    PersProperty.PersSettings.Is100Levels = false;
            //}
            var autoRecauntAbPointsPerLev = ap;//(int) Math.Floor(ap);
            if (autoRecauntAbPointsPerLev < 1) autoRecauntAbPointsPerLev = 1;
            return autoRecauntAbPointsPerLev;
        }

        /// <summary>
        /// Получить цвет рамки для элемента на который влияет данный элемент
        /// </summary>
        /// <param name="kExpRelayProperty">Коэффициент влияния</param>
        /// <returns></returns>
        public static Brush getBorderColorFromRelaysWithHardness(int kExpRelayProperty)
        {
            switch (kExpRelayProperty)
            {
                case 0:
                    return Brushes.Gray;

                case 1:
                    return Brushes.Goldenrod;

                case 2:
                    return Brushes.Goldenrod;

                case 3:
                    return Brushes.Goldenrod;

                case 4:
                    return Brushes.DarkGoldenrod;

                case 5:
                    return Brushes.DarkGoldenrod;

                case 6:
                    return Brushes.DarkGoldenrod;

                case 7:
                    return Brushes.DarkGoldenrod;

                case 8:
                    return Brushes.DarkGoldenrod;

                case 9:
                    return Brushes.Green;

                case 10:
                    return Brushes.Green;

                default:
                    return Brushes.Gray;
            }
        }

        /// <summary>
        /// Проверка доступности элемента по требованиям характеристик
        /// </summary>
        /// <param name="needOfCharacts">Требования характеристик</param>
        /// <param name="reqwirements">Строка с требованиями</param>
        /// <param name="isEnabled">Булевая переменная - доступно?</param>
        /// <returns></returns>
        public static void GetCharactReq(ObservableCollection<NeedCharact> needOfCharacts, ref string reqwirements,
            ref bool isEnabled)
        {
            var inChaNeed = (from ab in needOfCharacts
                             where ab.CharactProperty.CellValue < ab.ValueProperty
                             select new { ab.CharactProperty, ab.ValueProperty }).ToList();
            if (inChaNeed.Any())
            {
                foreach (var vvv in inChaNeed)
                {
                    reqwirements +=
                        $"{vvv.CharactProperty.NameOfProperty} >= {PersProperty.PersSettings.CharacteristicRangs[Convert.ToInt32(vvv.ValueProperty)].Name}; ";
                    isEnabled = false;
                }
            }
        }

        /// <summary>
        /// Вернуть строку, состоящую только из букв
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetClearString(string str)
        {
            return new string(str.Where(char.IsLetter).ToArray());
        }

        /// <summary>
        /// Получить дату из строки
        /// </summary>
        /// <param name="beginString">Дата в формате строки</param>
        /// <returns></returns>
        public static DateTime GetDateFromString(string beginString)
        {
            var minDateValue = DateTime.MinValue;
            var dateOfBegin = string.IsNullOrEmpty(beginString) ? minDateValue : DateTime.Parse(beginString);
            return dateOfBegin;
        }

        /// <summary/>
        ///        /// /// /// Расчет опыта
        /// <param name="pers">Персонаж</param>
        /// Игнорировать настройку - опыт только по завершению квестов?
        /// <returns>Опыт персонажа</returns>
        public static int GetExp(Pers pers)
        {
            pers.NewRecountExp();
            return pers.PersExpProperty;
            //List<AbilitiModel> mainAbilities;
            //List<AbilitiModel> secondAbilitis;
            //List<AbilitiModel> lastAbilities;
            //GetMainSecondLastAbs(out mainAbilities, pers, out secondAbilitis, out lastAbilities);

            //// Чтоб как в ТЕС
            //var expToReturn = mainAbilities.Sum(n => Pers.ExpK * n.Experiance) +
            //                  secondAbilitis.Sum(n => Pers.ExpK * n.Experiance) +
            //                  lastAbilities.Sum(n => Pers.ExpK * n.Experiance);
            //expToReturn = expToReturn - pers.ExpBuff + pers.PersExpFromeTasksAndQwests;
            //var round = (int)Math.Round(expToReturn);
            //if (round == 666)
            //{
            //    round = 667;
            //}

            //if (round < 0)
            //{
            //    foreach (var abilitiModel in PersProperty.Abilitis)
            //    {
            //        abilitiModel._experiance = 0;
            //    }

            //    return 0;
            //}

            //return round;
        }

        /// <summary>
        /// Получаем значения влияний на опыт для квестов, характеристик и скиллов
        /// </summary>
        /// <param name="maxExp">Значения влияний на опыт</param>
        /// <returns>The <see cref="List{T}"/>.</returns>
        public static List<int> GetExpChanges(double maxExp)
        {
            var changes = new List<int>();

            var oneRelay = maxExp / 15.0;
            changes.Add(0);
            changes.Add(Convert.ToInt32(oneRelay * 1.0));
            changes.Add(Convert.ToInt32(oneRelay * 3.0));
            changes.Add(Convert.ToInt32(oneRelay * 6.0));
            changes.Add(Convert.ToInt32(oneRelay * 10.0));
            changes.Add(Convert.ToInt32(maxExp));

            if (changes[1] == 0)
            {
                changes[1] = 1;
            }

            if (changes[2] <= changes[1])
            {
                changes[2] = changes[1] + 1;
            }

            if (changes[3] <= changes[2])
            {
                changes[3] = changes[2] + 1;
            }

            if (changes[4] <= changes[3])
            {
                changes[4] = changes[3] + 1;
            }

            if (changes[5] <= changes[4])
            {
                changes[5] = changes[4] + 1;
            }

            return changes;
        }

        public static int getExpFromHardness(int hardnessProperty)
        {
            var modifier = GetMod(hardnessProperty) * Config.QwestExp;

            var addGoldFromHardness = modifier;

            return Convert.ToInt32(addGoldFromHardness);
        }

        /// <summary>
        /// Прибавка к золоту за сложность задачи
        /// </summary>
        /// <param name="hard">Сложность</param>
        /// <param name="isQwest">Квест?</param>
        /// <param name="selectedAimProperty"></param>
        /// <returns></returns>
        public static int getGoldFromHardness(int hard, bool isQwest, Aim selectedAimProperty)
        {
            if (selectedAimProperty != null)
            {
                var inAnyComposite = from aim in PersProperty.Aims
                                     where aim.CompositeAims.Any(n => n.AimProperty == selectedAimProperty)
                                     select aim;
                if (inAnyComposite.Any())
                {
                    return 0;
                }
            }

            var modifier = 100;

            var addGoldFromHardness = GetMod(hard) * modifier;

            return addGoldFromHardness;
        }

        public static BitmapImage getImagePropertyFromImage(byte[] imag)
        {
            var im = new BitmapImage();
            if (imag == null)
            {
                return im;
            }

            //var thumb = CreateThumbnailBytes(imag, 400);

            try
            {
                using (var mem = new MemoryStream(imag))
                {
                    mem.Position = 0;
                    im.BeginInit();
                    im.CacheOption = BitmapCacheOption.OnLoad;
                    im.CreateOptions = BitmapCreateOptions.None;
                    // im.DecodePixelHeight = 480;
                    im.UriSource = null;
                    im.StreamSource = mem;

                    im.EndInit();
                }

                im.Freeze();
                return im;
            }
            catch
            {
                return new BitmapImage();
            }
        }

        /// <summary>
        /// Получаем коэффициент влияния на опыт характеристик и скиллов
        /// </summary>
        /// <param name="expToBalance">Опыт, который надо достичь</param>
        /// <param name="maxExpAbCha">
        /// Максимальное влияние скиллов и характеристик на опыт без коэффициентов
        /// </param>
        /// <returns>Коэффициент</returns>
        public static double getK(double expToBalance, double maxExpAbCha)
        {
            double k;
            if (expToBalance <= 0)
            {
                k = 1;
            }
            else
            {
                if (Math.Abs(maxExpAbCha) > 0.01)
                {
                    k = expToBalance / maxExpAbCha;
                }
                else
                {
                    k = 1;
                }
            }
            return k;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Метод, который расчитывает уровень персонажа
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="typeRpgItem">Тип РПГ Элемента</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int GetLevel(double exp, RpgItemsTypes typeRpgItem)
        {
            return (int)Math.Floor(exp / Pers.ExpOneLev)+1;
            ////var sqrt = Math.Sqrt((exp / 12.50) + 1.0);
            ////var d = (1 + sqrt) / 2.0;
            ////int lev = (int)Math.Floor(d);
            ////return lev;
            //return PersProperty.PersLevelProperty;
            //if (typeRpgItem != RpgItemsTypes.exp)
            //{
            //    return 0;
            //}

            //int i = 0;
            //while (exp >= Pers.ExpToLevel(i, RpgItemsTypes.exp))
            //{
            //    i++;
            //}

            //return i - 1;

            //var sqrt = Math.Sqrt((exp / 125.0) + 1.0);
            //int level = (int)Math.Floor((1 + sqrt) / 2.0);

            //return level;
        }

        /// <summary>
        /// Рамка для требования - минимальный уровень персонажа
        /// </summary>
        /// <param name="persLevelProperty">Текущий уровень персонажа</param>
        /// <param name="minLevelProperty">Минимальный уровень для элемента</param>
        /// <returns></returns>
        public static Brush GetLevelReqBorderColor(int persLevelProperty, int minLevelProperty)
        {
            if (persLevelProperty < minLevelProperty)
            {
                return Brushes.Red;
            }
            return Brushes.Green;
        }

        public static void GetLikeTESProgress(Pers pers, out double curProgress, out double maxProgress)
        {
            List<AbilitiModel> mainAbilities;
            List<AbilitiModel> secondAbilitis;
            List<AbilitiModel> lastAbilities;
            GetMainSecondLastAbs(out mainAbilities, pers, out secondAbilitis, out lastAbilities);
            mainAbilities.ForEach(n => n.TESPriority = 5);
            secondAbilitis.ForEach(n => n.TESPriority = 2);
            lastAbilities.ForEach(n => n.TESPriority = 1);
            double kMax = 3;
            double KMid = 2;
            double kMin = 1;

            maxProgress = mainAbilities.Sum(n => kMax * 100.0) + secondAbilitis.Sum(n => KMid * 100.0) +
                          lastAbilities.Sum(n => kMin * 100.0);
            maxProgress = maxProgress > 0 ? maxProgress : 1;
            curProgress = mainAbilities.Sum(n => kMax * n.ValueProperty) + secondAbilitis.Sum(n => KMid * n.ValueProperty) +
                          lastAbilities.Sum(n => kMin * n.ValueProperty);
        }

        /// <summary>
        /// Метод для определения скиллов, влияющих на конкретную характеристику
        /// </summary>
        /// <param name="chaParam">Характеристика</param>
        /// <returns>Связанные скиллы</returns>
        public static IOrderedEnumerable<AbilitiModel> GetLinkedAbilitis(Characteristic chaParam)
        {
            return
                chaParam.NeedAbilitisProperty.Where(n => n.KoeficientProperty != 0)
                    .Select(q => q.AbilProperty)
                    .OrderBy(q => q.NameOfProperty);
        }

        /// <summary>
        /// Получить основные, второстепенные,
        /// </summary>
        /// <param name="mainAbilities"></param>
        /// <param name="pers"></param>
        /// <param name="secondAbilitis"></param>
        /// <param name="lastAbilities"></param>
        public static void GetMainSecondLastAbs(out List<AbilitiModel> mainAbilities, Pers pers,
            out List<AbilitiModel> secondAbilitis,
            out List<AbilitiModel> lastAbilities)
        {
            mainAbilities = pers.Abilitis.ToList();
            secondAbilitis = new List<AbilitiModel>();
            lastAbilities = new List<AbilitiModel>();
            mainAbilities.ForEach(n => n.TESPriority = 5);

            //-----------------------------
            //var orderedAbills =
            //    pers.Abilitis.OrderByDescending(n => n.Hardness)
            //        .Select(n => new {n, n.ToChaRelaysProperty })
            //        .ToList();

            //mainAbilities = orderedAbills.Where(n => n.ToChaRelaysProperty >= MaxRelAbToCha).Select(n => n.n).ToList();
            //secondAbilitis = orderedAbills.Where(n => n.ToChaRelaysProperty < MaxRelAbToCha && n.ToChaRelaysProperty >= MidRelAbToCha)
            //        .Select(n => n.n)
            //        .ToList();
            //lastAbilities = orderedAbills.Where(n => n.ToChaRelaysProperty < MidRelAbToCha && n.ToChaRelaysProperty>0).Select(n => n.n).ToList();

            //// Задаем стоимости для покупки скиллов
            //lastAbilities.ForEach(n => n.AbCost = 1);
            //secondAbilitis.ForEach(n => n.AbCost = 1);
            //mainAbilities.ForEach(n => n.AbCost = 1);

            //mainAbilities.ForEach(n => n.TESPriority = 5);
            //secondAbilitis.ForEach(n => n.TESPriority = 2);
            //lastAbilities.ForEach(n => n.TESPriority = 1);
            //orderedAbills.Where(n=>n.ToChaRelaysProperty<=0).ToList().ForEach(q=>q.n.TESPriority = 0);
        }

        /// <summary>
        /// Получить максимальное здоровье персонажа
        /// </summary>
        /// <param name="pers">Персонаж</param>
        /// <returns>Макс здоровье перса</returns>
        public static int GetMaxHP(Pers pers)
        {
            //var levelPers = GetLevel(pers.PersExpProperty, RpgItemsTypes.exp);
            //var maxHp = BaseHp + levelPers * Config.PlusHPOneLevel;
            //pers.HPProperty.MaxHPProperty = maxHp;
            return pers.MaxHitPoints;
        }

        /// <summary>
        /// Модификатор влияния квеста и его сложности на золото и опыт от квеста
        /// </summary>
        /// <param name="hardness"></param>
        /// <returns></returns>
        public static int GetMod(int hardness)
        {
            switch (hardness)
            {
                case 0:
                    return 0;

                case 1:
                    return 1;

                case 2:
                    return 3;

                case 3:
                    return 6;

                case 4:
                    return 10;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Метод для получения новой задачи
        /// </summary>
        /// <param name="persProperty">Персонаж</param>
        /// <param name="taskTypeProperty">Тип задачи, который будет у новой задачи</param>
        /// <returns>Новая задача</returns>
        public static Task GetNewTask(Pers persProperty, TypeOfTask taskTypeProperty)
        {
            // MainViewModel.SaveLogTxt("ucTaskSetting - получаем изменения характеристик");

            var task = new Task
            {
                TaskType = taskTypeProperty,
                BeginDateProperty = MainViewModel.selectedTime,
                Cvet = persProperty.PersSettings.ColorTaskBorderProperty,
                HardnessProperty = 0,
                Recurrense =
                    new TypeOfRecurrense
                    {
                        TypeInterval = taskTypeProperty.IntervalForDefoult,
                        Interval = 1
                    },
                TaskStatus = taskTypeProperty.StatusForDefoult,
                TaskContext = taskTypeProperty.ContextForDefoult,
                TaskRangs = new ObservableCollection<Rangs>(),
                DaysOfWeekRepeats = MainViewModel.GetDaysOfWeekCollection(),
                SecondOfDone = Task.GetSecOfDone()
            };

            Task.SetEndDate(task);
            task.HardnessProperty = 1;

            task.Damage = task.Recurrense.TypeInterval == TimeIntervals.Нет
                ? persProperty.PersSettings.DamageFromeTask
                : persProperty.PersSettings.DamageFromHabbit;

            Task.RecountTaskLevel(task);
            task.GetEnamyImage();

            return task;
        }

        /// <summary>
        /// Метод чтобы назначить путь к картинке
        /// </summary>
        /// <returns>Путь к картинке</returns>
        public static byte[] GetPathToImage(byte[] im)
        {
            var of = new OpenFileDialog { Filter = "Изображения|*.jpg;*.png;*.jpeg" };
            of.ShowDialog();
            var path = of.FileName;
            byte[] data = null;

            if (string.IsNullOrEmpty(path) == false)
            {
                var bmp = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));

                var fi = new FileInfo(path);
                if (fi.Extension == ".png")
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }
                }
                else
                {
                    var encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }
                }
            }

            return data ?? im;
        }

        public static string GetPathToMegaPers()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Templates", "MegaPers");
        }

        /// <summary>
        /// Получить стоимость уровня, когда каждый следующий уровень дороже предыдущего на N%
        /// </summary>
        /// <param name="xp_for_last_level">Стоимость последнего уровня</param>
        /// <param name="xp_for_first_level">Стоимость первого уровня</param>
        /// <param name="levels">Всего уровней</param>
        /// <param name="level">Текущий уровень</param>
        /// <returns></returns>
        public static double GetPowerCost(double xp_for_last_level, double xp_for_first_level, double levels,
            double level)
        {
            if (level <= 0)
            {
                return 0;
            }

            var B = Math.Log(xp_for_last_level / xp_for_first_level) / (levels - 1);
            var A = xp_for_first_level / (Math.Exp(B) - 1.0);
            var old_xp = A * Math.Exp(B * (level - 1));
            var new_xp = A * Math.Exp(B * level);
            var ff = new_xp - old_xp;
            //ff = Math.Round(ff / 10.0) * 10.0;

            return ff;
        }

        /// <summary>
        /// Узнать предыдущие задачи для задачи
        /// </summary>
        /// <param name="task">Задача</param>
        /// <returns></returns>
        public static IEnumerable<Task> GetPrevActionsForTask(Task task)
        {
            var prevActionsForTask = from task1 in PersProperty.Tasks
                                     where MainViewModel.isThisTaskDone(task1) == false && task1.NextActions.Any(n => n == task)
                                     select task1;

            return prevActionsForTask;
        }

        /// <summary>
        /// Случайный текст для выполненной задачи
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetRandomeTasksDoneMessege(object obj)
        {
            var tsk = obj as Task;
            if (tsk != null)
            {
                var lst = new List<string>();
                var prsName = PersProperty.NameOfProperty;
                var tskName = $"{tsk.NameOfProperty}";
                lst.Add($"*(+) Великолепный {prsName} побеждает \"{tskName}\"*!");
                lst.Add($"*(+) \"{tskName}\" сражен наповал!*");
                lst.Add($"*(+) \"{tskName}\" побежден!*");
                lst.Add($"*(+) Отважный {prsName} справился с \"{tskName}\"*!");
                lst.Add($"*(+) Храбрый {prsName} наносит критические повреждения \"{tskName}\"*!");
                lst.Add($"*(+) \"{tskName}\" молит о пощаде!*");
                lst.Add($"*(+) \"{tskName}\" покинул поле боя!*");
                lst.Add($"*(+) {prsName} выигрывает у \"{tskName}\"!*");
                lst.Add($"*(+) После напряженного сражения героический {prsName} расправляется с \"{tskName}\"!*");
                lst.Add($"*(+) У \"{tskName}\" нет никаких шансов!*");
                lst.Add($"*(+) \"{tskName}\" больше не представляет опасности!*");
                var custom = MainViewModel.rnd.Next(0, lst.Count - 1);
                var cust = lst[custom];
                if (tsk.RelToQwests.Any())
                {
                    if (tsk.RelToQwests.Count == 1)
                    {
                        cust += $" *(Квест \"{tsk.RelToQwests.First().NameOfProperty}\")...*";
                    }
                    else
                    {
                        cust += $" *(Квесты: ";
                        for (var i = 0; i < tsk.RelToQwests.Count; i++)
                        {
                            if (i == 0) cust += $"\"{tsk.RelToQwests[i].NameOfProperty}\"";
                            else
                            {
                                cust += $", \"{tsk.RelToQwests[i].NameOfProperty}\"";
                            }
                        }
                        cust += ")...*";
                    }
                }
                return cust;
            }
            return "";
        }

        /// <summary>
        /// Случайный текст для выполненной задачи
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetRandomeTasksNotDoneMessege(object obj)
        {
            var tsk = obj as Task;
            if (tsk != null)
            {
                var lst = new List<string>();
                var prsName = PersProperty.NameOfProperty;
                lst.Add($"*(-) У {prsName} возникли сложности с \"{tsk.NameOfProperty}\"*!");
                lst.Add($"*(-) {prsName} получает критические повреждения от \"{tsk.NameOfProperty}\"*!");
                lst.Add($"*(-) \"{tsk.NameOfProperty}\" атакует!*");
                lst.Add($"*(-) \"{tsk.NameOfProperty}\" яростно нападает!*");
                lst.Add($"*(-) {prsName} избегает сражения с \"{tsk.NameOfProperty}\"*!");
                lst.Add($"*(-) {prsName} ошеломлен \"{tsk.NameOfProperty}\"*!");
                lst.Add($"*(-) \"{tsk.NameOfProperty}\" неистово атакует!*");
                lst.Add($"*(-) \"{tsk.NameOfProperty}\" побеждает!*");
                lst.Add($"*(-) {prsName} убегает от \"{tsk.NameOfProperty}\"!*");
                lst.Add(
                    $"*(-) После напряженного сражения героический {prsName} терпит поражение от \"{tsk.NameOfProperty}\"!*");
                lst.Add($"*(-) {prsName} в недоумении от \"{tsk.NameOfProperty}\"!*");
                lst.Add($"*(-) {prsName} недооценил \"{tsk.NameOfProperty}\"!*");
                var custom = MainViewModel.rnd.Next(0, lst.Count - 1);
                return lst[custom];
            }
            return "";
        }

        /// <summary>
        /// Получить случайное изображение
        /// </summary>
        /// <param name="pathToFolder">Путь к изображению</param>
        /// <returns>Изображение - Название файла с изображением</returns>
        public static Tuple<BitmapImage, string> GetRandomImage(string pathToFolder)
        {
            if (!Directory.Exists(pathToFolder))
            {
                return new Tuple<BitmapImage, string>(null, string.Empty);
            }

            var images =
                Directory.EnumerateFiles(pathToFolder).Where(n => n.EndsWith(".jpg") || n.EndsWith(".png")).ToList();

            if (!images.Any())
            {
                return new Tuple<BitmapImage, string>(null, string.Empty);
            }

            var index = MainViewModel.rnd.Next(0, images.Count);
            var uriString = images[index];

            var bi = new BitmapImage(new Uri(uriString, UriKind.RelativeOrAbsolute));

            return new Tuple<BitmapImage, string>(bi, Path.GetFileNameWithoutExtension(uriString));
        }

        /// <summary>
        /// Получаем интервалы повторения
        /// </summary>
        /// <returns>Интервалы повторов задач</returns>
        public static ObservableCollection<IntervalsModel> GetRepeatIntervals(Task tsk = null)
        {
            var inAbss = false;

            if (tsk != null)
            {
                var inAbs = (from abilitiModel in PersProperty.Abilitis
                             from needTaskse in abilitiModel.NeedTasks
                             where needTaskse.TaskProperty == tsk
                             select new { needTaskse.LevelProperty, Hard = abilitiModel.GetBaseCost() }).ToList();

                if (inAbs.Any())
                {
                    inAbss = true;
                }
            }

            var observableCollection = new ObservableCollection<IntervalsModel>();

            if (!inAbss)
            {
                observableCollection.Add(new IntervalsModel
                {
                    Interval = TimeIntervals.Нет,
                    NameInterval = "Нет"
                });
            }
            else
            {
                observableCollection.Add(new IntervalsModel
                {
                    Interval = TimeIntervals.Ежедневно,
                    NameInterval = "Ежедневно"
                });
                observableCollection.Add(new IntervalsModel
                {
                    Interval = TimeIntervals.Будни,
                    NameInterval = "Будни"
                });
                observableCollection.Add(new IntervalsModel
                {
                    Interval = TimeIntervals.Выходные,
                    NameInterval = "Выходные"
                });
                observableCollection.Add(new IntervalsModel
                {
                    Interval = TimeIntervals.День,
                    NameInterval = "Дни"
                });
                observableCollection.Add(new IntervalsModel
                {
                    Interval =
                        TimeIntervals.ДниНедели,
                    NameInterval = "Дни недели"
                });
                observableCollection.Add(new IntervalsModel {
                    Interval = TimeIntervals.Три,
                    NameInterval = "3"
                });
                observableCollection.Add(new IntervalsModel
                {
                    Interval = TimeIntervals.Четыре,
                    NameInterval = "4"
                });
                observableCollection.Add(new IntervalsModel
                {
                    Interval = TimeIntervals.Шесть,
                    NameInterval = "6"
                });

                //observableCollection.Add(new IntervalsModel
                //{
                //    Interval = TimeIntervals.Сразу,
                //    NameInterval = "Счетчик"
                //});
            }

            return observableCollection;
        }

        /// <summary>
        /// Получить цвет рамки для элемента на который влияет данный элемент
        /// </summary>
        /// <param name="kExpRelayProperty">Коэффициент влияния</param>
        /// <returns></returns>
        public static string getTextFromRelaysWithHardness(int kExpRelayProperty)
        {
            switch (kExpRelayProperty)
            {
                case 0:
                    return "Нет!";

                case 1:
                    return "Очень слабо";

                case 3:
                    return "Слабо";

                case 5:
                    return "Норм";

                case 7:
                    return "Сильно";

                case 9:
                    return "Очень сильно";

                case 10:
                    return "Супер!";

                default:
                    return "нет";
            }
        }

        /// <summary>
        /// Число колонок для униформгрида
        /// </summary>
        /// <param name="count">Число элементов</param>
        /// <returns></returns>
        public static int GetUniformNumOfColumns(int count)
        {
            var sqrt = Math.Sqrt(count);
            return Convert.ToInt32(Math.Ceiling(sqrt));
        }

        /// <summary>
        /// Добавить текст в начало файла
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newText"></param>
        public static void InsertLarge(string path, string newText)
        {
            var book = PersProperty.BookOfSuccess;
            if (!book.Any())
            {
                book.Add("# Приключение начинается...\n");
                WriteAutoBard(AutoBardOperations.КонецХода, null);
            }

            book.Add(newText+"\n");

            //if (!File.Exists(path))
            //{
            //    File.AppendAllText(path, Environment.NewLine, Encoding.UTF8);
            //    File.AppendAllText(path, $"Приключение начинается...", Encoding.UTF8);
            //    File.AppendAllText(path, Environment.NewLine, Encoding.UTF8);
            //    WriteAutoBard(AutoBardOperations.КонецХода, null);
            //    //InsertLarge(path, newText);
            //    //return;
            //}

            //var pathDir = Path.GetDirectoryName(path);
            //var tempPath = Path.Combine(pathDir, Guid.NewGuid().ToString("N"));
            //using (var stream = new FileStream(tempPath, FileMode.Create,
            //    FileAccess.Write, FileShare.None, 4 * 1024 * 1024))
            //{
            //    using (var sw = new StreamWriter(stream))
            //    {
            //        sw.WriteLine(newText + $"  *({DateTime.Now.ToShortTimeString()})*" + Environment.NewLine);
            //        sw.Flush();
            //        using (var old = File.OpenRead(path)) old.CopyTo(sw.BaseStream);
            //    }
            //}
            //File.Delete(path);
            //File.Move(tempPath, path);
        }

        /// <summary>
        /// Проверка - все ли дочерние задачи были выполнены, или хотя бы одна пропущена
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static bool IsAllChildrenTasksDone(Task task)
        {
            return !GetPrevActionsForTask(task).Any();
        }

        /// <summary>
        /// Все предыдущие задачи выполнены?
        /// </summary>
        /// <param name="task">Задача</param>
        /// <returns>Выполнены предыдущие задачи?</returns>
        public static bool isAllPrewTasksDone(Task task)
        {
            var prev = GetPrevActionsForTask(task);

            if (!prev.Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Опыт расчитывается динамически, а не после выполнения квестов?
        /// </summary>
        /// <returns></returns>
        public static bool IsDynamicExp()
        {
            return PersProperty?.PersSettings.IsDynamicExpCount ?? false;
        }

        public static bool IsEnRev(Revard shopItem, bool isCaseGold, int gold, int persLevel,
            out ObservableCollection<string> req)
        {
            var isEn = true;
            req = new ObservableCollection<string>();

            if (isCaseGold)
            {
                // Доступность по золоту
                if (shopItem.CostProperty > gold)
                {
                    isEn = false;
                    req.Add("Недостаточно золота;");
                }
            }

            // Доступность по уровню
            if (shopItem.NeedLevelProperty > persLevel)
            {
                isEn = false;
                req.Add(
                    "Уровень >= " + shopItem.NeedLevelProperty + " (" + persLevel + ");");
            }

            // Доступность по характеристикам
            var strCha = string.Empty;
            var issEn = true;
            GetAbillsReq(shopItem.AbilityNeeds, ref strCha, ref issEn);
            if (issEn == false)
            {
                isEn = false;
                req.Add(strCha);
            }

            // Доступность по скиллам
            var strAb = string.Empty;
            var issEn2 = true;
            GetCharactReq(shopItem.NeedCharacts, ref strAb, ref issEn2);
            if (issEn2 == false)
            {
                isEn = false;
                req.Add(strAb);
            }

            // Доступность по квестам
            foreach (var needQwest in shopItem.NeedQwests.Where(needQwest => needQwest.IsDoneProperty == false))
            {
                isEn = false;
                req.Add("Квест \"" + needQwest.NameOfProperty + "\" должен быть выполнен;");
            }
            return isEn;
        }

        ///// <summary>
        ///// Коэффициент для расчета опыта за уровни скилла. Умножается на опыта за первый уровень скилла???
        ///// </summary>
        ///// <param name="n"></param>
        ///// <returns></returns>
        //public static double KAbLevExp(AbilitiModel n)
        //{
        //    return OneLevExpCost(Config.ExpEasyAb, n.CellValue, Config.AbExpExponenta);
        //}

        /// <summary>
        /// Проверка - а можно ли добавлять скилл?
        /// </summary>
        /// <param name="_pers"></param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool MayAddAbility(Pers _pers)
        {
            if (_pers == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверка - а можно ли добавлять скилл?
        /// </summary>
        /// <param name="_pers"></param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool MayAddQwests(Pers _pers)
        {
            if (_pers == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Минус уровень в требованиях по скиллу
        /// </summary>
        /// <param name="_needVal">Персонаж</param>
        /// <returns>Новое, уменьшенное значение требования</returns>
        public static double minusAbilNeedLevel(double _needVal)
        {
            var value = _needVal;

            value = value - 1.0;

            if (value < 0)
            {
                value = 0;
            }

            return value;
        }

        //public static double OneLevExpCost(double baseXP, int i, double exponenta)
        //{
        //    return Math.Floor(baseXP * Math.Pow(i, exponenta));
        //}

        public static byte[] pathToImage(string path)
        {
            byte[] data = null;

            if (string.IsNullOrEmpty(path) == false && File.Exists(path))
            {
                var bmp = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)) { DecodePixelHeight = 700 };

                var fi = new FileInfo(path);
                if (fi.Extension == ".png")
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }
                }
                else
                {
                    var encoder = new JpegBitmapEncoder { QualityLevel = 80 };
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }
                }
            }

            return data;
        }

        public static BitmapImage pathToImageProperty(string path)
        {
            var im = new BitmapImage();
            var ImageProperty = pathToImage(path);

            if (ImageProperty == null)
            {
                return im;
            }

            using (var mem = new MemoryStream(ImageProperty))
            {
                mem.Position = 0;
                im.BeginInit();
                im.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                im.CacheOption = BitmapCacheOption.OnLoad;
                im.UriSource = null;
                im.StreamSource = mem;
                im.EndInit();
            }

            im.Freeze();
            return im;
        }

        /// <summary>
        /// Проиграть звук
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="soundName"></param>
        public static void PlaySound(UnmanagedMemoryStream stream, bool isSynch = false)
        {
            if (PersProperty.PersSettings.DisableSounds)
            {
                return;
            }

            NotificationSound.Stream = stream;

            if (NotificationSound.Stream != null)
            {
                if (!isSynch)
                {
                    NotificationSound.Play();
                }
                else
                {
                    NotificationSound.PlaySync();
                }
            }
        }

        /// <summary>
        /// The plus need ability level.
        /// </summary>
        /// <param name="_needVal">The _need ability.</param>
        /// <param name="maxLevel">The max level.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public static double plusNeedAbilityLevel(double _needVal, int maxLevel)
        {
            var value = _needVal;

            value = value + 1.0;

            if (value > maxLevel)
            {
                value = maxLevel;
            }

            return value;
        }

        public static double PowerProgression(double xp_for_last_level, double xp_for_first_level, int levels, int level)
        {
            double exp = 0;

            for (var i = 1; i <= level; i++)
            {
                var powerCost = GetPowerCost(xp_for_last_level, xp_for_first_level, levels, i);
                exp = exp + powerCost;
            }

            return exp;
        }

        /// <summary>
        /// Пересчитать все значения
        /// </summary>
        public static void RecauntAllValues()
        {
            PersProperty.RecountRangLevels();

            // Расчет квестов
            RefreshAllQwests(PersProperty, true, true, true);

            // Расчет скиллов
            foreach (var abilitiModel in PersProperty.Abilitis)
            {
                abilitiModel.SetMinMaxValue();
                //abilitiModel.SetToChaRelays();
            }

            AbillitisRefresh(PersProperty);

            // Расчет характеристик
            foreach (var characteristic in PersProperty.Characteristics)
            {
                characteristic.RecountChaValue();
                //characteristic.LevelProperty = characteristic.GetLevel();
                characteristic.SetMinMaxValue();
            }

            // Расчет опыта
            PersProperty.RefreshMaxPersLevel();
            refreshShopItems(PersProperty);
            PersProperty.UpdateAbilityPoints();
            RecountTaskLevels();

            StaticMetods.RecountPersExp();
        }

        /// <summary>
        /// Пересчитать опыт персонажа
        /// </summary>
        public static void RecountPersExp()
        {
            var prs = PersProperty;
            prs.PersExpProperty = GetExp(prs);
            prs.PersLevelProperty = GetLevel(prs.PersExpProperty, RpgItemsTypes.exp);
        }

        /// <summary>
        /// Пересчитать уровни задач
        /// </summary>
        public static void RecountTaskLevels()
        {
            return;
            foreach (var task in PersProperty.Tasks)
            {
                Task.RecountTaskLevel(task);
            }
        }

        /// <summary>
        /// Обновить квесты
        /// </summary>
        /// <param name="persProperty">Персонаж</param>
        /// <param name="_orderQwests">Перераспределять квесты</param>
        /// <param name="_refreshAimList">Обновлять список целей?</param>
        /// <param name="countQwestsProgress"></param>
        public static void RefreshAllQwests(
            Pers persProperty,
            bool _orderQwests,
            bool _refreshAimList,
            bool countQwestsProgress)
        {
            //var pAims = from aim1 in persProperty.Aims
            //    let allPrev = aim1.AllPrev(persProperty)
            //    let max = allPrev.Any() ? allPrev.Max(n => n.MinLevelProperty) : 0
            //    where aim1.MinLevelProperty < max
            //    select new {aim1, max};
            //foreach (var pAim in pAims)
            //{
            //    pAim.aim1.MinLevelProperty = pAim.max;
            //}

            // Подгоняем, чтобы уровни квестов были не меньше чем составные или предыдущие
            foreach (var qw in persProperty.Aims.Where(n => n.GetMinNeedsLev() > n.MinLevelProperty))
            {
                qw.MinLevelProperty = qw.GetMinNeedsLev();
            }

            if (countQwestsProgress)
            {
                AimsViewModel.countQwestsProgress(persProperty.Aims, persProperty);
            }

            AimsViewModel.getQwestReqwirements(
                persProperty.PersLevelProperty,
                persProperty.Aims.ToList(),
                persProperty.PersSettings.MinLevelQwestsMustDoneProperty);

            // Задаем статусы для целей
            AimsViewModel.setQwestStatuses(persProperty.Aims);

            if (_refreshAimList)
            {
                Locator.AimsVM.QCollectionViewProperty.Refresh();
            }
        }

        /// <summary>
        /// Проверить доступность награды
        /// </summary>
        /// <param name="shopItem">Награда</param>
        /// <param name="isCaseGold">Учитывать золото?</param>
        public static void RefreshShopItemEnabled(Revard shopItem, bool isCaseGold = true)
        {
            var gold = PersProperty.gold;
            var persLevel = PersProperty.PersLevelProperty;

            shopItem.IsEnabledProperty = true;
            shopItem.NotAllowReqwirement.Clear();

            ObservableCollection<string> req;
            var isEn = IsEnRev(shopItem, isCaseGold, gold, persLevel, out req);

            shopItem.IsEnabledProperty = isEn;
            shopItem.NotAllowReqwirement = req;

            shopItem.RefreshNeedString();
        }

        /// <summary>
        /// Обновление и проверка на доступность наград
        /// </summary>
        /// <param name="_pers">The _pers.</param>
        public static void refreshShopItems(Pers _pers)
        {
            var _shopItems = _pers.ShopItems;

            foreach (var shopItem in _shopItems)
            {
                RefreshShopItemEnabled(shopItem);
            }
        }

        /// <summary>
        /// Удалить квест
        /// </summary>
        /// <param name="persProperty">Персонаж</param>
        /// <param name="qwestToDelete">Квест который надо удалить</param>
        /// <param name="isAskToDel">Спрашивать удаление?</param>
        public static void RemoveQwest(Pers persProperty, Aim qwestToDelete, bool isAskToDel = true, bool notDel = false)
        {
            var qwests = persProperty.Aims;

            // Удаляем связанные задачи
            deleteLinkedToQwestTasks(persProperty, qwestToDelete);

            // Удаляем связи из всех квестов
            removeQwestFromAnotherQwests(qwestToDelete, qwests);

            // Удаляем из требований наград
            removeQwestFromRewordAndShop(persProperty, qwestToDelete);

            // Удаляем из требований скиллов
            removeQwestFromAbilitis(persProperty, qwestToDelete);
            removeQwestFromAbReqwire(persProperty, qwestToDelete);

            foreach (var abilitiModel in persProperty.Abilitis)
            {
                foreach (var needsForLevel in abilitiModel.NeedsForLevels)
                {
                    foreach (var source in needsForLevel.NeedAims.Where(n => n == qwestToDelete).ToList())
                    {
                        needsForLevel.NeedAims.Remove(source);
                    }
                }
            }

            qwests.Remove(qwestToDelete);

            // Обновляем основные элементы игры
            RecauntAllValues();
            RefreshAllQwests(persProperty, true, true, true);
            AbillitisRefresh(persProperty);
        }

        /// <summary>
        /// Округлить до 10
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double RoundTo10(double d)
        {
            return Math.Floor(d / 10.0) * 10;
        }

        /// <summary>
        /// Округлить до 100
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double RoundTo100(double d)
        {
            return Math.Ceiling(d / 100.0) * 100;
        }

        /// <summary>
        /// Округлить до 5
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double RoundTo5(double d)
        {
            return Math.Ceiling(d / 5) * 5;
        }

        /// <summary>
        /// Показать изменения элемента на персонажа
        /// </summary>
        /// <param name="header">Заголовок</param>
        /// <param name="itemName">Название элемента</param>
        /// <param name="itemImageProperty">Изображение элемента</param>
        /// <param name="viewChanges">Изменения</param>
        /// <param name="col">Цвет</param>
        public static void ShowItemToPersChanges(
            string header,
            byte[] itemImageProperty,
            List<viewChangesModel> viewChanges, Brush col, string date = "", UnmanagedMemoryStream sound = null,
            bool isHowEndOfTurn = false,
            bool isIgnoreEmpty = true)
        {
            if (!viewChanges.Any() && isIgnoreEmpty) return;

            PlaySound(sound);

            var vcw = new ViewChangesWindow();
            vcw.btnOk.Click += (sender, args) => { vcw.Close(); };
            vcw.headerText.Text = header;
            vcw.headerText.Foreground = col;
            vcw.dateText.Visibility = Visibility.Collapsed;
            vcw.Image.Source = getImagePropertyFromImage(itemImageProperty);

            if (!viewChanges.Any())
            {
                vcw.panelWidhIm.SetValue(Grid.RowProperty, 1);
                vcw.Image.MaxHeight = 280;
            }

            Messenger.Default.Send(viewChanges);
            vcw.imEndOfTurn.Visibility = isHowEndOfTurn ? Visibility.Visible : Visibility.Collapsed;

            // Добавляем хоткей
            var saveCommand = new RelayCommand(() => { vcw.Close(); });
            vcw.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Space)));
            vcw.InputBindings.Add(new KeyBinding(saveCommand, new KeyGesture(Key.Return)));
            vcw.ShowDialog();
        }

        /// <summary>
        /// Перетасовка массива
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = MainViewModel.rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void terribleBuffIfNeed(Pers _pers)
        {
            if (MainViewModel.isTerribleBuff(_pers))
            {
                _pers.PersSettings.IsFourViewEnabledProperty = false;
                _pers.PersSettings.IsThreedViewActiveProperty = true;
            }
        }

        /// <summary>
        /// Использовать бутылочку здоровья
        /// </summary>
        /// <param name="prs">Персонаж</param>
        /// <param name="val">Значение бутылочки здоровья</param>
        public static void UseHPBottle(Pers prs, int val)
        {
            // Подсчет изменений тип, гуид, значение
            var vc = new ViewChangesClass(prs.InventoryItems.Union(prs.ShopItems).ToList());
            vc.GetValBefore();

            // Меняем значения
            switch (val)
            {
                case 10:
                    prs.HPProperty.CurrentHPProperty += val;
                    prs.SmallHpBottles--;
                    break;

                case 20:
                    prs.HPProperty.CurrentHPProperty += val;
                    prs.MiddleHpBottles--;
                    break;

                case 40:
                    prs.HPProperty.CurrentHPProperty += val;
                    prs.BigHpBottles--;
                    break;
            }

            // Записываем значения после
            vc.GetValAfter();

            var header = $"Использована бутылочка здоровья!";
            Brush col = Brushes.Green;
            var itemImageProperty = pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png"));

            vc.ShowChanges(header, col, itemImageProperty);
        }

        /// <summary>
        /// Запись автобарда
        /// </summary>
        /// <param name="typeOfWrite">Текстом что произошло</param>
        /// <param name="obj">Объект, который записываем</param>
        public static void WriteAutoBard(AutoBardOperations typeOfWrite, object obj)
        {

            var appFolder = Path.Combine(
                Settings.Default.PathToPers);

            if (Directory.Exists(appFolder) == false)
            {
                Directory.CreateDirectory(appFolder);
            }

            var path = Path.Combine(appFolder, "TheBookOfSuccess.md");

            switch (typeOfWrite)
            {
                case AutoBardOperations.КонецХода:
                    var dateTime = new DateTime(MainViewModel.selectedTime.Date.Year,
                        MainViewModel.selectedTime.Date.Month, MainViewModel.selectedTime.Date.Day);
                    InsertLarge(path, $" # {dateTime.ToString("D")}");
                    break;

                case AutoBardOperations.ЗадачаВыполнена:
                    InsertLarge(path, GetRandomeTasksDoneMessege(obj));
                    break;

                case AutoBardOperations.ЗадачаНеВыполнена:
                    InsertLarge(path, GetRandomeTasksNotDoneMessege(obj));
                    break;

                case AutoBardOperations.КвестВыполнен:
                    var qw = obj as Aim;
                    if (qw != null)
                    {
                        InsertLarge(path,
                            $"### {PersProperty.NameOfProperty} выполняет квест \"{qw.NameOfProperty}\"...!!!");
                    }
                    break;

                case AutoBardOperations.КвестСоздан:
                    var aim = obj as Aim;
                    if (aim != null)
                    {
                        InsertLarge(path,
                            $"### {PersProperty.NameOfProperty} решается на новое приключение: \"{aim.NameOfProperty}\"!!!");
                    }
                    break;

                case AutoBardOperations.НавыкОткрыт:
                    var ab = obj as AbilitiModel;
                    if (ab != null)
                    {
                        InsertLarge(path,
                            $"### {PersProperty.NameOfProperty} решает развить у себя навык \"{ab.NameOfProperty}\"...");
                    }
                    break;

                case AutoBardOperations.НавыкПрокачан:
                    var ab2 = obj as AbilitiModel;
                    if (ab2 != null)
                    {
                        InsertLarge(path,
                            $"### Навык \"{ab2.NameOfProperty}\" изменен на значение \"{ab2.RangName}\"...");
                    }
                    break;

                case AutoBardOperations.ХарактеристикаПрокачана:
                    var cha = obj as Characteristic;
                    if (cha != null)
                    {
                        InsertLarge(path,
                            $"### Характеристика \"{cha.NameOfProperty}\" изменена на значение \"{cha.RangName}\"...");
                    }
                    break;

                case AutoBardOperations.НаградаПолучена:
                    var rev = obj as Revard;
                    if (rev != null)
                    {
                        InsertLarge(path,
                            $"За свои заслуги {PersProperty.NameOfProperty} получает награду - \"{rev.NameOfProperty}\".");
                    }
                    break;

                case AutoBardOperations.РангИзменен:
                    InsertLarge(path,
                        $"## {PersProperty.NameOfProperty} достигает нового звания - {PersProperty.RangName}!!!");
                    break;

                case AutoBardOperations.УровеньПовышен:
                    InsertLarge(path,
                        $"## Ценой невероятных усилий {PersProperty.NameOfProperty} достигает {PersProperty.PersLevelProperty} уровня!");
                    break;

                case AutoBardOperations.УровеньПонижен:
                    InsertLarge(path,
                        $"## Неудача! {PersProperty.NameOfProperty} понижает свой уровень до {PersProperty.PersLevelProperty}!");
                    break;

                case AutoBardOperations.СНачала:
                    InsertLarge(path, $"# {PersProperty.NameOfProperty} решает начать все сначала!");
                    break;

                case AutoBardOperations.ПолностьюСНачала:
                    PersProperty.BookOfSuccess.Clear();
                    break;
            }
        }

        private static void CollapsePrevNextQwests(EditQwestWindowView editQwest)
        {
            editQwest.btnNextQwest.Visibility = Visibility.Collapsed;
            editQwest.btnPrevQwest.Visibility = Visibility.Collapsed;
        }

        private static double CountKCharacteristics()
        {
            var k = 0;

            foreach (
                var characteristic in PersProperty.Characteristics.Where(n => n.FirstLevelProperty < n.MaxLevelProperty)
                )
            {
                k = k + characteristic.NeedAbilitisProperty.Count(n => n.KoeficientProperty > 0);
            }

            return k;
        }

        private static void deleteLinkedToQwestTasks(Pers persProperty, Aim qwestToDelete)
        {
            var linkTasks = qwestToDelete.NeedsTasks.ToList();
            foreach (var linkTask in linkTasks)
            {
                qwestToDelete.DeleteTaskNeed(persProperty, linkTask);
            }
        }

        ///// <summary>
        ///// Экспаненциальный рост
        ///// </summary>
        ///// <param name="level"></param>
        ///// <returns></returns>
        //private static double expa(int level)
        //{
        //    var baseXP = 100.0;
        //    double exp = 0;

        // for (var i = 0; i <= level; i++) { var powerCost = OneLevExpCost(baseXP, i,
        // Config.ExpExponenta); exp = exp + powerCost; }

        //    return exp;
        //}

        /// <summary>
        /// Опыт за скиллы и их влияние на характеристики
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        private static double ExpFromAbChaRelay(Pers pers)
        {
            double currentPersProgress = 0;

            currentPersProgress += pers.Abilitis.Sum(
                n =>
                {
                    var count =
                        PersProperty.Characteristics.Sum(
                            t => t.NeedAbilitisProperty.Where(q => q.AbilProperty == n).Sum(q => q.KoeficientProperty));
                    var dolya = count * n.ValueProperty / 5.0;
                    return dolya;
                });

            var k = 4.0 / PersProperty.Characteristics.Count;

            return k * currentPersProgress;
        }

        /// <summary>
        /// Опыт в зависимости от скиллов и их сложностей
        /// </summary>
        /// <param name="pers">Персонаж</param>
        /// <returns>Опыт</returns>
        private static double ExpFromAbHardness(Pers pers)
        {
            return 0;
        }

        /// <summary>
        /// Опыт в зависимости от прогресса в характеристиках
        /// </summary>
        /// <param name="pers">Персонаж</param>
        /// <returns></returns>
        private static double ExpFromChaProgress(Pers pers)
        {
            if (!PersProperty.Characteristics.Any())
            {
                return 0;
            }

            // Перерасчет коэффициентов характеристик
            var maxValChenge = pers.Characteristics.Max(n => n.GetValChenge());

            foreach (var characteristic in pers.Characteristics)
            {
                var k = characteristic.GetValChenge() / maxValChenge;
                var kExpRelayProperty = k;
                characteristic.KExpRelayProperty = kExpRelayProperty;
            }

            var sumOfK =
                pers.Characteristics.Sum(n => n.KExpRelayProperty);

            sumOfK = Math.Abs(sumOfK) < 0.001 ? 1 : sumOfK;

            double currentPersProgress = 0;

            currentPersProgress += pers.Characteristics.Sum(
                n =>
                {
                    if (n.FirstLevelProperty >= n.MaxLevelProperty) return 0;
                    var count = n.KExpRelayProperty;
                    var dolya = count / sumOfK;
                    var curProgress = n.CharacteristicProgress;
                    return dolya * curProgress;
                });

            var expForMaxLevel = Pers.ExpToLevel(
                pers.PersSettings.MaxPersLevelProperty,
                RpgItemsTypes.exp) + 1.0;

            var exp = expForMaxLevel * currentPersProgress;
            return exp;
        }

        /// <summary>
        /// Получить данные для расчета опыта на основе характеристик
        /// </summary>
        /// <param name="pers"></param>
        /// <param name="curProgress"></param>
        /// <param name="maxProgress"></param>
        private static void GetCurMaxPersProgressByCha(Pers pers, out double curProgress, out double maxProgress)
        {
            curProgress = pers.Characteristics.Sum(n => n.ValueProperty - n.FirstVal);
            maxProgress = pers.Characteristics.Sum(n => 10.0 - n.FirstVal);

            var mainAbs = Convert.ToInt32(PersProperty.Characteristics.Count);
            var secondAbs = Convert.ToInt32(PersProperty.Characteristics.Count * 2);
            var orderedAbills = pers.Abilitis.OrderByDescending(n => n.ChaPriority).ToList();
            var mainAbilities = orderedAbills.Take(mainAbs).ToList();
            var secondAbilitis = orderedAbills.Skip(mainAbs).Take(secondAbs).ToList();
            var lastAbilities = orderedAbills.Skip(mainAbs + secondAbs).ToList();
            mainAbilities.ForEach(n => n.TESPriority = 5);
            secondAbilitis.ForEach(n => n.TESPriority = 2);
            lastAbilities.ForEach(n => n.TESPriority = 1);
        }

        private static string getLinkedToAbilitiTasksNames(List<Task> linkedTasks)
        {
            return linkedTasks.Aggregate(
                string.Empty,
                (current, linkedTask) => current + "\n * " + linkedTask.NameOfProperty + ";");
        }

        private static List<Task> getLinkedToAbilityTasks(AbilitiModel _ability)
        {
            return _ability.NeedTasks.Select(n => n.TaskProperty).Distinct().ToList();
        }

        /// <summary>
        /// получить максимальное количество очков опыта
        /// </summary>
        /// <param name="persProperty"></param>
        /// <returns></returns>
        private static double GetMaxExp(Pers persProperty)
        {
            List<AbilitiModel> mainAbilities;
            List<AbilitiModel> secondAbilitis;
            List<AbilitiModel> lastAbilities;
            GetMainSecondLastAbs(out mainAbilities, persProperty, out secondAbilitis, out lastAbilities);

            var m = mainAbilities.Count * 6.0;

            return m;
        }

        private static double HabbitRpgExp(int level)
        {
            double exp = 0;

            for (var i = 0; i < level; i++)
            {
                var powerCost = 100 * Math.Pow(1.05, i);
                powerCost = Math.Ceiling(powerCost / 5.0) * 5;
                exp = exp + powerCost;
            }

            return exp;
        }

        private static void RefreshSelAimInAimsList(Aim aimProperty)
        {
            Locator.AimsVM.SelectedAimProperty = null;
            Locator.AimsVM.SelectedAimProperty = aimProperty;
        }

        private static void removeAbFromCharacteristics(Pers _pers, AbilitiModel _ability)
        {
            foreach (var characteristic in _pers.Characteristics)
            {
                foreach (
                    var source in characteristic.NeedAbilitisProperty.Where(n => n.AbilProperty == _ability).ToList())
                {
                    characteristic.NeedAbilitisProperty.Remove(source);
                }
            }
        }

        /// <summary>
        /// Удаляем скилл из инвентаря и магазина
        /// </summary>
        /// <param name="_pers"></param>
        /// <param name="_ability"></param>
        private static void removeAbFromInventoryAndShop(Pers _pers, AbilitiModel _ability)
        {
            foreach (var inventoryItem in _pers.InventoryItems)
            {
                foreach (var source in
                    inventoryItem.ChangeAbilitis.Where(n => n.AbilityProperty == _ability).ToList())
                {
                    inventoryItem.ChangeAbilitis.Remove(source);
                }
            }

            foreach (var shopItem in _pers.ShopItems)
            {
                foreach (var source in
                    shopItem.ChangeAbilitis.Where(n => n.AbilityProperty == _ability).ToList())
                {
                    shopItem.ChangeAbilitis.Remove(source);
                }
            }

            // Удаляем из требований наград
            foreach (var shopItem in _pers.ShopItems)
            {
                foreach (var source in shopItem.AbilityNeeds.Where(n => n.AbilProperty == _ability).ToList())
                {
                    shopItem.AbilityNeeds.Remove(source);
                }
            }

            foreach (var revard in _pers.InventoryItems)
            {
                foreach (var source in revard.AbilityNeeds.Where(n => n.AbilProperty == _ability).ToList())
                {
                    revard.AbilityNeeds.Remove(source);
                }
            }
        }

        /// <summary>
        /// Удаляем связанные квесты (по требованию)
        /// </summary>
        /// <param name="_pers"></param>
        /// <param name="_ability"></param>
        /// <param name="linkTaskName"></param>
        private static void removeLinkedWithAbQwests(Pers _pers, AbilitiModel _ability, string linkTaskName)
        {
            var linkedQwests = _ability.NeedAims.Select(n => n.AimProperty).Distinct().ToList();

            if (linkedQwests.Count > 0)
            {
                //var messageBoxResult =
                //    MessageBox.Show(
                //        "Удалить также связанные с скиллом квесты? \n" + linkTaskName,
                //        "Внимание!",
                //        MessageBoxButton.OKCancel);

                //if (messageBoxResult == MessageBoxResult.OK)
                //{
                foreach (var linkedQwest in linkedQwests)
                {
                    RemoveQwest(_pers, linkedQwest);
                }
                //}
            }
        }

        /// <summary>
        /// Удаляем квест из требований прокачки скиллов
        /// </summary>
        /// <param name="persProperty"></param>
        /// <param name="qwestToDelete"></param>
        private static void removeQwestFromAbilitis(Pers persProperty, Aim qwestToDelete)
        {
            foreach (var abilitiModel in persProperty.Abilitis)
            {
                // Удаляем из требований скиллов для прокачки
                foreach (var needTasks in
                    abilitiModel.NeedAims.Where(n => n.AimProperty == qwestToDelete).ToList())
                {
                    abilitiModel.NeedAims.Remove(needTasks);
                }
            }
        }

        /// <summary>
        /// Удаляем квест из условий активности скилла
        /// </summary>
        /// <param name="persProperty"></param>
        /// <param name="qwestToDelete"></param>
        private static void removeQwestFromAbReqwire(Pers persProperty, Aim qwestToDelete)
        {
            foreach (var abilitiModel in persProperty.Abilitis)
            {
                foreach (var aim in abilitiModel.ReqwireAims.Where(n => n == qwestToDelete).ToList())
                {
                    abilitiModel.ReqwireAims.Remove(aim);
                }
            }
        }

        /// <summary>
        /// Удаляем квест из других квестов (требований и составляющих)
        /// </summary>
        /// <param name="qwestToDelete"></param>
        /// <param name="qwests"></param>
        private static void removeQwestFromAnotherQwests(Aim qwestToDelete, ObservableCollection<Aim> qwests)
        {
            foreach (var aim in qwests)
            {
                foreach (var need in
                    aim.Needs.Where(need => need == qwestToDelete).ToList())
                {
                    aim.Needs.Remove(need);
                }

                // Удаляем из составляющих квестов
                foreach (var compositeAims in
                    aim.CompositeAims.Where(n => n.AimProperty == qwestToDelete).ToList())
                {
                    aim.CompositeAims.Remove(compositeAims);
                }
            }
        }

        /// <summary>
        /// Удаление квеста из наград и инвентаря
        /// </summary>
        /// <param name="persProperty"></param>
        /// <param name="qwestToDelete"></param>
        private static void removeQwestFromRewordAndShop(Pers persProperty, Aim qwestToDelete)
        {
            foreach (var shopItem in persProperty.ShopItems)
            {
                foreach (var needQwest in shopItem.NeedQwests.Where(n => n == qwestToDelete).ToList())
                {
                    shopItem.NeedQwests.Remove(needQwest);
                }
            }

            foreach (var revard in persProperty.InventoryItems)
            {
                foreach (var needQwest in revard.NeedQwests.Where(n => n == qwestToDelete).ToList())
                {
                    revard.NeedQwests.Remove(needQwest);
                }
            }
        }

        private static void showRemoveLinkedTasksMessege(Pers _pers, List<Task> linkedTasks, string linkTaskName)
        {
            if (linkedTasks.Count > 0)
            {
                //var messageBoxResult =
                //    MessageBox.Show(
                //        "Удалить также связанные с скиллом задачи? \n" + linkTaskName,
                //        "Внимание!",
                //        MessageBoxButton.OKCancel);

                //if (messageBoxResult == MessageBoxResult.OK)
                //{
                foreach (var linkedTask in linkedTasks)
                {
                    linkedTask.Delete(_pers);
                }
                //}
            }
        }

        /// <summary>
        /// Треугольная прогрессия
        /// </summary>
        /// <param name="level">Уровень</param>
        /// <returns></returns>
        private static double TriangleProgression(int level)
        {
            // Quadro
            //double exp = 0;

            //for (int i = 1; i <= level; i++)
            //{
            //    var powerCost = 100 * (2*i-1);
            //    exp = exp + powerCost;
            //}

            //return exp;

            var expToLevel = level * (level + 1) / 2.0;
            expToLevel = expToLevel * 100.0;
            return expToLevel;
        }

        /// <summary>
        /// Прогрессия как в WOW
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static double WoWProgression(int level)
        {
            var expToLevel = Math.Pow(level, 2) * 100.0 + level * 900.0;
            return expToLevel;
        }

        public enum AutoBardOperations
        {
            КонецХода,
            ЗадачаВыполнена,
            ЗадачаНеВыполнена,
            КвестВыполнен,
            КвестСоздан,
            НавыкОткрыт,
            НавыкПрокачан,
            ХарактеристикаПрокачана,
            НаградаПолучена,
            РангИзменен,
            УровеньПовышен,
            УровеньПонижен,
            СНачала,
            ПолностьюСНачала
        }

        public const int DefoultKForTaskNeed = 3;

        /// <summary>
        /// Влияение квестов на скиллы по умолчанию (слабо)
        /// </summary>
        public static double AbQwestKRelayDefoult = 10.0;

        public static StaticSettings Config;
        public static double ConstOfAbs = 1.0 / 4.0 / 30.0;
        public static double LowRelAbToCha = 1;
        public static double MaxRelAbToCha = 1;
        public static double MidRelAbToCha = 1;
        public static SoundPlayer NotificationSound = new SoundPlayer();

        /// <summary>
        /// Интервал для таймеров
        /// </summary>
        public static TimeSpan timeSpan = new TimeSpan(0, 0, 1, 0);

        private const int BaseHp = 50;

        public class AlterAbRelays
        {
            public AbilitiModel Ability { get; set; }

            public ChangeAbilityModele ChangeIfDone { get; set; }

            public ChangeAbilityModele ChangeIfNotDone { get; set; }

            public int Count { get; set; }
            public int Hardness { get; set; }
        }
    }
}
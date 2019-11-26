// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucQwestMap.cs" company="">
//
// </copyright>
// <summary>
//   Вью модель для карты целей
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Graphviz4Net.Graphs;
using Graphviz4Net.WPF.ViewModels;
using Sample.Annotations;
using Sample.Model;
using Sample.View;

namespace Sample.ViewModel
{
    /// <summary>
    ///     Вью модель для карты целей
    /// </summary>
    public class ucQwestMap : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ucQwestMap" /> class.
        /// </summary>
        public ucQwestMap()
        {
            OnPropertyChanged(nameof(PathToGraphvizProperty));
            LinkWithQwestLevelsProperty = PersProperty.PersSettings.ShowLinksWithNotSameLevelsProperty;
            ShowLevelsProperty = PersProperty.PersSettings.DivideByLevelsProperty;
            if (PersProperty.PersSettings.PathToMapBackgroundProperty == null
                || string.IsNullOrEmpty(PersProperty.PersSettings.PathToMapBackgroundProperty))
            {
                PersProperty.PersSettings.PathToMapBackgroundProperty =
                    Path.Combine(Directory.GetCurrentDirectory(), "Images", "map.jpg");
            }

            OnPropertyChanged(nameof(MapBackgroundProperty));

            Messenger.Default.Register<string>(
                this,
                mes =>
                {
                    if (mes == "mapUpdate")
                    {
                        StaticMetods.RefreshAllQwests(PersProperty, true, true, false);
                        buildGraph();
                        OnPropertyChanged(nameof(QwestsGraphProperty));
                    }
                });
        }

        #endregion Constructors and Destructors

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Fields

        /// <summary>
        ///     Персонаж.
        /// </summary>
        private Pers Pers;

        /// <summary>
        ///     Gets the Сделать квест посередине.
        /// </summary>
        private RelayCommand<EdgeLabelViewModel> addBetweenQwestCommand;

        /// <summary>
        ///     Gets the Добавить дочерний квест.
        /// </summary>
        private RelayCommand<Aim> addChildQwestCommand;

        /// <summary>
        ///     Комманда Добавить связь между квестами.
        /// </summary>
        private RelayCommand addLinkCommand;

        /// <summary>
        ///     Комманда Создать связь!.
        /// </summary>
        private RelayCommand addLinkToCommand;

        /// <summary>
        ///     Gets the Добавить родительский квест.
        /// </summary>
        private RelayCommand<Aim> addParrentQwestCommand;

        /// <summary>
        ///     Комманда Сменить фон.
        /// </summary>
        private RelayCommand changeBackgroundCommand;

        /// <summary>
        ///     Дочерний квест.
        /// </summary>
        private Aim childQwest;

        /// <summary>
        ///     Комманда Отменить добавление связи.
        /// </summary>
        private RelayCommand clearAddLinkCommand;

        /// <summary>
        ///     Комманда Копировать для связи.
        /// </summary>
        private RelayCommand copyToAddChildCommand;

        /// <summary>
        ///     Комманда Удалить связь между квестами.
        /// </summary>
        private RelayCommand deleteLinkCommand;

        /// <summary>
        ///     Дочернияя цель для перетаскивания.
        /// </summary>
        private Aim dragChildAim;

        /// <summary>
        ///     Родительская цель для перетаскивания и связи.
        /// </summary>
        private Aim dragParrent;

        /// <summary>
        ///     Gets the Понизить уровень квеста.
        /// </summary>
        private RelayCommand<Aim> levelDownCommand;

        /// <summary>
        ///     Gets the Поднять уровень квеста.
        /// </summary>
        private RelayCommand<Aim> levelUpCommand;

        /// <summary>
        ///     Связь между квестами разных уровней.
        /// </summary>
        private bool linkWithQwestLevels;

        /// <summary>
        ///     Минлен - расстояние между связями.
        /// </summary>
        private double minLen = 3;

        /// <summary>
        ///     Gets the Сдвигаем все квесты текущего уровня и выше на уровень вверх.
        /// </summary>
        private RelayCommand<string> moveAllUpCommand;

        /// <summary>
        ///     Gets the Сдвинуть все квесты этого уровня и выше на уровень вниз.
        /// </summary>
        private RelayCommand<string> moveDownCommand;

        /// <summary>
        ///     Родительский квест.
        /// </summary>
        private Aim parrentQwest;

        /// <summary>
        ///     Квест для которого надо добавить связь.
        /// </summary>
        private Aim qwestForAddChild;

        

        /// <summary>
        ///     Комманда Обновить карту.
        /// </summary>
        private RelayCommand refreshMapCommand;

        /// <summary>
        ///     Выбранный квест.
        /// </summary>
        private Aim selectedQwest;

       

        /// <summary>
        ///     Показывать разделение по уровням?.
        /// </summary>
        private bool showLevels = true;

        /// <summary>
        ///     Комманда Вернуть стандартный фон карты.
        /// </summary>
        private RelayCommand standartBackgroundCommand;

        #endregion Fields

        #region Public Properties

        /// <summary>
        ///     Gets the Удалить связь между квестами.
        /// </summary>
        private RelayCommand<EdgeLabelViewModel> delQwestRelayCommand;

        /// <summary>
        ///     Gets the Удалить связь между квестами.
        /// </summary>
        public RelayCommand<EdgeLabelViewModel> DelQwestRelayCommand
        {
            get
            {
                return delQwestRelayCommand
                       ?? (delQwestRelayCommand =
                           new RelayCommand<EdgeLabelViewModel>(
                               item =>
                               {
                                   var parAim = (Aim) item.Edge.Destination;
                                   var childAim = (Aim) item.Edge.Source;

                                   parAim.Needs.Remove(childAim);

                                   Messenger.Default.Send("mapUpdate");
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
        ///     Gets the Сделать квест посередине.
        /// </summary>
        public RelayCommand<EdgeLabelViewModel> AddBetweenQwestCommand
        {
            get
            {
                return addBetweenQwestCommand
                       ?? (addBetweenQwestCommand =
                           new RelayCommand<EdgeLabelViewModel>(
                               item =>
                               {
                                   var parAim = (Aim) item.Edge.Destination;
                                   var childAim = (Aim) item.Edge.Source;
                                   var betweenAim = new Aim(PersProperty);

                                   parAim.Needs.Add(betweenAim);
                                   betweenAim.Needs.Add(childAim);
                                   parAim.Needs.Remove(childAim);
                                   betweenAim.MinLevelProperty = childAim.MinLevelProperty;

                                   StaticMetods.editAim(betweenAim);

                                   SelectedQwestProperty = betweenAim;

                                   mapRefresh();
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
        ///     Gets the Добавить дочерний квест.
        /// </summary>
        public RelayCommand<Aim> AddChildQwestCommand
        {
            get
            {
                return addChildQwestCommand
                       ?? (addChildQwestCommand = new RelayCommand<Aim>(
                           parrentAim =>
                           {
                               if (Keyboard.Modifiers == ModifierKeys.Control)
                               {
                                   // Добавляем новый квест, для которого этот будет составным
                                   var chAim = new Aim(PersProperty);

                                   parrentAim.CompositeAims.Add(
                                       new CompositeAims {AimProperty = chAim, KoeficientProperty = 10});
                                   StaticMetods.Locator.AimsVM.SelectedAimProperty = chAim;

                                   chAim.ImageProperty = parrentAim.ImageProperty;
                                   chAim.MinLevelProperty = parrentAim.MinLevelProperty > PersProperty.PersLevelProperty ? parrentAim.MinLevelProperty : PersProperty.PersLevelProperty;

                                   StaticMetods.CopyAimSkills(parrentAim, chAim);
                                   StaticMetods.editAim(chAim);
                               }
                               else
                               {
                                   var _pers = PersProperty;
                                   var childAim = StaticMetods.addChildAim(_pers, parrentAim);

                                   SelectedQwestProperty = childAim;
                               }

                               mapRefresh();
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
        ///     Gets the комманда Добавить связь между квестами.
        /// </summary>
        public RelayCommand AddLinkCommand
        {
            get
            {
                return addLinkCommand ?? (addLinkCommand = new RelayCommand(
                    () =>
                    {
                        ParrentQwestProperty.Needs.Add(ChildQwestProperty);
                        Messenger.Default.Send("mapUpdate");
                    },
                    () =>
                    {
                        if (ChildQwestProperty == null || ParrentQwestProperty == null)
                        {
                            return false;
                        }

                        if (ParrentQwestProperty.Needs.Count(n => n == ChildQwestProperty) > 0)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }


        /// <summary>
        ///     Gets the Добавить следующий квест.
        /// </summary>
        private RelayCommand<object> addNextQwestCommand;

        /// <summary>
        ///     Gets the Добавить следующий квест.
        /// </summary>
        public RelayCommand<object> AddNextQwestCommand
        {
            get
            {
                return addNextQwestCommand
                       ?? (addNextQwestCommand = new RelayCommand<object>(
                           item =>
                           {
                               var aim = item as Aim;
                               StaticMetods.addParrentQwest(PersProperty, aim, true);
                               mapRefresh();
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
        ///     Gets the комманда Создать связь!.
        /// </summary>
        public RelayCommand AddLinkToCommand
        {
            get
            {
                return addLinkToCommand
                       ?? (addLinkToCommand = new RelayCommand(
                           () =>
                           {

                               StaticMetods.CopyAimSkills(QwestForAddChildProperty, SelectedQwestProperty);

                               if (Keyboard.Modifiers != ModifierKeys.Control)
                               {
                                   Messenger.Default.Send(
                                       new AddLinkMessege(
                                           QwestForAddChildProperty.GUID,
                                           SelectedQwestProperty.GUID));
                               }
                               else
                               {
                                   Messenger.Default.Send(
                                       new AddLinkMessege(
                                           QwestForAddChildProperty.GUID,
                                           SelectedQwestProperty.GUID),
                                       true);
                               }
                           },
                           () =>
                           {
                               if (SelectedQwestProperty == null
                                   || SelectedQwestProperty == QwestForAddChildProperty)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Добавить родительский квест.
        /// </summary>
        public RelayCommand<Aim> AddParrentQwestCommand
        {
            get
            {
                return addParrentQwestCommand
                       ?? (addParrentQwestCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               if (Keyboard.Modifiers == ModifierKeys.Control)
                               {
                                   // Добавляем новый квест, для которого этот будет составным
                                   var parAim = new Aim(PersProperty);
                                   parAim.CompositeAims.Add(
                                       new CompositeAims {AimProperty = item, KoeficientProperty = 10});
                                   parAim.ImageProperty = item.ImageProperty;
                                   StaticMetods.Locator.AimsVM.SelectedAimProperty = parAim;
                                   Messenger.Default.Send("Квест добавлен");
                                   parAim.MinLevelProperty = item.MinLevelProperty;
                                   parAim.ImageProperty = item.ImageProperty;
                                   parAim.MinLevelProperty = item.MinLevelProperty > PersProperty.PersLevelProperty ? item.MinLevelProperty : PersProperty.PersLevelProperty;
                                   //parAim.MinLevelProperty = item.MinLevelProperty>PersProperty.PersLevelProperty?item.MinLevelProperty:PersProperty.PersLevelProperty;
                                   StaticMetods.CopyAimSkills(item, parAim);
                                   StaticMetods.editAim(parAim);
                               }
                               else
                               {
                                   var aim = StaticMetods.addParrentQwest(PersProperty, item);
                                   SelectedQwestProperty = aim;
                               }

                               mapRefresh();
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
        ///     Gets the комманда Сменить фон.
        /// </summary>
        public RelayCommand ChangeBackgroundCommand
        {
            get
            {
                return changeBackgroundCommand
                       ?? (changeBackgroundCommand = new RelayCommand(
                           () =>
                           {
                               // string path = StaticMetods.GetPathToImage();
                               //if (path != null)
                               //{
                               //    this.PersProperty.PersSettings.PathToMapBackgroundProperty = path;
                               //    this.RaisePropertyChanged(() => this.MapBackgroundProperty);
                               //}
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Дочерний квест.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim ChildQwestProperty
        {
            get { return childQwest; }

            set
            {
                if (childQwest == value)
                {
                    return;
                }

                childQwest = value;
                OnPropertyChanged(nameof(ChildQwestProperty));
            }
        }

        /// <summary>
        ///     Gets the комманда Отменить добавление связи.
        /// </summary>
        public RelayCommand ClearAddLinkCommand
        {
            get
            {
                return clearAddLinkCommand
                       ?? (clearAddLinkCommand = new RelayCommand(
                           () =>
                           {
                               DragChildAimProperty = null;
                               DragParrentProperty = null;
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Копировать для связи.
        /// </summary>
        public RelayCommand CopyToAddChildCommand
        {
            get
            {
                return copyToAddChildCommand
                       ?? (copyToAddChildCommand =
                           new RelayCommand(
                               () => { QwestForAddChildProperty = SelectedQwestProperty; },
                               () =>
                               {
                                   if (SelectedQwestProperty == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the Копировать квест как дочерний для связи.
        /// </summary>
        private RelayCommand<Aim> copyToChildCommand;

        /// <summary>
        ///     Gets the Копировать квест как дочерний для связи.
        /// </summary>
        public RelayCommand<Aim> CopyToChildCommand
        {
            get
            {
                return copyToChildCommand
                       ?? (copyToChildCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               ChildQwestProperty = item;
                               DragChildAimProperty = item;
                               DragParrentProperty = null;
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
        ///     Gets the копировать квест как родительский (для связи).
        /// </summary>
        private RelayCommand<Aim> copyToParrentCommand;

        /// <summary>
        ///     Gets the копировать квест как родительский (для связи).
        /// </summary>
        public RelayCommand<Aim> CopyToParrentCommand
        {
            get
            {
                return copyToParrentCommand
                       ?? (copyToParrentCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               ParrentQwestProperty = item;
                               DragChildAimProperty = null;
                               DragParrentProperty = item;
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
        ///     Gets the комманда Удалить связь между квестами.
        /// </summary>
        public RelayCommand DeleteLinkCommand
        {
            get
            {
                return deleteLinkCommand
                       ?? (deleteLinkCommand = new RelayCommand(
                           () =>
                           {
                               ParrentQwestProperty.Needs.Remove(ChildQwestProperty);
                               Messenger.Default.Send("mapUpdate");
                           },
                           () =>
                           {
                               if (ChildQwestProperty == null || ParrentQwestProperty == null)
                               {
                                   return false;
                               }

                               if (ParrentQwestProperty.Needs.Count(n => n == ChildQwestProperty) > 0)
                               {
                                   return true;
                               }

                               return false;
                           }));
            }
        }

        /// <summary>
        ///     Sets and gets Дочернияя цель для перетаскивания.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim DragChildAimProperty
        {
            get { return dragChildAim; }

            set
            {
                if (dragChildAim == value)
                {
                    return;
                }

                dragChildAim = value;
                OnPropertyChanged(nameof(DragChildAimProperty));
                OnPropertyChanged(nameof(ShowLinkTooltipProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Родительская цель для перетаскивания и связи.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim DragParrentProperty
        {
            get { return dragParrent; }

            set
            {
                if (dragParrent == value)
                {
                    return;
                }

                dragParrent = value;
                OnPropertyChanged(nameof(DragParrentProperty));
                OnPropertyChanged(nameof(ShowLinkTooltipProperty));
            }
        }

        /// <summary>
        ///     Gets the Понизить уровень квеста.
        /// </summary>
        public RelayCommand<Aim> LevelDownCommand
        {
            get
            {
                return levelDownCommand
                       ?? (levelDownCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               item.MinLevelProperty--;
                               mapRefresh();
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
        ///     Gets the Поднять уровень квеста.
        /// </summary>
        public RelayCommand<Aim> LevelUpCommand
        {
            get
            {
                return levelUpCommand
                       ?? (levelUpCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               item.MinLevelProperty++;
                               mapRefresh();
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
        ///     Sets and gets Связь между квестами разных уровней.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool LinkWithQwestLevelsProperty
        {
            get { return linkWithQwestLevels; }

            set
            {
                if (linkWithQwestLevels == value)
                {
                    return;
                }

                linkWithQwestLevels = value;
                OnPropertyChanged(nameof(LinkWithQwestLevelsProperty));
                PersProperty.PersSettings.ShowLinksWithNotSameLevelsProperty = LinkWithQwestLevelsProperty;
            }
        }

        /// <summary>
        ///     Sets and gets Фон карты.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public BitmapImage MapBackgroundProperty
        {
            get
            {
                if (PersProperty == null)
                {
                    return null;
                }

                BitmapImage mapBackgroundProperty;

                try
                {
                    mapBackgroundProperty =
                        new BitmapImage(
                            new Uri(
                                PersProperty.PersSettings.PathToMapBackgroundProperty, UriKind.RelativeOrAbsolute));
                }
                catch (Exception)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", "map.jpg");
                    mapBackgroundProperty = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                    PersProperty.PersSettings.PathToMapBackgroundProperty = path;
                }

                return mapBackgroundProperty;
            }
        }

        /// <summary>
        ///     Sets and gets Минлен - расстояние между связями.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public double MinLenProperty
        {
            get { return minLen; }

            set
            {
                if (minLen == value)
                {
                    return;
                }

                minLen = value;
                OnPropertyChanged(nameof(MinLenProperty));
            }
        }

        /// <summary>
        ///     Gets the Сдвигаем все квесты текущего уровня и выше на уровень вверх.
        /// </summary>
        public RelayCommand<string> MoveAllUpCommand
        {
            get
            {
                return moveAllUpCommand
                       ?? (moveAllUpCommand = new RelayCommand<string>(
                           item =>
                           {
                               var level = Convert.ToInt32(item);
                               foreach (var source in PersProperty.Aims.Where(n => n.MinLevelProperty >= level))
                               {
                                   source.MinLevelProperty++;
                               }

                               mapRefresh();
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
        ///     Gets the Сдвинуть все квесты этого уровня и выше на уровень вниз.
        /// </summary>
        public RelayCommand<string> MoveDownCommand
        {
            get
            {
                return moveDownCommand
                       ?? (moveDownCommand = new RelayCommand<string>(
                           item =>
                           {
                               var level = Convert.ToInt32(item);
                               foreach (var source in PersProperty.Aims.Where(n => n.MinLevelProperty >= level))
                               {
                                   source.MinLevelProperty--;
                                   if (source.MinLevelProperty < 0)
                                   {
                                       source.MinLevelProperty = 0;
                                   }
                               }

                               mapRefresh();
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
        ///     Sets and gets Родительский квест.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim ParrentQwestProperty
        {
            get { return parrentQwest; }

            set
            {
                if (parrentQwest == value)
                {
                    return;
                }

                parrentQwest = value;
                OnPropertyChanged(nameof(ParrentQwestProperty));
            }
        }

        /// <summary>
        ///     Gets or sets Путь к программе Graphviz.
        /// </summary>
        public string PathToGraphvizProperty
        {
            get
            {
                if (PersProperty == null)
                {
                    return string.Empty;
                }

                return PersProperty.PersSettings.PathToGraphviz;
            }

            set
            {
                if (PathToGraphvizProperty == value)
                {
                    return;
                }

                PersProperty.PersSettings.PathToGraphviz = value;
                OnPropertyChanged(nameof(PathToGraphvizProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Персонаж.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Pers PersProperty
        {
            get { return StaticMetods.PersProperty; }
            set
            {
                StaticMetods.PersProperty = value;
                OnPropertyChanged(nameof(Pers));
            }
        }

        /// <summary>
        ///     Sets and gets Квест для которого надо добавить связь.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim QwestForAddChildProperty
        {
            get { return qwestForAddChild; }

            set
            {
                if (qwestForAddChild == value)
                {
                    return;
                }

                qwestForAddChild = value;
                OnPropertyChanged(nameof(QwestForAddChildProperty));
            }
        }

        /// <summary>
        ///     Gets the Настройка в квесте события отжим мышки.
        /// </summary>
        private RelayCommand<Aim> qwestMouseUpCommand;

        /// <summary>
        ///     Gets the Настройка в квесте события отжим мышки.
        /// </summary>
        public RelayCommand<Aim> QwestMouseUpCommand
        {
            get
            {
                return qwestMouseUpCommand
                       ?? (qwestMouseUpCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               var firstAim = DragChildAimProperty ?? item;
                               var secondAim = DragParrentProperty ?? item;

                               StaticMetods.CopyAimSkills(firstAim, secondAim);

                               if (secondAim == firstAim)
                               {
                                   return;
                               }

                               if (Keyboard.Modifiers == ModifierKeys.Control)
                               {
                                   if (secondAim.CompositeAims.Count(n => n.AimProperty == firstAim) == 0)
                                   {
                                       secondAim.CompositeAims.Add(
                                           new CompositeAims {AimProperty = firstAim, KoeficientProperty = 10});
                                   }
                                   else
                                   {
                                       secondAim.CompositeAims.Remove(
                                           secondAim.CompositeAims.First(n => n.AimProperty == firstAim));
                                   }
                               }
                               else
                               {
                                   if (secondAim.Needs.Count(n => n == firstAim) == 0)
                                   {
                                       secondAim.Needs.Add(firstAim);
                                   }
                                   else
                                   {
                                       secondAim.Needs.Remove(firstAim);
                                   }
                               }

                               Messenger.Default.Send("mapUpdate");
                               DragParrentProperty = null;
                               DragChildAimProperty = null;
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
        ///     Sets and gets Граф с квестами.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Graph<Aim> QwestsGraphProperty { get; set; }

        /// <summary>
        ///     Gets the комманда Обновить карту.
        /// </summary>
        public RelayCommand RefreshMapCommand
        {
            get
            {
                return refreshMapCommand
                       ?? (refreshMapCommand =
                           new RelayCommand(
                               () => { Messenger.Default.Send("mapUpdate"); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Комманда Добавить новый квест из карты приключений.
        /// </summary>
        private RelayCommand addNewQwestCommand;

        /// <summary>
        ///     Gets the комманда Добавить новый квест из карты приключений.
        /// </summary>
        public RelayCommand AddNewQwestCommand
        {
            get
            {
                return addNewQwestCommand ?? (addNewQwestCommand = new RelayCommand(
                    () =>
                    {
                        var aim = StaticMetods.AddNewAim(StaticMetods.PersProperty);
                        Messenger.Default.Send("mapUpdate");
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Выбранный квест.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim SelectedQwestProperty
        {
            get { return selectedQwest; }

            set
            {
                if (selectedQwest == value)
                {
                    return;
                }

                selectedQwest = value;
                OnPropertyChanged(nameof(SelectedQwestProperty));
            }
        }

        /// <summary>
        ///     Gets the Удалить выбранный квест.
        /// </summary>
        private RelayCommand<Aim> delAimCommand;

        /// <summary>
        ///     Gets the Удалить выбранный квест.
        /// </summary>
        public RelayCommand<Aim> DelAimCommand
        {
            get
            {
                return delAimCommand ?? (delAimCommand = new RelayCommand<Aim>(
                    item =>
                    {
                        StaticMetods.RemoveQwest(PersProperty, item);
                        mapRefresh();
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

        public bool isHideDone
        {
            get
            {
                return PersProperty.PersSettings.HideDoneQwestInMap;
            }
            set
            {
                if (PersProperty.PersSettings.HideDoneQwestInMap == value)
                {
                    return;
                }
                PersProperty.PersSettings.HideDoneQwestInMap = value;
                OnPropertyChanged(nameof(isHideDone));
                //buildGraph();
                RefreshMapCommand.Execute(null);
            }
        }

        /// <summary>
        ///     Gets the отправить цель.
        /// </summary>
        private RelayCommand<Aim> sendSelAimCommand;

        /// <summary>
        ///     Gets the отправить цель.
        /// </summary>
        public RelayCommand<Aim> SendSelAimCommand
        {
            get
            {
                return sendSelAimCommand
                       ?? (sendSelAimCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               StaticMetods.editAim(item);
                               StaticMetods.RefreshAllQwests(StaticMetods.PersProperty, true, true, true);
                               //Messenger.Default.Send<Effect>(new BlurEffect());
                               //SelectedQwestProperty = item;
                               //StaticMetods.Locator.AimsVM.SelectedAimProperty = item;
                               //var editQwest = new EditQwestWindowView();
                               //var context = StaticMetods.Locator.AimsVM;
                               //context.SelectedAimProperty = SelectedQwestProperty;
                               //FocusManager.SetFocusedElement(editQwest, editQwest.QwestsView.txtName);
                               //editQwest.btnOk.Click += (sender, args) => { editQwest.Close(); };
                               //editQwest.btnCansel.Visibility = Visibility.Collapsed;
                               //editQwest.ShowDialog();
                               //Messenger.Default.Send<Effect>(null);
                               mapRefresh();
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
        ///     Sets and gets Показывать разделение по уровням?.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool ShowLevelsProperty
        {
            get { return showLevels; }

            set
            {
                if (showLevels == value)
                {
                    return;
                }

                showLevels = value;
                OnPropertyChanged(nameof(ShowLevelsProperty));
                PersProperty.PersSettings.DivideByLevelsProperty = ShowLevelsProperty;
            }
        }

        /// <summary>
        ///     Sets and gets Показывать подсказку для связи.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool ShowLinkTooltipProperty
        {
            get
            {
                if (DragParrentProperty == null && DragChildAimProperty == null)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     Gets the комманда Вернуть стандартный фон карты.
        /// </summary>
        public RelayCommand StandartBackgroundCommand
        {
            get
            {
                return standartBackgroundCommand
                       ?? (standartBackgroundCommand = new RelayCommand(
                           () =>
                           {
                               PersProperty.PersSettings.PathToMapBackgroundProperty =
                                   Path.Combine(Directory.GetCurrentDirectory(), "Images", "map.jpg");
                               OnPropertyChanged(nameof(MapBackgroundProperty));
                           },
                           () => { return true; }));
            }
        }

        #endregion Public Properties

        #region Methods

        /// <summary>
        ///     Добавляем разделение по уровням
        /// </summary>
        /// <param name="listQwests">
        ///     список квестов
        /// </param>
        /// <param name="graph">
        ///     граф куда добавляем
        /// </param>
        private void AddLevelsSubgraphs(IEnumerable<Aim> listQwests, Graph<Aim> graph)
        {
            foreach (var qwest in
                listQwests.Cast<Aim>()
                    .Where(n => true)
                    .OrderBy(n => n.MinLevelProperty)
                    .GroupBy(n => n.MinLevelProperty))
            {
                var qwestMinLevel = qwest.First().MinLevelProperty;
                if (PersProperty != null)
                {
                    var levelsSubGraph = new SubGraph<Aim> {Label = qwestMinLevel.ToString()};
                    foreach (var aim in qwest)
                    {
                        levelsSubGraph.AddVertex(aim);
                    }

                    graph.AddSubGraph(levelsSubGraph);
                }
            }

            // Добавляем связи между уровнями
            for (var i = 0; i < graph.SubGraphs.Count() - 1; i++)
            {
                var first = graph.SubGraphs.ElementAt(i);
                var second = graph.SubGraphs.ElementAt(i + 1);

                var edge = new Edge<SubGraph<Aim>>(first, second, new ArrowLevels())
                {
                };

                edge.Attributes.Add("constraint", "true");

                graph.AddEdge(edge);
            }
        }

        /// <summary>
        ///     Добавляем связи между квестами
        /// </summary>
        /// <param name="listQwests">
        ///     список квестов
        /// </param>
        /// <param name="graph">
        ///     граф
        /// </param>
        private void AddQwestslinks(IEnumerable<Aim> listQwests, Graph<Aim> graph)
        {
            foreach (var qwest in listQwests.Cast<Aim>().Where(n => true))
            {
                var needs = qwest.Needs.ToList();

                foreach (var need in needs)
                {
                    Aim vertb = null;
                    Aim verta = null;

                    vertb = graph.AllVertices.FirstOrDefault(n => n.GUID == qwest.GUID);
                    verta = graph.AllVertices.FirstOrDefault(n => n.GUID == need.GUID);

                    if (verta != null && vertb != null)
                    {
                        var edge = new Edge<Aim>(verta, vertb, new Arrow()) {Label = "+"};
                        if (LinkWithQwestLevelsProperty == false)
                        {
                            if (vertb.MinLevelProperty == verta.MinLevelProperty)
                            {
                                graph.AddEdge(edge);
                            }
                        }
                        else
                        {
                            graph.AddEdge(edge);
                        }
                    }
                }

                var inAbils = (from abilitiModel in qwest.InAbills(PersProperty)
                               let lev = abilitiModel.NeedAims.First(n => n.AimProperty == qwest).LevelProperty
                               from na in abilitiModel.NeedAims
                               where na.LevelProperty < lev
                               select na.AimProperty).Union(qwest.NeedAbilities.SelectMany(n => n.AbilProperty.NeedAims.Select(q => q.AimProperty))).ToList();

                foreach (var need in inAbils)
                {
                    Aim vertb = null;
                    Aim verta = null;

                    vertb = graph.AllVertices.FirstOrDefault(n => n.GUID == qwest.GUID);
                    verta = graph.AllVertices.FirstOrDefault(n => n.GUID == need.GUID);

                    if (verta != null && vertb != null)
                    {
                        var edge = new Edge<Aim>(verta, vertb, new ArrowLevels());
                        graph.AddEdge(edge);
                    }
                }


                foreach (var need in qwest.CompositeAims)
                {
                    Aim vertb = null;
                    Aim verta = null;

                    vertb = graph.AllVertices.FirstOrDefault(n => n.GUID == qwest.GUID);
                    verta = graph.AllVertices.FirstOrDefault(n => n.GUID == need.AimProperty.GUID);

                    if (verta != null && vertb != null)
                    {
                        var edge = new Edge<Aim>(verta, vertb, new CompositeArrow());

                        if (LinkWithQwestLevelsProperty == false)
                        {
                            if (vertb.MinLevelProperty != verta.MinLevelProperty)
                            {
                                graph.AddEdge(edge);
                            }
                        }
                        else
                        {
                            graph.AddEdge(edge);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Добавляем квесты без уровней
        /// </summary>
        /// <param name="listQwests">
        ///     список квестов
        /// </param>
        /// <param name="graph">
        ///     граф куда добовляем
        /// </param>
        private void addQwestsWithoutLevels(IEnumerable<Aim> listQwests, Graph<Aim> graph)
        {
            if (PersProperty != null)
            {
                var levelsSubGraph = new SubGraph<Aim> {Label = string.Empty};
                foreach (var aim in listQwests)
                {
                    levelsSubGraph.AddVertex(aim);
                }

                graph.AddSubGraph(levelsSubGraph);
            }
        }

        /// <summary>
        ///     Строим граф - крату приключений!
        /// </summary>
        /// <param name="listQwests">
        ///     список квестов
        /// </param>
        private void buildGraph()
        {
            var listQwests = PersProperty.Aims.ToList();

            if (isHideDone)
            {
                listQwests = listQwests.Where(n => !n.IsDoneProperty).ToList();
            }
            
            //.Where(
            //    n =>
            //    {
            //        if (PersProperty.isCloseCompleteAims)
            //        {
            //            if (n.IsDoneProperty)
            //            {
            //                return false;
            //            }
            //        }

            //        if (PersProperty.isCloseNotNowAims)
            //        {
            //            if (n.StatusProperty == "2. Недоступно")
            //            {
            //                return false;
            //            }
            //        }

            //        if (PersProperty.closeMoreLev)
            //        {
            //            if (n.MinLevelProperty > PersProperty.PersLevelProperty)
            //            {
            //                return false;
            //            }
            //        }

            //        return true;
            //    }).ToList();

            ChildQwestProperty = null;
            ParrentQwestProperty = null;

            var graph = new Graph<Aim> {Rankdir = RankDirection.BottomToTop };

            if (ShowLevelsProperty)
            {
                AddLevelsSubgraphs(listQwests, graph);
            }
            else
            {
                addQwestsWithoutLevels(listQwests, graph);
            }

            AddQwestslinks(listQwests, graph);

            QwestsGraphProperty = graph;
        }

        /// <summary>
        ///     Обновление карты квестов
        /// </summary>
        private void mapRefresh()
        {
            Messenger.Default.Send("mapUpdate");
        }

        /// <summary>
        ///     Обновляем карту!
        /// </summary>
        private void updateMap()
        {
            Messenger.Default.Send("mapUpdate");
        }

        #endregion Methods
    }

    /// <summary>
    ///     The add link messege.
    /// </summary>
    public class AddLinkMessege
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddLinkMessege" /> class.
        /// </summary>
        /// <param name="parrentGuid">
        ///     Ид родителя
        /// </param>
        /// <param name="childGuid">
        ///     Ид дочернего
        /// </param>
        public AddLinkMessege(string parrentGuid, string childGuid, bool composite = false)
        {
            ParrentGuid = parrentGuid;
            ChildGuid = childGuid;
            isComposite = composite;
        }

        public bool isComposite { get; set; }

        #endregion Constructors and Destructors

        #region Public Properties

        /// <summary>
        ///     Ид дочернего
        /// </summary>
        public string ChildGuid { get; set; }

        /// <summary>
        ///     Ид родителя
        /// </summary>
        public string ParrentGuid { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    ///     The arrow levels.
    /// </summary>
    public class ArrowLevels
    {
    }
}
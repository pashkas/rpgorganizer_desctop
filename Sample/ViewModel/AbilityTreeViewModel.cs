using System.Collections.Generic;
using System.Collections.ObjectModel;
using Graphviz4Net.Graphs;
using Sample.Annotations;
using Sample.Model;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.View;

namespace Sample.ViewModel
{
    public class AbilityTreeViewModel : INotifyPropertyChanged
    {
        private AbilitiModel _childAbil;
        private Graph<AbilitiModel> _graphProperty;
        private AbilitiModel _parrentAbil;

        /// <summary>
        ///     Родительский скилл для связи
        /// </summary>
        public AbilitiModel ParrentAbil
        {
            get { return _parrentAbil; }
            set
            {
                if (Equals(value, _parrentAbil)) return;
                _parrentAbil = value;
                OnPropertyChanged(nameof(ParrentAbil));
            }
        }

        /// <summary>
        ///     Sets and gets Граф с квестами.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Graph<AbilitiModel> GraphProperty
        {
            get { return _graphProperty; }
            set
            {
                if (Equals(value, _graphProperty)) return;
                _graphProperty = value;
                OnPropertyChanged(nameof(GraphProperty));
            }
        }


        /// <summary>
        ///     Gets the копировать квест как родительский (для связи).
        /// </summary>
        private RelayCommand<AbilitiModel> copyToParrentCommand;

        /// <summary>
        ///     Gets the копировать квест как родительский (для связи).
        /// </summary>
        public RelayCommand<AbilitiModel> CopyToParrentCommand
        {
            get
            {
                return copyToParrentCommand
                       ?? (copyToParrentCommand = new RelayCommand<AbilitiModel>(
                           item =>
                           {
                               ParrentAbil = item;
                               ChildAbil = null;
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
        ///     Gets the отправить цель.
        /// </summary>
        private RelayCommand<AbilitiModel> editAbil;

        /// <summary>
        ///     Gets the отправить цель.
        /// </summary>
        public RelayCommand<AbilitiModel> EditAbilCommand
        {
            get
            {
                return editAbil
                       ?? (editAbil = new RelayCommand<AbilitiModel>(
                           item =>
                           {
                               item.EditAbility();
                               BuildGraph();
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
        ///     Gets the Копировать скилл как дочерний для связи.
        /// </summary>
        private RelayCommand<AbilitiModel> copyToChildCommand;

        /// <summary>
        ///     Gets the Копировать скилл как дочерний для связи.
        /// </summary>
        public RelayCommand<AbilitiModel> CopyToChildCommand
        {
            get
            {
                return copyToChildCommand
                       ?? (copyToChildCommand = new RelayCommand<AbilitiModel>(
                           item =>
                           {
                               ChildAbil = item;
                               ParrentAbil = null;
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
        ///     Gets the Настройка в квесте события отжим мышки.
        /// </summary>
        private RelayCommand<AbilitiModel> mouseUpCommand;

        private List<AbilitiModel> _abs;
        private Characteristic _selCha;

        /// <summary>
        ///     Gets the Настройка в квесте события отжим мышки.
        /// </summary>
        public RelayCommand<AbilitiModel> MouseUpCommand
        {
            get
            {
                return mouseUpCommand
                       ?? (mouseUpCommand = new RelayCommand<AbilitiModel>(
                           item =>
                           {
                               var firstAbil = ChildAbil ?? item;
                               var secondAbil = ParrentAbil ?? item;

                               if (secondAbil == firstAbil)
                               {
                                   return;
                               }

                               if (secondAbil.NeedAbilities.Count(n => n.AbilProperty == firstAbil) == 0)
                               {
                                   secondAbil.NeedAbilities.Add(
                                       new NeedAbility() { AbilProperty = firstAbil, TypeNeedProperty = ">=", ValueProperty = AbilitiModel.AbMaxLevel });
                               }
                               else
                               {
                                   secondAbil.NeedAbilities.Remove(
                                       secondAbil.NeedAbilities.First(n => n.AbilProperty == firstAbil));
                               }

                               BuildGraph();
                               ParrentAbil = null;
                               ChildAbil = null;
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
        ///     Дочерний скилл для связи
        /// </summary>
        public AbilitiModel ChildAbil
        {
            get { return _childAbil; }
            set
            {
                if (Equals(value, _childAbil)) return;
                _childAbil = value;
                OnPropertyChanged(nameof(ChildAbil));
            }
        }



        /// <summary>
        ///     The pers.
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

        public event PropertyChangedEventHandler PropertyChanged;

        public List<AbilitiModel> Abs
        {
            get => _abs ?? (PersProperty.Abilitis.ToList());
            set
            {
                if (Equals(value, _abs)) return;
                _abs = value;
                OnPropertyChanged(nameof(Abs));
            }
        }

        public Characteristic SelCha
        {
            get => _selCha;
            set
            {
                if (Equals(value, _selCha)) return;
                _selCha = value;
                OnPropertyChanged(nameof(SelCha));
            }
        }

        /// <summary>
        /// Построить дерево скиллов
        /// </summary>
        public void BuildGraph()
        {
            if (PersProperty == null)
            {
                return;
            }
            Abs = SelCha?.NeedAbilitisProperty
                      .Where(q => q.KoeficientProperty > 0)
                      .Select(n => n.AbilProperty).ToList() ?? PersProperty.Abilitis.ToList();
            var abilities = Abs;
            ChildAbil = null;
            ParrentAbil = null;
            var graph = new Graph<AbilitiModel> { Rankdir = RankDirection.BottomToTop };
            GetVertexes(abilities, graph);
            GetLinks(abilities, graph);
            GraphProperty = graph;
        }

        /// <summary>
        /// Строим связи между скиллами
        /// </summary>
        /// <param name="abilities"></param>
        /// <param name="graph"></param>
        private void GetLinks(List<AbilitiModel> abilities, Graph<AbilitiModel> graph)
        {
            foreach (var abilitiModel in abilities)
            {
                foreach (var reqwirement in abilitiModel.NeedAbilities)
                {
                    AbilitiModel vertb = null;
                    AbilitiModel verta = null;

                    vertb = graph.AllVertices.FirstOrDefault(n => n.GUID == abilitiModel.GUID);
                    verta = graph.AllVertices.FirstOrDefault(n => n.GUID == reqwirement.AbilProperty.GUID);

                    if (verta == null || vertb == null) continue;

                    var val = (int)reqwirement.ValueProperty;
                    var characteristicRangs = StaticMetods.PersProperty.PersSettings.AbRangs.ToList();

                    string label = $"{characteristicRangs[val].Name}";

                    if (reqwirement.AbilProperty.CellValue >= val)
                    {
                        label = "";
                    }

                    var edge = new Edge<AbilitiModel>(verta, vertb, new Arrow()) { Label = label };
                    graph.AddEdge(edge);
                }
            }
        }

        /// <summary>
        /// Добавляем все скиллы в дерево
        /// </summary>
        /// <param name="abilities"></param>
        /// <param name="graph"></param>
        private static void GetVertexes(List<AbilitiModel> abilities, Graph<AbilitiModel> graph)
        {

            foreach (var abilitiModel in abilities.OrderBy(n => n.NameOfProperty).ToList())
            {
                graph.AddVertex(abilitiModel);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
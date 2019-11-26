using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sample.Annotations;
using Sample.ViewModel;

namespace Sample.Model
{
    [Serializable]
    public class GroupedComlexNeed:INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lev">Уровень группы</param>
        /// <param name="nameOfNeedsGroup">Название группы для требований</param>
        /// <param name="complecsNeeds">Комплексные требования</param>
        public GroupedComlexNeed(int lev, string nameOfNeedsGroup, List<ComplecsNeed> complecsNeeds)
        {
            _groupLevel = lev;
            _nameOfNeedsGroup = nameOfNeedsGroup;
            ComplecsNeeds = complecsNeeds ?? new List<ComplecsNeed>();
        }


        /// <summary>
        /// Уровень группы
        /// </summary>
        private int _groupLevel;

        /// <summary>
        /// Уровень группы
        /// </summary>
        public int GroupLevel
        {
            get { return _groupLevel; }
            set
            {
                if (_groupLevel == value)
                {
                    return;
                }

                _groupLevel = value;
                OnPropertyChanged(nameof(GroupLevel));
            }
        }

        /// <summary>
        /// Название группы комплексных требований
        /// </summary>
        private string _nameOfNeedsGroup;

        /// <summary>
        /// Название группы комплексных требований
        /// </summary>
        public string NameOfNeedsGroup
        {
            get { return _nameOfNeedsGroup; }
            set
            {
                if (_nameOfNeedsGroup == value)
                {
                    return;
                }

                _nameOfNeedsGroup = value;
                OnPropertyChanged(nameof(NameOfNeedsGroup));
            }
        }

        /// <summary>
        /// Список требований в групированном списке требований
        /// </summary>
        public List<ComplecsNeed> ComplecsNeeds { get; set; } 

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
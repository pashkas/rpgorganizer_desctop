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
        /// <param name="lev">������� ������</param>
        /// <param name="nameOfNeedsGroup">�������� ������ ��� ����������</param>
        /// <param name="complecsNeeds">����������� ����������</param>
        public GroupedComlexNeed(int lev, string nameOfNeedsGroup, List<ComplecsNeed> complecsNeeds)
        {
            _groupLevel = lev;
            _nameOfNeedsGroup = nameOfNeedsGroup;
            ComplecsNeeds = complecsNeeds ?? new List<ComplecsNeed>();
        }


        /// <summary>
        /// ������� ������
        /// </summary>
        private int _groupLevel;

        /// <summary>
        /// ������� ������
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
        /// �������� ������ ����������� ����������
        /// </summary>
        private string _nameOfNeedsGroup;

        /// <summary>
        /// �������� ������ ����������� ����������
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
        /// ������ ���������� � ������������� ������ ����������
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
namespace Sample.Model
{
    using System.ComponentModel;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;

    /// <summary>
    /// ������� �� ��� ������ ������
    /// </summary>
    public class TaskRelaysItem : INotifyPropertyChanged
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        private string desc;

        /// <summary>
        /// ���������� �������.
        /// </summary>
        private bool isLevelVisible;

        /// <summary>
        /// �������.
        /// </summary>
        private int level;

        /// <summary>
        /// �������� �����.
        /// </summary>
        private string rangName;

        /// <summary>
        /// �������� ��������.
        /// </summary>
        private double val;

        /// <summary>
        /// ������������ �������� ��������.
        /// </summary>
        private int valMax;

        /// <summary>
        /// ����������� �������� ��������.
        /// </summary>
        private int valMin;

        /// <summary>
        /// Sets and gets �������� �����.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string RangNameProperty
        {
            get
            {
                return rangName;
            }

            set
            {
                if (rangName == value)
                {
                    return;
                }

                rangName = value;
                OnPropertyChanged(nameof(RangNameProperty));
            }
        }

        /// <summary>
        /// Sets and gets ���������� �������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsLevelVisibleProperty
        {
            get
            {
                return isLevelVisible;
            }

            set
            {
                if (isLevelVisible == value)
                {
                    return;
                }

                isLevelVisible = value;
                OnPropertyChanged(nameof(IsLevelVisibleProperty));
            }
        }

        /// <summary>
        /// Sets and gets �������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LevelProperty
        {
            get
            {
                return level;
            }

            set
            {
                if (level == value)
                {
                    return;
                }

                level = value;
                OnPropertyChanged(nameof(LevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets ����������� �������� ��������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ValMinProperty
        {
            get
            {
                return valMin;
            }

            set
            {
                if (valMin == value)
                {
                    return;
                }

                valMin = value;
                OnPropertyChanged(nameof(ValMinProperty));
            }
        }

        /// <summary>
        /// Sets and gets ������������ �������� ��������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ValMaxProperty
        {
            get
            {
                return valMax;
            }

            set
            {
                if (valMax == value)
                {
                    return;
                }

                valMax = value;
                OnPropertyChanged(nameof(ValMaxProperty));
            }
        }

        /// <summary>
        /// Sets and gets �������� ��������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ValProperty
        {
            get
            {
                return val;
            }

            set
            {
                if (val == value)
                {
                    return;
                }

                val = value;
                OnPropertyChanged(nameof(ValProperty));
            }
        }

        /// <summary>
        /// Sets and gets �������� ��������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DescProperty
        {
            get
            {
                return desc;
            }

            set
            {
                if (desc == value)
                {
                    return;
                }

                desc = value;
                OnPropertyChanged(nameof(DescProperty));
            }
        }

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
        /// �� ��������.
        /// </summary>
        private string guid;

        /// <summary>
        /// ��� ����, �� ��� ������ ������.
        /// </summary>
        private string name;

        /// <summary>
        /// ��� (��������������, ����� ��� �����).
        /// </summary>
        private string type;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets �� ��������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string GuidProperty
        {
            get
            {
                return this.guid;
            }

            set
            {
                if (this.guid == value)
                {
                    return;
                }

                this.guid = value;
                OnPropertyChanged(nameof(GuidProperty));
            }
        }

        /// <summary>
        /// Sets and gets ��� ����, �� ��� ������ ������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameProperty
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                OnPropertyChanged(nameof(NameProperty));
            }
        }

        /// <summary>
        /// �����������.
        /// </summary>
        private byte[] image;

        /// <summary>
        /// Sets and gets �����������.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte[] ImageProperty
        {
            get
            {
                return this.image;
            }

            set
            {
                if (this.image == value)
                {
                    return;
                }

                this.image = value;

                this.OnPropertyChanged(nameof(ImageProperty));
            }
        }

        /// <summary>
        /// Sets and gets ��� (��������������, ����� ��� �����).
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TypeProperty
        {
            get
            {
                return this.type;
            }

            set
            {
                if (this.type == value)
                {
                    return;
                }

                this.type = value;
                OnPropertyChanged(nameof(TypeProperty));
            }
        }

        public string Tag { get; set; }

        #endregion
    }
}
using System.IO;
using System.Linq;

namespace Sample.Model
{
    using GalaSoft.MvvmLight.Messaging;
    using Sample.Annotations;
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// ���������
    /// </summary>
    [Serializable]
    public class SubTask : INotifyPropertyChanged
    {
        public SubTask()
        {
            Guid = System.Guid.NewGuid().ToString();
            PathToIm = StaticMetods.PersProperty.RIG2.GetNextImage(); //Task.SetPathToMonster(Task.LevelForMonsters());
        }

        /// <summary>
        /// ��
        /// </summary>
        public string Guid
        {
            get
            {
                if (string.IsNullOrEmpty(_guid))
                {
                    _guid = System.Guid.NewGuid().ToString();
                }

                return _guid;
            }
            set
            {
                _guid = value;
            }
        }


        /// <summary>
        /// ���� � ��������. ���. �� ������ ������.
        /// </summary>
        public string PathToIm
        {
            get
            {
                string enamiesDirectory = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Enamies"));
                string path = Path.Combine(enamiesDirectory, _pathToIm);

                if (File.Exists(Path.Combine(_pathToIm, path)) == false)
                    PathToIm = StaticMetods.PersProperty.RIG2.GetNextImage();

                return Path.Combine(enamiesDirectory, _pathToIm);
            }
            set
            {
                if (value == _pathToIm) return;
                _pathToIm = value;
                OnPropertyChanged(nameof(PathToIm));
            }
        }

        private string _pathToIm;


        /// <summary>
        /// ��������� �������?
        /// </summary>
        public bool isDone
        {
            get
            {
                return _isDone;
            }
            set
            {
                if (value == _isDone) return;
                _isDone = value;
                OnPropertyChanged(nameof(isDone));
            }
        }

        /// <summary>
        /// Sets and gets ������. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string LinkProperty
        {
            get
            {
                return link;
            }

            set
            {
                if (link == value)
                {
                    return;
                }

                link = value;
                OnPropertyChanged(nameof(LinkProperty));
            }
        }

        /// <summary>
        /// Sets and gets �� ��������� � �������. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool NotRepiatWithTaskProperty
        {
            get
            {
                return notRepiatWithTask;
            }

            set
            {
                if (notRepiatWithTask == value)
                {
                    return;
                }

                notRepiatWithTask = value;
                OnPropertyChanged(nameof(NotRepiatWithTaskProperty));
            }
        }

        /// <summary>
        /// Sets and gets ����������� ����� �� ������ � �������. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public int TimeIsProperty
        {
            get
            {
                return this.timeIs;
            }

            set
            {
                if (this.timeIs == value)
                {
                    return;
                }

                this.timeIs = value;
                this.OnPropertyChanged(nameof(TimeIsProperty));
            }
        }

        /// <summary>
        /// Sets and gets ��������������� ����� �� ���������� ������ � �������. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public int TimeMustProperty
        {
            get
            {
                return this.timeMust;
            }

            set
            {
                if (this.timeMust == value)
                {
                    return;
                }

                this.timeMust = value;
                this.OnPropertyChanged(nameof(TimeMustProperty));
            }
        }

        /// <summary>
        /// Sets and gets ������ �������?. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Visibility TimerActiveProperty
        {
            get
            {
                return this.timerActive;
            }

            set
            {
                if (this.timerActive == value)
                {
                    return;
                }

                this.timerActive = value;
                this.OnPropertyChanged(nameof(TimerActiveProperty));
            }
        }

        /// <summary>
        /// �������� ���������
        /// </summary>
        public string Tittle { get; set; }

        /// <summary>
        /// ����� �� �������
        /// </summary>
        public void TimerPause()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
            }

            this.TimerActiveProperty = Visibility.Collapsed;
        }

        /// <summary>
        /// ������ �������
        /// </summary>
        public void TimerStart()
        {
            if (this.timer == null)
            {
                this.timer = new DispatcherTimer() { Interval = StaticMetods.timeSpan };
                this.timer.Tick += (sender, e) =>
                {
                    this.TimeIsProperty++;
                    Messenger.Default.Send<string>("������ ������!");
                };
            }

            this.timer.Start();
            this.TimerActiveProperty = Visibility.Visible;
        }

        /// <summary>
        /// ��������� �������
        /// </summary>
        public void TimerStop()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
            }

            this.TimeIsProperty = 0;
            this.TimerActiveProperty = Visibility.Collapsed;
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #region Methods

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

        #endregion Methods

        #region Fields

        /// <summary>
        /// ����������� ����� �� ������ � �������.
        /// </summary>
        private int timeIs;

        /// <summary>
        /// ��������������� ����� �� ���������� ������ � �������.
        /// </summary>
        private int timeMust;

        /// <summary>
        /// ������
        /// </summary>
        [NonSerialized]
        private DispatcherTimer timer = new DispatcherTimer() { Interval = StaticMetods.timeSpan };

        /// <summary>
        /// ������ �������?.
        /// </summary>
        private Visibility timerActive = Visibility.Collapsed;

        #endregion Fields

        /// <summary>
        /// ������.
        /// </summary>
        private string link;

        /// <summary>
        /// �� ��������� � �������.
        /// </summary>
        private bool notRepiatWithTask;

        private string _guid;
        private bool _isDone;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample.View
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.ViewModel;

    /// <summary>
    /// Interaction logic for ucSubtasks.xaml
    /// </summary>
    public partial class ucSubtasks : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register(
            "Task",
            typeof(Task),
            typeof(ucSubtasks),
            new PropertyMetadata(default(Task)));

        public ucSubtasks()
        {
            InitializeComponent();
            this.ItemsControll.DataContext = this;
        }

        public Task Task
        {
            get
            {
                return (Task)GetValue(TaskProperty);
            }
            set
            {
                SetValue(TaskProperty, value);
                OnPropertyChanged(nameof(SubTasks));
            }
        }

        /// <summary>
        /// Подзадачи
        /// </summary>
        public IEnumerable<SubTask> SubTasks
        {
            get
            {
                if (Task == null)
                {
                    return null;
                }

                return Task.SubTasks.Where(n => n.isDone == false);
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

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
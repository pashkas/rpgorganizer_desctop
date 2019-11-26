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
    using Sample.Model;

    /// <summary>
    /// Interaction logic for TaskItemInfoUserControll.xaml
    /// </summary>
    public partial class TaskItemInfoUserControll : UserControl
    {
        /// <summary>
        /// Родительский дата контекст - к которому мы обращаемся для того, чтобы вызывать команды
        /// </summary>
        public static readonly DependencyProperty UcParContextProperty = DependencyProperty.Register(
            "UcParContext",
            typeof(object),
            typeof(TaskItemInfoUserControll),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register(
            "Task",
            typeof(Task),
            typeof(TaskItemInfoUserControll),
            new PropertyMetadata(default(Task)));

        public static readonly DependencyProperty ImagVisibilityProperty = DependencyProperty.Register(
            "ImagVisibility",
            typeof(Visibility),
            typeof(TaskItemInfoUserControll),
            new PropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty TaskTypeVisibilityProperty =
            DependencyProperty.Register(
                "TaskTypeVisibility",
                typeof(Visibility),
                typeof(TaskItemInfoUserControll),
                new PropertyMetadata(default(Visibility)));

        public TaskItemInfoUserControll()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Родительский дата контекст к которому мы обращаемся для того, чтобы вызывать команды
        /// </summary>
        public object UcParContext
        {
            get
            {
                return (object)GetValue(UcParContextProperty);
            }
            set
            {
                SetValue(UcParContextProperty, value);
            }
        }

        public Visibility TaskTypeVisibility
        {
            get
            {
                return (Visibility)GetValue(TaskTypeVisibilityProperty);
            }
            set
            {
                SetValue(TaskTypeVisibilityProperty, value);
            }
        }

        public Visibility ImagVisibility
        {
            get
            {
                return (Visibility)GetValue(ImagVisibilityProperty);
            }
            set
            {
                SetValue(ImagVisibilityProperty, value);
            }
        }

        public Visibility relaysVisibility { get; set; }

        public Task Task
        {
            get
            {
                return (Task)GetValue(TaskProperty);
            }
            set
            {
                SetValue(TaskProperty, value);
            }
        }
    }
}
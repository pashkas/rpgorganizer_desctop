using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using Sample.Model;
using Sample.View;
using Sample.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Sample
{
    /// <summary>
    /// Interaction logic for TasksViewUC.xaml
    /// </summary>
    public partial class TasksViewUC : UserControl
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items",
            typeof(object),
            typeof(TasksViewUC),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty colNumsProperty = DependencyProperty.Register(
            "colNums",
            typeof(int),
            typeof(TasksViewUC),
            new PropertyMetadata(default(int)));

        public TasksViewUC()
        {
            this.InitializeComponent();
        }


        public static readonly DependencyProperty PipBoyVisibilityProperty = DependencyProperty.Register(
            "PipBoyVisibility", typeof (Visibility), typeof (TasksViewUC), new PropertyMetadata(default(Visibility)));

        public Visibility PipBoyVisibility
        {
            get { return (Visibility) GetValue(PipBoyVisibilityProperty); }
            set { SetValue(PipBoyVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ShowImageTaskVisibilityProperty = DependencyProperty.Register(
            "ShowImageTaskVisibility", typeof (Visibility), typeof (TasksViewUC), new PropertyMetadata(default(Visibility)));

        public Visibility ShowImageTaskVisibility
        {
            get { return (Visibility) GetValue(ShowImageTaskVisibilityProperty); }
            set { SetValue(ShowImageTaskVisibilityProperty, value); }
        }

        public object Items
        {
            get
            {
                return GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }



        public int colNums
        {
            get
            {
                return (int)GetValue(colNumsProperty);
            }
            set
            {
                SetValue(colNumsProperty, value);
            }
        }

        /// <summary>
        /// Положение
        /// </summary>
        public VerticalAlignment Alignment { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            StaticMetods.Locator.MainVM.RefreshTasksInMainView();
        }

        private void UIElement_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TaskAtackView ta = new TaskAtackView();

            var context = DataContext as IHaveTaskPanel;

            int val = 0;

            var timer = new DispatcherTimer() { Interval = new TimeSpan(0,0,0,0,100) };
            timer.Tick += (o, args) =>
            {
                val++;

                if (val == 2 && e.ButtonState == MouseButtonState.Pressed)
                {
                    ta.txtHeader.Text = context.SellectedTask.NameOfProperty;
                    ta.Show();
                }

                if (ta.barProgress.Value >= 100.0)
                {
                    timer.Stop();
                    ta.Close();

                    context.AlternatePlusTaskCommand.Execute(context.SellectedTask);
                }

                if (e.ButtonState == MouseButtonState.Released)
                {
                   timer.Stop();
                   ta.Close();
                }
            };
            timer.Start();
        }


        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer) sender;
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset-e.Delta);
            e.Handled = true;
        }

        private void Bord_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var context = DataContext as IHaveTaskPanel;
            var sellectedTask = (sender as Border)?.DataContext;
            var task = sellectedTask as Task;
            if (task!=null && context!=null)
            {
                context.SellectedTask = task;
            }
        }

       
    }
}
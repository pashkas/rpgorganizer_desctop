using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using Sample.Model;

namespace Sample.View
{
    /// <summary>
    ///     Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        /*
        /// <summary>
        /// The dc.
        /// </summary>
        private MainViewModel dc;*/

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainView" /> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();

            //MinimizeToTray.Enable(this);

            Messenger.Default.Register<Window>(this, item => { item.Owner = this; });

            Messenger.Default.Register<string>(
                this,
                item =>
                {
                    if (item == "!!!")
                    {
                        //this.UcSubtasksInthridView.Task = ((MainViewModel)this.DataContext).SellectedTask;
                    }
                    if (item == "Минимизировать!")
                    {
                        WindowState = WindowState.Minimized;
                    }
                });

            // this.fullScr.ToolTip = Settings.Default.LikeFullScreen == true ? "Оконный режим" : "Полноэкранный режим";
        }

        #endregion Constructors and Destructors

        private void MainView_OnActivated(object sender, EventArgs e)
        {
            if (StaticMetods.PersProperty.IsFirstUse)
            {
                StaticMetods.PersProperty.IsFirstUse = false;

                //try
                //{
                //    Process.Start(App.InstrPath);
                //}
                //catch
                //{
                //    System.Windows.Forms.MessageBox.Show(@"Опаньки! (((");
                //}

                //Process.Start("http://rpgorganizer.blogspot.ru/");

                //if (messageBoxResult == MessageBoxResult.OK)
                //{
                //    Process.Start(App.InstrPath);
                //}
            }

            
        }

        private void MainView_OnContentRendered(object sender, EventArgs e)
        {
            StaticMetods.Locator.MainVM.SyncToAndroid();
        }

        private void MainView_OnLoaded(object sender, RoutedEventArgs e)
        {
           
        }

        #region Methods

        /// <summary>
        ///     The list tasks_ drop.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void ListTasks_Drop(object sender, DragEventArgs e)
        {
            /*
            Task droppedData = e.Data.GetData(typeof(Task)) as Task;
            Task target = ((ListBoxItem)sender).DataContext as Task;

            int oldIndex = this.dc.Pers.Tasks.IndexOf(droppedData);
            int newIndex = this.dc.Pers.Tasks.IndexOf(target);

            // int targetIdx = ListTasks.Items.IndexOf(target);
            // this.pers.Tasks.Move(oldIndex, targetIdx);
            this.dc.Pers.Tasks.Move(oldIndex, newIndex);*/
        }

        /// <summary>
        ///     The list tasks_ preview mouse left button down.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void ListTasks_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*
            if (sender is ListBoxItem)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }*/
        }

        /// <summary>
        ///     The menu item_ click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Methods
    }
}
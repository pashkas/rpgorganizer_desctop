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

namespace Sample
{
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Interaction logic for MainWindowAllMenu.xaml
    /// </summary>
    public partial class MainWindowAllMenu : UserControl
    {
        public MainWindowAllMenu()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The menu item_ click_1.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>("Минимизировать!");
        }
    }
}
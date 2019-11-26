// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveAbTasksWindow.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for ActiveAbTasksWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
using System.Windows.Shapes;

namespace Sample.View
{
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Model;
    using Sample.Properties;

    /// <summary>
    /// Interaction logic for ActiveAbTasksWindow.xaml
    /// </summary>
    public partial class ActiveAbTasksWindow : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveAbTasksWindow"/> class.
        /// </summary>
        public ActiveAbTasksWindow()
        {
            this.InitializeComponent();
            Messenger.Default.Register<string>(
                this,
                item =>
                {
                    if (item.ToString() == "Закрыть активные задачи навыка!")
                    {
                        this.Close();
                    }
                });
        }

        #endregion

        #region Methods

        /// <summary>
        /// The button base_ on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>("Обновить после карты активных задач!");
            this.Close();
        }

        #endregion
    }
}
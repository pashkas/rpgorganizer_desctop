// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UcTasksSettingsView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Логика взаимодействия для UcTasksSettingsView.xaml
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample.View
{
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Model;

    /// <summary>
    /// Логика взаимодействия для UcTasksSettingsView.xaml
    /// </summary>
    public partial class UcTasksSettingsView : UserControl
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UcTasksSettingsView"/> class.
        /// </summary>
        public UcTasksSettingsView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Делаем выделенным первый видимый таб в контроле
        /// </summary>
        /// <param name="tabControl">
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        private object GetSelectedTab(TabControl tabControl)
        {
            foreach (var item in tabControl.Items)
            {
                Control control = item as Control;
                if (control.Visibility == Visibility.Visible)
                {
                    return control;
                }
            }

            return null;
        }

        /// <summary>
        /// The src abil if done_ on filter.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SrcAbilIfDone_OnFilter(object sender, FilterEventArgs e)
        {
            var abil = (ChangeAbilityModele)e.Item;
            e.Accepted = true;
        }

        /// <summary>
        /// The rel show.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void relShow(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// The usl show.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void uslShow(object sender, RoutedEventArgs e)
        {
        }

        #endregion
    }
}
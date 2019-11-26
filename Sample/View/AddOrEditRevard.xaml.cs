// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrEditRevard.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for AddOrEditRevard.xaml
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

    /// <summary>
    /// Interaction logic for AddOrEditRevard.xaml
    /// </summary>
    public partial class AddOrEditRevard : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditRevard"/> class.
        /// </summary>
        public AddOrEditRevard()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

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
            e.Accepted = abil.AbilityProperty.IsEnebledProperty == true;
        }

        #endregion

        private void uslShow(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            StaticMetods.refreshShopItems(StaticMetods.PersProperty);
        }
    }
}
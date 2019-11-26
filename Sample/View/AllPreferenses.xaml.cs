// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllPreferenses.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for AllPreferenses.xaml
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

    /// <summary>
    /// Interaction logic for AllPreferenses.xaml
    /// </summary>
    public partial class AllPreferenses : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllPreferenses"/> class.
        /// </summary>
        public AllPreferenses()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обновить ранги всех характеристик
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void BtnChaRangsRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>("Обновить ранги характеристик!");
        }

        /// <summary>
        /// Обновить ранги всех скиллов
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void BtnRangsAbRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>("Обновить ранги навыков!");
        }

        #endregion
    }
}
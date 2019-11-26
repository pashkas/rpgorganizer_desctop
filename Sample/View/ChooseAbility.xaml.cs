// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChooseAbility.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for ChooseAbility.xaml
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
    /// Interaction logic for ChooseAbility.xaml
    /// </summary>
    public partial class ChooseAbility : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChooseAbility"/> class.
        /// </summary>
        public ChooseAbility()
        {
            this.InitializeComponent();
            Messenger.Default.Register<string>(
                this,
                item =>
                {
                    if (item == "закрыть импорт!")
                    {
                        this.Close();
                    }
                });
        }

        #endregion

        #region Methods

        /// <summary>
        /// The choose ability_ on closed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ChooseAbility_OnClosed(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The wizard_ on cancel.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Wizard_OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
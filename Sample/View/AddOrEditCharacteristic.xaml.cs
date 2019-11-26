// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrEditCharacteristic.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for AddOrEditCharacteristic.xaml
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
    /// Interaction logic for AddOrEditCharacteristic.xaml
    /// </summary>
    public partial class AddOrEditCharacteristic : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditCharacteristic"/> class.
        /// </summary>
        public AddOrEditCharacteristic()
        {
            this.InitializeComponent();
        }

        #endregion

        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenRelayAbilitis(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
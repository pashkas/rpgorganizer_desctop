// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrEditAimView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for AddOrEditAimView.xaml
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
using GalaSoft.MvvmLight.Command;
using Sample.ViewModel;

namespace Sample.View
{
    /// <summary>
    /// Interaction logic for AddOrEditAimView.xaml
    /// </summary>
    public partial class AddOrEditAimView : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditAimView"/> class.
        /// </summary>
        public AddOrEditAimView()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}
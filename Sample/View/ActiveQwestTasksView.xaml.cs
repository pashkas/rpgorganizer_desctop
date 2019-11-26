// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveQwestTasksView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for ActiveQwestTasksView.xaml
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

    /// <summary>
    /// Interaction logic for ActiveQwestTasksView.xaml
    /// </summary>
    public partial class ActiveQwestTasksView : UserControl
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveQwestTasksView"/> class.
        /// </summary>
        public ActiveQwestTasksView()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}
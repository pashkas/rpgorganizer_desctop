// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskMap.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for TaskMap.xaml
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
    using Sample.Properties;

    /// <summary>
    /// Interaction logic for TaskMap.xaml
    /// </summary>
    public partial class TaskMap : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskMap"/> class.
        /// </summary>
        public TaskMap()
        {
            this.InitializeComponent();
        }

        #endregion

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
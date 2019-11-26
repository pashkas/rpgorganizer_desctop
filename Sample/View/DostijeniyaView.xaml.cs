// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DostijeniyaView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Логика взаимодействия для DostijeniyaView.xaml
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
    using System.Windows.Forms;

    using Sample.Properties;

    /// <summary>
    /// Логика взаимодействия для DostijeniyaView.xaml
    /// </summary>
    public partial class DostijeniyaView : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DostijeniyaView"/> class.
        /// </summary>
        public DostijeniyaView()
        {
            this.InitializeComponent();
        }

        #endregion

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
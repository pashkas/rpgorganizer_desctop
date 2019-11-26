// --------------------------------------------------------------------------------------------------------------------
// <copyright file="taskStatisticView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for taskStatisticView.xaml
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

    /// <summary>
    /// Interaction logic for taskStatisticView.xaml
    /// </summary>
    public partial class taskStatisticView : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="taskStatisticView"/> class.
        /// </summary>
        public taskStatisticView()
        {
            this.InitializeComponent();
            this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.7);
            this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.72);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Congranteletions.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for Congranteletions.xaml
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
    /// <summary>
    /// Interaction logic for Congranteletions.xaml
    /// </summary>
    public partial class Congranteletions : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Congranteletions"/> class.
        /// </summary>
        public Congranteletions()
        {
            this.InitializeComponent();
        }


        public static readonly DependencyProperty fromProgressProperty = DependencyProperty.Register(
            "fromProgress", typeof (double), typeof (Congranteletions), new PropertyMetadata(default(double)));

        public double fromProgress
        {
            get { return (double) GetValue(fromProgressProperty); }
            set { SetValue(fromProgressProperty, value); }
        }


        public static readonly DependencyProperty toProgressProperty = DependencyProperty.Register(
            "toProgress", typeof (double), typeof (Congranteletions), new PropertyMetadata(default(double)));

        public double toProgress
        {
            get { return (double) GetValue(toProgressProperty); }
            set { SetValue(toProgressProperty, value); }
        }

        #endregion
    }
}
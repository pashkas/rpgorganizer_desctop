// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucViewChangesView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Логика взаимодействия для ucViewChangesView.xaml
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
    /// <summary>
    /// Логика взаимодействия для ucViewChangesView.xaml
    /// </summary>
    public partial class ucViewChangesView : UserControl
    {

       public static readonly DependencyProperty isLeveableChangesShowProperty =
            DependencyProperty.Register(
                "isLeveableChangesShow",
                typeof(bool),
                typeof(ucViewChangesView),
                new PropertyMetadata(default(bool)));

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ucViewChangesView"/> class.
        /// </summary>
        public ucViewChangesView()
        {
            this.InitializeComponent();
        }

        #endregion


        public bool isLeveableChangesShow
        {
            get
            {
                return (bool)GetValue(isLeveableChangesShowProperty);
            }
            set
            {
                SetValue(isLeveableChangesShowProperty, value);
            }
        }
    }
}
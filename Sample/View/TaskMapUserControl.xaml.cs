// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskMapUserControl.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for TaskMapUserControl.xaml
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
    /// Interaction logic for TaskMapUserControl.xaml
    /// </summary>
    public partial class TaskMapUserControl : UserControl
    {
        #region Constructors and Destructors

        public static readonly DependencyProperty AddTaskVisibilityProperty =
            DependencyProperty.Register(
                "AddTaskVisibility",
                typeof(Visibility),
                typeof(TaskMapUserControl),
                new PropertyMetadata(default(Visibility)));

        public Visibility AddTaskVisibility
        {
            get
            {
                return (Visibility)GetValue(AddTaskVisibilityProperty);
            }
            set
            {
                SetValue(AddTaskVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskMapUserControl"/> class.
        /// </summary>
        public TaskMapUserControl()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AimView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Логика взаимодействия для AimView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample.View
{
    using System.Windows.Forms;

    using Sample.Model;
    using Sample.ViewModel;

    using Binding = System.Windows.Data.Binding;
    using UserControl = System.Windows.Controls.UserControl;

    /// <summary>
    /// Логика взаимодействия для AimView.xaml
    /// </summary>
    public partial class AimView : UserControl
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AimView"/> class.
        /// </summary>
        public AimView()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucQwestMap.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for ucQwestMap.xaml
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

    using Sample.Model;

    /// <summary>
    /// Interaction logic for ucQwestMap.xaml
    /// </summary>
    public partial class ucQwestMap : UserControl
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ucQwestMap"/> class.
        /// </summary>
        public ucQwestMap()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}
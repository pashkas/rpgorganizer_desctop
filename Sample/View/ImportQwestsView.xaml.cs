// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportQwestsView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for ImportQwestsView.xaml
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
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Interaction logic for ImportQwestsView.xaml
    /// </summary>
    public partial class ImportQwestsView : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportQwestsView"/> class.
        /// </summary>
        public ImportQwestsView()
        {
            this.InitializeComponent();
            Messenger.Default.Register<string>(
                this,
                s =>
                {
                    if (s == "Закрыть импорт квестов!")
                    {
                        this.Close();
                    }
                });
        }

        #endregion
    }
}
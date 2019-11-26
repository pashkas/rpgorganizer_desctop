// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportCharacts.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for ImportCharacts.xaml
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
    using System.Windows.Forms.VisualStyles;

    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Interaction logic for ImportCharacts.xaml
    /// </summary>
    public partial class ImportCharacts : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCharacts"/> class.
        /// </summary>
        public ImportCharacts()
        {
            this.InitializeComponent();
            Messenger.Default.Register<string>(
                this,
                item =>
                {
                    if (item == "Закрыть импорт характеристик!")
                    {
                        this.Close();
                    }
                });
        }

        #endregion
    }
}
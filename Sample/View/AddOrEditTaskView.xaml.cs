// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrEditTaskView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for AddOrEditTaskView.xaml
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
    /// Interaction logic for AddOrEditTaskView.xaml
    /// </summary>
    public partial class AddOrEditTaskView : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditTaskView"/> class.
        /// </summary>
        public AddOrEditTaskView()
        {
            this.InitializeComponent();
            FocusManager.SetFocusedElement(this, UcTasksSettingsView.nameTask);

            Messenger.Default.Register<string>(
                this,
                s =>
                {
                    if (s == "!Закрыть окно редактирования задачи")
                    {
                        this.Close();
                    }
                });
        }

        #endregion
    }
}
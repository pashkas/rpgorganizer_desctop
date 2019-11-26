// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QwestsView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Логика взаимодействия для QwestsView.xaml
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

    using GalaSoft.MvvmLight.Messaging;

    using Sample.Model;

    /// <summary>
    /// Логика взаимодействия для QwestsView.xaml
    /// </summary>
    public partial class QwestsView
    {
        public static readonly DependencyProperty VisibleAllPreferensesProperty =
            DependencyProperty.Register(
                "VisibleAllPreferenses",
                typeof(Visibility),
                typeof(QwestsView),
                new PropertyMetadata(default(Visibility)));

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QwestsView"/> class.
        /// </summary>
        public QwestsView()
        {
            this.InitializeComponent();
            FocusManager.SetFocusedElement(this, txtName);
            Messenger.Default.Register<string>(
                this,
                item =>
                {
                    switch (item.ToString())
                    {
                       
                    }
                });
        }

        #endregion

        /// <summary>
        /// Видны все настройки квеста? если это только добавление квеста из побочных окон, то нет
        /// </summary>
        public Visibility VisibleAllPreferenses
        {
            get
            {
                return (Visibility)GetValue(VisibleAllPreferensesProperty);
            }
            set
            {
                SetValue(VisibleAllPreferensesProperty, value);
            }
        }
    }
}
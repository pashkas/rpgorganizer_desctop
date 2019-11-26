// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersSettingsView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Логика взаимодействия для PersSettingsView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sample.ViewModel;

namespace Sample.View
{
    using System.Windows.Forms;
    using System.Windows.Media.Effects;

    using GalaSoft.MvvmLight.Messaging;

    using Sample.Model;
    using Sample.Properties;

    using TabControl = System.Windows.Controls.TabControl;

    /// <summary>
    /// Логика взаимодействия для PersSettingsView.xaml
    /// </summary>
    public partial class PersSettingsView : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersSettingsView"/> class.
        /// </summary>
        public PersSettingsView()
        {
            this.InitializeComponent();

            Messenger.Default.Register<Effect>(this, item => { this.Effect = item; });

            Messenger.Default.Register<Tuple<string, string>>(
                this,
                tuple =>
                {
                    if (tuple.Item1 == "Окно персонажа")
                    {
                        this.TabControl.SelectedItem =
                            new Func<TabControl, string, TabItem>(
                                (control, headerName) =>
                                {
                                    return
                                        control.Items.Cast<TabItem>()
                                            .FirstOrDefault(tabPage => tabPage?.Header?.ToString() == headerName);
                                })(
                                    this.TabControl,
                                    tuple.Item2);
                    }
                });

            Messenger.Default.Register<string>(
                this,
                item =>
                {
                    //switch (item.ToString())
                    //{
                    //    case "Отобразить информацию о квестах!":
                    //        this.QwestTabControll.SelectedItem = this.QwestTabControll.Items[0];
                    //        break;
                    //    case "Требования квеста!!!":
                    //        this.QwestTabControll.SelectedItem = this.QwestTabControll.Items[0];
                    //        //this.QwestsView.TabControl.SelectedItem = this.QwestsView.TabControl.Items[2];
                    //        break;
                    //    case "Активные задачи квеста!":
                    //        this.QwestTabControll.SelectedItem = this.QwestTabControll.Items[1];
                    //        break;
                    //    case "Закрыть настройки персонажа!":
                    //        this.Hide();
                    //        break;
                    //    case "Фокусировка на названии!":
                    //        //FocusManager.SetFocusedElement(this, this.QwestsView.txtName);
                    //        break;
                    //    case "ОткрытьКартуКвестов!":
                    //        this.QwestTabControll.SelectedItem = this.QwestTabControll.Items[1];
                    //        break;
                    //}
                });

            // this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.7);
            // this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.72);
        }

        #endregion

        private void closePSV(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Methods

        /// <summary>
        /// The color picker_ selected color changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
        }

        /// <summary>
        /// The window_ loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion
    }
}
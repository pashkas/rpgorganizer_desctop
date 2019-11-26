// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrEditAbilityView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for AddOrEditAbilityView.xaml
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
using GalaSoft.MvvmLight.Command;
using Sample.ViewModel;

namespace Sample.View
{
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Model;

    /// <summary>
    /// Interaction logic for AddOrEditAbilityView.xaml
    /// </summary>
    public partial class AddOrEditAbilityView : Window
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditAbilityView"/> class.
        /// </summary>
        public AddOrEditAbilityView()
        {
            this.InitializeComponent();

            


            var addCom = new RelayCommand(() =>
            {
                var cont = DataContext as AddOrEditAbilityViewModel;
                cont.SelectedAbilitiModelProperty.AddNeedTaskCommand.Execute("+");
            });

            InputBindings.Add(new InputBinding(addCom, new KeyGesture(Key.Insert)));
        }

        #endregion

        private void close(object sender, RoutedEventArgs e)
        {
            StaticMetods.Locator.MainVM.RefreshTasksInMainView();
            this.Close();
        }

        /// <summary>
        /// Настроить требования для прокачки скилла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenNeeds(object sender, RoutedEventArgs e)
        {
           
        }

        /// <summary>
        /// Настроить условия активности скилла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenUsl(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
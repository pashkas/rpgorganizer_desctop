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
    /// <summary>
    /// Interaction logic for EditQwestWindowView.xaml
    /// </summary>
    public partial class EditQwestWindowView : Window
    {
        public EditQwestWindowView()
        {
            InitializeComponent();


            var addCom = new RelayCommand(() =>
            {
                var cont = QwestsView.DataContext as QwestsViewModel;
                cont.AddNeedTaskCommand.Execute("+");
            });

            InputBindings.Add(new InputBinding(addCom, new KeyGesture(Key.Insert)));

        }
    }
}
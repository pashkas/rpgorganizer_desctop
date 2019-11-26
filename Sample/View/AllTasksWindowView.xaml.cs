using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// <summary>
    /// Логика взаимодействия для AllTasksWindowView.xaml
    /// </summary>
    public partial class AllTasksWindowView : Window
    {
        public AllTasksWindowView()
        {
            InitializeComponent();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

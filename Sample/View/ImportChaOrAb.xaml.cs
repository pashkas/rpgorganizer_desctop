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
using Sample.Model;

namespace Sample.View
{
    /// <summary>
    /// Логика взаимодействия для ImportChaOrAb.xaml
    /// </summary>
    public partial class ImportChaOrAb : Window
    {
        public ImportChaOrAb()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Colview_OnFilter(object sender, FilterEventArgs e)
        {
            var cha = e.Item as Characteristic;
            var ab = e.Item as AbilitiModel;
            if (cha!=null)
            {
                e.Accepted = !cha.IsChecked;
            }
            else if (ab!=null)
            {
                e.Accepted = !ab.IsChecked;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void Colview2_OnFilter(object sender, FilterEventArgs e)
        {
            var cha = e.Item as Characteristic;
            var ab = e.Item as AbilitiModel;
            if (cha != null)
            {
                e.Accepted = cha.IsChecked;
            }
            else if (ab != null)
            {
                e.Accepted = ab.IsChecked;
            }
            else
            {
                e.Accepted = false;
            }
        }
    }
}

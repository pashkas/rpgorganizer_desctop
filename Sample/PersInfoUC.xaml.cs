using System;
using System.Collections.Generic;
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

namespace Sample
{
    using Sample.Model;

    /// <summary>
    /// Interaction logic for PersInfoUC.xaml
    /// </summary>
    public partial class PersInfoUC : UserControl
    {
        public PersInfoUC()
        {
            this.InitializeComponent();
        }

        private void AbilsView_OnFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;
        }

        private void PerksView_OnFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;
        }
    }
}
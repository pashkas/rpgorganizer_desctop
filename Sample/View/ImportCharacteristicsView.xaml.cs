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
    /// <summary>
    /// Interaction logic for ImportCharacteristicsView.xaml
    /// </summary>
    public partial class ImportCharacteristicsView : Window
    {
        public ImportCharacteristicsView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
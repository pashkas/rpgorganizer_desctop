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
    /// Логика взаимодействия для ShortChangesWindow.xaml
    /// </summary>
    public partial class ShortChangesWindow : Window
    {
        public ShortChangesWindow()
        {
            InitializeComponent();
        }

        private void FormFadeOut_Completed(object sender, EventArgs e)
        {
            Close();
        }
    }
}

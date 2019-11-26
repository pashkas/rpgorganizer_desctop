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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample.View
{
    /// <summary>
    /// Логика взаимодействия для ucSetGoldExpRevard.xaml
    /// </summary>
    public partial class ucSetGoldExpRevard : UserControl
    {

        public static readonly DependencyProperty isExpSetProperty = DependencyProperty.Register(
            "isExpSet", typeof (bool), typeof (ucSetGoldExpRevard), new PropertyMetadata(default(bool)));

        public bool isExpSet
        {
            get { return (bool) GetValue(isExpSetProperty); }
            set { SetValue(isExpSetProperty, value); }
        }

        public ucSetGoldExpRevard()
        {
            InitializeComponent();
        }
    }
}

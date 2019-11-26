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
    /// <summary>
    /// Interaction logic for FirstTasksView.xaml
    /// </summary>
    public partial class FirstTasksView : UserControl
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items",
            typeof(object),
            typeof(FirstTasksView),
            new PropertyMetadata(default(object)));

        public FirstTasksView()
        {
            this.InitializeComponent();
        }

        public object Items
        {
            get
            {
                return GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }
    }
}
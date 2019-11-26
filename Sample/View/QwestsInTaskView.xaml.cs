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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample.View
{
    using Sample.Model;

    /// <summary>
    /// Interaction logic for QwestsInTaskView.xaml
    /// </summary>
    public partial class QwestsInTaskView : UserControl
    {
        public static readonly DependencyProperty colNumQwestsProperty = DependencyProperty.Register(
            "colNumQwests",
            typeof(int),
            typeof(QwestsInTaskView),
            new PropertyMetadata(default(int)));

        public static readonly DependencyProperty QwestsListProperty = DependencyProperty.Register(
            "QwestsList",
            typeof(List<Aim>),
            typeof(QwestsInTaskView),
            new PropertyMetadata(default(List<Aim>)));

        public QwestsInTaskView()
        {
            InitializeComponent();
        }

        public int colNumQwests
        {
            get
            {
                return (int)GetValue(colNumQwestsProperty);
            }
            set
            {
                SetValue(colNumQwestsProperty, value);
            }
        }

        public List<Aim> QwestsList
        {
            get
            {
                return (List<Aim>)GetValue(QwestsListProperty);
            }
            set
            {
                SetValue(QwestsListProperty, value);
            }
        }
    }
}
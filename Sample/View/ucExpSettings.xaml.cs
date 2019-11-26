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
    /// <summary>
    /// Interaction logic for ucExpSettings.xaml
    /// </summary>
    public partial class ucExpSettings : UserControl
    {
        public static readonly DependencyProperty ExpProperty = DependencyProperty.Register(
            "Exp",
            typeof(int),
            typeof(ucExpSettings),
            new PropertyMetadata(default(int)));

        public static readonly DependencyProperty ImportanceProperty = DependencyProperty.Register(
            "Importance",
            typeof(double),
            typeof(ucExpSettings),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty UrgencyProperty = DependencyProperty.Register(
            "Urgency",
            typeof(double),
            typeof(ucExpSettings),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty HardnessProperty = DependencyProperty.Register(
            "Hardness",
            typeof(double),
            typeof(ucExpSettings),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ExpTextProperty = DependencyProperty.Register(
            "ExpText",
            typeof(string),
            typeof(ucExpSettings),
            new PropertyMetadata(default(string)));

        public ucExpSettings()
        {
            InitializeComponent();
        }

        public int Exp
        {
            get
            {
                return (int)GetValue(ExpProperty);
            }
            set
            {
                SetValue(ExpProperty, value);
            }
        }

        public double Importance
        {
            get
            {
                return (double)GetValue(ImportanceProperty);
            }
            set
            {
                SetValue(ImportanceProperty, value);
            }
        }

        public double Urgency
        {
            get
            {
                return (double)GetValue(UrgencyProperty);
            }
            set
            {
                SetValue(UrgencyProperty, value);
            }
        }

        public double Hardness
        {
            get
            {
                return (double)GetValue(HardnessProperty);
            }
            set
            {
                SetValue(HardnessProperty, value);
            }
        }

        public string ExpText
        {
            get
            {
                return (string)GetValue(ExpTextProperty);
            }
            set
            {
                SetValue(ExpTextProperty, value);
            }
        }
    }
}
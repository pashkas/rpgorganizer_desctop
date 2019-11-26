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
    /// Логика взаимодействия для ucProgressBarWithScale.xaml
    /// </summary>
    public partial class ucProgressBarWithScale : UserControl
    {
        public ucProgressBarWithScale()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (ucProgressBarWithScale), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof (double), typeof (ucProgressBarWithScale), new PropertyMetadata(default(double)));

        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof (double), typeof (ucProgressBarWithScale), new PropertyMetadata(default(double)));

        public double Minimum
        {
            get { return (double) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof (double), typeof (ucProgressBarWithScale), new PropertyMetadata(default(double)));

        public double Maximum
        {
            get { return (double) GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty IsShowScaleProperty = DependencyProperty.Register(
            "IsShowScale", typeof (Visibility), typeof (ucProgressBarWithScale), new PropertyMetadata(default(Visibility)));

        public Visibility IsShowScale
        {
            get { return (Visibility) GetValue(IsShowScaleProperty); }
            set { SetValue(IsShowScaleProperty, value); }
        }
    }
}

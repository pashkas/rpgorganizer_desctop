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
using Sample.ViewModel;

namespace Sample.View
{
    /// <summary>
    /// Interaction logic for ucKRelay.xaml
    /// </summary>
    public partial class ucKRelay : UserControl
    {
        public static readonly DependencyProperty KRelayProperty = DependencyProperty.Register(
            "KRelay",
            typeof(double),
            typeof(ucKRelay),
            new PropertyMetadata(default(double)));

        public ucKRelay()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NeedKsProperty = DependencyProperty.Register(
            "NeedKs", typeof (List<NeedK>), typeof (ucKRelay), new PropertyMetadata(default(List<NeedK>)));

        public List<NeedK> NeedKs
        {
            get { return (List<NeedK>) GetValue(NeedKsProperty); }
            set { SetValue(NeedKsProperty, value); }
        }

        

        /// <summary>
        /// Коэффициент влияния
        /// </summary>
        public double KRelay
        {
            get
            {
                return (double)GetValue(KRelayProperty);
            }
            set
            {
                SetValue(KRelayProperty, value);
            }
        }
    }

   
}
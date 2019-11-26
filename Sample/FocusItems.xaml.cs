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
    /// Interaction logic for FocusItems.xaml
    /// </summary>
    public partial class FocusItems : UserControl
    {
        public static readonly DependencyProperty PicMarginProperty = DependencyProperty.Register(
            "PicMargin", typeof (double), typeof (FocusItems), new PropertyMetadata(default(double)));

        public double PicMargin
        {
            get { return (double) GetValue(PicMarginProperty); }
            set { SetValue(PicMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items",
            typeof(List<FocusModel>),
            typeof(FocusItems),
            new PropertyMetadata(default(List<FocusModel>)));

        public static readonly DependencyProperty columnsNumProperty = DependencyProperty.Register(
            "columnsNum",
            typeof(int),
            typeof(FocusItems),
            new PropertyMetadata(default(int)));

        public FocusItems()
        {
            this.InitializeComponent();



        }

        /// <summary>
        /// Фон кнопки элемента фокусировки
        /// </summary>
        public Brush BackgroundProp { get; set; }

        public int columnsNum
        {
            get
            {
                return (int)GetValue(columnsNumProperty);
            }
            set
            {
                SetValue(columnsNumProperty, value);
            }
        }

       

        /// <summary>
        /// Число строк
        /// </summary>
        public int NumOfRows { get; set; }

        public List<FocusModel> Items
        {
            get
            {
                return (List<FocusModel>)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }
    }
}
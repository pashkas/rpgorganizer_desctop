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
    /// Interaction logic for ucTaskItemMenu.xaml
    /// </summary>
    public partial class ucTaskItemMenu : UserControl
    {
        public static readonly DependencyProperty LinksVisibilityProperty =
            DependencyProperty.Register(
                "LinksVisibility",
                typeof(Visibility),
                typeof(ucTaskItemMenu),
                new PropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty UcContextProperty = DependencyProperty.Register(
            "UcContext",
            typeof(object),
            typeof(ucTaskItemMenu),
            new PropertyMetadata(default(object)));

        public ucTaskItemMenu()
        {
            InitializeComponent();
        }

        public Visibility LinksVisibility
        {
            get
            {
                return (Visibility)GetValue(LinksVisibilityProperty);
            }
            set
            {
                SetValue(LinksVisibilityProperty, value);
            }
        }

        public object UcContext
        {
            get
            {
                return (object)GetValue(UcContextProperty);
            }
            set
            {
                SetValue(UcContextProperty, value);
            }
        }
    }
}
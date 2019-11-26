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
    /// Interaction logic for ucChaAbilInfo.xaml
    /// </summary>
    public partial class ucChaAbilInfo : UserControl
    {
        public static readonly DependencyProperty isLevelVisibleProperty = DependencyProperty.Register(
            "isLevelVisible",
            typeof(bool),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty levelProperty = DependencyProperty.Register(
            "level",
            typeof(int),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(int)));

        public static readonly DependencyProperty rangAndDescrProperty = DependencyProperty.Register(
            "rangAndDescr",
            typeof(string),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty nameOfTypeProperty = DependencyProperty.Register(
            "nameOfType",
            typeof(string),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty nameProperty = DependencyProperty.Register(
            "name",
            typeof(string),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty descriptionProperty = DependencyProperty.Register(
            "description",
            typeof(string),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty valProperty = DependencyProperty.Register(
            "val",
            typeof(double),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty valMinProperty = DependencyProperty.Register(
            "valMin",
            typeof(int),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(int)));

        public static readonly DependencyProperty valMaxProperty = DependencyProperty.Register(
            "valMax",
            typeof(int),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(int)));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image",
            typeof(object),
            typeof(ucChaAbilInfo),
            new PropertyMetadata(default(object)));

        public ucChaAbilInfo()
        {
            InitializeComponent();
        }

        public object Image
        {
            get
            {
                return (object)GetValue(ImageProperty);
            }
            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public bool isLevelVisible
        {
            get
            {
                return (bool)GetValue(isLevelVisibleProperty);
            }
            set
            {
                SetValue(isLevelVisibleProperty, value);
            }
        }

        public int level
        {
            get
            {
                return (int)GetValue(levelProperty);
            }
            set
            {
                SetValue(levelProperty, value);
            }
        }

        public string rangAndDescr
        {
            get
            {
                return (string)GetValue(rangAndDescrProperty);
            }
            set
            {
                SetValue(rangAndDescrProperty, value);
            }
        }

        public string nameOfType
        {
            get
            {
                return (string)GetValue(nameOfTypeProperty);
            }
            set
            {
                SetValue(nameOfTypeProperty, value);
            }
        }

        public string name
        {
            get
            {
                return (string)GetValue(nameProperty);
            }
            set
            {
                SetValue(nameProperty, value);
            }
        }

        public string description
        {
            get
            {
                return (string)GetValue(descriptionProperty);
            }
            set
            {
                SetValue(descriptionProperty, value);
            }
        }

        public double val
        {
            get
            {
                return (double)GetValue(valProperty);
            }
            set
            {
                SetValue(valProperty, value);
            }
        }

        public int valMin
        {
            get
            {
                return (int)GetValue(valMinProperty);
            }
            set
            {
                SetValue(valMinProperty, value);
            }
        }

        public int valMax
        {
            get
            {
                return (int)GetValue(valMaxProperty);
            }
            set
            {
                SetValue(valMaxProperty, value);
            }
        }
    }
}
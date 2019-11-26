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
    /// Interaction logic for ucImageInTool.xaml
    /// </summary>
    public partial class ucImageInTool : UserControl
    {
        public static readonly DependencyProperty PictureProperty = DependencyProperty.Register(
            "Picture",
            typeof(object),
            typeof(ucImageInTool),
            new PropertyMetadata(default(object)));

        public ucImageInTool()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Изображение
        /// </summary>
        public object Picture
        {
            get
            {
                return (object)GetValue(PictureProperty);
            }
            set
            {
                SetValue(PictureProperty, value);
            }
        }
    }
}
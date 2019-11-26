using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using Sample.Model;
using Sample.ViewModel;
using Path = System.IO.Path;

namespace Sample.View
{
    /// <summary>
    /// Interaction logic for GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Window
    {
        public GameOverWindow()
        {
            InitializeComponent();
            var imagePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Images",
                "Splashes",
                "самурай.jpg");
            img.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

        private void BtnHallOfGlory_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.OpenLink("http://nerdistway.blogspot.com/2013/05/blog-post_91.html");
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

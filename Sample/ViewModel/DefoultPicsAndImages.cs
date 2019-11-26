using System;
using System.IO;
using System.Windows.Media.Imaging;
using Sample.Model;

namespace Sample.ViewModel
{
    public static class DefoultPicsAndImages
    {
        public static byte[] DefoultAbilImage { get; set; }
        public static BitmapImage DefoultAbilPic { get; set; }
        public static byte[] DefoultCharactImage { get; set; }
        public static BitmapImage DefoultCharactPic { get; set; }
        public static byte[] DefoultQwestImage { get; set; }
        public static BitmapImage DefoultQwestPic { get; set; }
        public static byte[] DefoultRewImage { get; set; }
        public static BitmapImage DefoultRewPic { get; set; }
        public static byte[] DefoultTaskImage { get; set; }
        public static BitmapImage DefoultTaskPic { get; set; }

        public static void SetDefoultImages()
        {
            DefoultTaskImage =
                StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "Task.png"));
            DefoultAbilImage =
                StaticMetods.pathToImage(
                    Path.Combine(Directory.GetCurrentDirectory(), "Images", "AbDefoult.png"));
            DefoultCharactImage =
                StaticMetods.pathToImage(
                    Path.Combine(Directory.GetCurrentDirectory(), "Images", "ChaDefoult.jpg"));
            DefoultQwestImage =
                StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "mission.jpg"));
            DefoultRewImage =
                StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "gold.png"));

            // Картинки
            DefoultTaskPic =
                StaticMetods.getImagePropertyFromImage(DefoultTaskImage);
            DefoultAbilPic =
                StaticMetods.getImagePropertyFromImage(DefoultAbilImage);
            DefoultCharactPic =
               StaticMetods.getImagePropertyFromImage(DefoultCharactImage);
            DefoultQwestPic =
                StaticMetods.getImagePropertyFromImage(DefoultQwestImage);
            DefoultRewPic =
               StaticMetods.getImagePropertyFromImage(DefoultRewImage);
        }
    }
}
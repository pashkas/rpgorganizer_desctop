// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="">
//
// </copyright>
// <summary>
//   Interaction logic for App.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Sample.Model;
using Sample.Properties;
using Sample.View;
using Sample.ViewModel;

namespace Sample
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string InstrPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Instructions.html");

        /// <summary>
        /// Закрыть заставку, если она запущена
        /// </summary>
        /// <param name="splash"></param>
        private static void CloseSplash(StartWindow splash)
        {
            if (Settings.Default.showSplashes)
            {
                splash.Close();
            }
        }

        /// <summary>
        ///     Заставки по умолчанию. Не пользовательские.
        /// </summary>
        /// <returns></returns>
        private static List<Tuple<string, string, string>> DefoultSplashes()
        {
            return new List<Tuple<string, string, string>>
            {
                new Tuple<string, string, string>(
                    "panda.jpg",
                    "Мастерами кунгфу не рождаются - мастерами кунгфу становятся! Опасностей не пугаются - приемы точны, все сбудется!",
                    string.Empty),
                new Tuple<string, string, string>(
                    "episode5Final.jpg",
                    "Да прибудет с тобой СИЛА!",
                    "- Оби Ван Кеноби, Звездные Войны."),
                new Tuple<string, string, string>(
                    "супермен.jpg",
                    "Гвозди бы делать из этих людей - крепче бы не было в мире гвоздей!",
                    "- Маяковский."),
                new Tuple<string, string, string>(
                    "bobba.jpeg",
                    "Я просто человек, который хочет найти свой путь во вселенной.",
                    "- Бобба Фет, Звездные Войны"),
                new Tuple<string, string, string>(
                    "даблдор.jpg",
                    "Наши решения показывают кем мы являемся в действительности, гораздо лучше, чем наши способности.",
                    "- Дамблдор, Гарри Поттер﻿."),
                new Tuple<string, string, string>(
                    "самурай.jpg",
                    "Преодоление трудного начинается с легкого, осуществление великого начинается с малого, ибо в мире трудное образуется из легкого, а великое — из малого.",
                    "- Лао Цзы."),
                new Tuple<string, string, string>(
                    "Завтра.jpg",
                    "В конце концов, завтра — это другой день!",
                    "- Скарлет О Хара - унесенные ветром﻿."),
                new Tuple<string, string, string>(
                    "гномКузнец.jpg",
                    "Не скрывай своих талантов. Они были даны для использования. Что такое солнечные часы в тени?﻿",
                    string.Empty),
                new Tuple<string, string, string>(
                    "иньянь.jpeg",
                    "-Есть ли во мне сущности не дружелюбные? \n-Только две: Твои негативные мысли и Твои негативные эмоции. И Твоя задача - научиться выбирать, что думать и что чувствовать!﻿",
                    string.Empty)
            };
        }

        /// <summary>
        ///     Загружаем данные перса при запуске программы
        /// </summary>
        private static void LoadPersData()
        {
            // MegaPers
            //StaticMetods.PersProperty =
            //        Pers.LoadPers(@"C:\Users\tretyakovpk\Documents\GoogleChromePortable\RPOrg\1. Life RPG Organizer — копия\Sample\bin\Debug\Templates\MegaPers");

            //MainViewModel.ExportWiki(StaticMetods.PersProperty);

            var appFolder = Settings.Default.PathToPers;
            var pathToLP = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "LearningPers");

            if (!Directory.Exists(appFolder))
            {
                StaticMetods.PersProperty =
                    Pers.LoadPers(pathToLP);
                StaticMetods.PersProperty.IsFirstUse = true;
                StaticMetods.PersProperty.LastDateOfUseProperty = DateTime.Now.ToString();
            }
            else
            {
                StaticMetods.PersProperty = (Pers.LoadPers(Path.Combine(appFolder, "Pers"))
                                             ?? Pers.LoadPers(
                                                 pathToLP));
            }

            if (StaticMetods.PersProperty.ViewForDefoult == null)
            {
                StaticMetods.PersProperty.ViewForDefoult = StaticMetods.PersProperty.Views.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Запрет на запуск второго экземпляра приложения
        /// </summary>
        private static void NotRunSecondProgCopy()
        {
            var pid = Process.GetCurrentProcess().Id;
            var pname = Process.GetCurrentProcess().ProcessName;
            foreach (var p in Process.GetProcesses())
            {
                if (p.ProcessName == pname && p.Id != pid)
                {
                    Thread.Sleep(1000);
                    p.Kill();
                }
            }
        }

        /// <summary>
        ///     Защита паролем
        /// </summary>
        private static void PassProtect()
        {
            if (Settings.Default.isPassProtect)
            {
                var pw = new PassWindow();
                pw.btnCansel.Click += (o, args) => { Current.Shutdown(); };
                pw.btnOk.Click += (o, args) =>
                {
                    if (pw.passBox.Text == Settings.Default.Pass)
                    {
                        pw.Close();
                    }
                    else
                    {
                        Current.Shutdown();
                    }
                };

                pw.ShowDialog();
            }
        }

        /// <summary>
        ///     Установить приколькный курсор
        /// </summary>
        private static void SetCoolCursor()
        {
            // var p = System.Windows.Input.InputType.;
            // int maxTouches = GetSystemMetrics(0x95);

            var sri = GetResourceStream(new Uri("gam1232.ani", UriKind.Relative));
            var customCursor = new Cursor(sri.Stream);

            if (!Settings.Default.IsNotOwerriteCursor)
                Mouse.OverrideCursor = customCursor;
            else
                Mouse.OverrideCursor = Cursors.None;
        }

        /// <summary>
        ///     Настроить видимость надписей в заставке. Если они пустые, то и надписи невидимы.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sw"></param>
        /// <param name="who"></param>
        private static void SetSplashTextboxVisibilitis(string text, StartWindow sw, string who)
        {
            if (string.IsNullOrEmpty(text))
            {
                sw.TextBlock.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(who))
            {
                sw.who.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Показать заставку
        /// </summary>
        private static StartWindow ShowSplash()
        {
            var sw = new StartWindow();

            var who = string.Empty;
            var text = string.Empty;
            BitmapImage img = null;
            try
            {
                if (Settings.Default.showUserSplashes)
                {
                    UserSplash(ref who, ref text, ref img);
                }
                else
                {
                    SysSplash(ref who, ref text, ref img);
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                string filename = ((ConfigurationErrorsException)ex.InnerException).Filename;
                File.Delete(filename);
            }

            sw.who.Text = who;
            sw.TextBlock.Text = text;
            sw.Image.Source = img;

            SetSplashTextboxVisibilitis(text, sw, who);

            if (Settings.Default.showSplashes)
            {
                sw.Show();
                Thread.Sleep(Settings.Default.timeShowSplash);
            }

            return sw;
        }

        /// <summary>
        ///     Заставка по умолчанию (вшитая в программу)
        /// </summary>
        /// <param name="who"></param>
        /// <param name="text"></param>
        /// <param name="img"></param>
        private static void SysSplash(ref string who, ref string text, ref BitmapImage img)
        {
            var defoultSplashes = DefoultSplashes();

            var index = MainViewModel.rnd.Next(0, defoultSplashes.Count);
            var imagePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Images",
                "Splashes",
                defoultSplashes[index].Item1);

            var summary = "\"" + defoultSplashes[index].Item2 + "\"";

            who = defoultSplashes[index].Item3;
            text = summary;
            img = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        ///     Пользовательская заставка (из папки с файлами)
        /// </summary>
        /// <param name="who"></param>
        /// <param name="text"></param>
        /// <param name="img"></param>
        private static void UserSplash(ref string who, ref string text, ref BitmapImage img)
        {
            var im = StaticMetods.GetRandomImage(Settings.Default.userSplashesFolder);
            var fileName = im.Item2;
            if (Settings.Default.ShowFileSplashName == false)
            {
                fileName = string.Empty;
            }

            who = string.Empty;
            text = fileName;
            img = im.Item1;
        }

        /// <summary>
        /// Загрузка приложения
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            NotRunSecondProgCopy();

            // var splash = ShowSplash();

            var path = Path.Combine(
                Directory.GetCurrentDirectory());

            // Загружаем конфиг
            path = Path.Combine(path, "Config" + ".json");
            string text = File.ReadAllText(path);
            var conf = JsonConvert.DeserializeObject<StaticSettings>(text);
            StaticMetods.Config = conf;

            // Загружаем пути
            // Путь к персу
            var p = Settings.Default.PathToPers;
            if (string.IsNullOrWhiteSpace(p) || !Directory.Exists(p))
            {
                p = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            Sample.Properties.Resources.NameOfTheGame);
                Settings.Default.PathToPers = p;
                Settings.Default.Save();
            }
            // Путь к дропбоксу
            var p1 = Settings.Default.PathToDropBox;
            if (string.IsNullOrWhiteSpace(p1) || !Directory.Exists(p1))
            {
                p1 = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Dropbox");
                Settings.Default.PathToDropBox = p1;
                Settings.Default.Save();
            }

            // Загружаем персонаж

            DefoultPicsAndImages.SetDefoultImages();

            LoadPersData();

            SetCoolCursor();

            var mv = new MainView();

            PassProtect();

            // CloseSplash(splash);

            if (StaticMetods.PersProperty.PersLevelProperty >= 100)
                StaticMetods.ShowGameOver();
            else
                mv.Show();
        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern int GetSystemMetrics(int nIndex);
    }
}
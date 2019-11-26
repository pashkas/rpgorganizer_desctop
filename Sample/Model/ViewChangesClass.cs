using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Sample.View;

namespace Sample.Model
{
    /// <summary>
    /// Класс для отображения изменений
    /// </summary>
    public class ViewChangesClass
    {
        /// <summary>
        /// Значения до изменений
        /// </summary>
        public List<Tuple<string, string, string>> ValBefore { get; set; }

        /// <summary>
        /// Значения после изменений
        /// </summary>
        public List<Tuple<string, string, string>> ValAfter { get; set; }

        /// <summary>
        /// Модель для отображения изменений
        /// </summary>
        public List<viewChangesModel> ViewChanges { get; set; }

        /// <summary>
        ///     The pers.
        /// </summary>
        private Pers Pers
        {
            get { return StaticMetods.PersProperty; }
            set
            {
                StaticMetods.PersProperty = value;
            }
        }

        public List<Revard> AllRevards { get; set; }

        /// <summary>
        /// Инициализатор
        /// </summary>
        public ViewChangesClass(List<Revard> allRev)
        {
            // тип, гуид, значение
            ValBefore = new List<Tuple<string, string, string>>();
            ValAfter = new List<Tuple<string, string, string>>();
            ViewChanges = new List<viewChangesModel>();
            AllRevards = allRev;
        }

        /// <summary>
        /// Получить значения до
        /// </summary>
        public void GetValBefore()
        {
            Pers.GetValuesCollection(ValBefore);
        }

        /// <summary>
        /// Получить значения после изменений
        /// </summary>
        public void GetValAfter()
        {
            bool b = Pers.CheckBuffExpByHp();
            Pers.GetValuesCollection(ValAfter);
            if (b)
            {
                ValAfter.Remove(ValAfter.FirstOrDefault(n => n.Item2 == "здоровье"));
            }
        }

        public void ShowChanges(string _header, Brush _color, byte[] _itemImage, string date = "", bool showEndTurn = false, bool isShowEditAb = true, bool isOnlyShowArts = false)
        {
            double beforeExp;
            double afterExp;
            GetChanges(out beforeExp, out afterExp);
            bool isShowInfo = false;
            var notRev = ViewChanges.Where(n => n.ТипХарактеристики != "награда").ToList();

            // Показываем сообщения по поводу наград
            var rev = ViewChanges.Where(n => n.ТипХарактеристики == "награда").ToList();
            foreach (viewChangesModel vcm in rev)
            {
                var lv = new LevelsChangesView();
                var cs = vcm.ChangeProperty >= 0 ? "+++" : "---";
                var header = $"{cs} {vcm.названиеХарактеристики} {cs}";
                lv.Image.Source = StaticMetods.getImagePropertyFromImage(vcm.ImageProperty);
                lv.down.Visibility = Visibility.Collapsed;
                lv.Header.Text = header;
                lv.Header.Foreground = vcm.ChangeProperty > 0 ? Brushes.Green : Brushes.Red;
                lv.btnOk.Click += (sender, args) =>
                {
                    lv.Close();
                };
                lv.down.Visibility = Visibility.Collapsed;
                lv.up.Visibility = Visibility.Collapsed;
                lv.ShowDialog();
            }

            // Показываем изменения.....................................................................................
            // Показываем изменения (навыки)

            if(!StaticMetods.PersProperty.PersSettings.IsNotShowNotifications)
                StaticMetods.ShowItemToPersChanges(_header, _itemImage, notRev.Where(n => n.ТипХарактеристики != "Ранг").ToList(), _color, date, null, showEndTurn, true);
            // StaticMetods.ShowItemToPersChanges(_header, _itemImage, notRev.Where(n => n.ТипХарактеристики != "ХарактеристикаУр" && n.ТипХарактеристики != "НавыкУр" && n.ТипХарактеристики != "Ранг").ToList(), _color, date, null, showEndTurn, true);
            // StaticMetods.ShowItemToPersChanges("Навыки", _itemImage, notRev.Where(n => n.ТипХарактеристики == "НавыкУр").ToList(), _color, date, null, showEndTurn);
            //StaticMetods.ShowItemToPersChanges("Характеристики", _itemImage, notRev.Where(n => n.ТипХарактеристики == "ХарактеристикаУр").ToList(), _color, date, null, showEndTurn);
            
            //----------------------------------------------------------------------------------
            // Выполненные квесты
            Pers.ShowDoneQwests(ValBefore, ValAfter, true);

            // Изменения уровней навыков
            //Pers.ShowChengeAbLevels(ValBefore, ValAfter, isShowEditAb, false);

            // Изменения уровней характеристик
            //Pers.ShowChangeChaLevels(ValBefore, ValAfter);

            // Показываем изменения (опыт)
            //StaticMetods.ShowItemToPersChanges("Опыт", _itemImage, notRev.Where(n => n.ТипХарактеристики == "Опыт").ToList(), _color, date, null, showEndTurn);

            // Изменение уровней персонажа
            Pers.ShowChangePersLevel(afterExp, beforeExp, notRev, out isShowInfo);
            //-----------------------------------------------------------------------------------

            // Открываем лист персонажа, если новый уровень
            if (isShowInfo)
                StaticMetods.Locator.MainVM.OpenPersWindow(new Tuple<string, string>("Окно персонажа", "Информация"));

            // Редактируем квесты
            //foreach (var aim in qwToEdit)
            //{
            //    StaticMetods.editAim(aim);
            //}
            //// Навыки
            //foreach (var abilitiModel in absToEdit)
            //{
            //    abilitiModel.EditAbility(null, -1, true);
            //}

            StaticMetods.AbillitisRefresh(Pers);
            StaticMetods.RefreshAllQwests(Pers, true, true, true);
        }


        public void GetChanges(out double beforeExp, out double afterExp)
        {
            int beforeGold;
            int afterGold;
            Pers.GetChanges(
                ValBefore,
                ValAfter,
                ViewChanges,
                out beforeExp,
                out afterExp,
                out beforeGold,
                out afterGold, AllRevards);
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Sample.Model;
using Sample.ViewModel;

namespace Sample
{
    public class ViewChangesMinMaxConverter : IMultiValueConverter
    {
        #region Public Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values[0] == DependencyProperty.UnsetValue) return 0;
            viewChangesModel vc = values[0] as viewChangesModel;
            if (vc == null) return 0;
            double val = System.Convert.ToDouble(values[1]);
            if (parameter.ToString() == "min")
            {
                switch (vc.“ип’арактеристики)
                {
                    case "Ќавык":
                        return getMinForAbility(val, vc);

                    case "’арактеристика":
                        return getMinForCha(val, vc);

                    case "ќпыт":
                        return getMinForExp(val, vc);

                    default:
                        return vc.MinValueProperty;
                }
            }
            else if (parameter.ToString() == "max")
            {
                switch (vc.“ип’арактеристики)
                {
                    case "Ќавык":
                        return getMaxForAbility(val, vc);

                    case "’арактеристика":
                        return getMaxForCha(val, vc);

                    case "ќпыт":
                        return getMaxForExp(val, vc);

                    default:
                        return vc.MaxValueProperty;
                }
            }
            else if (parameter.ToString() == "rang")
            {
                switch (vc.“ип’арактеристики)
                {
                    case "Ќавык":
                        return getRangForAbility(val, vc);

                    case "’арактеристика":
                        return getRangForCha(val, vc);

                    case "Ќавык”р":
                        return getRangForAbility(val, vc);

                    case "’арактеристика”р":
                        return getRangForCha(val, vc);

                    case "ќпыт":
                        return getRangForExp(val, vc);

                    default:
                        return vc.RangProperty;
                }
            }
            else if (parameter.ToString() == "rang2")
            {
                switch (vc.“ип’арактеристики)
                {
                    case "ќпыт":
                        var maxForExp = getMaxForExp(val, vc);
                        return "/ " + maxForExp.ToString().SplitInParts(3);

                    default:
                        return vc.RangProperty2;
                }
            }
            else if (parameter.ToString() == "vval")
            {
                switch (vc.“ип’арактеристики)
                {
                    case "ќпыт":
                        return System.Convert.ToInt32(val).ToString().SplitInParts(3);

                    default:
                        return System.Convert.ToInt32(val).ToString();
                }
            }
            else
            {
                return 0;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Private Methods

        private double getMaxForAbility(double val, viewChangesModel vc)
        {
            var minForAbility = Math.Floor(val) + 1.0;
            //if (vc.to < vc.from && val == minForAbility-1)
            //{
            //    minForAbility = minForAbility - 1;
            //}
            if (minForAbility < 1) minForAbility = 1;
            return minForAbility;
        }

        private double getMaxForCha(double val, viewChangesModel vc)
        {
            var minForCha = Math.Floor(val) + 1.0;
            //if (vc.to < vc.from && val == minForCha-1) minForCha = minForCha - 1;
            if (minForCha < 1) minForCha = 1;
            return minForCha;
        }

        private double getMaxForExp(double val, viewChangesModel vc)
        {
            var lev = StaticMetods.GetLevel(val, RpgItemsTypes.exp);
            var expToLevel = Pers.ExpToLevel(lev + 1, RpgItemsTypes.exp);
            //if (vc.to < vc.from && val == Pers.ExpToLevel(lev, RpgItemsTypes.exp))
            //{
            //    var level = lev - 1;
            //    if (level < 1) level = 1;
            //    expToLevel = Pers.ExpToLevel(level, RpgItemsTypes.exp);
            //}
            return System.Convert.ToInt32(expToLevel);
        }

        private double getMinForAbility(double val, viewChangesModel vc)
        {
            var minForAbility = Math.Floor(val) - 0.01;
            //if (vc.to < vc.from && val == minForAbility)
            //{
            //    minForAbility = minForAbility - 1;
            //}
            if (minForAbility < 0) minForAbility = 0;
            return minForAbility;
        }

        private double getMinForCha(double val, viewChangesModel vc)
        {
            var minForCha = Math.Floor(val) - 0.01;
            //if (vc.to < vc.from && val == minForCha) minForCha = minForCha - 1;
            if (minForCha < 0) minForCha = 0;
            return minForCha;
        }

        private double getMinForExp(double val, viewChangesModel vc)
        {
            var lev = StaticMetods.GetLevel(val, RpgItemsTypes.exp);
            var expToLevel = Pers.ExpToLevel(lev, RpgItemsTypes.exp) - 1;
            //if (vc.to < vc.from && val == expToLevel)
            //{
            //    var level = lev-1;
            //    if (level < 1) level = 1;
            //    expToLevel = Pers.ExpToLevel(level, RpgItemsTypes.exp);
            //}
            //var level = lev - 1;
            //if (level < 1) level = 1;
            //expToLevel = Pers.ExpToLevel(level, RpgItemsTypes.exp);
            if (expToLevel < 0) expToLevel = 0;
            return System.Convert.ToInt32(expToLevel);
        }

        private string getRang2ForExp(double val, viewChangesModel vc)
        {
            var lev = StaticMetods.GetLevel(val, RpgItemsTypes.exp);
            return lev.ToString() + ":";
        }

        private string getRangForAbility(double val, viewChangesModel vc)
        {
            if (StaticMetods.PersProperty.PersSettings.Is10AbLevels 
                || StaticMetods.PersProperty.PersSettings.Is5_5_50)
            {
                string v = Math.Round(val, 2).ToString().Replace(",", ".").Replace(" ", "");

                return $"{v}";
            }

            var lev = Math.Floor(val);

            if (StaticMetods.PersProperty.PersSettings.IsFUDGE)
            {
                return $"{StaticMetods.PersProperty.PersSettings.AbRangs[System.Convert.ToInt32(lev)].Name}";
            }

            return $"{StaticMetods.PersProperty.PersSettings.AbRangs[System.Convert.ToInt32(lev)].Name}%";
        }

        private string getRangForCha(double val, viewChangesModel vc)
        {
            if (StaticMetods.PersProperty.PersSettings.IsFUDGE || StaticMetods.PersProperty.PersSettings.IsNoAbs)
            {
                var lev = Math.Floor(val);

                return $"{StaticMetods.PersProperty.PersSettings.CharacteristicRangs[System.Convert.ToInt32(lev)].Name}";
            }

            //var lev = Math.Floor(val);
            //getMinForCha(val, vc);
            //string v = StaticMetods.PersProperty.PersSettings.CharacteristicRangs[System.Convert.ToInt32(lev)].Name;
            string v = Math.Round(val,2).ToString().Replace(",",".").Replace(" ", "");

            return $"{v}";
        }

        private string getRangForExp(double val, viewChangesModel vc)
        {
            var lev = StaticMetods.GetLevel(val, RpgItemsTypes.exp);

            return lev.ToString() + ":";
        }

        #endregion Private Methods
    }
}
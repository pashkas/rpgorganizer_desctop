using System;
using GalaSoft.MvvmLight.Command;
using Sample.Model;

namespace Sample.ViewModel
{
    public class ucSetGoldExpRevardViewModel
    {
        #region Public Properties

        public Aim Qwest { get; set; }
        public Task Task { get; set; }

        /// <summary>
        /// Gets the Задать награду.
        /// </summary>
        public RelayCommand<string> SetRewCommand
        {
            get
            {
                return setRewCommand
                       ?? (setRewCommand = new RelayCommand<string>(
                           item =>
                           {
                               double expToNextLev = Pers.ExpToLevel(StaticMetods.PersProperty.PersLevelProperty + 1,
                                   RpgItemsTypes.exp) -
                                                     Pers.ExpToLevel(StaticMetods.PersProperty.PersLevelProperty,
                                                         RpgItemsTypes.exp);
                               double rev = 0;
                               string type = "";

                               if (Qwest != null)
                               {
                                   switch (item)
                                   {
                                       case "простоGold":
                                           rev = expToNextLev / 40.0;
                                           type = "gold";
                                           break;

                                       case "нормGold":
                                           rev = expToNextLev / 20.0;
                                           type = "gold";
                                           break;

                                       case "сложноGold":
                                           rev = expToNextLev / 10.0;
                                           type = "gold";
                                           break;

                                       case "простоExp":
                                           rev = expToNextLev / 4.0;
                                           type = "exp";
                                           break;

                                       case "нормExp":
                                           rev = expToNextLev / 2.0;
                                           type = "exp";
                                           break;

                                       case "сложноExp":
                                           rev = expToNextLev / 1.0;
                                           type = "exp";
                                           break;
                                   }

                                   if (type == "exp")
                                   {
                                       Qwest.PlusExp = (int)Math.Ceiling(rev);
                                   }
                                   else
                                   {
                                       if (StaticMetods.PersProperty.PersSettings.IsGoldEnabled)
                                       {
                                           Qwest.GoldIfDoneProperty = (int)Math.Ceiling(rev);
                                       }
                                       else
                                       {
                                           Qwest.GoldIfDoneProperty = 0;
                                       }
                                   }
                               }
                               else if (Task != null)
                               {
                                   switch (item)
                                   {
                                       case "простоGold":
                                           rev = expToNextLev / 1000.0;
                                           type = "gold";
                                           break;

                                       case "нормGold":
                                           rev = expToNextLev / 500.0;
                                           type = "gold";
                                           break;

                                       case "сложноGold":
                                           rev = expToNextLev / 250.0;
                                           type = "gold";
                                           break;

                                       case "простоExp":
                                           rev = expToNextLev / 200.0;
                                           type = "exp";
                                           break;

                                       case "нормExp":
                                           rev = expToNextLev / 100.0;
                                           type = "exp";
                                           break;

                                       case "сложноExp":
                                           rev = expToNextLev / 50.0;
                                           type = "exp";
                                           break;
                                   }

                                   if (type == "exp")
                                   {
                                       Task.PlusExp = (int)Math.Ceiling(rev);
                                   }
                                   else
                                   {
                                       if (StaticMetods.PersProperty.PersSettings.IsGoldEnabled)
                                       {
                                           Task.PlusGold = (int)Math.Ceiling(rev);
                                       }
                                       else
                                       {
                                           Task.PlusGold = 0;
                                       }
                                   }
                               }
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        #endregion Public Properties

        #region Private Fields

        /// <summary>
        /// Gets the Задать награду.
        /// </summary>
        private RelayCommand<string> setRewCommand;

        #endregion Private Fields
    }
}
using GalaSoft.MvvmLight;

namespace Sample.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Media.Effects;

    using GalaSoft.MvvmLight.Messaging;

    using Sample.Model;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class UcPerksViewModel : ucAbilityViewModel
    {
        public override bool hideNotActiveAbilitisProperty
        {
            get
            {
                return this.PersProperty != null && this.PersProperty.PersSettings.HideNotActivPerksProperty;
            }

            set
            {
                if (this.hideNotActiveAbilitisProperty == value)
                {
                    return;
                }

                this.PersProperty.PersSettings.HideNotActivPerksProperty = value;
                OnPropertyChanged(nameof(hideNotActiveAbilitisProperty));
                RefreshAbilitis();
            }
        }

        public override string AllName
        {
            get
            {
                return "перков:";
            }
        }

        public override int NumOfAbilitisProperty
        {
            get
            {
                return ChaAbilitises.Count;
            }
        }

        /// <summary>
        /// Активные скиллы для обновления
        /// </summary>
        public override IEnumerable<AbilitiModel> activeAbsToRefresh
        {
            get
            {
                var active =
                    PersProperty.Abilitis.Where(
                        n => n.NameOfProperty.ToLower().Contains(this.FilterProperty.ToLower()));

                if (this.hideNotActiveAbilitisProperty == true)
                {
                    active = active.Where(n => n.IsEnebledProperty);
                }
                return active;
            }
        }

        public override void addAbility()
        {
            AbilitiModel addedAbility = AbilitiModel.AddAbility(this.PersProperty);

            if (addedAbility != null)
            {
                AbilitiModel selectedAbilitiModelProperty = addedAbility;
                this.SelectedAbilitiModelProperty = selectedAbilitiModelProperty;
            }

            RefreshAbilitis();
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelLocator.cs" company="">
//   
// </copyright>
// <summary>
//   This class contains static references to all the view models in the
//   application and provides an entry point for the bindings.
//   See http://www.galasoft.ch/mvvm
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;

    using Microsoft.Practices.ServiceLocation;

    using Sample.Model;
    using Sample.View;
    using Sample.ViewModel;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<AddOrEditTaskNeedViewModel>();

            SimpleIoc.Default.Register<AddOrEditAimNeedViewModel>();

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<AimsViewModel>();

            SimpleIoc.Default.Register<QwestsViewModel>();

            SimpleIoc.Default.Register<ucNeednessViewModel>();

            SimpleIoc.Default.Register<PersSettingsViewModel>();

            SimpleIoc.Default.Register<AddOrEditAbilityViewModel>();

            SimpleIoc.Default.Register<ActiveAbilsTasksVM>();

            SimpleIoc.Default.Register<ucNeednessInMainViewModel>();

            SimpleIoc.Default.Register<DostijeniaViewModel>();

            SimpleIoc.Default.Register<ucAllTasksViewModel>();

            SimpleIoc.Default.Register<ucAbilityViewModel>();

            SimpleIoc.Default.Register<UcPerksViewModel>();

            SimpleIoc.Default.Register<ucCharactViewModel>();

            SimpleIoc.Default.Register<SettingsPers>();

            SimpleIoc.Default.Register<AddOrEditCharacteristicViewModel>();

            SimpleIoc.Default.Register<AddOrEditAbilNeedViewModel>();

            SimpleIoc.Default.Register<PlaningViewModel>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }

        #endregion

        #region Public Properties

        public AddOrEditAbilNeedViewModel AddOrEditAbilNeedVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddOrEditAbilNeedViewModel>();
            }
        }

        public PlaningViewModel PlaningVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlaningViewModel>();
            }
        }

        public AddOrEditCharacteristicViewModel AddOrEditCharacteristicVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddOrEditCharacteristicViewModel>();
            }
        }

        public SettingsPers SettingPersVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsPers>();
            }
        }

        public ucCharactViewModel ucCharactVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ucCharactViewModel>();
            }
        }

        public ucAbilityViewModel ucAbilitisVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ucAbilityViewModel>();
            }
        }

        public UcPerksViewModel ucPerksVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UcPerksViewModel>();
            }
        }

        public ucAllTasksViewModel AllTasksVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ucAllTasksViewModel>();
            }
        }

        public DostijeniaViewModel DostijeniyaVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DostijeniaViewModel>();
            }
        }

        public ucNeednessInMainViewModel NeednessInMainViewVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ucNeednessInMainViewModel>();
            }
        }

        public ucNeednessViewModel NeednessVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ucNeednessViewModel>();
            }
        }

        /// <summary>
        /// Вью модель для требований задач
        /// </summary>
        public AddOrEditTaskNeedViewModel AddOrEditTaskNeedVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddOrEditTaskNeedViewModel>();
            }
        }

        /// <summary>
        /// Вью модель для требований целей скиллов
        /// </summary>
        public AddOrEditAimNeedViewModel AddOrEditAimNeedVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddOrEditAimNeedViewModel>();
            }
        }

        /// <summary>
        /// Вью модель для активных задач скилла
        /// </summary>
        public ActiveAbilsTasksVM ActiveAbTasksVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ActiveAbilsTasksVM>();
            }
        }

        /// <summary>
        /// Вью модель для главного окна программы
        /// </summary>
        public AddOrEditAbilityViewModel AddOrEditAbilityVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddOrEditAbilityViewModel>();
            }
        }

        /// <summary>
        /// Вью модель для главного окна программы
        /// </summary>
        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Вью модель для Целей
        /// </summary>
        public AimsViewModel AimsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AimsViewModel>();
            }
        }

        /// <summary>
        /// Вью модель для информации о квестах
        /// </summary>
        public QwestsViewModel QwestsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<QwestsViewModel>();
            }
        }

        /// <summary>
        /// Вью модель с настройками персонажа
        /// </summary>
        public PersSettingsViewModel PersSettingsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PersSettingsViewModel>();
            }
        }

        #endregion
    }
}
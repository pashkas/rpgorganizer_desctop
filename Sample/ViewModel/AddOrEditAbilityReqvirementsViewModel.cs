using System;
using System.Collections.ObjectModel;
using System.Linq;
using Sample.Model;
using WPFExtensions.Collections.ObjectModel;

namespace Sample.ViewModel
{
    public partial class AddOrEditAbilityViewModel : IHaveNeedAbilities, IHaveNeedCharacts
    {
        #region Public Properties

        /// <summary>
        /// ���������� �������������
        /// </summary>
        public ObservableCollection<NeedCharact> AbilityNeedCharacts
        {
            get { return SelectedAbilitiModelProperty?.NeedCharacts; }
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        public ObservableCollection<NeedAbility> AbilityNeedAbilities
        {
            get { return SelectedAbilitiModelProperty?.NeedAbilities; }
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        public ObservableCollection<Aim> ReqwireAims
        {
            get
            {
                return SelectedAbilitiModelProperty?.ReqwireAims;
            }
        }

        /// <summary>
        /// ��������� ��� ���������� ������ �������
        /// </summary>
        public ucRevardAbilityNeedViewModel NeedAbilitiesDataContext
            =>
                new ucRevardAbilityNeedViewModel(PersProperty, AbilityNeedAbilities,
                    SelectedAbilitiModelProperty, SelectedAbilitiModelProperty);

        /// <summary>
        /// ��������� ��� ���������� �������������
        /// </summary>
        public ucRevardNeedCharacteristics NeedChaDataContext
            => new ucRevardNeedCharacteristics(PersProperty, AbilityNeedCharacts, SelectedAbilitiModelProperty);

        #endregion Public Properties
    }
}
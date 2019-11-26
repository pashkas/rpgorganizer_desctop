using System.Collections.ObjectModel;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    ///     ��������� ��� ������������� ������������ ���������� ���������� �������
    /// </summary>
    public class InicilizeRevardNeedAbilityMessege
    {
        #region Properties

        /// <summary>
        ///     ��������
        /// </summary>
        public Pers PersProperty { get; set; }

        /// <summary>
        ///     ���������� ������� ��� �������
        /// </summary>
        public ObservableCollection<NeedAbility> RevardAbilityNeeds { get; set; }

        #endregion Properties
    }
}
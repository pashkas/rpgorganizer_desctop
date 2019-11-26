using System.Collections.ObjectModel;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    ///     ��������� ��� ������������� ������������ ���������� ���������� �������������
    /// </summary>
    public class InicilizeRevardNeedQwestsMessege
    {
        #region Properties

        /// <summary>
        ///     ��������
        /// </summary>
        public Pers PersProperty { get; set; }

        /// <summary>
        ///     ���������� ������� ��� �������
        /// </summary>
        public ObservableCollection<Aim> RevardQwestNeeds { get; set; }

        #endregion Properties
    }
}
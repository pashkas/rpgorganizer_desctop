namespace Sample.Model
{
    using Sample.ViewModel;

    /// <summary>
    /// ��������� ��� �������� � �������� ���� ����������
    /// </summary>
    public interface IItemsReqvirementsable
    {
        ucRelaysItemsVM ReqvireItemsVm { get; set; }

        /// <summary>
        /// �������� �������� - ���������� ��� ������� ��������
        /// </summary>
        void getReqvireItems();
    }
}
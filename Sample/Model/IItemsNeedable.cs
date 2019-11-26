namespace Sample.Model
{
    using Sample.ViewModel;

    public interface IItemsNeedable
    {
        /// <summary>
        /// ��� ������ ��� ����������
        /// </summary>
        ucRelaysItemsVM NeedsItemsVM { get; set; }

        /// <summary>
        /// �������� �������� �� ������� ������� ���� �������
        /// </summary>
        void getNeedsItems();
    }
}
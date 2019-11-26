namespace Sample.Model
{
    using Sample.ViewModel;

    public interface IItemsRelaysable
    {
        ucRelaysItemsVM RelaysItemsVm { get; set; }

        /// <summary>
        /// �������� �������� �� ������� ������ ���� �������
        /// </summary>
        void getRelaysItems();
    }
}
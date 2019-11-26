namespace Sample.Model
{
    using Sample.ViewModel;

    public interface IItemsRelaysable
    {
        ucRelaysItemsVM RelaysItemsVm { get; set; }

        /// <summary>
        /// Получить элементы на которые влияет этот элемент
        /// </summary>
        void getRelaysItems();
    }
}
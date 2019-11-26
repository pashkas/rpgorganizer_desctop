namespace Sample.Model
{
    using Sample.ViewModel;

    public interface IItemsNeedable
    {
        /// <summary>
        /// Вью модель для тробований
        /// </summary>
        ucRelaysItemsVM NeedsItemsVM { get; set; }

        /// <summary>
        /// Получить элементы от которых зависит этот элемент
        /// </summary>
        void getNeedsItems();
    }
}
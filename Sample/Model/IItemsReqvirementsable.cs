namespace Sample.Model
{
    using Sample.ViewModel;

    /// <summary>
    /// Интерфейс для элемента у которого есть требования
    /// </summary>
    public interface IItemsReqvirementsable
    {
        ucRelaysItemsVM ReqvireItemsVm { get; set; }

        /// <summary>
        /// Получить элементы - требования для данного элемента
        /// </summary>
        void getReqvireItems();
    }
}
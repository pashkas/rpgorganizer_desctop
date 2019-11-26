using System.Collections.ObjectModel;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    ///     Сообщение для инициализации юзерконтрола добавления требований характеристик
    /// </summary>
    public class InicilizeRevardNeedQwestsMessege
    {
        #region Properties

        /// <summary>
        ///     Персонаж
        /// </summary>
        public Pers PersProperty { get; set; }

        /// <summary>
        ///     Требования квестов для награды
        /// </summary>
        public ObservableCollection<Aim> RevardQwestNeeds { get; set; }

        #endregion Properties
    }
}
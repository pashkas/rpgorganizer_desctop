using System.Collections.ObjectModel;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    ///     Сообщение для инициализации юзерконтрола добавления требований характеристик
    /// </summary>
    public class InicilizeRevardNeedCharacteristicsMessege
    {
        #region Properties

        /// <summary>
        ///     Персонаж
        /// </summary>
        public Pers PersProperty { get; set; }

        /// <summary>
        ///     Требования скиллов для награды
        /// </summary>
        public ObservableCollection<NeedCharact> RevardCharactNeeds { get; set; }

        #endregion Properties
    }
}
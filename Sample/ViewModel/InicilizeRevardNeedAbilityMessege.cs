using System.Collections.ObjectModel;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    ///     Сообщение для инициализации юзерконтрола добавления требований скиллов
    /// </summary>
    public class InicilizeRevardNeedAbilityMessege
    {
        #region Properties

        /// <summary>
        ///     Персонаж
        /// </summary>
        public Pers PersProperty { get; set; }

        /// <summary>
        ///     Требования скиллов для награды
        /// </summary>
        public ObservableCollection<NeedAbility> RevardAbilityNeeds { get; set; }

        #endregion Properties
    }
}
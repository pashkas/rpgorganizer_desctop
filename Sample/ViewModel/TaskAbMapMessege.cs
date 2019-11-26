using System.Collections.Generic;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    /// Сообщение о создании карты квестов
    /// </summary>
    public class TaskAbMapMessege
    {
        #region Public Properties

        /// <summary>
        /// Отображать только эти задачи
        /// </summary>
        public List<Task> OnlyThisTasks { get; set; }

        /// <summary>
        /// Персонаж
        /// </summary>
        public Pers PersProperty { get; set; }

        /// <summary>
        /// Выбранный вид
        /// </summary>
        public ViewsModel SellectedViewProperty { get; set; }

        /// <summary>
        /// Карта из главного окна?
        /// </summary>
        public bool isFromeMainWindow { get; set; }

        #endregion
    }
}
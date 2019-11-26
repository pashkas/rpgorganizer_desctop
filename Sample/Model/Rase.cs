namespace Sample.Model
{
    using System;

    /// <summary>
    /// Класс с рассой персонажа
    /// </summary>
    [Serializable]
    public class Rase
    {
        #region Public Properties

        /// <summary>
        /// Название поля расса
        /// </summary>
        public string NameProperty { get; set; }

        /// <summary>
        /// Значение поля расса
        /// </summary>
        public string ValueProperty { get; set; }

        #endregion
    }
}
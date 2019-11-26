namespace Sample.Model
{
    using System;

    /// <summary>
    /// Контекст для задач, место где их можно выполнять
    /// </summary>
    [Serializable]
    public class Context
    {
        #region Public Properties

        /// <summary>
        /// Название контекста
        /// </summary>
        public string NameOfContext { get; set; }

        /// <summary>
        /// Уникальный номер контекста
        /// </summary>
        public string Uid { get; set; }

        #endregion
    }
}
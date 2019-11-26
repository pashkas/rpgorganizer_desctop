namespace Sample.Model
{
    using System;

    /// <summary>
    /// Статус задачи, типа первым делом, поручено, когда-нибудь и.т.д.
    /// </summary>
    [Serializable]
    public class StatusTask
    {

        #region Public Properties
        
        /// <summary>
        /// Название статуса
        /// </summary>
        public string NameOfStatus { get; set; }

        /// <summary>
        /// Уникальный номер статуса
        /// </summary>
        public string Uid { get; set; }

        #endregion
    }
}
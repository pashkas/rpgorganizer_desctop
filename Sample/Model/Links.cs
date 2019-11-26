using System;

namespace Sample.Model
{
    [Serializable]
    public class Links
    {
        #region Properties

        /// <summary>
        ///     Сама ссылка
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     Название ссылки
        /// </summary>
        public string LinkName { get; set; }

        #endregion Properties
    }
}
namespace Sample.Model
{
    using System;

    [Serializable]
    public class TaskLinks
    {
        /// <summary>
        /// Название элемента на который влияет задача
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип элемента на который влияет задача
        /// </summary>
        public RpgItemsTypes RPGItemType { get; set; }

        /// <summary>
        /// ИД элемента на который влияет задача
        /// </summary>
        public string IdElement { get; set; }
    }
}
namespace Sample.Model
{
    using System.Collections.Generic;

    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// Элемент влияет на другие элементы
    /// </summary>
    public interface IRelaysable
    {
        /// <summary>
        /// Список влияющих элементов
        /// </summary>
        List<RelaysItem> RelaysItems { get; }
    }
}
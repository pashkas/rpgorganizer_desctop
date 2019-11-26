// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesCharactEnum.cs" company="">
//   
// </copyright>
// <summary>
//   Типы на что влияют задачи
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    public enum typeOfSubTaskRecurrenses
    {
        послеПовтора,
        послеВыполненияПодзадач,
        неПовторять,
        неПовторятьУдалять
    }


    /// <summary>
    /// Типы на что влияют задачи
    /// </summary>
    public enum typesCharactEnum
    {
        /// <summary>
        /// The квест.
        /// </summary>
        квест,

        /// <summary>
        /// The скилл.
        /// </summary>
        навык,

        /// <summary>
        /// The характеристика.
        /// </summary>
        характеристика
    }
}
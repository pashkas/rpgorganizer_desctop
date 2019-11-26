// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopupInformationMessege.cs" company="">
//   
// </copyright>
// <summary>
//   Сообщение для отображения сообщения с информацией
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    /// <summary>
    /// Сообщение для отображения сообщения с информацией
    /// </summary>
    public class PopupInformationMessege
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupInformationMessege"/> class.
        /// </summary>
        /// <param name="messege">
        /// The messege.
        /// </param>
        /// <param name="linktext">
        /// The linktext.
        /// </param>
        /// <param name="link">
        /// The link.
        /// </param>
        public PopupInformationMessege(string messege, string linktext, string link)
        {
            this.Messege = messege;
            this.LinkText = linktext;
            this.Link = link;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Ссылка по которой будет переход
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Сообщение которое будет отображено на ссылке
        /// </summary>
        public string LinkText { get; set; }

        /// <summary>
        /// Сообщение которое будет отображено
        /// </summary>
        public string Messege { get; set; }

        #endregion
    }
}
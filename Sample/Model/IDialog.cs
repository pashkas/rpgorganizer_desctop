// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDialog.cs" company="">
//   
// </copyright>
// <summary>
//   The Dialog interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    /// <summary>
    /// The Dialog interface.
    /// </summary>
    public interface IDialog
    {
        #region Public Properties

        /// <summary>
        /// Свойство для закрытия окна
        /// </summary>
        bool CloseSignal { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Метод закрытия окна
        /// </summary>
        void Close();

        #endregion
    }
}
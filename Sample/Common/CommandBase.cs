// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="">
//   
// </copyright>
// <summary>
//   The command base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DotNetLead.DragDrop.UI.Common
{
    /// <summary>
    /// The command base.
    /// </summary>
    public class CommandBase : ICommand
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// </summary>
        /// <param name="executeDelegate">
        /// The execute delegate.
        /// </param>
        /// <param name="canExecuteDelegate">
        /// The can execute delegate.
        /// </param>
        public CommandBase(Action<object> executeDelegate, Predicate<object> canExecuteDelegate)
        {
            this.execute = executeDelegate;
            this.canExecute = canExecuteDelegate;
        }

        #endregion

        #region Explicit Interface Events

        /// <summary>
        /// The can execute changed.
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// The can execute.
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// The execute.
        /// </summary>
        private readonly Action<object> execute;

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool ICommand.CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute(parameter);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        void ICommand.Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion
    }
}
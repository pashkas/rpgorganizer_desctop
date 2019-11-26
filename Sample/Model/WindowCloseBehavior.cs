// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowCloseBehavior.cs" company="">
//   
// </copyright>
// <summary>
//   Поведение, закрывающее окно
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Windows;

    /// <summary>
    /// Поведение, закрывающее окно
    /// </summary>
    public static class WindowCloseBehaviour
    {
        #region Public Methods and Operators

        /// <summary>
        /// The set close.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetClose(DependencyObject target, bool value)
        {
            target.SetValue(CloseProperty, value);
        }

        #endregion

        #region Static Fields

        /// <summary>
        /// The close property.
        /// </summary>
        public static readonly DependencyProperty CloseProperty = DependencyProperty.RegisterAttached(
            "Close",
            typeof(bool),
            typeof(WindowCloseBehaviour),
            new UIPropertyMetadata(false, OnClose));

        #endregion

        #region Methods

        /// <summary>
        /// The get window.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <returns>
        /// The <see cref="Window"/>.
        /// </returns>
        private static Window GetWindow(DependencyObject sender)
        {
            Window window = null;
            if (sender is Window)
            {
                window = (Window)sender;
            }

            if (window == null)
            {
                window = Window.GetWindow(sender);
            }

            return window;
        }

        /// <summary>
        /// The on close.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnClose(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool && ((bool)e.NewValue))
            {
                Window window = GetWindow(sender);
                if (window != null)
                {
                    window.Close();
                }
            }
        }

        #endregion
    }
}
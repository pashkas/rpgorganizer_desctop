// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinimizeToTray.cs" company="">
//   
// </copyright>
// <summary>
//   Class implementing support for "minimize to tray" functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Forms;

    /// <summary>
    /// Class implementing support for "minimize to tray" functionality.
    /// </summary>
    public static class MinimizeToTray
    {
        #region Public Methods and Operators

        /// <summary>
        /// Enables "minimize to tray" behavior for the specified Window.
        /// </summary>
        /// <param name="window">
        /// Window to enable the behavior for.
        /// </param>
        public static void Enable(Window window)
        {
            // No need to track this instance; its event handlers will keep it alive
            new MinimizeToTrayInstance(window);
        }

        #endregion

        /// <summary>
        /// Class implementing "minimize to tray" functionality for a Window instance.
        /// </summary>
        private class MinimizeToTrayInstance
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the MinimizeToTrayInstance class.
            /// </summary>
            /// <param name="window">
            /// Window instance to attach to.
            /// </param>
            public MinimizeToTrayInstance(Window window)
            {
                Debug.Assert(window != null, "window parameter is null.");
                this._window = window;
                this._window.StateChanged += new EventHandler(this.HandleStateChanged);
            }

            #endregion

            #region Fields

            /// <summary>
            /// The _balloon shown.
            /// </summary>
            private bool _balloonShown;

            /// <summary>
            /// The _notify icon.
            /// </summary>
            private NotifyIcon _notifyIcon;

            /// <summary>
            /// The _window.
            /// </summary>
            private Window _window;

            #endregion

            #region Methods

            /// <summary>
            /// Handles a click on the notify icon or its balloon.
            /// </summary>
            /// <param name="sender">
            /// Event source.
            /// </param>
            /// <param name="e">
            /// Event arguments.
            /// </param>
            private void HandleNotifyIconOrBalloonClicked(object sender, EventArgs e)
            {
                // Restore the Window
                this._window.WindowState = WindowState.Normal;
            }

            /// <summary>
            /// Handles the Window's StateChanged event.
            /// </summary>
            /// <param name="sender">
            /// Event source.
            /// </param>
            /// <param name="e">
            /// Event arguments.
            /// </param>
            private void HandleStateChanged(object sender, EventArgs e)
            {
                if (this._notifyIcon == null)
                {
                    // Initialize NotifyIcon instance "on demand"
                    this._notifyIcon = new NotifyIcon();
                    this._notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
                    this._notifyIcon.MouseClick += new MouseEventHandler(this.HandleNotifyIconOrBalloonClicked);
                    this._notifyIcon.BalloonTipClicked += new EventHandler(this.HandleNotifyIconOrBalloonClicked);
                }

                // Update copy of Window NameOfProperty in case it has changed
                this._notifyIcon.Text = this._window.Title;

                // Show/hide Window and NotifyIcon
                var minimized = this._window.WindowState == WindowState.Minimized;
                this._window.ShowInTaskbar = !minimized;
                this._notifyIcon.Visible = minimized;
                if (minimized && !this._balloonShown)
                {
                    // If this is the first time minimizing to the tray, show the user what happened
                    this._notifyIcon.ShowBalloonTip(1000, null, this._window.Title, ToolTipIcon.None);
                    this._balloonShown = true;
                }
            }

            #endregion
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementDragBehavior.cs" company="">
//   
// </copyright>
// <summary>
//   The framework element drag behavior.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DotNetLead.DragDrop.UI.Behavior
{
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// The framework element drag behavior.
    /// </summary>
    public class FrameworkElementDragBehavior : Behavior<FrameworkElement>
    {
        #region Fields

        /// <summary>
        /// The is mouse clicked.
        /// </summary>
        private bool isMouseClicked = false;

        #endregion

        #region Methods

        /// <summary>
        /// The on attached.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown +=
                new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.PreviewMouseLeftButtonUp +=
                new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonUp);
            this.AssociatedObject.MouseLeave += new MouseEventHandler(this.AssociatedObject_MouseLeave);
        }

        /// <summary>
        /// The associated object_ mouse leave.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.isMouseClicked)
            {
                // set the item's DataContext as the data to be transferred
                IDragable dragObject = this.AssociatedObject.DataContext as IDragable;
                if (dragObject != null)
                {
                    DataObject data = new DataObject();
                    data.SetData(dragObject.DataType, this.AssociatedObject.DataContext);
                    System.Windows.DragDrop.DoDragDrop(this.AssociatedObject, data, DragDropEffects.Move);
                    //Messenger.Default.Send<string>("Обновить активные задачи!");
                }
            }

            this.isMouseClicked = false;
        }

        /// <summary>
        /// The associated object_ mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.isMouseClicked = true;
        }

        /// <summary>
        /// The associated object_ mouse left button up.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isMouseClicked = false;
        }

        #endregion
    }
}
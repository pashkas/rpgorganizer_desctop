// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementDropBehavior.cs" company="">
//   
// </copyright>
// <summary>
//   The framework element drop behavior.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;

namespace DotNetLead.DragDrop.UI.Behavior
{
    /// <summary>
    /// The framework element drop behavior.
    /// </summary>
    public class FrameworkElementDropBehavior : Behavior<FrameworkElement>
    {
        #region Fields

        /// <summary>
        /// The adorner.
        /// </summary>
        private FrameworkElementAdorner adorner;

        /// <summary>
        /// The data type.
        /// </summary>
        private Type dataType; // the type of the data that can be dropped into this control

        #endregion

        #region Methods

        /// <summary>
        /// The on attached.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.AllowDrop = true;
            this.AssociatedObject.DragEnter += new DragEventHandler(this.AssociatedObject_DragEnter);
            this.AssociatedObject.DragOver += new DragEventHandler(this.AssociatedObject_DragOver);
            this.AssociatedObject.DragLeave += new DragEventHandler(this.AssociatedObject_DragLeave);
            this.AssociatedObject.Drop += new DragEventHandler(this.AssociatedObject_Drop);
        }

        /// <summary>
        /// The associated object_ drag enter.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            // if the DataContext implements IDropable, record the data type that can be dropped
            if (this.dataType == null)
            {
                if (this.AssociatedObject.DataContext != null)
                {
                    IDropable dropObject = this.AssociatedObject.DataContext as IDropable;
                    if (dropObject != null)
                    {
                        this.dataType = dropObject.DataType;
                    }
                }
            }

            if (this.adorner == null)
            {
                this.adorner = new FrameworkElementAdorner(sender as UIElement);
            }

            e.Handled = true;
        }

        /// <summary>
        /// The associated object_ drag leave.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            if (this.adorner != null)
            {
                this.adorner.Remove();
            }

            e.Handled = true;
        }

        /// <summary>
        /// The associated object_ drag over.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            if (this.dataType != null)
            {
                // if item can be dropped
                if (e.Data.GetDataPresent(this.dataType))
                {
                    // give mouse effect
                    this.SetDragDropEffects(e);

                    // draw the dots
                    if (this.adorner != null)
                    {
                        this.adorner.Update();
                    }
                }
            }

            e.Handled = true;
        }

        /// <summary>
        /// The associated object_ drop.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            if (this.dataType != null)
            {
                // if the data type can be dropped 
                if (e.Data.GetDataPresent(this.dataType))
                {
                    // drop the data
                    IDropable target = this.AssociatedObject.DataContext as IDropable;
                    target.Drop(e.Data.GetData(this.dataType));

                    // remove the data from the source
                    IDragable source = e.Data.GetData(this.dataType) as IDragable;
                    source.Remove(e.Data.GetData(this.dataType));
                }
            }

            if (this.adorner != null)
            {
                this.adorner.Remove();
            }

            e.Handled = true;
            return;
        }

        /// <summary>
        /// Provides feedback on if the data can be dropped
        /// </summary>
        /// <param name="e">
        /// </param>
        private void SetDragDropEffects(DragEventArgs e)
        {
            e.Effects = DragDropEffects.None; // default to None

            // if the data type can be dropped 
            if (e.Data.GetDataPresent(this.dataType))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListBoxAdornerManager.cs" company="">
//   
// </copyright>
// <summary>
//   The list box adorner manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace DotNetLead.DragDrop.UI.Behavior
{
    /// <summary>
    /// The list box adorner manager.
    /// </summary>
    internal class ListBoxAdornerManager
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxAdornerManager"/> class.
        /// </summary>
        /// <param name="layer">
        /// The layer.
        /// </param>
        internal ListBoxAdornerManager(AdornerLayer layer)
        {
            this.adornerLayer = layer;
        }

        #endregion

        #region Fields

        /// <summary>
        /// The adorner.
        /// </summary>
        private ListBoxAdorner adorner;

        /// <summary>
        /// The adorner layer.
        /// </summary>
        private AdornerLayer adornerLayer;

        /// <summary>
        /// The should create new adorner.
        /// </summary>
        private bool shouldCreateNewAdorner = false;

        #endregion

        #region Methods

        /// <summary>
        /// Remove the adorner 
        /// </summary>
        internal void Clear()
        {
            if (this.adorner != null)
            {
                this.adorner.Remove();
                this.shouldCreateNewAdorner = true;
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="adornedElement">
        /// The adorned element.
        /// </param>
        /// <param name="isAboveElement">
        /// The is above element.
        /// </param>
        internal void Update(UIElement adornedElement, bool isAboveElement)
        {
            if (this.adorner != null && !this.shouldCreateNewAdorner)
            {
                // exit if nothing changed
                if (this.adorner.AdornedElement == adornedElement && this.adorner.IsAboveElement == isAboveElement)
                {
                    return;
                }
            }

            this.Clear();

            // draw new adorner
            this.adorner = new ListBoxAdorner(adornedElement, this.adornerLayer);
            this.adorner.IsAboveElement = isAboveElement;
            this.adorner.Update();
            this.shouldCreateNewAdorner = false;
        }

        #endregion
    }
}
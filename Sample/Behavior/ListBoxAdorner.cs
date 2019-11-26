// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListBoxAdorner.cs" company="">
//   
// </copyright>
// <summary>
//   The list box adorner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DotNetLead.DragDrop.UI.Behavior
{
    /// <summary>
    /// The list box adorner.
    /// </summary>
    internal class ListBoxAdorner : Adorner
    {
        #region Fields

        /// <summary>
        /// The adorner layer.
        /// </summary>
        private AdornerLayer adornerLayer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">
        /// The adorned element.
        /// </param>
        /// <param name="adornerLayer">
        /// The adorner layer.
        /// </param>
        public ListBoxAdorner(UIElement adornedElement, AdornerLayer adornerLayer)
            : base(adornedElement)
        {
            this.adornerLayer = adornerLayer;
            this.adornerLayer.Add(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether is above element.
        /// </summary>
        public bool IsAboveElement { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The remove.
        /// </summary>
        public void Remove()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update UI
        /// </summary>
        internal void Update()
        {
            this.adornerLayer.Update(this.AdornedElement);
            this.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// The on render.
        /// </summary>
        /// <param name="drawingContext">
        /// The drawing context.
        /// </param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            double width = this.AdornedElement.DesiredSize.Width;
            double height = this.AdornedElement.DesiredSize.Height;

            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Red);
            renderBrush.Opacity = 0.5;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.White), 1.5);
            double renderRadius = 5.0;

            if (this.IsAboveElement)
            {
                drawingContext.DrawEllipse(
                    renderBrush,
                    renderPen,
                    adornedElementRect.TopLeft,
                    renderRadius,
                    renderRadius);
                drawingContext.DrawEllipse(
                    renderBrush,
                    renderPen,
                    adornedElementRect.TopRight,
                    renderRadius,
                    renderRadius);
            }
            else
            {
                drawingContext.DrawEllipse(
                    renderBrush,
                    renderPen,
                    adornedElementRect.BottomLeft,
                    renderRadius,
                    renderRadius);
                drawingContext.DrawEllipse(
                    renderBrush,
                    renderPen,
                    adornedElementRect.BottomRight,
                    renderRadius,
                    renderRadius);
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementAdorner.cs" company="">
//   
// </copyright>
// <summary>
//   The framework element adorner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DotNetLead.DragDrop.UI.Behavior
{
    /// <summary>
    /// The framework element adorner.
    /// </summary>
    internal class FrameworkElementAdorner : Adorner
    {
        #region Fields

        /// <summary>
        /// The adorner layer.
        /// </summary>
        private AdornerLayer adornerLayer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkElementAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">
        /// The adorned element.
        /// </param>
        public FrameworkElementAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.adornerLayer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
            this.adornerLayer.Add(this);
        }

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
        /// The update.
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
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Red);
            renderBrush.Opacity = 0.5;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.White), 1.5);
            double renderRadius = 5.0;

            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
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

        #endregion
    }
}
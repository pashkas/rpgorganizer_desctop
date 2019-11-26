using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Sample.Behavior
{
    public class VerticalScrollBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObjectOnPreviewMouseWheel;
        }

        private void AssociatedObjectOnPreviewMouseWheel(object sender, MouseWheelEventArgs mouseWheelEventArgs)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            var delta = mouseWheelEventArgs.Delta;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - delta);
            mouseWheelEventArgs.Handled = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseWheel -= AssociatedObjectOnPreviewMouseWheel;
        }
    }



    public class VerticalScrollBehavior2 : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObjectOnPreviewMouseWheel;
        }

        private void AssociatedObjectOnPreviewMouseWheel(object sender, MouseWheelEventArgs mouseWheelEventArgs)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            var delta = mouseWheelEventArgs.Delta;
            if (delta > 0)
            {
                delta = 3;
            }
            else if (delta<0)
            {
                delta = -3;
            }


            scv.ScrollToVerticalOffset(scv.VerticalOffset - delta);
            mouseWheelEventArgs.Handled = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseWheel -= AssociatedObjectOnPreviewMouseWheel;
        }
    }
}

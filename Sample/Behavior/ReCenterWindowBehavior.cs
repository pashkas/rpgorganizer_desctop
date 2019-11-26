using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Behavior
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interactivity;

    using Sample.Model;

    using Button = System.Windows.Controls.Button;

    public class ReCenterWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MinWidth = 800;
            AssociatedObject.MinHeight = 600;
            AssociatedObject.MaxHeight = SystemParameters.WorkArea.Height;
            AssociatedObject.Width = SystemParameters.WorkArea.Width - 180;

            AssociatedObject.AddHandler(Window.SizeChangedEvent, new RoutedEventHandler(WindowResize), true);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(Window.SizeChangedEvent, new RoutedEventHandler(WindowResize));
        }

        private void WindowResize(object sender, RoutedEventArgs e)
        {
            System.Windows.Window window = (System.Windows.Window)sender;

            StaticMetods.CenterWindow(window);
        }
    }
}
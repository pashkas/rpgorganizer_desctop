using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Sample.Model;

namespace Sample.Behavior
{
    public class PlayEditBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObjectOnSourceInitialized;

            //AssociatedObject.AddHandler(Window.LoadedEvent, new RoutedEventHandler(WindowLoaded), true);
        }

      
        private void AssociatedObjectOnSourceInitialized(object sender, RoutedEventArgs e)
        {
            StaticMetods.PlaySound(Properties.Resources.edit);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObjectOnSourceInitialized;

        }
    }
}

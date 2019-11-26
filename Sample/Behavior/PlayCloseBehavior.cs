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
    public class PlayCloseBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
           
            AssociatedObject.Closing += AssociatedObjectOnClosing;
            //AssociatedObject.AddHandler(Window.LoadedEvent, new RoutedEventHandler(WindowLoaded), true);
        }

        private void AssociatedObjectOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            StaticMetods.PlaySound(Properties.Resources.doorClose);
        }

     

        protected override void OnDetaching()
        {
            base.OnDetaching();
            
            AssociatedObject.Closing -= AssociatedObjectOnClosing;
        }
    }
}

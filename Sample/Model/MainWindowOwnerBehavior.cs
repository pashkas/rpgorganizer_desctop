using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Messaging;

namespace Sample.Model
{
    public class MainWindowOwnerBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.ShowInTaskbar = false;
            Messenger.Default.Send<Window>(this.AssociatedObject);
        }

    

     
    }
}

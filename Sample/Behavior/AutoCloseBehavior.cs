using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace Sample.Behavior
{
    public class AutoCloseBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 6) };
            timer.Tick += (o, args) =>
            {
                timer.Stop();
                AssociatedObject.Close();
            };
            timer.Start();
        }
    }
}

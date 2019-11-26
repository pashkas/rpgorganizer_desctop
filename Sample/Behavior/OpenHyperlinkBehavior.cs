using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Sample.Behavior
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Interactivity;

    using Sample.ViewModel;

    public class OpenHyperlinkBehavior : Behavior<Hyperlink>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.Click += AssociatedObject_Click;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Click -= AssociatedObject_Click;
        }

        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            var navigateUri = this.AssociatedObject.NavigateUri;
            if (navigateUri != null && !string.IsNullOrEmpty(navigateUri.ToString()))
            {
                var uri = navigateUri.ToString();

                MainViewModel.OpenLink(uri);
            }
        }
    }

    public class OpenHyperlinkButtonBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.Click += AssociatedObject_Click;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Click -= AssociatedObject_Click;
        }

        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            var navigateUri = AssociatedObject.CommandParameter;
            if (navigateUri != null && !string.IsNullOrEmpty(navigateUri.ToString()))
            {
                var uri = navigateUri.ToString();
                MainViewModel.OpenLink(uri);
            }
        }
    }



}
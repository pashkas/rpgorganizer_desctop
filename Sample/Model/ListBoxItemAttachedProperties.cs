namespace Sample.Model
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static class ListBoxItemAttachedProperties
    {
        #region IsSelectableProperty

        public static readonly DependencyProperty IsSelectableProperty =
            DependencyProperty.RegisterAttached(
                "IsSelectable",
                typeof(bool),
                typeof(ListBoxItemAttachedProperties),
                new UIPropertyMetadata(true, IsSelectableChanged));

        public static bool GetIsSelectable(ListBoxItem obj)
        {
            return (bool)obj.GetValue(IsSelectableProperty);
        }

        public static void SetIsSelectable(ListBoxItem obj, bool value)
        {
            obj.SetValue(IsSelectableProperty, value);
        }

        private static void IsSelectableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBoxItem item = d as ListBoxItem;
            if (item == null)
            {
                return;
            }

            if ((bool)e.NewValue == false && (bool)e.OldValue == true)
            {
                item.Selected -= new RoutedEventHandler(item_Selected);
                item.Selected += new RoutedEventHandler(item_Selected);
                BindingOperations.ClearBinding(item, ListBoxItem.IsSelectedProperty);

                if (item.IsSelected)
                {
                    item.IsSelected = false;
                }
            }
            else if ((bool)e.NewValue == true && (bool)e.OldValue == false)
            {
                item.Selected -= new RoutedEventHandler(item_Selected);
            }
        }

        private static void item_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;
            if (item == null)
            {
                return;
            }

            item.IsSelected = false;
        }

        #endregion IsSelectableProperty
    }
}
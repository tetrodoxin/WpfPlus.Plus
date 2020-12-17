using System.Windows;
using System.Windows.Controls;

namespace WpfPlus
{
    public static class TabExt
    {
        public static readonly DependencyProperty ClickSelectBehaviorProperty =
            DependencyProperty.RegisterAttached("ClickSelectBehavior", typeof(bool), typeof(TabExt), new PropertyMetadata(false, handleClickSelectBehaviorPropertyChanged));

        public static bool GetClickSelectBehavior(TabItem obj) => (bool)obj.GetValue(ClickSelectBehaviorProperty);

        public static void SetClickSelectBehavior(TabItem obj, bool value) => obj.SetValue(ClickSelectBehaviorProperty, value);

        private static void handleClickSelectBehaviorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is bool shallSetBehaviour && shallSetBehaviour)
            {
                var ti = (TabItem)obj;
                ti.PreviewMouseLeftButtonDown += handleTabItemMouseDown;
            }
        }

        private static void handleTabItemMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var ti = (TabItem)sender;
            ti.IsSelected = true;
            e.Handled = true;
        }
    }
}
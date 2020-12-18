using System.Windows;
using System.Windows.Controls;
using WpfPlus.Behaviors;

namespace WpfPlus.Controls
{
    public static class ControlExt
    {
        public static Window GetDragAreaForWindow(UIElement obj) => (Window)obj.GetValue(DragAreaForWindowProperty);

        public static void SetDragAreaForWindow(UIElement obj, Window value) => obj.SetValue(DragAreaForWindowProperty, value);

        public static readonly DependencyProperty DragAreaForWindowProperty =
            DependencyProperty.RegisterAttached("DragAreaForWindow", typeof(Window), typeof(ControlExt), new PropertyMetadata(null, handleDragAreaForWindowPropertyChanged));

        private static void handleDragAreaForWindowPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var element = (UIElement)obj;
            if(args.NewValue is Window window)
            {
                var dragBehavior = new DragWindowAreaBehavior { Window = window };
                Behaviors.AddSingularBehaviorTo(element, dragBehavior);                
            }
            else
            {
                Behaviors.RemoveBehaviorTypeFrom<UIElement, DragWindowAreaBehavior>(element);
            }
        }
    }
}
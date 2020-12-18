using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace WpfPlus.Behaviors
{
    /// <summary>
    /// Behavior to move (drag) a Window, when the mouse is clicked-down on a certain UI-Element,
    /// like moving a window while "holding" its title bar.
    /// </summary>
    public class DragWindowAreaBehavior : Behavior<UIElement>
    {
        public Window Window
        {
            get { return (Window)GetValue(WindowProperty); }
            set { SetValue(WindowProperty, value); }
        }

        public static readonly DependencyProperty WindowProperty =
            DependencyProperty.Register("Window", typeof(Window), typeof(DragWindowAreaBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseDown += handleMouseDown; ;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseDown -= handleMouseDown; ;
        }

        private void handleMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Window?.DragMove();
            }
        }
    }
}
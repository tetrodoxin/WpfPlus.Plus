using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfPlus.Behaviors
{
    /// <summary>
    /// Behavior to prevent stop scrolling, when mouse is over a scrollviewer
    /// inside of a scrollable area.
    /// </summary>
    public class ScrollFixBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += handlePreviewMouseWheel;
        }

        private static void handlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled && sender != null)
            {
                var viewer = sender as ScrollViewer;
                viewer.PreviewMouseWheel -= handlePreviewMouseWheel;
                var originalSource = e.OriginalSource as UIElement;
                originalSource?.RaiseEvent(e);
                viewer.PreviewMouseWheel += handlePreviewMouseWheel;

                if (!e.Handled && !((e.Delta > 0 && viewer.VerticalOffset == 0)
                || (e.Delta <= 0 && viewer.VerticalOffset >= viewer.ExtentHeight - viewer.ViewportHeight)))
                {
                    e.Handled = true;
                    var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                    eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                    eventArg.Source = e.OriginalSource;
                    var us = (UIElement)((FrameworkElement)sender);
                    us.RaiseEvent(eventArg);
                }
            }
        }
    }
}
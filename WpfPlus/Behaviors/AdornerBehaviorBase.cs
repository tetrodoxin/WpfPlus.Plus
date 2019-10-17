using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfPlus.Behaviors
{
    public abstract class AdornerBehaviorBase<TElement, TAdorner> : Behavior<TElement>
        where TElement : FrameworkElement
        where TAdorner : Adorner
    {
        private TAdorner _adorner = null;
        private AdornerLayer _layer = null;

        public static readonly DependencyProperty AdornerParentProperty =
            DependencyProperty.Register("AdornerParent", typeof(Visual), typeof(AdornerBehaviorBase<TElement, TAdorner>), new PropertyMetadata(null, onAdornerParentPropertyChanged));

        public Visual AdornerParent
        {
            get { return (Visual)GetValue(AdornerParentProperty); }
            set { SetValue(AdornerParentProperty, value); }
        }

        protected TAdorner Adorner
        {
            get
            {
                if (AssociatedObject != null && _adorner == null)
                {
                    if (_layer == null)
                    {
                        initLayer();
                    }

                    if (_layer != null)
                    {
                        initAdorner();
                    }
                }

                return _adorner;
            }
        }

        protected bool IsAdornerVisible { get; private set; } = false;

        protected abstract bool ShouldShowAdorner { get; }

        protected abstract TAdorner CreateAdorner();

        protected override void OnAttached()
        {
            base.OnAttached();

            initLayer();
            if (_layer != null)
            {
                initAdorner();
            }
            else
            {
                // defer attaching to after window is loaded
                AssociatedObject.Loaded += handleAssociatedObject_Loaded;
            }
        }

        protected void UpdateAdornerVisibility()
        {
            if (_layer == null || Adorner == null)
            {
                return;
            }

            if (ShouldShowAdorner != IsAdornerVisible)
            {
                // now toggle visibility
                if (IsAdornerVisible)
                {
                    detachAdorner();
                }
                else
                {
                    attachAdorner();
                }
            }
            else
            {
            }
        }

        private static void onAdornerParentPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var bhv = (AdornerBehaviorBase<TElement, TAdorner>)obj;
            if (bhv.AdornerParent != null && bhv._adorner != null)
            {
                var doAttach = bhv.IsAdornerVisible;
                if (bhv._layer != null)
                {
                    bhv.detachAdorner();
                }
                bhv.initLayer();
                if (bhv._layer != null && doAttach)
                {
                    bhv.attachAdorner();
                }
            }
        }

        private void attachAdorner()
        {
            _layer.Add(Adorner);
            IsAdornerVisible = true;
        }

        private void detachAdorner()
        {
            _layer.Remove(Adorner);
            IsAdornerVisible = false;
        }

        private void handleAssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= handleAssociatedObject_Loaded;

            initLayer();
            if (_layer != null)
            {
                initAdorner();
                UpdateAdornerVisibility();
            }
        }

        private void initAdorner()
        {
            _adorner = CreateAdorner();
        }

        private void initLayer()
        {
            if (AdornerParent == null)
            {
                _layer = AdornerLayer.GetAdornerLayer(AssociatedObject);
            }
            else
            {
                _layer = AdornerLayer.GetAdornerLayer(AdornerParent);
            }
        }
    }
}
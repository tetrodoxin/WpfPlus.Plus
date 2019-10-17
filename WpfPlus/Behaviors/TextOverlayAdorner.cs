using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfPlus.Behaviors
{
    public class TextOverlayAdorner : Adorner
    {
        private ContentPresenter _contentPresenter;
        private TextBlock _textblock;
        private VisualCollection _visuals;

        #region Text

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextOverlayAdorner), new FrameworkPropertyMetadata(null, onTextPropertyChanged) { AffectsRender = true, AffectsMeasure = true });

        private static void onTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var ad = (TextOverlayAdorner)obj;
            ad.updateText();
        }

        #endregion Text

        #region TextMargin

        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }

        public static readonly DependencyProperty TextMarginProperty =
            DependencyProperty.Register("TextMargin", typeof(Thickness), typeof(TextOverlayAdorner), new FrameworkPropertyMetadata(new Thickness(4, 3, 0, 0), onTextMarginPropertyChanged) { AffectsRender = true, AffectsMeasure = true });

        private static void onTextMarginPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var ad = (TextOverlayAdorner)obj;
            ad._textblock.Margin = (Thickness)args.NewValue;
        }

        #endregion TextMargin

        #region TextColor

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(Brush), typeof(TextOverlayAdorner), new FrameworkPropertyMetadata(onTextColorPropertyChanged) { AffectsRender = true });

        public Brush TextColor
        {
            get { return (Brush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        private static void onTextColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextOverlayAdorner)obj;
            a._textblock.Foreground = (Brush)args.NewValue;
        }

        #endregion TextColor

        #region FontFamily

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(TextOverlayAdorner), new FrameworkPropertyMetadata(onFontFamilyPropertyChanged) { AffectsRender = true, AffectsArrange = true, AffectsMeasure = true });

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        private static void onFontFamilyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextOverlayAdorner)obj;
            a._textblock.FontFamily = (FontFamily)args.NewValue;
        }

        #endregion FontFamily

        #region FontSize

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(TextOverlayAdorner), new FrameworkPropertyMetadata(onFontSizePropertyChanged) { AffectsRender = true, AffectsArrange = true, AffectsMeasure = true });

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        private static void onFontSizePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextOverlayAdorner)obj;
            a._textblock.FontSize = (double)args.NewValue;
        }

        #endregion FontSize

        #region HorizontalTextAlignment

        public static readonly DependencyProperty HorizontalTextAlignmentProperty =
            DependencyProperty.Register("HorizontalTextAlignment", typeof(HorizontalAlignment), typeof(TextOverlayAdorner), new FrameworkPropertyMetadata(HorizontalAlignment.Left, onHorizontalTextAlignmentPropertyChanged) { AffectsRender = true, AffectsArrange = true, AffectsMeasure = true });

        public HorizontalAlignment HorizontalTextAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }

        private static void onHorizontalTextAlignmentPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextOverlayAdorner)obj;
            a._textblock.HorizontalAlignment = (HorizontalAlignment)args.NewValue;
        }

        #endregion HorizontalTextAlignment

        #region TextFontWeight

        public FontWeight TextFontWeight
        {
            get { return (FontWeight)GetValue(TextFontWeightProperty); }
            set { SetValue(TextFontWeightProperty, value); }
        }

        public static readonly DependencyProperty TextFontWeightProperty =
            DependencyProperty.Register("TextFontWeight", typeof(FontWeight), typeof(TextOverlayAdorner), new PropertyMetadata(FontWeights.Normal, onTextFontWeightPropertyChanged));

        private static void onTextFontWeightPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextOverlayAdorner)obj;
            a._textblock.FontWeight = (FontWeight)args.NewValue;
        }

        #endregion TextFontWeight

        #region TextFontStyle

        public FontStyle TextFontStyle
        {
            get { return (FontStyle)GetValue(TextFontStyleProperty); }
            set { SetValue(TextFontStyleProperty, value); }
        }

        public static readonly DependencyProperty TextFontStyleProperty =
            DependencyProperty.Register("TextFontStyle", typeof(FontStyle), typeof(TextOverlayAdorner), new PropertyMetadata(FontStyles.Normal, onTextFontStylePropertyChanged));

        private static void onTextFontStylePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextOverlayAdorner)obj;
            a._textblock.FontStyle = (FontStyle)args.NewValue;
        }

        #endregion TextFontStyle

        private void updateText()
        {
            _textblock.Inlines.Clear();

            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            _textblock.Inlines.Add(new Run() { Text = Text });
        }

        protected override int VisualChildrenCount
        {
            get { return _visuals.Count; }
        }

        public TextOverlayAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            _visuals = new VisualCollection(this);
            var control = AdornedElement as Control;
            IsHitTestVisible = false;  // make adorner transparent for mouse events

            _textblock = new TextBlock()
            {
                Margin = new Thickness(0),
                Padding = TextMargin,
                HorizontalAlignment = HorizontalTextAlignment,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = TextFontWeight,
                FontStyle = TextFontStyle,
            };

            _contentPresenter = new ContentPresenter()
            {
                Content = _textblock,
                Width = control.Width,
                Height = control.Height,
            };

            _visuals.Add(_contentPresenter);

            control.SizeChanged += control_SizeChanged;
        }

        private void control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var control = AdornedElement as Control;
            _contentPresenter.Width = control.ActualWidth;
            _contentPresenter.Height = control.ActualHeight;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));

            return _contentPresenter.RenderSize;
        }

        protected override Visual GetVisualChild(int index) => _visuals[index];

        protected override Size MeasureOverride(Size constraint)
        {
            _contentPresenter.Measure(constraint);
            return _contentPresenter.DesiredSize;
        }
    }
}
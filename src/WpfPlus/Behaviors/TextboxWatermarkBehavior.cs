using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfPlus.Behaviors
{
    public class TextboxWatermarkBehavior : AdornerBehaviorBase<TextBox, TextOverlayAdorner>
    {
        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextboxWatermarkBehavior), new PropertyMetadata(null, onTextPropertyChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void onTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var bhv = (TextboxWatermarkBehavior)obj;
            bhv.updateText();
        }

        #endregion Text

        #region Suggestion

        public static readonly DependencyProperty SuggestionProperty =
            DependencyProperty.Register("Suggestion", typeof(string), typeof(TextboxWatermarkBehavior), new PropertyMetadata(null, onSuggestionPropertyChanged, coerceSuggestionProperty));

        public string? Suggestion
        {
            get { return (string)GetValue(SuggestionProperty); }
            set { SetValue(SuggestionProperty, value); }
        }

        private static void onSuggestionPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var bhv = (TextboxWatermarkBehavior)obj;
            bhv.updateText();
        }

        private static object? coerceSuggestionProperty(DependencyObject obj, object value) => value ?? string.Empty;

        #endregion Suggestion

        #region SuggestionColor

        public static readonly DependencyProperty SuggestionColorProperty =
            DependencyProperty.Register("SuggestionColor", typeof(Brush), typeof(TextboxWatermarkBehavior), new FrameworkPropertyMetadata(onSuggestionColorPropertyChanged) { AffectsRender = true });

        public Brush? SuggestionColor
        {
            get { return (Brush)GetValue(SuggestionColorProperty); }
            set { SetValue(SuggestionColorProperty, value); }
        }

        private static void onSuggestionColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextboxWatermarkBehavior)obj;
            a.updateText();
        }

        #endregion SuggestionColor

        #region TextMargin

        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }

        public static readonly DependencyProperty TextMarginProperty =
            DependencyProperty.Register("TextMargin", typeof(Thickness), typeof(TextboxWatermarkBehavior), new FrameworkPropertyMetadata(new Thickness(0), onTextMarginPropertyChanged) { AffectsRender = true, AffectsMeasure = true });

        private static void onTextMarginPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var ad = (TextboxWatermarkBehavior)obj;
            if (ad.Adorner != null)
            {
                ad.Adorner.TextMargin = (Thickness)args.NewValue;
            }
        }

        #endregion TextMargin

        #region TextColor

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(Brush), typeof(TextboxWatermarkBehavior), new FrameworkPropertyMetadata(onTextColorPropertyChanged) { AffectsRender = true });

        public Brush? TextColor
        {
            get { return (Brush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        private static void onTextColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextboxWatermarkBehavior)obj;
            a.updateText();
        }

        #endregion TextColor

        #region FontFamily

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(TextboxWatermarkBehavior), new FrameworkPropertyMetadata(onFontFamilyPropertyChanged) { AffectsRender = true, AffectsArrange = true, AffectsMeasure = true });

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        private static void onFontFamilyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextboxWatermarkBehavior)obj;
            if (a.Adorner != null)
            {
                a.Adorner.FontFamily = (FontFamily)args.NewValue;
            }
        }

        #endregion FontFamily

        #region FontSize

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(TextboxWatermarkBehavior), new FrameworkPropertyMetadata(onFontSizePropertyChanged) { AffectsRender = true, AffectsArrange = true, AffectsMeasure = true });

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        private static void onFontSizePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextboxWatermarkBehavior)obj;
            if (a.Adorner != null)
            {
                a.Adorner.FontSize = (double)args.NewValue;
            }
        }

        #endregion FontSize

        #region HorizontalTextAlignment

        public static readonly DependencyProperty HorizontalTextAlignmentProperty =
            DependencyProperty.Register("HorizontalTextAlignment", typeof(HorizontalAlignment), typeof(TextboxWatermarkBehavior), new FrameworkPropertyMetadata(HorizontalAlignment.Left, onHorizontalTextAlignmentPropertyChanged));

        public HorizontalAlignment HorizontalTextAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }

        private static void onHorizontalTextAlignmentPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextboxWatermarkBehavior)obj;
            if (a.Adorner != null)
            {
                a.Adorner.HorizontalTextAlignment = (HorizontalAlignment)args.NewValue;
            }
        }

        #endregion HorizontalTextAlignment

        #region TextFontWeight

        public FontWeight TextFontWeight
        {
            get { return (FontWeight)GetValue(TextFontWeightProperty); }
            set { SetValue(TextFontWeightProperty, value); }
        }

        public static readonly DependencyProperty TextFontWeightProperty =
            DependencyProperty.Register("TextFontWeight", typeof(FontWeight), typeof(TextboxWatermarkBehavior), new PropertyMetadata(FontWeights.Normal, onTextFontWeightPropertyChanged));

        private static void onTextFontWeightPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextboxWatermarkBehavior)obj;
            if (a.Adorner != null)
            {
                a.Adorner.TextFontWeight = (FontWeight)args.NewValue;
            }
        }

        #endregion TextFontWeight

        #region TextFontStyle

        public FontStyle TextFontStyle
        {
            get { return (FontStyle)GetValue(TextFontStyleProperty); }
            set { SetValue(TextFontStyleProperty, value); }
        }

        public static readonly DependencyProperty TextFontStyleProperty =
            DependencyProperty.Register("TextFontStyle", typeof(FontStyle), typeof(TextboxWatermarkBehavior), new PropertyMetadata(FontStyles.Normal, onTextFontStylePropertyChanged));

        private static void onTextFontStylePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var a = (TextboxWatermarkBehavior)obj;
            if (a.Adorner != null)
            {
                a.Adorner.TextFontStyle = (FontStyle)args.NewValue;
            }
        }

        #endregion TextFontStyle

        protected override bool ShouldShowAdorner => string.IsNullOrEmpty(AssociatedObject.Text);

        protected override TextOverlayAdorner CreateAdorner()
        {
            var adorner = new TextOverlayAdorner(AssociatedObject)
            {
                Text = Text,
                TextMargin = TextMargin,
                TextColor = TextColor,
                TextFontStyle = TextFontStyle,
                TextFontWeight = TextFontWeight,
                FontFamily = FontFamily,
                FontSize = FontSize,
                HorizontalTextAlignment = HorizontalTextAlignment
            };

            return adorner;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.TextChanged += handleTextBox_TextChanged;
        }

        private void handleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAdornerVisibility();
        }

        private void updateText()
        {
            if (Adorner == null) return;
            if (!string.IsNullOrWhiteSpace(Suggestion))
            {
                Adorner.Text = Suggestion;
                Adorner.TextColor = SuggestionColor;
                Adorner.HorizontalTextAlignment = HorizontalAlignment.Left;
            }
            else if (!string.IsNullOrWhiteSpace(Text))
            {
                Adorner.Text = Text;
                Adorner.TextColor = TextColor;
                Adorner.HorizontalTextAlignment = HorizontalTextAlignment;
                Adorner.TextFontStyle = TextFontStyle;
                Adorner.TextFontWeight = TextFontWeight;
            }
            else
            {
                Adorner.Text = null;
            }

            UpdateAdornerVisibility();
        }
    }
}
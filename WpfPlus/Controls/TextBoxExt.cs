using Microsoft.Xaml.Behaviors;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfPlus.Behaviors;

namespace WpfPlus.Controls
{
    public static class TextBoxExt
    {
        public static readonly DependencyProperty WatermarkFontStyleProperty =
            DependencyProperty.RegisterAttached("WatermarkFontStyle", typeof(FontStyle), typeof(TextBoxExt), new PropertyMetadata(FontStyles.Normal, onWatermarkFontStyleChanged));

        public static readonly DependencyProperty WatermarkFontWeightProperty =
            DependencyProperty.RegisterAttached("WatermarkFontWeight", typeof(FontWeight), typeof(TextBoxExt), new PropertyMetadata(FontWeights.Normal, onWatermarkFontWeightChanged));

        public static readonly DependencyProperty WatermarkHorizontalTextAlignmentProperty =
                            DependencyProperty.RegisterAttached("WatermarkHorizontalTextAlignment", typeof(HorizontalAlignment), typeof(TextBoxExt), new PropertyMetadata(HorizontalAlignment.Center, onWatermarkHorizontalTextAlignmentPropertyChanged));

        public static readonly DependencyProperty WatermarkTextBrushProperty =
            DependencyProperty.RegisterAttached("WatermarkTextBrush", typeof(Brush), typeof(TextBoxExt), new PropertyMetadata(null, onWatermarkTextBrushChanged));

        public static readonly DependencyProperty WatermarkTextMarginProperty =
                    DependencyProperty.RegisterAttached("WatermarkTextMargin", typeof(Thickness), typeof(TextBoxExt), new PropertyMetadata(new Thickness(3, 3, 0, 0), onWatermarkTextMarginPropertyChanged));

        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.RegisterAttached("WatermarkText", typeof(string), typeof(TextBoxExt), new PropertyMetadata(null, onWatermarkTextPropertyChanged));

        static TextBoxExt()
        { }

        public static FontStyle GetWatermarkFontStyle(DependencyObject obj)
        {
            return (FontStyle)obj.GetValue(WatermarkFontStyleProperty);
        }

        public static FontWeight GetWatermarkFontWeight(DependencyObject obj)
        {
            return (FontWeight)obj.GetValue(WatermarkFontWeightProperty);
        }

        public static HorizontalAlignment GetWatermarkHorizontalTextAlignment(DependencyObject obj)
        {
            return (HorizontalAlignment)obj.GetValue(WatermarkHorizontalTextAlignmentProperty);
        }

        public static string GetWatermarkText(TextBox obj)
        {
            return (string)obj.GetValue(WatermarkTextProperty);
        }

        public static Brush GetWatermarkTextBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(WatermarkTextBrushProperty);
        }

        public static Thickness GetWatermarkTextMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(WatermarkTextMarginProperty);
        }

        public static void SetWatermarkFontStyle(DependencyObject obj, FontStyle value)
        {
            obj.SetValue(WatermarkFontStyleProperty, value);
        }

        public static void SetWatermarkFontWeight(DependencyObject obj, FontWeight value)
        {
            obj.SetValue(WatermarkFontWeightProperty, value);
        }

        public static void SetWatermarkHorizontalTextAlignment(DependencyObject obj, HorizontalAlignment value)
        {
            obj.SetValue(WatermarkHorizontalTextAlignmentProperty, value);
        }

        public static void SetWatermarkText(TextBox obj, string value)
        {
            obj.SetValue(WatermarkTextProperty, value);
        }

        public static void SetWatermarkTextBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(WatermarkTextBrushProperty, value);
        }

        public static void SetWatermarkTextMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(WatermarkTextMarginProperty, value);
        }

        private static Behavior<T> addSingularBehaviorTo<T>(T target, Behavior<T> behavior) where T : DependencyObject
        {
            if ((object)target == null || behavior == null) return null;

            var behaviors = Interaction.GetBehaviors(target);
            var tp = behavior.GetType();

            var existing = behaviors.FirstOrDefault(p => p.GetType().Equals(tp)) as Behavior<T>;

            if (existing == null)
            {
                behaviors.Add(behavior);
                existing = behavior;
            }

            return existing;
        }

        private static TextboxWatermarkBehavior createWatermarkBehavior(FrameworkElement c)
        {
            var textcolor = GetWatermarkTextBrush(c);
            var suggestionColor = c.FindResource("SelectionColor2EmphasizedBrush") as Brush;

            if (textcolor == null)
            {
                textcolor = c.FindResource("TextBoxDisabledForegroundBrush") as Brush;
                SetWatermarkTextBrush(c, textcolor);
            }

            return new TextboxWatermarkBehavior()
            {
                TextColor = textcolor,
                SuggestionColor = suggestionColor,
                HorizontalTextAlignment = HorizontalAlignment.Center
            };
        }

        private static void onWatermarkFontStyleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox != null)
            {
                if (e.NewValue is FontStyle newValue)
                {
                    var behavior = createWatermarkBehavior(textBox);

                    behavior = (TextboxWatermarkBehavior)addSingularBehaviorTo(textBox, behavior);
                    behavior.TextFontStyle = newValue;
                }
                else
                {
                    var behaviors = Microsoft.Xaml.Behaviors.Interaction.GetBehaviors(textBox);
                    var toDelete = behaviors.OfType<TextboxWatermarkBehavior>().FirstOrDefault();
                    if (toDelete != null && string.IsNullOrWhiteSpace(toDelete.Suggestion)) behaviors.Remove(toDelete);
                }
            }
        }

        private static void onWatermarkFontWeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox != null)
            {
                if (e.NewValue is FontWeight newValue)
                {
                    var behavior = createWatermarkBehavior(textBox);

                    behavior = (TextboxWatermarkBehavior)addSingularBehaviorTo(textBox, behavior);
                    behavior.TextFontWeight = newValue;
                }
                else
                {
                    var behaviors = Microsoft.Xaml.Behaviors.Interaction.GetBehaviors(textBox);
                    var toDelete = behaviors.OfType<TextboxWatermarkBehavior>().FirstOrDefault();
                    if (toDelete != null && string.IsNullOrWhiteSpace(toDelete.Suggestion)) behaviors.Remove(toDelete);
                }
            }
        }

        private static void onWatermarkHorizontalTextAlignmentPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox != null)
            {
                if (e.NewValue is HorizontalAlignment newValue)
                {
                    var behavior = createWatermarkBehavior(textBox);

                    behavior = (TextboxWatermarkBehavior)addSingularBehaviorTo(textBox, behavior);
                    behavior.HorizontalTextAlignment = newValue;
                }
                else
                {
                    var behaviors = Microsoft.Xaml.Behaviors.Interaction.GetBehaviors(textBox);
                    var toDelete = behaviors.OfType<TextboxWatermarkBehavior>().FirstOrDefault();
                    if (toDelete != null && string.IsNullOrWhiteSpace(toDelete.Suggestion)) behaviors.Remove(toDelete);
                }
            }
        }

        private static void onWatermarkTextBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox != null)
            {
                if (e.NewValue is Brush newValue)
                {
                    var behavior = createWatermarkBehavior(textBox);

                    behavior = (TextboxWatermarkBehavior)addSingularBehaviorTo(textBox, behavior);
                    behavior.TextColor = newValue;
                }
                else
                {
                    var behaviors = Microsoft.Xaml.Behaviors.Interaction.GetBehaviors(textBox);
                    var toDelete = behaviors.OfType<TextboxWatermarkBehavior>().FirstOrDefault();
                    if (toDelete != null && string.IsNullOrWhiteSpace(toDelete.Suggestion)) behaviors.Remove(toDelete);
                }
            }
        }

        private static void onWatermarkTextMarginPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox != null)
            {
                if (e.NewValue is Thickness newValue)
                {
                    var behavior = createWatermarkBehavior(textBox);

                    behavior = (TextboxWatermarkBehavior)addSingularBehaviorTo(textBox, behavior);
                    behavior.TextMargin = newValue;
                }
                else
                {
                    var behaviors = Microsoft.Xaml.Behaviors.Interaction.GetBehaviors(textBox);
                    var toDelete = behaviors.OfType<TextboxWatermarkBehavior>().FirstOrDefault();
                    if (toDelete != null && string.IsNullOrWhiteSpace(toDelete.Suggestion)) behaviors.Remove(toDelete);
                }
            }
        }

        private static void onWatermarkTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox != null)
            {
                if (string.IsNullOrEmpty(e.NewValue as string))
                {
                    var behaviors = Microsoft.Xaml.Behaviors.Interaction.GetBehaviors(textBox);
                    var toDelete = behaviors.OfType<TextboxWatermarkBehavior>().FirstOrDefault();
                    if (toDelete != null && string.IsNullOrWhiteSpace(toDelete.Suggestion)) behaviors.Remove(toDelete);
                }
                else
                {
                    var behavior = createWatermarkBehavior(textBox);

                    behavior = (TextboxWatermarkBehavior)addSingularBehaviorTo(textBox, behavior);
                    behavior.Text = (string)e.NewValue;
                }
            }
        }
    }
}
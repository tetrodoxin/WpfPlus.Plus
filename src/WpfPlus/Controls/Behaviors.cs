using Microsoft.Xaml.Behaviors;
using System.Linq;
using System.Windows;

namespace WpfPlus.Controls
{
    internal static class Behaviors
    {
        internal static Behavior<T>? AddSingularBehaviorTo<T>(T target, Behavior<T> behavior) where T : DependencyObject
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

        internal static void RemoveBehaviorTypeFrom<TElement, TBehavior>(TElement target) 
            where TElement : DependencyObject
            where TBehavior : Behavior<TElement>
        {
            if ((object)target == null ) return;

            var behaviors = Interaction.GetBehaviors(target);
            var tp = typeof(TBehavior);

            var existing = behaviors.OfType<Behavior<TBehavior>>().ToList();

            foreach(var behavior in existing)
            {
                behaviors.Remove(behavior);
            }
        }

    }
}
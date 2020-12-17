using System;
using System.Collections;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

namespace WpfPlus
{
    /// <summary>
    /// A markup extensions that introduces the ability to define alias-resource-keys
    /// for existing resources. That's useful, if you want to use existing resources
    /// but don't want to export that resource as actual dependency.
    /// </summary>
    /// <example>
    /// &lt;SolidColorBrush x:Key="StatusColor_OK" Color="#ff32a248" /&gt;
    /// &lt;Alias
    ///    x:Key="AliasKey"
    ///    ResourceKey="StatusColor_Error" /&gt;
    /// </example>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    [MarkupExtensionReturnType(typeof(object))]
    public class Alias : MarkupExtension
    {
        public object ResourceKey { get; set; } = string.Empty;

        public Alias()
        {
        }

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IRootObjectProvider)) is IRootObjectProvider rootObjectProvider)
            {
                if (rootObjectProvider.RootObject is IDictionary dictionary)
                {
                    return dictionary.Contains(ResourceKey) 
                        ? dictionary[ResourceKey] 
                        : Application.Current.TryFindResource(ResourceKey);
                }
            }
            
            return Application.Current.TryFindResource(ResourceKey);
        }
    }
}
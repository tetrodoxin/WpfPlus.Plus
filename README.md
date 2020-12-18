# WPF-Plus Toolkit
_Modern flat themes and useful controls for your next WPF Application_

**License:** MIT

__Features:__
 - flat dark and light styles for the most popular controls
 - highly customizable
 - custom Grid and StackPanel with adjustable column and row spacing
 - SimpleForm container to design forms easier than ever before
 - helper classes for easier MVVM implementation in conjunction with MVVM Light

__Plus:__

 - `Alias` markup extension, to define additional ReferenceKeys for existing references (may be external)
 - `FlatResourceDictionary` class, being a `ResourceDictionary` which 'unrolls' existing resource dictionaries, that contain nested resources, thus flattening a resource-tree to a resource list
 - `ViewModelBase` class, implementing `INotifyPropertyChanged` and rudimentary tracking for changed properties
 - `ViewModelsCollection` - a class for syncing a collection of model objects with a collection of associated view models
 - classes for implementing 'Delegate Commands", meaning objects that implement `ICommand` using delegates for the `CanExecute()` and `Execute()` methods
 - `TextboxWatermarkBehavior` - a WPF behavior for `TextBox` controls that realizes a Watermark text, that's visible (grayed) if the text box is empty
 - `DragWindowAreaBehavior` - a WPF behavior to mark a UI element as 'dragging area' for a window (just like the title bar of a window)

[Download NuGet-Package](https://www.nuget.org/packages/WpfPlus.Plus/)

![Screenshot Dark Theme](https://screenshots.marcusw.de/fddc68f0a9efd497617dcd99444e3d80.png)
![Screenshot Light Theme](https://screenshots.marcusw.de/906218b32cca4002dde62e32cef53340.png)

## Install
Getting started is as simple as adding the `WpfPlus.Plus` NuGet-Package to your WPF project. You can do this in the NuGet-Package-Manager or manually:
```
PM> Install-Package WpfPlus.Plus
```

After that, one way to activate WpfPlus is to slightly edit your project's `App.xaml` to make it looking like this:
``` xml
<Application x:Class="MyApplication.App"
             [...]
             xmlns:wpfPlus="clr-namespace:WpfPlus;assembly=WpfPlus"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <wpfPlus:DarkTheme />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```
In case you like the light theme more you can also write `<wpfPlus:LightTheme />` instead.

Alternatively, if you struggle with nested resource dictionaries or just want to use a `Program.main()` method, you may also do something like this:

`App.xaml`
```xml
<Application x:Class="MyApplication.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <!-- Notice! no StartupUri here, if you use Program.main -->
    <Application.Resources />
</Application>
```

`App.xaml.cs`
``` csharp
public partial class App : Application
{
    internal App()
    {
        var isDark = true;      // or obtain the value somehow

        Resources.Clear();
        Resources.MergedDictionaries.Clear();

        if (isDark)
        {
            inhaleResourceDictionary(new WpfPlus.DarkTheme());
            inhaleResourceDictionary("ADDITIONAL_DARK_RESC.xaml");
        }
        else
        {
            inhaleResourceDictionary(new WpfPlus.LightTheme());
            inhaleResourceDictionary("ADDITIONAL_LIGHT_RESC.xaml");
        }

        inhaleResourceDictionary("ADDITIONAL_APP_RESC.xaml");            
    }

    private void inhaleResourceDictionary(string sourceUri)
        => Resources.MergedDictionaries.Add(
            new FlatResourceDictionary(
                sourceUri.Otherwise(s => !s.EndsWith(".xaml", StringComparison.OrdinalIgnoreCase), $"{sourceUri}.xaml")
                ));

    private void inhaleResourceDictionary(ResourceDictionary dict)
        => Resources.MergedDictionaries.Add(
            new FlatResourceDictionary(dict));
}
```

`Program.cs`
``` csharp
public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var app = new App();        // must be before MainWindow instantiation

        // you may also have a variant with view model here
        var window = new MainWindow();
        app.Run(window);
    }
}
```

Finally you can add `Style="{DynamicResource FlatWindowStyle}"` to any window that should apply the flat style.

__That's it. Have fun!__

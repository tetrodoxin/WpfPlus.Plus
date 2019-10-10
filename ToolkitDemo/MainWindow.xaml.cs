using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToolkitDemo
{
    public partial class MainWindow : Window
    {
        public int TextBoxTestIntegerValue { get; set; } = 42;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            this.Activated += MainWindow_Activated;

            var props = typeof(SystemColors).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty)
                .Where(p => p.PropertyType == typeof(Color))
                .ToList();

            var l = props.Select(p => p.GetValue(null, new object[0])).ToList();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var it = ((ListBox)sender).SelectedItem as ListBoxItem;
            var bg = it.Background;
            var t = bg;
            var ad = AdornerLayer.GetAdornerLayer(it);
            var ads = ad.GetAdorners(it);

        }
    }
}

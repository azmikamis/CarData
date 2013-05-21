using System.Windows;
using System.Windows.Controls;

namespace CarData.Helpers
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnSourceChanged));

        public static string GetSource(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject dependencyObject, string source)
        {
            dependencyObject.SetValue(SourceProperty, source);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = (WebBrowser)d;
            webBrowser.Navigate((string)e.NewValue);
        }
    }
}

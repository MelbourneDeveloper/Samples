using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace HiringDashboard.Controls
{
    public partial class SidebarButton : Button
    {
        public static readonly AttachedProperty<Geometry> IconDataProperty =
            AvaloniaProperty.RegisterAttached<SidebarButton, Button, Geometry>("IconData");

        public static void SetIconData(Button element, Geometry value)
        {
            element.SetValue(IconDataProperty, value);
        }

        public static Geometry GetIconData(Button element)
        {
            return element.GetValue(IconDataProperty);
        }

        public static readonly AttachedProperty<string> ButtonTextProperty =
            AvaloniaProperty.RegisterAttached<SidebarButton, Button, string>("ButtonText");

        public static void SetButtonText(Button element, string value)
        {
            element.SetValue(ButtonTextProperty, value);
        }

        public static string GetButtonText(Button element)
        {
            return element.GetValue(ButtonTextProperty);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public SidebarButton()
        {
            InitializeComponent();
        }
    }
}
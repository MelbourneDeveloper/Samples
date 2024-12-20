namespace HiringDashboard.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml

type DashboardView() as this =
    inherit UserControl()
    
    do
        AvaloniaXamlLoader.Load(this) 
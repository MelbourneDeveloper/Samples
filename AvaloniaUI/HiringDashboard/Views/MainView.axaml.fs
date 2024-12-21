namespace HiringDashboard.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml

type MainView() as this =
    inherit UserControl()
    
    do
        AvaloniaXamlLoader.Load(this) 
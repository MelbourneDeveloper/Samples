namespace HiringDashboard.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml

type PostsView() as this =
    inherit UserControl()
    
    do
        AvaloniaXamlLoader.Load(this) 
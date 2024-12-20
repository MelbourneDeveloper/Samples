namespace HiringDashboard.Controls

open Avalonia
open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Media

type SidebarButton() as this =
    inherit Button()
    
    static let IconDataProperty =
        AvaloniaProperty.Register<SidebarButton, string>("IconData", "")
        
    static let ButtonTextProperty =
        AvaloniaProperty.Register<SidebarButton, string>("ButtonText", "")
        
    static let IsCheckedProperty =
        AvaloniaProperty.Register<SidebarButton, bool>("IsChecked", false)
    
    do AvaloniaXamlLoader.Load(this)
    
    member this.IconData
        with get() = this.GetValue(IconDataProperty)
        and set(value) = 
            this.SetValue(IconDataProperty, value) |> ignore
    
    member this.ButtonText
        with get() = this.GetValue(ButtonTextProperty)
        and set(value) = 
            this.SetValue(ButtonTextProperty, value) |> ignore
            
    member this.IsChecked
        with get() = this.GetValue(IsCheckedProperty)
        and set(value) = 
            this.SetValue(IsCheckedProperty, value) |> ignore 
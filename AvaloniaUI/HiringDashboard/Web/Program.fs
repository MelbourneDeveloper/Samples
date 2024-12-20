module Program

open Avalonia
open Avalonia.Browser
open Avalonia.ReactiveUI
open HiringDashboard

module Program =
    [<EntryPoint>]
    let main argv =        
        AppBuilder
            .Configure<App>()
            .UseReactiveUI()
        |> fun builder ->
            builder.SetupWithoutStarting() |> ignore
            builder.StartBrowserAppAsync("out") |> ignore
        0
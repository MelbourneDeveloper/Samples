namespace HiringDashboard

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open System
open System.Runtime.InteropServices
open Avalonia.ReactiveUI

module Program =
    /// Avalonia configuration, don't remove; also used by visual designer.
    let BuildAvaloniaApp() =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI()

    /// Initialization code. Don't use any Avalonia, third-party APIs or any
    /// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    /// yet and stuff might break.
    [<STAThread; EntryPoint>]
    let main args =
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args)
        0
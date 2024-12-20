namespace HiringDashboard

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Markup.Xaml
open HiringDashboard.ViewModels
open HiringDashboard.Views
open System

type App() =
    inherit Application()
    
    let mutable _canClose = false
    let _mainViewModel = MainViewModel()
    
    let desktopOnShutdownRequested = EventHandler<ShutdownRequestedEventArgs>(fun _ e ->
        e.Cancel <- not _canClose
        if not _canClose then
            _canClose <- true)
    
    override this.Initialize() =
        AvaloniaXamlLoader.Load(this)
    
    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktop ->
            let mainWindow = MainWindow(DataContext = _mainViewModel)
            desktop.MainWindow <- mainWindow
            desktop.ShutdownRequested.AddHandler(desktopOnShutdownRequested)
        | _ -> ()
        
        base.OnFrameworkInitializationCompleted() 
namespace HiringDashboard.ViewModels

open System.ComponentModel

/// Base class for view models
type ViewModelBase() =
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()
    
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member _.PropertyChanged = propertyChanged.Publish
    
    member this.RaisePropertyChanged(propertyName: string) =
        propertyChanged.Trigger(this, PropertyChangedEventArgs(propertyName)) 
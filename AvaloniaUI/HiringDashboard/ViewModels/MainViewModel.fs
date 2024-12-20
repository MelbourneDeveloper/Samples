namespace HiringDashboard.ViewModels

open System
open System.Windows.Input

/// Command implementation
type RelayCommand(execute: obj -> unit) =
    let event = Event<_, _>()
    interface ICommand with
        [<CLIEvent>]
        member _.CanExecuteChanged = event.Publish
        member _.CanExecute _ = true
        member _.Execute param = execute param

/// Main view model for the hiring dashboard
type MainViewModel() as this =
    inherit ViewModelBase()
    
    let sidebarVm = new SidebarViewModel()
    let dashboardVm = lazy(new DashboardViewModel())
    let postsVm = lazy(new PostsViewModel())
    
    let navigateCommand = RelayCommand(fun param ->
        match param with
        | :? string as tab -> 
            let navItem = sidebarVm.NavigationItems |> List.find (fun x -> x.DisplayText = tab)
            sidebarVm.SelectedItem <- navItem
            match navItem.View with
            | NavigationView.Posts -> postsVm.Value.Initialize()
            | NavigationView.Dashboard -> dashboardVm.Value.Initialize()
            | _ -> ()
            this.RaisePropertyChanged(nameof this.ShowApplicants)
            this.RaisePropertyChanged(nameof this.ShowPosts)
        | _ -> ())
        
    do
        dashboardVm.Value.Initialize()
    
    /// Gets the sidebar view model
    member _.SidebarViewModel = sidebarVm
    
    /// Gets the dashboard view model
    member _.DashboardViewModel = dashboardVm.Value
    
    /// Gets the posts view model
    member _.PostsViewModel = postsVm.Value
    
    /// Gets the navigation command
    member _.NavigateCommand = navigateCommand :> ICommand
            
    /// Gets whether to show the applicants list
    member _.ShowApplicants = sidebarVm.SelectedItem.View = NavigationView.Dashboard
    
    /// Gets whether to show the posts list
    member _.ShowPosts = sidebarVm.SelectedItem.View = NavigationView.Posts
    
    /// Gets the current user's name
    member _.CurrentUser = "Bob Smith"
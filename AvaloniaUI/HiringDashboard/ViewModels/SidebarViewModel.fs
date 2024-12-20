namespace HiringDashboard.ViewModels

/// Represents the different views in the application
type NavigationView =
    | Dashboard
    | Posts
    | Vacancies
    | Applicants
    | Employees
    | CompanyStructure
    | Projects
    | LogOut

/// Represents a navigation item in the sidebar
type NavigationItem = {
    View: NavigationView
    DisplayText: string
    IconData: string
}

/// View model for the sidebar
type SidebarViewModel() as this =
    inherit ViewModelBase()
    
    let navigationItems = [
        { View = Dashboard; DisplayText = "Dashboard"; IconData = "M12 3l7.5 7L21 21h-9v-9h-3v10.5H3V10.5L12 3z" }
        { View = Posts; DisplayText = "Posts"; IconData = "M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm-2 12H6v-2h12v2zm0-3H6V9h12v2zm0-3H6V6h12v2z" }
        { View = Vacancies; DisplayText = "Vacancies"; IconData = "M20 6h-4V4c0-1.1-.9-2-2-2h-4c-1.1 0-2 .9-2 2v2H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2zm-8-2h4v2h-4V4zm0 12H8v-2h4v2zm6 0h-4v-2h4v2zm0-4H8v-2h10v2z" }
        { View = Applicants; DisplayText = "Applicants"; IconData = "M16 11c1.7 0 3-1.3 3-3s-1.3-3-3-3-3 1.3-3 3 1.3 3 3 3zm-8 0c1.7 0 3-1.3 3-3S9.7 5 8 5 5 6.3 5 8s1.3 3 3 3zm0 2c-2.3 0-7 1.2-7 3.5V19h14v-2.5c0-2.3-4.7-3.5-7-3.5zm8 0c-.3 0-.6 0-.9.1 1.1.8 1.9 2 1.9 3.4V19h6v-2.5c0-2.3-4.7-3.5-7-3.5z" }
        { View = Employees; DisplayText = "Employees"; IconData = "M16 11c1.7 0 3-1.3 3-3s-1.3-3-3-3-3 1.3-3 3 1.3 3 3 3zm-8 0c1.7 0 3-1.3 3-3S9.7 5 8 5 5 6.3 5 8s1.3 3 3 3zm0 2c-2.3 0-7 1.2-7 3.5V19h14v-2.5c0-2.3-4.7-3.5-7-3.5zm8 0c-.3 0-.6 0-.9.1 1.1.8 1.9 2 1.9 3.4V19h6v-2.5c0-2.3-4.7-3.5-7-3.5z" }
        { View = CompanyStructure; DisplayText = "Company Structure"; IconData = "M12 7V3H2v18h20V7H12zM6 19H4v-2h2v2zm0-4H4v-2h2v2zm0-4H4V9h2v2zm0-4H4V5h2v2zm4 12H8v-2h2v2zm0-4H8v-2h2v2zm0-4H8V9h2v2zm0-4H8V5h2v2zm10 12h-8v-2h2v-2h-2v-2h2v-2h-2V9h8v10zm-2-8h-2v2h2v-2zm0 4h-2v2h2v-2z" }
        { View = Projects; DisplayText = "Projects"; IconData = "M20 6h-8l-1.4-1.4C10 4 9.3 3.7 8.7 3.7H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2zm-2.9 11.3c-.4.4-1 .7-1.6.7H9.4c-.6 0-1.1-.2-1.6-.7-.4-.4-.7-1-.7-1.6 0-.6.2-1.1.7-1.6.4-.4 1-.7 1.6-.7h6c.6 0 1.1.2 1.6.7.4.4.7 1 .7 1.6 0 .6-.2 1.1-.6 1.6z" }
        { View = LogOut; DisplayText = "Log Out"; IconData = "" }
    ]
    
    let mutable selectedItem = navigationItems.Head
    
    /// Gets all navigation items
    member _.NavigationItems = navigationItems
    
    /// Gets or sets the currently selected item
    member _.SelectedItem
        with get() = selectedItem
        and set(value) =
            selectedItem <- value
            this.RaisePropertyChanged(nameof this.SelectedItem)
            this.RaisePropertyChanged(nameof this.IsTabSelected)
            
    /// Gets whether a specific tab is selected
    member _.IsTabSelected(tab: string) = selectedItem.DisplayText = tab
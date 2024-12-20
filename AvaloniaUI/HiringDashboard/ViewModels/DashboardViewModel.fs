namespace HiringDashboard.ViewModels

open System
open System.Net.Http
open System.Collections.ObjectModel
open System.Text.Json
open Avalonia.Threading

/// View model for the dashboard tab
type DashboardViewModel() as this =
    inherit ViewModelBase()
    
    let mutable isLoading = false
    let client = new HttpClient()
    let applicants = ObservableCollection<Applicant>()
    let filterOptions = {
        Considerations = ObservableCollection<string>(["All"; "New"; "Urgent"; "On Hold"])
        Stages = ObservableCollection<string>(["All"; "Screening"; "Interview"; "Technical Test"; "Offer"; "Hired"; "Rejected"])
        Positions = ObservableCollection<string>(["All"; ".NET Developer"; "Frontend Developer"; "DevOps Engineer"])
    }
    
    let random = Random()
    
    let randomStage() =
        let stages = [|Screening; Interview; TechnicalTest; Offer; Hired; Rejected|]
        stages.[random.Next(stages.Length)]
        
    let randomPosition() =
        let positions = [|".NET Developer"; "Frontend Developer"; "DevOps Engineer"; "Full Stack Developer"; "Software Architect"|]
        positions.[random.Next(positions.Length)]
        
    let mapUserToApplicant (user: ApiUser) : Applicant =
        {
            Name = user.name
            Position = randomPosition()
            Stage = randomStage()
            Location = $"{user.address.city}, {user.address.street}"
            Phone = user.phone
            ApplicationDate = DateTime.Now.AddDays(-random.Next(1, 60))
        }
    
    /// Gets the collection of applicants
    member _.Applicants = applicants
    
    /// Gets the filter options for the dashboard
    member _.FilterOptions = filterOptions
    
    /// Gets or sets whether data is currently being loading
    member _.IsLoading
        with get() = isLoading
        and set(value) =
            isLoading <- value
            this.RaisePropertyChanged(nameof this.IsLoading)
            
    /// Initializes the view model and loads initial data
    member _.Initialize() =
        async {
            isLoading <- true
            this.RaisePropertyChanged(nameof this.IsLoading)
            
            try
                let! response = client.GetStringAsync("https://jsonplaceholder.typicode.com/users") |> Async.AwaitTask
                let options = JsonSerializerOptions(PropertyNameCaseInsensitive = true)
                let users = JsonSerializer.Deserialize<ApiUser[]>(response, options)
                
                let! _ = 
                    Dispatcher.UIThread.InvokeAsync(fun () ->
                        applicants.Clear()
                        for user in users do
                            applicants.Add(mapUserToApplicant user)
                    ).GetTask() |> Async.AwaitTask
                
                isLoading <- false
                this.RaisePropertyChanged(nameof this.IsLoading)
            with ex ->
                eprintfn "Error loading applicants: %s" ex.Message
                isLoading <- false
                this.RaisePropertyChanged(nameof this.IsLoading)
        } |> Async.Start

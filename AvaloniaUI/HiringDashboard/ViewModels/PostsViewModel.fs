namespace HiringDashboard.ViewModels

open System
open System.Net.Http
open System.Text.Json
open System.Collections.ObjectModel
open Avalonia.Threading

/// View model for the posts tab
type PostsViewModel() as this =
    inherit ViewModelBase()
    
    let mutable isLoading = false
    let client = new HttpClient()
    let posts = ObservableCollection<Post>()
    
    let loadPostsAsync() = 
        async {
            isLoading <- true
            this.RaisePropertyChanged(nameof this.IsLoading)
            
            try
                // Load users first
                let! usersResponse = client.GetStringAsync("https://jsonplaceholder.typicode.com/users") |> Async.AwaitTask
                let options = JsonSerializerOptions(PropertyNameCaseInsensitive = true)
                let users = JsonSerializer.Deserialize<ApiUser[]>(usersResponse, options)
                let userMap = users |> Array.map (fun u -> u.id, u) |> Map.ofArray
                
                // Then load posts
                let! postsResponse = client.GetStringAsync("https://jsonplaceholder.typicode.com/posts") |> Async.AwaitTask
                let apiPosts = JsonSerializer.Deserialize<ApiPost[]>(postsResponse, options)
                
                let dispatcherTask = 
                    Dispatcher.UIThread.InvokeAsync(fun () -> 
                        posts.Clear()
                        for post in apiPosts do
                            match Map.tryFind post.userId userMap with
                            | Some user ->
                                posts.Add({
                                    Title = post.title
                                    Body = post.body
                                    Author = user.name
                                    Company = user.company.name
                                    Email = user.email
                                })
                            | None -> ())
                do! Async.AwaitTask(dispatcherTask.GetTask())
            with ex ->
                eprintfn "Error loading posts: %s" ex.Message
            
            isLoading <- false
            this.RaisePropertyChanged(nameof this.IsLoading)
        }
    
    /// Gets the collection of posts
    member _.Posts = posts
    
    /// Gets or sets whether data is currently being loaded
    member _.IsLoading
        with get() = isLoading
        and set(value) =
            isLoading <- value
            this.RaisePropertyChanged(nameof this.IsLoading)
    
    /// Initializes the view model and loads initial data
    member _.Initialize() =
        async {
            do! loadPostsAsync()
        } |> Async.Start
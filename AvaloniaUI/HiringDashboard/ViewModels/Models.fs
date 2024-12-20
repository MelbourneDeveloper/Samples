namespace HiringDashboard.ViewModels

open System
open System.Collections.ObjectModel

/// Represents the stage of the hiring process
type Stage =
    | Screening
    | Interview
    | TechnicalTest
    | Offer
    | Hired
    | Rejected

/// Represents filter options for the dashboard
type FilterOptions = {
    Considerations: ObservableCollection<string>
    Stages: ObservableCollection<string>
    Positions: ObservableCollection<string>
}

/// Represents an applicant in the system
type Applicant = {
    Name: string
    Position: string
    Stage: Stage
    Location: string
    Phone: string
    ApplicationDate: DateTime
}

/// Represents a post with user information
type Post = {
    Title: string
    Body: string
    Author: string
    Company: string
    Email: string
}

/// JSON response from the API for users
type ApiUser = {
    id: int
    name: string
    username: string
    email: string
    phone: string
    company: {| name: string |}
    address: {| city: string; street: string |}
}

/// JSON response from the API for posts
type ApiPost = {
    userId: int
    id: int
    title: string
    body: string
} 
namespace HiringDashboard.Converters

open System
open System.Globalization
open Avalonia.Data.Converters

type StringEqualsConverter() =
    interface IValueConverter with
        member _.Convert(value, targetType, parameter, culture) =
            match value, parameter with
            | :? string as value, :? string as parameter -> value = parameter.ToString()
            | _ -> false
            |> box
            
        member _.ConvertBack(value, targetType, parameter, culture) =
            raise (NotImplementedException()) 
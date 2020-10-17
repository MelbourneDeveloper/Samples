namespace FSharpLibrary

type Point = { X: int; Y: int; Z: int; }

type Shape =
    | Rectangle of width : float * length : float
    | Circle of radius : float
    | Prism of width : float * float * height : float

module Say =
    let hello name =
        printfn "Hello %s" name

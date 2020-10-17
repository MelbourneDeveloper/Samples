namespace FSharpLibrary

type Point = { X: int; Y: int; Z: int; }

module Say =
    let hello name =
        printfn "Hello %s" name

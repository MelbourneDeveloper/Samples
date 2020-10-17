using Microsoft.FSharp.Collections;
using System;

namespace FSharpFromCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = FSharpList<int>.Cons(
            1,
            FSharpList<int>.Cons(2,
                FSharpList<int>.Cons(
                    3,
                    FSharpList<int>.Empty)));

            Console.WriteLine(string.Join(", ", numbers));

            var fsharpPoint = new FSharpLibrary.Point(1, 2, 3);
            var csharpPoint = new CsharpPoint(1, 2, 3);
        }
    } 
}



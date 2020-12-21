using DotNetStandardLibrary;
using System;
using System.Threading.Tasks;

namespace DotNet461ConsoleApp
{
    internal class Program
    {
        private static async Task Main()
        {
            var asyncDisposable = new AsyncDisposable();
            await asyncDisposable.DisposeAsync();

            _ = new ExampleClass("test").WriteSpans().DoSerialize();

            await ExampleClass.DoAsyncNumbersAsync();

            Console.WriteLine($"The character 7 {('7'.IsLetter() ? "is" : "is not")} a letter");
        }
    }
}

using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNetStandardLibrary
{
    public class ExampleClass
    {
        public string Test { get; }

        public ExampleClass(string test)
        {
            Test = test;
        }

        /// <summary>
        /// Uses the Span type
        /// </summary>
        public ExampleClass WriteSpans()
        {
            var spans = new Span<string>(new string[] { "One", "Two" });
            foreach (var span in spans)
            {
                Console.WriteLine(span);
            }

            return this;
        }

        /// <summary>
        /// Use an Async foreach with IAsyncEnumerable
        /// </summary>
        public static async Task DoAsyncNumbersAsync()
        {
            var asyncEnumerable = AsyncEnumerable.Range(0, 10);
            await foreach (var number in asyncEnumerable)
            {
                Console.WriteLine($"Awaited Number: {number}");
            }
        }

        /// <summary>
        /// Serialize and Deserialize with System.Text.Json
        /// </summary>
        public ExampleClass DoSerialize()
        {
            var dailyTemperature = new DailyTemperature(10, 20);
            var json = JsonSerializer.Serialize(dailyTemperature);
            dailyTemperature = JsonSerializer.Deserialize<DailyTemperature>(json);

            if (dailyTemperature == null)
            {
                throw new InvalidOperationException();
            }

            Console.WriteLine($"Json: {json}\r\nHigh: {dailyTemperature.HighTemp} Low: {dailyTemperature.LowTemp}");

            return this;
        }

    }

    public static class Extensions
    {
        /// <summary>
        /// C# Pattern matching example
        /// </summary>
        public static bool IsLetter(this char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z');
    }

    /// <summary>
    /// IAsyncDisposable Example
    /// </summary>
    public class AsyncDisposable : IAsyncDisposable
    {
        public ValueTask DisposeAsync() => new ValueTask(Task.FromResult(true));
    }

    /// <summary>
    /// Record example
    /// </summary>
    public record DailyTemperature(double HighTemp, double LowTemp);
}

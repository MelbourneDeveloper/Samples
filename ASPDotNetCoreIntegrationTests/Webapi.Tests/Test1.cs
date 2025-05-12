using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Webapi.Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public async Task GetWeatherForecast_ReturnsForecastsWithExpectedShape()
        {
            var _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    //Override the data service with a fake implementation
                    services.AddSingleton<IDataService, FakeDataService>();
                });
            });

            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/weatherforecast?city=London");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast>();
            Assert.IsNotNull(forecasts);
            Assert.AreEqual(forecasts.TemperatureC, 20);
            Assert.AreEqual(forecasts.Summary, "Sunny");
            Assert.AreEqual(forecasts.Date, DateOnly.FromDateTime(DateTime.Now));
        }
    }
}

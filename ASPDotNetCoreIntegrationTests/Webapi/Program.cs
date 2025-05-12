using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add the real data service to the DI container
builder.Services.AddOpenApi().AddSingleton<IDataService, RealDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet(
        "/weatherforecast",
        (IDataService dataService, [FromQuery] string city) =>
        {
            return Results.Ok(dataService.GetWeatherByCity(city));
        }
    )
    .WithName("GetWeatherForecast");

app.Run();

public partial class Program { }

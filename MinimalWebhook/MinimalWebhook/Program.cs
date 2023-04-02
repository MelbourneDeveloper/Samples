//This is an ASP .NET Core Minimal API that receives Webhook POST data
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Configure services
builder.Services.AddSingleton<IReceiveWebhook, ConsoleWebhookReceiver>();

var app = builder.Build();

app.UseHttpsRedirection();

//Listen for POST webhooks
app.MapPost("/webhook", async (HttpContext context, IReceiveWebhook receiveWebook) =>
{
    using StreamReader stream = new StreamReader(context.Request.Body);
    return await receiveWebook.ProcessRequest(await stream.ReadToEndAsync());
});

app.Run();

/// <summary>
/// An abstraction to process the POST request data that the webbook receives.
/// </summary>
public interface IReceiveWebhook
{
    Task<string> ProcessRequest(string requestBody);
}

/// <summary>
/// An implementation for IReceiveWebhook that writes to the console
/// </summary>
public class ConsoleWebhookReceiver : IReceiveWebhook
{
    /// <summary>
    /// Writes the POST request body to the console and returns JSON
    /// </summary>
    public async Task<string> ProcessRequest(string requestBody)
    {
        //This is where you would put your actual business logic for receiving webhooks

        Console.WriteLine($"Request Body: {requestBody}");

        return "{\"message\" : \"Thanks! We got your webhook\"}";
    }
}

/// <summary>
/// This is just a requirement for ASP .NET Core and testing this service
/// </summary>
public partial class Program { }
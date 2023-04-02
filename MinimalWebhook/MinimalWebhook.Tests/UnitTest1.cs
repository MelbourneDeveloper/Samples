//This is an ASP .NET Core Integration test that waits for a webhook and verifies that
//the service is correctly receiving the POST data

using System.Net;
using System.Text.Json;
using System.Xml;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using MinimalWebhook;

namespace MinimalWebhook.Tests;

/// <summary>
/// This is a fake implementation of IReceiveWebhook so we can keep track
/// of incoming webhook Calls
/// </summary>
public class FakewebhookReceiver : IReceiveWebhook
{
    public List<String> Receipts = new List<String>();

    public async Task<string> ProcessRequest(String requestBody)
    {
        Receipts.Add(requestBody);

        return "Hello back";
    }
}

public class Tests
{
    /// <summary>
    /// This test only works when the actual webhook is running. You should see the
    /// request body outputted to the console
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestLiveWebhook()
    {
        var client = new HttpClient();
        await client.PostAsync("https://localhost:60000/webhook", new StringContent("Hi"));
    }


    [Fact]
    public async Task TestReceivingWebhook()
    {
        var fakeReceiver = new FakewebhookReceiver();

        await WithTestServer(async (c, s) =>
        {
            //Wait 100 seconds to receive the webhook
            for (var i = 0; i < 100; i++)
            {
                //----------------------------------------
                //This emulates the webhook receipt locally. Delete this and use ngrok to tunnel the call through
                var response = await c.PostAsync("webhook", new StringContent("Hi"));

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var responseText = await response.Content.ReadAsStringAsync();

                Assert.Equal("Hello back", responseText);
                //----------------------------------------

                if (fakeReceiver.Receipts.Count > 0)
                {
                    break;
                }

                await Task.Delay(1000);
            }

            //Verify that we received the correct details from the webhook
            Assert.Equal("Hi", fakeReceiver.Receipts.First());



        }, s => s.AddSingleton((IReceiveWebhook)fakeReceiver));
    }

    /// <summary>
    /// Spins up a test server and allows us to configure the server with test doubles
    /// </summary>
    private async Task WithTestServer(
        Func<HttpClient, IServiceProvider, Task> test,
        Action<IServiceCollection> configureServices)
    {
        await using var application =
            new WebApplicationFactory<Program>().
            WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => configureServices(services));
            });

        using var client = application.CreateClient();
        await test(client, application.Services);
    }
}

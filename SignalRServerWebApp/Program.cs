using Microsoft.AspNetCore.SignalR;
using SignalRServerWebApp.Hubs;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add CORS for Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(_ => true)
              .AllowCredentials();
    });
});

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors("BlazorPolicy");

// Map SignalR Hub
app.MapHub<FlightHub>("/flightHub");

// HTTP endpoint to receive notifications from WinForms/WebSocket server
app.MapPost("/notify-flight-change", async (HttpContext context) =>
{
    try
    {
        var json = await new StreamReader(context.Request.Body).ReadToEndAsync();

        if (string.IsNullOrEmpty(json))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Empty request body");
            return;
        }

        var data = JsonSerializer.Deserialize<JsonElement>(json);

        if (!data.TryGetProperty("FlightId", out var flightIdElement) ||
            !data.TryGetProperty("Status", out var statusElement))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing FlightId or Status");
            return;
        }

        int flightId = flightIdElement.GetInt32();
        int status = statusElement.GetInt32();
        string source = data.TryGetProperty("Source", out var sourceElement) ?
            sourceElement.GetString() ?? "Unknown" : "Unknown";

        // Send SignalR notification to all Blazor clients
        var hubContext = context.RequestServices.GetRequiredService<IHubContext<FlightHub>>();
        await hubContext.Clients.All.SendAsync("FlightStatusChanged", flightId, status);

        Console.WriteLine($"✅ SignalR notification sent from {source} to Blazor clients - Flight: {flightId}, Status: {status}");

        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("OK");
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"❌ JSON parsing error: {ex.Message}");
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync($"Invalid JSON: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error in SignalR server: {ex.Message}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync($"Error: {ex.Message}");
    }
});

Console.WriteLine("🚀 SignalR Server starting on http://localhost:8080");
Console.WriteLine("   📡 SignalR Hub: /flightHub (Blazor WebAssembly клиентүүдэд)");
Console.WriteLine("   🔔 Notification endpoint: /notify-flight-change");

app.Urls.Add("http://localhost:8080");

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Failed to start SignalR server: {ex.Message}");
}
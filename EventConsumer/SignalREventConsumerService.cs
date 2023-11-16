using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

public class SignalREventConsumerService
{
    private readonly HubConnection _hubConnection;

    public SignalREventConsumerService(string hubUrl)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

        _hubConnection.Closed += async (error) =>
        {
            Console.WriteLine($"Connection closed. Attempting to restart...");
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await _hubConnection.StartAsync();
        };
    }

    public async Task StartAsync()
    {
        _hubConnection.On<string>("SendMessage", (message) =>
        {
            Console.WriteLine($"Received Message: {message}");
        });

        await _hubConnection.StartAsync();
        Console.WriteLine($"Connected to the SignalR hub.");
    }

    public async Task StopAsync()
    {
        await _hubConnection.StopAsync();
        Console.WriteLine($"Disconnected from the SignalR hub.");
    }
}

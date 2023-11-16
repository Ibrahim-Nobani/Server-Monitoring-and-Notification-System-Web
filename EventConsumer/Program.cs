using System;

var signalREventConsumerService = new SignalREventConsumerService("http://localhost:5079/alerthub");

Console.WriteLine("Press Enter to start the SignalR Event Consumer Service...");
Console.ReadLine();

await signalREventConsumerService.StartAsync();

Console.WriteLine("Press Enter to stop the SignalR Event Consumer Service...");
Console.ReadLine();

await signalREventConsumerService.StopAsync();

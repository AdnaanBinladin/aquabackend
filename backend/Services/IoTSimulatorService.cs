using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class IoTSimulatorService : BackgroundService
{
    private readonly IHubContext<WaterHub> _hubContext;
    private readonly Random _random = new Random();

    public IoTSimulatorService(IHubContext<WaterHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Simulate a record from an IoT device
            var record = new WaterRecord
            {
                UserId = "simulatedDevice01",
                Consumption = Math.Round(_random.NextDouble() * 10, 2),
                Date = DateTime.UtcNow
            };

            // Add the new record to in-memory data
            InMemoryDataStore.WaterRecords.Add(record);

            // Notify all connected clients about the new record
            await _hubContext.Clients.All.SendAsync("NewRecord", record, cancellationToken: stoppingToken);

            Console.WriteLine($"[IoT Simulator] New record added: {record.Consumption}L for {record.UserId}");

            // Wait for 4 seconds before generating the next record
            await Task.Delay(4000, stoppingToken);
        }
    }
}

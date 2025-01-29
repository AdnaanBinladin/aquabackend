using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); // Add SignalR for real-time updates

// Register the IoT simulator hosted service
builder.Services.AddHostedService<IoTSimulatorService>();


var app = builder.Build();

// Use Swagger if you want to test easily
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHub<WaterHub>("/waterhub"); // Real-time hub endpoint

app.Run();

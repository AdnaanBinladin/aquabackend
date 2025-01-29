using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class WaterController : ControllerBase
{
    private readonly IHubContext<WaterHub> _hubContext;

    public WaterController(IHubContext<WaterHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet]
    public IActionResult GetAllRecords()
    {
        return Ok(InMemoryDataStore.WaterRecords);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddRecord([FromBody] WaterRecord record)
    {
        if (string.IsNullOrEmpty(record.UserId) || record.Consumption <= 0)
            return BadRequest("Invalid record data");

        InMemoryDataStore.WaterRecords.Add(record);

        // Notify clients about the new record in real-time
        await _hubContext.Clients.All.SendAsync("NewRecord", record);

        return Created("", record);
    }

    [HttpPut("{index}")]
    public IActionResult UpdateRecord(int index, [FromBody] WaterRecord updatedRecord)
    {
        if (index < 0 || index >= InMemoryDataStore.WaterRecords.Count)
            return NotFound("Record not found");

        InMemoryDataStore.WaterRecords[index] = updatedRecord;
        return Ok(new { message = "Record updated", record = updatedRecord });
    }

    [HttpDelete("{index}")]
    public IActionResult DeleteRecord(int index)
    {
        if (index < 0 || index >= InMemoryDataStore.WaterRecords.Count)
            return NotFound("Record not found");

        InMemoryDataStore.WaterRecords.RemoveAt(index);
        return Ok(new { message = "Record deleted" });
    }
    [HttpGet("trends")]
    public IActionResult GetTrends([FromQuery] string userId)
    {
        var userRecords = InMemoryDataStore.WaterRecords
            .Where(r => r.UserId == userId)
            .ToList();

        if (!userRecords.Any())
            return Ok(new { message = "No records found for this user" });

        double totalConsumption = userRecords.Sum(r => r.Consumption);
        double averageConsumption = userRecords.Average(r => r.Consumption);

        return Ok(new
        {
            totalConsumption,
            averageConsumption,
            count = userRecords.Count
        });
    }

}

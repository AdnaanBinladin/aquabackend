using System.Collections.Generic;

public static class InMemoryDataStore
{
    public static List<User> Users = new List<User>
    {
        new User { Username = "admin", Password = "admin123" }
    };

    public static List<WaterRecord> WaterRecords = new List<WaterRecord>();
}

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class WaterRecord
{
    public string UserId { get; set; }
    public double Consumption { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}

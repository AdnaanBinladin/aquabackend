using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] User newUser)
    {
        if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password))
            return BadRequest("Username and Password required");

        var exists = InMemoryDataStore.Users.Any(u => u.Username == newUser.Username);
        if (exists)
            return Conflict("User already exists");

        InMemoryDataStore.Users.Add(newUser);
        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User loginRequest)
    {
        var user = InMemoryDataStore.Users
            .SingleOrDefault(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);

        if (user == null)
            return Unauthorized("Invalid username or password");

        // Normally you'd return a JWT or something secure.
        // For now, just return a success message.
        return Ok(new { message = "Login successful", user = user.Username });
    }
}

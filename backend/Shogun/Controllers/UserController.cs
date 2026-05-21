using Microsoft.AspNetCore.Mvc;
using Shogun.Models;

namespace Shogun.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController: ControllerBase
{
    [HttpGet(Name = "GetUser")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = new[]
        {
            new User {Id = 1, Username= "cicada", Email = "cicada@mail.org", AvatarURL = "", CreatedAt = DateTime.Now, IsAdmin = true, PasswordHash = ""}
        };
        return await Task.FromResult(Ok(users));
    }
}
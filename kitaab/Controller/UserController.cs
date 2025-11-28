using kitaab.Database;
using Microsoft.AspNetCore.Mvc;
using kitaab.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kitaab.Controller;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserController(ApplicationDbContext dbContext, UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet("/allUsers")]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var getUsers = await _dbContext.Users.ToListAsync();

        if (getUsers.Count == 0 || getUsers == null)
            return NotFound("No users found");
        
        return Ok(new {Message= "Users fetched", users= getUsers});
    }
}
using kitaab.Database;
using kitaab.DTO;
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

    [HttpPost("/register")]
    public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO registerRequest)
    {
        var userExists = await _userManager.FindByEmailAsync(registerRequest.userEmail);
        if (userExists != null)
            return Conflict("There is a user with ths mail already");

        var newUser = new User
        {
            Email = registerRequest.userEmail,
            PasswordHash = registerRequest.userPassword,
            UserName = registerRequest.FullName,
            fullName = registerRequest.FullName,
            createDate = DateTime.UtcNow,
            EmailConfirmed = false,
            isActive = true,
        };

        var creatingUser = await _userManager.CreateAsync(newUser, registerRequest.userPassword);
        if (!creatingUser.Succeeded)
            return BadRequest(creatingUser.Errors.First().Description);
        
        await _userManager.AddToRoleAsync(newUser, "User");

        var responseDTO = new RegisterDTO
        {
            FullName = registerRequest.FullName,
            userEmail =  registerRequest.userEmail,
            userPassword =  registerRequest.userPassword,
        };

        return Ok( new
        {
            Message = "User created",
            details = responseDTO
        });
    }
}
using Microsoft.AspNetCore.Identity;

namespace kitaab.Model;

public class User : IdentityUser
{
    public string? fullName { get; set; }
    public DateTime createDate { get; set; } = DateTime.UtcNow;
    public bool isActive { get; set; } = true;
}
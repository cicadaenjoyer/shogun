namespace Shogun.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required bool IsAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? AvatarURL { get; set; }

    
}
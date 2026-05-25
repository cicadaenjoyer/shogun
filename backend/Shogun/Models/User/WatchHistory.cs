namespace Shogun.Models;

public class WatchHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string MediaType { get; set; }
    public int MediaId { get; set; }
    public int ProgressSeconds { get; set; }
    public bool Completed { get; set; }
    public DateTime WatchedAt { get; set; }

    public User User { get; set; } = null!;
}
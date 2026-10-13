namespace Shogun.Server.Entities;

public class PlaybackState
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int? MovieId { get; set; }
    public int? EpisodeId { get; set; }
    public int? TrackId { get; set; }
    public int PositionSeconds { get; set; }
    public bool Completed { get; set; }
    public int PlayCount { get; set; }
    public DateTime LastPlayedAt { get; set; }

    public Member Member { get; set; } = null!;
    public Movie? Movie { get; set; }
    public Episode? Episode { get; set; }
    public Track? Track { get; set; }
}

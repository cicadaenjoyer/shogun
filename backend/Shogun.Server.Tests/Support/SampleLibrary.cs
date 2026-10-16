using Shogun.Server.Entities;

namespace Shogun.Server.Tests.Support;

/// <summary>
/// The IDs of a small library saved to a test database: one folder, one movie,
/// one TV episode, one music track, a file linked to the movie, and a member
/// with access to the library and progress through the movie.
/// </summary>
public sealed record SampleLibrary(
    int LibraryId,
    int LibraryPathId,
    int MovieId,
    int TvShowId,
    int SeasonId,
    int EpisodeId,
    int ArtistId,
    int AlbumId,
    int TrackId,
    int MediaFileId,
    int MemberId,
    int MovieProgressId)
{
    /// <summary>
    /// Saves the sample library and returns the IDs of everything in it.
    /// A real library holds one kind of media; this one holds all three so a
    /// single helper can serve every test. Nothing in the schema forbids it.
    /// </summary>
    public static async Task<SampleLibrary> CreateAsync(TestDatabase database, string name = "Sample")
    {
        await using var db = database.CreateContext();

        var library = new Library { Name = name, Type = LibraryType.Movie };
        var folder = new LibraryPath { Path = $"/media/{name}", Library = library };

        var movie = new Movie { Title = "Arrival", SortTitle = "Arrival", Library = library };

        var show = new TvShow { Title = "Severance", SortTitle = "Severance", Library = library };
        var season = new Season { SeasonNumber = 1, TvShow = show };
        var episode = new Episode { Title = "Good News About Hell", EpisodeNumber = 1, Season = season };

        var artist = new Artist { Name = "Radiohead", SortName = "Radiohead", Library = library };
        var album = new Album { Title = "OK Computer", SortTitle = "OK Computer", Artist = artist };
        var track = new Track { Title = "Airbag", TrackNumber = 1, Album = album };

        var file = new MediaFile
        {
            FilePath = $"/media/{name}/Arrival (2016).mkv",
            LibraryPath = folder,
            Movie = movie,
            LastModifiedAt = DateTime.UtcNow
        };

        var member = new Member
        {
            FirebaseUid = $"uid-{name}",
            DisplayName = "Sam",
            Role = MemberRole.Member,
            Status = MemberStatus.Active,
            AllowedLibraries = [library]
        };

        var progress = new PlaybackState { Member = member, Movie = movie, LastPlayedAt = DateTime.UtcNow };

        db.AddRange(folder, episode, track, file, progress);
        await db.SaveChangesAsync();

        return new SampleLibrary(
            library.Id, folder.Id, movie.Id, show.Id, season.Id, episode.Id,
            artist.Id, album.Id, track.Id, file.Id, member.Id, progress.Id);
    }
}

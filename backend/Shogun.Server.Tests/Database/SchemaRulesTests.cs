using Microsoft.EntityFrameworkCore;
using Shogun.Server.Entities;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Database;

/// <summary>
/// Database tests: checks that the rules in the schema are enforced by a real,
/// migrated SQLite database.
/// </summary>
public class SchemaRulesTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose()
    {
        _database.Dispose();
    }

    // Saves a library, an artist, and an album, and returns the album's ID
    private async Task<int> CreateAlbumAsync()
    {
        await using var db = _database.CreateContext();

        var album = new Album
        {
            Title = "OK Computer",
            SortTitle = "OK Computer",
            Artist = new Artist
            {
                Name = "Radiohead",
                SortName = "Radiohead",
                Library = new Library { Name = "Music", Type = LibraryType.Music }
            }
        };

        db.Albums.Add(album);
        await db.SaveChangesAsync();

        return album.Id;
    }

    private static Track NewTrack(int albumId, int discNumber, int trackNumber)
    {
        return new Track
        {
            AlbumId = albumId,
            DiscNumber = discNumber,
            TrackNumber = trackNumber,
            Title = $"Disc {discNumber} track {trackNumber}"
        };
    }

    [Fact]
    public async Task TwoTracksWithTheSameAlbumDiscAndNumber_AreRefused()
    {
        var albumId = await CreateAlbumAsync();
        await using var db = _database.CreateContext();

        db.Tracks.Add(NewTrack(albumId, discNumber: 1, trackNumber: 1));
        await db.SaveChangesAsync();

        db.Tracks.Add(NewTrack(albumId, discNumber: 1, trackNumber: 1));

        await Should.ThrowAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Fact]
    public async Task TheSameTrackNumberOnAnotherDisc_IsAccepted()
    {
        var albumId = await CreateAlbumAsync();
        await using var db = _database.CreateContext();

        db.Tracks.Add(NewTrack(albumId, discNumber: 1, trackNumber: 1));
        db.Tracks.Add(NewTrack(albumId, discNumber: 2, trackNumber: 1));
        await db.SaveChangesAsync();

        (await db.Tracks.CountAsync()).ShouldBe(2);
    }

    [Fact]
    public async Task TimesReadFromTheDatabase_AreMarkedAsUtc()
    {
        var savedAt = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        await using (var db = _database.CreateContext())
        {
            db.Libraries.Add(new Library { Name = "Movies", Type = LibraryType.Movie, LastScannedAt = savedAt });
            await db.SaveChangesAsync();
        }

        await using (var db = _database.CreateContext())
        {
            var library = await db.Libraries.SingleAsync();

            library.LastScannedAt.ShouldBe(savedAt);
            library.LastScannedAt!.Value.Kind.ShouldBe(DateTimeKind.Utc);
        }
    }

    // ── Check constraints ──────────────────────────────────────────────

    [Fact]
    public async Task ProgressPointingAtNoItem_IsRefused()
    {
        var sample = await SampleLibrary.CreateAsync(_database);
        await using var db = _database.CreateContext();

        db.PlaybackStates.Add(new PlaybackState { MemberId = sample.MemberId, LastPlayedAt = DateTime.UtcNow });

        await Should.ThrowAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Fact]
    public async Task ProgressPointingAtTwoItems_IsRefused()
    {
        var sample = await SampleLibrary.CreateAsync(_database);
        await using var db = _database.CreateContext();

        db.PlaybackStates.Add(new PlaybackState
        {
            MemberId = sample.MemberId,
            EpisodeId = sample.EpisodeId,
            TrackId = sample.TrackId,
            LastPlayedAt = DateTime.UtcNow
        });

        await Should.ThrowAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Fact]
    public async Task FileLinkedToNoWork_IsAccepted()
    {
        // A scanned file waits unlinked until it's matched to a movie, episode, or track
        var sample = await SampleLibrary.CreateAsync(_database);
        await using var db = _database.CreateContext();

        db.MediaFiles.Add(NewFile(sample, "/media/Sample/unmatched.mkv"));
        await db.SaveChangesAsync();

        (await db.MediaFiles.CountAsync(f => f.MovieId == null && f.EpisodeId == null && f.TrackId == null))
            .ShouldBe(1);
    }

    [Fact]
    public async Task FileLinkedToTwoWorks_IsRefused()
    {
        var sample = await SampleLibrary.CreateAsync(_database);
        await using var db = _database.CreateContext();

        var file = NewFile(sample, "/media/Sample/both.mkv");
        file.MovieId = sample.MovieId;
        file.EpisodeId = sample.EpisodeId;
        db.MediaFiles.Add(file);

        await Should.ThrowAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    // ── Unique rules ───────────────────────────────────────────────────

    [Fact]
    public async Task TheSameFolderInTwoLibraries_IsRefused()
    {
        var sample = await SampleLibrary.CreateAsync(_database);
        await using var db = _database.CreateContext();

        var folderPath = (await db.LibraryPaths.SingleAsync(p => p.Id == sample.LibraryPathId)).Path;
        var other = new Library { Name = "Other", Type = LibraryType.Movie };
        db.LibraryPaths.Add(new LibraryPath { Path = folderPath, Library = other });

        await Should.ThrowAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Fact]
    public async Task TwoMembersWithTheSameFirebaseUid_AreRefused()
    {
        await using var db = _database.CreateContext();

        db.Members.Add(NewMember("uid-123", "Sam"));
        db.Members.Add(NewMember("uid-123", "Alex"));

        await Should.ThrowAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Fact]
    public async Task GenresDifferingOnlyByLetterCase_AreRefused()
    {
        await using var db = _database.CreateContext();

        db.Genres.Add(new Genre { Name = "Science Fiction" });
        await db.SaveChangesAsync();

        db.Genres.Add(new Genre { Name = "science fiction" });

        await Should.ThrowAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Fact]
    public async Task TwoProgressRowsForOneMemberAndMovie_AreRefused()
    {
        // The sample already holds the member's progress through the movie
        var sample = await SampleLibrary.CreateAsync(_database);
        await using var db = _database.CreateContext();

        db.PlaybackStates.Add(new PlaybackState
        {
            MemberId = sample.MemberId,
            MovieId = sample.MovieId,
            LastPlayedAt = DateTime.UtcNow
        });

        await Should.ThrowAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    // ── Things that must be allowed ────────────────────────────────────

    [Fact]
    public async Task OneArtistWithTwoAlbumsOfTheSameTitle_IsAccepted()
    {
        // e.g. an original release and a remaster, both called "Greatest Hits"
        var sample = await SampleLibrary.CreateAsync(_database);
        await using var db = _database.CreateContext();

        db.Albums.Add(new Album { ArtistId = sample.ArtistId, Title = "Greatest Hits", SortTitle = "Greatest Hits" });
        db.Albums.Add(new Album { ArtistId = sample.ArtistId, Title = "Greatest Hits", SortTitle = "Greatest Hits" });
        await db.SaveChangesAsync();

        (await db.Albums.CountAsync(a => a.Title == "Greatest Hits")).ShouldBe(2);
    }

    [Fact]
    public async Task TwoArtistsWithTheSameName_AreAccepted()
    {
        // Different people can share a name; MusicBrainz IDs tell them apart
        var sample = await SampleLibrary.CreateAsync(_database);
        await using var db = _database.CreateContext();

        db.Artists.Add(new Artist { LibraryId = sample.LibraryId, Name = "Nirvana", SortName = "Nirvana" });
        db.Artists.Add(new Artist { LibraryId = sample.LibraryId, Name = "Nirvana", SortName = "Nirvana" });
        await db.SaveChangesAsync();

        (await db.Artists.CountAsync(a => a.Name == "Nirvana")).ShouldBe(2);
    }

    [Fact]
    public async Task OneTrackCreditedToTwoArtists_IsAccepted()
    {
        var sample = await SampleLibrary.CreateAsync(_database);

        await using (var db = _database.CreateContext())
        {
            var track = await db.Tracks.SingleAsync(t => t.Id == sample.TrackId);
            var first = await db.Artists.SingleAsync(a => a.Id == sample.ArtistId);
            var second = new Artist { LibraryId = sample.LibraryId, Name = "Guest", SortName = "Guest" };

            track.Artists.Add(first);
            track.Artists.Add(second);
            await db.SaveChangesAsync();
        }

        await using (var db = _database.CreateContext())
        {
            var track = await db.Tracks.Include(t => t.Artists).SingleAsync(t => t.Id == sample.TrackId);

            track.Artists.Count.ShouldBe(2);
        }
    }

    private static MediaFile NewFile(SampleLibrary sample, string path)
    {
        return new MediaFile
        {
            FilePath = path,
            LibraryPathId = sample.LibraryPathId,
            LastModifiedAt = DateTime.UtcNow
        };
    }

    private static Member NewMember(string firebaseUid, string displayName)
    {
        return new Member
        {
            FirebaseUid = firebaseUid,
            DisplayName = displayName,
            Role = MemberRole.Member,
            Status = MemberStatus.Active
        };
    }
}

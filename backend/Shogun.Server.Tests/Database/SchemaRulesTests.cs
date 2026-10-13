using Microsoft.EntityFrameworkCore;
using Shogun.Server.Entities;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Database;

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
}

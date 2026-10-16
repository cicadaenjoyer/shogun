using Microsoft.EntityFrameworkCore;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Database;

/// <summary>
/// Database tests for what happens to related rows when something is deleted.
/// Deletes are sent straight to the database with ExecuteDeleteAsync, and the
/// results are read back through a fresh context, so these test SQLite's
/// foreign key rules rather than EF Core's in-memory tracking.
/// </summary>
public class DeleteRulesTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose()
    {
        _database.Dispose();
    }

    [Fact]
    public async Task DeletingAMovie_RemovesProgressAndUnlinksItsFile()
    {
        var sample = await SampleLibrary.CreateAsync(_database);

        await using (var db = _database.CreateContext())
        {
            await db.Movies.Where(m => m.Id == sample.MovieId).ExecuteDeleteAsync();
        }

        await using (var db = _database.CreateContext())
        {
            (await db.PlaybackStates.AnyAsync(p => p.MovieId == sample.MovieId)).ShouldBeFalse();

            // The file on disk still exists, so its record stays, waiting to be matched again
            var file = await db.MediaFiles.SingleAsync(f => f.Id == sample.MediaFileId);
            file.MovieId.ShouldBeNull();

            (await db.Members.AnyAsync(m => m.Id == sample.MemberId)).ShouldBeTrue();
        }
    }

    [Fact]
    public async Task DeletingASeason_RemovesItsEpisodes()
    {
        var sample = await SampleLibrary.CreateAsync(_database);

        await using (var db = _database.CreateContext())
        {
            await db.Seasons.Where(s => s.Id == sample.SeasonId).ExecuteDeleteAsync();
        }

        await using (var db = _database.CreateContext())
        {
            (await db.Episodes.AnyAsync(e => e.SeasonId == sample.SeasonId)).ShouldBeFalse();
            (await db.TvShows.AnyAsync(t => t.Id == sample.TvShowId)).ShouldBeTrue();
        }
    }

    [Fact]
    public async Task DeletingALibrary_RemovesEverythingInItAndKeepsMembers()
    {
        var sample = await SampleLibrary.CreateAsync(_database);
        var other = await SampleLibrary.CreateAsync(_database, name: "Other");

        await using (var db = _database.CreateContext())
        {
            await db.Libraries.Where(l => l.Id == sample.LibraryId).ExecuteDeleteAsync();
        }

        await using (var db = _database.CreateContext())
        {
            (await db.LibraryPaths.AnyAsync(p => p.Id == sample.LibraryPathId)).ShouldBeFalse();
            (await db.MediaFiles.AnyAsync(f => f.Id == sample.MediaFileId)).ShouldBeFalse();
            (await db.Movies.AnyAsync(m => m.Id == sample.MovieId)).ShouldBeFalse();
            (await db.TvShows.AnyAsync(t => t.Id == sample.TvShowId)).ShouldBeFalse();
            (await db.Seasons.AnyAsync(s => s.Id == sample.SeasonId)).ShouldBeFalse();
            (await db.Episodes.AnyAsync(e => e.Id == sample.EpisodeId)).ShouldBeFalse();
            (await db.Artists.AnyAsync(a => a.Id == sample.ArtistId)).ShouldBeFalse();
            (await db.Albums.AnyAsync(a => a.Id == sample.AlbumId)).ShouldBeFalse();
            (await db.Tracks.AnyAsync(t => t.Id == sample.TrackId)).ShouldBeFalse();
            (await db.PlaybackStates.AnyAsync(p => p.Id == sample.MovieProgressId)).ShouldBeFalse();

            // The member stays, with access to the deleted library removed
            var member = await db.Members.Include(m => m.AllowedLibraries).SingleAsync(m => m.Id == sample.MemberId);
            member.AllowedLibraries.ShouldBeEmpty();

            // Another library is untouched
            (await db.Movies.AnyAsync(m => m.Id == other.MovieId)).ShouldBeTrue();
            (await db.PlaybackStates.AnyAsync(p => p.Id == other.MovieProgressId)).ShouldBeTrue();
        }
    }

    [Fact]
    public async Task DeletingAMember_RemovesTheirProgressAndNoMedia()
    {
        var sample = await SampleLibrary.CreateAsync(_database);

        await using (var db = _database.CreateContext())
        {
            await db.Members.Where(m => m.Id == sample.MemberId).ExecuteDeleteAsync();
        }

        await using (var db = _database.CreateContext())
        {
            (await db.PlaybackStates.AnyAsync(p => p.MemberId == sample.MemberId)).ShouldBeFalse();

            (await db.Libraries.AnyAsync(l => l.Id == sample.LibraryId)).ShouldBeTrue();
            (await db.Movies.AnyAsync(m => m.Id == sample.MovieId)).ShouldBeTrue();
            (await db.Episodes.AnyAsync(e => e.Id == sample.EpisodeId)).ShouldBeTrue();
            (await db.Tracks.AnyAsync(t => t.Id == sample.TrackId)).ShouldBeTrue();
            (await db.MediaFiles.AnyAsync(f => f.Id == sample.MediaFileId)).ShouldBeTrue();
        }
    }
}

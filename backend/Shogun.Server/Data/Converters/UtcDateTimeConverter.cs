using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shogun.Server.Data.Converters;

/// <summary>
/// Marks every time read from the database as UTC. SQLite stores times as plain
/// text, so without this they come back with no time zone label.
/// </summary>
public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter() : base(value => ToDatabase(value), value => FromDatabase(value)) { }

    public static DateTime ToDatabase(DateTime value)
    {
        return value;
    }

    public static DateTime FromDatabase(DateTime value)
    {
        return DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}

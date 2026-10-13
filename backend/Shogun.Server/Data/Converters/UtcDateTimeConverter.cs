using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shogun.Server.Data.Converters;

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter() : base(value => ToDatabase(value), value => FromDatabase(value)) {}

    public static DateTime ToDatabase(DateTime value)
    {
        return value;
    }

    public static DateTime FromDatabase(DateTime value)
    {
        return DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
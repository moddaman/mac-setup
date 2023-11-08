namespace LocalCommandSender;

public static class DateTimeExtensions
{
    /// <summary>
    ///  Returns a new DateTime with the same time value as the original DateTime, 
    ///  but rounded down to the nearest microsecond.
    ///  This is because Postgres only has 1 microsecond precision, 
    ///  while DateTime has 0.1 microsecond precision. 
    ///  This causes all kinds of problems, so just use a PGSafe DateTime instead.
    /// </summary>
    /// <param name="dt">Original DateTime to copy and round down</param>
    /// <returns>New DateTime rounded down to nearest microsecond</returns>
    public static DateTime ToPgSafe(this DateTime dt)
    {
        return new(dt.Ticks - (dt.Ticks % 10), dt.Kind);
    }

    /// <summary>
    /// Returns a new DateTime with the same time as the original, but with DateTimeKind.Utc.
    /// The difference between the standard .ToUniversalTime() function is that this one
    /// will NOT treat input DateTimes with DateTimeKind.Unspecified as local time,
    /// and it will therefore preserve the time when setting kind to utc.
    /// </summary>
    /// <param name="dt">The original DateTime</param>
    /// <returns>A new DateTime with DateTimeKind.Utc</returns>
    public static DateTime ToUtc(this DateTime dt)
    {
        return dt.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dt, DateTimeKind.Utc)
            : dt.ToUniversalTime();
    }
}
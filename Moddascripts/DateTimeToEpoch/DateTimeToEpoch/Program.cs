// See https://aka.ms/new-console-template for more information


DateTime utcNow = DateTime.UtcNow;

// Convert to Unix Epoch time (milliseconds since January 1, 1970)
long epochTimeMilliseconds = (long)(utcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

Console.WriteLine($"Current UTC Time: {utcNow}");
Console.WriteLine($"Epoch Time (Milliseconds): {epochTimeMilliseconds}");
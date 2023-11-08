// See https://aka.ms/new-console-template for more information

using System.Globalization;

Console.WriteLine("Input date time!");
var utcDateTimeString = Console.ReadLine();

// Specify the expected format of the input string
string format = "yyyy-MM-ddTHH:mm:ss.ffffffZ";

// Parse the string to a DateTime object
if (DateTime.TryParseExact(utcDateTimeString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime utcDateTime))
{
    Console.WriteLine($"Parsed UTC DateTime: {utcDateTime}");
}
else
{
    Console.WriteLine("Failed to parse the input string as a UTC DateTime.");
}

long ticks = utcDateTime.Ticks;

Console.WriteLine($"UTC DateTime: {utcDateTime}");
Console.WriteLine($"Ticks: {ticks}");
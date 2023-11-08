// See https://aka.ms/new-console-template for more information

using System.Text;
using Easee.Cos;
using Easee.IoT.DataTypes.Observations;

Console.WriteLine("Hello, World!");


CosWriter cosWriter = new();

var result = cosWriter.Serialize(new List<Observation>
{
    new Observation<int>(150, DateTime.UtcNow, 200),
});
// string byteString = BitConverter.ToString(result);
// Console.WriteLine("Method 1 - BitConverter.ToString:");
// Console.WriteLine(byteString);


// byte[] convert = Encoding.Convert(Encoding.UTF8, Encoding.UTF8, result);
// Console.WriteLine("HELLO:");
// Console.WriteLine("$");
foreach (var v in result)
{
    Console.Write(v.ToString("X"));
}
// Console.WriteLine("$");
// Console.WriteLine(convert.ToString());
// Console.WriteLine(Convert.ToBase64String(result));


// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Internal;
using Amazon.IotData;
using Amazon.IotData.Model;
using Amazon.Runtime.Endpoints;
using LocalCommandSender;
using DateTimeConverter = LocalCommandSender.DateTimeConverter;


string listOfserialNumbers = Environment.GetCommandLineArgs()[1];
var serialNumbers = listOfserialNumbers.Split(",");

var jsonOptions = new JsonSerializerOptions()
{
    Converters = { new DateTimeConverter() },
};
var key = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
var secret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var session = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN");
;

// var serialNumber = "EHFAAKSA";
var commandPayload =
    new CommandRequestPayload(1, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(15), Utils.GetBufferOverloadArgument());
// var commandPayload =
//     new CommandRequestPayload(1, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(15), Utils.GetBufferOverloadArgument());
var json = JsonSerializer.Serialize(commandPayload, jsonOptions);

var bytes = Encoding.UTF8.GetBytes(json);

foreach (var serialNumber in serialNumbers)
{
    Console.WriteLine($"serialNumber '{serialNumber}'");
    // prod: https://a1u0xyvhw6w6m8-ats.iot.eu-west-1.amazonaws.com
    // beta: afrwzkgi8a67v-ats.iot.eu-west-1.amazonaws.com
    var iotClient2 =
        new AmazonIotDataClient(key, secret, session, "https://a1u0xyvhw6w6m8-ats.iot.eu-west-1.amazonaws.com");
    var stream = new MemoryStream(bytes);
    var x = new PublishRequest()
    {
        Topic = $"{serialNumber}/C/{2}/{DateTime.UtcNow.Ticks}",
        Payload = stream,
        Qos = 0,
        MessageExpiry = (long)(commandPayload.ExpiresAt - DateTime.UtcNow).TotalSeconds
    };
    Thread.Sleep(1000);
    try
    {
        var response = await iotClient2.PublishAsync(x);
        Console.WriteLine($"Published message to topic '{x.Topic}'");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error publishing message: {ex.Message}");
    }
    Console.WriteLine($"Done with serialNumber '{serialNumber}'");
}
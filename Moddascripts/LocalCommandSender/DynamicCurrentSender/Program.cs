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
    Converters = {new DateTimeConverter()},
};
var key = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
var secret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var session = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN");
;

// var serialNumber = "EHFAAKSA";
var setDynamicCurrent = Utils.GetSetDeviceCli("net mode cell");
var commandPayload =
    new CommandRequestPayload(1, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(15), setDynamicCurrent.arguments);
var json = JsonSerializer.Serialize(commandPayload, jsonOptions);


foreach (var serialNumber in serialNumbers)
{
    Console.WriteLine($"serialNumber '{serialNumber}'");
    var iotClient2 =
        new AmazonIotDataClient(key, secret, session, "https://a1u0xyvhw6w6m8-ats.iot.eu-west-1.amazonaws.com");

    try
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        var stream = new MemoryStream(bytes);
        var x = new PublishRequest()
        {
            Topic = $"{serialNumber}/C/{setDynamicCurrent.commandId}/{DateTime.UtcNow.Ticks}",
            Payload = stream,
            Qos = 0,
            MessageExpiry = (long) (commandPayload.ExpiresAt - DateTime.UtcNow).TotalSeconds
        };
        var response = await iotClient2.PublishAsync(x);
        Console.WriteLine($"Published message to topic '{x.Topic}'");
        Thread.Sleep(10000);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error publishing message: {ex.Message}");
    }

    Console.WriteLine($"Done with serialNumber '{serialNumber}'");
}
// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.IotData;
using Amazon.IotData.Model;
using Easee.Cos;
using Easee.IoT.DataTypes.Observations;
using LocalObservationSender;

var serializerOptions = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new DateTimeJsonConverter("yyyy-MM-ddTHH:mm:ss.ffffffZ") },
};

string jsonString = @"{
  ""ChargeSession"": {
    ""Id"": 1,
    ""Start"": ""2023-9-25T13:4:24.000Z"",
    ""Stop"": ""2023-9-25T13:5:39.000Z"",
    ""Auth"": """",
    ""AuthReason"": 0,
    ""EnergyKWh"": 0,
    ""MeterValueStart"": 0.24,
    ""MeterValueStop"": 0.24,
    ""Unit"": ""kWh"",
    ""Sn"": ""test test""
  },
  ""Signature"": ""FishStick""
}";

var key = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
var secret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var session = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN"); ;

CosWriter cosWriter = new();

var chargeSession =  JsonSerializer.Deserialize<ChargeSessionWithSignature>(jsonString);
var chargeSessionWithSignature = JsonSerializer.Serialize(chargeSession);
var result = cosWriter.Serialize(new List<Observation>
{
    new Observation<string>(129, DateTime.UtcNow, chargeSessionWithSignature),
});
var stream = new MemoryStream(result);
var x = new PublishRequest()
{
    Topic = $"EH003764/OP/cos",
    Payload = stream,
    Qos = 1,
};

var iotClient2 = new AmazonIotDataClient(key, secret, session, "https://a1u0xyvhw6w6m8-ats.iot.eu-west-1.amazonaws.com");
try
{
    var response = await iotClient2.PublishAsync(x);
    Console.WriteLine($"Published message to topic '{x.Topic}'");
}
catch (Exception ex)
{
    Console.WriteLine($"Error publishing message: {ex.Message}");
}

using Amazon.IotData;
using Amazon.IotData.Model;
using Microsoft.Extensions.Logging;

namespace LocalCommandSender;

public interface IIotService
{
    Task<PublishResponse> PublishAsync(PublishRequest dataRequest, CancellationToken cancellationToken = default);
}

public class IoTService : IIotService
{
    private readonly IAmazonIotData _amazonIotClient;

    public IoTService( IAmazonIotData amazonIotClient)
    {
        _amazonIotClient = amazonIotClient;
    }

    public async Task<PublishResponse> PublishAsync(PublishRequest dataRequest, CancellationToken cancellationToken = default)
    {
        var publishTaskResponse = await _amazonIotClient.PublishAsync(dataRequest, cancellationToken);
        if ((int)publishTaskResponse.HttpStatusCode >= 300)
        {
            throw new Exception($"Failed to post message to {dataRequest.Topic}, status code: {publishTaskResponse.HttpStatusCode}.");
        }
        return publishTaskResponse;
    }
}
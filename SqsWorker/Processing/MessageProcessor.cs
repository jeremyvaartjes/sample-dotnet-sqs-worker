using SqsWorker.Entities;
using SqsWorker.MessageProviders;

namespace SqsWorker.Processing;

public class MessageProcessor(ILogger<MessageProcessor> logger, IMessageProvider provider)
{
    public async Task ProcessMessage(MyMessage message)
    {
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug($"Starting to process message {message.MessageId}");
        }

        await MainProcessing(message);
        await provider.DeleteMessage(message);
        
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug($"Finished processing message {message.MessageId}");
        }
    }

    private async Task MainProcessing(MyMessage message)
    {
        // TODO: replace this line with the code you would use to actually process a message
        await Task.Delay(message.TimeToSleep);
    }
}
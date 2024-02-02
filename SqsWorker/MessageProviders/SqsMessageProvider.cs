using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsWorker.Entities;

namespace SqsWorker.MessageProviders;

public class SqsMessageProvider(SqsOptions options) : IMessageProvider
{
    private readonly AmazonSQSClient _sqs = new();

    public async Task<List<MyMessage>> ReceiveMessages(CancellationToken cancellationToken = default)
    {
        var response = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = options.QueueUrl,
            MaxNumberOfMessages = options.MaxMessages,
            WaitTimeSeconds = 20
        }, cancellationToken);

        return response.Messages
            .Select(msg => new
            {
                Body = JsonSerializer.Deserialize<MessageBody>(msg.Body), msg.ReceiptHandle
            })
            .Where(msg => msg.Body != null)
            .Select(msg => new MyMessage(msg.Body!.MessageId, TimeSpan.FromSeconds(msg.Body.TimeToSleep), msg.ReceiptHandle))
            .ToList();
    }

    public async Task DeleteMessage(MyMessage message, CancellationToken cancellationToken = default)
    {
        await _sqs.DeleteMessageAsync(options.QueueUrl, message.MessageHandle, cancellationToken);
    }
    
    private class MessageBody
    {
        public Guid MessageId { get; set; }
        public double TimeToSleep { get; set; }
    }
}
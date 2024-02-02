using SqsWorker.Entities;

namespace SqsWorker.MessageProviders;

public class FakeMessageProvider(SqsOptions options) : IMessageProvider
{
    private readonly Random _rand = new ();
    
    public Task<List<MyMessage>> ReceiveMessages(CancellationToken cancellationToken = default)
    {
        var messages = new List<MyMessage>();

        for (int i = 0; i < options.MaxMessages; i++)
        {
            messages.Add(new MyMessage(
                Guid.NewGuid(), 
                TimeSpan.FromSeconds(_rand.Next(1,12)), 
                "some-handle"));
        }

        return Task.FromResult(messages);
    }

    public Task DeleteMessage(MyMessage message, CancellationToken cancellationToken = default)
    {
        // no-op
        return Task.CompletedTask;
    }
}
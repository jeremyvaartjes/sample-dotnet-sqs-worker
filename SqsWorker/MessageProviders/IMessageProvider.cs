using SqsWorker.Entities;

namespace SqsWorker.MessageProviders;

public interface IMessageProvider
{
    public Task<List<MyMessage>> ReceiveMessages(CancellationToken cancellationToken = default);
    public Task DeleteMessage(MyMessage message, CancellationToken cancellationToken = default);
}
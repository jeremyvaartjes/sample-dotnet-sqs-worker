namespace SqsWorker.Entities;

public record MyMessage(Guid MessageId, TimeSpan TimeToSleep, string MessageHandle);
namespace SqsWorker.Entities;

public class SqsOptions
{
    public bool UseRealSqs { get; set; }
    public int MaxMessages { get; set; }
    public string? QueueUrl { get; set; }
}
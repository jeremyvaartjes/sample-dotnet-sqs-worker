using SqsWorker.MessageProviders;
using SqsWorker.Processing;

namespace SqsWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly MessageProcessor _processor;
    private readonly IMessageProvider _provider;

    public Worker(ILogger<Worker> logger, MessageProcessor processor, IMessageProvider provider)
    {
        _logger = logger;
        _processor = processor;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Worker starting new processing iteration at: {time}", DateTimeOffset.Now);
            }

            var messages = await _provider.ReceiveMessages(stoppingToken);
            var messageTasks = messages.Select(mes => _processor.ProcessMessage(mes)).ToArray();
            
            Task.WaitAll(messageTasks, stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
using SqsWorker;
using SqsWorker.Entities;
using SqsWorker.MessageProviders;
using SqsWorker.Processing;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<MessageProcessor>();

var sqsOptions = builder.Configuration.GetSection("SqsOptions").Get<SqsOptions>();

if (sqsOptions == null)
{
    throw new Exception("Worker options not provided");
}

builder.Services.AddSingleton(sqsOptions);

if (sqsOptions.UseRealSqs)
{
    builder.Services.AddSingleton<IMessageProvider, SqsMessageProvider>();
}
else
{
    builder.Services.AddSingleton<IMessageProvider, FakeMessageProvider>();
}

var host = builder.Build();
host.Run();
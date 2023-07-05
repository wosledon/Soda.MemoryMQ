using Soda.MemoryMQ.Interfacure;

namespace Soda.MemoryMQ.Samples.Services;

public class MemoryMQService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MemoryMQService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scoped = _serviceProvider.CreateScope();
        var consumer = scoped.ServiceProvider.GetRequiredService<IMessageConsumer<string>>();

        consumer.OnMessage("test", async (message) =>
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"Key: test, Value: {message.ToString()}");
            }, stoppingToken);
        });

        return Task.CompletedTask;
    }
}
using System.Threading;
using System.Threading.Tasks;
using Soda.MemoryMQ.Interfacure;

namespace Soda.MemoryMQ;

public class MessageProducer<TKey, TValue> : IMessageProducer<TKey, TValue>
{
    private readonly IMemoryMQChannelPool _memoryMqChannelPool;

    public MessageProducer(IMemoryMQChannelPool memoryMqChannelPool)
    {
        _memoryMqChannelPool = memoryMqChannelPool;
    }

    public void Dispose()
    {
    }

    public async ValueTask ProduceAsync(string channel, TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        await _memoryMqChannelPool.Rent<TKey, TValue>(channel).Channel.Writer.WriteAsync((key, value), cancellationToken);
    }
}

public class MessageProducer<TValue> : IMessageProducer<TValue>
{
    private readonly IMemoryMQChannelPool _memoryMqChannelPool;

    public MessageProducer(IMemoryMQChannelPool memoryMqChannelPool)
    {
        _memoryMqChannelPool = memoryMqChannelPool;
    }

    public void Dispose()
    {
    }

    public async ValueTask ProduceAsync(string channel, TValue value, CancellationToken cancellationToken = default)
    {
        await _memoryMqChannelPool.Rent<TValue>(channel).Channel.Writer.WriteAsync(value, cancellationToken);
    }
}
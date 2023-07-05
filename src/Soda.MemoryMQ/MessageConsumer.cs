using Soda.MemoryMQ.Interfacure;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Soda.MemoryMQ;

public class MessageConsumer<TKey, TValue> : IMessageConsumer<TKey, TValue>
{
    private readonly IMemoryMQChannelPool _memoryMqChannelPool;

    public MessageConsumer(IMemoryMQChannelPool memoryMqChannelPool)
    {
        _memoryMqChannelPool = memoryMqChannelPool;
    }

    public CancellationTokenSource Cts { get; } = new CancellationTokenSource();

    public CancellationTokenSource Cancel()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }

    public void OnMessage(string channel, Action<(TKey, TValue)> callback)
    {
        Task.Run(async () =>
        {
            while (!Cts.IsCancellationRequested)
            {
                var reader = await _memoryMqChannelPool.Rent<TKey, TValue>(channel).Channel.Reader.ReadAsync(Cts.Token);
                callback(reader);
            }
        });
    }
}

public class MessageConsumer<TValue> : IMessageConsumer<TValue>
{
    private readonly IMemoryMQChannelPool _memoryMqChannelPool;

    public MessageConsumer(IMemoryMQChannelPool memoryMqChannelPool)
    {
        _memoryMqChannelPool = memoryMqChannelPool;
    }

    public CancellationTokenSource Cts { get; } = new CancellationTokenSource();

    public CancellationTokenSource Cancel()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }

    public void OnMessage(string channel, Action<TValue> callback)
    {
        Task.Run(async () =>
        {
            while (!Cts.IsCancellationRequested)
            {
                var reader = await _memoryMqChannelPool.Rent<TValue>(channel).Channel.Reader.ReadAsync(Cts.Token);
                callback(reader);
            }
        });
    }
}
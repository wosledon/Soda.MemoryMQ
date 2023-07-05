using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Soda.MemoryMQ.Interfacure;

// ReSharper disable once InconsistentNaming
public interface IMemoryMQChannelPool
{
    MessageChannel<TKey, TValue> Rent<TKey, TValue>(string channel);

    MessageChannel<TValue> Rent<TValue>(string channel);
}

// ReSharper disable once InconsistentNaming
public class MemoryMQChannelPool : IMemoryMQChannelPool
{
    private static ConcurrentDictionary<string, IChannel> Pool = new();

    public MessageChannel<TKey, TValue> Rent<TKey, TValue>(string channel)
    {
        if (Pool.TryGetValue(channel, out var extChannel)) return (MessageChannel<TKey, TValue>)extChannel;

        extChannel = new MessageChannel<TKey, TValue>();

        Pool.TryAdd(channel, extChannel);

        return (MessageChannel<TKey, TValue>)extChannel;
    }

    public MessageChannel<TValue> Rent<TValue>(string channel)
    {
        if (Pool.TryGetValue(channel, out var extChannel)) return (MessageChannel<TValue>)extChannel;

        extChannel = new MessageChannel<TValue>();

        Pool.TryAdd(channel, extChannel);

        return (MessageChannel<TValue>)extChannel;
    }
}

public interface IMessageConsumer<TKey, TValue> : IDisposable
{
    void OnMessage(string channel, Action<(TKey, TValue)> callback);

    CancellationTokenSource Cancel();
}

public interface IMessageConsumer<out TValue> : IDisposable
{
    void OnMessage(string channel, Action<TValue> callback);

    CancellationTokenSource Cancel();
}

public interface IMessageProducer<in TKey, in TValue> : IDisposable
{
    ValueTask ProduceAsync(string channel, TKey key, TValue value, CancellationToken cancellationToken = default);
}

public interface IMessageProducer<in TValue> : IDisposable
{
    ValueTask ProduceAsync(string channel, TValue value, CancellationToken cancellationToken = default);
}

public interface IPubSub
{
    /// <summary>
    /// 主题
    /// </summary>
    /// <value></value>
    string TopicName { get; }
}

public interface IChannel
{
}

public class MessageChannel<TKey, TValue> : IChannel
{
    public Channel<(TKey, TValue)> Channel { get; }

    public MessageChannel()
    {
        Channel = System.Threading.Channels.Channel.CreateUnbounded<(TKey, TValue)>();
    }
}

public class MessageChannel<TValue> : IChannel
{
    public Channel<TValue> Channel { get; }

    public MessageChannel()
    {
        Channel = System.Threading.Channels.Channel.CreateUnbounded<TValue>();
    }
}
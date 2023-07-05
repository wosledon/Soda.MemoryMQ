using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soda.MemoryMQ.Interfacure;

namespace Soda.MemoryMQ.Extensions;

// ReSharper disable once InconsistentNaming
public static class MemoryMQExtensions
{
    internal static IServiceCollection AddMemoryMQProducer(this IServiceCollection service)
    {
        service.AddScoped(typeof(IMessageProducer<,>), typeof(MessageProducer<,>));
        service.AddScoped(typeof(IMessageProducer<>), typeof(MessageProducer<>));
        return service;
    }

    internal static IServiceCollection AddMemoryMQConsumer(this IServiceCollection service)
    {
        service.AddScoped(typeof(IMessageConsumer<,>), typeof(MessageConsumer<,>));
        service.AddScoped(typeof(IMessageConsumer<>), typeof(MessageConsumer<>));
        return service;
    }

    public static IServiceCollection AddMemoryMQ(this IServiceCollection service)
    {
        service.TryAddSingleton<IMemoryMQChannelPool, MemoryMQChannelPool>();
        service.AddMemoryMQProducer();
        service.AddMemoryMQConsumer();

        return service;
    }
}
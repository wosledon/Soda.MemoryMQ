# Soda.MemoryMQ

支持多Channel、key-value。

用法:
```csharp
builder.Services.AddMemoryMQ();
``

生产者:
```csharp
await _producer.ProduceAsync(channel, value);
``

```csharp
await _producer.ProduceAsync(channel, key, value);
``

消费者:
```csharp
consumer.OnMessage("test", async (message) =>
{
    await Task.Run(() =>
    {
        Console.WriteLine($"Key: test, Value: {message.ToString()}");
    }, stoppingToken);
});
``
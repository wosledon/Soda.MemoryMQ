# Soda.MemoryMQ

֧�ֶ�Channel��key-value��

�÷�:
```csharp
builder.Services.AddMemoryMQ();
``

������:
```csharp
await _producer.ProduceAsync(channel, value);
``

```csharp
await _producer.ProduceAsync(channel, key, value);
``

������:
```csharp
consumer.OnMessage("test", async (message) =>
{
    await Task.Run(() =>
    {
        Console.WriteLine($"Key: test, Value: {message.ToString()}");
    }, stoppingToken);
});
``
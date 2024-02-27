namespace Ais.Receiver.EventBus
{
    internal interface IMessageQueueClient
    {
        Task PublishAsync(IEnumerable<string> messages);
    }
}

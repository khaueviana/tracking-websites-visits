namespace Infrastructure.CrossCutting.Interfaces
{
    using Application.Contracts.Events.Interfaces;

    public interface IMessageBroker
    {
        void Dispatch<T>(T message)
            where T : IEvent;

        void Consume<T>(Action<T> handler)
            where T : IEvent;
    }
}
namespace Infrastructure.CrossCutting.Interfaces
{
    public interface IMessageBroker
    {
        void Dispatch<T>(T message);

        void Consume<T>(Action<T> handler);
    }
}
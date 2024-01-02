namespace Gateway.Messaging.RabbitMQ
{
    using System.Text;
    using System.Text.Json;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;
    using Infrastructure.CrossCutting.Interfaces;

    public class RabbitMQBroker : IMessageBroker
    {
        private readonly IConnection connection;
        private readonly IMessageBrokerSettings messageBrokerSettings;

        public RabbitMQBroker(IMessageBrokerSettings messageBrokerSettings)
        {
            this.messageBrokerSettings = messageBrokerSettings;

            var connectionFactory = new ConnectionFactory
            {
                HostName = messageBrokerSettings.HostName,
                Port = messageBrokerSettings.Port,
                UserName = messageBrokerSettings.UserName,
                Password = messageBrokerSettings.Password,
            };

            this.connection = connectionFactory.CreateConnection();
        }

        public void Dispatch<T>(T message)
        {
            using var channel = this.connection.CreateModel();

            channel.QueueDeclare(queue: this.messageBrokerSettings.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: this.messageBrokerSettings.Queue,
                basicProperties: null,
                body: body);
        }

        public void Consume<T>(Action<T> handler)
        {
            var channel = this.connection.CreateModel();

            channel.QueueDeclare(queue: this.messageBrokerSettings.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(messageJson);

                handler(message);
            };

            channel.BasicConsume(queue: this.messageBrokerSettings.Queue, autoAck: true, consumer: consumer);
        }
    }
}
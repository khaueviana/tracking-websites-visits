namespace Infrastructure.CrossCutting.Interfaces
{
    public interface IMessageBrokerSettings
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Queue { get; set; }

        public int ConsumerIntervalInMs { get; set; }
    }
}
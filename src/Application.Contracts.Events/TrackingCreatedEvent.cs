namespace Application.Contracts.Events
{
    using Application.Contracts.Events.Interfaces;

    public class TrackingCreatedEvent : IEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Version { get; set; } = "1.0.0";

        public string Referrer { get; set; }

        public string UserAgent { get; set; }

        public string IpAddress { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
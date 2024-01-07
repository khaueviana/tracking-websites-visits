namespace Infrastructure.CrossCutting.Interfaces
{
    public interface IEvent
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Version { get; set; }
    }
}
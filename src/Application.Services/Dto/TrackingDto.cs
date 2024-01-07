namespace Application.Services.Dto
{
    public class TrackingDto
    {
        public string Referrer { get; set; }

        public string UserAgent { get; set; }

        public string IpAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
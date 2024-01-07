namespace Domain.Model
{
    using Infrastructure.CrossCutting.Extensions;

    public class Tracking
    {
        private Tracking(string referrer, string userAgent, string ipAddress, DateTime createdAt)
        {
            this.Referrer = referrer;
            this.UserAgent = userAgent;
            this.IpAddress = ipAddress;
            this.CreatedAt = createdAt;
        }

        public string Referrer { get; }

        public string UserAgent { get; }

        public string IpAddress { get; }

        public DateTime CreatedAt { get; }

        public string Summary => $"{this.CreatedAt.ToIso8601String()} | {this.Referrer.NullIfEmpty()} | {this.UserAgent.NullIfEmpty()} | {this.IpAddress}";

        public static Tracking Create(string referrer, string userAgent, string ipAddress, DateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return default;
            }

            return new Tracking(referrer, userAgent, ipAddress, createdAt);
        }
    }
}
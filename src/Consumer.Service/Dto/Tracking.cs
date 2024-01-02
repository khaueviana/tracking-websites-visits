namespace Consumer.Service.Dto
{
    using Infrastructure.CrossCutting.Extensions;

    public class Tracking
    {
        public string Referrer { get; set; }

        public string UserAgent { get; set; }

        public string IpAddress { get; set; }

        public string CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{this.CreatedAt} | {this.Referrer.NullIfEmpty()} | {this.UserAgent.NullIfEmpty()} | {this.IpAddress}";
        }
    }
}
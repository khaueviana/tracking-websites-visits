namespace Presentation.Api.Dto
{
    using System.Globalization;

    public class Tracking
    {
        public string Referrer { get; set; }

        public string UserAgent { get; set; }

        public string IpAddress { get; set; }

        public string CreatedAt => DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
    }
}
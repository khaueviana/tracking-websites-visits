namespace Presentation.Api.Endpoints
{
    using Infrastructure.CrossCutting.Interfaces;
    using Infrastructure.CrossCutting.Media;
    using Presentation.Api.Dto;

    public static class TrackingEndpoints
    {
        public static void MapTrackingEndpoints(this WebApplication app)
        {
            app.MapGet("/track", GetTrackingPixel)
               .WithName("Track")
               .WithOpenApi();
        }

        public static IResult GetTrackingPixel(HttpContext httpContext, IMessageBroker messageBroker)
        {
            var tracking = new Tracking
            {
                Referrer = httpContext.Request.Headers.Referer.ToString(),
                UserAgent = httpContext.Request.Headers.UserAgent.ToString(),
                IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            };

            messageBroker.Dispatch(tracking);

            return Results.File(ImageFactory.CreateTransparentGif(), MimeTypeConstants.Gif);
        }
    }
}
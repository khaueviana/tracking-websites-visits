namespace Presentation.Api.Endpoints
{
    using Application.Services.Dto;
    using Application.Services.Interfaces;
    using Infrastructure.CrossCutting.Media;

    public static class TrackingEndpoints
    {
        public static void MapTrackingEndpoints(this WebApplication app)
        {
            app.MapGet("/track", GetTrackingPixel)
               .WithName("Track")
               .WithOpenApi();
        }

        public static IResult GetTrackingPixel(HttpContext httpContext, ITrackingApplicationService trackingApplicationService)
        {
            var tracking = new TrackingDto
            {
                Referrer = httpContext.Request.Headers.Referer.ToString(),
                UserAgent = httpContext.Request.Headers.UserAgent.ToString(),
                IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            };

            trackingApplicationService.Dispatch(tracking);

            return Results.File(ImageFactory.CreateTransparentGif(), MimeTypeConstants.Gif);
        }
    }
}
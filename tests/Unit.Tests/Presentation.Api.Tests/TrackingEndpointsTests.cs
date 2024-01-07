namespace Presentation.Api.Tests
{
    using System.Net;
    using Application.Services.Dto;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Presentation.Api.Endpoints;

    public class TrackingEndpointsTests
    {
        private readonly Mock<ITrackingApplicationService> trackingApplicationServiceMock;
        private readonly Mock<HttpContext> httpContextMock;
        private readonly Mock<HttpRequest> httpRequestMock;
        private readonly Fixture fixture;

        public TrackingEndpointsTests()
        {
            this.trackingApplicationServiceMock = new Mock<ITrackingApplicationService>();
            this.httpContextMock = new Mock<HttpContext>();
            this.httpRequestMock = new Mock<HttpRequest>();
            this.fixture = new Fixture();
        }

        [Fact]
        public void GetTrackingPixel_WithValidHttpContext_DispatchesTrackingInfoAndReturnsTransparentGif()
        {
            // Arrange
            var trackingDtoDispatched = default(TrackingDto);
            var expectedReferer = this.fixture.Create<string>();
            var expectedUserAgent = this.fixture.Create<string>();
            var expectedIpAddress = "192.168.1.1";

            var expectedHeaders = new HeaderDictionary
            {
                { "Referer", expectedReferer },
                { "User-Agent", expectedUserAgent },
            };

            this.httpRequestMock.Setup(x => x.Headers).Returns(expectedHeaders);
            this.httpContextMock.Setup(x => x.Connection.RemoteIpAddress).Returns(IPAddress.Parse(expectedIpAddress));
            this.httpContextMock.Setup(x => x.Request).Returns(this.httpRequestMock.Object);

            this.trackingApplicationServiceMock.Setup(x => x.Dispatch(It.IsAny<TrackingDto>()))
                                               .Callback<TrackingDto>(tracking => trackingDtoDispatched = tracking)
                                               .Verifiable();

            // Act
            var result = TrackingEndpoints.GetTrackingPixel(this.httpContextMock.Object, this.trackingApplicationServiceMock.Object);

            // Assert
            result.Should().NotBeNull();

            var fileResult = result as FileContentHttpResult;
            fileResult.Should().NotBeNull();
            fileResult.ContentType.Should().Be("image/gif");
            fileResult.FileContents.ToArray().Should().BeEquivalentTo(Convert.FromBase64String("R0lGODlhAQABAIAAAAUEBAAAACwAAAAAAQABAAACAkQBADs="));

            this.trackingApplicationServiceMock.Verify();

            trackingDtoDispatched.Should().NotBeNull();
            trackingDtoDispatched.Referrer.Should().Be(expectedReferer);
            trackingDtoDispatched.UserAgent.Should().Be(expectedUserAgent);
            trackingDtoDispatched.IpAddress.Should().Be(expectedIpAddress);
            trackingDtoDispatched.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
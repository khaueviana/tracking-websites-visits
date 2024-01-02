namespace Presentation.Api.Tests
{
    using System.Globalization;
    using System.Net;
    using Infrastructure.CrossCutting.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Presentation.Api.Dto;
    using Presentation.Api.Endpoints;

    public class TrackingEndpointsTests
    {
        private readonly Mock<IMessageBroker> messageBrokerMock;
        private readonly Mock<HttpContext> httpContextMock;
        private readonly Mock<HttpRequest> httpRequestMock;
        private readonly Fixture fixture;

        public TrackingEndpointsTests()
        {
            this.messageBrokerMock = new Mock<IMessageBroker>();
            this.httpContextMock = new Mock<HttpContext>();
            this.httpRequestMock = new Mock<HttpRequest>();
            this.fixture = new Fixture();
        }

        [Fact]
        public void GetTrackingPixel_WithValidHttpContext_DispatchesTrackingInfoAndReturnsTransparentGif()
        {
            // Arrange
            var sentTracking = default(Tracking);
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

            this.messageBrokerMock.Setup(x => x.Dispatch(It.IsAny<Tracking>()))
                                               .Callback<object>(tracking => sentTracking = (Tracking)tracking)
                                               .Verifiable();

            // Act
            var result = TrackingEndpoints.GetTrackingPixel(this.httpContextMock.Object, this.messageBrokerMock.Object);

            // Assert
            result.Should().NotBeNull();

            var fileResult = result as FileContentHttpResult;
            fileResult.Should().NotBeNull();
            fileResult.ContentType.Should().Be("image/gif");
            fileResult.FileContents.ToArray().Should().BeEquivalentTo(Convert.FromBase64String("R0lGODlhAQABAIAAAAUEBAAAACwAAAAAAQABAAACAkQBADs="));

            this.messageBrokerMock.Verify();

            sentTracking.Should().NotBeNull();

            sentTracking.Referrer.Should().Be(expectedReferer);
            sentTracking.UserAgent.Should().Be(expectedUserAgent);
            sentTracking.IpAddress.Should().Be(expectedIpAddress);

            var sentTrackingCreatedAt = DateTime.Parse(sentTracking.CreatedAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            sentTrackingCreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
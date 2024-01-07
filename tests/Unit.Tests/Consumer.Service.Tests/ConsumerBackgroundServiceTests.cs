namespace Consumer.Service.Tests
{
    using Application.Contracts.Events;
    using Application.Services.Dto;
    using Application.Services.Interfaces;
    using Infrastructure.CrossCutting.Interfaces;

    public class ConsumerBackgroundServiceTests
    {
        private readonly Mock<IMessageBroker> messageBrokerMock;
        private readonly Mock<IMessageBrokerSettings> messageBrokerSettingsMock;
        private readonly Mock<ITrackingApplicationService> trackingApplicationServiceMock;
        private readonly Fixture fixture;
        private readonly CancellationTokenSource stoppingToken;

        public ConsumerBackgroundServiceTests()
        {
            this.messageBrokerMock = new Mock<IMessageBroker>();
            this.messageBrokerSettingsMock = new Mock<IMessageBrokerSettings>();
            this.trackingApplicationServiceMock = new Mock<ITrackingApplicationService>();
            this.fixture = new Fixture();
            this.stoppingToken = new CancellationTokenSource();
        }

        [Fact]
        public async Task ExecuteAsync_WithValidTracking_InsertsTrackingInfoToFileRepository()
        {
            // Arrange
            var trackingCreatedEvent = this.fixture.Create<TrackingCreatedEvent>();
            var trackingDtoInserted = default(TrackingDto);
            var consumerInterval = 1000;

            this.messageBrokerSettingsMock.SetupGet(x => x.ConsumerIntervalInMs).Returns(consumerInterval);

            this.messageBrokerMock.Setup(x => x.Consume(It.IsAny<Action<TrackingCreatedEvent>>()))
                                               .Callback<Action<TrackingCreatedEvent>>(consumeAction => consumeAction(trackingCreatedEvent));

            this.trackingApplicationServiceMock.Setup(x => x.InsertAsync(It.IsAny<TrackingDto>()))
                                                .Callback<TrackingDto>(trackingDto => trackingDtoInserted = trackingDto);

            var messageConsumer = new ConsumerBackgroundService(this.messageBrokerMock.Object, this.messageBrokerSettingsMock.Object, this.trackingApplicationServiceMock.Object);

            // Act
            var executeAsyncTask = messageConsumer.StartAsync(this.stoppingToken.Token);
            await this.WaitForConsumerToFinish(executeAsyncTask);

            // Assert
            this.trackingApplicationServiceMock.Verify(x => x.InsertAsync(It.IsAny<TrackingDto>()), Times.Once());
            trackingDtoInserted.Should().BeEquivalentTo(trackingCreatedEvent, o => o.ExcludingMissingMembers());
        }

        private async Task WaitForConsumerToFinish(Task executeAsyncTask)
        {
            await Task.Delay(500);

            this.stoppingToken.Cancel();

            await executeAsyncTask;
        }
    }
}
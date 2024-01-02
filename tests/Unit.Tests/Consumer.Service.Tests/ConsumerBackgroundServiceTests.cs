namespace Consumer.Service.Tests
{
    using Consumer.Service.Dto;
    using Infrastructure.CrossCutting.Interfaces;

    public class ConsumerBackgroundServiceTests
    {
        private readonly Mock<IMessageBroker> messageBrokerMock;
        private readonly Mock<IMessageBrokerSettings> messageBrokerSettingsMock;
        private readonly Mock<IFileRepository> fileRepositoryMock;
        private readonly Fixture fixture;
        private readonly CancellationTokenSource stoppingToken;

        public ConsumerBackgroundServiceTests()
        {
            this.messageBrokerMock = new Mock<IMessageBroker>();
            this.messageBrokerSettingsMock = new Mock<IMessageBrokerSettings>();
            this.fileRepositoryMock = new Mock<IFileRepository>();
            this.fixture = new Fixture();
            this.stoppingToken = new CancellationTokenSource();
        }

        [Fact]
        public async Task ExecuteAsync_WithValidTracking_InsertsTrackingInfoToFileRepository()
        {
            // Arrange
            var tracking = this.fixture.Create<Tracking>();
            var consumerInterval = 1000;

            this.messageBrokerSettingsMock.SetupGet(x => x.ConsumerIntervalInMs).Returns(consumerInterval);

            this.messageBrokerMock.Setup(x => x.Consume(It.IsAny<Action<Tracking>>()))
                                               .Callback<Action<Tracking>>(consumeAction => consumeAction(tracking));

            var messageConsumer = new ConsumerBackgroundService(this.messageBrokerMock.Object, this.messageBrokerSettingsMock.Object, this.fileRepositoryMock.Object);

            // Act
            var executeAsyncTask = messageConsumer.StartAsync(this.stoppingToken.Token);
            await this.WaitForConsumerToFinish(executeAsyncTask);

            // Assert
            this.fileRepositoryMock.Verify(x => x.InsertAsync(It.Is<string>(s => s.Contains(tracking.IpAddress))), Times.Once());
        }

        private async Task WaitForConsumerToFinish(Task executeAsyncTask)
        {
            await Task.Delay(500);

            this.stoppingToken.Cancel();

            await executeAsyncTask;
        }
    }
}
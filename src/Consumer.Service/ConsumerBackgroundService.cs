namespace Consumer.Service
{
    using Application.Contracts.Events;
    using Application.Services.Dto;
    using Application.Services.Interfaces;
    using Infrastructure.CrossCutting.Interfaces;
    using Microsoft.Extensions.Hosting;

    public class ConsumerBackgroundService(IMessageBroker messageBroker, IMessageBrokerSettings messageBrokerSettings, ITrackingApplicationService trackingApplicationService) : BackgroundService
    {
        private readonly IMessageBroker messageBroker = messageBroker;
        private readonly IMessageBrokerSettings messageBrokerSettings = messageBrokerSettings;
        private readonly ITrackingApplicationService trackingApplicationService = trackingApplicationService;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.messageBroker.Consume(async (TrackingCreatedEvent trackingCreatedEvent) =>
            {
                var trackingDto = new TrackingDto
                {
                    Referrer = trackingCreatedEvent.Referrer,
                    UserAgent = trackingCreatedEvent.UserAgent,
                    IpAddress = trackingCreatedEvent.IpAddress,
                    CreatedAt = trackingCreatedEvent.CreatedAt,
                };

                await this.trackingApplicationService.InsertAsync(trackingDto);
            });

            await this.WaitForCancellation(stoppingToken);
        }

        private async Task WaitForCancellation(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(this.messageBrokerSettings.ConsumerIntervalInMs, stoppingToken);
            }
        }
    }
}
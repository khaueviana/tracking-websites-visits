namespace Consumer.Service
{
    using Consumer.Service.Dto;
    using Infrastructure.CrossCutting.Interfaces;
    using Microsoft.Extensions.Hosting;

    public class ConsumerBackgroundService(IMessageBroker messageBroker, IMessageBrokerSettings messageBrokerSettings, IFileRepository fileRepository) : BackgroundService
    {
        private readonly IMessageBroker messageBroker = messageBroker;
        private readonly IMessageBrokerSettings messageBrokerSettings = messageBrokerSettings;
        private readonly IFileRepository fileRepository = fileRepository;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.messageBroker.Consume(async (Tracking tracking) =>
            {
                if (string.IsNullOrWhiteSpace(tracking?.IpAddress))
                {
                    return;
                }

                await this.fileRepository.InsertAsync(tracking.ToString());
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
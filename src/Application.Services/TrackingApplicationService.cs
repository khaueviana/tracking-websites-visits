namespace Application.Services
{
    using Application.Contracts.Events;
    using Application.Services.Dto;
    using Application.Services.Interfaces;
    using Domain.Model;
    using Infrastructure.CrossCutting.Interfaces;

    public class TrackingApplicationService(IMessageBroker messageBroker, IFileRepository fileRepository) : ITrackingApplicationService
    {
        private readonly IMessageBroker messageBroker = messageBroker;
        private readonly IFileRepository fileRepository = fileRepository;

        public void Dispatch(TrackingDto trackingDto)
        {
            var trackingCreated = new TrackingCreatedEvent
            {
                Referrer = trackingDto.Referrer,
                UserAgent = trackingDto.UserAgent,
                IpAddress = trackingDto.IpAddress,
                CreatedAt = trackingDto.CreatedAt,
            };

            this.messageBroker.Dispatch(trackingCreated);
        }

        public async Task InsertAsync(TrackingDto trackingDto)
        {
            var tracking = Tracking.Create(trackingDto.Referrer, trackingDto.UserAgent, trackingDto.IpAddress, trackingDto.CreatedAt);

            if (tracking is not null)
            {
                await this.fileRepository.InsertAsync(tracking.Summary);
            }
        }
    }
}
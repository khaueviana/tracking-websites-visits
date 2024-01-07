namespace Application.Services
{
    using Application.Contracts.Events;
    using Application.Services.Dto;
    using Application.Services.Interfaces;
    using Domain.Model;
    using Domain.Model.Interfaces;
    using Infrastructure.CrossCutting.Interfaces;

    public class TrackingApplicationService(IMessageBroker messageBroker, ITrackingRepository trackingRepository) : ITrackingApplicationService
    {
        private readonly IMessageBroker messageBroker = messageBroker;
        private readonly ITrackingRepository trackingRepository = trackingRepository;

        public void Dispatch(TrackingDto trackingDto)
        {
            var trackingCreatedEvent = new TrackingCreatedEvent
            {
                Referrer = trackingDto.Referrer,
                UserAgent = trackingDto.UserAgent,
                IpAddress = trackingDto.IpAddress,
                CreatedAt = trackingDto.CreatedAt,
            };

            this.messageBroker.Dispatch(trackingCreatedEvent);
        }

        public async Task InsertAsync(TrackingDto trackingDto)
        {
            var tracking = Tracking.Create(trackingDto.Referrer, trackingDto.UserAgent, trackingDto.IpAddress, trackingDto.CreatedAt);

            await this.trackingRepository.InsertAsync(tracking);
        }
    }
}
namespace Application.Services.Interfaces
{
    using Application.Services.Dto;

    public interface ITrackingApplicationService
    {
        void Dispatch(TrackingDto trackingDto);

        Task InsertAsync(TrackingDto trackingDto);
    }
}
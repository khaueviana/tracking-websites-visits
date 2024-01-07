namespace Domain.Model.Interfaces
{
    public interface ITrackingRepository
    {
        Task InsertAsync(Tracking tracking);
    }
}
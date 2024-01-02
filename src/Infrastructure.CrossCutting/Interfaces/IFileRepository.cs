namespace Infrastructure.CrossCutting.Interfaces
{
    public interface IFileRepository
    {
        Task InsertAsync(string text);
    }
}
namespace Gateway.Data.File
{
    using Domain.Model;
    using Domain.Model.Interfaces;
    using Infrastructure.CrossCutting.Interfaces;

    public class FileRepository(IFileRepositorySettings fileRepositorySettings) : ITrackingRepository
    {
        private readonly IFileRepositorySettings fileRepositorySettings = fileRepositorySettings;

        public async Task InsertAsync(Tracking tracking)
        {
            if (tracking is null)
            {
                return;
            }

            var directory = Path.GetDirectoryName(this.fileRepositorySettings.Path);

            Directory.CreateDirectory(directory);

            using var writer = new StreamWriter(this.fileRepositorySettings.Path, append: true);

            await writer.WriteLineAsync(tracking.Summary);
        }
    }
}
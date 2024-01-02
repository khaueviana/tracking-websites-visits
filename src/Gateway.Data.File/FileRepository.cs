namespace Gateway.Data.File
{
    using Infrastructure.CrossCutting.Interfaces;

    public class FileRepository(IFileRepositorySettings fileRepositorySettings) : IFileRepository
    {
        private readonly IFileRepositorySettings fileRepositorySettings = fileRepositorySettings;

        public async Task InsertAsync(string text)
        {
            var directory = Path.GetDirectoryName(this.fileRepositorySettings.Path);

            Directory.CreateDirectory(directory);

            using var writer = new StreamWriter(this.fileRepositorySettings.Path, append: true);

            await writer.WriteLineAsync(text);
        }
    }
}
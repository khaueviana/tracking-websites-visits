using Infrastructure.CrossCutting.Interfaces;

namespace Infrastructure.CrossCutting.Settings
{
    public class FileRepositorySettings : IFileRepositorySettings
    {
        public string Path { get; set; }
    }
}
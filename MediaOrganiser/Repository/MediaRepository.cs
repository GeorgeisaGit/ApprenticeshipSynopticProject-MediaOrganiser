using System.Collections.Generic;
using MediaOrganiser.Config;
using MediaOrganiser.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediaOrganiser.Repository
{
    public class MediaRepository : IMediaRepository
    {
        private readonly ILogger<MediaRepository> _logger;
        private RootDirectoryOptions _config;
        public MediaRepository(ILogger<MediaRepository> logger, IOptions<RootDirectoryOptions> config)
        {
            _logger = logger;
            _config = config.Value;
        }
        public List<MediaFile> GetAllMediaFiles()
        {
            _logger.LogInformation("{}", _config.RootPath);
            return new List<MediaFile>();
        }

        public bool DeleteMediaFiles(List<string> FQNs)
        {
            throw new System.NotImplementedException();
        }
    }
}
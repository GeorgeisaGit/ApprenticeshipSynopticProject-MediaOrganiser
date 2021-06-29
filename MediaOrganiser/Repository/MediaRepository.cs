using System.Collections.Generic;
using System.IO;
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
            var arr = Directory.GetFiles(_config.RootPath);
            foreach (var a in arr)
            {
                _logger.LogInformation("{}", a);
            }
            _logger.LogInformation("{}", Directory.GetFiles(_config.RootPath));
            return new List<MediaFile>();
        }

        public bool DeleteMediaFiles(List<string> FQNs)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Collections.Generic;
using MediaOrganiser.Models;
using Microsoft.Extensions.Logging;

namespace MediaOrganiser.Repository
{
    public class MediaRepository : IMediaRepository
    {
        private readonly ILogger<MediaRepository> _logger;
        public MediaRepository(ILogger<MediaRepository> logger)
        {
            _logger = logger;
        }
        public List<MediaFile> GetAllMediaFiles()
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteMediaFiles(List<string> FQNs)
        {
            throw new System.NotImplementedException();
        }
    }
}
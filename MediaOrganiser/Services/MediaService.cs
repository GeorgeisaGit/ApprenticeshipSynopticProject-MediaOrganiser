using MediaOrganiser.Repository;
using Microsoft.Extensions.Logging;

namespace MediaOrganiser.Services
{
    public class MediaService
    {
        private IMediaRepository _repo;
        private readonly ILogger<MediaService> _logger;

        public MediaService(ILogger<MediaService> logger, IMediaRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
    }
}
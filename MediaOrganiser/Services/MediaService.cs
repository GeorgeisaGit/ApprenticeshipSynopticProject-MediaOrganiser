using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaOrganiser.Config;
using MediaOrganiser.Models;
using MediaOrganiser.Repository;
using Microsoft.AspNetCore.Mvc;
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
        
        public List<MediaFile> GetAllMediaFiles(string fileNames, string extensions, string directories,
            DateTime? minDate, DateTime? maxDate, Sort sorting)
        {
            List<MediaFile> theList = _repo.GetAllMediaFiles();
            _logger.LogInformation("{}", theList);
            return theList;
        }

        public IActionResult DeleteMediaFiles(string FQNs)
        {
            try
            {
                List<string> fqnList = new List<string>((FQNs ?? "").Split(","));
                return _repo.DeleteMediaFiles(fqnList) == false ? new NoContentResult() : new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e);
            }
        }
    }
}
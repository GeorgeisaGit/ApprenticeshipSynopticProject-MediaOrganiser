using System;
using System.Collections.Generic;
using System.IO;
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

        public IActionResult DeleteMediaFiles(List<string> fileNames)
        {
            try
            {
                return _repo.DeleteMediaFiles(fileNames) == false ? new NoContentResult() : new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e);
            }
        }

        public List<MediaDirectory> GetMediaDirectory(List<string> directories)
        {
            try
            {
                return _repo.GetMediaDirectory(directories);
            }
            catch (Exception e)
            {
                _logger.LogInformation("{}", e);
                return new List<MediaDirectory>();
            }
        }
    }
}
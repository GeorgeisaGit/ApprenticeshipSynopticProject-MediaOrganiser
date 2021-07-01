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
        
        public List<MediaFile> GetAllMediaFiles()
        {
            try
            {
                return _repo.GetAllMediaFiles();
            }
            catch (Exception e)
            {
                _logger.LogInformation("{}", e);
                return new List<MediaFile>();
            }
        }

        public IActionResult DeleteMediaFile(List<string> fileNames)
        {
            try
            {
                return _repo.DeleteMediaFile(fileNames) == false ? new NoContentResult() : new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e);
            }
        }

        public List<MediaDirectory> GetMediaDirectory()
        {
            try
            {
                return _repo.GetMediaDirectory();
            }
            catch (Exception e)
            {
                _logger.LogInformation("{}", e);
                return new List<MediaDirectory>();
            }
        }

        public bool CreateMediaDirectory(List<string> directoriesList)
        {
            try
            {
                return _repo.CreateMediaDirectory(directoriesList);
            }
            catch (Exception e)
            {
                _logger.LogInformation("{}", e);
                return false;
            }
        }

        public IActionResult DeleteMediaDirectory(string directoryNames)
        {
            throw new NotImplementedException();
        }
    }
}
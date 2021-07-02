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

        public bool DeleteMediaFile(List<string> fileNames)
        {
            try
            {
                return _repo.DeleteMediaFile(fileNames);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
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

        public bool DeleteMediaDirectory(List<string> directoryNames)
        {
            try
            {
                return _repo.DeleteMediaDirectory(directoryNames);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public bool MoveFilesToDirectory(string directoryName, List<string> filePaths)
        {
            try
            {
                MediaDirectory mediaDir = new MediaDirectory {Name = directoryName};
                foreach (string filePath in filePaths)
                {
                    if (filePath != "")
                    {
                        MediaFile file = _repo.ConvertToMediaFile(filePath);
                        mediaDir.Files.Add(file);
                    }
                }
                return _repo.MoveFilesToDirectory(mediaDir);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }
    }
}
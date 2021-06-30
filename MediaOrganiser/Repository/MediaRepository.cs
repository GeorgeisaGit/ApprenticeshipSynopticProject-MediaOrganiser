using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// This method uses the System.IO library to retrieve files from the path defined within AppSettings.Json
        /// RootDirectory block.
        /// </summary>
        /// <returns>List<MediaFile></returns>
        public List<MediaFile> GetAllMediaFiles()
        {
            List<MediaFile> mediaFileList = new List<MediaFile>();
            var returnedFiles = Directory.GetFiles(_config.RootPath);
            foreach (var filePath in returnedFiles)
            {
                if (Path.GetFileName(filePath).StartsWith("."))
                {
                    _logger.LogInformation("Hidden file found, removing from program.");
                }
                else
                {
                    mediaFileList.Add(ConvertToMediaFile(filePath));
                }
            }
            return mediaFileList;
        }

        private MediaFile ConvertToMediaFile(string file)
        {
            MediaFile result = new MediaFile(_config);
            result.Name = Path.GetFileName(file);
            result.DateCreated = File.GetCreationTime(file);
            _logger.LogInformation("{} | {} | {}", result.Name, result.DateCreated,result.Path);
            return result;
        }

        public bool DeleteMediaFiles(List<string> FQNs)
        {
            var firstFileCount = Directory.GetFiles(_config.RootPath).Length;
            foreach (string filePath in FQNs)
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Issue deleting a file. {}", e);
                }
            }
            return firstFileCount < Directory.GetFiles(_config.RootPath).Length;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaOrganiser.Config;
using MediaOrganiser.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;

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
        /// <returns>List of MediaFile containing MediaFiles, Empty list of MediaFile if nothing to retrieve.</returns>
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
        
        //Use this method to create a MediaFile object for a file, from it's path.
        private MediaFile ConvertToMediaFile(string filePath)
        {
            MediaFile result = new MediaFile(_config);
            result.Name = Path.GetFileName(filePath);
            result.DateCreated = File.GetCreationTime(filePath);
            return result;
        }
        
        /// <summary>
        /// Call this method to delete media files from the root directory and sub-directories.
        /// </summary>
        /// <param name="fileNames">A list of file names, with file extension, to delete from the root directory.</param>
        /// <returns>True if one or more files from the list are deleted. False if no files deleted.</returns>
        public bool DeleteMediaFile(List<string> fileNames)
        {
            //TODO: Ensure MediaFiles are removed from sub-directories as well as root folder.
            var firstFileCount = Directory.GetFiles(_config.RootPath).Length;
            foreach (string fileName in fileNames)
            {
                try
                {
                    File.Delete(_config.RootPath + fileName);
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Issue deleting a file. {}", e);
                }
            }
            return firstFileCount > Directory.GetFiles(_config.RootPath).Length;
        }
        
        /// <summary>
        /// Call this method to get media directories from the root directory.
        /// </summary>
        /// <param name="directories">A list of directory names to retrieve from the root directory.</param>
        /// <returns>List of Media directory. If no params, retrieve all sub-directories. Otherwise, return all directories that match the names provided.
        /// Empty list if no directories found.</returns>
        public List<MediaDirectory> GetMediaDirectory()
        {
            List<MediaDirectory> mediaDirectoryList = new List<MediaDirectory>();

            foreach (var directory in Directory.GetDirectories(_config.RootPath))
            {
                MediaDirectory resultDirectory = new MediaDirectory();
                
                resultDirectory.Name = directory.Split("/").Last();
                
                var fileArray = Directory.GetFiles($"{_config.RootPath}{resultDirectory.Name}");
                foreach (string file in fileArray)
                {
                    if (file != "")
                    {
                        resultDirectory.Files.Add(ConvertToMediaFile(file));
                    }
                }
                mediaDirectoryList.Add(resultDirectory);
            }
            return mediaDirectoryList;
        }
        
        /// <summary>
        /// Call this method to create a sub directory within the root directory.
        /// </summary>
        /// <param name="directoriesList">A List of directory names to create.</param>
        /// <returns>True if one or more directories are created and false no directories are created.</returns>
        public bool CreateMediaDirectory(List<string> directoryList)
        {
            List<MediaDirectory> directories = GetMediaDirectory();
            directoryList = RemoveExistingDirectoryNamesFromList(directoryList);
            foreach (string directoryName in directoryList)
            {
                try
                {
                    Directory.SetCurrentDirectory(_config.RootPath);
                    Directory.CreateDirectory(directoryName);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }

            int compare = GetMediaDirectory().Count;
            return directories.Count < compare;
        }
        //CreateMediaDirectory() helper method.
        private List<string> RemoveExistingDirectoryNamesFromList(List<string> directoryList)
        {
            List<MediaDirectory> directories = GetMediaDirectory();
            foreach (var directory in directories)
            {
                directoryList.RemoveAll(dir => dir == directory.Name);
                _logger.LogInformation("Removing {} from directories to create", directory.Name);
            }

            return directoryList;
        }
        /// <summary>
        /// Call this method to delete directories from the root directory and copy contents back to root directory.
        /// </summary>
        /// <param name="directoryNames">A list of directory names to delete from the root directory.</param>
        /// <returns>True if one or more directories from the list are deleted. False if no directories deleted.</returns>
        public bool DeleteMediaDirectory(List<string> directoryNames)
        {
            var firstDirCount = Directory.GetDirectories(_config.RootPath).Length;
            foreach (string directory in directoryNames)
            {
                //TODO:Extract code to private helper method.
                try
                {
                    string path = $"{_config.RootPath}{directory}";
                    foreach (var file in Directory.GetFiles(path))
                    {
                        string destinationFile = $"{_config.RootPath}{file.Split("/").Last()}";
                        File.Move(file, destinationFile);
                    }
                    Directory.Delete(path);
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Issue deleting a directory. {}", e);
                }
            }
            return firstDirCount > Directory.GetDirectories(_config.RootPath).Length;
        }
    }
}
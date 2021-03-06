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
        private readonly RootDirectoryOptions _config;
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
        public MediaFile ConvertToMediaFile(string filePath)
        {
            MediaFile result = new MediaFile(_config);
            result.Name = Path.GetFileName(filePath);
            result.DateCreated = File.GetCreationTime(filePath);
            return result;
        }
        
        /// <summary>
        /// Call this method to delete media files from the root directory and sub directories.
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
        /// Call this method to get sub directories from the root directory.
        /// </summary>
        /// <returns>Returns all sub directories within the root directory. Empty list if no directories found.</returns>
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
                _logger.LogInformation("Removing existing directory {} from directoryList", directory.Name);
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
        
        /// <summary>
        /// Call this method to move files to sub directories from the root directory. Sub-directory will be created if it does not exist.
        /// </summary>
        /// <param name="directory">A MediaDirectory object populated with required values.</param>
        /// <returns>True if media directory contains 1 or more of the files requested. False if no files are added.</returns>
        public bool MoveFilesToDirectory(MediaDirectory directory)
        {
            if (!CheckMediaDirectoryExists(directory))
            {
                try
                {
                    List<string> directoryToCreateAsList = new List<string>();
                    directoryToCreateAsList.Add(directory.Name);
                    CreateMediaDirectory(directoryToCreateAsList);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return false;
                }
            }
            foreach (MediaFile file in directory.Files)
            {
                try
                {
                    var fqn = file.GetFQN();
                    File.Move(fqn, $"{_config.RootPath}{directory.Name}/{file.Name}");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }
            return Directory.GetFiles($"{_config.RootPath}{directory.Name}/").Length >= 1;
        }
        
        //MoveFilesToDirectory helper method. If directory is found, returns true else returns false.
        private bool CheckMediaDirectoryExists(MediaDirectory directory)
        {
            List<MediaDirectory> existingDirectories = GetMediaDirectory();
            List<string> existingDirectoryNames = new List<string>();
            foreach (MediaDirectory existingDirectory in existingDirectories)
            {
                existingDirectoryNames.Add(existingDirectory.Name);
            }

            return existingDirectoryNames.Contains(directory.Name);
        }
    }
}
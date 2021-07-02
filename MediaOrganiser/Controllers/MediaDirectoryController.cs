using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaOrganiser.Config;
using MediaOrganiser.Models;
using MediaOrganiser.Repository;
using MediaOrganiser.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediaOrganiser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaDirectoryController : ControllerBase
    {

        private readonly ILogger<MediaDirectoryController> _logger;
        private MediaService _service;
        public MediaDirectoryController(ILogger<MediaDirectoryController> logger, MediaService service)
        {
            _logger = logger;
            _service = service;
        }
        
        /// <summary>
        /// Call this method to retrieve the contents of user defined directories.
        /// </summary>
        /// <param name="directoryName">A csv list of directory file names to retrieve. If null, return all directories</param>
        /// <returns>200:OK with requested directories if 1 or more directories are found, 204: No Content if no directory found.</returns>
        [HttpGet]
        public IActionResult Get([FromQuery] string directoryName)
        {
            List<string> directoryList = new List<string>((directoryName ?? "").Split(","));
            try
            {
                List<MediaDirectory> mediaDirectoryList = _service.GetMediaDirectory();
                if (mediaDirectoryList.Count < 1)
                {
                    return new NoContentResult();
                }

                mediaDirectoryList = mediaDirectoryList
                    .Where(mediaDirectory => directoryName == null || directoryList.Contains(mediaDirectory.Name))
                    .ToList();
                return mediaDirectoryList.Count < 1 ? new NoContentResult() : new OkObjectResult(mediaDirectoryList);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
        
        /// <summary>
        /// Call this method to create empty sub directories within the root directory.
        /// </summary>
        /// <param name="directoryName">A csv list of directory names to create within the root directory.</param>
        /// <returns>200: Ok result if one or more directories are added. 500: Internal Server Error if no directories added.</returns>
        [HttpPost]
        public IActionResult Post([FromQuery] string directoryName)
        {
            try
            {
                List<string> directoriesList = new List<string>((directoryName ?? "").Split(","));
                return _service.CreateMediaDirectory(directoriesList) ? new OkResult() : StatusCode(500);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
        
        /// <summary>
        /// Call this method to move media files from the root directory to one specified sub-directory.
        /// If a sub-directory does not exist, this method will create one.
        /// </summary>
        /// <param name="directoryName">The directory name to move the media files to, do not include any slashes.</param>
        /// <param name="fileNames">A csv list of file names, with extension, to move from the root directory.</param>
        /// <returns>200: OK result if one or more files are found in the directory. 500: Internal Server Error if no files are found in the directory
        /// or if the directory does not exist.</returns>
        [HttpPut]
        public IActionResult Put([FromQuery] string directoryName,[FromQuery] string fileNames)
        {
            try
            {
                List<string> fileNameList = new List<string>((fileNames ?? "").Split(","));
                return _service.MoveFilesToDirectory(directoryName, fileNameList) ? new OkResult() : StatusCode(500);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
        
        /// <summary>
        /// Call this method to delete sub directories from the root directory and copy the files back to the root directory.
        /// </summary>
        /// <param name="directoryNames">A csv list of directory names to delete from the root directory.</param>
        /// <returns>200 OK if 1 or more directories are deleted. 204 No Content if no directories deleted.</returns>
        [HttpDelete]
        public IActionResult Delete([FromQuery] string directoryNames)
        {
            try
            {
                List<string> directoryNameList = new List<string>((directoryNames ?? "").Split(","));
                return _service.DeleteMediaDirectory(directoryNameList) ? new OkResult() : StatusCode(500);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
    }
}
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
        
        [HttpDelete]
        public IActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
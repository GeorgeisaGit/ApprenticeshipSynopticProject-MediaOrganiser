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
            try
            {
                List<string> directoriesList = new List<string>((directoryName ?? "").Split(","));
                List<MediaDirectory> resultList = _service.GetMediaDirectory(directoriesList);
                return resultList.Count < 1 ? new NoContentResult() : new OkObjectResult(resultList);
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult(e);
            }
        }
        
        [HttpDelete]
        public IActionResult Delete()
        {
            
        }
    }
}
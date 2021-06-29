using System;
using System.Collections.Generic;
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
    public class MediaFileController : ControllerBase
    {

        private readonly ILogger<MediaFileController> _logger;
        private MediaService _service;
        public MediaFileController(ILogger<MediaFileController> logger, MediaService service)
        {
            _logger = logger;
            _service = service;
        }
        
        /// <summary>
        /// Call this method to retrieve media files located in the root directory, configured via appsettings.json "" block.
        /// Provide no parameters to retrieve all media files as above.
        /// </summary>
        /// <param name="fileNames">A csv string of one or more file names with their extensions.</param>
        /// <param name="extensions">A csv string of one or more file extensions to retrieve.</param>
        /// <param name="directories">A csv string of one or more directories to retrieve media files from.</param>
        /// <param name="minDate">Retrieve all media files after this date, provided in format yyyy-MM-ddThh:mm:ss.SSSZ.</param>
        /// <param name="maxDate">Retrieve all media files before this date, provided in format yyyy-MM-ddThh:mm:ss.SSSZ.</param>
        /// <param name="sort">NameAsc (0) is default, other values are NameDesc (1), DateAsc (2), DateDesc (3)</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery]string fileNames, [FromQuery] string extensions, 
            [FromQuery] string directories, [FromQuery] DateTime? minDate, [FromQuery] DateTime? maxDate, 
            [FromQuery] Sort sort)
        {
            _logger.LogInformation("{}{}{}{}{}{}", fileNames, extensions, directories, minDate, maxDate, sort);
            _logger.LogInformation("{}", extensions == null);
            List<MediaFile> mediaFilesList = _service.GetAllMediaFiles(fileNames, extensions, directories, minDate, maxDate, sort);
            if (mediaFilesList.Count < 1)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(mediaFilesList);
        }

        [HttpDelete]
        public void Delete()
        {
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaOrganiser.Config;
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
        
        [HttpGet]
        public IActionResult Get([FromQuery]string fileNames, [FromQuery] string extensions, 
            [FromQuery] string directories, [FromQuery] DateTime? minDate, [FromQuery] DateTime? maxDate, 
            [FromQuery] Sort sort)
        {
            _logger.LogInformation("{}{}{}{}{}{}", fileNames, extensions, directories, minDate, maxDate, sort);
            _logger.LogInformation("{}", extensions == null);
            _service.GetAllMediaFiles(fileNames, extensions, directories, minDate, maxDate, sort);
            return new OkResult();
        }

        [HttpDelete]
        public void Delete()
        {
            
        }
    }
}
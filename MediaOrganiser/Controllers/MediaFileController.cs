using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaOrganiser.Config;
using MediaOrganiser.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediaOrganiser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaFileController : ControllerBase
    {

        private readonly ILogger<MediaFileController> _logger;

        public MediaFileController(ILogger<MediaFileController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Get([FromQuery]string fileNames, [FromQuery] string extensions, 
            [FromQuery] string directories, [FromQuery] DateTime? minDate, [FromQuery] DateTime? maxDate, 
            [FromQuery] Sort sort)
        {
            
        }

        [HttpDelete]
        public void Delete()
        {
            
        }
    }
}
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
        /// <returns>Returns a list of files if content is found, returns 204: No Content if no content found.</returns>
        [HttpGet]
        public IActionResult Get([FromQuery]string fileNames, [FromQuery] string extensions, 
            [FromQuery] string directories, [FromQuery] DateTime? minDate, [FromQuery] DateTime? maxDate, 
            [FromQuery] Sort sort)
        {
            List<string> fileNameList = new List<string>((fileNames ?? "").Split(","));
            List<string> directoryList = new List<String>((directories ?? "").Split(","));
            List<string> extensionList = new List<String>((directories ?? "").Split(","));
            try
            {
                List<MediaFile> mediaFilesList =
                    _service.GetAllMediaFiles(fileNames, extensions, directories, minDate, maxDate, sort);

                if (mediaFilesList.Count < 1)
                {
                    return new NoContentResult();
                }
                _logger.LogInformation("We've arrived at the LINQ.");
                mediaFilesList = mediaFilesList
                        .Where(mediaFile => fileNames == null || fileNameList.Contains(mediaFile.Name))
                        .Where(mediaFile => extensions == null || fileNameList.Contains(mediaFile.Name.Split(".")[1]))
                        .Where(mediaFile => directories == null || fileNameList.Contains(mediaFile.Path))
                        .Where(mediaFile => minDate == null || mediaFile.DateCreated > minDate)
                        .Where(mediaFile => maxDate == null || mediaFile.DateCreated < maxDate)
                        .ToList();
                    mediaFilesList = SortList(mediaFilesList, sort);
                    _logger.LogInformation("{}", mediaFilesList.Count);
                    return mediaFilesList.Count < 1 ? new NoContentResult() : new OkObjectResult(mediaFilesList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogInformation("{}", e);
                return new BadRequestObjectResult(e);
            }
        }

        private List<MediaFile> SortList(List<MediaFile> theList, Sort sort)
        {
            _logger.LogInformation("SSSSSSooooorting!!!!");
            switch (sort)
            {
                case Sort.NameAsc:
                case Sort.DateAsc:
                    return theList.OrderBy(mediaFile => GetMediaFilePropertyForSort(mediaFile, sort)).ToList(); 
                default:
                    return theList.OrderByDescending(mediaFile => GetMediaFilePropertyForSort(mediaFile, sort))
                        .ToList();
            }
        }

        private string GetMediaFilePropertyForSort(MediaFile mediaFile, Sort sort)
        {
            _logger.LogInformation("Getting PPPPPrrrooooperty!!!!!!");
            switch (sort)
            {
                case Sort.NameAsc:
                case Sort.DateDesc:
                    return mediaFile.Name;
                default: return mediaFile.DateCreated == null ? "" : mediaFile.DateCreated.Value.ToString("0");
            }
        }

        [HttpDelete]
        public void Delete()
        {
            
        }
    }
}
using System.Collections.Generic;
using MediaOrganiser.Models;

namespace MediaOrganiser.Repository
{
    public interface IMediaRepository
    {
        public List<MediaFile> GetAllMediaFiles();
        public bool DeleteMediaFile(List<string> fileNames);
        List<MediaDirectory> GetMediaDirectory();
        bool CreateMediaDirectory(List<string> directoriesList);
    }
}
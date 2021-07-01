using System.Collections.Generic;
using MediaOrganiser.Models;

namespace MediaOrganiser.Repository
{
    public interface IMediaRepository
    {
        List<MediaFile> GetAllMediaFiles();
        bool DeleteMediaFile(List<string> fileNames);
        List<MediaDirectory> GetMediaDirectory();
        bool CreateMediaDirectory(List<string> directoriesList);
        bool DeleteMediaDirectory(List<string> directoryNames);
    }
}
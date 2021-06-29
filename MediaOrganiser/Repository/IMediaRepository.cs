using System.Collections.Generic;
using MediaOrganiser.Models;

namespace MediaOrganiser.Repository
{
    public interface IMediaRepository
    {
        public List<MediaFile> GetAllMediaFiles();
        public bool DeleteMediaFiles(List<string> FQNs);
    }
}
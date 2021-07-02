using System;
using MediaOrganiser.Config;
using Microsoft.Extensions.Options;

namespace MediaOrganiser.Models
{
    public class MediaFile
    {
        public MediaFile(RootDirectoryOptions config)
        {
            Path = config.RootPath;
        }
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Path { get; private set; }
        public string Comment { get; set; }

        public string GetFQN()
        {
            return String.Concat(Path, Name);
        }
    }
}
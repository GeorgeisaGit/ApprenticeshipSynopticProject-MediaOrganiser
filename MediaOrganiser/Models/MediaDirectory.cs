using System.Collections.Generic;

namespace MediaOrganiser.Models
{
    public class MediaDirectory
    {
        public string Name { get; set; }
        public List<MediaFile> Files { get; set; }
    }
}
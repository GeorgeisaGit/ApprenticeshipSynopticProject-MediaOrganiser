using System;

namespace MediaOrganiser.Models
{
    public class MediaFile
    {
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Path { get; set; }
        public string? Comment { get; set; }

        public string GetFQN()
        {
            return String.Concat(Path + "/" + Name);
        }
    }
}
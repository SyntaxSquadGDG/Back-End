using Digital_Archive.Models;

namespace Digital_Archive.Dto
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double size { get; set; }
        public DateTime LastModified { get; set; }

        public int NumberOfFolders { get; set; }
        public int NumberOfEmployees { get; set; }
        List<Folder> folders { get; set; }
    }
}

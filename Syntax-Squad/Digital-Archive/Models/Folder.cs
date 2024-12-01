namespace Digital_Archive.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double size { get; set; }

        Folder? ParentFolder { get; set; }
        Section? ParentSection { get; set; }
        List<Sfile> files;
        List<Folder> folders;
    }
}

namespace Digital_Archive.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public double size { get; set; }
        public DateTime Last_Modifeid { get; set; }
        
        public int NumberOfFolders { get; set; }
        List<Folder> folders;
        
    }
}

namespace Digital_Archive.Dto
{
    public class FolderDto

    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double size { get; set; }
        public DateTime LastModified { get; set; }
        public int NumberOfEmployees { get; set; }
        public int NumberOfFiles { get; set; }
        public int NumberOfFolders { get; set; }

    }
}

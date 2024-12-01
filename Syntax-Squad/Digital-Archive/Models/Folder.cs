using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Digital_Archive.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double size { get; set; }

        public DateTime LastModified { get; set; }
        public int? ParentFolderId { get; set; }
        public int? ParentSectionId { get; set; }
        public List<Sfile> files { get; set; }
        public List<Folder> folders { get; set; }
    }
}

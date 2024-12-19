using System.Diagnostics.Contracts;

namespace Digital_Archive.Models
{
    public class Sfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string type { get; set; }
        public double size { get; set; }
        public DateTime UploadedAt { get; set; }

        public string Blob_Token { get; set; }

        public string Ocr {  get; set; }

        public int? ParentFolderId{ get; set; }

    }
}

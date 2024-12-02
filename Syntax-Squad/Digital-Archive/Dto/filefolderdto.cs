using Digital_Archive.Models;
using System.Web;

namespace Digital_Archive.Dto
{
    public class filefolderdto
    {
        public List<IFormFile> files {  get; set; }
        public int FolderId { get; set; }
    }
}

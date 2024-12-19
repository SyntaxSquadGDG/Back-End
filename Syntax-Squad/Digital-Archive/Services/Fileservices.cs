using Digital_Archive.Dto;
using Digital_Archive.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital_Archive.Services
{
    public class Fileservices
    {
        private readonly AppDbContext _context;
        public Fileservices(AppDbContext context, IConfiguration config, PermissionService permissionService)
        {
            _context = context;
        }
        public async Task<List<PathDto>> BuildPathsync(int id)
        {
            Sfile file = await _context.Sfiles.FirstAsync(x => x.Id == id);
            Folder folder = await _context.Folders.FirstAsync(x => x.Id == file.ParentFolderId);

            List<PathDto> ans = new List<PathDto>();
            ans.Add(new PathDto
            {
                type = "file",
                Name = file.Name,
                Id = file.Id
            });
            while (folder.ParentFolderId != null)
            {
                ans.Add(new PathDto
                {
                    type = "folder",
                    Name = folder.Name,
                    Id = folder.Id
                });
                 folder = await _context.Folders.FirstAsync(x => x.Id == folder.ParentFolderId);
            }
            ans.Add(new PathDto
            {
                type = "folder",
                Name = folder.Name,
                Id = folder.Id
            });
            var section = await _context.Sections.FindAsync(folder.ParentSectionId);
            ans.Add(new PathDto
            {
                type = "section",
                Name = section.Name,
                Id = section.Id
            });
            return ans;
        }

    }
}

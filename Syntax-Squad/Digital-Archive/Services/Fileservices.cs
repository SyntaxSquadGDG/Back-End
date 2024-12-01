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



        //public async Task<List<PathDto>> BuildPathsync(int id)
        //{
        //    Sfile file = await _context.Sfiles.FirstAsync(x => x.Id == id);
        //    Folder folder = await _context.Folders.FirstAsync(x => x.Id == file.ParentFolderId);
        //    folder = (await _FolderService.ByIdasync(folder.ParentFolderId ?? 1)).First();

        //    List<PathDto> ans = new List<PathDto>();
        //    ans.Add(new PathDto
        //    {
        //        type = "file",
        //        Name = file.Name,
        //        Id = file.Id
        //    });
        //    while (folder.ParentFolderId != null)
        //    {
        //        ans.Add(new PathDto
        //        {
        //            type = "folder",
        //            Name = folder.Name,
        //            Id = folder.Id
        //        });
        //        folder = (await _FolderService.ByIdasync (folder.ParentFolderId??1)).First();
        //    }
        //    ans.Add(new PathDto
        //    {
        //        type = "folder",
        //        Name = folder.Name,
        //        Id = folder.Id
        //    });
        //    ans.Add(new PathDto
        //    {
        //        type = "section",
        //        Name = (await _FolderService.ByIdasync(folder.ParentSectionId ?? 1)).First().Name,
        //        Id = (await _FolderService.ByIdasync(folder.ParentSectionId ?? 1)).First().Id
        //    });
        //    return ans;
        //}
    }
}

using Digital_Archive.Models;
using Digital_Archive.Dto;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
namespace Digital_Archive.Services
{
    public class FolderService
    {
        private readonly AppDbContext _context;
        private readonly PermissionService _PermissionService;
        

        public FolderService(AppDbContext context, IConfiguration config, PermissionService permissionService)
        {
            _context = context;
            _PermissionService = permissionService;
        }
        public async Task<List<FolderDto>> getallasync()
        {
            var folders = await _context.Folders.ToListAsync();
            List<FolderDto> ans = new List<FolderDto>();
            foreach (var fold in folders)
            {
                ans.Add(new FolderDto
                {
                    Id = fold.Id,
                    Name = fold.Name,
                    size = fold.size,
                    LastModified = fold.LastModified,
                    NumberOfEmloyees = _PermissionService.EmployPerFolder(fold.Id),
                    NumberOfFiles = fold.files?.Count??0,
                    NumberOfFolders = fold.folders?.Count??0,

                });
            }
            return ans;
        }
        public async Task<IEnumerable<Folder>> ByIdasync(int id)
        {
            var folder =  _context.Folders.Where(x=> x.Id == id);
            return folder;  
        }
        public async Task<List<FolderDto>> ByParentIdasync(int id)
        {
            Folder folder = await _context.Folders.FirstAsync(x => x.Id == id);
            List<FolderDto> ans = new List<FolderDto>();
            if(folder.folders != null&&folder.folders.Count()>0)
            foreach (var fold in folder.folders) {
                ans.Add(new FolderDto
                {
                    Id = fold.Id,
                    Name = fold.Name,
                    size = fold.size,
                    LastModified = fold.LastModified,
                    NumberOfEmloyees= _PermissionService.EmployPerFolder(fold.Id),
                    NumberOfFiles = fold.files?.Count ?? 0,
                    NumberOfFolders = fold.folders?.Count ?? 0,

                }) ;
            }
            return ans;
        }
        public async Task<List<Sfile>> GetAllFiles(int id)
        {
            Folder folder = await _context.Folders.FirstOrDefaultAsync(x => x.Id == id);
            return folder.files;
        }
        public async Task<List<PathDto>> BuildPathsync(int id)
        {
            Folder folder = await _context.Folders.FirstAsync(x => x.Id == id);
            List < PathDto > ans= new List<PathDto>();
            while(folder.ParentFolderId != null)
            {
                ans.Add(new PathDto
                {
                    type ="folder",
                    Name = folder.Name,
                    Id = folder.Id
                });
                folder = (await ByIdasync(folder.ParentFolderId ?? 1)).First();
            }
            ans.Add(new PathDto
            {
                type = "folder",
                Name = folder.Name,
                Id = folder.Id
            });
            ans.Add(new PathDto
            {
                type = "section",
                Name = (await ByIdasync(folder.ParentSectionId ?? 1)).First().Name,
                Id = (await ByIdasync(folder.ParentSectionId ?? 1)).First().Id
            });
            return ans;
        }
    }
}

using Digital_Archive.Models;
using Digital_Archive.Dto;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
                    NumberOfEmployees = _PermissionService.EmployPerFolder(fold.Id),
                    NumberOfFiles = _context.Sfiles.Where(x => x.ParentFolderId == fold.Id).Count(),
                    NumberOfFolders = _context.Folders.Where(x => x.ParentFolderId == fold.Id).Count(),

                });
            }
            return ans;
        }
        public async Task<Folder> ByIdasync(int id)
        {
            var folder = await _context.Folders.FindAsync(id);
            if (folder == null)
            {
                return new Folder();
            }
            return folder;  
        }
        public async void delete(Folder folder)
        {

            var inside = _context.Folders.Where(x => x.ParentFolderId == folder.Id).ToList();

            if (inside!=null)
                foreach(var item in inside)
                {
                    delete(item);
                }
            
            var files = _context.Sfiles.Where(x => x.Id == folder.Id).ToList();
            
            if(files!=null)
                foreach(var file in files)
                {
                    _context.Sfiles.Remove(file);
                }

            _context.Folders.Remove(folder);

        }
        public async Task<List<FolderDto>> ByParentIdasync(int id)
        {
            Folder folder = await _context.Folders.FirstAsync(x => x.Id == id);
            List<FolderDto> ans = new List<FolderDto>();
            if (folder == null) return ans;

            var folders = _context.Folders.Where(x => x.ParentFolderId == id).ToList();

            if (folders != null&& folders.Count()>0)
            foreach (var fold in folders) {
                ans.Add(new FolderDto
                {
                    Id = fold.Id,
                    Name = fold.Name,
                    size = fold.size,
                    LastModified = fold.LastModified,
                    NumberOfEmployees= _PermissionService.EmployPerFolder(fold.Id),
                    NumberOfFiles = _context.Sfiles.Where(x=> x.ParentFolderId== fold.Id).Count(),
                    NumberOfFolders = _context.Folders.Where(x => x.ParentFolderId == fold.Id).Count(),

                }) ;
            }
            return ans;
        }
        public async Task<List<Sfile>> GetAllFiles(int id)
        {
            var files = _context.Sfiles.Where(x => x.ParentFolderId == id).ToList();
            if(files == null)return new List<Sfile>();
            return files;
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
                folder = (await ByIdasync(folder.ParentFolderId ?? 1));
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

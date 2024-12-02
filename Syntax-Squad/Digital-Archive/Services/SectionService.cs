using Digital_Archive.Dto;
using Digital_Archive.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital_Archive.Services
{
    public class SectionService
    {

        private readonly AppDbContext _context;
        private readonly PermissionService _PermissionService;
        public SectionService(AppDbContext context, IConfiguration config, PermissionService permissionService)
        {
            _context = context;
            _PermissionService = permissionService;
        }
        public async Task<string> NameById(int id)
        {
            Section  sec=await _context.Sections.FirstAsync(s => s.Id == id);
            return sec.Name;
        }
        public async Task<List<SectionDto>> getallassync()
        {
            var sections = _context.Sections.ToList();
            List<SectionDto> result = new List<SectionDto>();
            if (sections == null || sections.Count == 0) return result;
            foreach (var section in sections)
            {
                result.Add(new SectionDto
                {
                    Id = section.Id,
                    Name = section.Name,
                    size = section.size,
                    LastModified = section.LastModified,
                    NumberOfEmployees = _PermissionService.EmployPerSection(section.Id),
                    NumberOfFolders = _context.Folders.Count(x=> x.ParentSectionId==section.Id)
                }) ;
            }
            return result;
        }
        public async Task<List<FolderDto>> ByParentIdasync(int id)
        {
            var folders = _context.Folders.Where(x=> x.ParentSectionId == id);
            if(folders==null)return new List<FolderDto>() ;
            List<FolderDto> ans = new List<FolderDto>() ;
            if (folders != null && folders.Count() > 0)
                foreach (var fold in folders)
                {
                    ans.Add(new FolderDto
                    {
                        Id = fold.Id,
                        Name = fold.Name,
                        size = fold.size,
                        LastModified = fold.LastModified,
                        NumberOfEmployees = _PermissionService.EmployPerFolder(fold.Id),
                        NumberOfFiles = fold.files?.Count ?? 0,
                        NumberOfFolders = fold.folders?.Count ?? 0,

                    });
                }
            return ans;
        }
        public async Task<List<PathDto>> BuildPathsync(int id)
        {
            Section section = await _context.Sections.FirstAsync(x => x.Id == id);

            List<PathDto> ans = new List<PathDto>();
           
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

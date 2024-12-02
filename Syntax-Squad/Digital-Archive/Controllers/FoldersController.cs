using Digital_Archive.Dto;
using Digital_Archive.Models;
using Digital_Archive.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly FolderService _FolderService;
        private readonly PermissionService _PermissionService;
        public FoldersController(AppDbContext context, IConfiguration config, FolderService folderService, PermissionService permissionService)
        {
            _config = config;
            _context = context;
            _FolderService = folderService;
            _PermissionService = permissionService;
        }
        [HttpGet("SectionNameForFolder")]
        public async Task<IActionResult> SectionNameForFolder(int id)
        {
            var folder = await _FolderService.ByIdasync(id);
            if (folder == null||folder.ParentSectionId==null)return NotFound();
            var section = _context.Sections.First(x => x.Id == folder.ParentSectionId);
            if (section == null) return NotFound();
            return Ok(new { name = section.Name });
        }
        [HttpGet("allfoldrs")]
        public async Task<IActionResult> AllFoldrs()
        {
            var folders = await _FolderService.getallasync();
            return Ok(folders);
        }
        [HttpGet("FolderById")]
        public async Task<IActionResult> FolderById(int id)
        {
            var folder = await _FolderService.ByIdasync(id);
            return Ok(folder);
        }
        [HttpGet("FoldersByParentId")]
        public async Task<IActionResult> FoldersByParentId(int id)
        {
            var folders = await _FolderService.ByParentIdasync(id);
            var files = await _FolderService.GetAllFiles(id);
            return Ok(new{Folders=folders,Files = files  });
        }
        [HttpGet("path")]
        public async Task<List<PathDto>> Path(int id)
        {
            var path = await _FolderService.BuildPathsync(id);
            return path;
        }
        [HttpPost("newfolder")]
        public async Task<IActionResult> NewFolder(string name,int? FolderParentId , int? SectionParentId)
        {
            if ((FolderParentId != null && SectionParentId != null) || (FolderParentId == null && SectionParentId == null)) return BadRequest();
            try
            {
                Folder folder = new Folder
                {
                    Name = name,
                    LastModified = DateTime.Now,
                    ParentFolderId = FolderParentId,
                    ParentSectionId = SectionParentId,
                };
                _context.Folders.Add(folder);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}

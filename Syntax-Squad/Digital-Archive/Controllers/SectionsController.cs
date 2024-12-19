using Digital_Archive.Dto;
using Digital_Archive.Models;
using Digital_Archive.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly SectionService _SectionService;
        private readonly PermissionService _PermissionService;
        private readonly FolderService _FolderService;
        public SectionsController(AppDbContext context, IConfiguration config, SectionService SectionService, PermissionService permissionService, FolderService folderService)
        {
            _config = config;
            _context = context;
            _SectionService = SectionService;
            _PermissionService = permissionService;
            _FolderService = folderService;
        }
        [HttpPost("newsection")]
        public  async Task<IActionResult> newsection(string name)
        {
            try
            {
                Section section = new Section
                {
                    Name = name,
                    LastModified = DateTime.Now,
                };
                _context.Sections.Add(section);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpDelete("deletesection")]
        public async Task<IActionResult> deletefolderbyid(int id)
        {
            try
            {
                var folders = _context.Folders.Where(x => x.ParentSectionId == id);
                if (folders != null) 
                foreach (var folder in folders)
                {
                    _FolderService.delete(folder);
                }
                _context.Sections.Remove(_context.Sections.Find(id));
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        [HttpGet("getallsections")]
        public async Task<IActionResult> GetAllSections()
        {
            var sections = await _SectionService.getallassync();
            return Ok(sections);
        }
        [HttpGet("FoldersByParentId")]
        public async Task<List<FolderDto>> FoldersByParentId(int id)
        {
            var folders = await _SectionService.ByParentIdasync(id);
            return folders;
        }
        [HttpGet("SectioNameById")]
        public async Task<IActionResult> SectioNameById(int id)
        {
            var name = await _SectionService.NameById(id);
            if (name == null) return BadRequest();
            return Ok(new { name = name });
        }
        [HttpGet("path")]
        public async Task<List<PathDto>> Path(int id)
        {
            var path = await _SectionService.BuildPathsync(id);
            return path;
        }
    }
}

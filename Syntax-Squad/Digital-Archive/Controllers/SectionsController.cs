using Digital_Archive.Dto;
using Digital_Archive.Models;
using Digital_Archive.Services;
using Microsoft.AspNetCore.Http;
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
        public SectionsController(AppDbContext context, IConfiguration config, SectionService SectionService, PermissionService permissionService)
        {
            _config = config;
            _context = context;
            _SectionService = SectionService;
            _PermissionService = permissionService;
        }

        [HttpGet("getallsections")]
        public async Task<IActionResult> GetAllSections()
        {
            var sections = await _SectionService.getallassync();
            return Ok(sections);
        }

        [HttpGet("path")]
        public async Task<List<PathDto>> Path(int id)
        {
            var path = await _SectionService.BuildPathsync(id);
            return path;
        }
    }
}

using Digital_Archive.Dto;
using Digital_Archive.Models;
using Digital_Archive.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly Fileservices _Fileservices;
        private readonly PermissionService _PermissionService;
        public FilesController(AppDbContext context, IConfiguration config, Fileservices fileservices, PermissionService permissionService)
        {                                            
            _config = config;
            _context = context;
            _Fileservices = fileservices;
            _PermissionService = permissionService;
        }

        //[HttpGet("path")]
        //public async Task<List<PathDto>> Path(int id)
        //{
        //    var path = await _Fileservices.BuildPathsync(id);
        //    return path;
        //}
    }
}

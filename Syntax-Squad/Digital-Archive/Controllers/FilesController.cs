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
        [HttpPost("filebyfid")]
        public async Task<IActionResult> filebyfid(int id ,[FromForm] List<FormFile> model)
        {
            try
            {
                foreach (var file in model)
                {

                    Sfile result = new Sfile
                    {
                        Name = Path.GetFileName(file.FileName),
                        UploadedAt = DateTime.Now,
                        type = Path.GetExtension(file.FileName),
                        size = file.Length / (1024.0 * 1024.0),
                        ParentFolderId = id,
                    };
                    _context.Sfiles.Add(result);
                    _context.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new {aa= "from sohil" });
            }
        }
        [HttpPost("test")]
        public async Task<IActionResult> test([FromForm] FormFile model)
        {
           return Ok(model);
        }
        [HttpPost("filebypath")]
        public async Task<IActionResult> filebypath(int id ,[FromForm] FormFile model)
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}

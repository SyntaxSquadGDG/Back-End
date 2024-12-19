using Digital_Archive.Dto;
using Digital_Archive.Models;
using Digital_Archive.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SfilesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly Fileservices _Fileservices;
        private readonly PermissionService _PermissionService;
        private readonly FolderService _folderService;
        public readonly AiService _AiService;
        public SfilesController(AppDbContext context, IConfiguration config, Fileservices fileservices, PermissionService permissionService, AiService aiService, FolderService folderService)
        {                                            
            _config = config;
            _context = context;
            _Fileservices = fileservices;
            _PermissionService = permissionService;
            _AiService = aiService;
            _folderService = folderService;
        }
        [HttpGet("filebyid")]
        public async Task<IActionResult> filebyid( int fileid)
        {

            try
            {

                var file = _context.Sfiles.Find(fileid);
                if (file == null)
                {
                    return NotFound();
                }
                return Ok(file);
            }
            catch
            {
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }
        [HttpGet("filebyidpreview")]
        public async Task<IActionResult> fileByIdPreview(int fileid)
        {

            try
            {

                var file = _context.Sfiles.Find(fileid);
                if (file == null)
                {
                    return NotFound();
                }


                if (System.IO.File.Exists(file.Blob_Token))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(file.Blob_Token);
                    return Ok(new { f = fileBytes, ocr = file.Ocr });
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }
        [HttpDelete("deleteById")]
        public async Task<IActionResult> DeleteById(int fileid)
        {

            try
            {

                var file = _context.Sfiles.Find(fileid);
                if (file == null)
                {
                    return NotFound();
                }
                _context.Sfiles.Remove(file);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] List<IFormFile> files, int folderid)
        {

            try
            {
                var folder = _context.Folders.Find(folderid);
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                foreach(var iFORMfile in files) {
                    var filePath = Path.Combine(uploadsFolderPath, iFORMfile.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await iFORMfile.CopyToAsync(stream);
                    }



                    var file = new Sfile {
                        Name = iFORMfile.FileName,
                        type = Path.GetExtension(filePath),
                        size = iFORMfile.Length,
                        UploadedAt = DateTime.Now,
                        Blob_Token = filePath,
                        ParentFolderId = folderid,
                    };

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    await _AiService.GetOcr(file, iFORMfile);

                    _context.Sfiles.Add(file);
                }
                _context.SaveChanges();


                //return Ok(new { Message = "File uploaded successfully", FilePath = filePath });

                return Ok(new {Message = "File uploaded successfully", FilePath = uploadsFolderPath });

            }
            catch
            {
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }
        [HttpPost("uploadbyai")]
        public async Task<IActionResult> UploadByAi([FromForm] List<IFormFile> files)
        {

            try
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                var filePath = Path.Combine(uploadsFolderPath, files[0].FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await files[0].CopyToAsync(stream);
                }



                List<string> responce =  await _AiService.ClassifyImage(filePath, files[0]);


                var id = responce[0] == "/SyntaxSquad/Advertisement" ? 4 :
                         responce[0] == "/SyntaxSquad/Email" ? 5 :
                         responce[0] == "/SyntaxSquad/Form" ? 6 :
                         responce[0] == "/SyntaxSquad/Letter" ? 7 :
                         responce[0] == "/SyntaxSquad/Memo" ? 8 :
                         responce[0] == "/SyntaxSquad/News" ? 9 :
                         responce[0] == "/SyntaxSquad/Note" ? 10 :
                         responce[0] == "/SyntaxSquad/Report" ? 11 :
                         responce[0] == "/SyntaxSquad/Resume" ? 12 :
                         responce[0] == "/SyntaxSquad/Scientific" ? 13 :
                         4;
                List<PathDto> result = await _folderService.BuildPathsync(id) ;
                return Ok(new {type =Path.GetExtension(filePath), accurate= int.Parse(responce[1])>30? "Accurate": "Not Accurate", accuracy = int.Parse(responce[1]), filename = files[0].FileName, folderid= id,path= result });

            }
            catch
            {
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }
        [HttpGet("Path/{fileid}")]
        public async Task<IActionResult> GetFilePath(int fileid)
        {
            var path = await _Fileservices.BuildPathsync(fileid);
            return Ok(path);
        }

    }
}

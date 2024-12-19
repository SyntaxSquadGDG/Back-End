using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Digital_Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile iFORMfile)
        {
           
            try
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var filePath = Path.Combine(uploadsFolderPath, iFORMfile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await iFORMfile.CopyToAsync(stream);
                }

                return Ok(new { Message = "File uploaded successfully", FilePath = filePath });
            }
            catch
            {
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }
    }
}
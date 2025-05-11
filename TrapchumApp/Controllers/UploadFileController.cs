using Microsoft.AspNetCore.Mvc;

namespace TrapchumApp.Controllers
{

    [ApiController]
    [Route("App/[controller]")]
    public class UploadFileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public UploadFileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            return Ok("Hello");
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0 && file.Length <= 10 * 1024 * 1024) // 10MB limit
                    {
                        var safeFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadFolder, safeFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    else if (file.Length > 10 * 1024 * 1024)
                    {
                        return BadRequest("File size exceeds 10MB.");
                    }
                }
                return Ok(new { Message = "Files uploaded successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading files: {ex.Message}");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;

namespace TrapchumApp.Controllers
{

    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("App/[controller]")]
    public sealed class UploadFileController : ControllerBase
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
        [Microsoft.AspNetCore.Mvc.Route("Upload")]
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



        public async Task<IActionResult> DownloadExcel()
        {
            var values = new[] {
                    new { Column1 = "MiniExcel", Column2 = 1 },
                    new { Column1 = "Github", Column2 = 2}
             };
            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(values);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "demo.xlsx"
            };
        }

        public async Task<IActionResult> DownloadExcelFromTemplatePath()
        {
            string templatePath = "TestTemplateComplex.xlsx";

            Dictionary<string, object> value = new Dictionary<string, object>()
            {
                ["title"] = "FooCompany",
                ["managers"] = new[] {
                new {name="Jack",department="HR"},
                new {name="Loan",department="IT"}
            },
                ["employees"] = new[] {
                new {name="Wade",department="HR"},
                new {name="Felix",department="HR"},
                new {name="Eric",department="IT"},
                new {name="Keaton",department="IT"}
            }
            };

            MemoryStream memoryStream = new MemoryStream();

            await memoryStream.SaveAsByTemplateAsync(templatePath, value);

            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "demo.xlsx"
            };
        }


        private static Dictionary<string, Byte[]> TemplateBytesCache = new Dictionary<string, byte[]>();

        static UploadFileController() 
        {
            string templatePath = "TestTemplateComplex.xlsx";
            byte[] bytes = System.IO.File.ReadAllBytes(templatePath);
            TemplateBytesCache.Add(templatePath, bytes);
        }

        public IActionResult DownloadExcelFromTemplateBytes()
        {
            byte[] bytes = TemplateBytesCache["TestTemplateComplex.xlsx"];

            Dictionary<string, object> value = new Dictionary<string, object>()
            {
                ["title"] = "FooCompany",
                ["managers"] = new[] {
                new {name="Jack",department="HR"},
                new {name="Loan",department="IT"}
            },
                ["employees"] = new[] {
                new {name="Wade",department="HR"},
                new {name="Felix",department="HR"},
                new {name="Eric",department="IT"},
                new {name="Keaton",department="IT"}
            }
            };

            MemoryStream memoryStream = new MemoryStream();
            memoryStream.SaveAsByTemplate(bytes, value);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "demo.xlsx"
            };
        }


        public IActionResult UploadExcel(IFormFile excel)
        {
            var stream = new MemoryStream();
            excel.CopyTo(stream);

            foreach (var item in stream.Query(true))
            {
                // do your logic etc.
            }

            return Ok("File uploaded successfully");
        }

    }
}

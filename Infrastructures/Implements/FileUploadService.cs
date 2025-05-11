using Applications.Interfaces.Files;
using Applications.Responses;
using Domains.Models.Files;
using Infrastructures.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private string fileUploadPath = "";
        private readonly ILogger<FileUploadService> _logger;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public FileUploadService(IWebHostEnvironment env, ApplicationDbContext context, ILogger<FileUploadService> logger, IDbContextFactory<DataContext> dbContextFactory)
        {
            _env = env;
            _context = context;
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Result<string>> RemovedFile(string? filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
                return await Result.FailureAsync<string>(ErrorResponse.NullValue);

            try
            {

                var extension = Path.GetExtension(filePath);

                if (AllowFileImage.Contains(extension.ToLower()))
                {
                    fileUploadPath = Path.Combine("document", "images");
                }

                if (AllowFileFiles.Contains(extension.ToLower()))
                {
                    fileUploadPath = Path.Combine("document", "files"); ;
                }

                var fileDeletePath = Path.Combine(_env.WebRootPath, fileUploadPath, filePath);

                if (File.Exists(fileDeletePath))
                {
                    File.Delete(fileDeletePath);

                    return await Result.SuccessAsync(filePath);

                }
                return await Result.FailureAsync<string>(ErrorResponse.NotFound("File not found"));
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<string>(ErrorResponse.ServerError(ex.Message));
            }
        }

        public async Task<Result<string>> RemovedFile(long fileId, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return await Result.FailureAsync<string>(ErrorResponse.NullValue);

            try
            {

                var extension = Path.GetExtension(filePath);

                if (AllowFileImage.Contains(extension.ToLower()))
                {
                    fileUploadPath = Path.Combine("document", "images");
                }

                if (AllowFileFiles.Contains(extension.ToLower()))
                {
                    fileUploadPath = Path.Combine("document", "files"); ;
                }

                var fileDeletePath = Path.Combine(_env.WebRootPath, fileUploadPath, filePath);

                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();


                var existingFiles = await dbContext.Set<FileModel>().FindAsync(fileId);

                if (File.Exists(fileDeletePath) && existingFiles is not null)
                {

                    var file = await dbContext.Set<FileModel>()
                                                .Where(x => x.Id == fileId)
                                                .ExecuteDeleteAsync();
                    if (file == 0)
                    {
                        return await Result.FailureAsync<string>(ErrorResponse.NotFound("File not found"));
                    }

                    File.Delete(fileDeletePath);

                    return await Result.SuccessAsync(filePath);

                }
                return await Result.FailureAsync<string>(ErrorResponse.NotFound("File not found"));
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<string>(ErrorResponse.ServerError(ex.Message));
            }
        }


        public async Task<Result<FileModel>> UploadFile(MultipartFormDataContent content)
        {
            try
            {
                var file = content.FirstOrDefault();

                if (file == null)
                    return await Result.FailureAsync<FileModel>(ErrorResponse.NullValue);

                var fileContent = await file.ReadAsStreamAsync();

                var fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.Headers.ContentDisposition!.Name).FileName?.Trim('"');

                return await SaveFile(fileName ?? "", fileContent);
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<FileModel>(ErrorResponse.ServerError(ex.Message));
            }
        }

        public async Task<Result<FileModel>> UploadFile(FileUploadModel model)
        {
            try
            {
                if (model.FileContent == null || model.FileContent.Length == 0)

                    return await Result.FailureAsync<FileModel>(ErrorResponse.NullValue);

                using var stream = new MemoryStream(model.FileContent);

                return await SaveFile(model.FileName, stream);
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<FileModel>(ErrorResponse.ServerError(ex.Message));
            }
        }

        public async Task<Result<IEnumerable<FileModel>>> UploadFile(IEnumerable<FileUploadModel> model)
        {
            try
            {

                ICollection<FileModel> fileModels = [];

                if (!model.Any()) return Result.Failure<IEnumerable<FileModel>>(ErrorResponse.NullValue);

                foreach (var item in model)
                {
                    if (item.FileContent == null || item.FileContent.Length == 0)
                    {
                        continue;
                    }

                    using var stream = new MemoryStream(item.FileContent);

                    var uploadFile = await UploadImageFile(item.FileName, stream);

                    fileModels.Add(uploadFile.Data!);
                }

                return Result.Success<IEnumerable<FileModel>>(fileModels);

            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<IEnumerable<FileModel>>(ErrorResponse.ServerError(ex.Message));
            }
        }

        public async Task<Result<IEnumerable<FileModel>>> UploadFiles(IEnumerable<FileModel> model)
        {
            try
            {

                ICollection<FileModel> fileModels = [];

                if (!model.Any()) return Result.Failure<IEnumerable<FileModel>>(ErrorResponse.NullValue);

                foreach (var item in model)
                {
                    if (item.FileContent == null || item.FileContent.Length == 0)
                    {
                        continue;
                    }

                    using var stream = new MemoryStream(item.FileContent);

                    var uploadFile = await UploadImageFile(item.FileName, stream);

                    fileModels.Add(uploadFile.Data!);
                }

                return Result.Success<IEnumerable<FileModel>>(fileModels);

            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<IEnumerable<FileModel>>(ErrorResponse.ServerError(ex.Message));
            }
        }

        public async Task<Result<FileModel>> UploadFile(FileModel model)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(model.FileName))
                {
                    return Result.Failure<FileModel>(ErrorResponse.NullValue);
                }

                if (model.FileContent == null || model.FileContent.Length == 0)
                {
                    return Result.Failure<FileModel>(ErrorResponse.NullValue);
                }

                using var stream = new MemoryStream(model.FileContent);

                return await UploadImageFile(model.FileName, stream);

            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<FileModel>(ErrorResponse.ServerError(ex.Message));
            }
        }

        public async Task<Result<FileModel>> UploadFile(StreamContent content)
        {
            try
            {
                var fileName = content.Headers?.ContentDisposition?.FileName?.Trim('"') ??
                    $"file_{Guid.NewGuid()}";
                var stream = await content.ReadAsStreamAsync();

                return await SaveFile(fileName, stream);
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<FileModel>(ErrorResponse.ServerError(ex.Message));
            }
        }

        public async Task<Result<IEnumerable<FileModel>>> UploadFiles(MultipartFormDataContent content)
        {
            try
            {
                var results = new List<FileModel>();

                foreach (var file in content)
                {
                    var fileContent = await file.ReadAsStreamAsync();

                    var fileName = file.Headers.ContentDisposition!.FileName!.Trim('"'); //Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.Headers.ContentDisposition.).FileName.Trim();

                    var result = await SaveFile(fileName!, fileContent);

                    if (result.IsSuccess && result.Data != null)
                    {
                        results.Add(result.Data);
                    }
                }
                return await Result.SuccessAsync<IEnumerable<FileModel>>(results);
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<IEnumerable<FileModel>>(ErrorResponse.ServerError(ex.Message));
            }
        }




        private async Task<Result<FileModel>> SaveFile(string originalFileName, Stream content)
        {
            try
            {
                var uploadDestination = Path.Combine(_env.WebRootPath, fileUploadPath);

                if (!Path.Exists(uploadDestination))
                {
                    Directory.CreateDirectory(uploadDestination);
                }

                var extension = Path.GetExtension(originalFileName);

                var fileName = $"{Guid.NewGuid()}{extension}";

                var fileSize = content.Length;

                var filePath = Path.Combine(uploadDestination, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);

                await content.CopyToAsync(fileStream);

                var fileUrl = $"/{fileUploadPath}/{fileName}";

                var fileModel = new FileModel(originalFileName, fileName, fileSize, fileUrl);

                return await Result.SuccessAsync(fileModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return await Result.FailureAsync<FileModel>(ErrorResponse.ServerError(ex.Message));
            }
        }

        private async Task<Result<FileModel>> UploadImageFile(string originalFileName, Stream content)
        {
            try
            {
                var uploadDestination = "";

                var extension = Path.GetExtension(originalFileName);

                if (AllowFileImage.Contains(extension.ToLower()))
                {
                    fileUploadPath = Path.Combine("document", "images");

                    uploadDestination = Path.Combine(_env.WebRootPath, fileUploadPath);
                }

                if (AllowFileFiles.Contains(extension.ToLower()))
                {
                    fileUploadPath = Path.Combine("document", "files");

                    uploadDestination = Path.Combine(_env.WebRootPath, fileUploadPath);
                }

                if (!Path.Exists(uploadDestination))
                {
                    Directory.CreateDirectory(uploadDestination);
                }

                var fileName = $"{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(uploadDestination, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);

                await content.CopyToAsync(fileStream);

                var fileSize = content.Length;

                var fileUrl = $"/{fileUploadPath}/{fileName}";

                var fileModel = new FileModel(originalFileName, fileName, fileSize, fileUrl);

                return await Result.SuccessAsync(fileModel);
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<FileModel>(ErrorResponse.ServerError(ex.Message));
            }
        }

        private IList<string> AllowFileImage => ["jpeg", ".jpg", ".png"];
        private IList<string> AllowFileFiles => [".xlsx", ".pdf", ".docx"];
    }
}

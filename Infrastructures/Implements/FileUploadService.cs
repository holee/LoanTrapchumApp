using Applications.Interfaces.Files;
using Applications.Responses;
using Domains.Models.Files;
using Infrastructures.DataAccess;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructures.Implements
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private const string fileUploadPath = "documents/files";
        private const string imageUploadPath = "documents/images";
        public FileUploadService(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }

        public async Task<Result<string>> RemovedFile(string? removedFile = null)
        {
            if (string.IsNullOrEmpty(removedFile))
                return await Result.FailureAsync<string>(ErrorResponse.NullValue);

            try
            {
                var filePath = Path.Combine(fileUploadPath, removedFile);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return await Result.SuccessAsync(removedFile);
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

                    var fileName = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.Headers.ContentDisposition!.FileName).FileName.Trim();

                    var result = await SaveFile(fileName.Value!, fileContent);
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

                var filePath = Path.Combine(uploadDestination, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await content.CopyToAsync(fileStream);
                }

                var fileUrl = $"/{fileUploadPath}/{fileName}";

                var fileModel = new FileModel(originalFileName, fileName, fileUrl);

                return await Result.SuccessAsync(fileModel);
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<FileModel>(ErrorResponse.ServerError(ex.Message));
            }
        }

        private async Task<Result<FileModel>> UploadImageFile(string originalFileName, Stream content)
        {
            try
            {

                var uploadDestination = Path.Combine(_env.WebRootPath, imageUploadPath);

                if (!Path.Exists(uploadDestination))
                {
                    Directory.CreateDirectory(uploadDestination);
                }


                var extension = Path.GetExtension(originalFileName);

                var fileName = $"{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(uploadDestination, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await content.CopyToAsync(fileStream);
                }

                var fileUrl = $"/{imageUploadPath}/{fileName}";

                var fileModel = new FileModel(originalFileName, fileName, fileUrl);

                return await Result.SuccessAsync(fileModel);
            }
            catch (Exception ex)
            {
                return await Result.FailureAsync<FileModel>(ErrorResponse.ServerError(ex.Message));
            }
        }

    }
}

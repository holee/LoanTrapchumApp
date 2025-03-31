using Applications.Interfaces.Files;
using Applications.Responses;
using Domains.Models.Files;

namespace Applications.Services.Files
{

    public sealed class FileUploadService(IFileUploadService fileUpload) : IFileUploadServiceImp
    {
        private readonly string[] permittedExtensions = { ".pdf", ".docx", ".jpg", ".png" };

        private bool IsValidFileExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            return !string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext);
        }


        public Task<Result<string>> RemovedUploadFile(string? removedFile = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<FileModel>> UploadFile(MultipartFormDataContent content)
        {
            return await fileUpload.UploadFile(content);
        }

        public async Task<Result<FileModel>> UploadFile(FileUploadModel model)
        {
            return await fileUpload.UploadFile(model);
        }

        public Task<Result<IEnumerable<FileModel>>> UploadFiles(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }
    }

}

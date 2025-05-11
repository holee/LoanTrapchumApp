using Applications.Interfaces.Files;
using Applications.Responses;
using Domains.Models.Files;

namespace Applications.Services.Files
{

    public sealed class FileUploadService(IFileUploadService fileUpload) : IFileUploadServiceImp
    {
        private readonly string[] permittedExtensions = { ".pdf", ".docx", ".jpg", ".png" };



        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes /= 1024;
            }
            return $"{bytes:0.##} {sizes[order]}";
        }
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream",
            };
        }


        private bool IsValidFileExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            return !string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext);
        }


        public async Task<Result<string>> RemovedUploadFile(string? removedFile = null)
        {
            return await fileUpload.RemovedFile(removedFile);
        }

        public async Task<Result<string>> RemovedUploadFile(long fileId, string removedFile)
        {
            return await fileUpload.RemovedFile(fileId, removedFile);
        }

        public async Task<Result<FileModel>> UploadFile(MultipartFormDataContent content)
        {
            return await fileUpload.UploadFile(content);
        }

        public async Task<Result<FileModel>> UploadFile(FileUploadModel model)
        {
            return await fileUpload.UploadFile(model);
        }

        public async Task<Result<IEnumerable<FileModel>>> UploadFiles(MultipartFormDataContent content)
        {
            return await fileUpload.UploadFiles(content);
        }

        public async Task<Result<IEnumerable<FileModel>>> UploadFiles(IEnumerable<FileUploadModel> content)
        {
            return await fileUpload.UploadFile(content);
        }
        public async Task<Result<IEnumerable<FileModel>>> UploadFiles(IEnumerable<FileModel> content)
        {
            return await fileUpload.UploadFiles(content);
        }

        public async Task<Result<FileModel>> UploadFile(FileModel content)
        {
            return await fileUpload.UploadFile(content);
        }

    }

}

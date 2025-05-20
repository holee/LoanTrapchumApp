using Applications.Responses;
using Domains.Models.Files;

namespace Applications.Interfaces.Files
{
    public interface IFileUploadService
    {
        Task<Result<string>> RemovedFile(string? removedFile = null);
        Task<Result<string>> RemovedFile(long fileId, string filePath);
        //Task<Result<FileModel>> UploadFile(MultipartFormDataContent content);
        Task<Result<FileModel>> UploadFile(FileUploadModel model);
        Task<Result<FileModel>> UploadFile(StreamContent content);
        Task<Result<IEnumerable<FileModel>>> UploadFile(IEnumerable<FileUploadModel> model);
        Task<Result<FileModel>> UploadFile(FileModel model);
        Task<Result<IEnumerable<FileModel>>> UploadFiles(MultipartFormDataContent content);
        Task<Result<IEnumerable<FileModel>>> UploadFiles(IEnumerable<FileModel> model);
    }
}

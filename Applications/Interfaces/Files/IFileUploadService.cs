using Applications.Responses;
using Domains.Models.Files;

namespace Applications.Interfaces.Files
{
    public interface IFileUploadService
    {
        Task<Result<string>> RemovedFile(string? removedFile = null); 
        Task<Result<FileModel>> UploadFile(MultipartFormDataContent content);
        Task<Result<FileModel>> UploadFile(FileUploadModel model);
        Task<Result<FileModel>> UploadFile(StreamContent content); 
        Task<Result<IEnumerable<FileModel>>> UploadFiles(MultipartFormDataContent content);
    }
}

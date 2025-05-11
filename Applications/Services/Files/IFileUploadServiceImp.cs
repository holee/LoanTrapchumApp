using Applications.Responses;
using Domains.Models.Files;

namespace Applications.Services.Files
{
    public interface IFileUploadServiceImp
    {
        Task<Result<string>> RemovedUploadFile(string? removedFile = null);
        Task<Result<string>> RemovedUploadFile(long fileId, string removedFile);
        Task<Result<FileModel>> UploadFile(MultipartFormDataContent content);
        Task<Result<FileModel>> UploadFile(FileUploadModel model);
        Task<Result<FileModel>> UploadFile(FileModel content);
        Task<Result<IEnumerable<FileModel>>> UploadFiles(MultipartFormDataContent content);
        Task<Result<IEnumerable<FileModel>>> UploadFiles(IEnumerable<FileUploadModel> content);
        Task<Result<IEnumerable<FileModel>>> UploadFiles(IEnumerable<FileModel> content);
    }

}

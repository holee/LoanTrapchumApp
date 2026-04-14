namespace Domains.Models.Files
{
    public sealed class FileModel
    {
        public FileModel()
        {
            
        }
        public FileModel(string originalName, string? fileName, string? fileUrl)
        {
            OriginalName = originalName;
            FileName = fileName;
            FileUrl = fileUrl;
        }

        public FileModel(string originalName, string? fileName, long fileSize, string? fileUrl)
            : this(originalName, fileName, fileUrl)
        {
            FileSize = fileSize;
        }


        public FileModel(string originalName, string fileName, byte[]? fileContent)
        {
            OriginalName = originalName;
            FileContent = fileContent;
            FileName = fileName;
        }

        public FileModel(string originalName, string fileName,long fileSize, byte[]? fileContent)
            :this(originalName, fileName,fileContent)
        {
            FileSize = fileSize;
        }

        public long Id { get; set; } 
        public string? FileName { get; set; }
        public long FileSize { get; set; } = 0;
        public string? OriginalName { get; set; }
        public string? FileUrl { get; set; }
        public byte[]? FileContent { get; set; } = null;
    }
}

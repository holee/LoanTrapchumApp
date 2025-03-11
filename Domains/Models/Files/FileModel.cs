namespace Domains.Models.Files
{
    public sealed class FileModel
    {
        public FileModel(string originalName, string? fileName, string? fileUrl)
        {
            OriginalName = originalName;
            FileName = fileName;
            FileUrl = fileUrl;
        }

        public string? OriginalName { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
    }
}

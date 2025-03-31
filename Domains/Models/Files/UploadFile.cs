namespace Domains.Models.Files
{
    public sealed class UploadedFile
    {
        public int Id { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public DateTime? UploadDate { get; set; }
    }
}

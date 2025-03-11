namespace Domains.Models.Files
{
    public sealed class FileUploadModel(string fileName = "", string? extension = "", long fileSize = 0, byte[]? fileContent = null)
    {
        public FileUploadModel()
            : this("", "", 0, null)
        {
        }

        public string FileName { get; set; } = fileName;
        public string? Extension { get; set; } = extension;
        public long FileSize { get; set; } = fileSize;
        public byte[] FileContent { get; set; } = fileContent ?? Array.Empty<byte>();
    }


    public static class FileUploadModelExts
    {
        public static FileUploadModel ToFileUpload(this FileInfo file)
        {
            using var ms = new MemoryStream();
            file.Create().CopyTo(ms);
            var fileName = file.Name;
            var extension = file.Extension;
            var fileSize = file.Length;
            var fileContent = ms.ToArray();
            return new FileUploadModel(fileName, extension, fileSize, fileContent);
        }

    }


}

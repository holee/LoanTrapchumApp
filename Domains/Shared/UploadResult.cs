using Domains.Models.Files;

namespace Domains.Shared;

public record struct UploadResult(bool Success, string? Message, FileModel? Model);

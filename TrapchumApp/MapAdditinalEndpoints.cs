using Applications.Services.Files;
using Domains.Models.Files;

namespace TrapchumApp;

internal static class MapAdditinalEndpoints
{
    public static IEndpointConventionBuilder MapUploadFileEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var uploadGroup = endpoints.MapGroup("/Upload");

        uploadGroup.MapPost("/File", async (IFormFile file, IFileUploadServiceImp fileUploadService, IWebHostEnvironment env) =>
        {
            if (file is null || file.Length == 0)
                return Results.BadRequest("No file uploaded.");

            const long maxFileSize = 10L * 1024 * 1024; // 10 MB

            if (file.Length > maxFileSize)
                return Results.BadRequest("File size exceeds 10 MB.");

            try
            {

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                var permittedExtensions = new[] { ".pdf" };

                if (!permittedExtensions.Contains(extension))
                    return Results.BadRequest("Invalid file type. Only PDF files are allowed.");


                var newFileName = $"{Guid.NewGuid()}{extension}";

                var destinationPath = Path.Combine(env.WebRootPath, "assets/docs");

                using var fileStream = new FileStream(Path.Combine(destinationPath, newFileName), FileMode.Create);

                await file.CopyToAsync(fileStream);

                var fileUrl = $"/assets/docs/{newFileName}";

                var fileModel = new FileModel(file.FileName, newFileName, file.Length, fileUrl);

                return Results.Ok(fileModel);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: $"Error uploading file: {ex.Message}",
                    statusCode: 500);
            }
        }).DisableAntiforgery();


        uploadGroup.MapPost("/Files", async (IFormFileCollection files,IWebHostEnvironment env) =>
        {

            List<FileModel> uploadedFiles = new List<FileModel>();

            if (files is null || files.Count == 0)
                return Results.BadRequest("No file uploaded.");

            const long maxFileSize = 10L * 1024 * 1024; // 10 MB

            try
            {

                var permittedExtensions = new[] { ".pdf" };

                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                    if (files.Any(f => !permittedExtensions.Contains(extension))) continue;

                    if (files.Any(f => f.Length > maxFileSize)) continue;

                    var newFileName = $"{Guid.NewGuid()}{extension}";

                    var destinationPath = Path.Combine(env.WebRootPath, "assets/docs");

                    using var fileStream = new FileStream(Path.Combine(destinationPath, newFileName), FileMode.Create);

                    await file.CopyToAsync(fileStream);

                    var fileUrl = $"/assets/docs/{newFileName}";

                    var fileModel = new FileModel(file.FileName, newFileName, file.Length, fileUrl);

                    uploadedFiles.Add(fileModel);
                }

                return Results.Ok(uploadedFiles);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: $"Error uploading file: {ex.Message}",
                    statusCode: 500);
            }
        }).DisableAntiforgery();


        return uploadGroup;
    }
}

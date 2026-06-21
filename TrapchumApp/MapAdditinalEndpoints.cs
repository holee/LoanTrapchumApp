using Applications.Features.Documents.Command.AddDocument;
using Applications.Features.Documents.Command.AddDocuments;
using Applications.Features.Documents.Dto;
using Applications.Interfaces.Identity;
using Applications.Utils;
using Domains.Models.Files;

namespace TrapchumApp;

internal static class MapAdditinalEndpoints
{
    public static IEndpointConventionBuilder MapUploadFileEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var uploadGroup = endpoints.MapGroup("/Upload");

        uploadGroup.MapPost("/File", async (IFormFile file,
            IMediator mediator,
            IWebHostEnvironment env) =>
        {
            if (file is null || file.Length == 0)
                return Results.BadRequest("No file uploaded.");

            const long maxFileSize = 50L * 1024 * 1024; // 50 MB

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


                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                using var fileStream = new FileStream(Path.Combine(destinationPath, newFileName), FileMode.Create);

                await file.CopyToAsync(fileStream);

                var fileUrl = $"/assets/docs/{newFileName}";

                var fileModel = new DocumentDtoCreate
                {
                    OriginalName = file.FileName,
                    NewFileName = newFileName,
                    FileSize = file.Length,
                    FileUrl = fileUrl
                };

                var command = new AddDocumentCommand(fileModel);

                var result = await mediator.Send(command);

                return Results.Ok(result.Value);

            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: $"Error uploading file: {ex.Message}",
                    statusCode: 500);
            }
        }).DisableAntiforgery();


        uploadGroup.MapPost("/Files", async (IFormFileCollection files,
            IWebHostEnvironment env,
            IMediator mediator,
            ILogger<Program> logger) =>
        {

            logger.LogInformation("Received {FileCount} files for upload.", files?.Count ?? 0);

            List<DocumentDtoCreate> uploadedFiles = new List<DocumentDtoCreate>();

            if (files is null || files.Count == 0)
                return Results.BadRequest("No file uploaded.");

            const long maxFileSize = 50L * 1024 * 1024; // 10 MB

            try
            {

                var permittedExtensions = new[] { ".pdf" };

                var destinationPath = Path.Combine(env.WebRootPath, "assets/docs");

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                    if (files.Any(f => !permittedExtensions.Contains(extension))) continue;

                    if (files.Any(f => f.Length > maxFileSize)) continue;

                    var newFileName = $"{Guid.NewGuid()}{extension}";

                    using var fileStream = new FileStream(Path.Combine(destinationPath, newFileName), FileMode.Create);

                    await file.CopyToAsync(fileStream);

                    var fileUrl = $"/assets/docs/{newFileName}";

                    var fileModel = new DocumentDtoCreate
                    {
                        OriginalName = file.FileName,
                        NewFileName = newFileName,
                        FileSize = file.Length,
                        FileUrl = fileUrl
                    };

                    uploadedFiles.Add(fileModel);
                }

                var command = new AddDocumentsCommand(uploadedFiles);

                var result = await mediator.Send(command);

                logger.LogInformation("Successfully uploaded {UploadedCount} files.", uploadedFiles.Count);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error uploading files: {ErrorMessage}", ex.Message);

                return Results.Problem(detail: $"Error uploading file: {ex.Message}", statusCode: 500);
            }
        }).DisableAntiforgery();


        uploadGroup.MapPost("/Customers/Profiles", async (IFormFile file,
            IMediator mediator,
            IWebHostEnvironment env,
            ILogger<Program> logger) =>
        {
            if (file is null || file.Length == 0)
            {
                logger.LogWarning("No file uploaded.");
                return Results.BadRequest("No file uploaded.");
            }
            const long maxFileSize = 50L * 1024 * 1024; // 50 MB

            if (file.Length > maxFileSize)
                return Results.BadRequest("File size exceeds 10 MB.");

            logger.LogInformation("Received file {FileName} for upload.", file.FileName);

            try
            {

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                var permittedExtensions = new[] { ".jpg", ".png" };

                if (!permittedExtensions.Contains(extension))
                    return Results.BadRequest("Invalid file type. Only JPG and PNG files are allowed.");

                var newFileName = $"{Guid.NewGuid()}{extension}";

                var destinationPath = Path.Combine(env.WebRootPath, "assets/profiles");

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                using var fileStream = new FileStream(Path.Combine(destinationPath, newFileName), FileMode.Create);

                await file.CopyToAsync(fileStream);

                var fileUrl = $"/assets/profiles/{newFileName}";

                var fileModel = new DocumentDtoCreate
                {
                    OriginalName = file.FileName,
                    NewFileName = newFileName,
                    FileSize = file.Length,
                    FileUrl = fileUrl
                };

                var command = new AddDocumentCommand(fileModel);

                var result = await mediator.Send(command);

                return Results.Ok(result.Value);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading file: {ErrorMessage}", ex.Message);

                return Results.Problem(detail: $"Error uploading file: {ex.Message}", statusCode: 500);
            }
        }).DisableAntiforgery();


        uploadGroup.MapPost("/Users/Profiles/{userId}", async (
            string userId,
            IFormFile file,
            IUserService userService,
            IWebHostEnvironment env,
            ILogger<Program> logger) =>
        {
            if (file is null || file.Length == 0)
            {
                logger.LogWarning("No file uploaded for user {UserId}.", userId);
                return Results.BadRequest("No file uploaded.");
            }

            const long maxFileSize = 10L * 1024 * 1024; // 10 MB

            if (file.Length > maxFileSize)
                return Results.BadRequest("File size exceeds 10 MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!permittedExtensions.Contains(extension))
                return Results.BadRequest("Invalid file type. Only JPG, PNG and WEBP are allowed.");

            try
            {
                var newFileName = $"{Guid.NewGuid()}{extension}";
                var destinationPath = Path.Combine(env.WebRootPath, "assets/profiles");

                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);

                var fullPath = Path.Combine(destinationPath, newFileName);
                using var fileStream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                var fileUrl = $"/assets/profiles/{newFileName}";

                using var stream = new MemoryStream();
                file.OpenReadStream().CopyTo(stream);

                var fileModel = new FileModel(file.FileName, newFileName, file.Length, stream.ToArray())
                {
                    FileUrl = fileUrl
                };

                var result = await userService.ChangeProfile(userId, fileModel);

                if (result.IsError)
                    return Results.Problem(detail: result.FirstError.Description, statusCode: 400);

                return Results.Ok(new { fileUrl });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading profile photo for user {UserId}.", userId);
                return Results.Problem(detail: $"Error uploading file: {ex.Message}", statusCode: 500);
            }
        }).DisableAntiforgery();


        uploadGroup.MapPost("/branches/images", async (IFormFile file, IWebHostEnvironment env, IMediator mediator, ILogger<Program> logger) =>
        {
            if (file is null || file.Length == 0)
            {
                logger.LogWarning("No file uploaded for branch image.");

                return Results.BadRequest("No file uploaded.");
            }

            const long maxFileSize = 10L * 1024 * 1024; // 10 MB

            if (file.Length > maxFileSize)

                return Results.BadRequest("File size exceeds 10 MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!permittedExtensions.Contains(extension))
                return Results.BadRequest("Invalid file type. Only JPG, PNG and WEBP are allowed.");

            try
            {
                var newFileName = $"{Guid.NewGuid()}{extension}";

                var destinationPath = Path.Combine(env.WebRootPath, "assets/branches");

                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);

                var fullPath = Path.Combine(destinationPath, newFileName);

                using var fileStream = new FileStream(fullPath, FileMode.Create);

                await file.CopyToAsync(fileStream);

                var fileUrl = $"assets/branches/{newFileName}";

                using var stream = new MemoryStream();

                file.OpenReadStream().CopyTo(stream);

                var fileModel = new DocumentDtoCreate
                {
                    OriginalName = file.FileName,
                    NewFileName = newFileName,
                    FileSize = file.Length,
                    FileUrl = fileUrl
                };

                var command = new AddDocumentCommand(fileModel);

                var result = await mediator.Send(command);

                return Results.Ok(result.Value);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading branch image: {ErrorMessage}", ex.Message);

                return Results.Problem(detail: $"Error uploading file: {ex.Message}", statusCode: 500);
            }
        }).DisableAntiforgery();





        return uploadGroup;
    }
}

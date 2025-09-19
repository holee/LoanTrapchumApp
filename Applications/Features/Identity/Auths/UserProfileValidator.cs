using Applications.Features.Identity.Users;
using Domains.Models.Files;
using Domains.Models.Identity;
using FluentValidation;

namespace Applications.Requests.Validations.Auths;

public class UserProfileValidator : AbstractValidator<UserProfiles>
{
    public UserProfileValidator()
    {
        RuleFor(x=>x.FileName)
                        .Cascade(CascadeMode.Stop)
                        .NotEmpty().WithMessage("No File Selected")
                        .NotNull().WithMessage("No File Selected");
        RuleFor(x => x.UserRefId)
                        .Cascade(CascadeMode.Stop)
                        .NotEmpty().WithMessage("Please select User")
                        .NotNull().WithMessage("User must not null.");
    }
}



public sealed class ProfileValidator : AbstractValidator<ProfileDtoCreate> 
{

    private readonly long _maxAllowedSize = 5 * 1024 * 1024; // 5 MB
   
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];


    public ProfileValidator()
    {
        RuleFor(x => x.FileName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("No file selected.")
            .Must(HaveValidExtension).WithMessage("File extension is not allowed.");

        RuleFor(x => x.FileSize)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("File is empty.")
            .LessThanOrEqualTo(_maxAllowedSize).WithMessage($"File size must be less than {_maxAllowedSize / (1024 * 1024)} MB.");
    }


    private bool HaveValidExtension(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return false;

        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return _allowedExtensions.Contains(extension);
    }
}
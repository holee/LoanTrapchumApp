namespace Application.Features.Branches.Command.CreateBranch;

public sealed class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchCommandValidator()
    {
        RuleFor(x => x.Branch).SetValidator(new CreateBranchValidator());    
    }
}

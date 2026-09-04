namespace Application.Features.Branches.Command.EditBranch; 

public sealed class EditBranchCommandValidator : AbstractValidator<EditBranchCommand>
{
    public EditBranchCommandValidator() 
    {
        RuleFor(x => x.Branch).SetValidator(new EditBranchValidator());    
    }
}

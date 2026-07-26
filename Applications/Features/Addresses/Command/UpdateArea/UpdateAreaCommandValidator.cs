namespace Applications.Features.Addresses.Command.UpdateArea;

public sealed class UpdateAreaCommandValidator : AbstractValidator<UpdateAreaCommand>
{
    public UpdateAreaCommandValidator()
    {
        RuleFor(x => x.Area).NotNull();
        RuleFor(x => x.Area.Id).GreaterThan(0)
                .WithMessage("លេខសម្គាល់ត្រូវតែធំជាង 0។");
        RuleFor(x => x.Area.VillageId)
                .GreaterThan(0).WithMessage("លេខសម្គាល់ភូមិត្រូវតែធំជាង 0។");
        RuleFor(x => x.Area.CommuneId)
                .GreaterThan(0).WithMessage("លេខសម្គាល់ឃុំ/សង្កាត់ត្រូវតែធំជាង 0។");
        RuleFor(x => x.Area.DistrictId)
                .GreaterThan(0).WithMessage("លេខសម្គាល់ស្រុក/ខណ្ឌត្រូវតែធំជាង 0។");
        RuleFor(x => x.Area.ProvinceId)
                .GreaterThan(0).WithMessage("លេខសម្គាល់ខេត្ត/រាជធានីត្រូវតែធំជាង 0។");
        RuleFor(x => x.Area.BranchId)
                .GreaterThan(0).WithMessage("លេខសម្គាល់សាខាត្រូវតែធំជាង 0។");
    }
}

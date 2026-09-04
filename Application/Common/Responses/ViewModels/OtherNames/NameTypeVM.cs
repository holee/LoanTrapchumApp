namespace Application.Responses.ViewModels.OtherNames
{
    public class NameTypeVm
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = default!;
        public string? Description { get; set; }
    }
}

namespace Applications.Features.Currencies.Dto
{
    public class NoteActivateDto(bool isActive)
    {
        public bool IsActive { get; set; } = isActive;
    }
}

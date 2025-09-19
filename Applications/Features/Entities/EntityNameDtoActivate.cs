namespace Applications.Features.Entities;

public class EntityNameDtoActivate 
{
    public long EntId { get; set; }
    public bool IsActive { get; set; } = true;
    public int EditSeq { get; set; } = 0;
    
}

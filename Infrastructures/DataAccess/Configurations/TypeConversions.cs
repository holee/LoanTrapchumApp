using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructures.DataAccess.Configurations;

public static class TypeConversions
{
    public static ValueConverter GuidConversion =>
        new ValueConverter<Guid, string>(x => x.ToString(), v => Guid.Parse(v));
    
    
    
    
}
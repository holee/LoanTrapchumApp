namespace Applications.Requests.Collaterals;

public sealed class CollateralLineDtoCreate 
{
    public long CollateralRefId { get; set; }
    public string? TypeName { get; set; }
    public string? SubTypeName { get; set; }
    public string? ReferenceNo { get; set; }
    public string? CollateralNo { get; set; }
    public string? OwnerName { get; set; }
    public override string ToString()
    {
        return $"{CollateralRefId}-{ReferenceNo}-{OwnerName}";
    }
    public override bool Equals(object? obj)
    {
        return obj is CollateralLineDtoCreate create &&
               CollateralRefId == create.CollateralRefId &&
               TypeName == create.TypeName &&
               SubTypeName == create.SubTypeName &&
               ReferenceNo == create.ReferenceNo &&
               CollateralNo == create.CollateralNo &&
               OwnerName == create.OwnerName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CollateralRefId, TypeName, SubTypeName, ReferenceNo, CollateralNo, OwnerName);
    }
}

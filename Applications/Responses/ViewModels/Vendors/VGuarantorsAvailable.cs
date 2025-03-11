namespace Applications.Responses.ViewModels.Vendors;

public class VGuarantorsAvailable
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Sex { get; set; }
    public string? Phone { get; set; }
    public string? ProInKhmer { get; set; }
    public string? DisInKhmer { get; set; }
    public string? ComInKhmer { get; set; }
    public string? VilInKhmer { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is VGuarantorsAvailable available &&
               Id == available.Id &&
               Name == available.Name &&
               Sex == available.Sex &&
               Phone == available.Phone &&
               ProInKhmer == available.ProInKhmer &&
               DisInKhmer == available.DisInKhmer &&
               ComInKhmer == available.ComInKhmer &&
               VilInKhmer == available.VilInKhmer;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Sex, Phone, ProInKhmer, DisInKhmer, ComInKhmer, VilInKhmer);
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}

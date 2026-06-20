namespace TrapchumApp.Services;

public interface IPdfService
{
    Task<byte[]> GeneratePdfAsync(string html);
}





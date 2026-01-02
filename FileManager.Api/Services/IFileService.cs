namespace FileManager.Api.Services;

public interface IFileService
{
    Task<Guid> Upload(IFormFile file, CancellationToken cancellation = default);
}

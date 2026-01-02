namespace FileManager.Api.Services;

public interface IFileService
{
    Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellation = default);
    Task<IEnumerable<Guid>> UploadManyAsync(IFormFileCollection files, CancellationToken cancellationToken = default);
}

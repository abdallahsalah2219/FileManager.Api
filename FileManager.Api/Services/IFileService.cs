using FileManager.Api.Contracts;

namespace FileManager.Api.Services;

public interface IFileService
{
    Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellation = default);
    Task<IEnumerable<Guid>> UploadManyAsync(IFormFileCollection files, CancellationToken cancellationToken = default);
    Task UploadImageAsync( IFormFile image, CancellationToken cancellationToken = default);
}

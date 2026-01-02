
namespace FileManager.Api.Services;

public class FileService : IFileService
{
    public Task<Guid> Upload(IFormFile file, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}

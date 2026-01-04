namespace FileManager.Api.Services;


public class FileService(IWebHostEnvironment webHostEnvironment , ApplicationDbContext context) : IFileService
{
    private readonly string _filesPath = $"{webHostEnvironment.WebRootPath}/uploads";
    private readonly ApplicationDbContext _context = context;

    public async Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationtoken = default)
    {
    
        var uploadedFile = await SaveFile(file, cancellationtoken);

        await _context.Files.AddAsync(uploadedFile, cancellationtoken);
        await _context.SaveChangesAsync(cancellationtoken);

        return uploadedFile.Id;
    }

    public async Task<IEnumerable<Guid>> UploadManyAsync(IFormFileCollection files, CancellationToken cancellationToken = default)
    {
        List<UploadedFile> uploadedFiles = [];
        foreach (var file in files)
        {
            var uploadedFile = await SaveFile(file, cancellationToken);
            uploadedFiles.Add(uploadedFile);
        }

        await _context.Files.AddRangeAsync(uploadedFiles, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return uploadedFiles.Select(x=>x.Id).ToList();
    }

    private async Task<UploadedFile> SaveFile(IFormFile file, CancellationToken cancellationToken = default) 
    {
       

        var randomFileName = Path.GetRandomFileName();

        var uploadedFile = new UploadedFile
        {
            FileName = file.FileName,
            StoredFileName = randomFileName,
            ContentType = file.ContentType,
            FileExtension = Path.GetExtension(file.FileName)
        };

        var path = Path.Combine(_filesPath, randomFileName);

        using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);

        return uploadedFile;
    }
}

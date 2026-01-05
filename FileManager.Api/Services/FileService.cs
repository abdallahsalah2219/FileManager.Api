using FileManager.Api.Contracts;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace FileManager.Api.Services;


public class FileService(IWebHostEnvironment webHostEnvironment , ApplicationDbContext context) : IFileService
{
    private readonly string _filesPath = $"{webHostEnvironment.WebRootPath}/uploads";
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";
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

    public async Task UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_imagesPath,image.FileName);

        using var stream = File.Create(path);
        await image.CopyToAsync(stream, cancellationToken);
    }

    public async Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Find the file metadata in the database
        var file = await  _context.Files.FindAsync(id , cancellationToken);
         if (file is null)
              return ([],string.Empty,string.Empty);
        // Build the full file path
        var path = Path.Combine(_filesPath, file.StoredFileName);

        // Read the file content
        MemoryStream memoryStream = new();
        // Copy the file content to the memory stream
        using FileStream fileStream = new(path, FileMode.Open);
        fileStream.CopyTo(memoryStream);
        
        memoryStream.Position = 0;
        return (memoryStream.ToArray(), file.ContentType, file.FileName);
    }

    public async Task<(FileStream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _context.Files.FindAsync(id, cancellationToken);
        if (file is null)
            return (null, string.Empty, string.Empty);

        var path = Path.Combine(_filesPath, file.StoredFileName);

        var  fileStream = File.OpenRead(path);

        return (fileStream, file.ContentType, file.FileName);
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

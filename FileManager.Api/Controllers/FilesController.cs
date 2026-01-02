using FileManager.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController(IFileService fileService) : ControllerBase
{
    private readonly IFileService _fileService = fileService;

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadFileRequest request,  CancellationToken cancellationToken)
    {
        var fileId = await _fileService.UploadAsync(request.File, cancellationToken);
        return Created();
    }
    [HttpPost("upload-many")]
    public async Task<IActionResult> UploadMany([FromForm] UploadManyFilesRequest request,  CancellationToken cancellationToken)
    {
        var filesIds = await _fileService.UploadManyAsync(request.Files, cancellationToken);
        return Ok(filesIds);
    }
}

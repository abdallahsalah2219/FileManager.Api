namespace FileManager.Api.Contracts;

public record UploadImageRequest(
    IFormFile Image
);
namespace FileManager.Api.Contracts;

public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
{
    public UploadImageRequestValidator()
    {
        RuleFor(x => x.Image)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator());


        RuleFor(x=>x.Image)
            .Must((request, context) =>
            {
                var extension = Path.GetExtension(request.Image.FileName);
                return FileSettings.AllowedImagesExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
            })
            .WithMessage($"not allowed file extension")
            .When(x => x.Image is not null); 
    }
}

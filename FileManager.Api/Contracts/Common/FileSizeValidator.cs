namespace FileManager.Api.Contracts.Common;

public class FileSizeValidator : AbstractValidator<IFormFile>
{
    public FileSizeValidator()
    {
        RuleFor(x => x)
            .Must((request, context) => request.Length <= FileSettings.MaxFileSizeInBytes)
            .WithMessage($"File size must not exceed {FileSettings.MaxFileSixeInMB} MB.")
            .When(x => x is not null);
    }
}

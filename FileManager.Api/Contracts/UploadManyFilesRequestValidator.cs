namespace FileManager.Api.Contracts;

public class UploadManyFilesRequestValidator : AbstractValidator<UploadManyFilesRequest>
{
    public UploadManyFilesRequestValidator()
    {
        RuleForEach(x=>x.Files)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator())
            .SetValidator(new FileNameValidator());

      
    }
}

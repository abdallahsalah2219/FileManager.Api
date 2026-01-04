

namespace FileManager.Api.Contracts;


public class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    public UploadFileRequestValidator()
    {

        RuleFor(x => x.File)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator())
            .SetValidator(new FileNameValidator());

        
    }
}

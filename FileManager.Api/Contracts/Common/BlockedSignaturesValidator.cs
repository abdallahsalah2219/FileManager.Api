namespace FileManager.Api.Contracts.Common;

public class BlockedSignaturesValidator : AbstractValidator<IFormFile>
{
    public BlockedSignaturesValidator()
    {
        RuleFor(x => x)
            .Must((request, context) =>
            {
                // Read the first two bytes to get the file signature
                BinaryReader binary = new(request.OpenReadStream());

                var bytes = binary.ReadBytes(2);
                var fileSequenceHex = BitConverter.ToString(bytes);

                foreach (var signature in FileSettings.BlockedSignatures)
                {
                    if (signature.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                return true;
            })
            .WithMessage($"not allowed file content")
            .When(x => x is not null);
    }
}

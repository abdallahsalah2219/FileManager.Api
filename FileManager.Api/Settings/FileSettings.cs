namespace FileManager.Api.Settings;

public static class FileSettings
{
    public const int MaxFileSixeInMB = 1;
    public const int MaxFileSizeInBytes = MaxFileSixeInMB * 1024 * 1024;

    // File signatures to block (e.g., exe(4D-5A), js(2F-2A), msi(D0-CF) files)
    public static readonly string[] BlockedSignatures = ["4D-5A", "2F-2A", "D0-CF"];

    public static readonly string[] AllowedImagesExtensions = [".jpg", ".jpeg", ".png"];
}

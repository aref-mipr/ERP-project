namespace ERP.Domain.Interface.Utility
{
    public interface IFileManager
    {
        string GetUnicFileName(string fileName);
        FileStream GetFileStream(string fileName, string savePath);
        string GetProfilePictureDirectory();
    }
}

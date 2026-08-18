using ERP.Domain.Interface.Utility;

namespace ERP.Infrastructure.Utility
{
    public class FileManager : IFileManager
    {
        public string GetUnicFileName(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException();

            return $"{Guid.NewGuid()}{fileName}";
        }
        public FileStream GetFileStream(string unicFileName, string savePath)
        {
            if (unicFileName == null || savePath == null)
                throw new ArgumentNullException();

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), savePath.Replace("/", "\\"));
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fullPath = Path.Combine(folderPath, unicFileName);
            var stream = new FileStream(fullPath, FileMode.Create);
            return stream;
        }

        public string GetProfilePictureDirectory()
        {
            return "wwwroot/images/profile";
        }
    }
}

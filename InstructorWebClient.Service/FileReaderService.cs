using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Infrastructure.Utils;


namespace InstructorWebClient.Service
{
    public class FileReaderService : IFileReaderService
    {
        public string ReadFileContent(string filePath)
        {
            string htmlTemplate = string.Empty;
            if (filePath.IsNullOrEmpty())
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            if (File.Exists(filePath))
                htmlTemplate = File.ReadAllText(filePath);

            return htmlTemplate;
        }
    }
}

using Hospital.Utilities.Interfaces;

namespace Hospital.Utilities
{
    public class FileService : IFileService
    {
        private const string DirectoryName = "Hospital management files";
        public static readonly string DirectoryPath = GetDirectoryPath();
        private const string LogFileName = "log.txt";
        public static readonly string LogFilePath = GetFilePath();

        public FileService() { }

        public void CreateLogFile()
        {
            try
            {
                if (!DoesLogFileExist(LogFilePath))
                {
                    File.Create(LogFilePath).Close();
                }
            }
            catch
            {
                throw;
            }
        }

        public void CreateDirectory()
        {
            try
            {
                if (!DoesDirectoryExist(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
            }
            catch
            {
                throw;
            }
        }

        private static string GetDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DirectoryName);
        }

        private static string GetFilePath()
        {
            return Path.Combine(DirectoryPath, LogFileName);
        }

        private bool DoesLogFileExist(string path)
        {
            return File.Exists(path);
        }

        private bool DoesDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }
    }
}
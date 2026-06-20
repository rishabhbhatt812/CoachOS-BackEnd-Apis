using CoachOS.Application.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;

        public FileStorageService()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName)
        {
            var folderPath = Path.Combine(_basePath, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            return Path.Combine("uploads", folderName, uniqueFileName).Replace("\\", "/");
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            var cleanPath = filePath.Replace("/", "\\").TrimStart('\\');
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cleanPath);
            if (File.Exists(fullPath))
            {
                return await File.ReadAllBytesAsync(fullPath);
            }
            throw new FileNotFoundException("File not found", filePath);
        }

        public void DeleteFile(string filePath)
        {
            var cleanPath = filePath.Replace("/", "\\").TrimStart('\\');
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cleanPath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}

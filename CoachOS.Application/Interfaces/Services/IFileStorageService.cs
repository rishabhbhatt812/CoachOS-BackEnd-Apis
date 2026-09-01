using System.IO;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName);
        Task<byte[]> GetFileAsync(string filePath);
        void DeleteFile(string filePath);
        Task DeleteFileAsync(string filePath);
        string GetFileUrl(string filePath);
    }
}

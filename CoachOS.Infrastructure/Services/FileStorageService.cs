using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly Cloudinary? _cloudinary;
        private readonly CloudinarySettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FileStorageService> _logger;
        private readonly string _localBasePath;

        private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp", ".svg", ".ico", ".tiff"
        };

        public FileStorageService(
            IOptions<CloudinarySettings> options,
            IConfiguration configuration,
            HttpClient httpClient,
            ILogger<FileStorageService> logger)
        {
            _settings = options.Value ?? new CloudinarySettings();
            _httpClient = httpClient;
            _logger = logger;

            // Also support reading from IConfiguration directly if options wasn't bound
            if (string.IsNullOrWhiteSpace(_settings.CloudName))
            {
                _settings.CloudName = configuration["Cloudinary:CloudName"] ?? configuration["CLOUDINARY_CLOUD_NAME"] ?? string.Empty;
                _settings.ApiKey = configuration["Cloudinary:ApiKey"] ?? configuration["CLOUDINARY_API_KEY"] ?? string.Empty;
                _settings.ApiSecret = configuration["Cloudinary:ApiSecret"] ?? configuration["CLOUDINARY_API_SECRET"] ?? string.Empty;
            }

            // Also check CLOUDINARY_URL format (e.g. cloudinary://api_key:api_secret@cloud_name)
            var cloudinaryUrl = configuration["CLOUDINARY_URL"] ?? Environment.GetEnvironmentVariable("CLOUDINARY_URL");
            if (!string.IsNullOrWhiteSpace(cloudinaryUrl))
            {
                _cloudinary = new Cloudinary(cloudinaryUrl);
                _cloudinary.Api.Secure = true;
                _logger.LogInformation("Cloudinary initialized using CLOUDINARY_URL.");
            }
            else if (!string.IsNullOrWhiteSpace(_settings.CloudName) &&
                     !string.IsNullOrWhiteSpace(_settings.ApiKey) &&
                     !string.IsNullOrWhiteSpace(_settings.ApiSecret) &&
                     _settings.CloudName != "your-cloud-name")
            {
                var account = new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret);
                _cloudinary = new Cloudinary(account);
                _cloudinary.Api.Secure = true;
                _logger.LogInformation("Cloudinary initialized successfully for cloud: {CloudName}", _settings.CloudName);
            }
            else
            {
                _logger.LogWarning("Cloudinary credentials are not configured or are placeholder values. Falling back to local storage temporarily until Cloudinary credentials are provided.");
            }

            _localBasePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_localBasePath))
            {
                Directory.CreateDirectory(_localBasePath);
            }
        }

        private static bool IsImage(string fileNameOrPath)
        {
            var ext = Path.GetExtension(fileNameOrPath);
            return !string.IsNullOrEmpty(ext) && ImageExtensions.Contains(ext);
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName)
        {
            if (_cloudinary != null)
            {
                try
                {
                    var isImage = IsImage(fileName);
                    var cleanFileName = Path.GetFileNameWithoutExtension(fileName).Replace(" ", "_");
                    var cleanExt = Path.GetExtension(fileName);
                    
                    // Generate unique Public ID inside coachos/{folderName}
                    var uniqueId = Guid.NewGuid().ToString("N")[..12];
                    var publicId = $"coachos/{folderName}/{cleanFileName}_{uniqueId}";

                    if (isImage)
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(fileName, fileStream),
                            PublicId = publicId,
                            DisplayName = fileName,
                            Overwrite = true
                        };

                        var result = await _cloudinary.UploadAsync(uploadParams);
                        if (result.Error != null)
                        {
                            _logger.LogError("Cloudinary image upload error: {Message}", result.Error.Message);
                            throw new InvalidOperationException($"Cloudinary image upload failed: {result.Error.Message}");
                        }

                        _logger.LogInformation("Uploaded image to Cloudinary. Public ID: {PublicId}", result.PublicId);
                        return result.PublicId;
                    }
                    else
                    {
                        // For raw files (PDFs, docs, spreadsheets, etc.), preserve extension in the Public ID
                        var rawPublicId = $"{publicId}{cleanExt}";
                        var rawParams = new RawUploadParams
                        {
                            File = new FileDescription(fileName, fileStream),
                            PublicId = rawPublicId,
                            DisplayName = fileName,
                            Overwrite = true
                        };

                        var result = await _cloudinary.UploadAsync(rawParams);
                        if (result.Error != null)
                        {
                            _logger.LogError("Cloudinary raw file upload error: {Message}", result.Error.Message);
                            throw new InvalidOperationException($"Cloudinary file upload failed: {result.Error.Message}");
                        }

                        _logger.LogInformation("Uploaded raw file to Cloudinary. Public ID: {PublicId}", result.PublicId);
                        return result.PublicId;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during Cloudinary upload for file: {FileName}", fileName);
                    throw;
                }
            }

            // Fallback to local storage if Cloudinary credentials are not configured yet
            _logger.LogWarning("Saving file locally as Cloudinary is not configured.");
            var localFolderPath = Path.Combine(_localBasePath, folderName);
            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }

            var localUniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var localFilePath = Path.Combine(localFolderPath, localUniqueFileName);

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            return Path.Combine("uploads", folderName, localUniqueFileName).Replace("\\", "/");
        }

        public string GetFileUrl(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;

            if (filePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                filePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return filePath;
            }

            // If it's a Cloudinary Public ID
            if (_cloudinary != null)
            {
                var isImage = IsImage(filePath);
                if (isImage)
                {
                    return _cloudinary.Api.UrlImgUp.Secure(true).BuildUrl(filePath);
                }
                else
                {
                    return _cloudinary.Api.Url.ResourceType("raw").Secure(true).BuildUrl(filePath);
                }
            }

            // Fallback for local path
            return $"/{filePath.TrimStart('/').Replace("\\", "/")}";
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path or ID cannot be empty.", nameof(filePath));

            // If it's a URL or Cloudinary Public ID
            if (_cloudinary != null || filePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || filePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var fileUrl = GetFileUrl(filePath);
                    if (fileUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || fileUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        return await _httpClient.GetByteArrayAsync(fileUrl);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to download file from Cloudinary URL for ID: {Id}. Checking local fallback.", filePath);
                }
            }

            // Local fallback (also handles legacy records saved on disk)
            var normalizedLocalPath = filePath.TrimStart('/').Replace("/", "\\");
            var localFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", normalizedLocalPath);
            if (File.Exists(localFullPath))
            {
                return await File.ReadAllBytesAsync(localFullPath);
            }

            throw new FileNotFoundException("File could not be found either on Cloudinary or locally.", filePath);
        }

        public void DeleteFile(string filePath)
        {
            DeleteFileAsync(filePath).GetAwaiter().GetResult();
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            if (_cloudinary != null && !filePath.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var isImage = IsImage(filePath);
                    var delParams = new DeletionParams(filePath)
                    {
                        ResourceType = isImage ? ResourceType.Image : ResourceType.Raw
                    };

                    var result = await _cloudinary.DestroyAsync(delParams);
                    _logger.LogInformation("Deleted Cloudinary asset {PublicId}: {Result}", filePath, result.Result);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error deleting file from Cloudinary for path: {FilePath}", filePath);
                }
            }

            // Also clean up local file if present
            try
            {
                var localFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.Replace("/", "\\"));
                if (File.Exists(localFullPath))
                {
                    File.Delete(localFullPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting local file for path: {FilePath}", filePath);
            }
        }
    }
}

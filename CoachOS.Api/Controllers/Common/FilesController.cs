using CoachOS.Application.Interfaces.Services;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Common
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FilesController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("upload")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string folder = "general")
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<object>.Fail("No file uploaded."));
            }

            try
            {
                using var stream = file.OpenReadStream();
                var publicId = await _fileStorageService.SaveFileAsync(stream, file.FileName, folder);
                var url = _fileStorageService.GetFileUrl(publicId);

                return Ok(ApiResponse<object>.Ok(new
                {
                    PublicId = publicId,
                    Url = url,
                    FileName = file.FileName,
                    Size = file.Length,
                    ContentType = file.ContentType
                }, "File uploaded successfully to Cloudinary."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"File upload failed: {ex.Message}"));
            }
        }

        [HttpGet("view")]
        [AllowAnonymous]
        public IActionResult ViewFile([FromQuery] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("File ID is required.");
            }

            var url = _fileStorageService.GetFileUrl(id);
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return Redirect(url);
            }

            // Local fallback
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", id.TrimStart('/').Replace("/", "\\"));
            if (System.IO.File.Exists(fullPath))
            {
                var ext = Path.GetExtension(fullPath).ToLower();
                var contentType = ext switch
                {
                    ".png" => "image/png",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".webp" => "image/webp",
                    ".gif" => "image/gif",
                    ".svg" => "image/svg+xml",
                    ".pdf" => "application/pdf",
                    _ => "application/octet-stream"
                };
                return PhysicalFile(fullPath, contentType);
            }

            return NotFound("File not found.");
        }

        [HttpGet("url")]
        [AllowAnonymous]
        public IActionResult GetUrl([FromQuery] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ApiResponse<string>.Fail("File ID is required."));
            }

            var url = _fileStorageService.GetFileUrl(id);
            return Ok(ApiResponse<object>.Ok(new { PublicId = id, Url = url }));
        }

        [HttpGet("download")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadFile([FromQuery] string id, [FromQuery] string? fileName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("File ID is required.");
            }

            try
            {
                var bytes = await _fileStorageService.GetFileAsync(id);
                var downloadName = !string.IsNullOrWhiteSpace(fileName) ? fileName : Path.GetFileName(id);
                var ext = Path.GetExtension(downloadName).ToLower();
                var contentType = ext switch
                {
                    ".pdf" => "application/pdf",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    ".png" => "image/png",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".webp" => "image/webp",
                    _ => "application/octet-stream"
                };

                return File(bytes, contentType, downloadName);
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving file: {ex.Message}");
            }
        }
    }
}

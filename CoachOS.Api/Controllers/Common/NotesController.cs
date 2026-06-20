using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Learning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Common
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public NotesController(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        [HttpGet("download/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var note = await _unitOfWork.Repository<Note>().FirstOrDefaultAsync(n => n.Id == id, ignoreQueryFilters: true);
            if (note == null)
            {
                return NotFound("Study material not found.");
            }



            try
            {
                var fileBytes = await _fileStorageService.GetFileAsync(note.FilePath);
                var contentType = !string.IsNullOrEmpty(note.FileType) ? note.FileType : "application/octet-stream";
                return File(fileBytes, contentType, note.OriginalFileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound("The physical file could not be found on the server.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while serving the file: {ex.Message}");
            }
        }
    }
}

using CoachOS.Application.Features.Students.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Student;
using CoachOS.Shared.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Application.Features.Students
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public StudentService(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        public async Task<ApiResponse<StudentDto>> CreateStudentAsync(CreateStudentRequest request)
        {
            var student = request.Adapt<Student>();
            await _unitOfWork.Repository<Student>().AddAsync(student);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<StudentDto>.Ok(student.Adapt<StudentDto>(), "Student created successfully.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<StudentDto>>> GetStudentsAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var studentsPaged = await _unitOfWork.Repository<Student>().GetPagedAsync(paginationParams);
            var studentsDto = studentsPaged.Data.Adapt<List<StudentDto>>();
            var result = new CoachOS.Shared.Responses.PagedResult<StudentDto>(studentsDto, studentsPaged.TotalCount, studentsPaged.CurrentPage, studentsPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<StudentDto>>.Ok(result);
        }

        public async Task<ApiResponse<StudentDto>> GetStudentByIdAsync(Guid id)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);
            if (student == null) return ApiResponse<StudentDto>.Fail("Student not found.");
            return ApiResponse<StudentDto>.Ok(student.Adapt<StudentDto>());
        }

        public async Task<ApiResponse<StudentDto>> UpdateStudentAsync(Guid id, UpdateStudentRequest request)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);
            if (student == null) return ApiResponse<StudentDto>.Fail("Student not found.");

            request.Adapt(student);
            _unitOfWork.Repository<Student>().Update(student);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<StudentDto>.Ok(student.Adapt<StudentDto>(), "Student updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteStudentAsync(Guid id)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);
            if (student == null) return ApiResponse<bool>.Fail("Student not found.");

            _unitOfWork.Repository<Student>().Remove(student);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Student deleted successfully.");
        }

        public async Task<ApiResponse<string>> GetNextStudentCodeAsync(string prefix)
        {
            var currentYear = DateTime.UtcNow.Year;
            var rawPrefix = string.IsNullOrWhiteSpace(prefix) ? "STU" : prefix.Trim();
            
            // Normalize prefix
            string normalizedPrefix;
            if (rawPrefix.Equals("DEMO", StringComparison.OrdinalIgnoreCase))
            {
                normalizedPrefix = "DEMO";
            }
            else if (rawPrefix.Equals("PERMANENT", StringComparison.OrdinalIgnoreCase) || rawPrefix.Equals("STU", StringComparison.OrdinalIgnoreCase))
            {
                normalizedPrefix = "STU";
            }
            else
            {
                normalizedPrefix = rawPrefix.TrimEnd('-');
            }

            var students = await _unitOfWork.Repository<Student>().GetAllAsync();
            
            // Find all student codes matching the prefix pattern
            var prefixPattern = normalizedPrefix.ToLower();
            var existingCodes = students
                .Select(s => s.StudentCode)
                .Where(c => !string.IsNullOrWhiteSpace(c) && c.ToLower().Contains(prefixPattern))
                .ToList();

            var maxSeq = 0;
            foreach (var code in existingCodes)
            {
                var match = System.Text.RegularExpressions.Regex.Match(code.Trim(), @"(\d+)$");
                if (match.Success && int.TryParse(match.Value, out var num))
                {
                    if (num > maxSeq)
                    {
                        maxSeq = num;
                    }
                }
            }

            var nextNumber = maxSeq + 1;
            var nextCode = $"{normalizedPrefix}-{currentYear}-{nextNumber:D3}";
            return ApiResponse<string>.Ok(nextCode);
        }

        public async Task<ApiResponse<string>> UploadProfilePictureAsync(Guid id, Stream fileStream, string fileName)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);
            if (student == null) return ApiResponse<string>.Fail("Student not found.");

            if (!string.IsNullOrEmpty(student.ProfileImagePath))
            {
                try
                {
                    _fileStorageService.DeleteFile(student.ProfileImagePath);
                }
                catch
                {
                    // Ignore deletion errors for legacy files
                }
            }

            var relativePath = await _fileStorageService.SaveFileAsync(fileStream, fileName, "profiles");
            student.ProfileImagePath = relativePath;
            
            _unitOfWork.Repository<Student>().Update(student);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<string>.Ok(relativePath, "Profile picture uploaded successfully.");
        }
    }
}

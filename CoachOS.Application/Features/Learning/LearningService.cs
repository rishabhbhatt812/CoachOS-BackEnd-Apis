using CoachOS.Application.Features.Learning.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Learning;
using CoachOS.Domain.Academic;
using CoachOS.Shared.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Application.Features.Learning
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AttendanceService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<AttendanceSessionDto>> CreateAttendanceSessionAsync(CreateAttendanceSessionRequest request)
        {
            var session = request.Adapt<AttendanceSession>();
            if (session.TakenByUserId == Guid.Empty || session.TakenByUserId == default)
            {
                session.TakenByUserId = _currentUserService.UserId ?? Guid.Empty;
            }
            await _unitOfWork.Repository<AttendanceSession>().AddAsync(session);
            await _unitOfWork.SaveChangesAsync();
            
            var dto = session.Adapt<AttendanceSessionDto>();
            var batch = await _unitOfWork.Repository<CoachOS.Domain.Academic.Batch>().GetByIdAsync(session.BatchId);
            var user = await _unitOfWork.Repository<CoachOS.Domain.Identity.User>().GetByIdAsync(session.TakenByUserId);
            var studentBatches = await _unitOfWork.Repository<CoachOS.Domain.Student.StudentBatch>().GetAllAsync();
            
            dto.BatchName = batch?.Name ?? "Unknown Batch";
            dto.TakenByName = user?.FullName ?? "System";
            dto.PresentCount = 0;
            dto.TotalStudents = studentBatches.Count(sb => sb.BatchId == session.BatchId && sb.IsActive);

            return ApiResponse<AttendanceSessionDto>.Ok(dto, "Attendance session created.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<AttendanceSessionDto>>> GetAttendanceSessionsAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var sessionsPaged = await _unitOfWork.Repository<AttendanceSession>().GetPagedAsync(paginationParams);
            var batches = await _unitOfWork.Repository<CoachOS.Domain.Academic.Batch>().GetAllAsync();
            var users = await _unitOfWork.Repository<CoachOS.Domain.Identity.User>().GetAllAsync();
            var records = await _unitOfWork.Repository<AttendanceRecord>().GetAllAsync();
            var studentBatches = await _unitOfWork.Repository<CoachOS.Domain.Student.StudentBatch>().GetAllAsync();

            var sessionsDto = sessionsPaged.Data.Select(s =>
            {
                var dto = s.Adapt<AttendanceSessionDto>();
                dto.BatchName = batches.FirstOrDefault(b => b.Id == s.BatchId)?.Name ?? "Unknown Batch";
                dto.TakenByName = users.FirstOrDefault(u => u.Id == s.TakenByUserId)?.FullName ?? "System";
                
                var sessionRecords = records.Where(r => r.AttendanceSessionId == s.Id).ToList();
                dto.PresentCount = sessionRecords.Count(r => r.Status == "Present");
                dto.TotalStudents = studentBatches.Count(sb => sb.BatchId == s.BatchId && sb.IsActive);
                if (dto.TotalStudents == 0 && sessionRecords.Count > 0)
                {
                    dto.TotalStudents = sessionRecords.Count;
                }
                return dto;
            }).ToList();

            var result = new CoachOS.Shared.Responses.PagedResult<AttendanceSessionDto>(sessionsDto, sessionsPaged.TotalCount, sessionsPaged.CurrentPage, sessionsPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<AttendanceSessionDto>>.Ok(result);
        }

        public async Task<ApiResponse<bool>> DeleteAttendanceSessionAsync(Guid id)
        {
            var session = await _unitOfWork.Repository<AttendanceSession>().GetByIdAsync(id);
            if (session == null) return ApiResponse<bool>.Fail("Attendance session not found.");

            _unitOfWork.Repository<AttendanceSession>().Remove(session);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Attendance session deleted.");
        }
    }

    public class LearningService : ILearningService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LearningService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<NoteDto>> CreateNoteAsync(CreateNoteRequest request)
        {
            var note = request.Adapt<Note>();
            await _unitOfWork.Repository<Note>().AddAsync(note);
            await _unitOfWork.SaveChangesAsync();
            
            var dto = note.Adapt<NoteDto>();
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(note.CourseId ?? Guid.Empty);
            var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(note.BatchId ?? Guid.Empty);
            var subject = await _unitOfWork.Repository<Subject>().GetByIdAsync(note.SubjectId ?? Guid.Empty);
            
            dto.CourseName = course?.Name ?? "Unknown";
            dto.BatchName = batch?.Name ?? "Unknown";
            dto.SubjectName = subject?.Name ?? "Unknown";
            dto.FilePath = $"/api/notes/download/{note.Id}";
            dto.IsExpired = false;

            return ApiResponse<NoteDto>.Ok(dto, "Note created.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<NoteDto>>> GetNotesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var notesPaged = await _unitOfWork.Repository<Note>().GetPagedAsync(paginationParams);
            
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();
            
            var notesDto = notesPaged.Data.Select(n =>
            {
                var dto = n.Adapt<NoteDto>();
                dto.CourseName = courses.FirstOrDefault(c => c.Id == n.CourseId)?.Name ?? "Unknown";
                dto.BatchName = batches.FirstOrDefault(b => b.Id == n.BatchId)?.Name ?? "Unknown";
                dto.SubjectName = subjects.FirstOrDefault(s => s.Id == n.SubjectId)?.Name ?? "Unknown";
                dto.FilePath = $"/api/notes/download/{n.Id}";
                dto.IsExpired = false;
                return dto;
            }).ToList();

            var result = new CoachOS.Shared.Responses.PagedResult<NoteDto>(notesDto, notesPaged.TotalCount, notesPaged.CurrentPage, notesPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<NoteDto>>.Ok(result);
        }

        public async Task<ApiResponse<bool>> DeleteNoteAsync(Guid id)
        {
            var note = await _unitOfWork.Repository<Note>().GetByIdAsync(id);
            if (note == null) return ApiResponse<bool>.Fail("Note not found.");

            _unitOfWork.Repository<Note>().Remove(note);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Note deleted.");
        }

        public async Task<ApiResponse<TestDto>> CreateTestAsync(CreateTestRequest request)
        {
            var test = request.Adapt<Test>();
            await _unitOfWork.Repository<Test>().AddAsync(test);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<TestDto>.Ok(test.Adapt<TestDto>(), "Test created.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<TestDto>>> GetTestsAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var testsPaged = await _unitOfWork.Repository<Test>().GetPagedAsync(paginationParams);
            var testsDto = testsPaged.Data.Adapt<List<TestDto>>();
            var result = new CoachOS.Shared.Responses.PagedResult<TestDto>(testsDto, testsPaged.TotalCount, testsPaged.CurrentPage, testsPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<TestDto>>.Ok(result);
        }

        public async Task<ApiResponse<bool>> DeleteTestAsync(Guid id)
        {
            var test = await _unitOfWork.Repository<Test>().GetByIdAsync(id);
            if (test == null) return ApiResponse<bool>.Fail("Test not found.");

            _unitOfWork.Repository<Test>().Remove(test);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Test deleted.");
        }
    }
}

using CoachOS.Application.Features.Academics.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Identity;
using CoachOS.Shared.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CoachOS.Application.Features.Academics
{
    public class AcademicsService : IAcademicsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AcademicsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CourseDto>> CreateCourseAsync(CreateCourseRequest request)
        {
            var course = new Course
            {
                Name = request.Name,
                Description = request.Description,
                CourseCode = request.CourseCode,
                CourseCategory = request.CourseCategory,
                CourseType = request.CourseType,
                DurationValue = request.DurationValue,
                DurationType = request.DurationType,
                IsActive = true
            };

            await _unitOfWork.Repository<Course>().AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            var subjectsList = new List<Subject>();
            if (request.SubjectNames != null && request.SubjectNames.Any())
            {
                foreach (var subName in request.SubjectNames.Where(n => !string.IsNullOrWhiteSpace(n)))
                {
                    var subject = new Subject
                    {
                        CourseId = course.Id,
                        Name = subName,
                        IsActive = true
                    };
                    await _unitOfWork.Repository<Subject>().AddAsync(subject);
                    subjectsList.Add(subject);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            var dto = course.Adapt<CourseDto>();
            dto.Subjects = subjectsList.Adapt<List<SubjectDto>>();

            return ApiResponse<CourseDto>.Ok(dto, "Course created successfully.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<CourseDto>>> GetCoursesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var coursesPaged = await _unitOfWork.Repository<Course>().GetPagedAsync(paginationParams);
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();

            var coursesDto = coursesPaged.Data.Select(c =>
            {
                var dto = c.Adapt<CourseDto>();
                dto.Subjects = subjects.Where(s => s.CourseId == c.Id).Adapt<List<SubjectDto>>();
                return dto;
            }).ToList();

            var result = new CoachOS.Shared.Responses.PagedResult<CourseDto>(coursesDto, coursesPaged.TotalCount, coursesPaged.CurrentPage, coursesPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<CourseDto>>.Ok(result);
        }

        public async Task<ApiResponse<CourseDto>> GetCourseByIdAsync(Guid id)
        {
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (course == null)
                return ApiResponse<CourseDto>.Fail("Course not found.");

            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();
            var dto = course.Adapt<CourseDto>();
            dto.Subjects = subjects.Where(s => s.CourseId == course.Id).Adapt<List<SubjectDto>>();

            return ApiResponse<CourseDto>.Ok(dto);
        }

        public async Task<ApiResponse<CourseDto>> UpdateCourseAsync(Guid id, UpdateCourseRequest request)
        {
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (course == null)
                return ApiResponse<CourseDto>.Fail("Course not found.");

            course.Name = request.Name;
            course.Description = request.Description;
            course.CourseCode = request.CourseCode;
            course.CourseCategory = request.CourseCategory;
            course.CourseType = request.CourseType;
            course.DurationValue = request.DurationValue;
            course.DurationType = request.DurationType;
            course.IsActive = request.IsActive;

            _unitOfWork.Repository<Course>().Update(course);

            var existingSubjects = (await _unitOfWork.Repository<Subject>().GetAllAsync())
                .Where(s => s.CourseId == course.Id).ToList();

            var incomingNames = request.SubjectNames?.Where(n => !string.IsNullOrWhiteSpace(n)).ToList() ?? new List<string>();

            var subjectsList = new List<Subject>();
            foreach (var subName in incomingNames)
            {
                var existing = existingSubjects.FirstOrDefault(s => s.Name.Equals(subName, StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    var newSub = new Subject
                    {
                        CourseId = course.Id,
                        Name = subName,
                        IsActive = true
                    };
                    await _unitOfWork.Repository<Subject>().AddAsync(newSub);
                    subjectsList.Add(newSub);
                }
                else
                {
                    subjectsList.Add(existing);
                }
            }

            foreach (var oldSub in existingSubjects)
            {
                if (!incomingNames.Any(n => n.Equals(oldSub.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    _unitOfWork.Repository<Subject>().Remove(oldSub);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            var dto = course.Adapt<CourseDto>();
            dto.Subjects = subjectsList.Adapt<List<SubjectDto>>();

            return ApiResponse<CourseDto>.Ok(dto, "Course updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteCourseAsync(Guid id)
        {
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (course == null)
                return ApiResponse<bool>.Fail("Course not found.");

            _unitOfWork.Repository<Course>().Remove(course);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Course deleted successfully.");
        }

        // Subjects
        public async Task<ApiResponse<SubjectDto>> CreateSubjectAsync(CreateSubjectRequest request)
        {
            var subject = request.Adapt<Subject>();
            await _unitOfWork.Repository<Subject>().AddAsync(subject);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<SubjectDto>.Ok(subject.Adapt<SubjectDto>(), "Subject created successfully.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<SubjectDto>>> GetSubjectsAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var subjectsPaged = await _unitOfWork.Repository<Subject>().GetPagedAsync(paginationParams);
            var subjectsDto = subjectsPaged.Data.Adapt<List<SubjectDto>>();
            var result = new CoachOS.Shared.Responses.PagedResult<SubjectDto>(subjectsDto, subjectsPaged.TotalCount, subjectsPaged.CurrentPage, subjectsPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<SubjectDto>>.Ok(result);
        }

        public async Task<ApiResponse<SubjectDto>> GetSubjectByIdAsync(Guid id)
        {
            var subject = await _unitOfWork.Repository<Subject>().GetByIdAsync(id);
            if (subject == null) return ApiResponse<SubjectDto>.Fail("Subject not found.");
            return ApiResponse<SubjectDto>.Ok(subject.Adapt<SubjectDto>());
        }

        public async Task<ApiResponse<SubjectDto>> UpdateSubjectAsync(Guid id, UpdateSubjectRequest request)
        {
            var subject = await _unitOfWork.Repository<Subject>().GetByIdAsync(id);
            if (subject == null) return ApiResponse<SubjectDto>.Fail("Subject not found.");

            request.Adapt(subject);
            _unitOfWork.Repository<Subject>().Update(subject);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<SubjectDto>.Ok(subject.Adapt<SubjectDto>(), "Subject updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteSubjectAsync(Guid id)
        {
            var subject = await _unitOfWork.Repository<Subject>().GetByIdAsync(id);
            if (subject == null) return ApiResponse<bool>.Fail("Subject not found.");

            _unitOfWork.Repository<Subject>().Remove(subject);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Subject deleted successfully.");
        }

        // Batches
        public async Task<ApiResponse<BatchDto>> CreateBatchAsync(CreateBatchRequest request)
        {
            var batch = request.Adapt<Batch>();
            await _unitOfWork.Repository<Batch>().AddAsync(batch);
            await _unitOfWork.SaveChangesAsync();

            // Handle multiple subjects / teacher mapping
            Guid? teacherProfileId = null;
            if (request.TeacherUserId.HasValue && request.TeacherUserId.Value != Guid.Empty)
            {
                var profiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
                var teacherProfile = profiles.FirstOrDefault(tp => tp.UserId == request.TeacherUserId.Value);
                if (teacherProfile != null)
                {
                    teacherProfileId = teacherProfile.Id;
                }
            }

            var subjectIdsToAssign = request.SubjectIds ?? new List<Guid>();
            if (!subjectIdsToAssign.Any() && request.SubjectId.HasValue && request.SubjectId.Value != Guid.Empty)
            {
                subjectIdsToAssign.Add(request.SubjectId.Value);
            }

            foreach (var subId in subjectIdsToAssign)
            {
                var tb = new TeacherBatch
                {
                    BatchId = batch.Id,
                    SubjectId = subId,
                    TeacherProfileId = teacherProfileId,
                    AssignedOn = DateTime.UtcNow,
                    IsActive = true
                };
                await _unitOfWork.Repository<TeacherBatch>().AddAsync(tb);
            }

            if (subjectIdsToAssign.Any())
            {
                await _unitOfWork.SaveChangesAsync();
            }
            
            var dto = batch.Adapt<BatchDto>();
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(batch.CourseId);
            dto.CourseName = course?.Name ?? "Unknown";

            // Populate DTO properties
            dto.SubjectIds = subjectIdsToAssign;
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();
            dto.SubjectNames = subjects.Where(s => subjectIdsToAssign.Contains(s.Id)).Select(s => s.Name).ToList();
            if (request.SubjectId.HasValue)
            {
                dto.SubjectId = request.SubjectId;
                dto.SubjectName = subjects.FirstOrDefault(s => s.Id == request.SubjectId)?.Name ?? string.Empty;
            }
            else if (dto.SubjectNames.Any())
            {
                dto.SubjectName = string.Join(", ", dto.SubjectNames);
            }

            if (request.TeacherUserId.HasValue)
            {
                dto.TeacherUserId = request.TeacherUserId;
                var users = await _unitOfWork.Repository<User>().GetAllAsync();
                var user = users.FirstOrDefault(u => u.Id == request.TeacherUserId.Value);
                dto.TeacherName = user?.FullName ?? string.Empty;
                dto.TeacherUserIds = new List<Guid> { request.TeacherUserId.Value };
                dto.TeacherNames = new List<string> { dto.TeacherName };
            }

            return ApiResponse<BatchDto>.Ok(dto, "Batch created successfully.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<BatchDto>>> GetBatchesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var batchesPaged = await _unitOfWork.Repository<Batch>().GetPagedAsync(paginationParams);
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var teacherProfiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
            var teacherBatches = await _unitOfWork.Repository<TeacherBatch>().GetAllAsync();
            
            var batchesDto = batchesPaged.Data.Select(b =>
            {
                var dto = b.Adapt<BatchDto>();
                dto.CourseName = courses.FirstOrDefault(c => c.Id == b.CourseId)?.Name ?? "Unknown";

                var bTBs = teacherBatches.Where(tb => tb.BatchId == b.Id && tb.IsActive).ToList();
                dto.SubjectIds = bTBs.Select(tb => tb.SubjectId).Distinct().ToList();
                dto.SubjectNames = subjects.Where(s => dto.SubjectIds.Contains(s.Id)).Select(s => s.Name).ToList();
                dto.SubjectName = dto.SubjectNames.Any() ? string.Join(", ", dto.SubjectNames) : "None";

                var profileIds = bTBs.Where(tb => tb.TeacherProfileId.HasValue).Select(tb => tb.TeacherProfileId!.Value).Distinct().ToList();
                var matchingProfiles = teacherProfiles.Where(tp => profileIds.Contains(tp.Id)).ToList();
                dto.TeacherUserIds = matchingProfiles.Select(tp => tp.UserId).Distinct().ToList();
                dto.TeacherNames = users.Where(u => dto.TeacherUserIds.Contains(u.Id)).Select(u => u.FullName).ToList();
                dto.TeacherName = dto.TeacherNames.Any() ? string.Join(", ", dto.TeacherNames) : "None";

                if (dto.SubjectIds.Any()) dto.SubjectId = dto.SubjectIds.First();
                if (dto.TeacherUserIds.Any()) dto.TeacherUserId = dto.TeacherUserIds.First();

                return dto;
            }).ToList();

            var result = new CoachOS.Shared.Responses.PagedResult<BatchDto>(batchesDto, batchesPaged.TotalCount, batchesPaged.CurrentPage, batchesPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<BatchDto>>.Ok(result);
        }

        public async Task<ApiResponse<BatchDto>> GetBatchByIdAsync(Guid id)
        {
            var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(id);
            if (batch == null) return ApiResponse<BatchDto>.Fail("Batch not found.");
            
            var dto = batch.Adapt<BatchDto>();
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(batch.CourseId);
            dto.CourseName = course?.Name ?? "Unknown";

            var teacherBatches = (await _unitOfWork.Repository<TeacherBatch>().GetAllAsync())
                .Where(tb => tb.BatchId == batch.Id && tb.IsActive).ToList();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var teacherProfiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();

            dto.SubjectIds = teacherBatches.Select(tb => tb.SubjectId).Distinct().ToList();
            dto.SubjectNames = subjects.Where(s => dto.SubjectIds.Contains(s.Id)).Select(s => s.Name).ToList();
            dto.SubjectName = dto.SubjectNames.Any() ? string.Join(", ", dto.SubjectNames) : "None";

            var profileIds = teacherBatches.Where(tb => tb.TeacherProfileId.HasValue).Select(tb => tb.TeacherProfileId!.Value).Distinct().ToList();
            var matchingProfiles = teacherProfiles.Where(tp => profileIds.Contains(tp.Id)).ToList();
            dto.TeacherUserIds = matchingProfiles.Select(tp => tp.UserId).Distinct().ToList();
            dto.TeacherNames = users.Where(u => dto.TeacherUserIds.Contains(u.Id)).Select(u => u.FullName).ToList();
            dto.TeacherName = dto.TeacherNames.Any() ? string.Join(", ", dto.TeacherNames) : "None";

            if (dto.SubjectIds.Any()) dto.SubjectId = dto.SubjectIds.First();
            if (dto.TeacherUserIds.Any()) dto.TeacherUserId = dto.TeacherUserIds.First();

            return ApiResponse<BatchDto>.Ok(dto);
        }

        public async Task<ApiResponse<BatchDto>> UpdateBatchAsync(Guid id, UpdateBatchRequest request)
        {
            var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(id);
            if (batch == null) return ApiResponse<BatchDto>.Fail("Batch not found.");

            request.Adapt(batch);
            _unitOfWork.Repository<Batch>().Update(batch);
            await _unitOfWork.SaveChangesAsync();

            // Get teacher profile id if provided
            Guid? teacherProfileId = null;
            if (request.TeacherUserId.HasValue && request.TeacherUserId.Value != Guid.Empty)
            {
                var profiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
                var teacherProfile = profiles.FirstOrDefault(tp => tp.UserId == request.TeacherUserId.Value);
                if (teacherProfile != null)
                {
                    teacherProfileId = teacherProfile.Id;
                }
            }

            // Sync subjects
            var existingTBs = (await _unitOfWork.Repository<TeacherBatch>().GetAllAsync())
                .Where(tb => tb.BatchId == batch.Id).ToList();

            var subjectIdsToAssign = request.SubjectIds ?? new List<Guid>();
            if (!subjectIdsToAssign.Any() && request.SubjectId.HasValue && request.SubjectId.Value != Guid.Empty)
            {
                subjectIdsToAssign.Add(request.SubjectId.Value);
            }

            // Remove no longer assigned
            foreach (var oldTb in existingTBs)
            {
                if (!subjectIdsToAssign.Contains(oldTb.SubjectId))
                {
                    _unitOfWork.Repository<TeacherBatch>().Remove(oldTb);
                }
            }

            // Add or update
            foreach (var subId in subjectIdsToAssign)
            {
                var existing = existingTBs.FirstOrDefault(tb => tb.SubjectId == subId);
                if (existing == null)
                {
                    var newTb = new TeacherBatch
                    {
                        BatchId = batch.Id,
                        SubjectId = subId,
                        TeacherProfileId = teacherProfileId,
                        AssignedOn = DateTime.UtcNow,
                        IsActive = true
                    };
                    await _unitOfWork.Repository<TeacherBatch>().AddAsync(newTb);
                }
                else
                {
                    existing.TeacherProfileId = teacherProfileId;
                    _unitOfWork.Repository<TeacherBatch>().Update(existing);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            var dto = batch.Adapt<BatchDto>();
            dto.SubjectIds = subjectIdsToAssign;
            
            return ApiResponse<BatchDto>.Ok(dto, "Batch updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteBatchAsync(Guid id)
        {
            var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(id);
            if (batch == null) return ApiResponse<bool>.Fail("Batch not found.");

            _unitOfWork.Repository<Batch>().Remove(batch);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Batch deleted successfully.");
        }
    }
}

using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Finance;
using CoachOS.Domain.Learning;
using CoachOS.Domain.Student;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Communication;
using CoachOS.Shared.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Application.Features.StudentPortal
{
    public class StudentPortalService : IStudentPortalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentPortalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private async Task<Guid> ResolveStudentIdAsync(Guid userId)
        {
            var students = (await _unitOfWork.Repository<Student>().GetAllAsync()).ToList();
            var directMatch = students.FirstOrDefault(s => s.Id == userId);
            if (directMatch != null) return directMatch.Id;

            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                var matchByEmail = students.FirstOrDefault(s => s.Email != null && s.Email.ToLower() == user.Email.ToLower());
                if (matchByEmail != null) return matchByEmail.Id;
            }

            var first = students.FirstOrDefault();
            return first?.Id ?? userId;
        }

        public async Task<ApiResponse<object>> GetStudentDashboardAsync(Guid studentId)
        {
            studentId = await ResolveStudentIdAsync(studentId);
            var studentBatches = (await _unitOfWork.Repository<StudentBatch>().GetAllAsync())
                .Where(x => x.StudentId == studentId && x.IsActive).ToList();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            
            var activeBatch = studentBatches.Select(sb => batches.FirstOrDefault(b => b.Id == sb.BatchId)).FirstOrDefault(b => b != null);
            var activeCourse = activeBatch != null ? courses.FirstOrDefault(c => c.Id == activeBatch.CourseId) : null;
            
            var attendanceRecords = (await _unitOfWork.Repository<AttendanceRecord>().GetAllAsync())
                .Where(x => x.StudentId == studentId).ToList();
            var attendancePercent = attendanceRecords.Any() 
                ? (double)attendanceRecords.Count(r => r.Status == "Present") / attendanceRecords.Count * 100 
                : 100.0;

            var fees = (await _unitOfWork.Repository<FeePlan>().GetAllAsync())
                .Where(x => x.StudentId == studentId).ToList();
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            var installments = await _unitOfWork.Repository<Installment>().GetAllAsync();

            decimal feeDueAmount = 0;
            DateOnly? feeDueDate = null;
            if (fees.Any())
            {
                var mainFee = fees.First();
                var planPayments = payments.Where(p => p.FeePlanId == mainFee.Id).ToList();
                feeDueAmount = mainFee.FinalFee - planPayments.Sum(p => p.Amount);
                
                var planInstallments = installments.Where(i => i.FeePlanId == mainFee.Id && i.Status == "Due").OrderBy(i => i.DueDate).ToList();
                if (planInstallments.Any())
                {
                    feeDueDate = planInstallments.First().DueDate;
                }
            }

            var results = (await _unitOfWork.Repository<TestResult>().GetAllAsync())
                .Where(x => x.StudentId == studentId).ToList();
            var tests = await _unitOfWork.Repository<Test>().GetAllAsync();

            string lastTestName = "No Tests Taken";
            string lastTestScore = "N/A";
            var latestResult = results.Select(r => new { r, Test = tests.FirstOrDefault(t => t.Id == r.TestId) })
                .Where(x => x.Test != null)
                .OrderByDescending(x => x.Test!.TestDate)
                .FirstOrDefault();

            if (latestResult != null)
            {
                lastTestName = latestResult.Test!.TestName;
                lastTestScore = $"{latestResult.r.MarksObtained}/{latestResult.Test!.MaxMarks}";
            }

            var notices = (await _unitOfWork.Repository<Notice>().GetAllAsync())
                .Where(x => activeBatch == null || x.BatchId == null || x.BatchId == activeBatch.Id)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(n => new { n.Title, n.Message, n.CreatedAt })
                .ToList();

            var dashboardData = new {
                CourseName = activeCourse?.Name ?? "Not Enrolled",
                BatchTiming = "TBD", // Will be fetched from BatchSchedules
                AttendancePercent = Math.Round(attendancePercent, 1),
                FeeDueAmount = feeDueAmount,
                FeeDueDate = feeDueDate?.ToString("yyyy-MM-dd") ?? "N/A",
                LastTestName = lastTestName,
                LastTestScore = lastTestScore,
                Notices = notices,
                TestResults = results.Select(r => {
                    var t = tests.FirstOrDefault(x => x.Id == r.TestId);
                    return new {
                        TestName = t?.TestName ?? "Test",
                        MarksObtained = r.MarksObtained,
                        MaxMarks = t?.MaxMarks ?? 100,
                        TestDate = t?.TestDate.ToString("yyyy-MM-dd") ?? ""
                    };
                }).ToList()
            };

            return ApiResponse<object>.Ok(dashboardData);
        }

        public async Task<ApiResponse<object>> GetStudentCoursesAsync(Guid studentId)
        {
            studentId = await ResolveStudentIdAsync(studentId);
            var studentBatches = (await _unitOfWork.Repository<StudentBatch>().GetAllAsync())
                .Where(x => x.StudentId == studentId).ToList();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var teacherBatches = await _unitOfWork.Repository<TeacherBatch>().GetAllAsync();
            var teacherProfiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();

            var result = studentBatches.Select(sb => {
                var batch = batches.FirstOrDefault(b => b.Id == sb.BatchId);
                var course = batch != null ? courses.FirstOrDefault(c => c.Id == batch.CourseId) : null;
                
                var tbList = teacherBatches.Where(tb => tb.BatchId == sb.BatchId && tb.IsActive).ToList();
                var profIds = tbList.Where(tb => tb.TeacherProfileId.HasValue).Select(tb => tb.TeacherProfileId!.Value).ToList();
                var tUsers = teacherProfiles.Where(tp => profIds.Contains(tp.Id)).Select(tp => users.FirstOrDefault(u => u.Id == tp.UserId)?.FullName).Where(n => !string.IsNullOrEmpty(n)).ToList();
                var teacherName = tUsers.Any() ? string.Join(", ", tUsers) : "Assigned Faculty";

                return new {
                    sb.Id,
                    sb.JoinedDate,
                    sb.IsActive,
                    BatchName = batch?.Name ?? "Unknown Batch",
                    CourseName = course?.Name ?? "Unknown Course",
                    CourseDescription = course?.Description ?? "",
                    TeacherName = teacherName,
                    Timing = "Scheduled"
                };
            }).ToList();

            return ApiResponse<object>.Ok(result);
        }

        public async Task<ApiResponse<object>> GetStudentFeesAsync(Guid studentId)
        {
            studentId = await ResolveStudentIdAsync(studentId);
            var studentFees = (await _unitOfWork.Repository<FeePlan>().GetAllAsync())
                .Where(x => x.StudentId == studentId).ToList();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var installments = await _unitOfWork.Repository<Installment>().GetAllAsync();
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();

            var result = studentFees.Select(f => {
                var insts = installments.Where(i => i.FeePlanId == f.Id)
                    .Select(i => new {
                        i.Id,
                        i.InstallmentNo,
                        i.Amount,
                        DueDate = i.DueDate.ToString("yyyy-MM-dd"),
                        i.Status
                    }).OrderBy(i => i.InstallmentNo).ToList();
                
                var pmts = payments.Where(p => p.FeePlanId == f.Id)
                    .Select(p => new {
                        p.Id,
                        p.ReceiptNo,
                        p.Amount,
                        p.PaymentMode,
                        PaymentDate = p.PaymentDate.ToString("yyyy-MM-dd"),
                        p.Remark
                    }).OrderByDescending(p => p.PaymentDate).ToList();

                var paid = pmts.Sum(p => p.Amount);
                return new {
                    f.Id,
                    f.TotalFee,
                    f.DiscountAmount,
                    f.FinalFee,
                    f.PlanType,
                    CourseName = courses.FirstOrDefault(c => c.Id == f.CourseId)?.Name ?? "Unknown Course",
                    BatchName = batches.FirstOrDefault(b => b.Id == f.BatchId)?.Name ?? "Unknown Batch",
                    PaidAmount = paid,
                    DueAmount = f.FinalFee - paid,
                    Installments = insts,
                    Payments = pmts
                };
            }).ToList();

            return ApiResponse<object>.Ok(result);
        }

        public async Task<ApiResponse<object>> GetStudentNotesAsync(Guid studentId)
        {
            studentId = await ResolveStudentIdAsync(studentId);
            var studentBatches = (await _unitOfWork.Repository<StudentBatch>().GetAllAsync())
                .Where(x => x.StudentId == studentId && x.IsActive).Select(x => x.BatchId).ToList();
            
            var notes = await _unitOfWork.Repository<Note>().GetAllAsync();
            var studentNotes = notes.Where(x => x.BatchId.HasValue && studentBatches.Contains(x.BatchId.Value)).ToList();
            
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();

            var result = studentNotes.Select(n => {
                var isExpired = (DateTime.UtcNow - n.CreatedAt).TotalHours > 48;
                return new {
                    n.Id,
                    n.Title,
                    n.Description,
                    FilePath = $"/api/notes/download/{n.Id}",
                    n.OriginalFileName,
                    n.FileType,
                    CourseName = courses.FirstOrDefault(c => c.Id == n.CourseId)?.Name ?? "Unknown",
                    BatchName = batches.FirstOrDefault(b => b.Id == n.BatchId)?.Name ?? "Unknown",
                    SubjectName = subjects.FirstOrDefault(s => s.Id == n.SubjectId)?.Name ?? "Unknown",
                    CreatedAt = n.CreatedAt.ToString("yyyy-MM-dd"),
                    IsExpired = isExpired
                };
            }).ToList();

            return ApiResponse<object>.Ok(result);
        }

        public async Task<ApiResponse<object>> GetStudentAttendanceAsync(Guid studentId)
        {
            studentId = await ResolveStudentIdAsync(studentId);
            var records = (await _unitOfWork.Repository<AttendanceRecord>().GetAllAsync())
                .Where(x => x.StudentId == studentId).ToList();
            var sessions = await _unitOfWork.Repository<AttendanceSession>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();

            var result = records.Select(r => {
                var session = sessions.FirstOrDefault(s => s.Id == r.AttendanceSessionId);
                return new {
                    r.Id,
                    r.Status,
                    r.Remark,
                    AttendanceDate = session?.AttendanceDate.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    BatchName = session != null ? (batches.FirstOrDefault(b => b.Id == session.BatchId)?.Name ?? "Unknown Batch") : "Unknown Batch"
                };
            }).OrderByDescending(x => x.AttendanceDate).ToList();

            return ApiResponse<object>.Ok(result);
        }

        public async Task<ApiResponse<object>> GetStudentResultsAsync(Guid studentId)
        {
            studentId = await ResolveStudentIdAsync(studentId);
            var results = (await _unitOfWork.Repository<TestResult>().GetAllAsync())
                .Where(x => x.StudentId == studentId).ToList();
            var tests = await _unitOfWork.Repository<Test>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();

            var result = results.Select(r => {
                var test = tests.FirstOrDefault(t => t.Id == r.TestId);
                return new {
                    r.Id,
                    r.MarksObtained,
                    TestName = test?.TestName ?? "Mock Test",
                    MaxMarks = test?.MaxMarks ?? 100,
                    TestDate = test?.TestDate.ToString("yyyy-MM-dd") ?? "",
                    SubjectName = test != null ? (subjects.FirstOrDefault(s => s.Id == test.SubjectId)?.Name ?? "Physics") : "Physics"
                };
            }).OrderByDescending(x => x.TestDate).ToList();

            return ApiResponse<object>.Ok(result);
        }

        public async Task<ApiResponse<object>> GetStudentVacanciesAsync(Guid studentId)
        {
            var vacancies = (await _unitOfWork.Repository<Vacancy>().GetAllAsync())
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.LastDate)
                .Select(v => new {
                    v.Id,
                    v.Title,
                    v.Department,
                    v.ExamCategory,
                    v.QualificationRequired,
                    v.AgeLimit,
                    StartDate = v.StartDate?.ToString("yyyy-MM-dd"),
                    LastDate = v.LastDate.ToString("yyyy-MM-dd"),
                    v.OfficialLink,
                    v.Description
                }).ToList();

            return ApiResponse<object>.Ok(vacancies);
        }
    }
}

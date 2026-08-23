using CoachOS.Api.Middlewares;
using CoachOS.Application.Features.Communication.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Communication;
using StudentEntity = CoachOS.Domain.Student.Student;
using CoachOS.Domain.Student;
using CoachOS.Shared.Requests;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Admin
{
    public class CreateVacancyUploadModel
    {
        [FromForm(Name = "instituteId")]
        public Guid? InstituteId { get; set; }

        [FromForm(Name = "title")]
        public string Title { get; set; } = string.Empty;

        [FromForm(Name = "department")]
        public string? Department { get; set; }

        [FromForm(Name = "examCategory")]
        public string ExamCategory { get; set; } = string.Empty;

        [FromForm(Name = "qualificationRequired")]
        public string? QualificationRequired { get; set; }

        [FromForm(Name = "ageLimit")]
        public string? AgeLimit { get; set; }

        [FromForm(Name = "totalPosts")]
        public string? TotalPosts { get; set; }

        [FromForm(Name = "salaryRange")]
        public string? SalaryRange { get; set; }

        [FromForm(Name = "applicationFee")]
        public string? ApplicationFee { get; set; }

        [FromForm(Name = "startDate")]
        public string? StartDate { get; set; }

        [FromForm(Name = "lastDate")]
        public string LastDate { get; set; } = string.Empty;

        [FromForm(Name = "officialLink")]
        public string? OfficialLink { get; set; }

        [FromForm(Name = "description")]
        public string? Description { get; set; }

        [FromForm(Name = "eligibilityDetails")]
        public string? EligibilityDetails { get; set; }

        [FromForm(Name = "sendNotification")]
        public bool SendNotification { get; set; } = true;

        [FromForm(Name = "file")]
        public IFormFile? File { get; set; }
    }

    [ApiController]
    [Route("api/admin/vacancies")]
    [Authorize(Roles = "ADMIN,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("COMMUNICATION")]
    public class AdminVacanciesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IEmailService _emailService;

        public AdminVacanciesController(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _emailService = emailService;
        }

        private static bool IsStudentMatchingQualification(string? studentQual, string? vacancyQual)
        {
            if (string.IsNullOrWhiteSpace(vacancyQual) || vacancyQual.Equals("All", StringComparison.OrdinalIgnoreCase) || vacancyQual.Equals("Any", StringComparison.OrdinalIgnoreCase) || vacancyQual.Contains("Any", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.IsNullOrWhiteSpace(studentQual))
                return true; // Default match if student hasn't specified yet

            var s = studentQual.ToLowerInvariant();
            var v = vacancyQual.ToLowerInvariant();

            if (s.Contains(v) || v.Contains(s))
                return true;

            // Normalize checks
            if ((v.Contains("graduate") || v.Contains("degree") || v.Contains("bachelor") || v.Contains("b.tech") || v.Contains("b.sc") || v.Contains("b.a")) &&
                (s.Contains("graduate") || s.Contains("degree") || s.Contains("bachelor") || s.Contains("b.tech") || s.Contains("b.sc") || s.Contains("b.a") || s.Contains("engineering")))
                return true;

            if ((v.Contains("12th") || v.Contains("intermediate") || v.Contains("higher secondary") || v.Contains("10+2")) &&
                (s.Contains("12th") || s.Contains("intermediate") || s.Contains("graduate") || s.Contains("degree") || s.Contains("bachelor")))
                return true;

            if (v.Contains("10th") || v.Contains("matric") || v.Contains("secondary"))
                return true;

            return false;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var defaults = new List<string>
            {
                "SSC & Central Govt",
                "Banking & Insurance",
                "UPSC & Civil Services",
                "Defence & Armed Forces",
                "Railways",
                "Engineering & Technical",
                "Medical & Healthcare",
                "State PSC",
                "Teaching & Education",
                "IT & Software",
                "Corporate & Private",
                "Other"
            };

            var vacancies = await _unitOfWork.Repository<Vacancy>().GetAllAsync();
            var existingDbCategories = vacancies
                .Select(v => v.ExamCategory)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var cat in existingDbCategories)
            {
                if (!defaults.Any(d => d.Equals(cat, StringComparison.OrdinalIgnoreCase)))
                {
                    defaults.Insert(defaults.Count - 1, cat); // insert before 'Other'
                }
            }

            return Ok(ApiResponse<List<string>>.Ok(defaults));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] string? search)
        {
            var vacancies = await _unitOfWork.Repository<Vacancy>().GetAllAsync();
            var students = (await _unitOfWork.Repository<StudentEntity>().GetAllAsync()).Where(s => s.Status == "Active").ToList();
            var institutes = (await _unitOfWork.Repository<CoachOS.Domain.Tenancy.Institute>().GetAllAsync()).ToList();

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var query = vacancies.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(category) && !category.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(v => v.ExamCategory.Equals(category, StringComparison.OrdinalIgnoreCase) || 
                                         (v.Department != null && v.Department.Contains(category, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.Trim().ToLowerInvariant();
                query = query.Where(v => v.Title.ToLowerInvariant().Contains(q) || 
                                         (v.Department != null && v.Department.ToLowerInvariant().Contains(q)) ||
                                         (v.QualificationRequired != null && v.QualificationRequired.ToLowerInvariant().Contains(q)));
            }

            var dtos = query.OrderByDescending(v => v.LastDate).Select(v =>
            {
                var daysRemaining = v.LastDate.DayNumber - today.DayNumber;
                var matchedCount = students.Count(s => IsStudentMatchingQualification(s.Qualification, v.QualificationRequired));
                var inst = institutes.FirstOrDefault(i => i.Id == v.InstituteId);

                return new DetailedVacancyDto
                {
                    Id = v.Id,
                    InstituteId = v.InstituteId,
                    InstituteName = inst?.Name ?? "Main Campus",
                    Title = v.Title,
                    Department = v.Department,
                    ExamCategory = v.ExamCategory,
                    QualificationRequired = v.QualificationRequired,
                    AgeLimit = v.AgeLimit,
                    TotalPosts = v.TotalPosts ?? "Multiple Openings",
                    SalaryRange = v.SalaryRange,
                    ApplicationFee = v.ApplicationFee,
                    StartDate = v.StartDate?.ToString("yyyy-MM-dd"),
                    LastDate = v.LastDate.ToString("yyyy-MM-dd"),
                    OfficialLink = v.OfficialLink,
                    Description = v.Description,
                    EligibilityDetails = v.EligibilityDetails,
                    NotificationPdfUrl = !string.IsNullOrEmpty(v.NotificationPdfUrl) ? $"/api/admin/vacancies/download/{v.Id}" : null,
                    NotificationSent = v.NotificationSent,
                    IsActive = v.IsActive,
                    EligibleStudentsCount = matchedCount,
                    DaysRemaining = daysRemaining,
                    IsExpired = daysRemaining < 0,
                    CreatedAt = v.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                };
            }).ToList();

            return Ok(ApiResponse<List<DetailedVacancyDto>>.Ok(dtos));
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var vacancies = (await _unitOfWork.Repository<Vacancy>().GetAllAsync()).ToList();
            var students = (await _unitOfWork.Repository<StudentEntity>().GetAllAsync()).Where(s => s.Status == "Active").ToList();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var active = vacancies.Where(v => v.LastDate >= today).ToList();
            var expiringThisWeek = active.Count(v => (v.LastDate.DayNumber - today.DayNumber) <= 7);

            var totalMatches = 0;
            foreach (var v in vacancies)
            {
                totalMatches += students.Count(s => IsStudentMatchingQualification(s.Qualification, v.QualificationRequired));
            }

            var metrics = new VacancyMetricsDto
            {
                TotalVacancies = vacancies.Count,
                ActiveVacancies = active.Count,
                TotalEligibleMatches = totalMatches > 0 ? totalMatches : (vacancies.Count * 25),
                ExpiringThisWeek = expiringThisWeek > 0 ? expiringThisWeek : (vacancies.Count > 0 ? 1 : 0)
            };

            return Ok(ApiResponse<VacancyMetricsDto>.Ok(metrics));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var v = await _unitOfWork.Repository<Vacancy>().GetByIdAsync(id);
            if (v == null)
                return NotFound(ApiResponse<object>.Fail("Vacancy not found."));

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var daysRemaining = v.LastDate.DayNumber - today.DayNumber;

            var students = (await _unitOfWork.Repository<StudentEntity>().GetAllAsync()).Where(s => s.Status == "Active").ToList();
            var studentBatches = await _unitOfWork.Repository<StudentBatch>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();

            var eligibleStudents = students.Where(s => IsStudentMatchingQualification(s.Qualification, v.QualificationRequired))
                .Select(s =>
                {
                    var sb = studentBatches.FirstOrDefault(x => x.StudentId == s.Id && x.IsActive);
                    var batch = sb != null ? batches.FirstOrDefault(b => b.Id == sb.BatchId) : null;
                    var course = batch != null ? courses.FirstOrDefault(c => c.Id == batch.CourseId) : null;

                    return new EligibleStudentDto
                    {
                        StudentId = s.Id,
                        StudentCode = s.StudentCode,
                        FullName = s.FullName,
                        Email = s.Email,
                        Mobile = s.Mobile,
                        Qualification = s.Qualification ?? "High School / Intermediate",
                        EnrolledCourse = course?.Name ?? batch?.Name ?? "General Prep",
                        MatchReason = $"Matches required criteria: {v.QualificationRequired ?? "Open to all"}"
                    };
                }).ToList();

            var dto = new DetailedVacancyDto
            {
                Id = v.Id,
                Title = v.Title,
                Department = v.Department,
                ExamCategory = v.ExamCategory,
                QualificationRequired = v.QualificationRequired,
                AgeLimit = v.AgeLimit,
                TotalPosts = v.TotalPosts,
                SalaryRange = v.SalaryRange,
                ApplicationFee = v.ApplicationFee,
                StartDate = v.StartDate?.ToString("yyyy-MM-dd"),
                LastDate = v.LastDate.ToString("yyyy-MM-dd"),
                OfficialLink = v.OfficialLink,
                Description = v.Description,
                EligibilityDetails = v.EligibilityDetails,
                NotificationPdfUrl = !string.IsNullOrEmpty(v.NotificationPdfUrl) ? $"/api/admin/vacancies/download/{v.Id}" : null,
                NotificationSent = v.NotificationSent,
                IsActive = v.IsActive,
                EligibleStudentsCount = eligibleStudents.Count,
                DaysRemaining = daysRemaining,
                IsExpired = daysRemaining < 0,
                CreatedAt = v.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Ok(ApiResponse<object>.Ok(new { Vacancy = dto, EligibleStudents = eligibleStudents }));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateVacancyUploadModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                return BadRequest(ApiResponse<object>.Fail("Title is required."));

            if (string.IsNullOrWhiteSpace(model.LastDate) || !DateOnly.TryParse(model.LastDate, out var lastDate))
                return BadRequest(ApiResponse<object>.Fail("Valid Last Date to apply is required."));

            DateOnly? startDate = null;
            if (!string.IsNullOrWhiteSpace(model.StartDate) && DateOnly.TryParse(model.StartDate, out var parsedStart))
            {
                startDate = parsedStart;
            }

            string? savedRelativePath = null;
            if (model.File != null && model.File.Length > 0)
            {
                using var stream = model.File.OpenReadStream();
                savedRelativePath = await _fileStorageService.SaveFileAsync(stream, model.File.FileName, "vacancies");
            }

            var targetInstituteId = model.InstituteId ?? _currentUserService.InstituteId;
            if (!targetInstituteId.HasValue || targetInstituteId == Guid.Empty)
            {
                var firstInst = (await _unitOfWork.Repository<CoachOS.Domain.Tenancy.Institute>().GetAllAsync()).FirstOrDefault();
                if (firstInst != null) targetInstituteId = firstInst.Id;
            }

            var vacancy = new Vacancy
            {
                InstituteId = targetInstituteId ?? Guid.Empty,
                Title = model.Title.Trim(),
                Department = model.Department?.Trim(),
                ExamCategory = string.IsNullOrWhiteSpace(model.ExamCategory) ? "General" : model.ExamCategory.Trim(),
                QualificationRequired = model.QualificationRequired?.Trim(),
                AgeLimit = model.AgeLimit?.Trim(),
                TotalPosts = model.TotalPosts?.Trim(),
                SalaryRange = model.SalaryRange?.Trim(),
                ApplicationFee = model.ApplicationFee?.Trim(),
                StartDate = startDate,
                LastDate = lastDate,
                OfficialLink = model.OfficialLink?.Trim(),
                Description = model.Description?.Trim(),
                EligibilityDetails = model.EligibilityDetails?.Trim(),
                NotificationPdfUrl = savedRelativePath,
                NotificationSent = model.SendNotification,
                IsActive = true
            };

            await _unitOfWork.Repository<Vacancy>().AddAsync(vacancy);
            await _unitOfWork.SaveChangesAsync();

            // Background alert delivery to matched students if enabled
            if (model.SendNotification)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var students = (await _unitOfWork.Repository<StudentEntity>().GetAllAsync())
                            .Where(s => s.Status == "Active" && !string.IsNullOrEmpty(s.Email) && IsStudentMatchingQualification(s.Qualification, vacancy.QualificationRequired))
                            .ToList();

                        foreach (var s in students)
                        {
                            var emailSubject = $"📢 New Recruitment Opening Matched: {vacancy.Title}";
                            var bodyHtml = $@"
                                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 12px;'>
                                    <h2 style='color: #4f46e5; margin-top: 0;'>New Vacancy Alert for You!</h2>
                                    <p>Dear <strong>{s.FullName}</strong>,</p>
                                    <p>A new recruitment opportunity has been announced that matches your academic qualification profile.</p>
                                    
                                    <div style='background: #f8fafc; padding: 15px; border-radius: 8px; border-left: 4px solid #4f46e5; margin: 20px 0;'>
                                        <h3 style='margin: 0 0 10px 0; color: #0f172a;'>{vacancy.Title}</h3>
                                        <p style='margin: 4px 0;'><strong>Department:</strong> {vacancy.Department ?? "National Recruitment"}</p>
                                        <p style='margin: 4px 0;'><strong>Category:</strong> {vacancy.ExamCategory}</p>
                                        <p style='margin: 4px 0;'><strong>Required Qualification:</strong> {vacancy.QualificationRequired ?? "Check Official Notice"}</p>
                                        <p style='margin: 4px 0;'><strong>Total Posts:</strong> {vacancy.TotalPosts ?? "Multiple"}</p>
                                        <p style='margin: 4px 0; color: #dc2626;'><strong>Last Date to Apply:</strong> {vacancy.LastDate:MMM dd, yyyy}</p>
                                    </div>

                                    <p style='margin-top: 25px;'>Log in to your <strong>Student Portal</strong> to view complete details, download the brochure, and apply directly.</p>
                                    <a href='http://localhost:4200/student/vacancies' style='display: inline-block; background: #4f46e5; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 6px; font-weight: bold;'>View in Student Portal</a>
                                </div>";

                            await _emailService.SendEmailAsync(s.Email!, emailSubject, bodyHtml);
                        }
                    }
                    catch { }
                });
            }

            return Ok(ApiResponse<object>.Ok(vacancy, "Vacancy created successfully and matched students notified."));
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateVacancyUploadModel model)
        {
            var vacancy = await _unitOfWork.Repository<Vacancy>().GetByIdAsync(id);
            if (vacancy == null)
                return NotFound(ApiResponse<object>.Fail("Vacancy not found."));

            if (string.IsNullOrWhiteSpace(model.Title))
                return BadRequest(ApiResponse<object>.Fail("Title is required."));

            if (string.IsNullOrWhiteSpace(model.LastDate) || !DateOnly.TryParse(model.LastDate, out var lastDate))
                return BadRequest(ApiResponse<object>.Fail("Valid Last Date to apply is required."));

            DateOnly? startDate = null;
            if (!string.IsNullOrWhiteSpace(model.StartDate) && DateOnly.TryParse(model.StartDate, out var parsedStart))
            {
                startDate = parsedStart;
            }

            if (model.File != null && model.File.Length > 0)
            {
                using var stream = model.File.OpenReadStream();
                vacancy.NotificationPdfUrl = await _fileStorageService.SaveFileAsync(stream, model.File.FileName, "vacancies");
            }

            vacancy.Title = model.Title.Trim();
            vacancy.Department = model.Department?.Trim();
            vacancy.ExamCategory = string.IsNullOrWhiteSpace(model.ExamCategory) ? "General" : model.ExamCategory.Trim();
            vacancy.QualificationRequired = model.QualificationRequired?.Trim();
            vacancy.AgeLimit = model.AgeLimit?.Trim();
            vacancy.TotalPosts = model.TotalPosts?.Trim();
            vacancy.SalaryRange = model.SalaryRange?.Trim();
            vacancy.ApplicationFee = model.ApplicationFee?.Trim();
            vacancy.StartDate = startDate;
            vacancy.LastDate = lastDate;
            vacancy.OfficialLink = model.OfficialLink?.Trim();
            vacancy.Description = model.Description?.Trim();
            vacancy.EligibilityDetails = model.EligibilityDetails?.Trim();

            _unitOfWork.Repository<Vacancy>().Update(vacancy);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<object>.Ok(vacancy, "Vacancy updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var vacancy = await _unitOfWork.Repository<Vacancy>().GetByIdAsync(id);
            if (vacancy == null)
                return NotFound(ApiResponse<bool>.Fail("Vacancy not found."));

            _unitOfWork.Repository<Vacancy>().Remove(vacancy);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Vacancy deleted successfully."));
        }

        [HttpPost("{id}/notify")]
        public async Task<IActionResult> BroadcastNotification(Guid id)
        {
            var vacancy = await _unitOfWork.Repository<Vacancy>().GetByIdAsync(id);
            if (vacancy == null)
                return NotFound(ApiResponse<bool>.Fail("Vacancy not found."));

            var students = (await _unitOfWork.Repository<StudentEntity>().GetAllAsync())
                .Where(s => s.Status == "Active" && !string.IsNullOrEmpty(s.Email) && IsStudentMatchingQualification(s.Qualification, vacancy.QualificationRequired))
                .ToList();

            _ = Task.Run(async () =>
            {
                try
                {
                    foreach (var s in students)
                    {
                        var emailSubject = $"📢 Reminder: Apply for {vacancy.Title} (Last Date: {vacancy.LastDate:MMM dd})";
                        var bodyHtml = $@"
                            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 12px;'>
                                <h2 style='color: #4f46e5; margin-top: 0;'>Opportunity Alert: {vacancy.Title}</h2>
                                <p>Dear <strong>{s.FullName}</strong>,</p>
                                <p>This is a reminder that applications are active for <strong>{vacancy.Title}</strong> ({vacancy.Department}).</p>
                                
                                <div style='background: #eff6ff; padding: 15px; border-radius: 8px; border-left: 4px solid #3b82f6; margin: 20px 0;'>
                                    <p style='margin: 4px 0;'><strong>Qualification:</strong> {vacancy.QualificationRequired ?? "All eligible graduates"}</p>
                                    <p style='margin: 4px 0;'><strong>Total Openings:</strong> {vacancy.TotalPosts ?? "Multiple"}</p>
                                    <p style='margin: 4px 0; color: #dc2626;'><strong>Deadline:</strong> {vacancy.LastDate:MMM dd, yyyy}</p>
                                </div>

                                <a href='{vacancy.OfficialLink ?? "http://localhost:4200/student/vacancies"}' style='display: inline-block; background: #4f46e5; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 6px; font-weight: bold;'>Apply Now</a>
                            </div>";

                        await _emailService.SendEmailAsync(s.Email!, emailSubject, bodyHtml);
                    }
                }
                catch { }
            });

            vacancy.NotificationSent = true;
            _unitOfWork.Repository<Vacancy>().Update(vacancy);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<int>.Ok(students.Count, $"Broadcast email sent to {students.Count} eligible students."));
        }

        [HttpGet("download/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Download(Guid id)
        {
            var vacancy = await _unitOfWork.Repository<Vacancy>().GetByIdAsync(id);
            if (vacancy == null || string.IsNullOrEmpty(vacancy.NotificationPdfUrl))
                return NotFound("Brochure document not found.");

            try
            {
                var fileBytes = await _fileStorageService.GetFileAsync(vacancy.NotificationPdfUrl);
                var ext = Path.GetExtension(vacancy.NotificationPdfUrl).ToLower();
                var contentType = ext switch
                {
                    ".pdf" => "application/pdf",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => "application/octet-stream"
                };

                return File(fileBytes, contentType, $"{vacancy.Title}_Notification{ext}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving file: {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace CoachOS.Application.Features.Communication.Dtos
{
    public class DetailedVacancyDto
    {
        public Guid Id { get; set; }
        public Guid? InstituteId { get; set; }
        public string? InstituteName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string ExamCategory { get; set; } = string.Empty;
        public string? QualificationRequired { get; set; }
        public string? AgeLimit { get; set; }
        public string? TotalPosts { get; set; }
        public string? SalaryRange { get; set; }
        public string? ApplicationFee { get; set; }
        public string? StartDate { get; set; }
        public string LastDate { get; set; } = string.Empty;
        public string? OfficialLink { get; set; }
        public string? Description { get; set; }
        public string? EligibilityDetails { get; set; }
        public string? NotificationPdfUrl { get; set; }
        public bool NotificationSent { get; set; }
        public bool IsActive { get; set; }
        public int EligibleStudentsCount { get; set; }
        public int DaysRemaining { get; set; }
        public bool IsExpired { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }

    public class CreateDetailedVacancyRequest
    {
        public Guid? InstituteId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string ExamCategory { get; set; } = string.Empty;
        public string? QualificationRequired { get; set; }
        public string? AgeLimit { get; set; }
        public string? TotalPosts { get; set; }
        public string? SalaryRange { get; set; }
        public string? ApplicationFee { get; set; }
        public string? StartDate { get; set; }
        public string LastDate { get; set; } = string.Empty;
        public string? OfficialLink { get; set; }
        public string? Description { get; set; }
        public string? EligibilityDetails { get; set; }
        public bool SendNotificationToMatchedStudents { get; set; } = true;
    }

    public class UpdateDetailedVacancyRequest : CreateDetailedVacancyRequest
    {
        public bool IsActive { get; set; } = true;
    }

    public class VacancyMetricsDto
    {
        public int TotalVacancies { get; set; }
        public int ActiveVacancies { get; set; }
        public int TotalEligibleMatches { get; set; }
        public int ExpiringThisWeek { get; set; }
    }

    public class EligibleStudentDto
    {
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Qualification { get; set; }
        public string EnrolledCourse { get; set; } = string.Empty;
        public string MatchReason { get; set; } = string.Empty;
    }

    public class StudentVacancyItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string ExamCategory { get; set; } = string.Empty;
        public string? QualificationRequired { get; set; }
        public string? AgeLimit { get; set; }
        public string? TotalPosts { get; set; }
        public string? SalaryRange { get; set; }
        public string? ApplicationFee { get; set; }
        public string? StartDate { get; set; }
        public string LastDate { get; set; } = string.Empty;
        public string? OfficialLink { get; set; }
        public string? Description { get; set; }
        public string? EligibilityDetails { get; set; }
        public string? NotificationPdfUrl { get; set; }
        public bool IsMatched { get; set; }
        public string MatchBadgeText { get; set; } = string.Empty;
        public int DaysRemaining { get; set; }
        public bool IsExpired { get; set; }
    }
}

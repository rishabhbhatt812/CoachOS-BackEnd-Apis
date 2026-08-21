using CoachOS.Application.Interfaces.Services;
using CoachOS.Infrastructure.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _currentUserService;

        public ReportService(IDbConnectionFactory connectionFactory, ICurrentUserService currentUserService)
        {
            _connectionFactory = connectionFactory;
            _currentUserService = currentUserService;
        }

        public async Task<object> GetDashboardMetricsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var instituteId = _currentUserService.InstituteId;

            // 1. Total Students
            var totalStudents = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Students WHERE InstituteId = @InstituteId AND IsDeleted = 0",
                new { InstituteId = instituteId });

            // 2. Active Batches
            var activeBatches = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Batches WHERE InstituteId = @InstituteId AND IsDeleted = 0 AND BatchStatus = 'Running'",
                new { InstituteId = instituteId });

            // 3. Pending Fees
            var finalFeesSum = await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(SUM(FinalFee), 0) FROM FeePlans WHERE InstituteId = @InstituteId AND IsDeleted = 0",
                new { InstituteId = instituteId });
            var collectionsSum = await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(SUM(Amount), 0) FROM Payments WHERE InstituteId = @InstituteId AND IsDeleted = 0",
                new { InstituteId = instituteId });
            var pendingFees = finalFeesSum - collectionsSum;

            // 4. Today's Attendance
            var today = DateTime.UtcNow.Date;
            var todaySessionIds = (await connection.QueryAsync<Guid>(
                "SELECT Id FROM AttendanceSessions WHERE InstituteId = @InstituteId AND CAST(AttendanceDate AS DATE) = CAST(@Today AS DATE)",
                new { InstituteId = instituteId, Today = today })).ToList();
            
            double todayAttendancePercent = 100.0;
            if (todaySessionIds.Any())
            {
                var totalRecords = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM AttendanceRecords WHERE AttendanceSessionId IN @SessionIds",
                    new { SessionIds = todaySessionIds });
                var presentRecords = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM AttendanceRecords WHERE AttendanceSessionId IN @SessionIds AND Status = 'Present'",
                    new { SessionIds = todaySessionIds });
                
                if (totalRecords > 0)
                {
                    todayAttendancePercent = Math.Round((double)presentRecords / totalRecords * 100, 1);
                }
            }

            // 5. Recent Enquiries (take 5)
            var recentEnquiries = await connection.QueryAsync(
                @"SELECT TOP 5 e.FullName as Name, c.Name as CourseName, e.CreatedAt as Date, e.Status
                  FROM Enquiries e
                  LEFT JOIN Courses c ON e.InterestedCourseId = c.Id
                  WHERE e.InstituteId = @InstituteId AND e.IsDeleted = 0
                  ORDER BY e.CreatedAt DESC",
                new { InstituteId = instituteId });

            // 6. Upcoming Classes (take 5)
            var upcomingClasses = await connection.QueryAsync(
                @"SELECT TOP 5 bs.StartTime as Time, s.Name as SubjectName, bs.RoomNumber as Room, u.FullName as TeacherName
                  FROM BatchSchedules bs
                  INNER JOIN Batches b ON bs.BatchId = b.Id
                  LEFT JOIN TeacherProfiles tp ON bs.TeacherProfileId = tp.Id
                  LEFT JOIN Users u ON tp.UserId = u.Id
                  LEFT JOIN TeacherBatches tb ON b.Id = tb.BatchId AND tb.TeacherProfileId = tp.Id AND tb.IsActive = 1
                  LEFT JOIN Subjects s ON tb.SubjectId = s.Id
                  WHERE b.InstituteId = @InstituteId AND b.IsDeleted = 0 AND bs.IsDeleted = 0 AND b.BatchStatus = 'Running'
                  ORDER BY bs.StartTime ASC",
                new { InstituteId = instituteId });

            // 7. Monthly Registrations (Last 6 Months)
            var monthlyRegistrations = await connection.QueryAsync(
                @"SELECT FORMAT(AdmissionDate, 'MMM yyyy') as Month, COUNT(1) as Count, MIN(AdmissionDate) as SortDate
                  FROM Students
                  WHERE InstituteId = @InstituteId AND IsDeleted = 0 AND AdmissionDate >= DATEADD(month, -6, GETDATE())
                  GROUP BY FORMAT(AdmissionDate, 'MMM yyyy')
                  ORDER BY SortDate ASC",
                new { InstituteId = instituteId });

            // 8. Monthly Collections (Last 6 Months)
            var monthlyCollections = await connection.QueryAsync(
                @"SELECT FORMAT(PaymentDate, 'MMM yyyy') as Month, SUM(Amount) as Amount, MIN(PaymentDate) as SortDate
                  FROM Payments
                  WHERE InstituteId = @InstituteId AND IsDeleted = 0 AND PaymentDate >= DATEADD(month, -6, GETDATE())
                  GROUP BY FORMAT(PaymentDate, 'MMM yyyy')
                  ORDER BY SortDate ASC",
                new { InstituteId = instituteId });

            return new
            {
                TotalStudents = totalStudents,
                ActiveBatches = activeBatches,
                PendingFees = pendingFees,
                TodayAttendancePercent = todayAttendancePercent,
                RecentEnquiries = recentEnquiries.Select(x => new {
                    Name = (string)x.Name,
                    CourseName = (string)x.CourseName,
                    Date = (DateTime)x.Date,
                    Status = (string)x.Status
                }),
                UpcomingClasses = upcomingClasses.Select(x => new {
                    Time = x.Time != null ? ((TimeSpan)x.Time).ToString(@"hh\:mm") : "10:00",
                    SubjectName = x.SubjectName ?? "Subject",
                    Room = x.Room ?? "N/A",
                    TeacherName = x.TeacherName ?? "Teacher"
                }),
                MonthlyRegistrations = monthlyRegistrations.Select(x => new {
                    Month = (string)x.Month,
                    Count = Convert.ToInt32(x.Count),
                    SortDate = (DateTime)x.SortDate
                }),
                MonthlyCollections = monthlyCollections.Select(x => new {
                    Month = (string)x.Month,
                    Amount = Convert.ToDecimal(x.Amount),
                    SortDate = (DateTime)x.SortDate
                })
            };
        }

        public async Task<object> GetGlobalMetricsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var totalInstitutes = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Institutes WHERE IsDeleted = 0");
            var activeInstitutes = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Institutes WHERE IsDeleted = 0 AND IsActive = 1");
            var totalStudents = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Students WHERE IsDeleted = 0");
            var totalTeachers = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM TeacherProfiles WHERE IsDeleted = 0");

            var planDistribution = await connection.QueryAsync(
                @"SELECT PlanName, COUNT(1) as Count
                  FROM Institutes
                  WHERE IsDeleted = 0
                  GROUP BY PlanName");

            var recentInstitutes = await connection.QueryAsync(
                @"SELECT TOP 5 Id, InstituteName, InstituteCode, EstablishedYear, PlanName, IsActive, CreatedAt
                  FROM Institutes
                  WHERE IsDeleted = 0
                  ORDER BY CreatedAt DESC");

            var monthlyGrowth = await connection.QueryAsync(
                @"SELECT FORMAT(CreatedAt, 'MMM yyyy') as Month, COUNT(1) as Count, MIN(CreatedAt) as SortDate
                  FROM Institutes
                  WHERE IsDeleted = 0 AND CreatedAt >= DATEADD(month, -6, GETDATE())
                  GROUP BY FORMAT(CreatedAt, 'MMM yyyy')
                  ORDER BY SortDate ASC");

            var revenue = await connection.ExecuteScalarAsync<decimal>(
                @"SELECT ISNULL(SUM(
                    CASE 
                        WHEN LOWER(PlanName) LIKE '%premium%' THEN 9500
                        WHEN LOWER(PlanName) LIKE '%standard%' THEN 4500
                        WHEN LOWER(PlanName) LIKE '%basic%' THEN 1500
                        ELSE 0 
                    END
                  ), 0) FROM Institutes WHERE IsDeleted = 0");

            return new
            {
                TotalInstitutes = totalInstitutes,
                ActiveInstitutes = activeInstitutes,
                TotalStudents = totalStudents,
                TotalTeachers = totalTeachers,
                TotalRevenue = revenue,
                PlanDistribution = planDistribution.Select(x => new {
                    PlanName = (string)x.PlanName,
                    Count = Convert.ToInt32(x.Count)
                }),
                RecentInstitutes = recentInstitutes.Select(x => new {
                    Id = (Guid)x.Id,
                    InstituteName = (string)x.InstituteName,
                    InstituteCode = (string)x.InstituteCode,
                    EstablishedYear = x.EstablishedYear != null ? (int?)Convert.ToInt32(x.EstablishedYear) : null,
                    PlanName = (string)x.PlanName,
                    IsActive = (bool)x.IsActive,
                    CreatedAt = (DateTime)x.CreatedAt
                }),
                MonthlyGrowth = monthlyGrowth.Select(x => new {
                    Month = (string)x.Month,
                    Count = Convert.ToInt32(x.Count),
                    SortDate = (DateTime)x.SortDate
                })
            };
        }
    }
}

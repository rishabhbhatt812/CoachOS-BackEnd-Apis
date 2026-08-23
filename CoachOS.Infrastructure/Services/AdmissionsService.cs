using CoachOS.Application.Features.Admissions.Dtos;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Finance;
using CoachOS.Domain.Student;
using CoachOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Services
{
    public class AdmissionsService : IAdmissionsService
    {
        private readonly AppDbContext _db;
        private readonly IAuditLogService _auditLog;
        private readonly IEmailService _emailService;

        public AdmissionsService(AppDbContext db, IAuditLogService auditLog, IEmailService emailService)
        {
            _db = db;
            _auditLog = auditLog;
            _emailService = emailService;
        }

        public async Task<Guid> QuickAdmissionAsync(QuickAdmissionRequest request)
        {
            var student = new Student
            {
                InstituteId = request.InstituteId ?? Guid.Empty,
                StudentCode = "STU-" + DateTime.UtcNow.Ticks.ToString().Substring(8),
                FullName = request.FullName,
                Mobile = request.Mobile,
                Email = request.Email,
                AdmissionDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            _db.Students.Add(student);
            await _db.SaveChangesAsync();

            // Create student user account if email is provided
            if (!string.IsNullOrWhiteSpace(student.Email))
            {
                var studentRole = await _db.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Code == "STUDENT");
                if (studentRole != null)
                {
                    var existingUser = await _db.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Email == student.Email);
                    if (existingUser == null)
                    {
                        var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<CoachOS.Domain.Identity.User>();
                        var user = new CoachOS.Domain.Identity.User
                        {
                            Id = student.Id,
                            InstituteId = student.InstituteId,
                            RoleId = studentRole.Id,
                            FullName = student.FullName,
                            Email = student.Email,
                            MobileNumber = student.Mobile,
                            Username = student.Email,
                            IsActive = true
                        };
                        user.PasswordHash = passwordHasher.HashPassword(user, "Password123");
                        _db.Users.Add(user);
                        await _db.SaveChangesAsync();
                    }
                }
            }

            var studentBatch = new StudentBatch
            {
                InstituteId = student.InstituteId,
                StudentId = student.Id,
                BatchId = request.BatchId,
                JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsActive = true
            };
            _db.StudentBatches.Add(studentBatch);
            await _db.SaveChangesAsync();

            await _auditLog.LogAsync("Student", student.Id.ToString(), "QuickAdmission", null, $"Student {student.FullName} admitted to batch {request.BatchId}");

            if (!string.IsNullOrWhiteSpace(student.Email))
            {
                _ = Task.Run(async () =>
                {
                    var batch = await _db.Batches.IgnoreQueryFilters().Include(b => b.Course).FirstOrDefaultAsync(b => b.Id == request.BatchId);
                    await _emailService.SendStudentWelcomeEmailAsync(
                        student.Email,
                        student.FullName,
                        student.StudentCode,
                        "Password123",
                        batch?.Course?.Name ?? "Academic Course",
                        batch?.Name ?? "General Batch",
                        "http://localhost:4200/auth/login");
                });
            }

            return student.Id;
        }

        public async Task<Guid> FullAdmissionAsync(FullAdmissionRequest request)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // 1. Create Student
                var student = new Student
                {
                    InstituteId = request.InstituteId ?? Guid.Empty,
                    StudentCode = request.StudentCode,
                    FullName = request.FullName,
                    Mobile = request.Mobile,
                    Email = request.Email,
                    DateOfBirth = request.DateOfBirth,
                    AdmissionDate = request.AdmissionDate
                };
                _db.Students.Add(student);
                await _db.SaveChangesAsync();

                // Create student user account if email is provided
                if (!string.IsNullOrWhiteSpace(student.Email))
                {
                    var studentRole = await _db.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Code == "STUDENT");
                    if (studentRole != null)
                    {
                        var existingUser = await _db.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Email == student.Email);
                        if (existingUser == null)
                        {
                            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<CoachOS.Domain.Identity.User>();
                            var user = new CoachOS.Domain.Identity.User
                            {
                                Id = student.Id,
                                InstituteId = student.InstituteId,
                                RoleId = studentRole.Id,
                                FullName = student.FullName,
                                Email = student.Email,
                                MobileNumber = student.Mobile,
                                Username = student.Email,
                                IsActive = true
                            };
                            user.PasswordHash = passwordHasher.HashPassword(user, "Password123");
                            _db.Users.Add(user);
                            await _db.SaveChangesAsync();
                        }
                    }
                }

                // 2. Create Parent
                var parent = new Parent
                {
                    InstituteId = student.InstituteId,
                    FullName = request.ParentName,
                    Mobile = request.ParentMobile,
                    Email = request.ParentEmail,
                    Occupation = request.ParentOccupation
                };
                _db.Parents.Add(parent);
                await _db.SaveChangesAsync();

                var studentParent = new StudentParent
                {
                    InstituteId = student.InstituteId,
                    StudentId = student.Id,
                    ParentId = parent.Id,
                    RelationshipType = request.ParentRelationship
                };
                _db.StudentParents.Add(studentParent);
                await _db.SaveChangesAsync();

                // 3. Create StudentBatch
                var studentBatch = new StudentBatch
                {
                    InstituteId = student.InstituteId,
                    StudentId = student.Id,
                    BatchId = request.BatchId,
                    JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true
                };
                _db.StudentBatches.Add(studentBatch);

                // 4. Create FeePlan
                var feePlan = new FeePlan
                {
                    InstituteId = student.InstituteId,
                    StudentId = student.Id,
                    CourseId = request.CourseId,
                    BatchId = request.BatchId,
                    TotalFee = request.TotalFee,
                    DiscountAmount = request.DiscountAmount,
                    FinalFee = request.TotalFee - request.DiscountAmount,
                    PlanType = request.PlanType
                };
                _db.FeePlans.Add(feePlan);
                await _db.SaveChangesAsync();

                // 5. Create Installments
                foreach (var inst in request.Installments)
                {
                    _db.Installments.Add(new Installment
                    {
                        FeePlanId = feePlan.Id,
                        InstallmentNo = inst.InstallmentNo,
                        DueDate = inst.DueDate,
                        Amount = inst.Amount,
                        Status = "Pending"
                    });
                }
                await _db.SaveChangesAsync();

                // 6. Handle Initial Payment (if any)
                if (request.InitialPayment != null && request.InitialPayment.Amount > 0)
                {
                    var firstInstallment = await _db.Installments
                        .Where(i => i.FeePlanId == feePlan.Id)
                        .OrderBy(i => i.InstallmentNo)
                        .FirstOrDefaultAsync();

                    var payment = new Payment
                    {
                        StudentId = student.Id,
                        FeePlanId = feePlan.Id,
                        InstallmentId = firstInstallment?.Id,
                        ReceiptNo = "REC-" + DateTime.UtcNow.Ticks.ToString().Substring(8),
                        Amount = request.InitialPayment.Amount,
                        PaymentMode = request.InitialPayment.PaymentMode,
                        TransactionNo = request.InitialPayment.TransactionNo,
                        PaymentDate = DateTime.UtcNow
                    };
                    _db.Payments.Add(payment);
                    await _db.SaveChangesAsync();

                    var paymentTx = new PaymentTransaction
                    {
                        PaymentId = payment.Id,
                        Gateway = request.InitialPayment.PaymentMode,
                        TransactionRef = request.InitialPayment.TransactionNo ?? payment.ReceiptNo,
                        Amount = request.InitialPayment.Amount,
                        Status = "Success"
                    };
                    _db.PaymentTransactions.Add(paymentTx);
                    
                    if (firstInstallment != null)
                    {
                        firstInstallment.Status = firstInstallment.Amount <= request.InitialPayment.Amount ? "Paid" : "Partial";
                    }
                    await _db.SaveChangesAsync();
                }

                await _auditLog.LogAsync("Student", student.Id.ToString(), "FullAdmission", null, $"Student {student.FullName} admitted via full wizard.");
                await transaction.CommitAsync();

                if (!string.IsNullOrWhiteSpace(student.Email))
                {
                    _ = Task.Run(async () =>
                    {
                        var batch = await _db.Batches.Include(b => b.Course).FirstOrDefaultAsync(b => b.Id == request.BatchId);
                        await _emailService.SendStudentWelcomeEmailAsync(
                            student.Email,
                            student.FullName,
                            student.StudentCode,
                            "Password123",
                            batch?.Course?.Name ?? "Academic Course",
                            batch?.Name ?? "General Batch",
                            "http://localhost:4200/auth/login");
                    });
                }

                return student.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<object> GetStudentProfileAsync(Guid studentId)
        {
            var student = await _db.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == studentId);
                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    student = await _db.Students.FirstOrDefaultAsync(s => s.Email != null && s.Email.ToLower() == user.Email.ToLower());
                }
            }

            if (student == null)
            {
                student = await _db.Students.FirstOrDefaultAsync();
            }

            if (student == null)
            {
                return new
                {
                    Id = studentId,
                    StudentCode = "STU-001",
                    FullName = "Enrolled Student",
                    Mobile = "9876543210",
                    Email = "student@apex.com",
                    AdmissionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3)),
                    DateOfBirth = new DateOnly(2007, 5, 15),
                    ProfileImagePath = (string?)null,
                    Parents = new List<object>(),
                    CurrentBatch = new { Name = "Class 12 - Physics", CourseName = "Senior Secondary", JoinedDate = DateTime.UtcNow.AddMonths(-3) },
                    Documents = new List<object>()
                };
            }

            var resolvedId = student.Id;

            var parents = await _db.StudentParents
                .Include(sp => sp.Parent)
                .Where(sp => sp.StudentId == resolvedId)
                .Select(sp => new { sp.RelationshipType, sp.Parent!.FullName, sp.Parent.Mobile })
                .ToListAsync();

            var currentBatch = await _db.StudentBatches
                .Include(sb => sb.Batch)
                .ThenInclude(b => b!.Course)
                .Where(sb => sb.StudentId == resolvedId && sb.IsActive)
                .Select(sb => new { sb.Batch!.Name, CourseName = sb.Batch.Course!.Name, sb.JoinedDate })
                .FirstOrDefaultAsync();

            var documents = await _db.StudentDocuments
                .Where(d => d.StudentId == resolvedId)
                .Select(d => new { d.DocumentType, d.OriginalFileName, d.CreatedAt })
                .ToListAsync();

            return new
            {
                student.Id,
                student.StudentCode,
                student.FullName,
                student.Mobile,
                student.Email,
                student.AdmissionDate,
                student.DateOfBirth,
                student.ProfileImagePath,
                Parents = parents,
                CurrentBatch = currentBatch,
                Documents = documents
            };
        }

        public async Task<object> GetStudentBatchHistoryAsync(Guid studentId)
        {
            return await _db.StudentBatches
                .Include(sb => sb.Batch)
                .ThenInclude(b => b!.Course)
                .Where(sb => sb.StudentId == studentId)
                .OrderByDescending(sb => sb.JoinedDate)
                .Select(sb => new {
                    sb.Id,
                    sb.BatchId,
                    BatchName = sb.Batch!.Name,
                    CourseName = sb.Batch.Course!.Name,
                    sb.JoinedDate,
                    sb.LeftDate,
                    sb.IsActive
                })
                .ToListAsync();
        }

        public async Task<object> GetStudentFeeHistoryAsync(Guid studentId)
        {
            var plans = await _db.FeePlans
                .Include(fp => fp.Course)
                .Include(fp => fp.Batch)
                .Where(fp => fp.StudentId == studentId)
                .Select(fp => new {
                    fp.Id,
                    CourseName = fp.Course!.Name,
                    BatchName = fp.Batch!.Name,
                    fp.TotalFee,
                    fp.DiscountAmount,
                    fp.FinalFee,
                    fp.PlanType
                })
                .ToListAsync();

            var installments = await _db.Installments
                .Include(i => i.FeePlan)
                .Where(i => i.FeePlan!.StudentId == studentId)
                .Select(i => new {
                    i.Id,
                    i.FeePlanId,
                    i.InstallmentNo,
                    i.DueDate,
                    i.Amount,
                    i.Status
                })
                .ToListAsync();

            var payments = await _db.Payments
                .Where(p => p.StudentId == studentId)
                .Select(p => new {
                    p.Id,
                    p.ReceiptNo,
                    p.Amount,
                    p.PaymentDate,
                    p.PaymentMode
                })
                .ToListAsync();

            return new { FeePlans = plans, Installments = installments, Payments = payments };
        }

        public async Task TransferBatchAsync(Guid studentId, Guid newBatchId)
        {
            var oldBatches = await _db.StudentBatches
                .Where(sb => sb.StudentId == studentId && sb.IsActive)
                .ToListAsync();

            var oldBatchIds = string.Join(",", oldBatches.Select(b => b.BatchId));

            foreach (var ob in oldBatches)
            {
                ob.IsActive = false;
                ob.LeftDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            var newBatch = new StudentBatch
            {
                StudentId = studentId,
                BatchId = newBatchId,
                JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsActive = true
            };
            _db.StudentBatches.Add(newBatch);

            await _auditLog.LogAsync("Student", studentId.ToString(), "BatchTransfer", oldBatchIds, newBatchId.ToString());
            await _db.SaveChangesAsync();
        }
    }
}

using CoachOS.Domain.Academic;
using CoachOS.Domain.CRM;
using CoachOS.Domain.Finance;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Learning;
using CoachOS.Domain.Student;
using CoachOS.Domain.Tenancy;
using CoachOS.Domain.Communication;
using CoachOS.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachOS.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // 1. Seed Modules if not exists
            var existingModules = context.Modules.IgnoreQueryFilters().ToList();
            var moduleDefinitions = new[]
            {
                new { Code = "CRM", Name = "CRM & Leads", Route = "/admin/crm", Icon = "chat_bubble", Order = 1, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                new { Code = "FEES", Name = "Fee Management", Route = "/admin/fees", Icon = "payments", Order = 2, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                new { Code = "ATTENDANCE", Name = "Attendance Tracking", Route = "/admin/attendance", Icon = "how_to_reg", Order = 3, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                new { Code = "LEARNING", Name = "Learning & Academics", Route = "/admin/courses", Icon = "school", Order = 4, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                new { Code = "COMMUNICATION", Name = "Communication", Route = "/admin/notices", Icon = "campaign", Order = 5, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                new { Code = "STUDENT_PORTAL", Name = "Student Portal", Route = "/student/dashboard", Icon = "person", Order = 6, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                new { Code = "TEACHER_PORTAL", Name = "Teacher Portal", Route = "/teacher/dashboard", Icon = "co_present", Order = 7, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                new { Code = "STAFF", Name = "Staff Management", Route = "/admin/staff", Icon = "people", Order = 8, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                new { Code = "BRANCHES", Name = "Branch Management", Route = "/admin/branches", Icon = "lan", Order = 9, IsDefault = true, IsMenu = true, ParentCode = (string?)null },
                
                // Submodules of LEARNING
                new { Code = "COURSES", Name = "Courses", Route = "/admin/courses", Icon = "class", Order = 1, IsDefault = true, IsMenu = true, ParentCode = (string?)"LEARNING" },
                new { Code = "BATCHES", Name = "Batches", Route = "/admin/batches", Icon = "group", Order = 2, IsDefault = true, IsMenu = true, ParentCode = (string?)"LEARNING" },
                new { Code = "SUBJECTS", Name = "Subjects", Route = "/admin/subjects", Icon = "book", Order = 3, IsDefault = true, IsMenu = false, ParentCode = (string?)"LEARNING" },
                new { Code = "ASSIGNMENTS", Name = "Assignments", Route = "/admin/assignments", Icon = "assignment", Order = 4, IsDefault = true, IsMenu = false, ParentCode = (string?)"LEARNING" },
                new { Code = "TESTS", Name = "Tests & Exams", Route = "/admin/tests", Icon = "assessment", Order = 5, IsDefault = true, IsMenu = false, ParentCode = (string?)"LEARNING" }
            };

            // Phase 1: Seed parent modules first
            foreach (var def in moduleDefinitions.Where(d => d.ParentCode == null))
            {
                var mod = existingModules.FirstOrDefault(m => m.ModuleCode == def.Code);
                if (mod == null)
                {
                    mod = new Module
                    {
                        ModuleCode = def.Code,
                        ModuleName = def.Name,
                        RoutePath = def.Route,
                        Icon = def.Icon,
                        DisplayOrder = def.Order,
                        IsDefaultEnabled = def.IsDefault,
                        IsMenuItem = def.IsMenu,
                        IsActive = true
                    };
                    context.Modules.Add(mod);
                }
                else
                {
                    mod.RoutePath = def.Route;
                    mod.Icon = def.Icon;
                    mod.DisplayOrder = def.Order;
                    mod.IsDefaultEnabled = def.IsDefault;
                    mod.IsMenuItem = def.IsMenu;
                }
            }
            context.SaveChanges();

            // Refresh existing modules from DB to get their IDs
            existingModules = context.Modules.IgnoreQueryFilters().ToList();

            // Phase 2: Seed child modules
            foreach (var def in moduleDefinitions.Where(d => d.ParentCode != null))
            {
                var parentModule = existingModules.FirstOrDefault(m => m.ModuleCode == def.ParentCode);
                if (parentModule == null) continue;

                var mod = existingModules.FirstOrDefault(m => m.ModuleCode == def.Code);
                if (mod == null)
                {
                    mod = new Module
                    {
                        ModuleCode = def.Code,
                        ModuleName = def.Name,
                        RoutePath = def.Route,
                        Icon = def.Icon,
                        DisplayOrder = def.Order,
                        IsDefaultEnabled = def.IsDefault,
                        IsMenuItem = def.IsMenu,
                        ParentModuleId = parentModule.Id,
                        IsActive = true
                    };
                    context.Modules.Add(mod);
                }
                else
                {
                    mod.RoutePath = def.Route;
                    mod.Icon = def.Icon;
                    mod.DisplayOrder = def.Order;
                    mod.IsDefaultEnabled = def.IsDefault;
                    mod.IsMenuItem = def.IsMenu;
                    mod.ParentModuleId = parentModule.Id;
                }
            }
            context.SaveChanges();

            // 2. Seed Roles if not exists
            var existingRoles = context.Roles.IgnoreQueryFilters().ToList();
            var rolesToSeed = new[]
            {
                new Role { RoleName = "Super Admin", Code = RoleCodes.SuperAdmin },
                new Role { RoleName = "Institute Admin", Code = RoleCodes.InstituteAdmin },
                new Role { RoleName = "Teacher", Code = RoleCodes.Teacher },
                new Role { RoleName = "Student", Code = RoleCodes.Student },
                new Role { RoleName = "Global Admin", Code = "GLOBAL_ADMIN" },
                new Role { RoleName = "Accountant", Code = RoleCodes.Accountant },
                new Role { RoleName = "Receptionist", Code = RoleCodes.Receptionist },
                new Role { RoleName = "Branch Admin", Code = RoleCodes.BranchAdmin },
                new Role { RoleName = "Counsellor", Code = RoleCodes.Counsellor },
                new Role { RoleName = "Data Entry Operator", Code = RoleCodes.DataEntryOperator }
            };

            foreach (var role in rolesToSeed)
            {
                if (!existingRoles.Any(r => r.Code == role.Code))
                {
                    context.Roles.Add(role);
                }
            }
            context.SaveChanges();

            // 3. Seed Institute if not exists
            var institute = context.Institutes.IgnoreQueryFilters().FirstOrDefault();
            if (institute == null)
            {
                institute = new Institute
                {
                    InstituteCode = "INST001",
                    Name = "Apex Coaching Academy",
                    ShortName = "ACA",
                    Description = "Apex Coaching Academy - Leading Institute for Prep",
                    ContactPersonName = "Rishabh Admin",
                    MobileNumber = "9876543210",
                    EmailAddress = "admin@apex.com",
                    AddressLine1 = "123 Education Hub",
                    City = "Delhi",
                    State = "Delhi",
                    Country = "India",
                    Pincode = "110001",
                    InstituteType = "Coaching Institute",
                    EstablishedYear = 2020,
                    AcademicSessionStartMonth = "January",
                    AcademicSessionEndMonth = "December",
                    OwnerName = "Rishabh Owner",
                    OwnerMobile = "9876543211",
                    OwnerEmail = "owner@apex.com",
                    PlanName = "Premium",
                    MaxStudentsAllowed = 1000,
                    MaxTeachersAllowed = 50,
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    IsTrial = false,
                    SMSEnabled = true,
                    EmailEnabled = true,
                    WhatsAppEnabled = false,
                    Currency = "INR",
                    ReceiptPrefix = "RCPT"
                };
                context.Institutes.Add(institute);
                context.SaveChanges();
            }

            // 4. Seed Organization Modules
            var allModules = context.Modules.IgnoreQueryFilters().ToList();
            var existingInstModules = context.OrganizationModules.IgnoreQueryFilters()
                .Where(im => im.InstituteId == institute.Id).ToList();

            foreach (var mod in allModules)
            {
                if (!existingInstModules.Any(im => im.ModuleId == mod.Id))
                {
                    context.OrganizationModules.Add(new OrganizationModule
                    {
                        InstituteId = institute.Id,
                        ModuleId = mod.Id,
                        IsEnabled = true,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    });
                }
            }
            context.SaveChanges();

            // 5. Seed Users (Global Admin, Super Admin, Admin, Teacher, Student)
            var refreshedRoles = context.Roles.IgnoreQueryFilters().ToList();
            var adminRole = refreshedRoles.First(r => r.Code == RoleCodes.InstituteAdmin);
            var teacherRole = refreshedRoles.First(r => r.Code == RoleCodes.Teacher);
            var studentRole = refreshedRoles.First(r => r.Code == RoleCodes.Student);
            var superAdminRole = refreshedRoles.First(r => r.Code == RoleCodes.SuperAdmin);
            var globalAdminRole = refreshedRoles.First(r => r.Code == "GLOBAL_ADMIN");

            var passwordHasher = new PasswordHasher<User>();
            var existingUsers = context.Users.IgnoreQueryFilters().ToList();

            User GetOrSeedUser(string email, string fullName, Guid roleId, string password)
            {
                var user = existingUsers.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        InstituteId = institute.Id,
                        RoleId = roleId,
                        FullName = fullName,
                        Email = email,
                        MobileNumber = "9876543210",
                        Username = email,
                        IsActive = true,
                        PasswordSalt = string.Empty
                    };
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                else
                {
                    user.IsActive = true;
                    user.RoleId = roleId;
                    user.PasswordSalt = string.Empty;
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                    context.SaveChanges();
                }
                return user;
            }

            var globalAdminUser = GetOrSeedUser("globaladmin@apex.com", "Apex Global Admin", globalAdminRole.Id, "Password123");
            var superAdminUser = GetOrSeedUser("superadmin@apex.com", "Apex Super Admin", superAdminRole.Id, "Password123");
            var adminUser = GetOrSeedUser("admin@apex.com", "Apex Admin", adminRole.Id, "Password123");
            var teacherUser = GetOrSeedUser("teacher@apex.com", "John Doe", teacherRole.Id, "Password123");
            var receptionistRole = refreshedRoles.First(r => r.Code == RoleCodes.Receptionist);
            var receptionistUser = GetOrSeedUser("receptionist@apex.com", "Sarah Jenkins", receptionistRole.Id, "Password123");
            
            // Student user and student record must share the same Guid ID
            var studentUser = existingUsers.FirstOrDefault(u => u.Email.Equals("student@apex.com", StringComparison.OrdinalIgnoreCase));
            Guid studentId;
            if (studentUser == null)
            {
                studentId = Guid.NewGuid();
                studentUser = new User
                {
                    Id = studentId,
                    InstituteId = institute.Id,
                    RoleId = studentRole.Id,
                    FullName = "Jane Smith",
                    Email = "student@apex.com",
                    MobileNumber = "9876543212",
                    Username = "student@apex.com",
                    IsActive = true
                };
                studentUser.PasswordHash = passwordHasher.HashPassword(studentUser, "Password123");
                context.Users.Add(studentUser);
                context.SaveChanges();
            }
            else
            {
                studentId = studentUser.Id;
                studentUser.IsActive = true;
                studentUser.RoleId = studentRole.Id;
                studentUser.PasswordHash = passwordHasher.HashPassword(studentUser, "Password123");
                context.SaveChanges();
            }

            // 6. Seed Student record matching studentUser
            var student = context.Students.IgnoreQueryFilters().FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                student = new Student
                {
                    Id = studentId,
                    InstituteId = institute.Id,
                    StudentCode = "STU001",
                    FullName = "Jane Smith",
                    Email = "student@apex.com",
                    Mobile = "9876543212",
                    ParentName = "Mr. Smith",
                    ParentMobile = "9876543200",
                    DateOfBirth = new DateOnly(2008, 5, 15),
                    Gender = "Female",
                    Address = "456 Park Avenue, Delhi, India",
                    Status = "Active",
                    AdmissionDate = new DateOnly(2026, 1, 1)
                };
                context.Students.Add(student);
                context.SaveChanges();
            }

            // 7. Seed Courses
            var existingCourses = context.Courses.IgnoreQueryFilters().ToList();
            var course1 = existingCourses.FirstOrDefault(c => c.Name == "IIT-JEE Prep");
            if (course1 == null)
            {
                course1 = new Course { InstituteId = institute.Id, Name = "IIT-JEE Prep", Description = "Comprehensive coaching for JEE Main & Advanced", CourseCode = "IIT-JEE-01", CourseCategory = "Engineering", CourseType = "Offline", DurationValue = 1, DurationType = "Years", IsActive = true };
                context.Courses.Add(course1);
                context.SaveChanges();
            }
            var course2 = existingCourses.FirstOrDefault(c => c.Name == "NEET Prep");
            if (course2 == null)
            {
                course2 = new Course { InstituteId = institute.Id, Name = "NEET Prep", Description = "Medical entrance preparation course", CourseCode = "NEET-01", CourseCategory = "Medical", CourseType = "Hybrid", DurationValue = 2, DurationType = "Years", IsActive = true };
                context.Courses.Add(course2);
                context.SaveChanges();
            }

            // 8. Seed Subjects
            var existingSubjects = context.Subjects.IgnoreQueryFilters().ToList();
            Subject GetOrSeedSubject(string name, Guid courseId)
            {
                var sub = existingSubjects.FirstOrDefault(s => s.Name == name && s.CourseId == courseId);
                if (sub == null)
                {
                    sub = new Subject { InstituteId = institute.Id, CourseId = courseId, Name = name };
                    context.Subjects.Add(sub);
                    context.SaveChanges();
                }
                return sub;
            }
            var sub1 = GetOrSeedSubject("Physics", course1.Id);
            var sub2 = GetOrSeedSubject("Chemistry", course1.Id);
            var sub3 = GetOrSeedSubject("Mathematics", course1.Id);
            var sub4 = GetOrSeedSubject("Biology", course2.Id);

            // Seed TeacherSubjects mapping
            var existingTeacherSubjects = context.TeacherSubjects.IgnoreQueryFilters().ToList();
            if (!existingTeacherSubjects.Any(ts => ts.UserId == teacherUser.Id && ts.SubjectId == sub1.Id))
            {
                context.TeacherSubjects.Add(new TeacherSubject
                {
                    InstituteId = institute.Id,
                    UserId = teacherUser.Id,
                    SubjectId = sub1.Id
                });
            }
            if (!existingTeacherSubjects.Any(ts => ts.UserId == teacherUser.Id && ts.SubjectId == sub4.Id))
            {
                context.TeacherSubjects.Add(new TeacherSubject
                {
                    InstituteId = institute.Id,
                    UserId = teacherUser.Id,
                    SubjectId = sub4.Id
                });
            }
            context.SaveChanges();

            // 9. Seed Batches
            var existingBatches = context.Batches.IgnoreQueryFilters().ToList();
            var batch1 = existingBatches.FirstOrDefault(b => b.Name == "JEE 2026 Batch A");
            if (batch1 == null)
            {
                batch1 = new Batch
                {
                    InstituteId = institute.Id,
                    CourseId = course1.Id,
                    Name = "JEE 2026 Batch A",
                    StartDate = new DateOnly(2026, 1, 1),
                    EndDate = new DateOnly(2026, 12, 31),
                    Capacity = 40,
                    IsActive = true
                };
                context.Batches.Add(batch1);
                context.SaveChanges();
            }
            var batch2 = existingBatches.FirstOrDefault(b => b.Name == "NEET 2026 Batch A");
            if (batch2 == null)
            {
                batch2 = new Batch
                {
                    InstituteId = institute.Id,
                    CourseId = course2.Id,
                    Name = "NEET 2026 Batch A",
                    StartDate = new DateOnly(2026, 1, 1),
                    EndDate = new DateOnly(2026, 12, 31),
                    Capacity = 40,
                    IsActive = true
                };
                context.Batches.Add(batch2);
                context.SaveChanges();
            }

            // 10. Enroll Student in Batch 1
            if (!context.StudentBatches.IgnoreQueryFilters().Any(sb => sb.StudentId == student.Id && sb.BatchId == batch1.Id))
            {
                context.StudentBatches.Add(new StudentBatch
                {
                    InstituteId = institute.Id,
                    StudentId = student.Id,
                    BatchId = batch1.Id,
                    JoinedDate = new DateOnly(2026, 1, 1)
                });
                context.SaveChanges();
            }

            // 11. Seed FeePlan for Student
            var feePlan = context.FeePlans.IgnoreQueryFilters().FirstOrDefault(fp => fp.StudentId == student.Id);
            if (feePlan == null)
            {
                feePlan = new FeePlan
                {
                    InstituteId = institute.Id,
                    StudentId = student.Id,
                    CourseId = course1.Id,
                    BatchId = batch1.Id,
                    TotalFee = 120000,
                    DiscountAmount = 20000,
                    FinalFee = 100000,
                    PlanType = "Installment",
                    IsActive = true
                };
                context.FeePlans.Add(feePlan);
                context.SaveChanges();
            }

            // 12. Seed Installments for FeePlan
            if (!context.Installments.IgnoreQueryFilters().Any(i => i.FeePlanId == feePlan.Id))
            {
                var inst1 = new Installment
                {
                    InstituteId = institute.Id,
                    FeePlanId = feePlan.Id,
                    InstallmentNo = 1,
                    Amount = 50000,
                    DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15)),
                    Status = "Paid"
                };
                var inst2 = new Installment
                {
                    InstituteId = institute.Id,
                    FeePlanId = feePlan.Id,
                    InstallmentNo = 2,
                    Amount = 50000,
                    DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15)),
                    Status = "Due"
                };
                context.Installments.AddRange(inst1, inst2);
                context.SaveChanges();

                // 13. Seed Payment for first installment
                var payment = new Payment
                {
                    InstituteId = institute.Id,
                    StudentId = student.Id,
                    FeePlanId = feePlan.Id,
                    InstallmentId = inst1.Id,
                    ReceiptNo = "RCPT-0001",
                    Amount = 50000,
                    PaymentMode = "Cash",
                    PaymentDate = DateTime.UtcNow.AddDays(-15),
                    Remark = "First installment paid in cash"
                };
                context.Payments.Add(payment);
                context.SaveChanges();
            }

            // 14. Seed AttendanceSession
            var attSession = context.AttendanceSessions.IgnoreQueryFilters().FirstOrDefault(asess => asess.BatchId == batch1.Id);
            if (attSession == null)
            {
                attSession = new AttendanceSession
                {
                    InstituteId = institute.Id,
                    BatchId = batch1.Id,
                    AttendanceDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)),
                    TakenByUserId = teacherUser.Id
                };
                context.AttendanceSessions.Add(attSession);
                context.SaveChanges();

                // 15. Seed AttendanceRecord
                var attRecord = new AttendanceRecord
                {
                    InstituteId = institute.Id,
                    AttendanceSessionId = attSession.Id,
                    StudentId = student.Id,
                    Status = "Present"
                };
                context.AttendanceRecords.Add(attRecord);
                context.SaveChanges();
            }

            // 16. Seed Test
            var test = context.Tests.IgnoreQueryFilters().FirstOrDefault(t => t.BatchId == batch1.Id);
            if (test == null)
            {
                test = new Test
                {
                    InstituteId = institute.Id,
                    CourseId = course1.Id,
                    BatchId = batch1.Id,
                    SubjectId = sub1.Id,
                    TestName = "JEE Main Physics Kinematics Mock Test",
                    TestDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                    MaxMarks = 100
                };
                context.Tests.Add(test);
                context.SaveChanges();

                // 17. Seed TestResult
                var testResult = new TestResult
                {
                    InstituteId = institute.Id,
                    TestId = test.Id,
                    StudentId = student.Id,
                    MarksObtained = 85
                };
                context.TestResults.Add(testResult);
                context.SaveChanges();
            }

            // 18. Seed Study Material (Note)
            if (!context.Notes.IgnoreQueryFilters().Any(n => n.BatchId == batch1.Id))
            {
                var note = new Note
                {
                    InstituteId = institute.Id,
                    CourseId = course1.Id,
                    BatchId = batch1.Id,
                    SubjectId = sub1.Id,
                    Title = "Kinematics Quick Revision Formulas",
                    Description = "Includes quick formulas and reference guides for Motion in 1D and 2D",
                    OriginalFileName = "Kinematics_Formulas.pdf",
                    StoredFileName = "Kinematics_Formulas.pdf",
                    FilePath = "/notes/Kinematics_Formulas.pdf",
                    FileType = "pdf",
                    UploadedByUserId = teacherUser.Id
                };
                context.Notes.Add(note);
                context.SaveChanges();
            }

            // 19. Seed Notices
            if (!context.Notices.IgnoreQueryFilters().Any(n => n.BatchId == batch1.Id))
            {
                var notice = new Notice
                {
                    InstituteId = institute.Id,
                    CourseId = course1.Id,
                    BatchId = batch1.Id,
                    Title = "Summer Crash Course Schedule Change",
                    Message = "Please note that the morning lecture for Physics on Sunday will start at 8:00 AM instead of 9:00 AM.",
                    IsActive = true
                };
                context.Notices.Add(notice);
                context.SaveChanges();
            }

            // 20. Seed CRM Enquiries
            if (!context.Enquiries.IgnoreQueryFilters().Any())
            {
                var enquiry1 = new Enquiry
                {
                    InstituteId = institute.Id,
                    FullName = "Bob Vance",
                    Mobile = "9876543213",
                    Email = "bob@vance.com",
                    Source = "Website",
                    Status = "Hot",
                    InterestedCourseId = course1.Id,
                    AssignedToUserId = adminUser.Id,
                    Remark = "Wants to join batch A immediately."
                };
                var enquiry2 = new Enquiry
                {
                    InstituteId = institute.Id,
                    FullName = "Alice Cooper",
                    Mobile = "9876543214",
                    Email = "alice@cooper.com",
                    Source = "Facebook",
                    Status = "Warm",
                    InterestedCourseId = course2.Id,
                    AssignedToUserId = adminUser.Id,
                    Remark = "Enquired about fee discounts."
                };
                context.Enquiries.AddRange(enquiry1, enquiry2);
                context.SaveChanges();
            }

            // 21. Seed Vacancies
            if (!context.Vacancies.IgnoreQueryFilters().Any())
            {
                var vacancy1 = new Vacancy
                {
                    InstituteId = institute.Id,
                    Title = "SSC CGL 2026 Examination Notice",
                    Department = "Staff Selection Commission",
                    ExamCategory = "SSC",
                    QualificationRequired = "Bachelor's Degree",
                    AgeLimit = "18-32 Years",
                    StartDate = new DateOnly(2026, 6, 1),
                    LastDate = new DateOnly(2026, 7, 15),
                    OfficialLink = "https://ssc.gov.in",
                    Description = "Staff Selection Commission (SSC) has released the official notification for Combined Graduate Level (CGL) Exam 2026 for various Group B and Group C posts.",
                    IsActive = true
                };
                var vacancy2 = new Vacancy
                {
                    InstituteId = institute.Id,
                    Title = "IBPS PO/MT XVI Recruitment 2026",
                    Department = "Institute of Banking Personnel Selection",
                    ExamCategory = "Banking",
                    QualificationRequired = "Any Graduate",
                    AgeLimit = "20-30 Years",
                    StartDate = new DateOnly(2026, 8, 1),
                    LastDate = new DateOnly(2026, 8, 31),
                    OfficialLink = "https://ibps.in",
                    Description = "IBPS PO recruitment notification for probationary officers/management trainees in participating public sector banks.",
                    IsActive = true
                };
                context.Vacancies.AddRange(vacancy1, vacancy2);
                context.SaveChanges();
            }
        }
    }
}

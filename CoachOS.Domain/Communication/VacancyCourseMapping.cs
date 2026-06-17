using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using System;

namespace CoachOS.Domain.Communication
{
    public class VacancyCourseMapping : TenantBaseEntity
    {
        public Guid VacancyId { get; set; }
        public Guid CourseId { get; set; }

        public Vacancy? Vacancy { get; set; }
        public Course? Course { get; set; }
    }
}

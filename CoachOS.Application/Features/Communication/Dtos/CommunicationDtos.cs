using System;

namespace CoachOS.Application.Features.Communication.Dtos
{
    public class NoticeDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Guid? CourseId { get; set; }
        public Guid? BatchId { get; set; }
    }

    public class CreateNoticeRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Guid? CourseId { get; set; }
        public Guid? BatchId { get; set; }
    }

    public class VacancyDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ExamCategory { get; set; } = string.Empty;
        public DateTime LastDate { get; set; }
    }

    public class CreateVacancyRequest
    {
        public string Title { get; set; } = string.Empty;
        public string ExamCategory { get; set; } = string.Empty;
        public DateTime LastDate { get; set; }
    }
}

using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Student;
using System;
using System.Collections.Generic;

namespace CoachOS.Domain.Learning
{
    public class Doubt : TenantBaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? BatchId { get; set; }
        public Guid? SubjectId { get; set; }
        
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public string Status { get; set; } = "Open"; // Open, Answered, Closed
        
        public CoachOS.Domain.Student.Student? Student { get; set; }
        public Course? Course { get; set; }
        public Batch? Batch { get; set; }
        public Subject? Subject { get; set; }
        
        public ICollection<DoubtReply> Replies { get; set; } = new List<DoubtReply>();
    }
}

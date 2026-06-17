using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Student
{
    public class StudentParent : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid ParentId { get; set; }
        public string RelationshipType { get; set; } = "Parent"; // Father, Mother, Guardian

        public Student? Student { get; set; }
        public Parent? Parent { get; set; }
    }
}

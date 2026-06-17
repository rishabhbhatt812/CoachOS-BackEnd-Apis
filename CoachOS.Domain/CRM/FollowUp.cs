using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.CRM
{
    public class FollowUp : TenantBaseEntity
    {
        public Guid EnquiryId { get; set; }
        public DateTime FollowUpDate { get; set; } = DateTime.UtcNow;
        public DateOnly? NextFollowUpDate { get; set; }
        public string Remark { get; set; } = string.Empty;

        public Enquiry? Enquiry { get; set; }
    }
}

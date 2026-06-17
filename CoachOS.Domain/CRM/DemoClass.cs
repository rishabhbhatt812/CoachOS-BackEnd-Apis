using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using System;

namespace CoachOS.Domain.CRM
{
    public class DemoClass : TenantBaseEntity
    {
        public Guid EnquiryId { get; set; }
        public Guid? BatchId { get; set; }
        public DateTime DemoDate { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string? Remark { get; set; }

        public Enquiry? Enquiry { get; set; }
        public Batch? Batch { get; set; }
    }
}

using CoachOS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoachOS.Domain.Tenancy
{
    public class Institute : BaseEntity
    {
        // Basic Information
        public string InstituteCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // Institute Name
        public string? ShortName { get; set; }
        public string? LogoPath { get; set; } // Logo Path
        public string? Description { get; set; }

        // Contact Information
        public string ContactPersonName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? AlternateMobileNumber { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string? WebsiteUrl { get; set; }

        // Address Information
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;

        // Academic Information
        public string InstituteType { get; set; } = string.Empty; // Coaching Institute, School, College, Training Center, Tuition Center
        public int? EstablishedYear { get; set; }
        public string AcademicSessionStartMonth { get; set; } = string.Empty;
        public string AcademicSessionEndMonth { get; set; } = string.Empty;

        // Owner / Director Information
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerMobile { get; set; } = string.Empty;
        public string? OwnerEmail { get; set; }
        public string? AadhaarNumber { get; set; }
        public string? PANNumber { get; set; }
        public string? GSTNumber { get; set; }

        // Subscription / Plan Information
        public string PlanName { get; set; } = string.Empty;
        public int MaxStudentsAllowed { get; set; }
        public int MaxTeachersAllowed { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsTrial { get; set; }

        // Settings / Config
        public bool SMSEnabled { get; set; } = false;
        public bool EmailEnabled { get; set; } = false;
        public bool WhatsAppEnabled { get; set; } = false;
        public string Currency { get; set; } = "INR";

        // Other existing properties
        public string? ReceiptPrefix { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

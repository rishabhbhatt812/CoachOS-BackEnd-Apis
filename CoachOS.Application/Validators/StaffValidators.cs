using CoachOS.Application.Features.Staff.Dtos;
using FluentValidation;

namespace CoachOS.Application.Validators
{
    public class CreateStaffRequestValidator : AbstractValidator<CreateStaffRequest>
    {
        public CreateStaffRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(150).WithMessage("Full Name cannot exceed 150 characters.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("BranchId is required.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Invalid email address format.");

            RuleFor(x => x.MobileNumber)
                .Matches(@"^\+?\d{10,15}$").When(x => !string.IsNullOrEmpty(x.MobileNumber)).WithMessage("Invalid mobile number format.");

            // Email or MobileNumber is required
            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.MobileNumber))
                .WithMessage("Either Email or Mobile Number must be provided.");
        }
    }

    public class UpdateStaffRequestValidator : AbstractValidator<UpdateStaffRequest>
    {
        public UpdateStaffRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(150).WithMessage("Full Name cannot exceed 150 characters.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("BranchId is required.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Invalid email address format.");

            RuleFor(x => x.MobileNumber)
                .Matches(@"^\+?\d{10,15}$").When(x => !string.IsNullOrEmpty(x.MobileNumber)).WithMessage("Invalid mobile number format.");

            // Email or MobileNumber is required
            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.MobileNumber))
                .WithMessage("Either Email or Mobile Number must be provided.");
        }
    }

    public class UpdateTeacherProfileRequestValidator : AbstractValidator<UpdateTeacherProfileRequest>
    {
        public UpdateTeacherProfileRequestValidator()
        {
            RuleFor(x => x.SubjectExpertise)
                .MaximumLength(250).WithMessage("Subject expertise cannot exceed 250 characters.");
            
            RuleFor(x => x.TeacherType)
                .MaximumLength(50).WithMessage("Teacher type cannot exceed 50 characters.");

            RuleFor(x => x.Bio)
                .MaximumLength(1000).WithMessage("Bio cannot exceed 1000 characters.");
        }
    }
}

using CoachOS.Application.Features.Students.Dtos;
using FluentValidation;

namespace CoachOS.Application.Validators;

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.StudentCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Mobile).Matches(@"^\+?\d{10,15}$").When(x => !string.IsNullOrEmpty(x.Mobile)).WithMessage("Invalid mobile format.");
        RuleFor(x => x.AdmissionDate).NotEmpty();
    }
}

public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
{
    public UpdateStudentRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Mobile).Matches(@"^\+?\d{10,15}$").When(x => !string.IsNullOrEmpty(x.Mobile)).WithMessage("Invalid mobile format.");
    }
}

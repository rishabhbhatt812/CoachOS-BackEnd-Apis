using CoachOS.Application.Features.Academics.Dtos;
using FluentValidation;

namespace CoachOS.Application.Validators;

public class CreateBatchRequestValidator : AbstractValidator<CreateBatchRequest>
{
    public CreateBatchRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseId).NotEmpty();
    }
}

public class UpdateBatchRequestValidator : AbstractValidator<UpdateBatchRequest>
{
    public UpdateBatchRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseId).NotEmpty();
    }
}

public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CourseCategory).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseType).NotEmpty();
        RuleFor(x => x.DurationValue).GreaterThan(0);
        RuleFor(x => x.DurationType).NotEmpty();
    }
}

public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CourseCategory).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseType).NotEmpty();
        RuleFor(x => x.DurationValue).GreaterThan(0);
        RuleFor(x => x.DurationType).NotEmpty();
    }
}

public class CreateSubjectRequestValidator : AbstractValidator<CreateSubjectRequest>
{
    public CreateSubjectRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseId).NotEmpty();
    }
}

using ErrorOr;

using FluentValidation;

using Kimmen.FeatureSlicing.Api.Web.Shared.Model;

using MediatR;

namespace Kimmen.FeatureSlicing.Api.Web.Features;

public static class AddStudent
{
    public record WithName(string FirstName, string LastName) : IRequest<ErrorOr<Success>>;
    internal class Handler : IRequestHandler<WithName, ErrorOr<Success>>
    {
        private readonly Classroom<NamedTeacher, NamedStudent> classroom;

        public Handler(Classroom<NamedTeacher, NamedStudent> classroom) 
        {
            this.classroom = classroom;
        }

        public async Task<ErrorOr<Success>> Handle(WithName request, CancellationToken cancellationToken)
        {
            this.classroom.AddStudent(new NamedStudent(request.FirstName, request.LastName));

            return Result.Success;
        }
    }
}

public class AddStudentWithNameValidator : AbstractValidator<AddStudent.WithName>
{
    public AddStudentWithNameValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(255);
    }
}

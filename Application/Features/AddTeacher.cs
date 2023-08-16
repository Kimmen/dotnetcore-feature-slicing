using ErrorOr;

using FluentValidation;
using FluentValidation.Results;

using Kimmen.FeatureSlicing.Api.Web.Shared.Model;

using MediatR;

namespace Kimmen.FeatureSlicing.Api.Web.Features;


public static class AddTeacher
{
    public record WithName(string AddressedAs, string LastName) : IRequest<ErrorOr<Success>>;

    internal class Handler : IRequestHandler<WithName, ErrorOr<Success>>
    {
        private readonly Classroom<NamedTeacher, NamedStudent> classroom;

        public Handler(Classroom<NamedTeacher, NamedStudent> classroom)
        {
            this.classroom = classroom;
        }

        public async Task<ErrorOr<Success>> Handle(WithName request, CancellationToken cancellationToken)
        {
            var teacher = new NamedTeacher(request.AddressedAs, request.LastName);
            this.classroom.AddTeacher(teacher);

            return Result.Success;
        }
    }
}

public class AddTeacherValidator : AbstractValidator<AddTeacher.WithName>
{
    public AddTeacherValidator()
    {
        RuleFor(x => x.AddressedAs)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(4)
            .Must(a => new[] { "Mr", "Mrs", "Miss" }.Contains(a)).WithMessage("Must be either Mr, Mrs or Miss");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(256);
    }
}
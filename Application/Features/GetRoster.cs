using ErrorOr;

using Kimmen.FeatureSlicing.Api.Web.Shared.Model;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Kimmen.FeatureSlicing.Api.Web.Features
{
    public static class GetRoster
    {
        public record Request() : IRequest<ErrorOr<Roster>>;
        public record Roster(string TeacherFullName, IReadOnlyCollection<string> StudentsFullNames);

        internal class Handler : IRequestHandler<Request, ErrorOr<Roster>>
        {
            private readonly Classroom<NamedTeacher, NamedStudent> classroom;
            private readonly ILogger<Handler> logger;

            public Handler(Classroom<NamedTeacher, NamedStudent> classroom, ILogger<Handler> logger)
            {
                this.classroom = classroom;
                this.logger = logger;
            }

            public async Task<ErrorOr<Roster>> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    var (teacher, students) = this.classroom.GetRoster();

                    return new Roster(teacher.FullName(), students.Select(x => x.FullName()).Order().ToArray());
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Failed to get roster");

                    //Its a matter of opinion, I think if the classroom is incomplete the roster doesn't exists.
                    return Error.NotFound(description: "Classroom not full");
                }
            }
        }

    }
}

using Kimmen.FeatureSlicing.Api.Web.Features;

using MediatR;

namespace Kimmen.FeatureSlicing.Api.Web.Endpoints;

public static class EndpointConfiguration
{
    public static WebApplication MapClassroomEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/classroom");

        group.MapPut("teacher", async (AddTeacher.WithName addTeachWitName, ISender sender, CancellationToken cancellationToken) =>
            await sender.Send(addTeachWitName, cancellationToken).AsResult(_ => Results.Accepted())
        )
            .WithName("AddTeacher")
            .WithOpenApi()
            .Produces(StatusCodes.Status202Accepted)
            .ProducesValidationProblem();

        return app;
    }
}
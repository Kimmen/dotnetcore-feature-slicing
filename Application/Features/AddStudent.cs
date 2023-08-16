using ErrorOr;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kimmen.FeatureSlicing.Api.Web.Features
{
    public static class AddStudent
    {
        public record Request(string FirstName, string LastName) : IRequest<ErrorOr<Success>>;
        internal class Handler : IRequestHandler<Request, ErrorOr<Success>>
        {
            public Task<ErrorOr<Success>> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

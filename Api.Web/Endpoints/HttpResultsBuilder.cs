using ErrorOr;

namespace Kimmen.FeatureSlicing.Api.Web.Endpoints
{
    public static class HttpResultsBuilder
    {
        /// <summary>
        /// Used to build consistent HTTP results to our conventions.
        /// </summary>
        public static async Task<IResult> AsResult<T>(this Task<ErrorOr<T>> responseFactory, Func<T, IResult>? resultFactory = null)
        {
            var response = await responseFactory;
            resultFactory ??= r => Results.Ok(r);

            var result = response.Match(
                value => resultFactory(value),
                errors => errors.Count > 1
                ? CreateValidationProblems(errors)
                : ReturnSingleError(errors.First()));

            return result!;
        }

        private static IResult ReturnSingleError(Error error)
        {
            return error.Type switch
            {
                ErrorType.NotFound => Results.NotFound(error.Description),
                ErrorType.Conflict => Results.Conflict(error.Description),
                ErrorType.Validation => CreateValidationProblems(new [] { error}),
                _ => throw new Exception(error.Description)
            };
        }

        private static IResult CreateValidationProblems(IEnumerable<Error> errors)
        {
            //Note: PoC, in real life this should be more carefully constructed, maybe not even using ValidationProblem, but another type of Problem result?
            return Results.ValidationProblem(errors
                .ToLookup(e => e.Code)
                .ToDictionary(e => e.Key, errors => errors.Select(e => e.Description).ToArray()));
        }
    }
}

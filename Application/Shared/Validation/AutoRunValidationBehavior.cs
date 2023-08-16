using ErrorOr;

using FluentValidation;
using FluentValidation.Results;

using MediatR;

using System.Reflection;

namespace Kimmen.FeatureSlicing.Api.Web.Shared.Validation;

//public class AutoRunValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, ErrorOr<TResponse>> where TRequest: notnull
//{
//    private readonly IValidator<TRequest>? validator;

//    public AutoRunValidationBehavior(IValidator<TRequest>? validator)
//    {
//        this.validator = validator;
//    }

//    public async Task<ErrorOr<TResponse>> Handle(TRequest request, RequestHandlerDelegate<ErrorOr<TResponse>> next, CancellationToken cancellationToken)
//    {
//        if (this.validator is null)
//        {
//            return await next();
//        }

//        var validation = await this.validator.ValidateAsync(request, cancellationToken);
//        if(validation.IsValid)
//        {
//            return await next();
//        }

//        return validation.Errors
//            .Select(x => Error.Validation(x.ErrorCode, $"{x.PropertyName}-{x.ErrorMessage}"))
//            .ToList();
//    }
//}

//Steal with pride from ErrorOr github..
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator == null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        return TryCreateResponseFromErrors(validationResult.Errors, out var response)
            ? response
            : throw new ValidationException(validationResult.Errors);
    }

    private static bool TryCreateResponseFromErrors(List<ValidationFailure> validationFailures, out TResponse response)
    {
        List<Error> errors = validationFailures.ConvertAll(x => Error.Validation(
                code: x.PropertyName,
                description: x.ErrorMessage));

        response = (TResponse?)typeof(TResponse)
            .GetMethod(
                name: nameof(ErrorOr<object>.From),
                bindingAttr: BindingFlags.Static | BindingFlags.Public,
                types: new[] { typeof(List<Error>) })?
            .Invoke(null, new[] { errors })!;

        return response is not null;
    }
}

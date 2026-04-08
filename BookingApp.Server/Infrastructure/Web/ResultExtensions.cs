using BookingApp.Server.Infrastructure.Errors;

namespace BookingApp.Server.Infrastructure.Web;

public static class ResultExtensions
{
    public static IResult ToOkResult<T, TResponse>(this ErrorOr<T> result)
    {
        return result.Match(
            value => 
            {
                var response = value.Adapt<TResponse>();

                return Results.Ok(response);
            },
            errors => MapErrorsToHttpResult(errors)
        );
    }

    public static IResult ToCreatedResult<T, TResponse>(
        this ErrorOr<T> result,
        Func<T, string> locationFactory)
    {
        return result.Match(
            value =>
            {
                var response = value.Adapt<TResponse>();
                var location = locationFactory(value);

                return Results.Created(location, response);
            },
            errors => MapErrorsToHttpResult(errors)
        );
    }

    public static IResult ToNoContentResult<T>(this ErrorOr<T> result)
    {
        return result.Match(
            _ => Results.NoContent(),
            errors => MapErrorsToHttpResult(errors)
        );
    }

    public static IResult ToHttpResult<T>(this ErrorOr<T> result, Func<T, IResult> onSuccess)
    {
        return result.Match(
            value => onSuccess(value),
            errors => MapErrorsToHttpResult(errors)
        );
    }

    private static IResult MapErrorsToHttpResult(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            return Results.ValidationProblem(
                errors.ToDictionary(
                    e => e.Code,
                    e => new[] { e.Description }
                )
            );
        }

        var primaryError = errors.First();

        return Results.Problem(
            statusCode: ErrorTypeToStatusCode.Map(primaryError.Type),
            title: primaryError.Description
        );
    }
}

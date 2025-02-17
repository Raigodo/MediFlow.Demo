using System.Dynamic;

namespace MediFlow.Api.Modules._Shared.Services;

public sealed class ResponseFactory
{
    public IResult Unauthorized() => TypedResults.Unauthorized();
    public IResult Forbid() => TypedResults.Forbid();
    public IResult NoContent() => TypedResults.NoContent();
    public IResult Ok(object responseBody) => TypedResults.Ok(responseBody);

    public IResult Conflict<TEntity>(string propertyName = "id")
    {
        var responseBody = new
        {
            Title = $"{typeof(TEntity).Name} with provided {propertyName} already exists.",
        };
        return TypedResults.Conflict(responseBody);
    }

    public IResult NotFound<TEntity>()
    {
        var responseBody = new
        {
            Title = $"{typeof(TEntity).Name} was not found.",
        };
        return TypedResults.NotFound(responseBody);
    }

    public IResult BadRequest() => BadRequest("One or more validation errors occurred.");
    public IResult BadRequest(Dictionary<string, IEnumerable<string>> errors) => 
        BadRequest("One or more validation errors occurred.", errors);

    public IResult BadRequest(
        string message,
        IDictionary<string, IEnumerable<string>>? errors = null)
    {
        var expando = new ExpandoObject();
        var expandoDict = (IDictionary<string, object>)expando;

        foreach (var kvp in errors)
        {
            expandoDict[kvp.Key] = kvp.Value;
        }
        var responseBody = new
        {
            Title = message,
            Errors = expandoDict,
        };
        return TypedResults.BadRequest(responseBody);
    }
}

using FluentValidation.Results;

namespace MinimalAPI.Extensions;

public static class ValidationExtensions
{
    public static IEnumerable<object> ToResponse(this ValidationResult errors)
    {
        var items = errors.Errors.Select(x => new
        {
            Property = x.PropertyName,
            Message = x.ErrorMessage
        });

        return items;
    }
}

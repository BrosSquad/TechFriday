using MongoDB.Bson;

namespace FastEndpoints.Example.RouteConstraints;

public class MongoIdConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext,
        IRouter? route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        var value = values[routeKey];

        if (value is null)
        {
            return false;
        }

        if (value is not string)
        {
            return false;
        }

        if (value is string s && s.Length != 24)
        {
            return false;
        }

        return ObjectId.TryParse((string)value, out _);
    }
}

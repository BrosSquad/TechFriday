using FastEndpoints.Example.RouteConstraints;
using Microsoft.AspNetCore.Routing;

namespace Service.Unit.Tests.RouteConstraints;

public class MongoIdConstraintTests
{
    [Theory]
    [InlineData("", false)]
    [InlineData(1, false)]
    [InlineData(1.1, false)]
    [InlineData("629097284d5fcd21f81f726", false)]
    [InlineData("629097284d5fcd21f81f7263", true)]
    public void Test(object input, bool expected)
    {
        var constraint = new MongoIdConstraint();

        var result = ConstraintsTestHelper.TestConstraint(constraint, input);

        result.Should().Be(expected);
    }
}

internal class ConstraintsTestHelper
{
    public static bool TestConstraint(IRouteConstraint constraint, object value)
    {
        var parameterName = "fake";
        var values = new RouteValueDictionary
        {
            { parameterName, value }
        };
        var routeDirection = RouteDirection.IncomingRequest;

        return constraint.Match(
            httpContext: null,
            route: null,
            parameterName,
            values,
            routeDirection
        );
    }
}

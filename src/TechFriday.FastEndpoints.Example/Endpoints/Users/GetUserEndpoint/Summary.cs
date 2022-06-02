using FastEndpoints.Example.Models;

namespace FastEndpoints.Example.Endpoints.Users.GetUserEndpoint;

public class GetUserEndpointSummary : Summary<GetUserEndpoint>
{
	public GetUserEndpointSummary()
	{
		Summary = "";
		Description = "";
		Response<User>(200, "User found.");
		Response<object>(404, "User not found.");
	}
}


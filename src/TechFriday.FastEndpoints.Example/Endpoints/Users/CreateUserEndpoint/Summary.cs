namespace FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;

public class CreateUserEndpointSummary : Summary<CreateUserEndpoint>
{
    public CreateUserEndpointSummary()
    {
        Summary = "Create user endpoint.";
        Description = "This endpoint creates user (register).";
        ExampleRequest = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "password"
        };

        Response<CreateUserResponse>(StatusCodes.Status201Created, "User succesfully created.");
        Response<List<Extensions.ErrorResponse>>(StatusCodes.Status422UnprocessableEntity, "Validation errors.");
    }
}

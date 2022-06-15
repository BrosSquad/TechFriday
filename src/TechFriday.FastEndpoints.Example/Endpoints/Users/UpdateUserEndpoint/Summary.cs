namespace FastEndpoints.Example.Endpoints.Users.UpdateUserEndpoint;

public class UpdateUserEndpointSummary : Summary<UpdateUserEndpoint>
{
    public UpdateUserEndpointSummary()
    {
        Summary = "Update user endpoint";
        Description = "This endpoint updates the user";
        ExampleRequest = new UpdateUserRequest
        {
            FirstName = "test",
            LastName = "test",
            Email = "test@test.com",
            Password = "password"
        };
        Response<EmptyResponse>(StatusCodes.Status204NoContent, "Updated user successfully.");
        Response<Extensions.ErrorResponse>(StatusCodes.Status422UnprocessableEntity, "Validation errors.");
        Response<Extensions.ErrorResponse>(StatusCodes.Status400BadRequest, "User not found.");

    }
}

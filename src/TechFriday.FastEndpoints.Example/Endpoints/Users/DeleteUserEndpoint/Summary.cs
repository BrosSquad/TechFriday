namespace FastEndpoints.Example.Endpoints.Users.DeleteUserEndpoint;

public class DeleteUserEndpointSummary : Summary<DeleteUserEndpoint>
{
    public DeleteUserEndpointSummary()
    {
        Summary = "Delete user endpoint";
        Description = "This endpoint deletes user.";

        Response<EmptyResponse>(StatusCodes.Status204NoContent, "User deleted successfully.");
        Response<Extensions.ErrorResponse>(StatusCodes.Status404NotFound, "User not found.");
    }
}

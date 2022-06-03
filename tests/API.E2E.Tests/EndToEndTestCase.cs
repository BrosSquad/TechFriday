namespace API.E2E.Tests;

public abstract class EndToEndTestCase
{
    protected readonly WebApplicationFactory<Program> Application;
    protected readonly HttpClient Client;
    protected readonly Fixture Fixture = new();

    protected EndToEndTestCase()
    {
        Application = new WebApplicationFactory<Program>();
        Client = Application.CreateClient();
    }
}
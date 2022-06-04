using System.Text.Json;
using Microsoft.Net.Http.Headers;

namespace API.E2E.Tests.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static string ExtractCookie(this HttpResponseMessage response, string cookieName)
    {
        var cookies = response.Headers.GetValues(HeaderNames.SetCookie);

        foreach (var cookie in cookies)
        {
            if (cookie.Contains(cookieName))
            {
                return cookie;
            }
        }

        return string.Empty;
    }

    public static async Task<(HttpStatusCode statusCode, T data)> Extract<T>(this HttpResponseMessage response)
    {
        var stream = await response.Content.ReadAsStreamAsync();

        var body = await JsonSerializer.DeserializeAsync<T>(stream);

        return (response.StatusCode, body)!;
    }
}

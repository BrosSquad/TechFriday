﻿using API.E2E.Tests.Extensions;
using FastEndpoints.Example.Endpoints.Login;
using FastEndpoints.Example.Models;

namespace API.E2E.Tests.Endpoints.Auth;

public class LoginEndpointTests : EndToEndTestCase
{
    //[Fact]
    //public async Task Error_When_User_Not_Found()
    //{
    //    // Arrange
    //    const string url = "/auth/login";

    //    var user = new LoginRequest
    //    {
    //        Email = "test@test.com",
    //        Password = "password"
    //    };

    //    // Act
    //    var response = await Client.PostAsJsonAsync(url, user);

    //    var authCookie = response.Headers.GetValues(HeaderNames.SetCookie).First();
    //    var (status, body) = await response.Extract<User>();

    //    // Assert
    //    status.Should().Be(HttpStatusCode.OK);
    //    authCookie.Should().NotBeNull();
    //    authCookie.Should().BeOfType<string>();
    //    body.Should().NotBeNull().And.BeOfType<User>();
    //}
}


using System.Net;
using System.Net.Http.Json;
using Shogun.Server.DTO;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Integration;

/// <summary>
/// Integration tests: sends real HTTP requests to the Server running in memory.
/// </summary>
public class HealthEndpointTests : IClassFixture<ShogunApiFactory>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(ShogunApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_WithoutSigningIn_ReturnsTheServerDetails()
    {
        var response = await _client.GetAsync("/api/v1/health");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var health = await response.Content.ReadFromJsonAsync<HealthResponse>();

        health.ShouldNotBeNull();
        health.Status.ShouldBe("ok");
        health.Name.ShouldNotBeNullOrWhiteSpace();
        health.Version.ShouldNotBeNullOrWhiteSpace();
        Guid.TryParse(health.Id, out _).ShouldBeTrue();
    }

    [Fact]
    public async Task UnknownAddress_WithoutSigningIn_ReturnsUnauthorizedProblemDetails()
    {
        var response = await _client.GetAsync("/api/v1/does-not-exist");

        // The fallback policy answers 401 before routing can answer 404,
        // so a stranger can't tell which addresses exist
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var mediaType = response.Content.Headers.ContentType?.MediaType;

        mediaType.ShouldBe("application/problem+json");
    }
}

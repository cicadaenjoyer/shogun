using System.Net;
using System.Net.Http.Json;
using Shogun.Server.DTO;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Integration;

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
    public async Task UnknownAddress_ReturnsProblemDetails()
    {
        var response = await _client.GetAsync("/api/v1/does-not-exist");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var mediaType = response.Content.Headers.ContentType?.MediaType;

        mediaType.ShouldBe("application/problem+json");
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Integration;

/// <summary>
/// Every endpoint requires a login unless it's on <see cref="PublicEndpoints"/>.
/// The fallback policy closes endpoints by default; these tests make sure
/// opening one is always a deliberate, reviewed decision.
/// </summary>
public class PublicEndpointTests : IClassFixture<ShogunApiFactory>
{
    /// <summary>
    /// Endpoints anyone can call without signing in, written as "METHOD /path".
    /// Each entry needs a reason.
    /// </summary>
    private static readonly string[] PublicEndpoints =
    [
        "GET /api/v1/health", // the web app checks an address is a Shogun server before anyone signs in
    ];

    private readonly List<RouteEndpoint> _endpoints;

    public PublicEndpointTests(ShogunApiFactory factory)
    {
        _endpoints = factory.Services
            .GetRequiredService<EndpointDataSource>()
            .Endpoints
            .OfType<RouteEndpoint>()
            .ToList();
    }

    [Fact]
    public void EveryPublicEndpoint_IsOnThePublicList()
    {
        var publicByAccident = PublicEndpointNames().Except(PublicEndpoints).ToList();

        publicByAccident.ShouldBeEmpty("These endpoints allow anonymous access but aren't on the public list");
    }

    [Fact]
    public void EveryEntryOnThePublicList_IsAPublicEndpoint()
    {
        var stale = PublicEndpoints.Except(PublicEndpointNames()).ToList();

        stale.ShouldBeEmpty("These entries are on the public list, but the endpoint is missing or requires a login");
    }

    private IEnumerable<string> PublicEndpointNames() =>
        _endpoints
            .Where(endpoint => endpoint.Metadata.GetMetadata<IAllowAnonymous>() is not null)
            .SelectMany(Describe);

    // One name per HTTP method, e.g. "GET /api/v1/health"
    private static IEnumerable<string> Describe(RouteEndpoint endpoint)
    {
        var path = "/" + endpoint.RoutePattern.RawText?.TrimStart('/');
        var methods = endpoint.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods ?? ["ANY"];

        return methods.Select(method => $"{method} {path}");
    }
}

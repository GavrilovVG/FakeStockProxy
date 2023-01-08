using FakeStockProxy.Application.DataTransferObjects;
using FakeStockProxy.Application.Miscellaneous.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FakeStockProxy.FunctionalTests;

public class FakeStockProxyTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IConfigurationRoot _configuration;

    public FakeStockProxyTests(WebApplicationFactory<Program> factory)
    {
        var projectDir = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(projectDir, "appsettings.production.json");
                   
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Production");
        });

        _configuration = new ConfigurationBuilder()
            .AddJsonFile(configPath, optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

    [Fact]
    public async void Regular_request()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{_configuration["FsProxyService:username"]}:{_configuration["FsProxyService:password"]}")));

        var response = await client.GetAsync("/api/v1/FsStockRequest/GetStock?Take=50");

        var retval = await response.Content.ReadFromJsonAsync<FsStockRequestDto>();

        Assert.NotNull(retval);
        Assert.NotEqual(StockRequestResultEnum.None, retval!.StockRequestResultCode);
    }
    
    [Fact]
    public async void Unauthorized_request()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"asdf:sdfasdf")));

        var response = await client.GetAsync("/api/v1/FsStockRequest/GetStock?Take=50");

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

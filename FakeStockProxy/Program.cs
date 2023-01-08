using FakeStockProxy.Application.DataTransferObjects;
using FakeStockProxy.Application.Interfaces;
using FakeStockProxy.Application.Services;
using FakeStockProxy.Core.Configuration;
using FakeStockProxy.Core.Interfaces;
using FakeStockProxy.Core.Interfaces.Repositories;
using FakeStockProxy.Infrastracture.Authentication;
using FakeStockProxy.Infrastracture.BackgroundServices;
using FakeStockProxy.Infrastracture.HostedServices;
using FakeStockProxy.Infrastructure.Data;
using FakeStockProxy.Infrastructure.Logging;
using FakeStockProxy.Infrastructure.Repository;
using FakeStockProxy.Web.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

#region services

builder.Host.ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment}.json", optional: true)
            .AddEnvironmentVariables();
});

builder.Services.AddControllers();
builder.Services.AddApiVersioning();
builder.Services.AddVersionedApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerJsonIgnoreFilter>();
    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "basic"
                    }
                },
                new string[] {}
        }
    });
});
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
});
builder.Services.AddHttpClient<IHttpClientTypedService<FsResponseDataDto>, HttpClientStockRequestService>(config =>
{
    config.BaseAddress = new Uri(builder.Configuration["FakeStockRemoteServiceUrl"]);
    
    if (!builder.Environment.IsDevelopment())
    {
        config.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII
            .GetBytes($"{builder.Configuration["FsRemoteService:username"]}:{builder.Configuration["FsRemoteService:password"]}")));
    }
});
builder.Services.AddEntityFrameworkSqlite();
builder.Services.AddDbContextPool<FakeStockProxyContext>((sp, optionsBuilder) =>
{
    if (FakeStockProxySettings.DatabaseProvider == builder.Configuration["DatabaseProvider"])
    {
        optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase"));
        optionsBuilder.UseInternalServiceProvider(sp);
    }
});
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
builder.Services.AddScoped<IFsStockRequestRepository, FsStockRequestRepository>();
builder.Services.AddScoped<IFsStockRequestService, FsStockRequestService>();
builder.Services.AddScoped<IFsBackgroundTasksService, FsBackgroundTasksService>();

builder.Services.AddHostedService<FsBackgroundService>();
builder.Services.AddSingleton<IFsBackgroundTaskQueue>(ctx =>
{
    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
        queueCapacity = 100;
    return new FsBackgroundTaskQueue(queueCapacity);
});

#endregion

#region middleware

var app = builder.Build();

using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetService<FakeStockProxyContext>();
await context!.Database.EnsureCreatedAsync();

app.UseSwagger();
app.UseSwaggerUI();
app.UseResponseCompression();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

#endregion

app.Run();

public partial class Program { }
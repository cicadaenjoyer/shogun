using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shogun.Server.Data;
using Shogun.Server.Options;
using Shogun.Server.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Adding Services...

    // Logging
    builder.Services.AddSerilog();

    // Settings (Hub, Web, Storage, & Transcoding)
    builder.Services
        .AddOptions<HubOptions>()
        .BindConfiguration(HubOptions.SectionName)
        .ValidateDataAnnotations()
        .ValidateOnStart();
    builder.Services
        .AddOptions<WebOptions>()
        .BindConfiguration(WebOptions.SectionName)
        .ValidateDataAnnotations()
        .ValidateOnStart();
    // StorageOptions has file-system rules that attributes can't express, so it
    // gets a validator class as well. Both run, and their failures are reported
    // together.
    builder.Services.AddSingleton<IValidateOptions<StorageOptions>, StorageOptionsValidator>();
    builder.Services
        .AddOptions<StorageOptions>()
        .BindConfiguration(StorageOptions.SectionName)
        .ValidateDataAnnotations()
        .ValidateOnStart();
    builder.Services
        .AddOptions<TranscodingOptions>()
        .BindConfiguration(TranscodingOptions.SectionName)
        .ValidateDataAnnotations()
        .ValidateOnStart();

    // JWT
    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication("Bearer").AddJwtBearer();

    // Controllers
    builder.Services.AddControllers();

    // Databases
    builder.Services.AddDbContext<ShogunDbContext>(opt =>
    {
        opt.UseSqlite(builder.Configuration.GetConnectionString("Default"));
    });

    // API & Controllers
    builder.Services.AddOpenApi();
    builder.Services.AddSingleton<HealthService>();

    // Problem Details
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();
    app.UseStatusCodePages();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerUi(options =>
        {
            options.DocumentPath = "/openapi/v1.json";
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
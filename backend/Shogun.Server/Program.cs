using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shogun.Server.Data;
using Shogun.Server.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Settings
//
// Reads each section (Hub, Web, Storage, and Transcoding) into its matching
// XOptions object, checks its rules, and refuses to start if any of them fail.
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

// Controllers & Database
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ShogunDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

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

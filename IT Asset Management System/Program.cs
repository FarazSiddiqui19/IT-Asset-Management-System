using IT_Asset_Management_System.Common.Extensions; // Service extension methods
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Data.Configurations; // To ensure configurations are discoverable
using IT_Asset_Management_System.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "IT Asset Management API", Version = "v1" });

    const string schemeId = "Bearer";

    options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",          // lowercase per RFC 7235
        BearerFormat = "JWT",
        Description = "Enter your JWT token"
    });

    // ? New delegate-based syntax for .NET 10 / Swashbuckle v10
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(schemeId, document)] = []
    });
});

builder.Services.AddDbContext<dbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Unit of Work


// Register repositories
builder.Services.AddRepositories();
// JWT and auth services
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Add Swagger UI for development
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

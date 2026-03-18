using IT_Asset_Management_System.Common.Settings;
using IT_Asset_Management_System.Repository;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Services;
using IT_Asset_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IT_Asset_Management_System.Common.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IAssignmentRequestRepository, AssignmentRequestRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            if (jwtSettings == null)
            {
                throw new InvalidOperationException("JwtSettings section not found in configuration or failed to deserialize. Ensure 'JwtSettings' section exists in appsettings.json with SecretKey, Issuer, Audience, and ExpiryInHours.");
            }

            if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) || jwtSettings.SecretKey.Length < 32)
            {
                throw new InvalidOperationException("JwtSettings.SecretKey must be at least 32 characters long.");
            }

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsJsonAsync(new { message = "Token has expired" });
                        }

                        System.Diagnostics.Debug.WriteLine($"Authentication failed: {context.Exception?.Message}");
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAssignmentRequestService, AssignmentRequestService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAssetService, AssetService>();
        
            return services;
        }

      
    }
}

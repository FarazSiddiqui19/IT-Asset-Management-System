using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using IT_Asset_Management_System;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.Category;
using IT_Asset_Management_System.Common.Settings;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using IT_Asset_Management_System.Controllers;
using IT_Asset_Management_System.Services.Interfaces;

namespace IT_Asset_Management_System.Tests.Controller
{
    public class CategoryControllerTests
    {
        private const string TestSecret = "test_secret_key_at_least_32_chars_long__";
        private const string TestIssuer = "TestIssuer";
        private const string TestAudience = "TestAudience";

        private WebApplicationFactory<Program> CreateFactory(SqliteConnection connection)
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((ctx, config) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        { "JwtSettings:SecretKey", TestSecret },
                        { "JwtSettings:Issuer", TestIssuer },
                        { "JwtSettings:Audience", TestAudience },
                        { "JwtSettings:ExpiryInHours", "24" }
                    };

                    config.AddInMemoryCollection(dict);
                });

                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContextOptions registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<dbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // Also remove any provider-specific service registrations (e.g., Npgsql) to avoid multiple providers registered
                    var toRemove = services.Where(d =>
                        (d.ServiceType?.Assembly?.GetName().Name?.Contains("Npgsql") ?? false) ||
                        (d.ImplementationType?.Assembly?.GetName().Name?.Contains("Npgsql") ?? false) ||
                        (d.ServiceType?.FullName?.Contains("Npgsql") ?? false) ||
                        (d.ImplementationType?.FullName?.Contains("Npgsql") ?? false)
                    ).ToList();

                    foreach (var d in toRemove)
                        services.Remove(d);

                    // Register SQLite using the provided open connection
                    services.AddDbContext<dbContext>(options => options.UseSqlite(connection));
                });
            });

            return factory;
        }

        private string GenerateJwt(string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: TestIssuer,
                audience: TestAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task SeedData(dbContext context)
        {
            // Ensure clean DB and add a sample admin user and some categories
            context.Database.EnsureCreated();

            context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@test.local",
                Username = "admin",
                PasswordHash = "x", // not used by tests
                Role = Entities.Enums.UserRole.Admin
            });

            context.Categories.AddRange(new[] {
                new Category { Name = "Laptops" },
                new Category { Name = "Monitors" }
            });

            await context.SaveChangesAsync();
        }

        [Fact]
        // Rule 1.3 — Admin can get all categories and response body contains seeded categories
        public async Task GetAll_ShouldReturn200AndBody_WhenAdminRequestsCategories()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using var factory = CreateFactory(connection);
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<dbContext>();
            await SeedData(context);

            var client = factory.CreateClient();
            var token = GenerateJwt("Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/Category");
            response.EnsureSuccessStatusCode();

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            Assert.NotNull(categories);
            Assert.Contains(categories, c => c.Name == "Laptops");
            Assert.Contains(categories, c => c.Name == "Monitors");
        }

        [Fact]
        // Rule 1.3 — Employee can get all categories (dropdown access)
        public async Task GetAll_ShouldReturn200AndBody_WhenEmployeeRequestsCategories()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using var factory = CreateFactory(connection);
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<dbContext>();
            await SeedData(context);

            var client = factory.CreateClient();
            var token = GenerateJwt("Employee");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/Category");
            response.EnsureSuccessStatusCode();

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            Assert.NotNull(categories);
            Assert.Contains(categories, c => c.Name == "Laptops");
        }

        [Fact]
        // Rule 1.3 — Unauthenticated request is rejected
        public async Task GetAll_ShouldReturn401_WhenUnauthenticated()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using var factory = CreateFactory(connection);
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<dbContext>();
            await SeedData(context);

            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/Category");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        // Rule 1.3 — Admin can get category by ID and body is correct
        public async Task GetById_ShouldReturn200AndBody_WhenAdminRequestsExistingCategory()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using var factory = CreateFactory(connection);
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<dbContext>();
            await SeedData(context);

            var cat = await context.Categories.FirstAsync(c => c.Name == "Laptops");

            var client = factory.CreateClient();
            var token = GenerateJwt("Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/Category/{cat.Id}");
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<CategoryDto>();
            Assert.NotNull(body);
            Assert.Equal(cat.Id, body.Id);
            Assert.Equal("Laptops", body.Name);
        }

        [Fact]
        // Rule 1.3 — Employee cannot get category by ID
        public async Task GetById_ShouldReturn403_WhenEmployeeRequestsCategory()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using var factory = CreateFactory(connection);
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<dbContext>();
            await SeedData(context);

            var cat = await context.Categories.FirstAsync(c => c.Name == "Laptops");

            var client = factory.CreateClient();
            var token = GenerateJwt("Employee");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/Category/{cat.Id}");
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        // Generic — Returns 404 for non-existent category
        public async Task GetById_ShouldReturn404_WhenCategoryDoesNotExist()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using var factory = CreateFactory(connection);
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<dbContext>();
            await SeedData(context);

            var client = factory.CreateClient();
            var token = GenerateJwt("Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/Category/{Guid.NewGuid()}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        // Rule 1.3 — Admin can create category and response body contains created category
        public async Task Create_ShouldReturn201AndBody_WhenAdminCreatesValidCategory()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using var factory = CreateFactory(connection);
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<dbContext>();
            await SeedData(context);

            var client = factory.CreateClient();
            var token = GenerateJwt("Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new CreateCategoryDto { Name = "NewCategory" };
            var response = await client.PostAsJsonAsync("/api/Category", dto);

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<CategoryDto>();
            Assert.NotNull(created);
            Assert.Equal("NewCategory", created.Name);
        }

        [Fact]
        // Rule 1.3 — Employee cannot create category
        public async Task Create_ShouldReturn403_WhenEmployeeTriesToCreateCategory()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using var factory = CreateFactory(connection);
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<dbContext>();
            await SeedData(context);

            var client = factory.CreateClient();
            var token = GenerateJwt("Employee");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new CreateCategoryDto { Name = "NewCategory" };
            var response = await client.PostAsJsonAsync("/api/Category", dto);

            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        // Unit tests for controller response bodies using mocked ICategoryService

        [Fact]
        // Validate GetAll returns 200 and body contains categories
        public async Task GetAll_ReturnsOk_WithCategoryList()
        {
            var service = Substitute.For<ICategoryService>();
            var list = new List<CategoryDto>
            {
                new CategoryDto { Id = Guid.NewGuid(), Name = "A" },
                new CategoryDto { Id = Guid.NewGuid(), Name = "B" }
            };
            service.GetAllAsync().Returns(list);

            var controller = new CategoryController(service);

            var result = await controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<List<CategoryDto>>(ok.Value);
            Assert.Equal(2, value.Count);
        }

        [Fact]
        // Validate GetById returns 200 and body contains the expected category
        public async Task GetById_ReturnsOk_WithCategory()
        {
            var service = Substitute.For<ICategoryService>();
            var id = Guid.NewGuid();
            var dto = new CategoryDto { Id = id, Name = "Laptops" };
            service.GetByIdAsync(id).Returns(dto);

            var controller = new CategoryController(service);

            var result = await controller.GetById(id);
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<CategoryDto>(ok.Value);
            Assert.Equal(id, value.Id);
            Assert.Equal("Laptops", value.Name);
        }

        [Fact]
        // Validate Create returns 201 and body contains created category
        public async Task Create_ReturnsCreated_WithCategory()
        {
            var service = Substitute.For<ICategoryService>();
            var id = Guid.NewGuid();
            var created = new CategoryDto { Id = id, Name = "NewCat" };
            service.AddAsync(Arg.Any<CreateCategoryDto>()).Returns(created);

            var controller = new CategoryController(service);

            var result = await controller.Create(new CreateCategoryDto { Name = "NewCat" });
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            var value = Assert.IsType<CategoryDto>(createdAt.Value);
            Assert.Equal(id, value.Id);
            Assert.Equal("NewCat", value.Name);
            Assert.Equal(nameof(controller.GetById), createdAt.ActionName);
        }

        [Fact]
        // Validate Update returns 200 OK
        public async Task Update_ReturnsOk()
        {
            var service = Substitute.For<ICategoryService>();
            var controller = new CategoryController(service);

            var result = await controller.Update(Guid.NewGuid(), new UpdateCategoryDto { Name = "X" });
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        // Validate Delete returns 204 NoContent
        public async Task Delete_ReturnsNoContent()
        {
            var service = Substitute.For<ICategoryService>();
            var controller = new CategoryController(service);

            var result = await controller.Delete(Guid.NewGuid());
            Assert.IsType<NoContentResult>(result);
        }
    }
}

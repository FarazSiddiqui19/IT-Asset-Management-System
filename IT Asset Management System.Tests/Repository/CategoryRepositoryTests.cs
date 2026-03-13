using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Repository;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.Category;
using System.Collections.Generic;

namespace IT_Asset_Management_System.Tests.Repository
{
    public class CategoryRepositoryTests
    {
        private dbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<dbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new dbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        // Rule 2 — Add category and retrieve by ID
        public async Task AddAsync_ShouldReturnCategory_WhenCategoryIsValid()
        {
            using var context = CreateContext();
            var repo = new CategoryRepository(context);

            var category = new Category { Name = "Laptops" };
            var added = await repo.AddAsync(category);

            Assert.NotEqual(Guid.Empty, added.Id);

            var fetched = await repo.GetByIdAsync(added.Id);
            Assert.NotNull(fetched);
            Assert.Equal("Laptops", fetched.Name);
        }

        [Fact]
        // Rule 2 — Name uniqueness enforced by database
        public async Task AddAsync_ShouldThrow_WhenCategoryNameIsDuplicate()
        {
            using var context = CreateContext();
            var repo = new CategoryRepository(context);

            await repo.AddAsync(new Category { Name = "Laptops" });

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await repo.AddAsync(new Category { Name = "Laptops" });
            });
        }

        [Fact]
        // Rule 2 — Get all returns alphabetically ordered list
        public async Task GetAllAsync_ShouldReturnAllCategories_OrderedByName()
        {
            using var context = CreateContext();
            var repo = new CategoryRepository(context);

            await repo.AddAsync(new Category { Name = "Zebra" });
            await repo.AddAsync(new Category { Name = "Apple" });
            await repo.AddAsync(new Category { Name = "Monkey" });

            var all = await repo.GetAllAsync();
            var names = all.Select(c => c.Name).ToList();

            Assert.Equal(new List<string> { "Apple", "Monkey", "Zebra" }, names);
        }

        [Fact]
        // Rule 2 — Get by name returns correct category
        public async Task GetByNameAsync_ShouldReturnCategory_WhenNameExists()
        {
            using var context = CreateContext();
            var repo = new CategoryRepository(context);

            var added = await repo.AddAsync(new Category { Name = "Laptops" });

            var found = await repo.GetByNameAsync("Laptops");
            Assert.NotNull(found);
            Assert.Equal(added.Id, found.Id);
        }

        [Fact]
        // Rule 2 — Get by name returns null when not found
        public async Task GetByNameAsync_ShouldReturnNull_WhenNameDoesNotExist()
        {
            using var context = CreateContext();
            var repo = new CategoryRepository(context);

            var found = await repo.GetByNameAsync("NonExistent");
            Assert.Null(found);
        }

        [Fact]
        // Generic — Update category name
        public async Task UpdateAsync_ShouldReturnTrue_WhenCategoryExists()
        {
            using var context = CreateContext();
            var repo = new CategoryRepository(context);

            var added = await repo.AddAsync(new Category { Name = "Old" });
            added.Name = "New";

            var ok = await repo.UpdateAsync(added);
            Assert.True(ok);

            var fetched = await repo.GetByIdAsync(added.Id);
            Assert.Equal("New", fetched.Name);
        }

        [Fact]
        // Generic — Delete category
        public async Task DeleteAsync_ShouldReturnTrue_AndRemoveCategory()
        {
            using var context = CreateContext();
            var repo = new CategoryRepository(context);

            var added = await repo.AddAsync(new Category { Name = "ToDelete" });

            var ok = await repo.DeleteAsync(added);
            Assert.True(ok);

            var fetched = await repo.GetByIdAsync(added.Id);
            Assert.Null(fetched);
        }

        [Fact]
        // Generic — Get by ID returns null for non-existent
        public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            using var context = CreateContext();
            var repo = new CategoryRepository(context);

            var fetched = await repo.GetByIdAsync(Guid.NewGuid());
            Assert.Null(fetched);
        }
    }
}

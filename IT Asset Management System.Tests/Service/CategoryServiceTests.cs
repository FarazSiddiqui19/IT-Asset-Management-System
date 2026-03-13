using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Services;
using IT_Asset_Management_System.DTOs.Category;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common.Exceptions;

namespace IT_Asset_Management_System.Tests.Service
{
    public class CategoryServiceTests
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepository = Substitute.For<ICategoryRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _categoryService = new CategoryService(_categoryRepository, _productRepository);
        }

        [Fact]
        // Rule 2 — Successfully add a new category
        public async Task AddAsync_ShouldReturnCategoryDto_WhenNameIsUnique()
        {
            var dto = new CreateCategoryDto { Name = "Laptops" };
            _categoryRepository.GetByNameAsync(dto.Name).Returns((Category)null);

            var createdEntity = new Category { Id = Guid.NewGuid(), Name = dto.Name };
            _categoryRepository.AddAsync(Arg.Any<Category>()).Returns(createdEntity);

            var result = await _categoryService.AddAsync(dto);
            Assert.Equal(dto.Name, result.Name);
        }

        [Fact]
        // Rule 2 — Throw ConflictException when name already exists
        public async Task AddAsync_ShouldThrowConflictException_WhenNameIsDuplicate()
        {
            var dto = new CreateCategoryDto { Name = "Laptops" };
            _categoryRepository.GetByNameAsync(dto.Name)
                .Returns(new Category { Id = Guid.NewGuid(), Name = dto.Name });

            await Assert.ThrowsAsync<ConflictException>(() => _categoryService.AddAsync(dto));
        }

        [Fact]
        // Generic — Successfully retrieve category by ID
        public async Task GetByIdAsync_ShouldReturnCategoryDto_WhenCategoryExists()
        {
            var id = Guid.NewGuid();
            _categoryRepository.GetByIdAsync(id).Returns(new Category { Id = id, Name = "Laptops" });

            var result = await _categoryService.GetByIdAsync(id);
            Assert.Equal("Laptops", result.Name);
        }

        [Fact]
        // Generic — Throw NotFoundException when category not found
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var id = Guid.NewGuid();
            _categoryRepository.GetByIdAsync(id).Returns((Category)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.GetByIdAsync(id));
        }

        [Fact]
        // Generic — Successfully retrieve all categories
        public async Task GetAllAsync_ShouldReturnListOfCategoryDtos()
        {
            var list = new List<CategoryDto>
            {
                new CategoryDto { Id = Guid.NewGuid(), Name = "A" },
                new CategoryDto { Id = Guid.NewGuid(), Name = "B" }
            };
            _categoryRepository.GetAllAsync().Returns(list);

            var result = await _categoryService.GetAllAsync();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        // Rule 2 — Successfully update category name
        public async Task UpdateAsync_ShouldSucceed_WhenCategoryExistsAndNameIsUnique()
        {
            var id = Guid.NewGuid();
            var existing = new Category { Id = id, Name = "Old" };
            _categoryRepository.GetByIdAsync(id).Returns(existing);
            _categoryRepository.GetByNameAsync("New").Returns((Category)null);
            _categoryRepository.UpdateAsync(existing).Returns(true);

            await _categoryService.UpdateAsync(id, new UpdateCategoryDto { Name = "New" });

            await _categoryRepository.Received().UpdateAsync(existing);
        }

        [Fact]
        // Rule 2 — Throw NotFoundException when updating non-existent category
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var id = Guid.NewGuid();
            _categoryRepository.GetByIdAsync(id).Returns((Category)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.UpdateAsync(id, new UpdateCategoryDto { Name = "New" }));
        }

        [Fact]
        // Rule 2 — Throw ConflictException when updating to existing name
        public async Task UpdateAsync_ShouldThrowConflictException_WhenNameIsTakenByAnotherCategory()
        {
            var id = Guid.NewGuid();
            var existing = new Category { Id = id, Name = "Old" };
            _categoryRepository.GetByIdAsync(id).Returns(existing);
            _categoryRepository.GetByNameAsync("Taken").Returns(new Category { Id = Guid.NewGuid(), Name = "Taken" });

            await Assert.ThrowsAsync<ConflictException>(() => _categoryService.UpdateAsync(id, new UpdateCategoryDto { Name = "Taken" }));
        }

        [Fact]
        // Rule 2 — Throw ValidationException when products exist under category
        public async Task DeleteAsync_ShouldThrowValidationException_WhenProductsExistUnderCategory()
        {
            var categoryId = Guid.NewGuid();
            _categoryRepository.GetByIdAsync(categoryId)
                .Returns(new Category { Id = categoryId, Name = "Laptops" });
            _productRepository.HasProductsAsync(categoryId).Returns(true);

            await Assert.ThrowsAsync<ValidationException>(() => _categoryService.DeleteAsync(categoryId));
        }

        [Fact]
        // Rule 2 — Successfully delete category with no products
        public async Task DeleteAsync_ShouldSucceed_WhenNoproductsExistUnderCategory()
        {
            var categoryId = Guid.NewGuid();
            var cat = new Category { Id = categoryId, Name = "Laptops" };
            _categoryRepository.GetByIdAsync(categoryId).Returns(cat);
            _productRepository.HasProductsAsync(categoryId).Returns(false);
            _categoryRepository.DeleteAsync(cat).Returns(true);

            await _categoryService.DeleteAsync(categoryId);

            await _categoryRepository.Received().DeleteAsync(cat);
        }

        [Fact]
        // Generic — Throw NotFoundException when deleting non-existent category
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var categoryId = Guid.NewGuid();
            _categoryRepository.GetByIdAsync(categoryId).Returns((Category)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.DeleteAsync(categoryId));
        }
    }
}

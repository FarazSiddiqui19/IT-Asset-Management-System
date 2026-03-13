using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.Category;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace IT_Asset_Management_System.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<CategoryDto> AddAsync(CreateCategoryDto dto)
        {
            var existing = await _categoryRepository.GetByNameAsync(dto.Name);
            if (existing != null)
                throw new ConflictException("Category with this name already exists.");

            var category = new Category
            {
                Name = dto.Name
            };

            await _categoryRepository.AddAsync(category);
            return category.ToDto();
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category not found.");

            return category.ToDto();
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
           
            return await _categoryRepository.GetAllAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category not found.");

            var byName = await _categoryRepository.GetByNameAsync(dto.Name);
            if (byName != null && byName.Id != id)
                throw new ConflictException("Category with this name already exists.");

            category.Name = dto.Name;

            var ok = await _categoryRepository.UpdateAsync(category);
            if (!ok)
                throw new ValidationException("Failed to update category.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category not found.");
            var hasProducts = await _productRepository.HasProductsAsync(id);
            if (hasProducts)
                throw new ValidationException("Category cannot be deleted because products exist under it.");
            if (await _categoryRepository.HasActiveAssignmentsAsync(id))
                throw new ValidationException("Category cannot be deleted because active assignments exist under it.");

        

            var ok = await _categoryRepository.DeleteAsync(category);
            if (!ok)
                throw new ValidationException("Failed to delete category.");
        }
    }
}

using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.Product;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Common.Mappers;

namespace IT_Asset_Management_System.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IAssetRepository assetRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> AddAsync(CreateProductDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new NotFoundException("Category not found.");

            var product = dto.ToEntity();

            await _productRepository.AddAsync(product);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");

           var Added = await _productRepository.GetByIdWithDetailsAsync(product.Id) ??
                throw new InternalServerException("Failed to retrieve the created product. Please try again.");
            return Added;
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var dto = await _productRepository.GetByIdWithDetailsAsync(id);
            if (dto == null)
                throw new NotFoundException("Product not found.");
            return dto;
        }

        public async Task<PagedResult<ProductDto>> GetAllAsync(ProductFilter filter)
        {
            return await _productRepository.GetAllAsync(filter);
        }

        public async Task UpdateAsync(Guid id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException("Product not found.");
            
            if(dto.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value);
                if (category == null)
                    throw new NotFoundException("Category not found.");
            }
            

            product.Name = dto.Name?? product.Name;
            product.Description = dto.Description?? product.Description;
            product.CategoryId = dto.CategoryId ?? product.CategoryId;

            await _productRepository.UpdateAsync(product);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException("Product not found.");

            var hasAssets = await _assetRepository.HasAssetsAsync(id);
            if (hasAssets)
                throw new ValidationException("Product cannot be deleted because assets exist under it.");

            await _productRepository.DeleteAsync(product);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }
    }
}

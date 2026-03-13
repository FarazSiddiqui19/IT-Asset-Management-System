using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Product;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> AddAsync(CreateProductDto dto);
        Task<ProductDto> GetByIdAsync(Guid id);
        Task<PagedResult<ProductDto>> GetAllAsync(ProductFilter filter);
        Task UpdateAsync(Guid id, UpdateProductDto dto);
        Task DeleteAsync(Guid id);
    }
}

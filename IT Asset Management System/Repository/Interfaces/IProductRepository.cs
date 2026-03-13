using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Product;
using System.Threading.Tasks;

namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedResult<ProductDto>> GetAllAsync(ProductFilter filter);
        Task<ProductDto?> GetByIdWithDetailsAsync(Guid id);
        Task<bool> HasProductsAsync(Guid categoryId);
    }
}

using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.Category;

namespace IT_Asset_Management_System.Common.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}

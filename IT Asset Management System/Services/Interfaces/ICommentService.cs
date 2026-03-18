using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Comment;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> AddAsync(CreateCommentDto dto);
        Task<PagedResult<CommentDto>> GetAllAsync(CommentFilter filter);
        Task<CommentDto> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, UpdateCommentDto dto);
        Task DeleteAsync(Guid id, Guid requestingUser);
    }
}

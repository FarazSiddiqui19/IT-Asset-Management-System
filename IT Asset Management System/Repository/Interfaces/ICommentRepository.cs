using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Comment;

namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<CommentDto?> GetByIdWithDetailsAsync(Guid id);
        Task<PagedResult<CommentDto>> GetAllAsync(CommentFilter filter);
    }
}

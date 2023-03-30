﻿using Conduit.Domain.Entities;

namespace Conduit.Domain.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment> GetCommentById(int Id);
    }
}

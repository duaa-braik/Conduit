using Conduit.Domain.Entities;

namespace Conduit.Domain.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> CreateTag(Tag tag);
        Task<Tag> GetTag(string tagName);
    }
}

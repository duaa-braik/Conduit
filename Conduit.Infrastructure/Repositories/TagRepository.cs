using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ConduitDbContext context;

        public TagRepository(ConduitDbContext context)
        {
            this.context = context;
        }
        public async Task<Tag> GetTag(string tagName)
        {
            return await context.Tag.FirstAsync(tag => tag.TagName == tagName);
        }

        public async Task<Tag> CreateTag(Tag tag)
        {
            context.Tag.Add(tag);
            await context.SaveChangesAsync();
            return tag;
        }
    }
}

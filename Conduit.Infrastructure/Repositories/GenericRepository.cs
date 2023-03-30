using Conduit.Domain.Interfaces;

namespace Conduit.Infrastructure.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        public readonly ConduitDbContext context;

        public GenericRepository(ConduitDbContext context)
        {
            this.context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async virtual Task<T> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}

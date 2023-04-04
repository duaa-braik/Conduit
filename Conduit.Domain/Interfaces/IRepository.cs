namespace Conduit.Domain.Interfaces
{
    public interface IRepository <T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}

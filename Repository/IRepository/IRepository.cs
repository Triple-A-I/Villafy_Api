using System.Linq.Expressions;

namespace Villafy_Api.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = "");
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = "");
        Task CreateAsync(T entity);
        Task Remove(T entity);
        Task SaveAsync();
    }
}

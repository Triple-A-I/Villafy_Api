using System.Linq.Expressions;
using Villafy_Api.Models;

namespace Villafy_Api.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null, bool tracked = true);
        Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true);
        Task CreateAsync(Villa entity);
        Task UpdateAsync(Villa entity);
        Task Remove(Villa entity);
        Task SaveAsync();
    }
}
